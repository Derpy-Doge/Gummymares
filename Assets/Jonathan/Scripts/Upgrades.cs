using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    [Tooltip("Buttons in order: Attack, Defense, Health, Speed, Stealth")]
    public Button[] upgradeButtons;

    private Stats pStats;
    private Leveling_System ls;

    private int attackUpgradeCount = 0;
    public int attackUpgradeCost = 0;

    private int defenseUpgradeCount = 0;
    public int defenseUpgradeCost = 0;

    private int healthUpgradeCount = 0;
    public int healthUpgradeCost = 0;

    private int speedUpgradeCount = 0;
    public int speedUpgradeCost = 0;

    private int stealthUpgradeCount = 0;
    public int stealthUpgradeCost = 0;

    public TextMeshProUGUI skillText; // for display in shop menu


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pStats = GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<Stats>();
        ls = GetComponent<Leveling_System>();

        attackUpgradeCount = 0;
        defenseUpgradeCount = 0;
        healthUpgradeCount = 0;
        speedUpgradeCount = 0;
        stealthUpgradeCount = 0;

        attackUpgradeCost = 1;
        defenseUpgradeCost = 1;
        healthUpgradeCost = 1;
        speedUpgradeCost = 1;
        stealthUpgradeCost = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackUpgrade() //0
    {
        if (time_manager.instance.isDay)
        {
            if(skillText != null)
            {
                skillText.gameObject.SetActive(true);
                skillText.text = "Skill Points: " + Leveling_System.instance.skill;
            }
        }

        if (ls.SpendSkill(attackUpgradeCost))
        {
            if (attackUpgradeCount <= 2)
            {
                pStats.Attack_Power += 10;
                attackUpgradeCount++;
                attackUpgradeCost++;

                if (attackUpgradeCount == 3)
                {
                    upgradeButtons[0].interactable = false;
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }

       
        
    }

    public void DefenseUpgrade() //1
    {
        if (ls.SpendSkill(defenseUpgradeCost))
        {
            if (defenseUpgradeCount <= 2)
            {
               
                pStats.Defense += 2;
                defenseUpgradeCount++;
                defenseUpgradeCost++;

                if (defenseUpgradeCount == 3)
                {
                    upgradeButtons[1].interactable = false;
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }



    }

    public void HealthUpgrade() //2
    {
        if (ls.SpendSkill(healthUpgradeCost))
        {
            if (healthUpgradeCount <= 2)
            {

                pStats.Health_Max += 20;
                healthUpgradeCount++;
                healthUpgradeCost++;

                if (healthUpgradeCount == 3)
                {
                    upgradeButtons[2].interactable = false;
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }



    }

    public void SpeedUpgrade() //3
    {
        if (ls.SpendSkill(speedUpgradeCost))
        {
            if (speedUpgradeCount <= 2)
            {

                pStats.Speed += +.5f;
                speedUpgradeCount++;
                speedUpgradeCost++;

                if (speedUpgradeCount == 3)
                {
                    upgradeButtons[3].interactable = false;
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }



    }

    public void StealthUpgrade() //4
    {
        if (ls.SpendSkill(stealthUpgradeCost))
        {
            if (stealthUpgradeCount <= 2)
            {

                pStats.Stealth += .25f;
                stealthUpgradeCount++;
                stealthUpgradeCost++;

                if (stealthUpgradeCount == 3)
                {
                    upgradeButtons[4].interactable = false;
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    public void ContinueButton()
    {
        time_manager.instance.isDay = false;
    }
}
