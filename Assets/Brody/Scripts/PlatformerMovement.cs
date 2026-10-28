using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlatformerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpHeight;

    private float _movement;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Vector2 _boxOffset;
    [SerializeField] private Vector2 _boxSize;
    [SerializeField] private LayerMask _boxLayer;

    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer rndr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rndr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.linearVelocityX = _movement;

        anim.SetBool("isgrounded", IsGrounded());
        anim.SetFloat("Yvelocity", rb2d.linearVelocityY);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        _movement = ctx.ReadValue<Vector2>().x * moveSpeed;

        anim.SetFloat("walk", Mathf.Abs(_movement));

        if (_movement > 0)
        {
            rndr.flipX = true;
        }
        else if (_movement < 0)
        {
            rndr.flipX = false;
        }
    }
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 1)
        {
            if (ctx.canceled || !IsGrounded())
            return;

            rb2d.linearVelocityY = jumpHeight;
            anim.SetTrigger("jump");
        }

    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3)_boxOffset, _boxSize, 0, Vector2.zero, 0, _boxLayer);
        //Debug.Log(hit.collider.name);

        return hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)_boxOffset, _boxSize);
    }
}
