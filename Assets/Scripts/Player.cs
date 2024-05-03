using UnityEngine;

public class Movement : MonoBehaviour
{
    private float mouseSensitivity = 0.1f;
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
        controls.gameplay.reset.performed += ctx =>
        {
            transform.position = originalPosition;
            rb.velocity = Vector3.zero;
        };

        controls.gameplay.look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.gameplay.look.canceled += ctx => lookInput = Vector2.zero;

        originalPosition = transform.position;
    }

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Hide the cursor from view
        Cursor.visible = false;
    }

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
            GroundMove(moveInput);
        }
        else
        {
            AirMove(moveInput);
        }
    }

    private void GroundMove(Vector3 moveInput)
    {
        if (isJumping)
        {
            if (rb.velocity.y == 0f)
            {
                rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
                return;
            }
        }

        if (moveInput.sqrMagnitude > 0)
        {
            // Apply a force that adds to the current velocity in the desired direction.
            Vector3 cameraDirection = playerCamera.TransformDirection(moveInput);
            cameraDirection = new Vector3(cameraDirection.x, 0f, cameraDirection.z);
            float movementForce = 60f;
            rb.AddForce(cameraDirection * movementForce, ForceMode.Force);
        }
        else
        {
            // Remove speed through "friction".
            rb.velocity -= rb.velocity * 0.2f;
        }

        ClampVelocity();
    }

    private void AirMove(Vector3 moveInput)
    {
        if (moveInput.sqrMagnitude == 0)
        {
            return;
        }

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        // Remove the pitch that the camera is at from the movement calculation.
        forward.y = 0;
        forward.Normalize();

        Vector3 wishVel = forward * moveInput.z + right * moveInput.x;
        wishVel.y = 0;

        Vector3 wishDir = wishVel.normalized;
        float wishSpeed = wishVel.magnitude;

        float maxSpeed = 3500;
        float airAccelerate = 20;

        // Clamp to max speed.
        if (wishSpeed > maxSpeed)
        {
            wishVel = wishVel.normalized * maxSpeed;
            wishSpeed = maxSpeed;
        }

        AirAccelerate(wishDir, wishSpeed, airAccelerate);
    }

    private void AirAccelerate(Vector3 wishDir, float wishSpeed, float accel)
    {
        float airSpeedCap = 3500;

        // Cap speed.
        float wishSpd = Mathf.Min(wishSpeed, airSpeedCap);

        // Determine veer amount.
        float currentSpeed = Vector3.Dot(rb.velocity, wishDir);

        // See how much speed to add.
        float addSpeed = wishSpd - currentSpeed;
        if (addSpeed <= 0)
        {
            return;
        }

        // Determine speed after acceleration.
        float accelSpeed = accel * wishSpeed;

        // Cap acceleration speed.
        accelSpeed = Mathf.Min(accelSpeed, addSpeed);

        // Adjust player velocity.
        rb.velocity += accelSpeed * wishDir;
    }

    void ClampVelocity()
    {
        // We need the y value later as we're not clamping that.
        Vector3 velocity = rb.velocity;

        // Clamp horizontal movement.
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        float movementSpeed = 8.0f;
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, movementSpeed);

        // Apply clamped x and z velocity.
        rb.velocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f))
        {
            // Beware, a 35 degree angle may be considered 34.999.
            if (Vector3.Angle(Vector3.up, hit.normal) < 35f)
            {
                return true;
            }
        }
        return false;
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
