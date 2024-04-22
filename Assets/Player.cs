using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    PlayerControls controls;
    Rigidbody rb;

    public float speed = 5.0f;
    public float mouseSensitivity = 0.1f;

    Vector2 moveInput;
    Vector2 lookInput;
    float xRotation = 0f;

    bool isJumping = false;

    void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();

        controls.gameplay.move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.gameplay.move.canceled += ctx => moveInput = Vector2.zero;
        controls.gameplay.jump.performed += OnJump;

        controls.gameplay.look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.gameplay.look.canceled += ctx => lookInput = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Hide the cursor from view
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + mouseX, 0f);
        Camera.main.transform.localEulerAngles = new Vector3(xRotation, 0f, 0f);
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        rb.MovePosition(rb.position + transform.TransformDirection(move) * speed * Time.fixedDeltaTime);

        if (isJumping)
        {
            rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
            isJumping = false;
        }
    }

    // Utility method to check if the player is on the ground
    private bool IsGrounded()
    {
        // Checks if there's a collision on the player's feet
        return Physics.Raycast(transform.position, Vector3.down, 0.6f);
    }

    void OnEnable()
    {
        controls.gameplay.Enable();
    }

    void OnDisable()
    {
        controls.gameplay.Disable();
    }
}