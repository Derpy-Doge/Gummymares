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
}
