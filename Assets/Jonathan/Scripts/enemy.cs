using Unity.VisualScripting;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public Transform target;

    public float moveSpeed = 2f;
    public float chaseRange;
    private float adjustedChaseRange;

    public int xp;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    [Range(0f, 360f)]
    public float fovAngle; // currently dont use this caus eenemy sprite always faces player so its weird
    public bool canSeePlayer;

    public AudioSource enemyAudioSource; 



    [HideInInspector]public Rigidbody rb;
  
    
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        adjustedChaseRange = chaseRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (time_manager.instance.isDay)
        {
            Destroy(this.gameObject);

            Debug.Log("icicle");
        }

        Stats player = FindAnyObjectByType<PlayerMovement>().gameObject.GetComponent<Stats>();

        adjustedChaseRange = chaseRange / player.Stealth;

        FieldOfViewCheck();
        if (target != null && canSeePlayer)
        {          
            // Calculate the direction towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0;

            // Move towards the target if the distance is greater than the stopping distance;
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= adjustedChaseRange)
            {
                if(enemyAudioSource != null)
                {
                    if(enemyAudioSource.clip != AudioManager.instance.enemyMovement || !enemyAudioSource.isPlaying)
                    {
                        enemyAudioSource.clip = AudioManager.instance.enemyMovement;
                        enemyAudioSource.Play();
                    }
                    else
                    {
                        // gulp do nothing :man_juggling:
                    }
                }
                else
                {
                    //stupid ahh smug emoji
                }

                    rb.linearVelocity = new Vector3(direction.x, 0, direction.z) * (moveSpeed * 250f) * Time.deltaTime;
            }
            else
            {
                rb.linearVelocity = Vector3.zero;
                if (enemyAudioSource != null)
                {
                    if (enemyAudioSource.clip != AudioManager.instance.enemyMovement || enemyAudioSource.isPlaying)
                    {
                        enemyAudioSource.clip = AudioManager.instance.enemyMovement;
                        enemyAudioSource.Stop();
                    }
                    else
                    {
                        enemyAudioSource.Stop();
                    }
                }
                else
                {
                    
                }
            }

         
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, adjustedChaseRange, targetMask);

        if (rangeChecks.Length > 0)
        {
            Transform target = rangeChecks[0].transform;

            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0;

            float distance = Vector3.Distance(transform.position, target.position);
            if(distance <= adjustedChaseRange)
            {
                if (!Physics.Raycast(transform.position, direction, distance, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false ;
            }
            
            
            // uhh g-gulp ignore this i promise i might remove it latr
                //if(Vector3.Angle(transform.forward, direction) < fovAngle / 2)
                //{
                //    float distance = Vector3.Distance(transform.position, target.position);

                //    if (!Physics.Raycast(transform.position, direction, distance, obstructionMask))
                //    {
                //        canSeePlayer = true;
                //    }
                //    else
                //    {
                //        canSeePlayer = false;
                //    }
                //}
                //else
                //{
                //    canSeePlayer = false;
                //}
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, adjustedChaseRange);

        Gizmos.color = Color.darkSlateBlue;

        Vector3 fovAngle1 = DirecFromAngle(transform.eulerAngles.y, -fovAngle / 2);
        Vector3 fovAngle2 = DirecFromAngle(transform.eulerAngles.y, fovAngle / 2);

        Gizmos.DrawLine(transform.position, transform.position + fovAngle1 * adjustedChaseRange);
        Gizmos.DrawLine(transform.position, transform.position + fovAngle2 * adjustedChaseRange);

        Gizmos.color = Color.lemonChiffon;

        Gizmos.DrawLine(transform.position, target.position);
    }

    private Vector3 DirecFromAngle(float eulerY, float AngleInDeg)
    {
        AngleInDeg += eulerY;

        return new Vector3(Mathf.Sin(AngleInDeg * Mathf.Deg2Rad),0, Mathf.Cos(AngleInDeg * Mathf.Deg2Rad));
    }


}
