using Assets.Scripts.IsometricRenders.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IsometricRenderController : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [SerializeField] private Transform CameraPivat;

    public void RenderToFile(IsometricRenderConfig config) 
    {
        Texture2D renderTexture = new Texture2D(
                config.ResultImageWidth, 
                config.ResultImageHeight, 
                TextureFormat.RGB24, 
                false
            );

        for (int y = 0; y < config.TilesPerRow; y++)
        {
            for (int x = 0; x < config.TilesPerRow; x++)
            {
                int xPos = x * config.TileOffsetX;
                int yPos = y * config.TileOffsetY;

                int textureXPos = x * config.TileWidth;
                int textureYPos = y * config.TileHeight;

                // Устанавливаем позицию камеры на текущую часть карты
                //Camera.transform.position = new Vector3(xPos + config.TileWidth / 2, config.CameraHeight, yPos + config.TileHeight / 2);
                CameraPivat.position = new Vector3(xPos + config.TileWidth / 2, config.CameraHeight, yPos + config.TileHeight / 2);

                // Рендерим текущую часть карты в текстуру
                RenderTexture rt = new RenderTexture(config.TileWidth, config.TileHeight, 24);
                Camera.targetTexture = rt;
                Camera.Render();
                RenderTexture.active = rt;

                // Копируем результат в целевую текстуру
                renderTexture.ReadPixels(new Rect(0, 0, config.TileWidth, config.TileHeight), textureXPos, textureYPos);
                RenderTexture.active = null;
                Camera.targetTexture = null;
                Destroy(rt);
            }
        }

        SaveToFile(renderTexture, config.FilePath);
    }

    private void SaveToFile(Texture2D renderTexture, string filePath) 
    {
        byte[] bytes = renderTexture.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
    }
}
