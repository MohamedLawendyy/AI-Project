using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerScript3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("IK Settings")]
    public TwoBoneIKConstraint handIKConstraint;
    public GameObject handGripTarget;

    // runtime state
    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody _rb;
    private Animator _animator;
    private InputSystem_Actions _inputActions;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        if (_rb == null)
        {
            Debug.LogError("PlayerScript3D requires a Rigidbody component on the same GameObject.");
        }

        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        // enable the action map and register callbacks
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMoveCancelled;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMoveCancelled;
        _inputActions.Player.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

    private void OnAnimatorMove()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            DoorScript3D doorScript = other.GetComponent<DoorScript3D>();
            if (doorScript != null)
            {
                handGripTarget.transform.position = new Vector3(transform.position.x + 0.321442f, doorScript.doorPushTarget.position.y, doorScript.doorPushTarget.position.z);

                handIKConstraint.weight = 1f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            // when we exit the trigger, disable IK
            handIKConstraint.weight = 0f;
        }
    }

    // use FixedUpdate for physics-driven movement
    private void FixedUpdate()
    {
        if (_rb == null)
            return;

        if (_animator != null)
            _animator.SetBool("isRunning", _moveInput.sqrMagnitude > 0.001f);

        if (Camera.main == null)
            return;

        // build movement vector relative to camera orientation on the horizontal plane
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = camRight * _moveInput.x + camForward * _moveInput.y;
        moveDir.y = 0f;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            // move and rotate according to camera-relative input
            Vector3 targetPosition = _rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(targetPosition);

            Quaternion targetRotation = Quaternion.LookRotation(moveDir.normalized, Vector3.up);
            Quaternion smoothedRotation = Quaternion.Slerp(_rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            _rb.MoveRotation(smoothedRotation);
        }
    }    
}
