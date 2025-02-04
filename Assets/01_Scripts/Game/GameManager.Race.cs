using UnityEngine;

namespace Game
{
    public partial class GameManager
    {
        
        
        private void SpawnRace()
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                ServerBehaviour.Instance.racePrefab.SetActive(true);
                ServerBehaviour.Instance.showReelPrefab.SetActive(false);
            });
        }
    }
}