using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text amountText;

    [Header("Pop-Out Panels")]
    [SerializeField] private GameObject betPanel;
    [SerializeField] private GameObject restartPanel;

    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Economy Values")]
    public int playerBalance = 1000;
    public int currentBet = 10;
    public int minBet = 10;

    // FEATURE: Initial Game State Auto-Popup
    private void Start()
    {
        if (gameManager == null) gameManager = FindFirstObjectByType<GameManager>();

        currentBet = 10;
        if (statusText != null) statusText.text = "SELECT BET OR PULL LEVER";

        if (restartPanel != null) restartPanel.SetActive(false);

        UpdateUI();
        OpenBetPanel();
    }

    public void OpenBetPanel()
    {
        if (gameManager != null && !gameManager.CanSpin) return;

        if (playerBalance < minBet)
        {
            CheckOutOfGold();
            return;
        }

        if (betPanel != null)
        {
            betPanel.SetActive(true);
        }
    }

    public void ToggleBetPanel()
    {
        if (gameManager != null && !gameManager.CanSpin) return;

        if (betPanel != null)
        {
            betPanel.SetActive(!betPanel.activeSelf);
        }
    }

    public void CloseBetPanel()
    {
        if (betPanel != null)
        {
            betPanel.SetActive(false);
        }
    }

    public void SetBet(int amount)
    {
        currentBet = amount;
        UpdateUI();
        CloseBetPanel();
    }

    // FEATURE: Transaction Validation & Bet Locking
    public bool TryDeductBet()
    {
        if (playerBalance >= currentBet)
        {
            playerBalance -= currentBet;
            if (statusText != null) statusText.text = "SPINNING...";

            CloseBetPanel();
            UpdateUI();
            return true;
        }

        if (playerBalance < minBet)
        {
            CheckOutOfGold();
        }
        else
        {
            if (statusText != null) statusText.text = "LOWER YOUR BET!";
            OpenBetPanel();
        }

        return false;
    }

    // FEATURE: Post-Spin Workflow 
    public void ShowResult(bool won, int winAmount)
    {
        if (won)
        {
            playerBalance += winAmount;
            if (statusText != null) statusText.text = $"JACKPOT! WON {winAmount} G!";
        }
        else
        {
            if (statusText != null) statusText.text = "TRY AGAIN!";
        }

        UpdateUI();

        if (playerBalance < minBet)
        {
            CheckOutOfGold();
        }
        else
        {
            OpenBetPanel();
        }
    }

    // FEATURE:  Game Over State Handler
    private void CheckOutOfGold()
    {
        if (statusText != null) statusText.text = "OUT OF GOLD! RESTART TO PLAY";

        CloseBetPanel();

        if (restartPanel != null)
        {
            restartPanel.SetActive(true);
        }
    }

    private void UpdateUI()
    {
        if (amountText != null)
        {
            amountText.text = $"GOLD: {playerBalance} G\nBET: {currentBet} G";
        }
    }

    // FEATURE: WebGL-Safe Asynchronous Scene Reloading
    public void RestartGame()
    {
        // Using Async prevents the single-threaded WebGL player from freezing during reloads
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    
}