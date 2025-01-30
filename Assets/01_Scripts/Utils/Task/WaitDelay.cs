using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class WaitDelay : InstanceBase<WaitDelay>
    {
        /// <summary>
        ///  Activate the gameObject
        /// </summary>
        /// <param name="gameObject">GameObject to activate</param>
        public void Show(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        
        public async Task<Task> WaitFor(float delay)
        {
            await Task.Delay((int) (delay * 1000));
            return Task.CompletedTask;
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