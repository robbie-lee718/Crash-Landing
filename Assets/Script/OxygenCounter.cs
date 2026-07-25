using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OxygenCounter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI oxygenText;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [Header("Oxygen Settings")]
    [SerializeField, Range(0f, 100f)] private float oxygenPercentage = 100f;
    [SerializeField] private float oxygenDecreaseAmount = 10f;
    [SerializeField] private float countdownInterval = 2f;

    private float countdownTimer;
    private bool gameOver;

    private void Start()
    {
        Time.timeScale = 1f;
        UpdateOxygenDisplay();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameOver)
        {
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            return;
        }

        countdownTimer += Time.deltaTime;

        if (countdownTimer >= countdownInterval)
        {
            countdownTimer = 0f;
            oxygenPercentage = Mathf.Max(0f, oxygenPercentage - oxygenDecreaseAmount);
            UpdateOxygenDisplay();

            if (oxygenPercentage <= 0f)
            {
                TriggerGameOver();
            }
        }
    }

    private void UpdateOxygenDisplay()
    {
        if (oxygenText != null)
        {
            oxygenText.text = $"Oxygen: {Mathf.RoundToInt(oxygenPercentage)}%";
        }
    }

    private void TriggerGameOver()
    {
        gameOver = true;
        Time.timeScale = 0f;

        if (gameOverText != null)
        {
            gameOverText.text = "Oxygen Depleted\nPress Any Key to Restart";
            gameOverText.gameObject.SetActive(true);
        }
    }
}
