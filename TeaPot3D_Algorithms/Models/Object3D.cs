using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaPot3D_Algorithms.Models
{
    public class Object3D
    {
        public Dictionary<int, Vertex> Vertices = new Dictionary<int, Vertex>();
        public Dictionary<int, Face> Faces = new Dictionary<int, Face>();
    }
}
