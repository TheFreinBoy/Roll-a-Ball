using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // игрок

    public float distance = 5f;
    public float mouseSensitivity = 200f;

    public float minY = -40f;
    public float maxY = 80f;

    float currentX = 0f;
    float currentY = 20f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // чтобы не смотрела вниз при старте
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, minY, maxY);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        Vector3 position = target.position - (rotation * Vector3.forward * distance);

        transform.position = position;
        transform.LookAt(target);
    }
}