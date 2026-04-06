using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;
using TMPro;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class SphereController : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    
    [Header("Movement")]
    public float speed = 40f;      
    public float maxSpeed = 10f;   
    public float jumpForce = 5f;

    [Header("Boost Settings")]
    public float maxSpeedBoost = 5f; 
    public float speedAccelerationBoost = 20f; 
    
    [Header("Audio Sounds")]
    public AudioClip coinSound;
    [Range(0f, 1f)] public float coinVolume = 0.5f; 
    
    public AudioClip potionSound;
    [Range(0f, 1f)] public float potionVolume = 1.0f; 

    [Header("Camera Reference")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private AudioSource audioSource;
    private bool isGrounded;
    
    private Vector2 moveInput;
    private int count;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); 
        
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
            gameObject.SetActive(false); 
            
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TMPro.TextMeshProUGUI>().text = "You Lose!";
            

            FindObjectOfType<MainMenu>().GameOver();
        }
    }
    
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            if (coinSound != null)
            {
                audioSource.PlayOneShot(coinSound, coinVolume); 
            }
            
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("SpeedPotion"))
        {
            if (potionSound != null)
            {
                audioSource.PlayOneShot(potionSound, potionVolume); 
            }
            
            other.gameObject.SetActive(false); 
            maxSpeed += maxSpeedBoost;
            speed += speedAccelerationBoost;
        }
    }
    
    void SetCountText() 
    {
        countText.text = "" + count.ToString();
        if (count >= 13) 
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            StartCoroutine(HideWinTextRoutine());
        }
    }
    private IEnumerator HideWinTextRoutine()
    {
        yield return new WaitForSeconds(3f);
        
        winTextObject.SetActive(false);
    }
}