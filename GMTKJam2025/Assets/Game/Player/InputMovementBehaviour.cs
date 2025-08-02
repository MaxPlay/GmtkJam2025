using UnityEngine;
using UnityEngine.InputSystem;

public class InputMovementBehaviour : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction;
    [SerializeField] private Transform rotator;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float acceleartion = 5f;
    private Rigidbody rb;

    private Vector3 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void UpdateMovement(float deltaTime)
    {
        Vector2 movementInput = moveAction.action.ReadValue<Vector2>();
        velocity = Vector3.Lerp(velocity, new Vector3(movementInput.x, 0, movementInput.y) * movementSpeed, acceleartion * deltaTime);
        if (movementInput != Vector2.zero)
            rotator.rotation = Quaternion.Slerp(rotator.rotation, Quaternion.LookRotation(new Vector3(movementInput.x, 0, movementInput.y), Vector3.up), rotationSpeed * deltaTime);
        rb.MovePosition(transform.position + velocity * deltaTime);
        //transform.Translate(new Vector3(velocity.x, 0, velocity.y) * deltaTime * movementSpeed);
    }
}
