using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Lever : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image leverImage;

    [Header("Optional Sprite Swap")]
    [SerializeField] private Sprite leverUpSprite;
    [SerializeField] private Sprite leverDownSprite;

    [Header("Pull Motion")]
    [SerializeField] private float pullTime = 0.25f;

    private bool _isPulling = false;

    private void Start()
    {
        if (gameManager == null) gameManager = FindFirstObjectByType<GameManager>();
        if (leverImage == null) leverImage = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Pull();
    }

    public void Pull()
    {
        if (_isPulling || gameManager == null || !gameManager.CanSpin) return;
        StartCoroutine(PullRoutine());
    }

    private IEnumerator PullRoutine()
    {
        _isPulling = true;

        if (leverDownSprite != null && leverImage != null)
        {
            leverImage.sprite = leverDownSprite;
        }

        gameManager.TrySpin();

        yield return new WaitForSeconds(pullTime);

        if (leverUpSprite != null && leverImage != null)
        {
            leverImage.sprite = leverUpSprite;
        }

        _isPulling = false;
    }
}