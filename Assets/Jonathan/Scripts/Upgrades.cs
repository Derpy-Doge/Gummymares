using System;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public Button[] upgradeButtons;

    private Stats pStats;
    private Leveling_System ls;

    private int attackUpgradeCount = 0;
    private int attackUpgradeCost = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pStats = GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<Stats>();
        ls = GetComponent<Leveling_System>();

        attackUpgradeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackUpgrade() //0
    {
        if (ls.SpendSkill(attackUpgradeCost))
        {

        }
        else
        {
            return;
        }
        

        if (attackUpgradeCount <= 2)
        {
            pStats.Attack_Power += 10;
            attackUpgradeCount++;

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

}
