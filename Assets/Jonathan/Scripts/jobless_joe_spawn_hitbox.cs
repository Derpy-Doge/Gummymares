using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy_Spawn_Hitbox : MonoBehaviour
{
    public float attackRadius = 1.5f;
    

    private bool isAttack;
    private bool canAttack = true;
    public float attackCooldown;
    public float attackDuration;

    public LayerMask attackLayer;
    private enemy enemy;

    public Stats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<enemy>();
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttack)
        {
            enemy.rb.linearVelocity = Vector3.zero;
        }
        
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius, attackLayer);
        foreach (Collider col in hits)
        {
            if (canAttack)
            {

            StartCoroutine(Attack());
            }
            else
            {
                //Debug.Log("my father left");
            }
        }
    }


    public IEnumerator Attack()

    {
        isAttack = true;
        canAttack = false;
        
            Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius, attackLayer);

            foreach (Collider col in hits)
            {
                Debug.Log(col.gameObject.name);

                if (col.gameObject.GetComponent<Stats>())
            {
                Stats Player = col.gameObject.GetComponent<Stats>();

                Player.Health_Current -= stats.Attack_Power;
                Debug.Log(Player.Health_Current);
            }
            }
        yield return new WaitForSeconds(attackDuration);
        isAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }



}
