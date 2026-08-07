using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image[] displaySlots;

    [Header("Animation Settings")]
    [SerializeField] private float spinSpeed = 2500f;
    [SerializeField] private float bounceOvershoot = 20f;
    [SerializeField] private float symbolHeight = 96f;

    [Header("Symbol Pool")]
    [SerializeField] private List<SymbolData> symbolPool;

    private RectTransform _rectTransform;
    private Vector2 _startPosition;

    public bool IsSpinning { get; private set; }
    public SymbolData FinalSymbol { get; private set; }

    // Caches the RectTransform and its original layout position for the animation math to reference later.
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startPosition = _rectTransform.anchoredPosition;
    }

    // Acts as the entry point for spinning. Predetermines the winning symbol immediately upon starting, 
    // so the visual spinning is just an illusion catching up to the generated result.
    public void StartSpin(float duration)
    {
        if (!IsSpinning && symbolPool != null && symbolPool.Count > 0)
        {
            FinalSymbol = symbolPool[Random.Range(0, symbolPool.Count)];
            StartCoroutine(SmoothSpinRoutine(duration));
        }
    }

    // Core animation loop for the reel. 
    // Part 1: Moves the reel downwards based on time and speed, snapping it back up to create an infinite loop effect.
    // Part 2: Injects the pre-calculated 'FinalSymbol' into the center slot right before stopping.
    // Part 3: Applies a mathematical lerp to create an elastic overshoot "bounce" effect when settling into place.
    private IEnumerator SmoothSpinRoutine(float duration)
    {
        IsSpinning = true;
        float elapsed = 0f;
        float currentY = _startPosition.y;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentY -= spinSpeed * Time.deltaTime;

            if (currentY <= _startPosition.y - symbolHeight)
            {
                currentY += symbolHeight;
                CycleSymbols();
            }

            _rectTransform.anchoredPosition = new Vector2(_startPosition.x, currentY);
            yield return null;
        }

        if (displaySlots.Length > 1 && FinalSymbol != null)
        {
            displaySlots[1].sprite = FinalSymbol.icon;
        }

        float bounceDuration = 0.12f;
        float bounceElapsed = 0f;
        float startBounceY = _rectTransform.anchoredPosition.y;
        float overshootY = _startPosition.y - bounceOvershoot;

        while (bounceElapsed < bounceDuration / 2f)
        {
            bounceElapsed += Time.deltaTime;
            _rectTransform.anchoredPosition = new Vector2(_startPosition.x, Mathf.Lerp(startBounceY, overshootY, bounceElapsed / (bounceDuration / 2f)));
            yield return null;
        }

        bounceElapsed = 0f;
        while (bounceElapsed < bounceDuration / 2f)
        {
            bounceElapsed += Time.deltaTime;
            _rectTransform.anchoredPosition = new Vector2(_startPosition.x, Mathf.Lerp(overshootY, _startPosition.y, bounceElapsed / (bounceDuration / 2f)));
            yield return null;
        }

        _rectTransform.anchoredPosition = _startPosition;
        IsSpinning = false;
    }

    // Visually shifts the sprites down the array list (top to bottom) as the reel physically moves down.
    // Pulls a random sprite from the pool for the newly exposed top slot to maintain the infinite scrolling illusion.
    private void CycleSymbols()
    {
        for (int i = displaySlots.Length - 1; i > 0; i--)
        {
            displaySlots[i].sprite = displaySlots[i - 1].sprite;
        }
        if (symbolPool != null && symbolPool.Count > 0)
        {
            displaySlots[0].sprite = symbolPool[Random.Range(0, symbolPool.Count)].icon;
        }
    }
}