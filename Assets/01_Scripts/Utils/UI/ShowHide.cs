using UnityEngine;

namespace Utils
{
    public class ShowHide : MonoBehaviour
    {
        /// <summary>
        ///  Activate the gameObject
        /// </summary>
        /// <param name="gameObject">GameObject to activate</param>
        public void Show(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Deactivate the gameObject
        /// </summary>
        /// <param name="gameObject">GameObject to deactivate</param>
        public void Hide(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}