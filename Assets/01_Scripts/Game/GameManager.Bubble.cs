using Game.Customisation;
using Game;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public partial class GameManager
    {
        
        private void DespawnAllBubbles()
        {
            var bubbles = GameObject.FindGameObjectsWithTag("Bubble");
            foreach (var bubble in bubbles)
            {
                Destroy(bubble);
            }
        }

        private void SpawnCarrosserieBubbles()
        {
            Debug.Log("Spawning carrosserie bubbles");

            var mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<RectTransform>();

            float canvasWidth = mainCanvas.rect.width;
            float canvasHeight = mainCanvas.rect.height;

            float minY = -canvasHeight / 2f + (canvasHeight * 0.35f); // Bottom 35% locked
            float maxY = canvasHeight / 2f - 50f; // Top allowed area

            foreach (var bubble in currentLevel.carrosserieList)
            {
                BubbleBehaviour bubbleBehaviour = Instantiate(bubblePrefab, mainCanvas);
                bubbleBehaviour.carPartData = bubble;

                RectTransform bubbleRect = bubbleBehaviour.GetComponent<RectTransform>();

                float x = Random.Range(-canvasWidth / 2f + 50f, canvasWidth / 2f - 50f);
                float y = Random.Range(minY, maxY);

                bubbleRect.anchoredPosition = new Vector2(x, y);
            }
        }
        
        private void SpawnRouesBubbles()
        {
            Debug.Log("Spawning roues bubbles");

            var mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<RectTransform>();

            float canvasWidth = mainCanvas.rect.width;
            float canvasHeight = mainCanvas.rect.height;

            float minY = -canvasHeight / 2f + (canvasHeight * 0.35f); // Bottom 35% locked
            float maxY = canvasHeight / 2f - 50f; // Top allowed area

            foreach (var bubble in currentLevel.rouesList)
            {
                BubbleBehaviour bubbleBehaviour = Instantiate(bubblePrefab, mainCanvas);
                bubbleBehaviour.carPartData = bubble;

                RectTransform bubbleRect = bubbleBehaviour.GetComponent<RectTransform>();

                float x = Random.Range(-canvasWidth / 2f + 50f, canvasWidth / 2f - 50f);
                float y = Random.Range(minY, maxY);

                bubbleRect.anchoredPosition = new Vector2(x, y);
            }
        }

        private void SpawnPharesBubbles()
        {
            Debug.Log("Spawning phares bubbles");

            var mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<RectTransform>();

            float canvasWidth = mainCanvas.rect.width;
            float canvasHeight = mainCanvas.rect.height;

            float minY = -canvasHeight / 2f + (canvasHeight * 0.35f); // Bottom 35% locked
            float maxY = canvasHeight / 2f - 50f; // Top allowed area

            foreach (var bubble in currentLevel.pharesList)
            {
                BubbleBehaviour bubbleBehaviour = Instantiate(bubblePrefab, mainCanvas);
                bubbleBehaviour.carPartData = bubble;

                RectTransform bubbleRect = bubbleBehaviour.GetComponent<RectTransform>();

                float x = Random.Range(-canvasWidth / 2f + 50f, canvasWidth / 2f - 50f);
                float y = Random.Range(minY, maxY);

                bubbleRect.anchoredPosition = new Vector2(x, y);
            }
        }
        
        private void SpawnAccessoiresBubbles()
        {
            Debug.Log("Spawning accessoires bubbles");

            var mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<RectTransform>();

            float canvasWidth = mainCanvas.rect.width;
            float canvasHeight = mainCanvas.rect.height;

            float minY = -canvasHeight / 2f + (canvasHeight * 0.35f); // Bottom 35% locked
            float maxY = canvasHeight / 2f - 50f; // Top allowed area

            foreach (var bubble in currentLevel.accessoiresList)
            {
                BubbleBehaviour bubbleBehaviour = Instantiate(bubblePrefab, mainCanvas);
                bubbleBehaviour.carPartData = bubble;

                RectTransform bubbleRect = bubbleBehaviour.GetComponent<RectTransform>();

                float x = Random.Range(-canvasWidth / 2f + 50f, canvasWidth / 2f - 50f);
                float y = Random.Range(minY, maxY);

                bubbleRect.anchoredPosition = new Vector2(x, y);
            }
        }
        
    }
}
