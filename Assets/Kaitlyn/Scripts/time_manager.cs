using TMPro;
using UnityEngine;

public class time_manager : MonoBehaviour
{
    public bool isDay;
    public int bearsKilled;
    public int totalBearsKilled;
    [SerializeField] private int totalBearThreshold = 50;
    public int threshold;

    private int interval;

    public GameObject dayUI;
    public GameObject shopButtons;
    public GameObject nightUI;
    public GameObject levelText;

    public Transform playerSpawnPoint;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI skillText;

    private CameraController cameraController;

    

    public static time_manager instance;


    [SerializeField]private GameObject devMenu;
    [SerializeField] private GameObject devMenuButtons;

    private void Awake()
    {

    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;

            isDay = false;

        }

        devMenu.SetActive(false);

        cameraController = GameObject.FindAnyObjectByType<CameraController>();

        threshold = 8;

        GameObject player = GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;

        player.transform.position = playerSpawnPoint.position;
    }


    void Update()
    {
        if (isDay)
        {
            cameraController.isLocked = false;

            dayUI.SetActive(true);
            shopButtons.SetActive(true);
            skillText.text = "Skill Points: " + Leveling_System.instance.skill;
            nightUI.SetActive(false);
            levelText.SetActive(false);
        }
        else
        {
            cameraController.isLocked = true;

            nightUI.SetActive(true);
            levelText.SetActive(true);

            dayUI.SetActive(false);
            shopButtons.SetActive(false);

            FakeTimer(bearsKilled);
        }

        if (bearsKilled == threshold && !isDay)
        {
            isDay = true;

            GameObject player = GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;

            player.transform.position = playerSpawnPoint.position;
            player.GetComponent<Stats>().Health_Current = player.GetComponent<Stats>().Health_Max;

            bearsKilled = 0;
            float newThreshold = threshold * 1.5f;
            threshold = (int)newThreshold;
        }

        if (totalBearsKilled == totalBearThreshold && !isDay)
        {
            Bear5.spawned = true;

            Bear5 bear = FindAnyObjectByType<Bear5>();
            bear.StartCoroutine(bear.Stun(7f));
        }
    }

    private void FakeTimer(int killCount)
    {

        if(killCount <= 2)
        {
            interval = 0;
        }
        else
        {
            interval = (killCount / 2) - 1;
        }
        
        int intervals = threshold / 2;

        int timeInterval = 360 / intervals;

        float time = (timeInterval*interval) / 60f;

        int hours = Mathf.FloorToInt(time);

        float minutes = 60 * (time - Mathf.FloorToInt(time));
        int minutesInt = (int)minutes;

        timerText.text = hours.ToString("00") + ":" + minutesInt.ToString("00");
    }


    #region Dev Tools
    public void DevTools()
    {
        if (devMenu.activeSelf)
        {
            devMenu.SetActive(false);
            cameraController.isLocked = true;
        }
        else
        {
            devMenu.SetActive(true);
            cameraController.isLocked = false;
        }
    }

    public void IsDayBool()
    {
        if (isDay)
        {
            isDay = false;
        }
        else
        {
            isDay = true;
        }
    }

    public void XPCheat()
    {
        Leveling_System.instance.AddXp(5000);
    }

    public void SkillPointCheat()
    {
        Leveling_System.instance.skill += (20);
    }

    public void SpawnBear5()
    {
        Bear5.spawned = true;
    }

    public void StatBoost()
    {
        Stats plyr = GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<Stats>();

        plyr.Attack_Power += 100;
        plyr.Defense += 100;
        plyr.Speed += 3;
        plyr.Health_Max += 1000;
        plyr.Health_Current = plyr.Health_Max;
        plyr.Stealth += 100;
    }

    #endregion
}
