using UnityEngine;
using UnityEngine.SceneManagement; // Обязательно добавляем эту строку для работы со сценами

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Minigame"); 
        
        // Альтернативный вариант - загрузка по индексу (сцена 1 в Build Settings):
        // SceneManager.LoadScene(1);
    }


    public void QuitGame()
    {
        #if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
    }
}