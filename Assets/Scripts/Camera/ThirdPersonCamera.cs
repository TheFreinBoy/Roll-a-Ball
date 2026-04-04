using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // игрок

    [Header("Настройки дистанции")]
    public float distance = 5f;        
    public float minDistance = 5.0f;   
    public float maxDistance = 12f; // Увеличил максимальную дистанцию, чтобы было куда отдалять
    
    [Header("Настройки зума")]
    public float zoomSensitivity = 5f; // Чувствительность колесика мыши
    
    public Vector3 targetOffset = new Vector3(0f, 0.5f, 0f);

    [Header("Настройки мыши")]
    public float mouseSensitivity = 200f;
    public float minY = -40f;
    public float maxY = 80f;

    [Header("Настройки столкновений (SphereCast)")]
    public LayerMask obstacleLayers; 
    public float cameraRadius = 0.3f; 
    public float cameraCollisionOffset = 0.1f; 

    [Header("Сглаживание")]
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
        // 1. Управление вращением
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        // 2. Управление зумом (колесико мыши)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            // Вычитаем, потому что кручение колесика вперед дает положительное значение,
            // и мы хотим, чтобы дистанция уменьшалась (камера приближалась)
            distance -= scroll * zoomSensitivity;
            // Ограничиваем дистанцию в заданных пределах
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 focusPoint = target.position + targetOffset;
        
        Vector3 desiredPosition = focusPoint - (rotation * Vector3.forward * distance);
        Vector3 directionToCamera = (desiredPosition - focusPoint).normalized;

        float desiredDistance = distance;
        RaycastHit hit;
        
        // 3. Проверка столкновений
        if (Physics.SphereCast(focusPoint, cameraRadius, directionToCamera, out hit, distance, obstacleLayers))
        {
            desiredDistance = hit.distance - cameraCollisionOffset;
        }

        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        currentDistance = Mathf.SmoothDamp(currentDistance, desiredDistance, ref distanceSmoothVelocity, smoothTime);

        // 4. Применение позиции и поворота
        transform.position = focusPoint - (rotation * Vector3.forward * currentDistance);
        transform.LookAt(focusPoint);
    }
}