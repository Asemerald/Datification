using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/NewLevel", order = 1)]
    public class LevelsScriptable : ScriptableObject
    {
        // A scriptable object that contains the levels data
        // A question theme 
        // a list of CarPartScriptable objects

        [Header("Theme du niveau")] public string theme;

        [Header("Liste des Carrosseries")] 
        public List<CarPartScriptable> carrosserieList;

        [Header("Liste des Roues")] 
        public List<CarPartScriptable> rouesList;

        [Header("Liste des Phares")]
        public List<CarPartScriptable> pharesList;
        
        [Header("Liste des Accessoires")]
        public List<CarPartScriptable> accessoiresList;
    }
}
