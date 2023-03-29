using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Animations;
//using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{

    //Components
    private Player player;
    private Animator animator;

    //Levels
    public int lungeLevel = 0, spinLevel = 0, criticalLevel = 0, invulnerabilityLevel = 0, tantrumLevel = 0;

    //Damages
    public float spinBaseDamage = 30, lungeBaseDamage = 60;
    public float spinDamagePerLevel = 10, lungeDamagePerLevel = 10;
    public float spinDamage = 0, lungeDamage = 0;

    //Range
    private float spinRange = 3f;
    private float lungeRange = 2.5f;

    //Cooldowns (in seconds)
    public float spinBaseCooldown = 5, lungeBaseCooldown = 5, criticalBaseCooldown = 5, invulnerabilityBaseCooldown = 5, tantrumBaseCooldown = 5;
    private float spinCooldown, lungeCooldown, criticalCooldown, invulnerabilityCooldown, tantrumCooldown;
    private float spinTimer = 7, lungeTimer = 5, criticalTimer = 10, invulnerabilityTimer = 20, tantrumTimer = 30;

    //Permitions
    private bool spinUnlocked = false, lungeUnlocked = false, criticalUnlocked = false, invulnerabilityUnlocked = false, tantrumUnlocked = false;
    private bool spinAvailable = false, lungeAvailable = false, criticalAvailable = false, invulnerabilityAvailable = false, tantrumAvailable = false; //Available for use

    //Durations (in seconds)
    private float criticalDuration = 0, invulnerabilityDuration = 0, tantrumDuration = 0;
    public float criticalBaseDuration = 4, invulnerabilityBaseDuration = 4, tantrumBaseDuration = 4;
    public float criticalDurationPerLevel = 1, invulnerabilityDurationPerLevel = 1, tantrumDurationPerLevel = 1;

    private bool criticalActive = false, invulnerabilityActive = false, tantrumActive = false; //Is active
    private float criticalTime = 0, invulnerabilityTime = 0, tantrumTime = 0; //Time passed active

    [SerializeField] private Image cd1;
    [SerializeField] private Image cd2;
    [SerializeField] private Image cd3;
    [SerializeField] private Image cd4;
    [SerializeField] private Image cd5;

    [SerializeField] private GameObject buff3prefab;
    [SerializeField] private GameObject buff4prefab;
    [SerializeField] private GameObject buff5prefab;
    [SerializeField] private Transform buffarea;

    private GameObject mybuff3;
    private GameObject mybuff4;
    private GameObject mybuff5;


    //Effects
    [SerializeField] private ParticleSystem estocadaParticle;
    [SerializeField] private ParticleSystem spinParticle;
    [SerializeField] private ParticleSystem critParticle;
    [SerializeField] private ParticleSystem shieldParticle;
    [SerializeField] private ParticleSystem tantrumParticle;

    //UI
    [SerializeField] private GameObject lungeLock1, lungLock2;
    [SerializeField] private GameObject spinLock1, spinLock2;
    [SerializeField] private GameObject critLock1, critLock2;
    [SerializeField] private GameObject invulnerabilityLock1, invulnerabilityLock2;
    [SerializeField] private GameObject tantrumLock1, tantrumLock2;
    [SerializeField] private GameObject[] skillFeedback = new GameObject[5];

    //Getters
    public bool LungeUnlocked
    {
        get { return lungeUnlocked; }
    }

    public bool SpinUnlocked
    {
        get { return spinUnlocked; }
    }

    public bool CriticalUnlocked
    {
        get { return CriticalUnlocked; }
    }

    public bool InvulnerabilityUnlocked
    {
        get { return invulnerabilityUnlocked; }
    }

    public bool TantrumUnlocked
    {
        get { return tantrumUnlocked; }
    }


    public float CriticalDuration
    {
        get { return criticalDuration; }
    }

    public float InvulnerabilityDuration
    {
        get { return invulnerabilityDuration; }
    }

    public float TantrumDuration
    {
        get { return tantrumDuration; }
    }

    void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        UpdateCooldowns();

    }

    // Update is called once per frame
    void Update()
    {
        CooldownManager();
        InputManager();

    }

    public void UpdateCooldowns() //Chamar sempre que o cooldown do Player for alterado
    {
        lungeCooldown = lungeBaseCooldown * (1-player.cooldown);
        spinCooldown = spinBaseCooldown * (1-player.cooldown);
        criticalCooldown = criticalBaseCooldown * (1 - player.cooldown);
        tantrumCooldown = tantrumBaseCooldown * (1-player.cooldown);
        invulnerabilityCooldown = invulnerabilityBaseCooldown *(1- player.cooldown);
    }


    //=========================================== UNLOCK SKILLS ==================================== //

    public void UnlockLunge()
    {
        lungeUnlocked = true;
        LungeLevelUp();
        lungeLock1.SetActive(false);
        lungLock2.SetActive(false);
        print("Lunge Unlocked!");
    }

    public void UnlockSpin()
    {
        spinUnlocked = true;
        SpinLevelUp();
        spinLock1.SetActive(false);
        spinLock2.SetActive(false);
        print("Spin Unlocked!");
    }

    public void UnlockCritical()
    {
        criticalUnlocked = true;
        CriticalLevelUp();
        critLock1.SetActive(false);
        critLock2.SetActive(false);
        print("Critical Unlocked!");
    }

    public void UnlockInvulnerability()
    {
        invulnerabilityUnlocked = true;
        InvulnerabilityLevelUp();
        invulnerabilityLock1.SetActive(false);
        invulnerabilityLock2.SetActive(false);
        print("Invulnerability Unlocked");
    }

    public void UnlockTantrum()
    {
        tantrumUnlocked = true;
        TantrumLevelUp();
        tantrumLock1.SetActive(false);
        tantrumLock2.SetActive(false);
        print("Tantrum Unlocked");
    }

    //=========================================== LEVEL-UP SKILLS ==================================== //


    public void LungeLevelUp()
    {
        lungeLevel++;
        if (lungeLevel == 1)
        {
            StartCoroutine(ShowSkillFeedback("Unlocked!", 0));
        }
        else
        {
            StartCoroutine(ShowSkillFeedback("Level Up!", 0));
        }
        print(lungeLevel);
        lungeDamage = lungeBaseDamage + lungeDamagePerLevel * lungeLevel;
        print("Lunge LevelUp!");
    }

    public void SpinLevelUp()
    {
        spinLevel++;
        if (spinLevel == 1)
        {
            StartCoroutine(ShowSkillFeedback("Unlocked!", 1));
        }
        else
        {
            StartCoroutine(ShowSkillFeedback("Level Up!", 1));
        }
        spinDamage = spinBaseDamage + spinDamagePerLevel * spinLevel;
        print("Spin LevelUp!");
    }

    public void CriticalLevelUp()
    {
        criticalLevel++;
        if (criticalLevel == 1)
        {
            StartCoroutine(ShowSkillFeedback("Unlocked!", 2));
        }
        else
        {
            StartCoroutine(ShowSkillFeedback("Level Up!", 2));
        }
        criticalDuration = criticalBaseDuration + criticalDurationPerLevel * criticalLevel;
        critParticle.startLifetime = criticalDuration;
        print("Critical LevelUp!");
    }

    public void InvulnerabilityLevelUp()
    {
        invulnerabilityLevel++;
        if (invulnerabilityLevel == 1)
        {
            StartCoroutine(ShowSkillFeedback("Unlocked!", 3));
        }
        else
        {
            StartCoroutine(ShowSkillFeedback("Level Up!", 3));
        }
        invulnerabilityDuration = invulnerabilityBaseDuration + invulnerabilityDurationPerLevel * invulnerabilityLevel;
        shieldParticle.startLifetime = invulnerabilityDuration;
        print("Invulnerabilty LevelUp!");
    }

    public void TantrumLevelUp()
    {
        tantrumLevel++;
        if (tantrumLevel == 1)
        {
            StartCoroutine(ShowSkillFeedback("Unlocked!", 4));
        }
        else
        {
            StartCoroutine(ShowSkillFeedback("Level Up!", 4));
        }
        tantrumDuration = tantrumBaseDuration + tantrumDurationPerLevel * tantrumLevel;
        print("Tantrum LevelUp!");
    }

    //========================================= Use/End Skills =================================//

    public void UseLunge()
    {
        estocadaParticle.Play();
        animator.SetTrigger("Lunge");
        lungeAvailable = false;
        lungeTimer = 0;
        player.isUsingSkill = true;
    }

    public void UseSpin()
    {
        spinParticle.Play();
        animator.SetTrigger("Spin");
        spinAvailable = false;
        spinTimer = 0;
        player.isUsingSkill = true;
    }

    public void EndSkill()
    {
        player.isUsingSkill = false;
    }

    public void UseCritical()
    {
        critParticle.Play();
        criticalActive = true;
        player.critChance += 100;
        cd3.fillAmount = 1;
        criticalTime = 0;
        criticalActive = true;
        mybuff3 = Instantiate(buff3prefab, buffarea); 
    }

    private void EndCritical()
    {
        criticalActive = false;
        criticalTimer = 0;
        player.critChance -= 100;
        if (mybuff3!=null)
        {
        Destroy(mybuff3.gameObject);
        }
    }

    public void UseInvulnerability()
    {
        shieldParticle.Play();
        invulnerabilityActive = true;
        cd4.fillAmount = 1;
        invulnerabilityTime = 0;
        player.invulnerability = true;
        mybuff4 = Instantiate(buff4prefab, buffarea);
    }

    private void EndInvulnerability()
    {
        invulnerabilityActive = false;
        invulnerabilityTimer = 0;
        player.invulnerability = false;
        if (mybuff4 != null)
        {
            Destroy(mybuff4.gameObject);
        }
    }

    private void UseTantrum()
    {
        tantrumParticle.Play();
        player.movementSpeed = 10f;
        cd5.fillAmount = 1;
        player.RefreshStat("strength", player.strength);
        tantrumActive = true;
        tantrumTime = 0;
        mybuff5 = Instantiate(buff5prefab, buffarea);
    }

    private void EndTantrum()
    {
        player.movementSpeed = 9f;
        player.RefreshStat("strength", -player.strength/2);
        tantrumActive = false;
        tantrumTimer = 0;
        if (mybuff5 != null)
        {
            Destroy(mybuff5.gameObject);
        }

    }

    //=========================================== Managers ======================================//

    private void CooldownManager()
    {

        //Lunge
        if (!lungeAvailable)
        {
            if (lungeTimer < lungeCooldown)
            {
                lungeTimer += Time.deltaTime;
                cd1.fillAmount = 1 - (lungeTimer / lungeCooldown);
                lungeAvailable = false;
            }
            else
            {
                lungeTimer = lungeCooldown;
                cd1.fillAmount = 0;
                lungeAvailable = true;
            }
        }

        //Spin
        if (!spinAvailable)
        {
            if (spinTimer < spinCooldown)
            {
                spinTimer += Time.deltaTime;
                cd2.fillAmount = 1 - (spinTimer / spinCooldown);
                spinAvailable = false;
            }
            else
            {
                spinTimer = spinCooldown;
                cd2.fillAmount = 0;
                spinAvailable = true;
            }
        }

        //Critical
        if (!criticalActive)
        {
            if (criticalTimer < criticalCooldown)
            {
                print("TESTE");
                criticalTimer += Time.deltaTime;
                cd3.fillAmount = 1 - (criticalTimer / criticalCooldown);
                criticalAvailable = false;
            }
            else
            {
                criticalTimer = criticalCooldown;
                cd3.fillAmount = 0;
                criticalAvailable = true;
            }
        }
        else
        {
            if (criticalTime < criticalDuration)
            {
                criticalTime += Time.deltaTime;
            }
            else
            {
                EndCritical();
            }
        }

        //Invulnerability
        if (!invulnerabilityActive)
        {
            if (invulnerabilityTimer < invulnerabilityCooldown)
            {
                invulnerabilityTimer += Time.deltaTime;
                cd4.fillAmount = 1 - (invulnerabilityTimer / invulnerabilityCooldown);
                invulnerabilityAvailable = false;
            }
            else
            {
                invulnerabilityTimer = invulnerabilityCooldown;
                cd4.fillAmount = 0;
                invulnerabilityAvailable = true;
            }
        }
        else
        {
            if (invulnerabilityTime < invulnerabilityDuration)
            {
                invulnerabilityTime += Time.deltaTime;
            }
            else
            {
                EndInvulnerability();
            }
        }

        //Tantrum
        if (!tantrumActive)
        {
            if (tantrumTimer < tantrumCooldown)
            {
                tantrumTimer += Time.deltaTime;
                cd5.fillAmount = 1 - (tantrumTimer / tantrumCooldown);
                tantrumAvailable = false;
            }
            else
            {
                tantrumTimer = tantrumCooldown;
                cd5.fillAmount = 0;
                tantrumAvailable = true;
            }
        }
        else
        {
            if (tantrumTime < tantrumDuration)
            {
                tantrumTime += Time.deltaTime;
            }
            else
            {
                EndTantrum();
            }
        }
    }

    private void InputManager()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && lungeAvailable && lungeUnlocked && !player.isUsingSkill)
        {
            UseLunge();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && spinAvailable && spinUnlocked && !player.isUsingSkill)
        {
            UseSpin();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && criticalAvailable && criticalUnlocked && !criticalActive)
        {
            UseCritical();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && invulnerabilityAvailable && invulnerabilityUnlocked && !invulnerabilityActive)
        {
            UseInvulnerability();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && tantrumAvailable && tantrumUnlocked && !tantrumActive)
        {
            UseTantrum();
        }
    }


    //=========================================== Damage ======================================//
    public void SpinDamage()
    {
        Collider[] enemies = Physics.OverlapSphere(player.transform.position, spinRange);

        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>()) enemy.gameObject.GetComponent<Enemy>().TakeDamage((int)(spinDamage + player.meleeDmg), 0);
        }
    }

    public void LungeDamage()
    {
        //Vector3 spherePos = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z + 2.5f);
        Collider[] enemies = Physics.OverlapSphere(player.damageSphere.position, 1.5f);

        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>()) enemy.gameObject.GetComponent<Enemy>().TakeDamage((int)(lungeDamage + player.meleeDmg), 0);
        }
    }

    public IEnumerator ShowSkillFeedback(string type,int skill)
    {
        skillFeedback[skill].SetActive(true);
        skillFeedback[skill].transform.GetComponent<TextMeshProUGUI>().text = type;
        yield return new WaitForSeconds(4);
        skillFeedback[skill].SetActive(false);
        
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Vector3 sas = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z + 2.5f);
    //    Gizmos.DrawSphere(sas, lungeRange);
    //}
}
