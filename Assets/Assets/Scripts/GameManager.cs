using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Reel[] reels;
    [SerializeField] private UIManager uiManager;

    [Header("Fallback Settings")]
    [SerializeField] private int defaultMultiplier = 5;

    private bool _isSpinning = false;
    public bool CanSpin => !_isSpinning;

    // Checks if the machine is already active. If not, it pings the UIManager to pay for the spin.
    // Only starts the mechanical sequence if the transaction is successful.
    public void TrySpin()
    {
        if (_isSpinning) return;

        if (uiManager != null && uiManager.TryDeductBet())
        {
            StartCoroutine(RunSpinSequence());
        }
    }

    // Polls the New Input System every frame to allow spinning via a keyboard hotkey (G).
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame && CanSpin)
        {
            TrySpin();
        }
    }

    // Orchestrates the physical reel spinning.
    // Assigns incrementally longer spin durations to each reel to create a staggered, dramatic stopping effect.
    // Uses a polling loop to wait until all reels have completely stopped before checking for a win.
    private IEnumerator RunSpinSequence()
    {
        _isSpinning = true;

        for (int i = 0; i < reels.Length; i++)
        {
            float duration = 1.2f + (i * 0.4f);
            reels[i].StartSpin(duration);
        }

        bool anyReelSpinning = true;
        while (anyReelSpinning)
        {
            anyReelSpinning = false;
            foreach (var reel in reels)
            {
                if (reel.IsSpinning) anyReelSpinning = true;
            }
            yield return null;
        }

        _isSpinning = false;

        CheckWinCondition();
    }

    // Evaluates the final outcome after all reels rest.
    // Because we use ScriptableObjects, we can verify a win by simply checking if the objects in memory match,
    // rather than doing slow string or tag comparisons. Grabs the specific multiplier from the winning object.
    private void CheckWinCondition()
    {
        if (reels.Length < 3) return;

        SymbolData sym1 = reels[0].FinalSymbol;
        SymbolData sym2 = reels[1].FinalSymbol;
        SymbolData sym3 = reels[2].FinalSymbol;

        if (sym1 != null && sym1 == sym2 && sym2 == sym3)
        {
            int multiplier = sym1.payoutMultiplier > 0 ? sym1.payoutMultiplier : defaultMultiplier;
            int winAmount = uiManager.currentBet * multiplier;
            uiManager.ShowResult(true, winAmount);
        }
        else
        {
            uiManager.ShowResult(false, 0);
        }
    }
}