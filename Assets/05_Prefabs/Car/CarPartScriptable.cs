using System.Collections.Generic;
using UnityEngine;

namespace Prefabs
{
    [CreateAssetMenu(fileName = "CarPart", menuName = "ScriptableObjects/NewCarPart", order = 1)]
    public class CarPartScriptable : ScriptableObject
    {
        public enum CarPartType
        {
            Carrosserie,
            Roues,
            Phares,
            Accessoires
        }
        
        [Header("Unique ID")] 
        public string id;
        
        [Header("Nom de la pièce")]
        public string name;
        
        [Header("Mesh de la pièce")]
        public Mesh mesh;
            
        
    }
}