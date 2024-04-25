using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float airAcceleration = 1f;
    public float movementForce = 10f;

    public float mouseSensitivity = 0.1f;

    private PlayerControls controls;
    private Rigidbody rb;
    private Transform playerCamera;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private bool isJumping = false;

    private Vector3 originalPosition;

    void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main.transform;

        controls.gameplay.move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.gameplay.move.canceled += ctx => moveInput = Vector2.zero;
        controls.gameplay.jump.performed += ctx => isJumping = true;
        controls.gameplay.jump.canceled += ctx => isJumping = false;
        controls.gameplay.reset.performed += ctx => {
            transform.position = originalPosition;
            rb.velocity = Vector3.zero;
        };

        controls.gameplay.look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.gameplay.look.canceled += ctx => lookInput = Vector2.zero;

        originalPosition = transform.position;
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

        yRotation += mouseX;
        yRotation %= 360;

        playerCamera.transform.localEulerAngles = new Vector3(xRotation, yRotation, 0f);
    }

    private void FixedUpdate()
    {
        Vector3 moveInput = new Vector3(this.moveInput.x, 0, this.moveInput.y);

        if (IsGrounded())
        {
            if (isJumping)
            {
                rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
                // Without this we jump multiple times instantaneously.
                isJumping = false;
            }

            if (moveInput.sqrMagnitude > 0)
            {
                // Apply a force that adds to the current velocity in the desired direction.
                Vector3 cameraDirection = playerCamera.TransformDirection(moveInput);
                cameraDirection = new Vector3(cameraDirection.x, 0f, cameraDirection.z);
                rb.AddForce(cameraDirection * movementForce, ForceMode.Force);
            }
            else
            {
                rb.velocity -= rb.velocity * 0.2f;
            }

            ClampVelocity();


        }
        else
        {
            Strafe(moveInput);
        }
    }

    private void Strafe(Vector3 moveInput)
    {
        // Skip calculations if we can.
        if (moveInput.sqrMagnitude == 0)
        {
            return;
        }

        if (moveInput.x > 0 || moveInput.x < 0)
        {
            // Don't forget gravity!
            Vector3 y = new Vector3(0, rb.velocity.y, 0);
            Vector3 velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            float magnitude = velocity.magnitude;
            Vector3 cameraForward = playerCamera.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            rb.velocity = cameraForward * magnitude + y;
        }
    }

    void ClampVelocity()
    {
        // We need the y value later as we're not clamping that.
        Vector3 velocity = rb.velocity;

        // Clamp horizontal movement.
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, movementSpeed);

        // Apply clamped x and z velocity.
        rb.velocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
    }

    private bool IsGrounded()
    {
        // TODO: Could check the normal of the ground to make sure jump on a ramp is impossible.
        // Checks if there's a collision on the player's feet.
        // Slides on 31 degrees, jumps on 30 degrees.
        return Physics.Raycast(transform.position, Vector3.down, 0.90875f);
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
