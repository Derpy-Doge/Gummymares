using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; 

public class ProjectileZone : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject projectile;
    public TMPro.TMP_Text uI;

    void Start()
    {
        InvokeRepeating("SpawnProjectile", 3, 2);
    }

    public void SpawnProjectile()
    {
        int index = Random.Range(0, 10);
        GameObject newProjectile = Instantiate(projectile, spawnPoints[index].position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody2D>().linearVelocity = (FindAnyObjectByType<PlayerInput>().transform.position - newProjectile.transform.position).normalized * 10;
        
        Destroy(newProjectile, 4);
    }
}
