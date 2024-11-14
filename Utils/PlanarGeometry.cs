using System;
using System.Collections.Generic;


namespace Graphic3D.Utils
{
    public static class PlanarGeometry
    {
        //trianglefan
        public static List<Vertex> createCircle(float radius, Vertex center)
        {
            int NUM_SEGMENTS = 64;
            
            var points = new List<Vertex>
            {
                new Vertex(center.X, center.Y, center.Z)
            };
            for (int i = 0; i <= NUM_SEGMENTS; i++)
            {
                float angle = i  * (float)(2 * Math.PI / NUM_SEGMENTS); ;
                float x = center.X + radius * (float)Math.Cos(angle);
                float y = center.Y + radius * (float)Math.Sin(angle);
                points.Add(new Vertex(x, y, center.Z));
            }
            return points;
        }

        //trianglestrip
        public static List<Vertex> createCurvedSurface(float height, float radius, bool vertical = true)
        {
            int NUM_SEGMENTS = 36;
            float angleIncrement = (float)(2 * Math.PI) / NUM_SEGMENTS;
            var points = new List<Vertex>();

            for (int i = 0; i <= NUM_SEGMENTS; i++)
            {
                float angle = i * angleIncrement;
                float x = radius * (float)Math.Cos(angle);
                float y = radius * (float)Math.Sin(angle);

                if(vertical)
                {
                    //bottom vertex
                    points.Add(new Vertex(x, 0, y));

                    // top vertex
                    points.Add(new Vertex(x, height, y));
                }
                else
                {
                    //bottom vertex
                    points.Add(new Vertex(x, y, 0));

                    // top vertex
                    points.Add(new Vertex(x, y,height));
                }
            }

            return points;
        }



    }
}
