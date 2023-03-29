using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivesManager : MonoBehaviour
{

    /* Passives
     * 1 - Life Steal
     * 2 - "Thorns" (Return a % of damage)
     * 3 - Poison
     * 4 - Revive
     * 5 - Bargain (Affect prices)
     * 6 - Looting (Change to get more loot)
     */

    //Player
    private Player player;

    //LifeSteal
    private int lifeStealPercentage = 0;
    public int[] lifeStealPercentageSteps = new int[5];
    private bool lifeStealUnlocked = false;

    //Thornmail
    private int thornsPercentage = 0;
    public int[] thornsPercentageSteps = new int[5];
    private bool thornsUnlocked = false;

    //Poison
    private int poisonPercentage;
    public int[] poisonPercentageSteps = new int[5];
    public int poisonLimit;
    public int poisonTime;
    private bool poisonUnlocked = false;

    //Revive
    private int reviveChance, reviveHpPercentage;
    public int[] reviveChanceSteps = new int[5];
    public int[] reviveHpPercentageSteps = new int[5];
    private bool reviveUnlocked = false;

    //Bargain
    private int bargainPercentage;
    public int[] bargainPercentageSteps = new int[5];
    private bool bargainUnlocked = false;

    //DoubleLoot
    private int lootingChance;
    public int[] lootingChanceSteps = new int[5];
    private bool lootingUnlocked = false;

    //Others
    public int[] skillCost = new int[] { 1, 2, 2, 3, 3, 3 };

    //Getters

    //LifeSteal
    public int LifeStealLevel { get; private set; } = 0;

    public int getLifeStealPercentage()
    {
        return lifeStealPercentage;
    }

    public bool getLifeStealUnlocked()
    {
        return lifeStealUnlocked;
    }

    //Thornmail
    public int ThornsLevel { get; private set; } = 0;

    public int getThornsPercentage()
    {
        return thornsPercentage;
    }

    public bool getThornsUnlocked()
    {
        return thornsUnlocked;
    }

    //Poison
    public int PoisonLevel { get; private set; } = 0;

    public int getPoisonPercentage()
    {
        return poisonPercentage;
    }

    public int getPoisonLimit()
    {
        return poisonLimit;
    }

    public int getPoisonTime()
    {
        return poisonTime;
    }

    public bool getPoisonUnlocked()
    {
        return poisonUnlocked;
    }

    //Revive
    public int ReviveLevel { get; private set; } = 0;

    public int getReviveChance()
    {
        return reviveChance;
    }

    public int getReviveHpPercentage()
    {
        return reviveHpPercentage;
    }

    public bool getReviveUnlocked()
    {
        return reviveUnlocked;
    }

    //Bargain
    public int BargainLevel { get; private set; } = 0;

    public int getBargainPercentage()
    {
        return bargainPercentage;
    }

    public bool getBargainUnlocked()
    {
        return bargainUnlocked;
    }

    //Looting
    public int LootingLevel { get; private set; } = 0;

    public int getLootingChance()
    {
        return lootingChance;
    }

    public bool getLootingUnlocked()
    {
        return lootingUnlocked;
    }

    void Start()
    {
        player = GetComponent<Player>();

        //Comentei tudo porque no Start não há pontos de skill para que esses métodos sejam chamados
        //LifeStealLevelUp();
        //ThornsLevelUp();
        //PoisonLevelUp();
        //ReviveLevelUp();
        //BargainLevelUp();
    }

    //LevelUp

    public void LifeStealLevelUp()
    {
        if (LifeStealLevel < lifeStealPercentageSteps.Length && player.skillPoints >= skillCost[LifeStealLevel])
        {
            player.skillPoints -= skillCost[LifeStealLevel];
            LifeStealLevel++;
            lifeStealPercentage = lifeStealPercentageSteps[LifeStealLevel - 1];
            lifeStealUnlocked = true;
        }
        else
        {
            Debug.Log("Não há pontos suficientes ou a habilidade já está no level máximo!");
        }
    }

    public void ThornsLevelUp()
    {
        if (ThornsLevel < thornsPercentageSteps.Length && player.skillPoints >= skillCost[ThornsLevel])
        {
            player.skillPoints -= skillCost[ThornsLevel];
            ThornsLevel++;
            thornsPercentage = thornsPercentageSteps[ThornsLevel - 1];
            thornsUnlocked = true;

        }
        else
        {
            Debug.Log("Não há pontos suficientes ou a habilidade já está no level máximo!");
        }
    }

    public void PoisonLevelUp()
    {
        if (PoisonLevel < poisonPercentageSteps.Length && player.skillPoints >= skillCost[PoisonLevel])
        {
            player.skillPoints -= skillCost[PoisonLevel];
            PoisonLevel++;
            poisonPercentage = poisonPercentageSteps[PoisonLevel - 1];
            poisonUnlocked = true;
        }
        else
        {
            Debug.Log("Não há pontos suficientes ou a habilidade já está no level máximo!");
        }
    }

    public void ReviveLevelUp()
    {
        if (ReviveLevel < reviveChanceSteps.Length && player.skillPoints >= skillCost[ReviveLevel])
        {
            player.skillPoints -= skillCost[ReviveLevel];
            ReviveLevel++;
            reviveChance = reviveChanceSteps[ReviveLevel - 1];
            reviveHpPercentage = reviveHpPercentageSteps[ReviveLevel - 1];
            reviveUnlocked = true;
        }
        else
        {
            Debug.Log("Não há pontos suficientes ou a habilidade já está no level máximo!");
        }
    }

    public void BargainLevelUp()
    {
        if (BargainLevel < bargainPercentageSteps.Length && player.skillPoints >= skillCost[BargainLevel])
        {
            player.skillPoints -= skillCost[BargainLevel];
            BargainLevel++;
            bargainPercentage = bargainPercentageSteps[BargainLevel - 1];
            bargainUnlocked = true;
        }
        else
        {
            Debug.Log("Não há pontos suficientes ou a habilidade já está no level máximo!");
        }
    }

    public void LootingLevelUp()
    {
        if (LootingLevel < lootingChanceSteps.Length && player.skillPoints >= skillCost[LootingLevel])
        {
            player.skillPoints -= skillCost[LootingLevel];
            LootingLevel++;
            lootingChance = lootingChanceSteps[LootingLevel - 1];
            lootingUnlocked = true;
        }
        else
        {
            Debug.Log("Não há pontos suficientes ou a habilidade já está no level máximo!");
        }
    }


}
