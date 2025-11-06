using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject playButton;   // Reference to the Play Button
    public float delayBeforeShow = 3f; // Time before button appears

    void Start()
    {
        // Hide button initially
        playButton.SetActive(false);
        // Call the function after a delay
        Invoke(nameof(ShowPlayButton), delayBeforeShow);
    }

    void ShowPlayButton()
    {
        playButton.SetActive(true);
    }

    public void PlayGame()
    {
        // Load your main game scene (make sure it's added to Build Settings)
        SceneManager.LoadScene("GameScene");
    }
}
