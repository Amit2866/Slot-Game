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

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startPosition = _rectTransform.anchoredPosition;
    }

    public void StartSpin(float duration)
    {
        if (!IsSpinning && symbolPool != null && symbolPool.Count > 0)
        {
            FinalSymbol = symbolPool[Random.Range(0, symbolPool.Count)];
            StartCoroutine(SmoothSpinRoutine(duration));
        }
    }

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

        // Bounce overshoot effect
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