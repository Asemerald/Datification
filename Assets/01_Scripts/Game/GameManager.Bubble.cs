using Game.Customisation;
using Game;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public partial class GameManager
    {
        private List<Vector2> usedPositions = new List<Vector2>(); // Stocker les positions déjà utilisées
        private float minDistanceBetweenBubbles = 150f; // Distance minimale entre bulles

        private void SpawnCarrosserieBubbles()
        {
            Debug.Log("Spawning carrosserie bubbles");

            var mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<RectTransform>();

            float canvasWidth = mainCanvas.rect.width;
            float canvasHeight = mainCanvas.rect.height;

            float minY = -canvasHeight / 2f + (canvasHeight * 0.35f); // Bas limité à 35% du canvas
            float maxY = canvasHeight / 2f - 50f; // Haut de l'écran

            usedPositions.Clear(); // Reset des positions

            foreach (var bubble in currentLevel.carrosserieList)
            {
                BubbleBehaviour bubbleBehaviour = Instantiate(bubblePrefab, mainCanvas);
                bubbleBehaviour.carPartData = bubble;

                RectTransform bubbleRect = bubbleBehaviour.GetComponent<RectTransform>();

                Vector2 spawnPos;
                int attempts = 0;

                do
                {
                    float x = Random.Range(-canvasWidth / 2f + 50f, canvasWidth / 2f - 50f);
                    float y = Random.Range(minY, maxY);
                    spawnPos = new Vector2(x, y);
                    attempts++;

                } while (IsTooCloseToOthers(spawnPos) && attempts < 10);

                usedPositions.Add(spawnPos);
                bubbleRect.anchoredPosition = spawnPos;
            }
        }

        private bool IsTooCloseToOthers(Vector2 newPos)
        {
            foreach (var pos in usedPositions)
            {
                if (Vector2.Distance(newPos, pos) < minDistanceBetweenBubbles)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
