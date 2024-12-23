using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement_Script : MonoBehaviour
{
    public float speed = 12f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float jumpHeight = 3f;
    public LayerMask groundMask;
    public CharacterController controller;

    Player_InputActions inputActions;
    InputAction movement;
    InputAction jump;
    InputAction reset;

    InputAction toggleCollision;
    bool isGrounded;
    bool isCollisionActive = true;

    Vector3 originalSpawnPoint;
    Vector3 velocity;
    void Awake()
    {
        inputActions = new Player_InputActions();
        originalSpawnPoint = transform.position;
    }
    void OnEnable()
    {
        movement = inputActions.Player.Movement;
        jump = inputActions.Player.Jump;
        toggleCollision = inputActions.Player.ToggleCollision;
        reset = inputActions.Player.Reset;

        movement.Enable();
        jump.Enable();
        toggleCollision.Enable();
        reset.Enable();

        jump.performed += DoJump;
        toggleCollision.performed += ToggleCollision;
        reset.performed += ResetPosition;
    }
    void OnDisable()
    {
        movement.Disable();
        jump.Disable();
        toggleCollision.Disable();
        reset.Disable();
    }
    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 v2 = movement.ReadValue<Vector2>();
        Vector3 move = transform.right * v2.x + transform.forward * v2.y;

        if (isCollisionActive)
        {
            if (controller.enabled)
            {
                controller.Move(move * speed * Time.deltaTime);
                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime);
            }
        }
        else
        {
            Vector3 newPosition = transform.position + (move * speed * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            if (controller.enabled)
            {
                controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
            }
            transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        }
    }

    void DoJump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ToggleCollision(InputAction.CallbackContext obj)
    {
        isCollisionActive = !isCollisionActive;
        controller.enabled = isCollisionActive;
    }

    void ResetPosition(InputAction.CallbackContext obj)
    {
        controller.enabled = false;
        transform.position = originalSpawnPoint;
        velocity = Vector3.zero;
        controller.enabled = true;
    }

}
