using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]


public class CustomPlayerInput : MonoBehaviour
{
    public Rigidbody body;
    Vector2 movement;

    void Reset()
    {
        body = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    void OnJump()
    {
        body.AddForce(Vector3.up * 10, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        body.AddForce(new Vector3(movement.x, 0, movement.y) * 2);
    }
}
