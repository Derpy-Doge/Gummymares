using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Space(3f)]
    private float _moveSpeed = 5f;

    public float walkSpeed = 5f;

    public float sprintSpeed = 10f;

    private bool _isSprinting;
    private bool _canSprint;

    [Space(7f)]
    public float dashForce = 30f;
    public float dashDuration = 1f;
    public float dashCooldown = 2.5f;

    private bool _canDash = true;
    private bool _isDashing = false;
    private Vector3 _dashDirection;
    [Space(7f)]
    public float jumpForce = 5f;
    public float airMultiplier = .5f;
    private bool _canJump = true;

    [Header("Physics Settings")]
    [Space(3f)]
    public float gravity = -9.81f;
    public float gravityMultiplier = 2f;

    [Header("Ground Check Settings")]
    [Space(3f)]
    public float groundCheckDistance = 6f;
    public LayerMask groundLayer;

    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector3 _groundCheckSize;
    private bool _isGrounded;
    private bool _movingDownSlope = false;


    private CharacterController _characterController;
    private CameraController _camController;
    private InputSystem_Actions _controls;
    private Vector2 _moveInput;
    private Vector3 _movementVelocity;
    private Vector3 _externalMomentum = Vector3.zero;
    public Vector3 LastDirection { get; private set; }
    public Vector3 LookingDirection { get; private set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _camController = FindAnyObjectByType<CameraController>();
        _controls = new InputSystem_Actions();
        LastDirection = transform.forward;

        #region CamSwicth
        _controls.Player.SwitchCamera.performed += ctx =>
        {
            Debug.Log("yo bro is this thing on");
            if (_camController.firstPersonCam.Priority > _camController.thirdPersonCam.Priority)
            {
                _camController.SwitchToThirdPerson();
            }
            else
            {
                _camController.SwitchToFirstPerson();
            }
        };
        #endregion

        #region Walking
        float deadzoneAngle = _camController.horizontalDeadzoneAngle;
        _controls.Player.Move.performed += ctx =>
        {
            _moveInput = ctx.ReadValue<Vector2>();
            _moveSpeed = walkSpeed;
            Quaternion camRotation = Quaternion.Euler(0f, _camController.transform.eulerAngles.y, 0f);
            transform.rotation = camRotation;
            _camController._resetXRotation = 0f;
            _camController.SetDefaultRotation(); //jesus that was harder to figure out than i thought itd be
            _camController.horizontalDeadzoneAngle = 0f; //so when you look around while walking youll automatically rotate
        };
        _controls.Player.Move.canceled += ctx =>
        {
            _moveInput = Vector2.zero;
            _camController.horizontalDeadzoneAngle = deadzoneAngle; //reset the deadzone angle when you stop moving
        };
        #endregion


        #region Sprint
        _controls.Player.Sprint.performed += ctx =>
        {
            if (_canSprint)
            {
                _isSprinting = true;
                _moveSpeed = sprintSpeed;
            }
        };
        _controls.Player.Sprint.canceled += ctx =>
        {
            _isSprinting = false;
        };
        #endregion

        #region Dash/Jump
        _controls.Player.Dash.performed += ctx =>
        {
            if (_canDash)
            {
                _canDash = false;

                if (_moveInput.magnitude > 0.01f)
                {
                    StartCoroutine(InputDash());
                }
                else
                {
                    StartCoroutine(LookDirectionDash());
                }             
            }
            else
            {
                return;
            }
        };

        _controls.Player.Jump.performed += ctx =>
        {
            _controls.Player.Jump.performed += ctx => Jump();
        };
        #endregion
    }

    void OnEnable()
    {
        _controls.Player.Enable();
    }

    void OnDisable()
    {
        _controls.Player.Disable();
    }

   

    void Update()
    {
        GameObject camera = _camController.gameObject; //_camController.gameObject;
        _camController._MouseInput = _controls.Player.Look.ReadValue<Vector2>();


     

        if (_isDashing)
        {
            return;
        }

        if (_isSprinting)
        {
            _moveSpeed = sprintSpeed * .1f;
        }
        else
        {
            _moveSpeed = walkSpeed * .1f;
        }

        Vector3 forward = camera.transform.forward; // directions based off cam position :man_juggling:
        Vector3 right = camera.transform.right;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * _moveInput.y) + (right * _moveInput.x);
        Vector3 adjustedMoveDirection = AdjustMovementToSlope(moveDirection);
        adjustedMoveDirection = adjustedMoveDirection.normalized;
        adjustedMoveDirection.y = _movementVelocity.y;

        if (IsMovingDownSlope(moveDirection))
        {
            _movingDownSlope = true;
        }
        else
        {
            _movingDownSlope = false;
        }
        
        #region dash direction

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            LastDirection = new Vector3(moveDirection.x, 0f, moveDirection.z);
        }

        LookingDirection = forward.normalized;
        #endregion

        #region drag on exterrnal momentum?        
        _externalMomentum.x -= _externalMomentum.x * (Time.deltaTime * 3);
        _externalMomentum.z -= _externalMomentum.z * (Time.deltaTime * 3);

        if (Mathf.Abs(_externalMomentum.x) < 0.01f) _externalMomentum.x = 0f;
        if (Mathf.Abs(_externalMomentum.z) < 0.01f) _externalMomentum.z = 0f;
        #endregion


        if (_moveInput.magnitude > 0.01f)
        {
            _movementVelocity.x = adjustedMoveDirection.x * _moveSpeed;
            _movementVelocity.z = adjustedMoveDirection.z * _moveSpeed;
        }
        else
        {
            //_movementVelocity.x -= _movementVelocity.x * (Time.deltaTime * 3);
            //_movementVelocity.z -= _movementVelocity.z * (Time.deltaTime * 3);

            //if (Mathf.Abs(_movementVelocity.x) < 0.01f) _movementVelocity.x = 0f;
            //if (Mathf.Abs(_movementVelocity.z) < 0.01f) _movementVelocity.z = 0f;
            //_externalMomentum.y = 0f;

            _movementVelocity.x = 0;
            _movementVelocity.z = 0;
        }   

        _characterController.Move(_movementVelocity + _externalMomentum * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckBox(_groundCheckPoint.position, _groundCheckSize / 2, _groundCheckPoint.rotation, groundLayer);     

        if (_movingDownSlope)
        {
            _movementVelocity.y = -.25f;
        }

        if (_isGrounded && _movementVelocity.y < 0f)
        {
            _canJump = true;
            _movementVelocity.y = -.15f;
        }
        else
        {
            if (_isDashing)
            {
                _movementVelocity.y = 0f; //gravity on dash is lame
            }
            else
            {
                _movementVelocity.y += gravity * (gravityMultiplier * .1f) * Time.deltaTime;
            }
               }
    }

    private Vector3 AdjustMovementToSlope(Vector3 inputDir)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            if (slopeAngle <= _characterController.slopeLimit)
            {               
                return Vector3.ProjectOnPlane(inputDir, hit.normal).normalized;
            }
            else
            {
                return Vector3.zero;
            }
        }

        return inputDir;
    }

    public bool IsMovingDownSlope(Vector3 inputDir)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            Vector3 groundNormal = hit.normal;

            if (Mathf.Approximately(groundNormal.y, 1f))
            {
                return false;
            }

            Vector3 downhillDirection = Vector3.ProjectOnPlane(Vector3.down, groundNormal).normalized;

            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.z).normalized;

            float dotProduct = Vector3.Dot(moveDir, downhillDirection);

            return dotProduct > 0.05f;
        }
        return false;
    }


    public void AddForce(Vector3 force)
    {
        _externalMomentum += force;
    }

    private void Jump()
    {
        if (!_canJump)
        {
            return;
        }
        _movementVelocity.y = 0f;

        if (_isGrounded)
        {
            _canJump = false;
            _movementVelocity.y = Mathf.Sqrt((jumpForce * .001f) * -2f * gravity);
        }

        _movementVelocity.y += gravity * gravityMultiplier * Time.deltaTime;
    }

    private IEnumerator InputDash()
    {
          _isDashing = true;

        _dashDirection = LastDirection;

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            AddForce(_dashDirection * dashForce);
            _characterController.Move(_movementVelocity + _externalMomentum * Time.deltaTime);
            yield return null;
        }

        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    private IEnumerator LookDirectionDash()
    {
        _canDash = false;
        _isDashing = true;

        _dashDirection = LookingDirection.normalized;

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            AddForce(_dashDirection * dashForce);
            _characterController.Move(_movementVelocity + _externalMomentum * Time.deltaTime);
            yield return null;
        }


        _isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 originalMatrix = Gizmos.matrix;

        Gizmos.matrix = _groundCheckPoint.localToWorldMatrix;

        Gizmos.color = Color.green; //groundcheck
        Gizmos.DrawCube(Vector3.zero, _groundCheckSize);

        Gizmos.matrix = originalMatrix;

        Gizmos.color = Color.red; //groundcheck ray
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);


        Gizmos.color = Color.blue; //direction lines
        Gizmos.DrawLine(transform.position, transform.position + LastDirection * 2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + LookingDirection * 2);
    }
}
