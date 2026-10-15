using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target References")]
    public Transform playerBody;
    public Transform cameraTransform;

    [Header("Camera Settings")]
    public float verticalMouseSensitivity = 10f;
    public float horizontalMouseSensitivity = 10f;
    [Tooltip("How far to the left or right you can look before the player's body turns.")]
    public float horizontalDeadzoneAngle = 80f;
    [Tooltip("How far down you can look.")]
    public float verticalMinAngle = 56f;
    [Tooltip("How far up you can look.")]
    public float verticalMaxAngle = 90f;


    private float _yRotation = 0f; // left and right
    private float _xRotation = 0f; // up and down
    [HideInInspector] public float _resetXRotation = 0f;
    // god i hate 3d

    [Header("Camera Swicther")]
    public CinemachineCamera firstPersonCam;
    public CinemachineCamera thirdPersonCam;

    [HideInInspector] public Vector2 _MouseInput;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _xRotation = playerBody.eulerAngles.y;
    }

    void Update()
    {
        if (playerBody == null || cameraTransform == null) return;

        float mouseX = _MouseInput.x * horizontalMouseSensitivity * Time.deltaTime;
        float mouseY = _MouseInput.y * verticalMouseSensitivity * Time.deltaTime;

        //cause 3d sucks ahh x is up and down and y is left and right
        _yRotation -= mouseY;
        _xRotation += mouseX;
        _yRotation = Mathf.Clamp(_yRotation, -verticalMaxAngle, verticalMinAngle);

        float currentAngle = Mathf.Clamp(playerBody.rotation.eulerAngles.y, -horizontalDeadzoneAngle, horizontalDeadzoneAngle);
        if (_xRotation > horizontalDeadzoneAngle)
        {
            float excessRotation = _xRotation - horizontalDeadzoneAngle;
            playerBody.Rotate(Vector3.up, excessRotation);
            _xRotation = horizontalDeadzoneAngle;
        }
        else if (_xRotation < -horizontalDeadzoneAngle)
        {
            float excessRotation = _xRotation + horizontalDeadzoneAngle;
            playerBody.Rotate(Vector3.up, excessRotation);
            _xRotation = -horizontalDeadzoneAngle;
        }

        cameraTransform.localRotation = Quaternion.Euler(_yRotation, _xRotation, 0f);
    }

    public void SwitchToFirstPerson()
    {
        if (firstPersonCam != null && thirdPersonCam != null)
        {
            firstPersonCam.Priority = 10;
            thirdPersonCam.Priority = 0;
        }
    }

    public void SwitchToThirdPerson()
    {
        if (firstPersonCam != null && thirdPersonCam != null)
        {
            firstPersonCam.Priority = 0;
            thirdPersonCam.Priority = 10;
        }
    }

    public void SetDefaultRotation()
    {
        if (playerBody != null)
        {
            _xRotation = _resetXRotation;
        }
    }
}
