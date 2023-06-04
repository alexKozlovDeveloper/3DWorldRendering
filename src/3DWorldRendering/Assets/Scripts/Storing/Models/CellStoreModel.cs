using Assets.Scripts.Storing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Storing.Models
{
    public class CellStoreModel
    {
        public int X { get; set; }
        public int Y { get; set; }

        public LayerStoreModel[] CellLayers { get; set; }

        private IList<ILayer> _layers;
        public IList<ILayer> Layers 
        { 
            get 
            {
                if (_layers == null) 
                {
                    _layers = new List<ILayer>();

                    foreach (var layerStoreModel in CellLayers)
                    {
                        _layers.Add(layerStoreModel.GetAsCellLayer());
                    }
                }

                return _layers;
            } 
        }

        public L GetLayer<L>() where L : ILayer
        {
            return (L)Layers.FirstOrDefault(a => a.GetType() == typeof(L));
        }

        public CellStoreModel() { }

        public CellStoreModel(ICell cell)
        {
            X = cell.X;
            Y = cell.Y;

            CellLayers = new LayerStoreModel[cell.CellLayers.Count];

            for (int i = 0; i < cell.CellLayers.Count; i++)
            {
                CellLayers[i] = new LayerStoreModel(cell.CellLayers[i]);
            }
        }
    }
}
