using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; 

    [Header("Distance Settings")]
    public float distance = 5f;        
    public float minDistance = 5.0f;   
    public float maxDistance = 12f; 
    
    [Header("Zoom Settings")]
    public float zoomSensitivity = 5f; 
    
    public Vector3 targetOffset = new Vector3(0f, 0.5f, 0f);

    [Header("Mouse Settings")]
    public float mouseSensitivity = 200f;
    public float minY = -40f;
    public float maxY = 80f;

    [Header("Collision Settings")]
    public LayerMask obstacleLayers; 
    public float cameraRadius = 0.3f; 
    public float cameraCollisionOffset = 0.1f; 

    [Header("Smoothing")]
    public float smoothTime = 0.1f;

    private float currentX = 0f;
    private float currentY = 20f;
    private float currentDistance;
    private float distanceSmoothVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

        currentDistance = distance;
    }

    void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {

            distance -= scroll * zoomSensitivity;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 focusPoint = target.position + targetOffset;
        
        Vector3 desiredPosition = focusPoint - (rotation * Vector3.forward * distance);
        Vector3 directionToCamera = (desiredPosition - focusPoint).normalized;

        float desiredDistance = distance;
        RaycastHit hit;
        
        if (Physics.SphereCast(focusPoint, cameraRadius, directionToCamera, out hit, distance, obstacleLayers))
        {
            desiredDistance = hit.distance - cameraCollisionOffset;
        }

        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        currentDistance = Mathf.SmoothDamp(currentDistance, desiredDistance, ref distanceSmoothVelocity, smoothTime);

        transform.position = focusPoint - (rotation * Vector3.forward * currentDistance);
        transform.LookAt(focusPoint);
    }
}