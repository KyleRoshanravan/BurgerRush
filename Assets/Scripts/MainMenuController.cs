using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject playButton;
    public Animator titleAnimator;
    public float delayBeforeShow = 3f;

    void Start()
    {
        playButton.SetActive(false);
        if (titleAnimator != null)
            titleAnimator.SetTrigger("Play");
        Invoke(nameof(ShowPlayButton), delayBeforeShow);
    }

    void ShowPlayButton()
    {
        playButton.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("RestaurantScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
