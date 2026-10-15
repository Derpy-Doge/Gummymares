using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy_Spawn_Hitbox : MonoBehaviour
{
    public float attackRadius = 1.5f;
    public float attackRange;
    public LayerMask attackLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Attack(InputAction.CallbackContext ctx)
    {
      Physics.SphereCast(transform.position, attackRadius, Vector3.forward, out RaycastHit hit, attackRange, attackLayer);

        if (hit.collider)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }



}
