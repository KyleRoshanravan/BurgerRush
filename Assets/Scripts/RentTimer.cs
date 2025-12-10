using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class RentTimer : MonoBehaviour
{
    [Header("Player Data")]
    public PlayerData playerData;      // Drag your Player object here

    [Header("UI Elements")]
    public TextMeshProUGUI alertText;  // The text for alerts/game over

    [Header("Settings")]
    public float intervalMinutes = 3f;
    public float amountToDeduct = 20f;
    public float alertDuration = 3f;

    private float timer;
    private bool isGameOver = false;

    void Start()
    {
        timer = intervalMinutes * 60f;

        if (alertText != null)
            alertText.gameObject.SetActive(false);

        // Ensure game runs at normal speed
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver)
        {
            // Only allow R to restart
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f; // restore normal time
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        timer -= Time.unscaledDeltaTime; // use unscaled time so timer works even if paused

        if (timer <= 0f)
        {
            ChargePlayer();
            timer = intervalMinutes * 60f;
        }
    }

    void ChargePlayer()
    {
        if (playerData == null)
        {
            Debug.LogError("RentTimer: PlayerData not assigned!");
            return;
        }

        bool success = playerData.TrySpend(amountToDeduct);

        if (!success)
        {
            ShowGameOver("You couldn’t pay the $20 rent.");
            return;
        }

        ShowAlert("Rent paid: $20");
    }

    // --------------------------------------------------------
    // NORMAL ALERT
    // --------------------------------------------------------
    void ShowAlert(string message)
    {
        if (alertText == null) return;

        StopAllCoroutines();
        alertText.text = message;
        alertText.gameObject.SetActive(true);
        StartCoroutine(HideAlertAfterSeconds(alertDuration));
    }

    private IEnumerator HideAlertAfterSeconds(float duration)
    {
        yield return new WaitForSecondsRealtime(duration); // use real time
        if (alertText != null)
            alertText.gameObject.SetActive(false);
    }

    // --------------------------------------------------------
    // GAME OVER
    // --------------------------------------------------------
    void ShowGameOver(string reason)
    {
        if (alertText == null) return;

        isGameOver = true;

        StopAllCoroutines();

        alertText.text =
            $"<size=70><b>GAME OVER</b></size>\n\n" +
            reason +
            "\n\nPress <b>R</b> to restart";

        alertText.gameObject.SetActive(true);

        // Freeze the game
        Time.timeScale = 0f;

        // Disable player controls
        PlayerInteractor interactor = FindObjectOfType<PlayerInteractor>();
        if (interactor != null)
            interactor.enabled = false;
    }
}
