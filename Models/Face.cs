using Graphic3D.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;


namespace Graphic3D.Models
{
    public class Face
    {
        // Almacena los vértices de una cara, key=índice del vértice,valor=Vertex
        public List<Vertex> Points { get;  set; }
        public Color Color { get; set; } = Color.White;
        public MyPrimitiveType PType { get; set; } = MyPrimitiveType.Triangles;

        private Matrix4 _trans;
        private Matrix4 _scaleMatrix;
        private Matrix4 _translationMatrix;
        private Matrix4 _rotationMatrix;

        public Face()
        {
            Points = new List<Vertex>();
            _trans = Matrix4.Identity;
            _scaleMatrix = Matrix4.Identity;
            _translationMatrix = Matrix4.Identity;
            _rotationMatrix = Matrix4.Identity;
        }


        public Face(List<Vertex> points, MyPrimitiveType type = MyPrimitiveType.Triangles)
        {
            Points = points ?? throw new ArgumentNullException(nameof(points));
            PType = type;
        }

        public Face(List<Vertex> points,Color color, MyPrimitiveType type = MyPrimitiveType.Triangles)
        {
            Points = points ?? throw new ArgumentNullException(nameof(points)); 
            Color = color;
            PType = type;
            
        }

        public void AddPoint(Vertex point)
        {
            Points.Add(point);
        }

        public Vertex GetPoint(int index)
        {
            if (index < 0 || index >= Points.Count)
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }

            return Points[index];
        }


        public void ClearPoints()
        {
            Points.Clear();
        }

        public void Translate(Vertex center,float x = 0, float y = 0, float z = 0)
        {
            
            _translationMatrix = Matrix4.CreateTranslation(x+center.X, y+center.Y, z+center.Z);
            Console.WriteLine("translation: " + _translationMatrix);
            //Console.WriteLine("center: " + center);
            //Console.WriteLine($"{x} {y} {z}");
            ApplyTransformationMatrix();  
        }

        public void Translate(float x = 0, float y = 0, float z = 0)
        {

            _translationMatrix = Matrix4.CreateTranslation(x ,y, z);
            //Console.WriteLine("translation: " + _translationMatrix);
            ApplyTransformationMatrix();
        }
        public void Scale(float x, float y, float z)
        {
            _scaleMatrix = Matrix4.CreateScale(x, y, z);
            ApplyTransformationMatrix();
        }

        public void Rotate(float x, float y, float z)
        {
            float radiansX = MathHelper.DegreesToRadians(x);
            float radiansY = MathHelper.DegreesToRadians(y);
            float radiansZ = MathHelper.DegreesToRadians(z);
            _rotationMatrix = Matrix4.CreateRotationZ(radiansZ) * Matrix4.CreateRotationY(radiansY)* Matrix4.CreateRotationX(radiansX) ;
            
            ApplyTransformationMatrix();
        }

        public void Rotate(Vertex center, float x, float y, float z)
        {
            Matrix4 translationMatrixToOrigin = Matrix4.CreateTranslation(-center.X, -center.Y, -center.Z);
            Matrix4 rotationMatrixX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(x));
            Matrix4 rotationMatrixY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(y));
            Matrix4 rotationMatrixZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(z));
            Matrix4 translationMatrixBack = Matrix4.CreateTranslation(center.X, center.Y, center.Z);

            _rotationMatrix = translationMatrixToOrigin * rotationMatrixX * rotationMatrixY * rotationMatrixZ * translationMatrixBack;
            ApplyTransformationMatrix();
        }

        public void Rotate(Vertex center, float angle, string axis)
        {
            Matrix4 translationMatrixToOrigin = Matrix4.CreateTranslation(-center.X, -center.Y, -center.Z);
            Matrix4 rot = Matrix4.Identity;
            switch (axis)
            {
                case "x":
                    rot = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
                    break;
                case "y":
                    rot = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angle));
                    break;
                case "z":
                    rot = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
                    break;
            }
            
            Matrix4 translationMatrixBack = Matrix4.CreateTranslation(center.X, center.Y, center.Z);

            _rotationMatrix = translationMatrixToOrigin * rot * translationMatrixBack;
            
            ApplyTransformationMatrix();
        }

        private void ApplyTransformationMatrix()
        {
            _trans = _translationMatrix * _rotationMatrix * _scaleMatrix;
        }
        
        public void Draw(Vertex center)
        {
            //Console.WriteLine("drawing: ");

            GL.PushMatrix();
            GL.MultMatrix(ref _trans);
            
            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(Color);

            foreach (var point in Points)
            {
                //GL.Vertex3(point.X + center.X, point.Y+center.Y, point.Z+center.Z);
                GL.Vertex3(point.X, point.Y , point.Z );
            }
            GL.End();
            
            GL.PopMatrix();
        }
    }

}
