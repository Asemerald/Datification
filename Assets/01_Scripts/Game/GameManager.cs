using Game.Customisation;
using Game;
using UI;
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
        private int currentCustomStageIndex = 0;
    
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

        public void NextCustomStage()
        {
            currentCustomStageIndex++;
            switch (currentCustomStageIndex)
            {
                case 1:
                    DespawnAllBubbles();
                    SpawnRouesBubbles();
                    break;
                case 2:
                    DespawnAllBubbles();
                    SpawnRouesBubbles();
                    break;
                case 3:
                    DespawnAllBubbles();
                    SpawnAccessoiresBubbles();
                    break;
                case 4:
                    EndCustomisation();
                    break;
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
        
            StartGameClientRpc();
        }
    
        [ClientRpc]
        private void StartGameClientRpc()
        {
            CustomisationManager.Instance.SetThemeText(currentLevel.theme, true);
            SpawnCarrosserieBubbles(); // TODO delay ?
            
            InGameUI.Instance.ShowNextButton(true);
            
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
        
        private void EndCustomisation()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                //EndCustomisationServerRpc();
            }
        }
    
        #endregion

    }
}
