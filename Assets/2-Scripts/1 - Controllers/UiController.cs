using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class UiController : MonoBehaviour
{
    //Audio
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider fxVolume;

    //HUD
    [SerializeField] private RectTransform coracao;
    [SerializeField] private Sprite coracaosprite;
    [SerializeField] private Sprite caveirasprite;
    [SerializeField] private Animator coracaoanimator;
    [SerializeField] private TextMeshProUGUI hudlevel;
    public Potion potionref;
    [SerializeField] private TextMeshProUGUI potionquantitytext;


    [SerializeField] private PostProcessVolume ppv;
    private Vignette vignette;
    private ChromaticAberration aberration;
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private GameObject levelUPButton;

    //Tooltip Itens
    //Equipable
    [SerializeField] private GameObject equipableTooltip;
    [SerializeField] private GameObject equipTooltipSprite;
    [SerializeField] private GameObject equipTooltipName;
    [SerializeField] private GameObject tooltipMain;
    [SerializeField] private GameObject tooltipPrimary;
    [SerializeField] private GameObject tooltipSecondary1;
    [SerializeField] private GameObject tooltipSecondary2;
    //CraftMat
    [SerializeField] private GameObject craftmatTooltip;
    [SerializeField] private GameObject craftTooltipSprite;
    [SerializeField] private GameObject craftTooltipName;
    [SerializeField] private GameObject craftTooltipDescription;
    //Skills
    [SerializeField] private GameObject skillTooltip;


    [SerializeField] private Drag dragobj;

    //Primary Panels
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] public GameObject questlogPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject mapPanel;

    //SecondaryPanels
    [SerializeField] private GameObject blackshoppanel;
    [SerializeField] private GameObject blackcraftpanel;
    [SerializeField] private GameObject blackshopgrid;
    [SerializeField] private GameObject blackcopygrid;
    [SerializeField] private GameObject shopshopgrid;
    [SerializeField] private GameObject shopcopygrid;
    [SerializeField] private GameObject hotkeysimg;
    [SerializeField] private GameObject audiopanel;
    [SerializeField] private GameObject videopanel;
    [SerializeField] private GameObject equipbuttom;
    [SerializeField] private GameObject skiptut;


    //References
    //Feedbacks
    [SerializeField] private GameObject completedQuestAnim;
    public GameObject itempicked;
    public GameObject item2picked;

    //Quest
    public QuestMenu questMenu;
    public ShopNPC shopNPC;

    //Shop - Craft - Inventory
    public GameObject shopMenu;
    public GameObject inventoryCopy;
    public GameObject inventory;
    public CraftGrid craftgrid;
    public Text playerleveltext;
    public Text goldtext;
    public Text goldtextCopy;
    public Text goldtextCopyCraft;
    public Text refreshCostText;

    //Other

    public SaveController savecontroller;
    public Fade fade;
    [SerializeField] private GameObject talkaction;
    [SerializeField] private GameObject pickupaction;
    [SerializeField] private GameObject deathPanel;
    private int refreshcontblack = 0; // contador de refreshs do blacksmith
    private int refreshcostblack = 0; // custo do refresh do blacksmith
    private int refreshcontshop = 0; // contador de refreshs do blacksmith
    private int refreshcostshop = 0; // custo do refresh do blacksmith
    [SerializeField] private GameObject mysword;
    [SerializeField] private GameObject myshield;


    //Camera
    public CinemachineFreeLook mainCam;
    public float camYSpeed = 2, camXSpeed = 200;

    //Temporary
    public MaterialMachine machine;

    //Tutorial
    [SerializeField] GameObject tutmush;
    [SerializeField] GameObject potionTutorial;
    [SerializeField] GameObject dialogtut;
    [SerializeField] GameObject dialogshop;
    [SerializeField] GameObject dialogblack;
    [SerializeField] GameObject dialogbox;

    [SerializeField] GameObject confirmdelete;
    [SerializeField] GameObject confirmquit;

    

    //Equip glow
    [SerializeField] private GameObject glowhead;
    [SerializeField] private GameObject glowonehanded;
    [SerializeField] private GameObject glowshield;
    [SerializeField] private GameObject glowbody;
    [SerializeField] private GameObject glowfoot;

    void Start()
    {
        GameController.controller.uiController = this;
        GameController.controller.transform.GetChild(0).GetComponent<AudioSource>().clip = GameController.controller.myaudios[3];
        GameController.controller.transform.GetChild(0).GetComponent<AudioSource>().Play();
        CloseAll();
        CloseOptions();
        RefreshUI();
        ppv.profile.TryGetSettings(out vignette);
        ppv.profile.TryGetSettings(out aberration);
        UseMouse(true);
        GameController.controller.player.GetComponent<Player>().isDead = true;
        skiptut.SetActive(true);

    }

    void Update()
    {
        CheckHotkeys();
    }

    public void SkipTutorial(bool opcao)
    {
        if (opcao)
        {
            GameController.controller.tutorialdone = true;
            Item myItem;
            //Creating Initial Wooden Sword
            Equipable myEquip1 = new Equipable();
            myEquip1.itemName = "Cardboard Sword";
            myEquip1.mainAtribute = "PDmg";
            myEquip1.mainValue = 1;
            myEquip1.price = 1;
            myEquip1.primaryAtribute = "Str";
            myEquip1.primaryValue = 1;
            myEquip1.subType = "Cardboard Sword";
            myEquip1.tier = 1;
            myEquip1.type = "Weapon";
            myEquip1.itemIcon = DataBase.dataBase.myEquipSprites[0];

            myItem = myEquip1;
            GameController.controller.player.GetComponent<Player>().myinventory.AddItem(myItem, 1);
            GameController.controller.player.GetComponent<Player>().myinventory.SelectSlotCopy(0);
            GameController.controller.player.GetComponent<Player>().myinventory.EquipItem();

            //Creating Initial Wooden Shield
            Equipable myEquip2 = new Equipable();
            myEquip2.itemName = "Cardboard Shield";
            myEquip2.mainAtribute = "Armor";
            myEquip2.mainValue = 1;
            myEquip2.price = 1;
            myEquip2.primaryAtribute = "Vit";
            myEquip2.primaryValue = 1;
            myEquip2.subType = "Cardboard Shield";
            myEquip2.tier = 1;
            myEquip2.type = "Shield";
            myEquip2.itemIcon = DataBase.dataBase.myEquipSprites[3];

            myItem = myEquip2;

            GameController.controller.player.GetComponent<Player>().myinventory.AddItem(myItem, 1);
            GameController.controller.player.GetComponent<Player>().myinventory.SelectSlotCopy(0);
            GameController.controller.player.GetComponent<Player>().myinventory.EquipItem();

            mysword.SetActive(true);
            myshield.SetActive(true);

            StartCoroutine(DestroyTutorial());
            GameController.controller.player.GetComponent<Player>().isDead = false;
            savecontroller.SaveGame();

        }
        else
        {
            GameController.controller.player.GetComponent<Player>().isDead = false;
        }

    }

    IEnumerator DestroyTutorial()
    {
        tutmush.GetComponent<TutorialMush>().animator.SetTrigger("destroy");
        yield return new WaitForSeconds(2);
        Destroy(tutmush.GetComponent<TutorialMush>().tutarea.gameObject);
    }

    public void UseMouse(bool option)
    {
        if (option)
        {
            mainCam.m_YAxis.m_MaxSpeed = 0;
            mainCam.m_XAxis.m_MaxSpeed = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            mainCam.m_YAxis.m_MaxSpeed = camYSpeed;
            mainCam.m_XAxis.m_MaxSpeed = camXSpeed;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public void LevelUP()
    {
        StartCoroutine(LevelUPcor());
    }

    IEnumerator LevelUPcor()
    {
        levelUPButton.SetActive(true);
        yield return new WaitForSeconds(3);
        levelUPButton.SetActive(false);

    }

    public void HideLevelUP()
    {
        levelUPButton.SetActive(false);
    }

    public void ShowAction(string type, bool state)
    {
        if (type == "talk")
        {
            talkaction.SetActive(state);
        }
        else if (type == "pickup")
        {
            pickupaction.SetActive(state);
        }

    }

    public void RefreshUI()
    {
        potionquantitytext.text = potionref.potionquantity.ToString();
        RefreshInventory();
    }

    public void RefreshPlayerLifeBar(float value)
    {
        hudlevel.text = GameController.controller.player.GetComponent<Player>().level.ToString();
        if (value <= 0)
        {
            coracao.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            coracao.GetComponent<Image>().sprite = caveirasprite;
            coracaoanimator.enabled = false;
            lifeSlider.value = 0;
        }
        else
        {
            coracao.localScale = new Vector3(1, 1, 1);
            coracaoanimator.enabled = true;
            //coracao.localScale = new Vector3(value, value, value);
            coracao.GetComponent<Image>().sprite = coracaosprite;
            lifeSlider.value = value;
            if (GameController.controller.player.GetComponent<Player>().currentlife <= GameController.controller.player.GetComponent<Player>().maxlife * 0.1f)
            { //vida abaixo de 10%
                ResetTriggers();
                coracaoanimator.SetTrigger("10");
                vignette.intensity.value = 0.7f;
                float myaberration = aberration.intensity.value;
                aberration.intensity.value = 0.7f;
            }
            else if (GameController.controller.player.GetComponent<Player>().currentlife <= GameController.controller.player.GetComponent<Player>().maxlife * 0.25f)
            {//vida abaixo de 25%
                ResetTriggers();
                coracaoanimator.SetTrigger("25");
                vignette.intensity.value = 0.6f;
                float myaberration = aberration.intensity.value;
                aberration.intensity.value = 0.5f;
            }
            else if (GameController.controller.player.GetComponent<Player>().currentlife <= GameController.controller.player.GetComponent<Player>().maxlife * 0.5f)
            {//vida abaixo de 50%
                ResetTriggers();
                coracaoanimator.SetTrigger("50");
                vignette.intensity.value = 0.5f;
                float myaberration = aberration.intensity.value;
                aberration.intensity.value = 0.3f;
            }

            else if (GameController.controller.player.GetComponent<Player>().currentlife <= GameController.controller.player.GetComponent<Player>().maxlife * 0.75f)
            {//vida abaixo de 75%
                ResetTriggers();
                coracaoanimator.SetTrigger("75");
                vignette.intensity.value = 0.4f;
                float myaberration = aberration.intensity.value;
                aberration.intensity.value = 0.1f;

            }
            else //acima de 75%
            {
                ResetTriggers();
                coracaoanimator.SetTrigger("100");
                vignette.intensity.value = 0.3f;
                float myaberration = aberration.intensity.value;
                aberration.intensity.value = 0;
            }
        }
    }

    private void ResetTriggers()
    {
        coracaoanimator.ResetTrigger("10");
        coracaoanimator.ResetTrigger("25");
        coracaoanimator.ResetTrigger("50");
        coracaoanimator.ResetTrigger("75");
        coracaoanimator.ResetTrigger("100");
    }

    public void CleanGlow()
    {
        glowhead.SetActive(false);
        glowonehanded.SetActive(false);
        glowshield.SetActive(false);
        glowbody.SetActive(false);
        glowfoot.SetActive(false);
    }

    public void ShowEquipableTooltip(bool show, Equipable myitem)
    {
        if (show)
        {
            equipableTooltip.SetActive(true);
            equipTooltipSprite.GetComponent<Image>().sprite = myitem.itemIcon;
            equipTooltipSprite.transform.GetChild(0).GetComponent<Image>().sprite = DataBase.dataBase.myTierSprites[myitem.tier - 1];
            equipTooltipName.GetComponent<Text>().text = myitem.itemName;
            tooltipMain.GetComponent<Text>().text = "+ " + myitem.mainValue + " " + myitem.mainAtribute;
            tooltipPrimary.GetComponent<Text>().text = "+ " + myitem.primaryValue + " " + myitem.primaryAtribute;
            switch (myitem.tier)
            {
                case 1:
                    tooltipSecondary1.SetActive(false);
                    tooltipSecondary2.SetActive(false);
                    break;
                case 2:
                    tooltipSecondary1.SetActive(true);
                    tooltipSecondary1.GetComponent<Text>().text = "+ " + myitem.secondaryValue1 + " " + myitem.secondaryAtribute1;
                    tooltipSecondary2.SetActive(false);
                    break;
                case 3:
                    tooltipSecondary1.SetActive(true);
                    tooltipSecondary2.SetActive(true);
                    tooltipSecondary1.GetComponent<Text>().text = "+ " + myitem.secondaryValue1 + " " + myitem.secondaryAtribute1;
                    tooltipSecondary2.GetComponent<Text>().text = "+ " + myitem.secondayValue2 + " " + myitem.secondaryAtribute2;
                    break;
                default:
                    break;
            }
        }
        else
        {
            equipableTooltip.SetActive(false);

        }
    }

    public void ShowCraftMatTooltip(bool show, CraftMat myitem)
    {
        if (show)
        {
            craftmatTooltip.SetActive(true);
            craftTooltipSprite.GetComponent<Image>().sprite = myitem.itemIcon;
            craftTooltipName.GetComponent<Text>().text = myitem.itemName;
            craftTooltipDescription.GetComponent<Text>().text = myitem.description;
        }
        else
        {
            craftmatTooltip.SetActive(false);
        }
    }

    public void ShowSkillTooltip(bool show, string skill)
    {
        if (show)
        {
            skillTooltip.SetActive(true);
            switch (skill)
            { // Sprite (1) , Title (2), Description (3), LevelAtual (4), Prox Level (5), InfoAtual (6), InfoNext(7), Points Needed (8)
                case "skill1": // lunge attack
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.mySkillSprites[0];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "Lunge Attack";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.mySkillDescriptions[0];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<SkillsManager>().lungeLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "Deals " + GameController.controller.player.GetComponent<SkillsManager>().lungeDamage.ToString() + " damage";
                    if (GameController.controller.player.GetComponent<Player>().level < 27)
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Deals " + (GameController.controller.player.GetComponent<SkillsManager>().lungeDamage + GameController.controller.player.GetComponent<SkillsManager>().lungeDamagePerLevel).ToString() + " damage";
                        if (GameController.controller.player.GetComponent<Player>().level < 2)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                            skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Deals " + (GameController.controller.player.GetComponent<SkillsManager>().lungeBaseDamage + GameController.controller.player.GetComponent<SkillsManager>().lungeDamagePerLevel).ToString() + " damage";
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 2)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 7)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 7)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 12)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 12";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 17)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 17)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 22)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 22)";
                        }
                        else
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 27)";
                        }
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                case "skill2": //Spin attack
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.mySkillSprites[1];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "Spin Attack";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.mySkillDescriptions[1];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<SkillsManager>().spinLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "Deals " + GameController.controller.player.GetComponent<SkillsManager>().spinDamage.ToString() + " damage";
                    if (GameController.controller.player.GetComponent<Player>().level < 28)
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Deals " + (GameController.controller.player.GetComponent<SkillsManager>().spinDamage + GameController.controller.player.GetComponent<SkillsManager>().spinDamagePerLevel).ToString() + " damage";
                        if (GameController.controller.player.GetComponent<Player>().level < 3)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                            skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Deals " + (GameController.controller.player.GetComponent<SkillsManager>().spinBaseDamage + GameController.controller.player.GetComponent<SkillsManager>().spinDamagePerLevel).ToString() + " damage";
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 3)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 8)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 8)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 13)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 13)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 18)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 18)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 23)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 23)";
                        }
                        else
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 28)";
                        }
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                case "skill3": // Critical Frenzy
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.mySkillSprites[2];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "Critical Frenzy";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.mySkillDescriptions[2];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<SkillsManager>().criticalLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "Lasts for " + GameController.controller.player.GetComponent<SkillsManager>().CriticalDuration.ToString() + " seconds";
                    if (GameController.controller.player.GetComponent<Player>().level < 29)
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Lasts for " + (GameController.controller.player.GetComponent<SkillsManager>().CriticalDuration + GameController.controller.player.GetComponent<SkillsManager>().criticalDurationPerLevel).ToString() + " seconds";
                        if (GameController.controller.player.GetComponent<Player>().level < 4)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                            skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Lasts for " + (GameController.controller.player.GetComponent<SkillsManager>().criticalBaseDuration + GameController.controller.player.GetComponent<SkillsManager>().criticalDurationPerLevel).ToString() + " seconds"; ;
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 4)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 9)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 9)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 14)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 14)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 19)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 19)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 24)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 24)";
                        }
                        else
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 29)";
                        }
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                case "skill4": // Invulnerability
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.mySkillSprites[3];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "Invulnerability";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.mySkillDescriptions[3];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<SkillsManager>().invulnerabilityLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "Lasts for " + GameController.controller.player.GetComponent<SkillsManager>().InvulnerabilityDuration.ToString() + " seconds";
                    if (GameController.controller.player.GetComponent<Player>().level < 30)
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Lasts for " + (GameController.controller.player.GetComponent<SkillsManager>().InvulnerabilityDuration + GameController.controller.player.GetComponent<SkillsManager>().invulnerabilityDurationPerLevel).ToString() + " seconds";
                        if (GameController.controller.player.GetComponent<Player>().level < 5)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Lasts for " + (GameController.controller.player.GetComponent<SkillsManager>().invulnerabilityBaseDuration + GameController.controller.player.GetComponent<SkillsManager>().invulnerabilityDurationPerLevel).ToString() + " seconds";
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 5)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 10)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 10)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 15)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 15)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 20)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 20)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 25)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 25)";
                        }
                        else
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 30)";
                        }
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                case "skill5": // Tantrum Mode
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.mySkillSprites[4];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "Tantrum Mode";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.mySkillDescriptions[4];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<SkillsManager>().tantrumLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "Lasts for " + GameController.controller.player.GetComponent<SkillsManager>().TantrumDuration.ToString() + " seconds";
                    if (GameController.controller.player.GetComponent<Player>().level < 26)
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Lasts for " + (GameController.controller.player.GetComponent<SkillsManager>().TantrumDuration + GameController.controller.player.GetComponent<SkillsManager>().tantrumDurationPerLevel).ToString() + " seconds";
                        if (GameController.controller.player.GetComponent<Player>().level < 6)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Lasts for " + (GameController.controller.player.GetComponent<SkillsManager>().tantrumBaseDuration + GameController.controller.player.GetComponent<SkillsManager>().tantrumDurationPerLevel).ToString() + " seconds";
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 6)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 11)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 11)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 16)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 16)";
                        }
                        else if (GameController.controller.player.GetComponent<Player>().level < 21)
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 21)";
                        }
                        else
                        {
                            skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Unlocks at level 26)";
                        }
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                case "passive1": // Life Steal
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.myPassiveSprites[0];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "LifeSteal";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.myPassiveDescriptions[0];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<PassivesManager>().LifeStealLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "Drains " + GameController.controller.player.GetComponent<PassivesManager>().getLifeStealPercentage().ToString() + "%";
                    if (GameController.controller.player.GetComponent<PassivesManager>().LifeStealLevel<5)
                    {
                        if (GameController.controller.player.GetComponent<PassivesManager>().LifeStealLevel < 1)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                        }
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Drains " + GameController.controller.player.GetComponent<PassivesManager>().lifeStealPercentageSteps[GameController.controller.player.GetComponent<PassivesManager>().LifeStealLevel].ToString() + "%";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Need " + GameController.controller.player.GetComponent<PassivesManager>().skillCost[GameController.controller.player.GetComponent<PassivesManager>().LifeStealLevel] + " to level up!)";
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                case "passive2": // Thorns
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.myPassiveSprites[1];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "Thorns";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.myPassiveDescriptions[1];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<PassivesManager>().ThornsLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "Reflects " + GameController.controller.player.GetComponent<PassivesManager>().getThornsPercentage().ToString() + "%";
                    if (GameController.controller.player.GetComponent<PassivesManager>().ThornsLevel < 5)
                    {
                        if (GameController.controller.player.GetComponent<PassivesManager>().ThornsLevel < 1)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                        }
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Reflects " + GameController.controller.player.GetComponent<PassivesManager>().thornsPercentageSteps[GameController.controller.player.GetComponent<PassivesManager>().ThornsLevel].ToString() + "%";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Need " + GameController.controller.player.GetComponent<PassivesManager>().skillCost[GameController.controller.player.GetComponent<PassivesManager>().ThornsLevel] + " to level up!)";
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                case "passive3": // Poison
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.myPassiveSprites[2];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "Poison";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.myPassiveDescriptions[2];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<PassivesManager>().PoisonLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "Inflict " + GameController.controller.player.GetComponent<PassivesManager>().poisonPercentageSteps.ToString() + "% of enemy's max life in 5 sec";
                    if (GameController.controller.player.GetComponent<PassivesManager>().PoisonLevel < 5)
                    {
                        if (GameController.controller.player.GetComponent<PassivesManager>().PoisonLevel < 1)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                        }
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "Inflict " + GameController.controller.player.GetComponent<PassivesManager>().poisonPercentageSteps[GameController.controller.player.GetComponent<PassivesManager>().PoisonLevel].ToString() + "% of enemy's max life in 5 sec";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Need " + GameController.controller.player.GetComponent<PassivesManager>().skillCost[GameController.controller.player.GetComponent<PassivesManager>().PoisonLevel] + " to level up!)";
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                case "passive4": // Revive
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.myPassiveSprites[3];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "Revive";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.myPassiveDescriptions[3];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<PassivesManager>().ReviveLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = GameController.controller.player.GetComponent<PassivesManager>().getReviveChance().ToString() + "% chance";
                    if (GameController.controller.player.GetComponent<PassivesManager>().ReviveLevel < 5)
                    {
                        if (GameController.controller.player.GetComponent<PassivesManager>().ReviveLevel < 1)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                        }
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = GameController.controller.player.GetComponent<PassivesManager>().reviveChanceSteps[GameController.controller.player.GetComponent<PassivesManager>().ReviveLevel].ToString() + "% chance";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Need " + GameController.controller.player.GetComponent<PassivesManager>().skillCost[GameController.controller.player.GetComponent<PassivesManager>().ReviveLevel] + " to level up!)";
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                case "passive5": // Bargain
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.myPassiveSprites[4];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "Bargain";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.myPassiveDescriptions[4];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<PassivesManager>().BargainLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = GameController.controller.player.GetComponent<PassivesManager>().getBargainPercentage().ToString() + "% discount";
                    if (GameController.controller.player.GetComponent<PassivesManager>().BargainLevel < 5)
                    {
                        if (GameController.controller.player.GetComponent<PassivesManager>().BargainLevel < 1)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                        }
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = GameController.controller.player.GetComponent<PassivesManager>().bargainPercentageSteps[GameController.controller.player.GetComponent<PassivesManager>().BargainLevel].ToString() + "% discount"; ;
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Need " + GameController.controller.player.GetComponent<PassivesManager>().skillCost[GameController.controller.player.GetComponent<PassivesManager>().BargainLevel] + " to level up!)";
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                case "passive6": // Looting
                    skillTooltip.transform.GetChild(1).GetComponent<Image>().sprite = DataBase.dataBase.myPassiveSprites[5];
                    skillTooltip.transform.GetChild(2).GetComponent<Text>().text = "Looting";
                    skillTooltip.transform.GetChild(3).GetComponent<Text>().text = DataBase.dataBase.myPassiveDescriptions[5];
                    skillTooltip.transform.GetChild(4).GetComponent<Text>().text = "Level " + GameController.controller.player.GetComponent<PassivesManager>().LootingLevel.ToString();
                    skillTooltip.transform.GetChild(6).GetComponent<Text>().text = GameController.controller.player.GetComponent<PassivesManager>().getBargainPercentage().ToString() + "% chance";
                    if (GameController.controller.player.GetComponent<PassivesManager>().LootingLevel < 5)
                    {
                        if (GameController.controller.player.GetComponent<PassivesManager>().LootingLevel < 1)
                        {
                            skillTooltip.transform.GetChild(6).GetComponent<Text>().text = "";
                        }
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Next Level";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = GameController.controller.player.GetComponent<PassivesManager>().lootingChanceSteps[GameController.controller.player.GetComponent<PassivesManager>().LootingLevel].ToString() + "% chance"; ;
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "(Need " + GameController.controller.player.GetComponent<PassivesManager>().skillCost[GameController.controller.player.GetComponent<PassivesManager>().LootingLevel] + " to level up!)";
                    }
                    else
                    {
                        skillTooltip.transform.GetChild(5).GetComponent<Text>().text = "Max Level Reached!";
                        skillTooltip.transform.GetChild(7).GetComponent<Text>().text = "";
                        skillTooltip.transform.GetChild(8).GetComponent<Text>().text = "";
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            skillTooltip.SetActive(false);
        }
    }

    public void RefreshXP(float value)
    {
        xpSlider.value = value;
    }

    public void RefreshInventory()
    {
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            if (inventory.transform.GetChild(i).GetComponent<Slot>().item == null || inventory.transform.GetChild(i).GetComponent<Slot>().item is Equipable)
            {
                inventory.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }

        }
        goldtext.text = inventory.GetComponent<Inventory>().goldAmount.ToString();
        playerleveltext.text = " Level " + GameController.controller.player.GetComponent<Player>().level.ToString();
    }

    /// <summary>
    /// Refresh shop using shop button
    /// </summary>
    public void RefreshShop()
    {
        if (shopMenu.CompareTag("Blacksmith"))
        {
            if (inventory.GetComponent<Inventory>().goldAmount > refreshcostblack)
            {
                refreshcontblack++;
                inventory.GetComponent<Inventory>().goldAmount -= refreshcostblack;
                //refreshcostblack += (int)(2 + Mathf.Pow(10, refreshcontblack / 2));
                refreshcostblack += 10 + refreshcontblack / 2;
                RefreshUI();
                SyncShopInventory();
                shopMenu.GetComponent<Shop>().GenItens();
                for (int i = 0; i < shopMenu.GetComponent<Shop>().shopPanel.transform.childCount; i++)
                {
                    shopMenu.GetComponent<Shop>().shopPanel.transform.GetChild(i).GetComponent<ShopSlot>().RefreshShopSlot();
                }
            }
            else
            {
                Debug.Log("Not enough money to Refresh");
            }
        }
        else
        {
            if (inventory.GetComponent<Inventory>().goldAmount > refreshcostshop)
            {
                refreshcontshop++;
                inventory.GetComponent<Inventory>().goldAmount -= refreshcostshop;
                //refreshcostshop += (int)(2 + Mathf.Pow(10, refreshcontshop / 2));
                refreshcostshop += 5 + refreshcontshop / 2;
                RefreshUI();
                SyncShopInventory();
                shopMenu.GetComponent<Shop>().GenMaterials();
                for (int i = 0; i < shopMenu.GetComponent<Shop>().shopPanel.transform.childCount; i++)
                {
                    shopMenu.GetComponent<Shop>().shopPanel.transform.GetChild(i).GetComponent<ShopSlot>().RefreshShopSlot();
                }
            }
            else
            {
                Debug.Log("Not enough money to Refresh");
            }
        }

    }

    public void SyncShopInventory()
    {
        //Sync shop gold text copy
        goldtextCopy.text = goldtext.text;
        goldtextCopyCraft.text = goldtext.text;

        //Sync shop refresh cost text
        if (shopMenu.gameObject.tag == "Blacksmith")
        {
            refreshCostText.text = refreshcostblack.ToString();
        }
        else
        {
            refreshCostText.text = refreshcostshop.ToString();
        }
        //Sync shop inventory slots copies
        for (int i = 0; i < inventoryCopy.transform.childCount; i++)
        {
            inventoryCopy.transform.GetChild(i).GetComponent<Image>().sprite = inventory.transform.GetChild(i).GetComponent<Image>().sprite;
            inventoryCopy.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = inventory.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite;
            inventoryCopy.transform.GetChild(i).GetComponent<Image>().color = inventory.transform.GetChild(i).GetComponent<Image>().color; // copia a cor do slot do inventario
            inventoryCopy.transform.GetChild(i).GetChild(1).GetComponent<Image>().color = inventory.transform.GetChild(i).GetChild(1).GetComponent<Image>().color;

            if (inventory.transform.GetChild(i).GetChild(0).gameObject.activeSelf) // se o item amount estiver ativo nesse slot
            {
                inventoryCopy.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = inventory.transform.GetChild(i).GetChild(0).GetComponent<Text>().text; //seta o valor da copia
                inventoryCopy.transform.GetChild(i).GetChild(0).gameObject.SetActive(true); // mostra a quantidade na copia
            }
            else
            {
                inventoryCopy.transform.GetChild(i).GetChild(0).gameObject.SetActive(false); // não mostra quantidade na copia
            }
        }
    }

    public bool CheckPanels()
    {
        if (optionsPanel.activeSelf || questlogPanel.activeSelf || inventoryPanel.activeSelf ||
            skillsPanel.activeSelf || statsPanel.activeSelf || mapPanel.activeSelf)
        {
            return true;
        }
        else if (questMenu != null && questMenu.gameObject.activeSelf)
        {
            return true;
        }
        else if (shopMenu != null && shopMenu.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckDialogs()
    {
        if (dialogtut.activeSelf || dialogshop.activeSelf || dialogblack.activeSelf || dialogbox.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowDeathPanel()
    {
        deathPanel.SetActive(true);
        UseMouse(true);

    }

    public void ChangeScene(int index)
    {
        if (index == 0)
        {
            //Destroy(GameController.controller.gameObject);
        }
        fade.StartFading(index);
    }

    public void CheckHotkeys()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!skiptut.activeSelf)
            {
                if (CheckPanels() || CheckDialogs())
                {
                    if (tutmush != null)
                    {
                        tutmush.GetComponent<TutorialMush>().StopAllCoroutines();
                    }

                    CloseOptions();
                    CloseAll();
                    if (questMenu != null)
                    {
                        questMenu.gameObject.SetActive(false);
                    }
                    if (shopMenu != null)
                    {
                        shopMenu.gameObject.SetActive(false);
                        inventory.GetComponent<Inventory>().shopsellingpricetext.text = "";
                        inventory.GetComponent<Inventory>().blacksellingpricetext.text = "";
                    }
                    UseMouse(false);
                }
                else
                {
                    optionsPanel.SetActive(true);
                    Time.timeScale = 0;
                    UseMouse(true);
                }
            }
        }

        if (Input.GetKeyDown("e"))
        {
            if (!optionsPanel.activeSelf)
            {
                if (!CheckDialogs())
                {
                    ShowAction("talk", false);
                    if (questMenu != null)
                    {
                        if (!questMenu.gameObject.activeSelf)
                        {
                            CloseAll();
                            questMenu.gameObject.SetActive(true);
                            questMenu.initial = true;
                            UseMouse(true);
                        }
                        else
                        {
                            ShowAction("talk", true);
                            questMenu.gameObject.SetActive(false);
                            UseMouse(false);
                        }
                    }
                    else if (shopMenu != null)
                    {
                        if (shopNPC.unlockquestdone) //já fez a quest do shop
                        {
                            if (!shopMenu.activeSelf)
                            {
                                CloseAll();
                                SyncShopInventory();
                                shopMenu.SetActive(true);
                                UseMouse(true);
                                ClearAllShopInfo();
                            }
                            else
                            {
                                ShowAction("talk", true);
                                shopMenu.gameObject.SetActive(false);
                                equipableTooltip.SetActive(false);
                                craftmatTooltip.SetActive(false);
                                skillTooltip.SetActive(false);
                                UseMouse(false);
                            }
                        }
                        else //não fez a quest do shop
                        {
                            UseMouse(true);
                            shopNPC.WriteQuest();
                            if (shopNPC.CompareTag("Blacksmith"))
                            {
                                dialogblack.SetActive(true);
                            }
                            else
                            {
                                dialogshop.SetActive(true);
                            }
                        }
                    }
                    else if (machine != null)
                    {
                        machine.CreateMaterials();
                        //machine.CreateHoneyCardboard();
                    }
                }
            }
        }

        if (Input.GetKeyDown("q") || Input.GetKeyDown("j"))
        {
            if (!optionsPanel.activeSelf && !CheckDialogs() && !skiptut.activeSelf)
            {
                if (!CheckPanels())
                {
                    questlogPanel.SetActive(true);
                    UseMouse(true);
                }
                else if (questlogPanel.activeSelf)
                {
                    questlogPanel.SetActive(false);
                    ResetQuestlog();
                    UseMouse(false);
                }
                else
                {
                    CloseAll();
                    questlogPanel.SetActive(true);
                    UseMouse(true);
                }
            }
        }


        if (Input.GetKeyDown("i") || Input.GetKeyDown(KeyCode.Tab))
        {
            if (!optionsPanel.activeSelf && !CheckDialogs() && !skiptut.activeSelf)
            {
                if (!CheckPanels())
                {
                    inventoryPanel.SetActive(true);
                    UseMouse(true);
                }
                else if (inventoryPanel.activeSelf)
                {
                    CleanGlow();
                    inventoryPanel.SetActive(false);
                    confirmdelete.SetActive(false);
                    equipableTooltip.SetActive(false);
                    craftmatTooltip.SetActive(false);
                    skillTooltip.SetActive(false);
                    UseMouse(false);
                }
                else
                {
                    CloseAll();
                    inventoryPanel.SetActive(true);
                    UseMouse(true);
                }
            }
        }

        if (Input.GetKeyDown("k"))
        {
            if (!optionsPanel.activeSelf && !CheckDialogs() && !skiptut.activeSelf)
            {
                if (!CheckPanels())
                {
                    skillsPanel.SetActive(true);
                    UseMouse(true);
                }
                else if (skillsPanel.activeSelf)
                {
                    skillsPanel.SetActive(false);
                    equipableTooltip.SetActive(false);
                    craftmatTooltip.SetActive(false);
                    skillTooltip.SetActive(false);
                    UseMouse(false);
                }
                else
                {
                    CloseAll();
                    skillsPanel.SetActive(true);
                    UseMouse(true);
                }
            }
        }

        if (Input.GetKeyDown("p"))
        {
            if (!optionsPanel.activeSelf && !CheckDialogs() && !skiptut.activeSelf)
            {
                if (!CheckPanels())
                {
                    statsPanel.SetActive(true);
                    levelUPButton.SetActive(false);
                    UseMouse(true);
                }
                else if (statsPanel.activeSelf)
                {
                    statsPanel.SetActive(false);
                    UseMouse(false);
                }
                else
                {
                    CloseAll();
                    statsPanel.SetActive(true);
                    UseMouse(true);
                }
            }
        }

        if (Input.GetKeyDown("m"))
        {
            if (!optionsPanel.activeSelf && !CheckDialogs() && !skiptut.activeSelf)
            {
                if (!CheckPanels())
                {
                    Time.timeScale = 0;
                    mapPanel.SetActive(true);
                    UseMouse(true);
                }
                else if (mapPanel.activeSelf)
                {
                    CloseAll();
                    UseMouse(false);
                }
                else
                {
                    Time.timeScale = 0;
                    CloseAll();
                    mapPanel.SetActive(true);
                    UseMouse(true);
                }
            }
        }
    }

    //Reseta as info de itens selecionados de todas as shops

    public void ClearAllShopInfo()
    {
        //Reset Craft Info
        craftgrid.ClearMaterialMirrors();
        craftgrid.SelectedRecipeSlot = null;
        blackshoppanel.SetActive(true);
        blackcraftpanel.SetActive(false);

        //Shopinfo
        inventory.GetComponent<Inventory>().selectedSlot = null;
        if (shopNPC != null)
        {
            if (shopNPC.CompareTag("Blacksmith"))
            {
                blackshopgrid.GetComponent<ShopSlotGrid>().ResetSelected();
                blackcopygrid.GetComponent<InvCopyGrid>().ResetSelected();
            }
            else
            {
                shopshopgrid.GetComponent<ShopSlotGrid>().ResetSelected();
                shopcopygrid.GetComponent<InvCopyGrid>().ResetSelected();
            }
        }

    }

    //Reseta o questlog para Descrição nula e esconde o botão de cancelar quest
    public void ResetQuestlog()
    {
        questlogPanel.GetComponent<QuestLog>().description.text = "";
        questlogPanel.GetComponent<QuestLog>().buttonCancel.SetActive(false);
    }

    public void CloseOptions()
    {

        Time.timeScale = 1;
        optionsPanel.SetActive(false);
        hotkeysimg.SetActive(false);
        audiopanel.SetActive(true);
        videopanel.SetActive(false);
        UseMouse(false);
    }

    private void CloseDialogtut()
    {
        if (tutmush != null)
        {
            GameController.controller.player.GetComponent<Player>().isDead = false;
            dialogtut.SetActive(false);
            tutmush.GetComponent<BoxCollider>().enabled = true;
        }

    }

    public void CloseAll()
    {
        if (dragobj.isdragging == true)
        {
            dragobj.originslot.GetComponent<Image>().color = new Color(1, 1, 1);
            dragobj.StopDrag();
        }
        ShowAction("talk", false);
        ShowAction("pickup", false);
        CleanGlow();
        Time.timeScale = 1;
        dialogblack.SetActive(false);
        dialogshop.SetActive(false);
        dialogbox.SetActive(false);
        confirmdelete.SetActive(false);
        confirmquit.SetActive(false);
        CloseDialogtut();
        questlogPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        equipbuttom.SetActive(false);
        skillsPanel.SetActive(false);
        statsPanel.SetActive(false);
        mapPanel.SetActive(false);
        ResetQuestlog();
        equipableTooltip.SetActive(false);
        craftmatTooltip.SetActive(false);
        skillTooltip.SetActive(false);
        UseMouse(false);

        if (questMenu != null)
        {
            questMenu.gameObject.SetActive(false);
        }
        if (shopMenu != null)
        {
            shopMenu.SetActive(false);
            inventory.GetComponent<Inventory>().shopsellingpricetext.text = "";
            inventory.GetComponent<Inventory>().blacksellingpricetext.text = "";
        }
    }

    public GameObject GetQuestLog()
    {
        return questlogPanel;
    }

    public IEnumerator ShowPotionTutorial()
    {
        potionTutorial.SetActive(true);
        yield return new WaitForSeconds(3);
        potionTutorial.SetActive(false);
    }

    public void CompleteQuestAnimationCall(string questtitle)
    {
        StartCoroutine(CompleteQuestAnimation(questtitle));
    }

    public IEnumerator CompleteQuestAnimation(string questtitle)
    {
        completedQuestAnim.SetActive(true);
        completedQuestAnim.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = questtitle;
        yield return new WaitForSeconds(4f);
        completedQuestAnim.SetActive(false);
    }

    public void ItemPickedFeedback(Item item)
    {
        StartCoroutine(ItemPicked(item));
    }
    public void Item2PickedFeedback(Item item)
    {
        StartCoroutine(Item2Picked(item));
    }

    IEnumerator ItemPicked(Item item)
    {
        itempicked.SetActive(true);
        itempicked.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.itemName;
        itempicked.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = item.itemIcon;

        yield return new WaitForSeconds(2.5f);
        itempicked.SetActive(false);
    }
    IEnumerator Item2Picked(Item item)
    {
        item2picked.SetActive(true);
        item2picked.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.itemName;
        item2picked.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = item.itemIcon;
        yield return new WaitForSeconds(2.5f);
        item2picked.SetActive(false);
    }
}
