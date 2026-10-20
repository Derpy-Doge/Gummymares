using UnityEngine;

public class enemy : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public float chaseRange;
   

    [HideInInspector]public Rigidbody rb;
  
    private bool isGrounded = false;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Calculate the direction towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0;

            // Move towards the target if the distance is greater than the stopping distance;
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= chaseRange)
            {
                rb.linearVelocity = new Vector3(direction.x,0,direction.z) * moveSpeed * Time.deltaTime;
            }
            else
            {
                rb.linearVelocity = Vector3.zero;
            }

         
        }
    }
    private void FixedUpdate()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }
}
