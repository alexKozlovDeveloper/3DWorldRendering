using Assets.Scripts.Storing;
using Assets.Scripts.Storing.Models;
using Assets.Scripts.Storing.Models.Layers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Assets.Scripts.IsometricRenders.Models;
using System.IO;
//using ArcanumIsland.

public class VoxelMapRender : MonoBehaviour
{
    [SerializeField] private GameObject World;

    [SerializeField] private GameObject IceVoxelPrefab;
    [SerializeField] private GameObject SendVoxelPrefab;
    [SerializeField] private GameObject RiverVoxelPrefab;
    [SerializeField] private GameObject GreenVoxelPrefab;
    [SerializeField] private GameObject SeaVoxelPrefab;
    [SerializeField] private GameObject DunVoxelPrefab;

    public float step = 1f;
    public float heightScale = 0.5f;

    [SerializeField] private GameObject Render;
    private IsometricRenderController _renderController;
    private IsometricRenderConfig _config = new IsometricRenderConfig { };

    // Start is called before the first frame update
    void Start()
    {
        _renderController = Render.GetComponent<IsometricRenderController>();

        //var seaLevel = 0.7f;
        //var sea = Instantiate(SeaVoxelPrefab);

        //sea.transform.position = new Vector3(5, seaLevel, 5);
        //sea.transform.localScale = new Vector3(step * 100, 1, step * 100);


        //for (float i = 0; i < 100; i++)
        //{
        //    var ice = Instantiate(IceVoxelPrefab);

        //    ice.transform.position = new Vector3(i * step, 0, 0);
        //}

        //

        var model = ModelStoringExtensions.DeserializeModelFromXml<MapStoreModel>(@"D:\ArcanumIsland\Models\for_test_xml_map.xml");


        for (int x = 0; x < model.Width; x++)
        {
            for (int z = 0; z < model.Height; z++)
            {
                var cell = model.Cells[x][z];

                ProcessCell(cell);
            }
        }

        _config.ObjectWidth = model.Width;
        _config.ObjectHeight = model.Height;

        _config.CameraHeight = 20;

        _config.TilesPerRow = 10;

        _config.BasePorint = new Vector3(0, 0, 0);

        string savePath = "Screenshots/";
        string fileName = "screenshot_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string filePath = Path.Combine(savePath, fileName);

        _config.FilePath = filePath;

        var pixelPerUnit = 100;

        _config.ResultImageWidth = model.Width * pixelPerUnit;
        _config.ResultImageHeight = model.Height * pixelPerUnit;
    }

    private void ProcessCell(CellStoreModel cell)
    {
        //var altitude = cell.CellLayers.First(a => a.Name == nameof(Altitude)).GetAsCellLayer() as Altitude;
        var altitude = cell.GetLayer<Altitude>();

        var ocean = cell.GetLayer<Ocean>();

        if (ocean != null)
        {
            CreateVoxel(SeaVoxelPrefab, GetXZ(cell.X), GetY(ocean.Value), GetXZ(cell.Y));
        }

        var sand = cell.GetLayer<Sand>();
        var grass = cell.GetLayer<Grass>();
        var snow = cell.GetLayer<Snow>();

        if (sand != null)
        {
            CreateVoxel(SendVoxelPrefab, GetXZ(cell.X), GetY(altitude.Value), GetXZ(cell.Y));
        }

        if (grass != null)
        {
            CreateVoxel(GreenVoxelPrefab, GetXZ(cell.X), GetY(altitude.Value), GetXZ(cell.Y));
        }

        if (snow != null)
        {
            CreateVoxel(IceVoxelPrefab, GetXZ(cell.X), GetY(altitude.Value), GetXZ(cell.Y));
        }

        if (sand == null && grass == null && snow == null)
        {
            CreateVoxel(DunVoxelPrefab, GetXZ(cell.X), GetY(altitude.Value), GetXZ(cell.Y));
        }

        //var green = Instantiate(GreenVoxelPrefab);

        //var y = (float)Math.Round((double)layer.Value * heightScale, 2);

        //green.transform.position = new Vector3(x * step, y, z * step);
        //green.transform.localScale *= 3;
    }

    private void CreateVoxel(GameObject voxelPrefab, float x, float y, float z)
    {
        var voxel = Instantiate(voxelPrefab);

        voxel.transform.position = new Vector3(x, y, z);

        var scale = voxel.transform.localScale;

        scale.y *= 3;

        voxel.transform.localScale = scale;

        // Получаем Transform компоненты объектов
        var worldTransform = World.GetComponent<Transform>();
        var voxelTransform = voxel.GetComponent<Transform>();

        // Устанавливаем родителя для объекта childObject
        voxelTransform.SetParent(worldTransform);
    }

    private float GetY(double value)
    {
        return (float)Math.Round((double)value * heightScale, 2);
    }

    private float GetXZ(double value)
    {
        return (float)value * step;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _renderController.RenderToFile(_config);
        }
    }
}
