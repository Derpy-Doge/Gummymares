using TMPro;
using UnityEngine;

public class time_manager : MonoBehaviour
{
    public bool isDay;
    public int bearsKilled;
    public int threshold;

    private int interval;

    public GameObject shopObject;

    public Transform playerSpawnPoint;

    public TextMeshProUGUI timerText;

    private CameraController cameraController;

    

    public static time_manager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;

        }

        cameraController = GameObject.FindAnyObjectByType<CameraController>();

        threshold = 8;
    }

    
    void Update()
    {
        if (isDay)
        {
            cameraController.isLocked = false;

            
        }
        else
        {
            cameraController.isLocked = true;

            FakeTimer(bearsKilled);
        }

        if (bearsKilled == threshold && !isDay)
        {
            isDay = true;

            bearsKilled = 0;
            float newThreshold = threshold * 1.5f;
            threshold = (int)newThreshold;
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

        timerText.text = hours.ToString() + ":" + minutesInt.ToString();
    }
}
