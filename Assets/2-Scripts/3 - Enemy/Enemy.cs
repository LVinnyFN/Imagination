using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    //UIatributes
    [SerializeField] Slider hpSlider;
    [SerializeField] Image fillColor;
    [SerializeField] TextMeshProUGUI levelUI;
    [SerializeField] private Canvas mycanvas;

    //Stats
    public string enemyName;
    private int hp;
    private int level;
    private int xp;
    private int totalHp;
    private float dmg;

    //Base Stats
    [SerializeField] private int baseHp = 10;
    [SerializeField] private int hpPerLevel = 10;
    [SerializeField] private int baseDmg = 10;
    [SerializeField] private int dmgPerLevel = 10;
    [SerializeField] public int minLvl = 10, maxLvl = 10, range = 10;
    [SerializeField] private int xpBase = 10;
    [SerializeField] private int xpPerLevel = 10;


    [SerializeField] private int id;

    //Ranges
    [SerializeField] private float detectRadius = 20f;
    [SerializeField] private float chaseRadius = 10f;
    [SerializeField] private float attackRadius = 3f;

    //Player position
    Transform playerPos;

    //Components
    NavMeshAgent agent;
    Rigidbody rb;
    Animator anim;
    EnemySoundManager enemySoundManager;

    //useful variables
    [SerializeField] private GameObject lifeLossPrefab;
    public Player player;
    [SerializeField] private float attackRate = 1.0f; //Attacks per second
    [SerializeField] private float rotSpeed = 0.1f;
    private float nextAttack = 0;
    private bool detected = false;
    public bool isDead = false;
    public bool isAttacking = false;
    public bool canDamage = true;

    //Skills
    public bool poisoned = false;
    private float poisonTimer;
    private bool poisonInvoked = false;

    public GameObject dropPrefab;

    //Respawn
    private Vector3 initialPos;
    private Quaternion initialRot;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        enemySoundManager = GetComponent<EnemySoundManager>();

        player = GameController.controller.player.GetComponent<Player>();

        GenerateStats();

        rb.useGravity = false;

        playerPos = GameController.controller.player.transform;

        //Respawn Variables
        initialPos = transform.position;
        initialRot = transform.rotation;

        fillColor = hpSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        Patrol();
        Poison();
    }

    //Generate the stats
    private void GenerateStats()
    {

        //                      WHAT??????

        level = Mathf.Clamp(UnityEngine.Random.Range(player.level - (range / 2), player.level + range), minLvl, maxLvl);
        totalHp = (int)((baseHp + (hpPerLevel * level)) * GameController.controller.difficulty);
        hp = totalHp;
        dmg = (int)((baseDmg + (dmgPerLevel * level)) * GameController.controller.difficulty);
        xp = xpBase + (xpPerLevel * level);
        levelUI.text = enemyName + " (" + level + ")";
    }

    private void Patrol()
    {
        float distance = Vector3.Distance(playerPos.position, transform.position);

        if (distance <= detectRadius && !isDead && !player.isDead)
        {
            agent.isStopped = false;
            if (!detected)
            {
                anim.SetTrigger("detectPlayer"); //Play the detection animation
                detected = true;
            }

            Vector3 direction = playerPos.position - this.transform.position; //Get the player direction
            direction.y = 0; //Prevents the enemy from inclining
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime); //Rotate towards the player

            if (distance <= chaseRadius)
            {
                anim.SetBool("isChasing", true);
                agent.SetDestination(playerPos.position);

                if (distance <= attackRadius && !player.isDead)
                {
                    agent.isStopped = true;

                    if (Time.time > nextAttack) //"Fire Rate"
                    {
                        nextAttack = Time.time + attackRate;
                        anim.SetTrigger("attack");
                        canDamage = true;
                    }
                    else anim.SetBool("isWaitingAttack", true);

                    if (anim.GetCurrentAnimatorStateInfo(0).IsTag("1")) isAttacking = true;
                    else isAttacking = false;
                }
                else if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("1"))
                {
                    agent.isStopped = false;
                    anim.SetBool("isWaitingAttack", false);
                }
            }
        }
        else
        {
            anim.SetBool("isChasing", false);
            detected = false;
            agent.isStopped = true; //Stop chasing
            anim.SetBool("isChasing", false);
        }
    }

    public void DamagePlayer()
    {
        float distance = Vector3.Distance(playerPos.position, transform.position);

        if (distance <= attackRadius)
        {
            int dmg = DealDamage();
            player.TakeDamage(dmg); //Player will only be damaged if he is inside the attack area

            if (player.passivesManager.getThornsUnlocked()) TakeDamage(dmg * player.passivesManager.getThornsPercentage() / 100, 0);
        }
    }

    public int DealDamage()
    {
        float damage = dmg;
        return (int)damage;
    }

    //Receive damage
    public void TakeDamage(int damage, int iscritical)
    {
        GameObject dmgtext = Instantiate(lifeLossPrefab, mycanvas.transform.position, transform.rotation);
        dmgtext.GetComponent<EnemyLifeloss>().SetDmg(damage, iscritical);
        hp -= damage;
        enemySoundManager.TakeHit();

        if (hp <= 0)
        {
            hpSlider.value = 0;
            enemySoundManager.Die();
            StartCoroutine(Die());
        }
        hpSlider.value = (float)hp / totalHp;
    }

    public void TakePoisonDamage(int damage) //Poison Passive
    {
        GameObject dmgtext = Instantiate(lifeLossPrefab, mycanvas.transform.position, transform.rotation);
        dmgtext.GetComponent<EnemyLifeloss>().SetPoison(damage);
        hp -= damage;
        enemySoundManager.TakeHit();

        if (hp <= 0)
        {
            hpSlider.value = 0;
            enemySoundManager.Die();
            StartCoroutine(Die());
        }
        hpSlider.value = (float)hp / totalHp;
    }

    public IEnumerator Die()
    {
        if (Random.Range(0, 100) <= 80)
        {
            GameObject drop = Instantiate(dropPrefab, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
            drop.GetComponent<Drop>().deadenemy = enemyName;
        }
        GameController.controller.globalStats.EnemyKilled(id);
        mycanvas.gameObject.SetActive(false);
        isDead = true;
        poisoned = false;
        anim.SetBool("dead", true);
        player.GainXP(xp);

        if (id == 2) agent.baseOffset = 0;

        if (id == 3 || id == 1)
        {
            this.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
        }

        yield return new WaitForSeconds(3f);
        if (this.name!="TutorialBee")
        {
            GameController.controller.respawnController.Respawn(id, initialPos, initialRot, minLvl, maxLvl);
        }
        Destroy(gameObject);
    }

    //Skills
    private void Poison()
    {
        if (poisoned)
        {
            fillColor.color = Color.green;
            if (!poisonInvoked)
            {

                InvokeRepeating("Poisoned", 1, 1);
                poisonInvoked = true;

            }

            if (poisonTimer > 0)
            {
                poisonTimer -= Time.deltaTime;
            }
            else
            {
                poisoned = false;
            }
        }
        else
        {
            poisonInvoked = false;
            fillColor.color = Color.red;
            CancelInvoke();
        }
    }

    private void Poisoned()
    {     
        print("POISON DAMAGE");
        int damage = Mathf.Clamp((int)(totalHp * player.passivesManager.getPoisonPercentage() / 100f), 1, player.passivesManager.getPoisonLimit());

        TakePoisonDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Attacked
        if (other.gameObject.CompareTag("Weapon") && !isDead)
        {
            if (player.isAttacking && !player.dealtDamage) //If the player is attacking and haven't already caused damage in the current attack (to prevend taking damage when the player passes nearby)
            {
                //Generate damage
                int[] dmg = new int[2];
                dmg = player.DealDamage();

                //Take full damage if i'm not attacking
                if (!isAttacking)
                {
                    TakeDamage(dmg[0], dmg[1]);

                    //LifeSteal
                    if (player.passivesManager.getLifeStealUnlocked())
                    {
                        player.HealValue(dmg[0] * player.passivesManager.getLifeStealPercentage() / 100);
                    }
                }
                //Take reduced damage if i'm attacking
                else
                {
                    TakeDamage(dmg[0] / 2, dmg[1]);

                    //LifeSteal
                    if (player.passivesManager.getLifeStealUnlocked())
                    {
                        player.HealValue((dmg[0] / 2) * (player.passivesManager.getLifeStealPercentage() / 100));
                    }
                }

                //Poison
                if (player.passivesManager.getPoisonUnlocked())
                {
                    poisoned = true;
                    poisonTimer = player.passivesManager.getPoisonTime();
                }

                player.dealtDamage = true;
            }
        }
    }

    //Draw Gizmos
    private void OnDrawGizmosSelected()
    {
        //Detection Radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        //Chase Radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        //Attack Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}