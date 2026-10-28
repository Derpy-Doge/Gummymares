using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour, IDamage
{
    //activates when currentHealth = zero
    public bool dead;
    //canDeathSound is used only if there is no damageSoundsClip so there are no conflicts with the audio source. If you want to use a damageSoundsClip, set canDeathSound to true
    public bool canDeathSound = false;
    //health the enemy starts with
    [SerializeField] private float maxHealth = 40f;
    //sound effect for when the enemy dies (can be turned off by setting canDeathSound to false)
    [SerializeField] private AudioClip damageSoundsClip;

    //the health that can change during the game by damaging the enemy
    private float currentHealth;
    //reference to get the audio clip
    private AudioSource audioSource;

    //lines that only run on the first frame
    private void Start()
    {
        //sets current health to maxHealth value
        currentHealth = maxHealth;

        //fetches the audio source
        audioSource = GetComponent<AudioSource>();
    }

    //scripts that check or run every frame after the first
    public void Update()
    {
        
    }

    //lines that run when a player hitbox collides with the enemy hurtbox
    public void Damage(float damageAmount)
    {
        //removes HP from the amount set by the player's attack damage value
        currentHealth -= damageAmount;

        //checking if the enemy has died
        if (currentHealth <= 0)
        { 
            //the enemy has died
            dead = true;
            //Rewards Points / XP
            //Leveling_System.instance.PointRise();

            //Plays Damage SoundFX if the canDamageSound == true
            
            if (canDeathSound == true)
            {
                audioSource.clip = damageSoundsClip;
                audioSource.Play();
            }
            //Stops the audio clip from playing and stopping the enemy's death script
            else 
            {
                audioSource.Stop();
            }
               


            //Destroys GameObject
            Destroy(gameObject);
            
        }
        else if (currentHealth > 0)
        {
            //the enemy has not died and will have their currentHealth reduced
            dead = false;
        }
    }
}
