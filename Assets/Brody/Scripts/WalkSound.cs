using UnityEngine;

public class WalkSound : MonoBehaviour
{
    public AudioSource footStepsSound;
    public int jigglephysics;

    void Start()
    {
        jigglephysics = 100;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            footStepsSound.enabled = true; 
        }
        else
        {
            footStepsSound.enabled = false; 
        }
    }
}
