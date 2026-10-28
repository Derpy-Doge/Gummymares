using UnityEngine;
using UnityEngine.InputSystem;

public class HealthBar : MonoBehaviour
{
    public float Health, MaxHealth;
    public HealthBarUI playerHealth;

    [SerializeField]
    private HealthBarUI healthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void SetHealth(float healthChange)
    {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);
    }

}
