using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.IsometricRenders.Models
{
    public class IsometricRenderConfig
    {
        public Vector3 Center { get; set; }

        public int CameraHeight { get; set; }

        public int ObjectWidth { get; set; }
        public int ObjectHeight { get; set; }

        public int ResultImageWidth { get; set; }
        public int ResultImageHeight { get; set; }

        public int TilesPerRow { get; set; }

        public int TileWidth { get { return ResultImageWidth / TilesPerRow; } }
        public int TileHeight { get { return ResultImageHeight / TilesPerRow; } }

        public int TileOffsetX { get { return ObjectWidth / TilesPerRow; } }
        public int TileOffsetY { get { return ObjectHeight / TilesPerRow; } }

        public string FilePath { get; set; }
    }
}
 