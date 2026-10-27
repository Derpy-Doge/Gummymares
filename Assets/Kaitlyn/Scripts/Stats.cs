using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    public float Health_Max;
    [HideInInspector]public float Health_Current;
    [Space(7f)]

    public float Attack_Speed;
    public float Attack_Power;
    [Space(4f)]
    public float Defense;
    [Space(4f)]
    public float Speed;
    [HideInInspector] public float Passive_Attack; // for enemies only, damage you take when you run into them


    private bool isPlayer;
    public static bool isDead = false;

    void Awake()
    {

    }

    void Start()
    {
        Health_Current = Health_Max;

        if(GetComponent<PlayerMovement>() != null)
        {
            isPlayer = true;
        }
        else
        {
            isPlayer = false;
        }


        if (isPlayer)
        {
           PlayerMovement plyr = GetComponent<PlayerMovement>();

            Speed = plyr.moveSpeed;
        }
    }

    void Update()
    {
        bool expAwarded = false;

        if (Health_Current <= 0 && !isPlayer && !expAwarded)
        {
            // xp reward logic

            expAwarded = true;
            Destroy(gameObject);
        }
        if (Health_Current <= 0 && isPlayer && !isDead)
        {
            isDead = true;
            Debug.Log("YOURE DEAD CUH");
        }


        if (Health_Current > Health_Max)
        {
            Health_Current = Health_Max;
        }

        Passive_Attack = Attack_Power * 0.5f;
    }
}
