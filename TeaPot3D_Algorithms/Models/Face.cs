using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaPot3D_Algorithms.Models
{
    public class Face
    {
        public int Id { get; set; } = 0;
        public List<int> Vertices { get; set; } = new List<int>();
    }
}
