using Game.Customisation;
using Game;
using UI;
using Unity.Netcode;
using UnityEngine;
using Utils;
using Debug = System.Diagnostics.Debug;

namespace Game
{
    public partial class GameManager : NetworkInstanceBase<GameManager>
    {
    
        #region SerializeField
    
        [Header("Game Prefabs")]
        [SerializeField] private NetworkObject carLeftPrefab;
        [SerializeField] private NetworkObject carRightPrefab;
        [SerializeField] public BubbleBehaviour bubblePrefab;
        [SerializeField] public Vector3 MainCameraLeftPosition;
        [SerializeField] public Vector3 MainCameraLeftRotation;
        [SerializeField] public Vector3 MainCameraRightPosition;
        [SerializeField] public Vector3 MainCameraRightRotation;
        
    
        #endregion
    
        #region Fields
    
        public LevelsScriptable currentLevel;
        public string levelName;
        private int currentCustomStageIndex = 0;
        public bool hasRightCar = false;
        public CarCustomControl car;
        
        public NetworkVariable<bool> hostFinishedCustomisation = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> clientFinishedCustomisation = new NetworkVariable<bool>(false);

        //Ajout UI
        private CustomisationPhaseUIManager phaseCustomUIManager;
        
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
            
            Camera.main.transform.position = hasRightCar ? MainCameraRightPosition : MainCameraLeftPosition;
            Camera.main.transform.eulerAngles = hasRightCar ? MainCameraRightRotation : MainCameraLeftRotation;
            
            
            //Ajout UI
            phaseCustomUIManager = CustomisationPhaseUIManager.Instance;
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
                    SpawnPharesBubbles();
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
            //CustomisationManager.Instance.SetThemeText(currentLevel.theme, true);
            
            //Ajout ui
            phaseCustomUIManager.CustomisationPhaseActivation(true);
            phaseCustomUIManager.ThemeActivation(true, currentLevel.theme);
            //Set text Theme
            
            SpawnCarrosserieBubbles(); // TODO delay ?
            
            InGameUI.Instance.ShowNextButton(true);
            
            if (NetworkManager.Singleton.IsHost) return;
        
        }
    
        [ServerRpc]
        public void ClientJoinedServerRpc()
        {
            ClientJoinedClientRpc(levelName, hasRightCar);
        }
    
        [ClientRpc]
        private void ClientJoinedClientRpc(string levelNameServer, bool rightCarBool)
        {
            if (NetworkManager.Singleton.IsHost) return;
            GetDataFromServer(levelNameServer, rightCarBool);

            Debug.Assert(Camera.main != null, "Camera.main != null");
            Camera.main.transform.position = hasRightCar ? MainCameraRightPosition : MainCameraLeftPosition;
            Camera.main.transform.eulerAngles = hasRightCar ? MainCameraRightRotation : MainCameraLeftRotation;
        }
        
        private void EndCustomisation()
        {
            InGameUI.Instance.ShowNextButton(false);

            ChangeFinishBoolServerRpc(NetworkManager.Singleton.IsHost, true);
        }
        
        [ServerRpc (RequireOwnership = false)]
        private void ChangeFinishBoolServerRpc(bool isHost, bool value, ServerRpcParams rpcParams = default)
        {
            if (isHost)
            {
                hostFinishedCustomisation.Value = value;
                
                //Ajout ui
                phaseCustomUIManager.WaitingActivation(true,true);
                CheckEndCustomisation();
            }
            else
            {
                clientFinishedCustomisation.Value = value;
                
                //Ajout ui
                DisplayEndUiClientRpc();
            }
        }
        
        [ClientRpc]
        private void DisplayEndUiClientRpc()
        {
            if (NetworkManager.Singleton.IsHost) return;
            
            //Ajout ui
            phaseCustomUIManager.WaitingActivation(false,false);
            phaseCustomUIManager.ResultActivation(true);
        }
    
        #endregion

    }
}
