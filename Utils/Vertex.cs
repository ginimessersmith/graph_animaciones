

namespace Graphic3D.Utils
{
    public struct Vertex
    {
       

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vertex(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public static Vertex operator +(Vertex v1, Vertex v2)
        {
            return new Vertex(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
