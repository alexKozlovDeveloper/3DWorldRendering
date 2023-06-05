using Assets.Scripts.IsometricRenders.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IsometricRenderController : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [SerializeField] private Transform CameraPivat;

    //public float CameraTileSizeX = 19.2f;
    //public float CameraTileSizeY = 10.8f;

    public float CameraTileSizeXCoef = 0.01f;
    public float CameraTileSizeYCoef = 0.01f;

    public float CameraAltitude = 10f;

    //public int textureTileSizeX = 198;
    //public int textureTileSizeY = 108;

    public float TextureResolutionCoef = 0.3f;

    public int ResolutionX = 1920;
    public int ResolutionY = 1080;

    public void RenderToFile(IsometricRenderConfig config)
    {
        float CameraTileSizeX = (int)(ResolutionX * CameraTileSizeXCoef);
        float CameraTileSizeY = (int)(ResolutionY * CameraTileSizeYCoef);

        var cameraTilesPerRowX = (int)(config.ObjectWidth / CameraTileSizeX) + 2;
        var cameraTilesPerRowY = (int)(config.ObjectHeight / CameraTileSizeY) + 2;

        var textureTileSizeX = (int)(ResolutionX * TextureResolutionCoef);
        var textureTileSizeY = (int)(ResolutionY * TextureResolutionCoef);

        Texture2D renderTexture = new Texture2D(
                textureTileSizeX * cameraTilesPerRowX,
                textureTileSizeY * cameraTilesPerRowY,
                TextureFormat.RGB24,
                false
            );

        //CameraTileSizeX *= CameraTileSizeCoof;
        //CameraTileSizeY *= CameraTileSizeCoof;

        //int textureTileSizeX = (int)(config.ResultImageWidth / cameraTilesPerRowX);
        //int textureTileSizeY = (int)(config.ResultImageHeight / cameraTilesPerRowY);

        for (float cameraX = 0, textureX = 0, i = 0; cameraX < config.ObjectWidth + CameraTileSizeX; cameraX += CameraTileSizeX, textureX += textureTileSizeX, i++)
        {
            for (float cameraY = 0, textureY = 0, j = 0; cameraY < config.ObjectHeight + CameraTileSizeY; cameraY += CameraTileSizeY, textureY += textureTileSizeY, j++)
            {
                //float xPos = cameraX * config.TileOffsetX;
                //float yPos = cameraY * config.TileOffsetY;

                //int textureXPos = x * config.TileWidth;
                //int textureYPos = y * config.TileHeight;

                // Устанавливаем позицию камеры на текущую часть карты
                //Camera.transform.position = new Vector3(xPos + config.TileWidth / 2, config.CameraHeight, yPos + config.TileHeight / 2);
                CameraPivat.position = new Vector3(cameraX, CameraAltitude, cameraY) + config.BasePorint;

                // Рендерим текущую часть карты в текстуру
                RenderTexture rt = new RenderTexture(textureTileSizeX, textureTileSizeY, 24);
                Camera.targetTexture = rt;
                Camera.Render();
                RenderTexture.active = rt;

                i = 0;
                j = 0;

                // Копируем результат в целевую текстуру
                renderTexture.ReadPixels(new Rect(0, 0, textureTileSizeX, textureTileSizeY), (int)textureX + (int)i, (int)textureY + (int)j);
                RenderTexture.active = null;
                Camera.targetTexture = null;
                Destroy(rt);

                //return;
            }
        }

        SaveToFile(renderTexture, config.FilePath);
    }

    public void RenderToFileOLD(IsometricRenderConfig config)
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
