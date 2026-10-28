using UnityEngine;

public class HealthUp : MonoBehaviour

{
    public int heal = 20;
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
            playerHealth.HealDamage(heal);
        }
    }
}
