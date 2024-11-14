using Graphic3D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic3D.Utils
{
    class VertexHelper
    {
        public static void SetVertexToAbsScene(Scene scene)
        {
            foreach (var o in scene.Objects.Values)
            {
                SetVertexToAbsObject(o);
            }
        }

        public static void SetVertexToAbsObject(IObject obj)
        {
            foreach (var part in obj.Parts.Values)
            {
                SetVertexToAbsPart(part);
            }
        }

        public static void SetVertexToAbsPart(Part part)
        {
            
            foreach (var face in part.Faces.Values)
            {
                List<Vertex> newPoints = new List<Vertex>();
                foreach (var point in face.Points)
                {
                    newPoints.Add(point + part.Center);
                    
                }
                face.Points = newPoints;
            }

        }
    }
}
