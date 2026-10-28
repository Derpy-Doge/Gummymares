using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class Bear5 : MonoBehaviour
{
    public Animator animator;
    [Space(5f)]
    [Header("Movement")]
    public float moveSpeed;
    public Transform target;
    private Rigidbody rb;

    [Space(5f)]
    public float meleeRange = 1.5f;
    public LayerMask targetMask;

    [Space(6f)]
    [Header("Punch")]
    public float pRadius = 1f;
    public float pForce = 1f;
    public float pDamage = 35;
    [Space(3f)]
    private bool canPunch = true;
    private bool isPunching = false;
    public float pCooldown = 5f;

    [Space(6f)]
    [Header("Shockwave")]
    public VisualEffect swVFX;
    public DecalProjector swWarning;
    [Space(3f)]
    public AnimationClip up;
    public AnimationClip down;
    [Space(3f)]
    public float swMaxRadius = 5f;
    [Space(2f)]
    public float swOuterRadius = 2f;
    private float outerRadius;
    [Space(2f)]
    private float swInnerRadius = 1.5f;
    private float innerRadius; 
    [Space(3f)]
    public float expansionDuration = 5f;
    public float swHieght = 1f;
    private Vector3 point0;
    private Vector3 point1;
    public float swForce = 7f;
    public float swDamage = 25;

    private List<Collider> alreadyHit;
    [Space(3f)]
    public int crushDamage = 60; // when bear5 lands on the player during shockwave
    public float crushRadius = 1f;
    public float crushForce = 1f;
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
        target = GameObject.FindAnyObjectByType<PlayerMovement>().transform;

        swVFX.Stop();
        swWarning.gameObject.SetActive(false);

        swInnerRadius = swOuterRadius * .75f;

        point0 = transform.position + Vector3.up * -(swHieght * .5f);
        point1 = transform.position + Vector3.up * (swHieght * .5f);

        alreadyHit = new List<Collider>();
    }

    
    void Update()
    {        
            Collider[] hits = Physics.OverlapSphere(transform.position + (Vector3.forward*1), meleeRange, targetMask);
        foreach (Collider col in hits)
        {
            if (col.gameObject.GetComponent<PlayerMovement>() && col.gameObject.GetComponent<Stats>())
            {
                if (canPunch)
                {
                    StartCoroutine(Punch());
                    return;
                }
                else
                {
                    rb.linearVelocity = Vector3.zero;
                    return;
                }
            }
        }

        if(hits.Length == 0 && canSW)
        {
            StartCoroutine(Shockwave());
            rb.linearVelocity = Vector3.zero;
            return;
        }

        if(isSW || isPunching)
        {
            rb.linearVelocity = Vector3.zero;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        if (hits.Length == 0 && !canSW)
        {
            rb.linearVelocity = new Vector3(direction.x, 0, direction.z) * moveSpeed * Time.deltaTime;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }


    }


    public IEnumerator Punch()
    {
        canPunch = false;
        isPunching = true;

        yield return new WaitForSeconds(.1f);
        AudioManager.instance.PlayPlayerSfx(AudioManager.instance.playerAttack);
        Collider[] Hits = Physics.OverlapSphere(transform.position , pRadius, targetMask);
        foreach (Collider col in Hits)
        {
            if (col.gameObject.GetComponent<PlayerMovement>() && col.gameObject.GetComponent<Stats>())
            {
                PlayerMovement plyrMov = col.gameObject.GetComponent<PlayerMovement>();
                Stats plyrSts = col.gameObject.GetComponent<Stats>();

                Vector3 direc = col.transform.position - transform.position;

                plyrMov.AddForce(direc.normalized * pForce);

                float calcDmg = pDamage - plyrSts.Defense;
                plyrSts.Health_Current -= calcDmg;
            }
            else
            {
                
            }
        }

        isPunching = false;
        yield return new WaitForSeconds(pCooldown);
        canPunch = true;
    }

    public IEnumerator Shockwave()
    {
        if (!canSW)
        {
            yield break;
        }

        canSW = false;
        isSW = true;

        float elapsedTime = 0f;

        innerRadius = swInnerRadius;
        outerRadius = swOuterRadius;

        Vector3 plyrPos = GameObject.FindAnyObjectByType<PlayerMovement>().transform.position;

        swWarning.gameObject.SetActive(true);
        yield return WaitForAnimEnd(up);
        while ((transform.parent.position.x != plyrPos.x) && (transform.parent.position.z != plyrPos.z))
        {
            elapsedTime += Time.deltaTime;

            float xPos = transform.parent.position.x;
            float zPos = transform.parent.position.z;

            xPos = Mathf.Lerp(xPos, plyrPos.x, elapsedTime / 2f);
            zPos = Mathf.Lerp(zPos, plyrPos.z, elapsedTime / 2f);

            transform.parent.position = new Vector3 (xPos, transform.parent.position.y, zPos);
            yield return null;
        }

        yield return WaitForAnimEnd(down);;

        Collider[] boom = Physics.OverlapSphere(transform.position, crushRadius, targetMask);
        foreach (Collider col in boom)
        {
            if (col.gameObject.GetComponent<PlayerMovement>() && col.gameObject.GetComponent<Stats>())
            {
                PlayerMovement plyrMov = col.gameObject.GetComponent<PlayerMovement>();
                Stats plyrSts = col.gameObject.GetComponent<Stats>();

                Vector3 direc = col.transform.position - transform.position;

                plyrMov.AddForce(direc.normalized * crushForce);

                float calcDmg = crushDamage - plyrSts.Defense;
                plyrSts.Health_Current -= calcDmg;

                Debug.Log("groot");
            }

            alreadyHit.Add(col);
        }


            elapsedTime = 0f;
        //insert vfx here
        swVFX.Play();

        while (outerRadius < swMaxRadius)
        {
            elapsedTime += Time.deltaTime;

            swVFX.SetFloat("MajorRadius", outerRadius);
            
            innerRadius = Mathf.Lerp(swInnerRadius, (swMaxRadius * .75f), elapsedTime / expansionDuration);
            outerRadius = Mathf.Lerp(swOuterRadius, swMaxRadius, elapsedTime / expansionDuration);


            Collider[] uppies = Physics.OverlapBox(transform.position + (Vector3.up * swHieght), new Vector3(swMaxRadius * .75f, swHieght * .5f, swMaxRadius * .75f), transform.rotation, targetMask);
            Collider[] hits = Physics.OverlapCapsule(point0, point1, outerRadius, targetMask);
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
                                if (col.gameObject.name == alreadyHit[i].gameObject.name || col.gameObject.GetComponent<PlayerMovement>()._isDashing) // if been hit dont run hit logic
                                {
                                    continue;
                                }
                                else
                                {
                                    alreadyHit.Add(col);
                                    Debug.Log("AAAAAAAAAA " + col.gameObject.name);
                                    //hurt logic
                                     if (col.gameObject.GetComponent<PlayerMovement>() && col.gameObject.GetComponent<Stats>())
                                     {
                                        PlayerMovement plyrMov = col.gameObject.GetComponent<PlayerMovement>();
                                        Stats plyrSts = col.gameObject.GetComponent<Stats>();
                                        
                                        Vector3 direc = col.transform.position - transform.position;
                                        
                                        plyrMov.AddForce(direc.normalized * swForce);
                                         
                                        float calcDmg = swDamage - plyrSts.Defense;
                                        plyrSts.Health_Current -= calcDmg;
                                     }

                                }
                            }
                        }
                        else
                        {
                            alreadyHit.Add(col);
                            Debug.Log("AAAAAAAAAA " + col.gameObject.name);
                            //hurt logic
                            if (col.gameObject.GetComponent<PlayerMovement>() && col.gameObject.GetComponent<Stats>())
                            {
                                PlayerMovement plyrMov = col.gameObject.GetComponent<PlayerMovement>();
                                Stats plyrSts = col.gameObject.GetComponent<Stats>();

                                Vector3 direc = col.transform.position - transform.position;

                                plyrMov.AddForce(direc.normalized * swForce);

                                float calcDmg = swDamage - plyrSts.Defense;
                                plyrSts.Health_Current -= calcDmg;
                            }
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

        isSW = false;

        Debug.Log("gurt");
        swVFX.Stop();
        swWarning.gameObject.SetActive(false);
        alreadyHit.Clear();
        yield return new WaitForSeconds(swCooldown);

        canSW = true;
        Debug.Log("e");
    }

    public IEnumerator WaitForAnimEnd(AnimationClip clip) //gen js stole ts hope it works :man_juggling:
    {
        float timeout;

        if (animator == null)
            yield break;

        timeout = .5f;

        animator.Play(clip.name);
        animator.Update(0f);

        int hash = Animator.StringToHash(clip.name);

        // wait for anim to start (or transition to it) up to timeout

        animator.CrossFade(clip.name, 0f, 0);

        //wait for anim to finish
        float timer = 0f;
        while (timer < timeout)
        {
            yield return new WaitForEndOfFrame();
            //var current = player.animator.GetCurrentAnimatorStateInfo(0);

            //if (current.shortNameHash != hash && !player.animator.IsInTransition(0))
            //    break;

            timer += Time.deltaTime;
            yield return null;
        }
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

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, crushRadius);
    }
}
