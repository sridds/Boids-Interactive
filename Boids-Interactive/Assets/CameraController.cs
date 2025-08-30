using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 move = Vector3.zero;

        if(Input.GetMouseButton(1))
        {
            transform.RotateAround(transform.position, transform.right, mouseY * -_rotationSpeed);
            transform.RotateAround(transform.position, Vector3.up, mouseX * _rotationSpeed);

            move = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
            move *= _moveSpeed * Time.deltaTime;
        }

        transform.Translate(move);
    }
}
