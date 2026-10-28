using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public Slider slider;
    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        slider.value = health;

        if (health <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void HealDamage(int amount)
    {
        health += amount;
        slider.value = health;
    }
}
