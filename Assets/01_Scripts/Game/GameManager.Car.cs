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
            var carGameObject = Instantiate(rightCar ? carRightPrefab : carLeftPrefab);
            car = carGameObject.GetComponent<CarCustomControl>();
        }
        
    }
}
