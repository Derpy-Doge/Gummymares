using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class ArcadePlayer : MonoBehaviour
{
    public float Speed = 10f;

    private float currentSpeed;
    private Vector2 movement;
    private Rigidbody2D rb2D;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        currentSpeed = Speed;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.linearVelocity = movement * currentSpeed;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }

    public void Run(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 1)
        {
            currentSpeed = Speed;
        }
        else
        {
            currentSpeed = Speed;
        }
    }


}
