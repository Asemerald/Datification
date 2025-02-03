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
        [SerializeField] private NetworkObject carLeftPrefab;
        [SerializeField] private NetworkObject carRightPrefab;
        [SerializeField] public BubbleBehaviour bubblePrefab;
    
        #endregion
    
        #region Fields
    
        public LevelsScriptable currentLevel;
        public string levelName;
        private int currentCustomStageIndex = 0;
        public bool hasRightCar = false;
        public CarCustomControl car;
    
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
                hasRightCar = Random.Range(0, 2) == 0;
                levelName = currentLevel.name;
            }

            // Find cameras by tag
            GameObject mainCamRight = GameObject.FindWithTag("MainCameraRight");
            GameObject mainCamLeft = GameObject.FindWithTag("MainCameraLeft");

            // Ensure cameras are found before setting active states
            if (mainCamRight != null && mainCamLeft != null)
            {
                mainCamRight.SetActive(hasRightCar);
                mainCamLeft.SetActive(!hasRightCar);
            }
            else
            {
                Debug.LogError("MainCameraRight or MainCameraLeft not found! Make sure they have the correct tags.");
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

        private void GetDataFromServer(string serverLevelName, bool rightCar)
        {
            if (IsClient)
            {
                currentLevel = CustomisationManager.Instance.GetLevelByName(serverLevelName);
                hasRightCar = !rightCar;
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
            SpawnCar(hasRightCar);
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
            GetDataFromServer(levelNameServer, hasRightCar);
            
            // Find cameras by tag
            GameObject mainCamRight = GameObject.FindWithTag("MainCameraRight");
            GameObject mainCamLeft = GameObject.FindWithTag("MainCameraLeft");

            // Ensure cameras are found before setting active states
            if (mainCamRight != null && mainCamLeft != null)
            {
                mainCamRight.SetActive(hasRightCar);
                mainCamLeft.SetActive(!hasRightCar);
            }
            else
            {
                Debug.LogError("MainCameraRight or MainCameraLeft not found! Make sure they have the correct tags.");
            }
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
