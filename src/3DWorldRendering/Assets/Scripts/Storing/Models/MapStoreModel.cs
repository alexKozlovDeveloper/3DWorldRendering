using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Storing.Models
{
    [Serializable]
    //[DataContract]
    public class MapStoreModel
    {
        //[DataMember]
        public int Width { get; set; }

        //[DataMember]
        public int Height { get; set; }

        //[DataMember]
        public CellStoreModel[][] Cells { get; set; }
        //public Cell[][] Cells { get; set; }

        public MapStoreModel() { }

        //public MapStoreModel(IMap map)
        //{
        //    Width = map.Width;
        //    Height = map.Height;

        //    Cells = map.CellsMatrix
        //        .Convert(a => new CellStoreModel(a))
        //        .GetAsArray();

        //    //Cells = map.CellsMatrix.GetAsArray();
        //}

        //public IMap GetAsMap()
        //{
        //    var map = new Map(Height, Width);

        //    for (int x = 0; x < Width; x++)
        //    {
        //        for (int y = 0; y < Height; y++)
        //        {
        //            var cell = Cells[x][y];

        //            foreach (var cellLayer in cell.CellLayers)
        //            {
        //                map.CellsMatrix[x, y].AddLayer(cellLayer.GetAsCellLayer());
        //            }
        //        }
        //    }

        //    return map;
        //}
    }
}
