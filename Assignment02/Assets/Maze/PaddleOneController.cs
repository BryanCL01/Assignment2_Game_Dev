using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle1Controller : MonoBehaviour
{
    public InputActionReference paddle1MovementAction; // Reference to Paddle1 movement Input Action
    public float speed = 5.0f; // Speed at which the paddle moves
    public float paddleLength = 1.0f; // Half of the paddle length, adjust based on actual size
    public float boundaryLimit = 4.0f; // Boundary limit for paddle movement

    private Vector2 moveInput = Vector2.zero;

    private void OnEnable()
    {
        paddle1MovementAction.action.Enable();
        paddle1MovementAction.action.performed += OnMoveInput;
        paddle1MovementAction.action.canceled += OnMoveInputCanceled;
    }

    private void OnDisable()
    {
        paddle1MovementAction.action.performed -= OnMoveInput;
        paddle1MovementAction.action.canceled -= OnMoveInputCanceled;
        paddle1MovementAction.action.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move Input Performed: " + moveInput);
    }

    private void OnMoveInputCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        Debug.Log("Movement Canceled");
    }

    private void Update()
    {
        // Calculate movement and update paddle position along the Z-axis
        Vector3 currentPosition = transform.position;
        float newZ = currentPosition.z + moveInput.y * speed * Time.deltaTime;

        // Clamp the movement to prevent the paddle from going out of bounds
        newZ = Mathf.Clamp(newZ, -boundaryLimit + paddleLength, boundaryLimit - paddleLength);

        // Update the position of the paddle
        transform.position = new Vector3(currentPosition.x, currentPosition.y, newZ);
    }
}
