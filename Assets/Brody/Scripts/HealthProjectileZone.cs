using UnityEngine;
using UnityEngine.InputSystem;

public class HealthProjectileZone : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject HealthUp;


    void Start()
    {
        InvokeRepeating("SpawnProjectile", 10, 10);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnProjectile()
    {
        int index = Random.Range(0, 10);
        GameObject newHealthUp = Instantiate(HealthUp, spawnPoints[index].position, Quaternion.identity);
        newHealthUp.GetComponent<Rigidbody2D>().linearVelocity = (FindAnyObjectByType<PlayerInput>().transform.position - newHealthUp.transform.position).normalized * 10;
        Destroy(newHealthUp, 3);
    }
    
}

