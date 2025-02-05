using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public partial class GameManager
    {
        public NetworkVariable<bool> rightPlayerInput = new NetworkVariable<bool>();
        public NetworkVariable<bool> leftPlayerInput = new NetworkVariable<bool>();

        [ServerRpc (RequireOwnership = false)]
        public void SetInputServerRpc(bool inputValue, bool hasRightCar)
        {
            if (hasRightCar)
            {
                rightPlayerInput.Value = inputValue;
            }
            else
            {
                leftPlayerInput.Value = inputValue;
            }
        }


    }
}