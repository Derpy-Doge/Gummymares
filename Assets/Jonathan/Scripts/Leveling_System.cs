using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leveling_System : MonoBehaviour
{
    public int level;

    public int skill;
    
    public float currentXp;

    [SerializeField] private float targetXp;

    public TextMeshProUGUI levelText;

    public TextMeshProUGUI skillText;

    [SerializeField]private Image xpProgressbar;

    [SerializeField]private GameObject xpBar;

    public static Leveling_System instance;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        XpController();
    }

    public void XpController()
    {
       if (levelText == null)
        {
            return;
        }

        levelText.text = "Level: " + level.ToString();

        skillText.text = "Skill Points: " + skill.ToString();
        xpProgressbar.fillAmount = (currentXp / targetXp);

        if (currentXp >= targetXp)
        {
            currentXp = currentXp - targetXp;

            level++;

            skill++;

            targetXp *= (float)1.3f;
        }
    }

    public bool SpendSkill(int cost)
    {
        if (skill <= 0)
        { 
            return false;
        }

        if (skill >= cost)
        {
            skill -= cost;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddXp(int xp)
    {
        currentXp += xp;
    }



}
