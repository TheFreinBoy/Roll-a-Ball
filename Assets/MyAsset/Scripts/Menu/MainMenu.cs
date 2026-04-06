using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class MainMenu : MonoBehaviour
{
    [Header("interfaces")]
    public GameObject menuPanel; 
    public GameObject hudPanel;  
    public GameObject settingsPanel; 
    public GameObject pausePanel; 
    public GameObject loseText;

    [Header("Cameras")]
    public Camera menuCamera;
    public Camera mainCamera; 

    [Header("Player & Enemy")]
    public SphereController playerMovement; 
    public GameObject enemy;
    

    [Header("Music")]
    public AudioSource menuMusic; 
    public AudioSource gameMusic; 

    private bool isGameStarted = false; 
    private bool isPaused = false;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        menuPanel.SetActive(true);
        hudPanel.SetActive(false);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false); 
        
        menuCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        playerMovement.enabled = false;
        // Enemy disactivation
        if (enemy != null) 
        {
            enemy.SetActive(false); 
        }
        
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame(); 
            }
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        isGameStarted = true;
        isPaused = false;

        menuPanel.SetActive(false);
        hudPanel.SetActive(true);

        menuCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        menuMusic.Stop(); 
        gameMusic.Play(); 

        // Enemy activation
        if (enemy != null) 
        {
            enemy.SetActive(true); 
        }

        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; 

        pausePanel.SetActive(true); 
        hudPanel.SetActive(false);  

        playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; 

        pausePanel.SetActive(false);
        settingsPanel.SetActive(false); 
        hudPanel.SetActive(true);

        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }


    public void OpenSettings()
    {
        menuPanel.SetActive(false);
        pausePanel.SetActive(false); 
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        if (isPaused)
        {
            pausePanel.SetActive(true); 
        }
        else
        {
            menuPanel.SetActive(true); 
        }
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); 
        #endif
    }
    public void GameOver()
    {
        loseText.SetActive(true); 
        
        StartCoroutine(WaitAndLoadMenu());
    }
    private IEnumerator WaitAndLoadMenu()
    {

        yield return new WaitForSecondsRealtime(2f); 
        
        loseText.SetActive(false); 

        LoadMainMenu(); 
    }
}