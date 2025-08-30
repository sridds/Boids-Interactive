using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            // Lock mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Apply rotation
            transform.RotateAround(transform.position, transform.right, mouseY * -_rotationSpeed);
            transform.RotateAround(transform.position, Vector3.up, mouseX * _rotationSpeed);

            // Calculate and apply movement
            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
            float speed = Input.GetKey(KeyCode.LeftShift) ? _moveSpeed * 2.0f : _moveSpeed;
            move *= speed * Time.deltaTime;

            transform.Translate(move);
        }

        // Unlock cursor if not moving
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
