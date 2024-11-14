using Graphic3D.Models;
using System;
using System.Collections.Generic;
using OpenTK;
using System.Drawing;

namespace Graphic3D.Utils
{
    public static class StandardGeometry
    {
        //triangles
        public static Dictionary<string, Face> CreateSphere(float radius, int segments)
        {
           
            Dictionary<string, Face> faces = new Dictionary<string, Face>();

            for (int i = 0; i <= segments; i++)
            {
                float phi1 = MathHelper.Pi * i / segments;
                float phi2 = MathHelper.Pi * (i + 1) / segments;
                Face face = new Face();
                face.PType = MyPrimitiveType.TriangleFan;
                face.Color = Color.White;
                for (int j = 0; j <= segments; j++)
                {
                    float theta1 = MathHelper.TwoPi * j / segments;
                    float theta2 = MathHelper.TwoPi * (j + 1) / segments;

                    Vertex p1 = GetPosition(radius, phi1, theta1);
                    Vertex p2 = GetPosition(radius, phi1, theta2);
                    Vertex p3 = GetPosition(radius, phi2, theta1);
                    Vertex p4 = GetPosition(radius, phi2, theta2);

                    face.AddPoint(p1);
                    face.AddPoint(p2);
                    face.AddPoint(p3);
                    face.AddPoint(p3);
                    face.AddPoint(p2);
                    face.AddPoint(p4);
                }
                faces.Add(i+"", face);
            }
            return faces;
        }

        public static Dictionary<string, Face> CreateCube(float width, float height, float length, Color color)
        {
            Dictionary<string, Face> faces = new Dictionary<string, Face>();

            Vertex[] Vertices =
            {
                new Vertex(-width / 2f, -height / 2f, -length / 2f),
                new Vertex(width / 2f, -height / 2f, -length / 2f),
                new Vertex(width / 2f, height / 2f, -length / 2f),
                new Vertex(-width / 2f, height / 2f, -length / 2f),
                new Vertex(-width / 2f, -height / 2f, length / 2f),
                new Vertex(width / 2f, -height / 2f, length / 2f),
                new Vertex(width / 2f, height / 2f, length / 2f),
                new Vertex(-width / 2f, height / 2f, length / 2f)
            };

            uint[] Indices =
            {
                // front
                0, 7, 3, 0, 4, 7,
                // back
                1, 2, 6, 6, 5, 1,
                // left
                0, 2, 1, 0, 3, 2,
                // right
                4, 5, 6, 6, 7, 4,
                // top
                2, 3, 6, 6, 3, 7,
                // bottom
                0, 1, 5, 0, 5, 4,
            };

            

            for (int i = 0; i < Indices.Length; i += 6)
            {
                Face face = new Face();
                //face.Color = RandomColor();
                face.Color = color;
                for (int j = 0; j < 6; j++)
                {

                    face.AddPoint(Vertices[Indices[i + j]]);

                    //Console.WriteLine(Vertices[Indices[i + j]]);
                }
                faces.Add(i / 6 + "", face);
            }
            return faces;
        }

        private static Vertex GetPosition(float radius, float phi, float theta)
        {
            float x = radius * (float)Math.Sin(phi) * (float)Math.Cos(theta);
            float y = radius * (float)Math.Cos(phi);
            float z = radius * (float)Math.Sin(phi) * (float)Math.Sin(theta);
            return new Vertex(x, y, z);
        }

        private static Random random = new Random();
        private static Color RandomColor()
        {
            byte red = (byte)random.Next(0, 256);
            byte green = (byte)random.Next(0, 256);
            byte blue = (byte)random.Next(0, 256);
            return Color.FromArgb(red, green, blue);
        }

        //trianglestrip
        /*public static Dictionary<int, Face> CreateCylinder(float height, float radius)
        {
            Dictionary<int, Face> faces = new Dictionary<int, Face>();
            int NUM_SEGMENTS = 36;
            float angleIncrement = (float)(2 * Math.PI) / NUM_SEGMENTS;


            // Side wall vertices
            Face sideFace = new Face();
            for (int i = 0; i <= NUM_SEGMENTS; i++)
            {
                float angle = i * angleIncrement;
                float x = radius * (float)Math.Cos(angle);
                float y = radius * (float)Math.Sin(angle);

                //bottom vertex
                sideFace.AddPoint(new Vertex(x, y, 0));

                // top vertex
                sideFace.AddPoint(new Vertex(x, y, height));
            }
            faces.Add(1, sideFace);

            // top cap vertices
            var topVertices = PlanarGeometry.createCircle(radius, 0, 0, height);
            faces.Add(2, new Face(topVertices));
            //bottom cap vertices
            var bottomVertices = PlanarGeometry.createCircle(radius, 0, 0, 0);
            faces.Add(3, new Face(bottomVertices));
            return faces;
        }*/


    }
}
