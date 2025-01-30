using Game.Customisation;
using Game;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace Game
{
    public partial class GameManager
    {
        private void SpawnCarrosserieBubbles()
        {
            Debug.Log("Spawning carrosserie bubbles");
    
            var mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<RectTransform>();

            foreach (var bubble in currentLevel.carrosserieList)
            {
                BubbleBehaviour bubbleBehaviour = Instantiate(bubblePrefab, mainCanvas);
                bubbleBehaviour.carPartData = bubble;

                RectTransform bubbleRect = bubbleBehaviour.GetComponent<RectTransform>();

                // Générer une position aléatoire dans l'espace du canvas
                float x = Random.Range(-mainCanvas.rect.width / 2f + 50f, mainCanvas.rect.width / 2f - 50f);
                float y = Random.Range(-mainCanvas.rect.height / 2f + 50f, mainCanvas.rect.height / 2f - 50f);

                bubbleRect.anchoredPosition = new Vector2(x, y); // Position en UI space
            }
        }

    }
}
