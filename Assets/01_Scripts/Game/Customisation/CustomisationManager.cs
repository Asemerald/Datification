using System;
using Cinemachine;
using Game;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Game.Customisation
{
    public class CustomisationManager : InstanceBase<CustomisationManager>
    {
        #region Serialize Fields 

        [Header("Theme UI")]
        [SerializeField] private TMP_Text themeText;
        
        
        #endregion
        
        #region Fields 
        
        private GameObject themeGameObject;
        public CarPartScriptable LastCarPartScriptable { get; set; }
        
        #endregion
        
        #region Methods
        
        #region Unity Methods
        
        private void Start()
        {
            themeGameObject = themeText.transform.parent.gameObject;
            themeGameObject.SetActive(false);
        }
        
        
        
        #endregion
        
        public void SetLastCarPart(CarPartScriptable carPartScriptable = null)
        {
            LastCarPartScriptable = carPartScriptable;
        }
        
        public void SpawnLastBubble()
        {
            if (LastCarPartScriptable == null) return;
            
            var mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<RectTransform>();

            float canvasWidth = mainCanvas.rect.width;
            float canvasHeight = mainCanvas.rect.height;

            float minY = -canvasHeight / 2f + (canvasHeight * 0.35f); // Bottom 35% locked
            float maxY = canvasHeight / 2f - 50f; // Top allowed area

            BubbleBehaviour bubbleBehaviour = Instantiate(GameManager.Instance.bubblePrefab, mainCanvas);
            bubbleBehaviour.carPartData = LastCarPartScriptable;

            RectTransform bubbleRect = bubbleBehaviour.GetComponent<RectTransform>();

            float x = Random.Range(-canvasWidth / 2f + 50f, canvasWidth / 2f - 50f);
            float y = Random.Range(minY, maxY);

            bubbleRect.anchoredPosition = new Vector2(x, y);
            
        }
        
        public void SetThemeText(string text, bool show)
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                themeGameObject.SetActive(show);
                themeText.text = text;
            });
        }
        
        public LevelsScriptable SelectRandomLevel()
        {
            // Load all Level ScriptableObjects from the Resources/Levels folder
            LevelsScriptable[] levels = Resources.LoadAll<LevelsScriptable>("Levels");

            if (levels.Length == 0)
            {
                Debug.LogError("No levels found in Resources/Levels!");
                return null;
            }

            // Select a random level
            int randomLevelSelected = UnityEngine.Random.Range(0, levels.Length);
            return levels[randomLevelSelected];
        }

        public LevelsScriptable GetLevelByName(string levelName)
        {
            // Load all Level ScriptableObjects from the Resources/Levels folder
            LevelsScriptable[] levels = Resources.LoadAll<LevelsScriptable>("Levels");
            
            if (levels.Length == 0)
            {
                Debug.LogError("No levels found in Resources/Levels!");
                return null;
            }
            
            // Find the Level with the name 
            foreach (LevelsScriptable level in levels)
            {
                if (level.name == levelName)
                {
                    return level;
                }
            }
            Debug.LogError("Level not found");
            return null;
        }
        
        #endregion
        
    }
}