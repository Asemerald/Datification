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

        private bool _isTouchingCar = false; // To track if the touch is on the car

        [SerializeField] private GameObject carrosserie;
        [SerializeField] private GameObject roues;
        [SerializeField] private GameObject phares;
        [SerializeField] private GameObject accessoires;

        void Update()
        {
            HandleDrag();
            HandlePinchToZoom();
        }

        #region Touch Controls
        void HandleDrag()
        {
            if (Input.touchCount == 1) // One finger for dragging
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));

                if (touch.phase == TouchPhase.Began)
                {
                    // Perform a raycast to check if we hit the car
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == transform) // Check if the touched object is this car
                        {
                            _isTouchingCar = true;
                            _dragOffset = transform.position - touchPosition; // Store the offset to prevent snapping
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Moved && _isTouchingCar)
                {
                    float newX = Mathf.Clamp(touchPosition.x + _dragOffset.x, _minX, _maxX); // Apply offset
                    transform.position = new Vector3(newX, transform.position.y, transform.position.z);
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _isTouchingCar = false; // Reset when the finger is lifted
                }
            }
        }

        void HandlePinchToZoom()
        {
            if (Input.touchCount == 2) // Two fingers for scaling
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (!_isTouchingCar) return; // Ensure zooming happens only if touching the car

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
        
        #endregion

        #region Change Skins

        public void ChangeCarPart(CarPartScriptable carPart)
        {
            switch (carPart.type)
            {
                case CarPartScriptable.CarPartType.Carrosserie:
                    ChangeCarrosserie(carPart);
                    break;
                case CarPartScriptable.CarPartType.Roues:
                    ChangeRoues(carPart);
                    break;
                case CarPartScriptable.CarPartType.Phares:
                    ChangePhares(carPart);
                    break;
                case CarPartScriptable.CarPartType.Accessoires:
                    ChangeAccessoires(carPart);
                    break;
            }
        }
        
        private void ChangeCarrosserie(CarPartScriptable carPart)
        {
            //delete all childs of carrosserie
            foreach (Transform child in carrosserie.transform)
            {
                Destroy(child.gameObject);
            }
            //instantiate new carrosserie
            GameObject newCarrosserie = Instantiate(GameManager.Instance.hasRightCar ? carPart.RightMesh : carPart.LeftMesh, carrosserie.transform);
        }
        
        private void ChangeRoues(CarPartScriptable carPart)
        {
            //delete all childs of roues
            foreach (Transform child in roues.transform)
            {
                Destroy(child.gameObject);
            }
            //instantiate new roues
            GameObject newRoues = Instantiate(GameManager.Instance.hasRightCar ? carPart.RightMesh : carPart.LeftMesh, roues.transform);
        }
        
        private void ChangePhares(CarPartScriptable carPart)
        {
            //delete all childs of phares
            foreach (Transform child in phares.transform)
            {
                Destroy(child.gameObject);
            }
            //instantiate new phares
            GameObject newPhares = Instantiate(GameManager.Instance.hasRightCar ? carPart.RightMesh : carPart.LeftMesh, phares.transform);
        }
        
        private void ChangeAccessoires(CarPartScriptable carPart)
        {
            //delete all childs of accessoires
            foreach (Transform child in accessoires.transform)
            {
                Destroy(child.gameObject);
            }
            //instantiate new accessoires
            GameObject newAccessoires = Instantiate(GameManager.Instance.hasRightCar ? carPart.RightMesh : carPart.LeftMesh, accessoires.transform);
        }

        #endregion
    }
}
