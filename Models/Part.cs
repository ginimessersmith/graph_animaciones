

using Graphic3D.Rendering;
using Graphic3D.Utils;
using System;
using System.Collections.Generic;

namespace Graphic3D.Models
{
    // La clase Part representa las partes que componen una figura
    public class Part
    {
        // Almacena las caras de una parte, el id de la cara (como un número), y el valor = Face
        public Dictionary<string, Face> Faces { get; private  set; }
        public Vertex Center = new Vertex(0f, 0f, 0f) ;
        public MyPrimitiveType PType { get; set; } = MyPrimitiveType.Triangles;
        private Vertex _center = new Vertex(0f, 0f, 0f);
        public Part()
        {
            Faces = new Dictionary<string, Face>();
        }

        public Part(Dictionary<string, Face> faces, MyPrimitiveType type = MyPrimitiveType.Triangles)
        {
            Faces = faces;
            PType = type;
        }
        public Part(Vertex center)
        {
            Faces = new Dictionary<string, Face>();
            Center = center;
        }

        public Part(Dictionary<string, Face> faces,Vertex center,MyPrimitiveType type = MyPrimitiveType.Triangles)
        {
            Faces = faces;
            Center = center;
            PType = type;
        }

        // Agrega o actualiza una cara
        public void AddFace(string name, Face face)
        {
            Faces.Add(name, face);
        }

        public void ClearFaces()
        {
            Faces.Clear();
        }

        public void Translate(Vertex center, float x=0, float y=0, float z=0)
        {
            //Console.WriteLine("part center: " + Center);
            foreach (var face in Faces.Values)
            {
                face.Translate(x, y, z);
            }
        }

        public void Translate(float x = 0, float y = 0, float z = 0)
        {
            _center = Center + new Vertex(x, y, z);
            foreach (var face in Faces.Values)
            {
                face.Translate(Center,x, y, z);
            }
        }

        public void Scale(float x, float y, float z)
        {
            foreach (var face in Faces.Values)
            {
                face.Scale(x, y, z);
            }
        }

        public void Rotate(float x, float y, float z)
        {
            //Console.WriteLine("part center: " + Center);
            foreach (var face in Faces.Values)
            {
                face.Rotate(x, y, z);
            }
        }

        public void Rotate(Vertex center, float x, float y, float z)
        {
            foreach (var face in Faces.Values)
            {
                face.Rotate(center, x, y, z);
            }
        }

        public void Rotate(Vertex center, float angle,string axis)
        {
            foreach (var face in Faces.Values)
            {
                face.Rotate(center, angle, axis);
            }
        }

        public void Draw()
        {
            foreach (var face in Faces)
            {
                face.Value.Draw(Center);
            }
        }

    }

}
