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

    // Required by the IPointerClickHandler interface. 
    // Intercepts left-clicks directly on the UI Image element, allowing it to act like a button.
    public void OnPointerClick(PointerEventData eventData)
    {
        Pull();
    }

    // Validates that the lever isn't already being pulled and that the game manager is ready to accept a spin request.
    public void Pull()
    {
        if (_isPulling || gameManager == null || !gameManager.CanSpin) return;
        StartCoroutine(PullRoutine());
    }

    // Simulates the physical action of pulling a slot machine lever.
    // Swaps the UI sprite to a "pulled down" state, triggers the game logic, waits, and resets the sprite to its idle state.
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