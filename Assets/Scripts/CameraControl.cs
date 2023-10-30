using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float speed = 150.0f;
    private float rotationSpeed = 60.0f;
    private bool allowYRotation = false;

    private void Start()
    {
        // Setting the initial position and rotation
        transform.position = new Vector3(0, 28, 0);
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        // Movement controls
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalMovement, 0.0f, verticalMovement);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // Toggle Y rotation with ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            allowYRotation = !allowYRotation;
        }

        // Rotate Y-axis based on mouse movement
        if (allowYRotation)
        {
            float mouseX = Input.GetAxis("Mouse X");
            Quaternion newRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + mouseX * rotationSpeed * Time.deltaTime, transform.eulerAngles.z);
            transform.rotation = newRotation;
        }
    }
}
