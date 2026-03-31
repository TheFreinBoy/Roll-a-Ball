using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float jumpForce = 5f;
    
    [Header("Camera Reference")]
    public Transform cameraTransform; // Ссылка на трансформ камеры

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Если камеру забыли назначить в инспекторе, скрипт найдет главную камеру сам
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); 
        float moveVertical = Input.GetAxis("Vertical");    

        // Получаем векторы направления камеры
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Обнуляем ось Y, чтобы шар двигался только в горизонтальной плоскости
        camForward.y = 0f;
        camRight.y = 0f;

        // Нормализуем векторы, чтобы скорость не менялась при разном угле наклона камеры
        camForward.Normalize();
        camRight.Normalize();

        // Высчитываем итоговый вектор движения относительно камеры
        Vector3 movement = (camForward * moveVertical) + (camRight * moveHorizontal);

        rb.AddForce(movement * speed);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; 
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Простая проверка на землю (в идеале стоит проверять теги или слои, 
        // чтобы шар не мог прыгать от стен)
        isGrounded = true;
    }
}