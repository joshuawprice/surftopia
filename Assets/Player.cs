using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Movement : MonoBehaviour
{
    MovementControls actions;
    Vector3 moveInput;
    Rigidbody rb;
    bool isJumping;

    void Awake()
    {
        actions = new MovementControls();

        actions.gameplay.move.performed += OnMove;
        actions.gameplay.move.canceled += OnMove;

        actions.gameplay.jump.performed += OnJump;

        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start() { }
    // Update is called once per frame
    void Update() { }

    void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            moveInput = new Vector3(inputVector.x, 0, inputVector.y);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector3.zero;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumping = true;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * 5.0f * Time.fixedDeltaTime);
    }

    void OnEnable()
    {
        actions.gameplay.Enable();
    }
    void OnDisable()
    {
        actions.gameplay.Disable();
    }
}
