using UnityEngine;

public class time_manager : MonoBehaviour
{
    public bool isDay;
    public int bearsKilled;
    public int threshold;

    private CameraController cameraController;

    void Start()
    {
        cameraController = GameObject.FindAnyObjectByType<CameraController>();
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
        }
    }
}
