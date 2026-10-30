using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public float Health_Max;
    public Image healthBar;
    [HideInInspector]public float Health_Current;
    [Space(7f)]

    public float Attack_Power;
    [Space(4f)]
    public float Defense;
    [Space(4f)]
    public float Speed;
    [Space(4f)]
    public float Stealth = 1;

    [HideInInspector] public float Passive_Attack; // for enemies only, damage you take when you run into them

    public TextMeshProUGUI attackText;




    private bool isPlayer;
    public static bool isDead = false;

    void Awake()
    {

    }

    void Start()
    {
        Health_Current = Health_Max;

        if(GetComponent<PlayerMovement>() != null)
        {
            isPlayer = true;
        }
        else
        {
            isPlayer = false;
        }


        if (isPlayer)
        {
           PlayerMovement plyr = GetComponent<PlayerMovement>();

            Speed = plyr.moveSpeed;
        }
    }

    void Update()
    {
        if (isPlayer && attackText != null)
        {
            attackText.text = "atk: " + Attack_Power.ToString();
        }

        bool expAwarded = false;

        if (!isPlayer)
        {
            if (GetComponent<enemy>() != null && Bear5.spawned)
            {
                Destroy(gameObject);
            }
        }


        if (Health_Current <= 0 && !isPlayer && !expAwarded)
        {       
            if(GetComponent<enemy>() != null)
            {
                int Xp = GetComponent<enemy>().xp;
                Leveling_System.instance.AddXp(Xp);

                time_manager.instance.bearsKilled++;
                time_manager.instance.totalBearsKilled++;

            }

            expAwarded = true;
            Destroy(gameObject);
        }
        if (Health_Current <= 0 && isPlayer && !isDead)
        {
            isDead = true;

            CameraController camcontroller = FindAnyObjectByType<CameraController>();
            camcontroller.isLocked = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene("Kaitlyn White");
        }

        if (isPlayer)
        {
            if(healthBar != null)
                healthBar.fillAmount = (Health_Current / Health_Max);
        }


        if (Health_Current > Health_Max)
        {
            Health_Current = Health_Max;
        }

        Passive_Attack = Attack_Power * 0.5f;
    }
}
