﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaPot3D_Algorithms.Models
{
    public class SlicedFaceModel
    {
        public List<Face> FacesAbovePLane { get; set; } = new List<Face> { };
        public List<Face> FacesBelowPlane { get; set; } = new List<Face> { };
    }
}