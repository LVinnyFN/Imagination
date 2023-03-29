using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;

//Automatically create a CharacterController Component when assisgned to an object
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public Inventory myinventory;
    [SerializeField] private GameObject lifeLossPrefab;
    [SerializeField] private GameObject lifeGainPrefab;
    [SerializeField] private SaveController saveController;

    //Save Extrainfo
    public PlayerInfo player1;

    //Primary Stats
    public int strength, agility, intelligence, vitality;
    [SerializeField] private Text strengthText, agilityText, intelligenceText, vitalityText;
    private int atributePoints = 0;
    [SerializeField] private Text atributepointsText;
    [SerializeField] private GameObject strplus;
    [SerializeField] private GameObject agiplus;
    [SerializeField] private GameObject intplus;
    [SerializeField] private GameObject vitplus;
    [SerializeField] private GameObject useatributepoints;
    [SerializeField] private GameObject useskillpoints;

    //Skills
    public int skillPoints = 0;
    [SerializeField] private Text skillpointsText;

    [SerializeField] private GameObject skill1;
    [SerializeField] private GameObject skill2;
    [SerializeField] private GameObject skill3;
    [SerializeField] private GameObject skill4;
    [SerializeField] private GameObject skill5;

    [SerializeField] private GameObject passive1;
    [SerializeField] private GameObject passive2;
    [SerializeField] private GameObject passive3;
    [SerializeField] private GameObject passive4;
    [SerializeField] private GameObject passive5;
    [SerializeField] private GameObject passive6;

    //Secondary Stats
    public float meleeDmg, critDmg, critChance;
    public float currentlife, maxlife, cooldown = 1;
    public bool invulnerability;
    [SerializeField] private Text lifeText, meleeDmgText, critDmgText, critChanceText, cooldownText;
    private int armor;
    [SerializeField] private Text armorText;
    public int level = 1, maxLevel, currentXP, nextLevelXP, lastlevelXP;
    [SerializeField] private Text levelText, nextLevelXPText;
    [SerializeField] private Slider xpsliderStats;
    public int potioneffect; //em percentual
    [SerializeField] private Text potioneffecttext;

    //Movement Variables
    [SerializeField]
    public float movementSpeed = 5f;
    private float rotationSpeed = 0.3f, gravity = 5f;
    private float defendingSpeed, finalSpeed;
    private float allowRotation = 0.1f;
    private float inputX, inputZ, speed;
    private Vector3 desiredMoveDirection;
    private Camera cam;
    public GameObject target; //Lock on
    public float lockRange = 10.0f;
    public bool isUsingSkill = false;

    //Components
    private CharacterController characterController;
    public Animator animator;
    private PlayerSoundManager playerSoundManager;
    private SkillsManager skillsManager;
    public PassivesManager passivesManager;

    //Useful
    public bool isAttacking;
    private bool isDefending;
    public bool dealtDamage = false;
    public bool isDead;
    public Transform damageSphere;

    //Particles
    [SerializeField] private ParticleSystem levelUPParticle;
    [SerializeField] private ParticleSystem healParticle;

    //Tutorial
    bool potionTutorialBool = false;
    public GameObject tutarea;
    public GameObject tutquestion;

    //Skills

    private void Awake()
    {
        GameController.controller.player = this.gameObject;
    }
    void Start()
    {
        if (GameController.controller.savenumber == 0) // caso new game, seta info inicial do player
        {
            player1 = new PlayerInfo();
            // transform.position = new Vector3(-316.9f, -6.127f, 67.6f);
            level = 1;
            nextLevelXP = 100;
            lastlevelXP = 0;
            maxlife = 300;
            currentlife = maxlife;
            strength = 5;
            agility = 5;
            intelligence = 5;
            vitality = 5;
            armor = 1;
            cooldown = 0;
            potioneffect = 10;
            meleeDmg = 25;
            critDmg = 1.5f;
            critChance = 10;
        }
        else
        {
            Load();
        }

        //Assigning the components
        defendingSpeed = movementSpeed / 4.0f;
        characterController = GetComponent<CharacterController>();
        playerSoundManager = GetComponent<PlayerSoundManager>();
        animator = GetComponent<Animator>();
        skillsManager = GetComponent<SkillsManager>();
        passivesManager = GetComponent<PassivesManager>();
        cam = Camera.main;

        //RefreshStats
        RefreshStatsUI();
        RefreshSkillsUI();
        //HideCursor
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = true;

    }

    void Update()
    {
        //Update the input values
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        if (!isDead)
        {
            Walk();
            CombatManager();
        }

        //CHEATS de teste

        if (Input.GetKeyDown(KeyCode.F4)) // F4 para adicionar 500 de ouro
        {
            myinventory.AddGold(500);
        }
        if (Input.GetKeyDown(KeyCode.F5)) // F5 para adquirir um item aleatório tier 1
        {
            Item cheatitem = new Equipable();
            cheatitem = DataBase.dataBase.CreateEquipableByTier(1);
            myinventory.AddItem(cheatitem, 1);
        }
        if (Input.GetKeyDown(KeyCode.F6)) // F6 para adquirir um item aleatório tier 2
        {
            Item cheatitem = new Equipable();
            cheatitem = DataBase.dataBase.CreateEquipableByTier(2);
            myinventory.AddItem(cheatitem, 1);
        }
        if (Input.GetKeyDown(KeyCode.F7)) // F7 para adquirir um item aleatório tier 3
        {
            Item cheatitem = new Equipable();
            cheatitem = DataBase.dataBase.CreateEquipableByTier(3);
            myinventory.AddItem(cheatitem, 1);
        }
        //if (Input.GetKeyDown(KeyCode.F9)) // F9 para Level UP
        //{
        //    LevelUP();
        //}
        if (Input.GetKeyDown(KeyCode.F2)) // F2 para perder 5% da vida
        {
            TakeDamage((int)(maxlife * 0.05f));
        }
        if (Input.GetKeyDown(KeyCode.F9)) // F3 para ganhar 2000 de xp
        {
            GainXP(nextLevelXP-currentXP);
        }
    }


    public void RefreshStatsUI()
    {
        strengthText.text = strength.ToString();
        agilityText.text = agility.ToString();
        intelligenceText.text = intelligence.ToString();
        vitalityText.text = vitality.ToString();
        atributepointsText.text = atributePoints.ToString();
        lifeText.text = ((int)currentlife).ToString() + " / " + ((int)maxlife).ToString();
        meleeDmgText.text = meleeDmg.ToString();
        critDmgText.text = critDmg.ToString();
        critChanceText.text = critChance.ToString();
        armorText.text = armor.ToString();
        levelText.text = level.ToString();
        xpsliderStats.value = (float)(currentXP - lastlevelXP) / (nextLevelXP - lastlevelXP);
        nextLevelXPText.text = currentXP.ToString() + " / " + nextLevelXP.ToString();
        potioneffecttext.text = potioneffect.ToString() + "%";
        cooldownText.text = (cooldown * 100).ToString() + "%";

        //Refreshes HP Slider on HUD
        GameController.controller.uiController.RefreshPlayerLifeBar(currentlife / maxlife);
    }

    public void RefreshSkillsUI()
    {
        //Atualiza num skill points
        skillpointsText.text = skillPoints.ToString();

        //Atualiza textos de leveis
        //Ativas

        skill1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<SkillsManager>().lungeLevel.ToString();
        skill2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<SkillsManager>().spinLevel.ToString();
        skill3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<SkillsManager>().criticalLevel.ToString();
        skill4.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<SkillsManager>().invulnerabilityLevel.ToString();
        skill5.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<SkillsManager>().tantrumLevel.ToString();
        //Passivas
        passive1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<PassivesManager>().LifeStealLevel.ToString();
        passive2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<PassivesManager>().ThornsLevel.ToString();
        passive3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<PassivesManager>().PoisonLevel.ToString();
        passive4.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<PassivesManager>().ReviveLevel.ToString();
        passive5.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<PassivesManager>().BargainLevel.ToString();
        passive6.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level " + GetComponent<PassivesManager>().LootingLevel.ToString();

        //Atualiza cadeado
        //Ativas
        if (GetComponent<SkillsManager>().lungeLevel > 0)
        {
            skill1.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (GetComponent<SkillsManager>().spinLevel > 0)
        {
            skill2.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (GetComponent<SkillsManager>().criticalLevel > 0)
        {
            skill3.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (GetComponent<SkillsManager>().invulnerabilityLevel > 0)
        {
            skill4.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (GetComponent<SkillsManager>().tantrumLevel > 0)
        {
            skill5.transform.GetChild(0).gameObject.SetActive(false);
        }

        //Passivas
        if (GetComponent<PassivesManager>().LifeStealLevel > 0)
        {
            passive1.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().LifeStealLevel >=5)
        {
            passive1.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().ThornsLevel > 0)
        {
            passive2.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().ThornsLevel >= 5)
        {
            passive2.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().PoisonLevel > 0)
        {
            passive3.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().PoisonLevel >= 5)
        {
            passive3.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().ReviveLevel > 0)
        {
            passive4.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().ReviveLevel >= 5)
        {
            passive4.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().BargainLevel > 0)
        {
            passive5.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().BargainLevel >= 5)
        {
            passive5.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().LootingLevel > 0)
        {
            passive6.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (GetComponent<PassivesManager>().LootingLevel >= 5)
        {
            passive6.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    //Add points to a specific atribute after level up
    public void AddPoints(string where)
    {
        if (atributePoints > 0)
        {
            RefreshStat(where, 1);
            atributePoints--;
            RefreshStatsUI();
        }
        if (atributePoints == 0)
        {
            strplus.SetActive(false);
            agiplus.SetActive(false);
            intplus.SetActive(false);
            vitplus.SetActive(false);
            useatributepoints.SetActive(false);
        }
    }

    //Level up skill
    public void LevelUpSkill(string which)
    {
        if (skillPoints > 0)
        {
            RefreshSkill(which);
            RefreshSkillsUI();
        }
        if (skillPoints == 0)
        {
            passive1.transform.GetChild(2).gameObject.SetActive(false);
            passive2.transform.GetChild(2).gameObject.SetActive(false);
            passive3.transform.GetChild(2).gameObject.SetActive(false);
            passive4.transform.GetChild(2).gameObject.SetActive(false);
            passive5.transform.GetChild(2).gameObject.SetActive(false);
            passive6.transform.GetChild(2).gameObject.SetActive(false);
            useskillpoints.SetActive(false);

        }
    }

    public void RefreshStat(string stat, float value)
    {
        switch (stat)
        {
            case "strength":
                strength += (int)value;
                meleeDmg += value;
                break;
            case "agility":
                agility += (int)value;
                RefreshStat("critChance", value);
                RefreshStat("critDmg", value);
                break;
            case "intelligence":
                intelligence += (int)value;
                RefreshStat("cooldown", value);
                RefreshStat("potioneffect", value);
                break;
            case "vitality":
                vitality += (int)value;
                RefreshStat("life", value);
                break;
            case "meleeDmg":
                meleeDmg += value;
                break;
            case "armor":
                armor += (int)value;
                break;
            case "life":
                currentlife += value * 10 * currentlife / maxlife;
                maxlife += value * 10;
                break;
            case "critDmg":
                critDmg += value * 0.1f;
                break;
            case "critChance":
                critChance += value;
                break;
            case "cooldown":
                if (cooldown < 0.9)
                {
                    cooldown += 0.01f;
                }
                GetComponent<SkillsManager>().UpdateCooldowns();
                break;
            case "potioneffect":
                if (potioneffect < 100)
                {
                    potioneffect += (int)value;
                }
                break;
            default:
                break;
        }
        RefreshStatsUI();
    }

    public void RefreshSkill(string skill)
    {
        switch (skill)
        {
            case "passive1":
                GetComponent<PassivesManager>().LifeStealLevelUp();
                break;
            case "passive2":
                GetComponent<PassivesManager>().ThornsLevelUp();
                break;
            case "passive3":
                GetComponent<PassivesManager>().PoisonLevelUp();
                break;
            case "passive4":
                GetComponent<PassivesManager>().ReviveLevelUp();
                break;
            case "passive5":
                GetComponent<PassivesManager>().BargainLevelUp();
                break;
            case "passive6":
                GetComponent<PassivesManager>().LootingLevelUp();
                break;
            default:
                break;
        }
    }

    // =============================== SAVE INFO ========================================

    public void Sincronize()
    {
        player1.savenumber = GameController.controller.savenumber;
        player1.playername = GameController.controller.playername;
        player1.tutorialdone = GameController.controller.tutorialdone;
        player1.positionx = transform.position.x;
        player1.positiony = transform.position.y;
        player1.positionz = transform.position.z;
        player1.life = currentlife;
        player1.maxlife = maxlife;
        player1.level = level;
        player1.strength = strength;
        player1.agility = agility;
        player1.intelligence = intelligence;
        player1.vitality = vitality;
        player1.armor = armor;
        player1.meleeDmg = meleeDmg;
        player1.critChance = critChance;
        player1.critDmg = critDmg;
        player1.cooldown = cooldown;
        player1.potioneffect = potioneffect;
        player1.nextLevelXP = nextLevelXP;
        player1.atributePoints = atributePoints;
        player1.currentXP = currentXP;
        player1.lastlevelXP = lastlevelXP;
        player1 = myinventory.SincronizeInv(player1);
    }


    public void Load()
    {
        player1 = new PlayerInfo();
        player1 = GameController.controller.playerData;
        GameController.controller.savenumber = player1.savenumber;
        GameController.controller.playername = player1.playername;
        GameController.controller.tutorialdone = player1.tutorialdone;
        if (player1.tutorialdone)
        {
            if (tutarea!=null)
            {
            tutarea.SetActive(false);
            }
            tutquestion.SetActive(false);
            isDead = false;
            GameController.controller.uiController.UseMouse(false);
        }
        lastlevelXP = player1.lastlevelXP;
        GetComponent<CharacterController>().enabled = false;
        transform.position = new Vector3(player1.positionx, player1.positiony, player1.positionz);
        GetComponent<CharacterController>().enabled = true;
        strength = player1.strength;
        agility = player1.agility;
        intelligence = player1.intelligence;
        vitality = player1.vitality;
        currentlife = player1.life;
        maxlife = player1.maxlife;
        level = player1.level;
        currentXP = player1.currentXP;
        armor = player1.armor;
        critChance = player1.critChance;
        critDmg = player1.critDmg;
        cooldown = player1.cooldown;
        potioneffect = player1.potioneffect;
        meleeDmg = player1.meleeDmg;
        nextLevelXP = player1.nextLevelXP;
        atributePoints = player1.atributePoints;
        myinventory.LoadInventory(player1);
        RefreshStatsUI();
        RefreshSkillsUI();
    }

    // =============================== MOVEMENT ========================================

    //Call the movement functions
    private void Walk()
    {
        //Set the animation speed parameter
        animator.SetFloat("speed", Mathf.Max(Mathf.Abs(inputX), Mathf.Abs(inputZ)) * finalSpeed / movementSpeed);

        //LockOnTarget();
        InputDecider();
        MovementManager();
    }

    //Attack
    private void CombatManager()
    {
        if (!GameController.controller.uiController.CheckPanels() && !GameController.controller.uiController.CheckDialogs() && !isDead)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                animator.SetTrigger("attack");
                dealtDamage = false;
            }
            else if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Joystick1Button10))
            {
                isDefending = true;
                animator.SetBool("isDefending", true);
                finalSpeed = defendingSpeed;
            }

            else
            {
                isDefending = false;
                animator.SetBool("isDefending", false);

                finalSpeed = movementSpeed;
            }

            if (animator.GetCurrentAnimatorStateInfo(1).IsTag("1")) //Check if player is attacking
            {
                isAttacking = true;
            }
            else isAttacking = false;
        }
    }

    private void AttackAnim()
    {
        Collider[] enemies = Physics.OverlapSphere(damageSphere.position, 1f);

        //Generate damage
        int[] dmg = new int[2];
        dmg = DealDamage();

        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>()) enemy.gameObject.GetComponent<Enemy>().TakeDamage(dmg[0], dmg[1]);
        }
    }

    //Decide which direction the player will go
    private void InputDecider()
    {
        speed = new Vector2(inputX, inputZ).sqrMagnitude;

        if (speed > allowRotation) //if can rotate and isn't locked on target
        {
            RotationManager();
        }
        else desiredMoveDirection = Vector3.zero;

    }

    //Rotate to the desired move direction
    private void RotationManager()
    {
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * inputZ + right * inputX;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
    }

    //Move using the CharacterController component
    private void MovementManager()
    {
        Vector3 moveDirection = desiredMoveDirection * finalSpeed * Time.deltaTime;
        moveDirection.y -= gravity * Time.deltaTime;
        if (isUsingSkill)
        {
            moveDirection.x = 0;
            moveDirection.z = 0;
        }
        characterController.Move(moveDirection);
    }

    ////Lock on a target
    //void LockOnTarget()
    //{
    //    bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit); //Ray towards cursor (that is locked in center)

    //    if (Input.GetMouseButtonDown(2))
    //    {
    //        if (hasHit && target == null)
    //        {
    //            if (hit.transform.gameObject.CompareTag("Enemy"))
    //            {
    //                target = hit.transform.gameObject;
    //            }
    //        }
    //        else
    //        {
    //            target = null;
    //        }
    //    }
    //}

    ////Returns a ray originated from the screen (camera near plane) and direct to cursor at the world space
    //private static Ray GetMouseRay()
    //{
    //    return Camera.main.ScreenPointToRay(Input.mousePosition);
    //}

    // =============================== MECHANICS ========================================



    //Gain XP
    public void GainXP(int value)
    {
        if (level < maxLevel)
        {
            currentXP += value;
            //level up
            if (currentXP >= nextLevelXP)
            {
                LevelUP();
            }
            GameController.controller.uiController.RefreshXP((float)(currentXP-lastlevelXP) / (nextLevelXP - lastlevelXP));
            RefreshStatsUI();
            RefreshSkillsUI();
        }
    }

    private void LevelUP()
    {
        currentlife = maxlife;
        GameController.controller.uiController.LevelUP();
        int aux = lastlevelXP;
        lastlevelXP = nextLevelXP;
        nextLevelXP = nextLevelXP + (int)((nextLevelXP - aux) * 1.4f);
        level++;
        atributePoints += 5;
        skillPoints++;
        ActivePlusSignals();
        UpdateSkills();
        if (currentXP >= nextLevelXP)
        {
            LevelUP();
            return;
        }
        currentlife = maxlife;
        RefreshStatsUI();
        RefreshSkillsUI();
        levelUPParticle.Play();
        GameController.controller.uiController.RefreshUI();
    }

    private void UpdateSkills()
    {
        int skill = (level - 1) % 5;

        if (!skillsManager.TantrumUnlocked)
        {
            switch (skill)
            {
                case 1:
                    skillsManager.UnlockLunge();
                    break;

                case 2:
                    skillsManager.UnlockSpin();
                    break;

                case 3:
                    skillsManager.UnlockCritical();
                    break;

                case 4:
                    skillsManager.UnlockInvulnerability();
                    break;

                case 0:
                    skillsManager.UnlockTantrum();
                    break;
            }
        }
        else
        {
            switch (skill)
            {
                case 1:
                    skillsManager.LungeLevelUp();
                    break;

                case 2:
                    skillsManager.SpinLevelUp();
                    break;

                case 3:
                    skillsManager.CriticalLevelUp();
                    break;

                case 4:
                    skillsManager.InvulnerabilityLevelUp();
                    break;

                case 0:
                    skillsManager.TantrumLevelUp();
                    break;
            }
        }
    }

    public void ActivePlusSignals()
    {
        if (atributePoints>0)
        {
            useatributepoints.SetActive(true);
            strplus.SetActive(true);
            agiplus.SetActive(true);
            intplus.SetActive(true);
            vitplus.SetActive(true);
        }
        if (skillPoints>0)
        {
            useskillpoints.SetActive(true);
            if (GetComponent<PassivesManager>().LifeStealLevel <= 5)
            {
                passive1.transform.GetChild(2).gameObject.SetActive(true);
            }
            if (GetComponent<PassivesManager>().ThornsLevel <= 5)
            {
                passive2.transform.GetChild(2).gameObject.SetActive(true);
            }
            if (GetComponent<PassivesManager>().PoisonLevel <= 5)
            {
                passive3.transform.GetChild(2).gameObject.SetActive(true);
            }
            if (GetComponent<PassivesManager>().ReviveLevel <= 5)
            {
                passive4.transform.GetChild(2).gameObject.SetActive(true);
            }
            if (GetComponent<PassivesManager>().BargainLevel <= 5)
            {
                passive5.transform.GetChild(2).gameObject.SetActive(true);
            }
            if (GetComponent<PassivesManager>().LootingLevel <= 5)
            {
                passive6.transform.GetChild(2).gameObject.SetActive(true);
            }
        }


    }

    //Deal Damage
    public int[] DealDamage()
    {
        int[] damage = new int[2];

        if (UnityEngine.Random.Range(0, 100) <= critChance)
        {
            damage[0] = (int)(meleeDmg * critDmg);
            damage[1] = 1;
        }
        else
        {
            damage[0] = (int)meleeDmg;
            damage[1] = 0;
        }
        return damage;
    }


    //Take Damage
    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            if (!invulnerability) //If isn't invulnerable
            {
                int realDmg;
                if (!isDefending)
                {
                    realDmg = Mathf.Max((damage - armor / 2), 1);

                    animator.SetTrigger("takeDamage");
                    playerSoundManager.TakeDamage(); //Sound
                }
                else
                {
                    realDmg = Mathf.Max((damage / 3 - armor), 1);
                    playerSoundManager.Defend();
                }
                GameController.controller.uiController.potionref.StopAllCoroutines();
                currentlife -= realDmg;
                GameController.controller.uiController.RefreshPlayerLifeBar(currentlife / maxlife);

                if (currentlife <= 0)
                {
                    StartCoroutine(Die());
                }
                GameObject dmgtext = Instantiate(lifeLossPrefab, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
                dmgtext.GetComponent<PlayerLifeLoss>().SetDmg(realDmg, isDefending);
                //dmgtext.GetComponent<PlayerLifeLoss>().SetDmg(realDmg,isDefending);
                RefreshStatsUI();
            }
            else
            {
                //Invulnerability feedback
                print("CAN'T TOUCH THIS!");
            }

        }
        if (!potionTutorialBool)
        {
            potionTutorialBool = true;
            StartCoroutine(GameController.controller.uiController.ShowPotionTutorial());
        }
    }

    public void Heal(int percentage)
    {
        GameObject healtxt = Instantiate(lifeGainPrefab, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
        float heal = maxlife * percentage / 100;
        healtxt.GetComponent<PlayerLifeGain>().SetHeal(heal);
        currentlife += heal;
        if (currentlife > maxlife)
        {
            currentlife = maxlife;
        }
        RefreshStatsUI();
        healParticle.Play();
    }

    public void Revive()
    {
        currentlife += maxlife * passivesManager.getReviveHpPercentage() / 100;
        if (currentlife > maxlife)
        {
            currentlife = maxlife;
        }
        RefreshStatsUI();
        //TOCAR FEEDBACK
    }

    //Death
    public IEnumerator Die()
    {
        if (passivesManager.getReviveUnlocked() && UnityEngine.Random.Range(0, 100) <= passivesManager.getReviveChance()) //Revive
        {
            Revive();
        }
        else
        {
            playerSoundManager.Die();
            isDead = true;
            animator.SetBool("isDead", true);
            yield return new WaitForSeconds(2f);
            GameController.controller.uiController.ShowDeathPanel();
        }
    }


    //========================= PASSIVES ===================//
    public void HealValue(int value)
    {
        currentlife += value;

        if (currentlife > maxlife) currentlife = maxlife;

        RefreshStatsUI();
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(damageSphere.position, 1f);
    }
}
