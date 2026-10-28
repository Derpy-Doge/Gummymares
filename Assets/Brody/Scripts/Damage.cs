using UnityEngine;
using UnityEngine.InputSystem;

public class Damage : MonoBehaviour
{
    public int damage = 20;
    private Health playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth = collision.gameObject.GetComponent<Health>();
            playerHealth.TakeDamage(damage);
        }
    }
}