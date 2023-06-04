using Assets.Scripts.Storing.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Storing.Models
{
    public class LayerStoreModel
    {
        public string Name { get; set; }

        public string Json { get; set; }

        public LayerStoreModel() { }

        public LayerStoreModel(ILayer layer)
        {
            var type = layer.GetType();
            Name = type.Name;

            Json = JsonConvert.SerializeObject(layer);
        }

        public ILayer GetAsCellLayer()
        {
            var dict = ModelStoringExtensions.FindCellLayerTypes().ToDictionary(a => a.Name, a => a);

            if (dict.ContainsKey(Name))
            {
                var layer = JsonConvert.DeserializeObject(Json, dict[Name]) as ILayer;

                return layer;
            }

            return null;
        }
    }
}
