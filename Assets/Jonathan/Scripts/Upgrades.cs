using System;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    private Stats pStats;
    private Leveling_System ls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pStats = GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<Stats>();
        ls = GetComponent<Leveling_System>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackUpgrade()
    {
        int upgradecount = 0;

        if (upgradecount <= 3)
        {
            pStats.Attack_Power += 10;
            upgradecount++;
        }
        else
        {
            return;
        }
        
    }

}
