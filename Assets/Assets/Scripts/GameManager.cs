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

    public void TrySpin()
    {
        if (_isSpinning) return;

        if (uiManager != null && uiManager.TryDeductBet())
        {
            StartCoroutine(RunSpinSequence());
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame && CanSpin)
        {
            TrySpin();
        }
    }

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

        // Set spinning state false before win check so UI opens cleanly
        _isSpinning = false;

        CheckWinCondition();
    }

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