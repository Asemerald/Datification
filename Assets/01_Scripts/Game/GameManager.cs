using Game.Customisation;
using Game;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace Game
{
    public partial class GameManager : NetworkInstanceBase<GameManager>
    {
    
        #region SerializeField
    
        [Header("Game Prefabs")]
        [SerializeField] private GameObject carLeftPrefab;
        [SerializeField] private GameObject carRightPrefab;
        [SerializeField] private BubbleBehaviour bubblePrefab;
    
        #endregion
    
        #region Fields
    
        public LevelsScriptable currentLevel;
        public string levelName;
    
        #endregion
    
        #region Methods

        #region Unity Methods

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        
            InitGame();
        }
    
        #endregion
        private void InitGame()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                currentLevel = CustomisationManager.Instance.SelectRandomLevel();
                levelName = currentLevel.name;
            }
        }

        private void GetDataFromServer(string serverLevelName)
        {
            if (IsClient)
            {
                currentLevel = CustomisationManager.Instance.GetLevelByName(serverLevelName);
            }
        }
    
        public void StartGame()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                StartGameServerRpc();
            }
        }
    
        [ServerRpc]
        private void StartGameServerRpc()
        {
        
            StartGameClientRpc(levelName);
        }
    
        [ClientRpc]
        private void StartGameClientRpc(string levelName)
        {
            CustomisationManager.Instance.SetThemeText(currentLevel.theme, true);
            SpawnCarrosserieBubbles();
            if (NetworkManager.Singleton.IsHost) return;
        
        }
    
        [ServerRpc]
        public void ClientJoinedServerRpc()
        {
            ClientJoinedClientRpc(levelName);
        }
    
        [ClientRpc]
        private void ClientJoinedClientRpc(string levelNameServer)
        {
            if (NetworkManager.Singleton.IsHost) return;
            GetDataFromServer(levelNameServer);
        }
    
        #endregion

    }
}
