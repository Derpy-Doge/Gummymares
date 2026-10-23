using System.Collections;
using System.Threading;
using UnityEngine;

public class Bear5 : MonoBehaviour
{
    public float meleeRange = 1.5f;
    public LayerMask targetMask;

    [Space(5f)]
    [Header("Punch")]
    public float pRadius = 1f;
    public float pForce = 1f;
    public float pDamage = 35;
    [Space(3f)]
    private bool canPunch = true;
    private bool isPunching = false;
    public float pCooldown = 5f;

    [Space(5f)]
    [Header("Shockwave")]
    public float swMaxRadius = 5f;
    private float swCurrentRadius = 0f;
    public float swInnerRadius = 1f;
    public float swOuterRadius = 2.5f;
    public float expansionSpeed = 7f;
    public float swForce = 7f;
    public float swDamage = 25;
    public int crushDamage = 60; // when bear5 lands on the player during shockwave
    [Space(3f)]
    private bool isExpanding = false;
    private bool canSW = true;
    private bool isSW = false;
    public float swCooldown = 20f;

    [Space(5f)]
    public int xp = 1000;

    public static bool defeated = false;


    void Start()
    {
        
    }

    
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, meleeRange, targetMask);
        foreach (Collider col in hits)
        {
            if (col.gameObject.GetComponent<PlayerMovement>() && col.gameObject.GetComponent<Stats>())
            {
                //punch coroutine
            }
        }
    }


    public IEnumerator Shockwave()
    {
        canSW = false;
        isSW = true;
        //insert vfx here

        while(swOuterRadius <= swMaxRadius)
        {
            swInnerRadius += expansionSpeed * Time.deltaTime;
            swOuterRadius += expansionSpeed * Time.deltaTime;
           
        }

        Vector3 point0 = transform.position + Vector3.up * -1;
        Vector3 point1 = transform.position + Vector3.up * 1;

        Collider[] hits = Physics.OverlapCapsule(point0, point1, swOuterRadius);
        //Physics.OverlapSphere(transform.position, swOuterRadius, targetMask);
        foreach (Collider col in hits)
        {
            Vector3 offset = col.transform.position - transform.position;
            offset.y = 0f;
            float distance = offset.magnitude;

            if (distance >= swInnerRadius && distance <= swOuterRadius)
            {
                Debug.Log("AAAAAAAAAA " + col.gameObject.name);
            }
        }


        //Collider[] hits = Physics.OverlapSphere(transform.position, swMaxRadius, targetMask);
        //foreach (Collider col in hits)
        //{
        //    if (col.gameObject.GetComponent<PlayerMovement>() && col.gameObject.GetComponent<Stats>())
        //    {
        //        PlayerMovement plyrMov = col.gameObject.GetComponent<PlayerMovement>();
        //        Stats plyrSts = col.gameObject.GetComponent<Stats>();

        //        Vector3 direc = col.transform.position - transform.position;

        //        plyrMov.AddForce(direc.normalized * swForce);

        //        float calcDmg = swDamage - plyrSts.Defense;
        //        plyrSts.Health_Current -= calcDmg;
        //    }
        //}

        isSW = false;

        yield return new WaitForSeconds(swCooldown);

        canSW = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;

        Gizmos.DrawSphere(transform.position, swOuterRadius);

        Gizmos.color = Color.pink;

        Gizmos.DrawSphere(transform.position, swInnerRadius);
    }
}
