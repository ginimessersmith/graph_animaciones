using Graphic3D.Models;
using Graphic3D.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Windows.Forms;

namespace Graphic3D
{
    public class CustomGLControl: GLControl
    {
        
        private Scene scene = new Scene();
        public IObject obj = new IObject();

        public CustomGLControl()
        : base()
        {
            Resize += OnResize;
            Paint += OnPaint;
            //InitializeScene();
        }

        public void LoadScene(Scene scene)
        {
            this.scene = scene ?? throw new ArgumentNullException(nameof(scene), "Scene cannot be null."); ;
        }
        
        private void OnPaint(object sender, PaintEventArgs e)
        {
            GL.ClearColor(0.3f, 0.4f, 0.5f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.LoadIdentity();

            //obj.Draw();
            scene.Draw();
            Console.WriteLine("onpaint.....");

            SwapBuffers();
        }

        private void OnResize(object sender, EventArgs e)
        {
            float aspectRatio = (float)Width / Height;
            float d = 50;

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            if (aspectRatio >= 1.0f)
            {
                GL.Ortho(-d * aspectRatio, d * aspectRatio, -d, d, -d, d);
            }
            else
            {
                GL.Ortho(-d, d, -d / aspectRatio, d / aspectRatio, -d, d);
            }

            GL.MatrixMode(MatrixMode.Modelview);
        }

        
    }
}
