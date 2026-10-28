using UnityEngine;
using UnityEngine.AI;

public class ERR_HEADMOVE : MonoBehaviour
{
    public GameObject Target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().SetDestination(Target.transform.position);
    }
}
