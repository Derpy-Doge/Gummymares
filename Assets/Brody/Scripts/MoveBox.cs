using UnityEngine;

public class MoveBox : MonoBehaviour
{
    public Vector3 endPos;
    public float endTime;
    private Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(startPos, endPos, Time.timeSinceLevelLoad / endTime);
    }

}
