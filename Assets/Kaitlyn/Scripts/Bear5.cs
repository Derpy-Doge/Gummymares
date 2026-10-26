using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class Bear5 : MonoBehaviour
{
    public VisualEffect swVFX;
    [Space(5f)]
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
    public float swOuterRadius = 2f;
    private float innerRadius; 
    private float swInnerRadius = 1.5f;
    private float outerRadius;
    [Space(3f)]
    public float expansionDuration = 5f;
    public float swHieght = 1f;
    private Vector3 point0;
    private Vector3 point1;
    public float swForce = 7f;
    public float swDamage = 25;
    public int crushDamage = 60; // when bear5 lands on the player during shockwave
    private List<Collider> alreadyHit;
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
        swInnerRadius = swOuterRadius * .75f;

        point0 = transform.position + Vector3.up * -(swHieght * .5f);
        point1 = transform.position + Vector3.up * (swHieght * .5f);

        alreadyHit = new List<Collider>();
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
        if (!canSW)
        {
            yield break;
        }

        canSW = false;
        isSW = true;

       innerRadius = swInnerRadius;
        outerRadius = swOuterRadius;

        float elapsedTime = 0f;
        //insert vfx here

        while (outerRadius < swMaxRadius)
        {
            elapsedTime += Time.deltaTime;

            innerRadius = Mathf.Lerp(swInnerRadius, (swMaxRadius * .75f), elapsedTime / expansionDuration);
            outerRadius = Mathf.Lerp(swOuterRadius, swMaxRadius, elapsedTime / expansionDuration);


            Collider[] uppies = Physics.OverlapBox(transform.position + (Vector3.up * swHieght), new Vector3(swMaxRadius * .75f, swHieght * .5f, swMaxRadius * .75f), transform.rotation, targetMask);
            Collider[] hits = Physics.OverlapCapsule(point0, point1, outerRadius, targetMask);
            //Physics.OverlapSphere(transform.position, swOuterRadius, targetMask);
            foreach (Collider col in hits)
            {
                Vector3 offset = col.transform.position - transform.position;
                offset.y = 0f;
                float distance = offset.magnitude;
                if(uppies.Length <= 0)
                {

                    if (distance >= innerRadius && distance <= outerRadius)
                    {
                        if (alreadyHit.Count > 0)
                        {
                            for (int i = 0; i < alreadyHit.Count; i++)
                            {
                                if (col.gameObject.name == alreadyHit[i].gameObject.name) // if been hit dont run hit logic
                                {
                                    continue;
                                }
                                else
                                {
                                    alreadyHit.Add(col);
                                    Debug.Log("AAAAAAAAAA " + col.gameObject.name);
                                }
                            }
                        }
                        else
                        {
                            alreadyHit.Add(col);
                            Debug.Log("AAAAAAAAAA " + col.gameObject.name);
                        }
                    }                   
                }
                else
                {
                    Debug.Log("nuh uh");
                }
            }

            yield return null;
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

        Debug.Log("gurt");
        alreadyHit.Clear();
        yield return new WaitForSeconds(swCooldown);

        canSW = true;
        Debug.Log("e");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;

        Gizmos.DrawSphere(transform.position, outerRadius);

        Gizmos.color = Color.pink;

        Gizmos.DrawSphere(transform.position, innerRadius);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(point0, point1);

        Gizmos.color = Color.orange;

        Gizmos.DrawCube(transform.position + (Vector3.up * swHieght), new Vector3(swMaxRadius * 1.5f, swHieght, swMaxRadius * 1.5f));
    }
}
