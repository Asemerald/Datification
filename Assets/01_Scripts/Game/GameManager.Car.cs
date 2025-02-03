using Game.Customisation;
using Game;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public partial class GameManager
    {

        private void SpawnCar(bool rightCar)
        {
            // Instantiate and spawn my caar depending on hasRightCar, also give ownership to the one who spawned it
            if (rightCar)
            {
                NetworkManager.SpawnManager.InstantiateAndSpawn(carRightPrefab, 0);
                NetworkManager.SpawnManager.InstantiateAndSpawn(carLeftPrefab, 1);
            }
            else
            {
                NetworkManager.SpawnManager.InstantiateAndSpawn(carRightPrefab, 1);
                NetworkManager.SpawnManager.InstantiateAndSpawn(carLeftPrefab, 0);
            }
        }
        
    }
}
