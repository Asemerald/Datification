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

        public CarPartScriptable GetCarPartById(int carPartId)
        {
            // Load all CarPart ScriptableObjects from the Resources/CarParts folder
            // return the CarPart with the id
            
            CarPartScriptable[] carParts = Resources.LoadAll<CarPartScriptable>("CarParts");
            
            if (carParts.Length == 0)
            {
                Debug.LogError("No car parts found in Resources/CarParts!");
                return null;
            }
            
            foreach (CarPartScriptable carPart in carParts)
            {
                if (carPart.id == carPartId)
                {
                    return carPart;
                }
            }
            
            Debug.LogError("Car part not found");
            return null;
        }
        
        #endregion
        
    }
}