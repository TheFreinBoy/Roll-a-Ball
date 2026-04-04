using UnityEngine;
using UnityEngine.InputSystem; 
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class SphereController : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    
    [Header("Movement")]
    public float speed = 40f;      // Силу можно сделать больше для резкого старта
    public float maxSpeed = 10f;   // Ограничитель скорости (чтобы не улетал в космос)
    public float jumpForce = 5f;
    
    [Header("Camera Reference")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isGrounded;
    
    private Vector2 moveInput;
    private int count;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; 

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; 
        }
    }

    void FixedUpdate()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * moveInput.y) + (camRight * moveInput.x);

        rb.AddForce(movement * speed);

        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); 
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
    
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }
    
    void SetCountText() 
    {
        countText.text = "" + count.ToString();
        if (count >= 13) 
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }
}