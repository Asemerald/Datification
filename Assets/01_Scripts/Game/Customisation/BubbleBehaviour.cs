using System.Collections;
using Game;
using UnityEditor.U2D;
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
    private float moveSpeed = 0.5f; // Vitesse du mouvement aléatoire
    private float moveRange = 50f;  // Distance max autour du spawn
    private bool isDragging = false;
    private Image _bubbleImage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Assigner l'icône de la pièce si un Image est présent dans l'enfant
        _bubbleImage = GetComponent<Image>();
        
        if (carPartData != null && _bubbleImage != null)
        {
            _bubbleImage.sprite = carPartData.icon; // Appliquer l'icône du scriptable
        }

        // Sauvegarde la position initiale pour le mouvement aléatoire
        initialPosition = rectTransform.anchoredPosition;
        StartCoroutine(FloatAround());
    }

    private IEnumerator FloatAround()
    {
        while (true)
        {
            if (!isDragging) // Ne pas bouger quand on est en train de drag
            {
                targetPosition = initialPosition + new Vector3(
                    Random.Range(-moveRange, moveRange), 
                    Random.Range(-moveRange, moveRange), 
                    0
                );

                // Clamp la position pour éviter de sortir de l'écran
                targetPosition.x = Mathf.Clamp(targetPosition.x, -canvas.pixelRect.width / 2, canvas.pixelRect.width / 2);
                targetPosition.y = Mathf.Clamp(targetPosition.y, -canvas.pixelRect.height * 0.35f, canvas.pixelRect.height / 2);
            }

            float elapsedTime = 0f;
            Vector3 startPosition = rectTransform.anchoredPosition;

            while (elapsedTime < 2f && !isDragging) // Animation sur 2 secondes
            {
                rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 2f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f)); // Pause avant de choisir un nouveau mouvement
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

        // Vérifier si on lâche la bulle sur la voiture
        if (IsDroppedOnCar(eventData))
        {
            Debug.Log($"Bulle {carPartData.name} déposée sur la voiture !");
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
        // TODO: Ajouter l'application de la pièce sur la voiture
    }
}
