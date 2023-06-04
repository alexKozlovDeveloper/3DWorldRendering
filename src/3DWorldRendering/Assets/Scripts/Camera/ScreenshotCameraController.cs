using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotCameraController : MonoBehaviour
{
    public Camera cameraToSave;
    public string savePath = "Screenshots/";

    public int mapSize = 1000; // Размер карты
    public int tileSize = 100; // Размер каждой части карты, которая будет рендериться
    public string fileName = "map.png"; // Имя файла, в который будет сохранен результат

    private Texture2D mapTexture; // Текстура, в которую будет сохраняться результат
    private int tilesPerRow; // Количество частей карты в строке

    void Start()
    {
        // Вычисляем количество частей карты в строке
        tilesPerRow = Mathf.CeilToInt((float)mapSize / tileSize);

        // Создаем пустую текстуру, в которую будем сохранять результат
        mapTexture = new Texture2D(mapSize, mapSize, TextureFormat.RGB24, false);
    }

    //void LateUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        TakeScreenshot();
    //    }

    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        TakeFullScreenshot();
    //    }
    //}

    void TakeScreenshot()
    {
        // Создаем текстуру с размерами экрана и копируем содержимое экрана на нее
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        cameraToSave.targetTexture = rt;
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cameraToSave.Render();
        RenderTexture.active = rt;
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        // Переворачиваем текстуру по вертикали, чтобы получить правильное изображение
        //int width = screenshotTexture.width;
        //int height = screenshotTexture.height;
        //Color[] pixels = screenshotTexture.GetPixels();
        //for (int y = 0; y < height / 2; ++y)
        //{
        //    int swapY = height - y - 1;
        //    for (int x = 0; x < width; ++x)
        //    {
        //        Color pixel = pixels[y * width + x];
        //        pixels[y * width + x] = pixels[swapY * width + x];
        //        pixels[swapY * width + x] = pixel;
        //    }
        //}
        //screenshotTexture.SetPixels(pixels);
        //screenshotTexture.Apply();

        // Сохраняем текстуру в файл
        string fileName = "screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string filePath = Path.Combine(savePath, fileName);
        File.WriteAllBytes(filePath, screenshotTexture.EncodeToPNG());

        // Очищаем рендер-текстуру и активную текстуру
        cameraToSave.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
    }

    void TakeFullScreenshot() 
    {
        // Проходим циклом через каждую часть карты и рендерим ее в текстуру
        for (int y = 0; y < tilesPerRow; y++)
        {
            for (int x = 0; x < tilesPerRow; x++)
            {
                // Вычисляем координаты текущей части карты
                int xPos = x * tileSize;
                int yPos = y * tileSize;

                // Устанавливаем позицию камеры на текущую часть карты
                cameraToSave.transform.position = new Vector3(xPos + tileSize / 2, 0, yPos + tileSize / 2);

                // Рендерим текущую часть карты в текстуру
                RenderTexture rt = new RenderTexture(tileSize, tileSize, 24);
                cameraToSave.targetTexture = rt;
                cameraToSave.Render();
                RenderTexture.active = rt;

                // Копируем результат в целевую текстуру
                mapTexture.ReadPixels(new Rect(0, 0, tileSize, tileSize), xPos, yPos);
                RenderTexture.active = null;
                cameraToSave.targetTexture = null;
                Destroy(rt);
            }
        }

        // Сохраняем результат в файл
        //byte[] bytes = mapTexture.EncodeToPNG();
        //File.WriteAllBytes(Application.dataPath + "/" + fileName, bytes);

        string fileName = "screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string filePath = Path.Combine(savePath, fileName);
        byte[] bytes = mapTexture.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
    }
}

