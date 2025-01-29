using UnityEngine;

namespace Game.Customisation
{
    public class CarCustomControl : MonoBehaviour
    {
        private Vector3 _startPosition;
        private Vector3 _dragOffset;
        private float _minX = -2.5f;
        private float _maxX = 2.5f;
    
        private float _minScale = 0.5f;
        private float _maxScale = 1.5f;
        private float _initialDistance;
        private Vector3 _initialScale;

        void Update()
        {
            HandleDrag();
            HandlePinchToZoom();
        }

        void HandleDrag()
        {
            if (Input.touchCount == 1) // One finger for dragging
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));

                if (touch.phase == TouchPhase.Began)
                {
                    _dragOffset = transform.position - touchPosition; // Store the offset to prevent snapping
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    float newX = Mathf.Clamp(touchPosition.x + _dragOffset.x, _minX, _maxX); // Apply offset
                    transform.position = new Vector3(newX, transform.position.y, transform.position.z);
                }
            }
        }

        void HandlePinchToZoom()
        {
            if (Input.touchCount == 2) // Two fingers for scaling
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    _initialDistance = Vector2.Distance(touch1.position, touch2.position);
                    _initialScale = transform.localScale;
                }
                else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                    float scaleFactor = currentDistance / _initialDistance;
                    Vector3 newScale = _initialScale * scaleFactor;

                    float clampedScale = Mathf.Clamp(newScale.x, _minScale, _maxScale);
                    transform.localScale = new Vector3(clampedScale, clampedScale, clampedScale);
                }
            }
        }
    }
}
