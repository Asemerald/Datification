using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BubbleBehaviour : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Bubble Properties")]
    public float floatSpeed = 0.5f; // Vitesse du mouvement flottant
    public float floatRange = 20f; // Amplitude du flottement

    private RectTransform _rectTransform;
    private Vector2 _screenBounds;
    private Vector2 _startPosition;
    private Vector2 _randomDirection;
    private bool _isDragging = false;
    
    [Header("Car Part Data")]
    public CarPartScriptable carPartData;
    private Image _bubbleImage;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _bubbleImage = GetComponent<Image>();
        
        if (carPartData != null && _bubbleImage != null)
        {
            _bubbleImage.sprite = carPartData.icon; // Appliquer l'icône du scriptable
        }

        _screenBounds = new Vector2(Screen.width, Screen.height);
        _startPosition = _rectTransform.anchoredPosition;
        _randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        StartCoroutine(FloatBubble());
    }

    private IEnumerator FloatBubble()
    {
        while (true)
        {
            if (!_isDragging)
            {
                Vector2 movement = _randomDirection * (floatSpeed * Time.deltaTime * 100);
                _rectTransform.anchoredPosition += movement;

                // Rebondir si on touche les bords de l'écran
                if (_rectTransform.anchoredPosition.x < 0 || _rectTransform.anchoredPosition.x > _screenBounds.x)
                    _randomDirection.x *= -1;
                if (_rectTransform.anchoredPosition.y < 0 || _rectTransform.anchoredPosition.y > _screenBounds.y)
                    _randomDirection.y *= -1;
            }

            yield return null;
        }
    }

    // Détection du début du drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        transform.localScale = Vector3.one * 1.2f; // Agrandit la bulle quand elle est sélectionnée
    }

    // Déplacement de la bulle
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.position = eventData.position; // Suivi du doigt
    }

    // Fin du drag
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localScale = Vector3.one; // Retour à la taille normale
        _isDragging = false;

        // Vérifier si on l'a lâchée sur la voiture
        if (IsOverCar(eventData))
        {
            OnBubbleDropped();
        }
    }

    // Vérifie si la bulle est au-dessus de la voiture
    private bool IsOverCar(PointerEventData eventData)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = eventData.position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Car"))
            {
                return true;
            }
        }

        return false;
    }

    // Fonction appelée lorsqu'une bulle est déposée sur la voiture
    private void OnBubbleDropped()
    {
        Debug.Log($"Bubble {carPartData.name} applied to the car!");
        // TODO: Ajouter ici l'application de la pièce sur la voiture
    }
}
