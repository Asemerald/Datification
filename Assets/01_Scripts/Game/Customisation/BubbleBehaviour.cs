using System.Collections;
using Game;
using Game.Customisation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BubbleBehaviour : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public CarPartScriptable carPartData;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float moveRange = 80f;  // Distance max autour du spawn
    private bool isDragging = false;
    private Image _bubbleImage;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        _bubbleImage = GetComponent<Image>();

        if (carPartData != null && _bubbleImage != null)
        {
            _bubbleImage.sprite = carPartData.icon; // Appliquer l'icône du scriptable
        }

        StartCoroutine(InitializePositionAndStartFloating());
    }

    private IEnumerator InitializePositionAndStartFloating()
    {
        yield return null; // Wait for one frame to let Unity apply the spawned position
        initialPosition = rectTransform.anchoredPosition; // Now we set it correctly
        StartCoroutine(FloatAround());
    }


    private IEnumerator FloatAround()
    {
        while (this != null) // Ensure the object exists
        {
            if (!isDragging)
            {
                targetPosition = initialPosition + new Vector3(
                    Random.Range(-moveRange, moveRange), 
                    Random.Range(-moveRange / 2f, moveRange / 2f), 
                    0
                );

                float elapsedTime = 0f;
                Vector3 startPosition = rectTransform.anchoredPosition;

                while (elapsedTime < 2f && !isDragging)
                {
                    rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 2f);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            }
            else
            {
                yield return null;
            }
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (IsDroppedOnCar(eventData))
        {
            Debug.Log($"Bulle {carPartData.name} déposée sur la voiture !");
            
            CustomisationManager.Instance.SetLastCarPart(carPartData);
            GameManager.Instance.SpawnSingleBubble(CustomisationManager.Instance.LastCarPartScriptable);
            
            ApplyCarPart();
            
            Destroy(gameObject);
        }
    }

    private bool IsDroppedOnCar(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"Hit object: {hit.collider.gameObject.name}");

            if (hit.collider.CompareTag("Car"))
            {
                return true;
            }
        }
        return false;
    }

    private void ApplyCarPart()
    {
        Debug.Log($"Appliquer {carPartData.name} à la voiture !");
        GameManager.Instance.car.ChangeCarPart(carPartData);
    }
}
