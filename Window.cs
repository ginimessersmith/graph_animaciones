using System;

using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System.Collections.Generic;

using Graphic3D.Utils;
using Graphic3D.Models;
using Graphic3D.Rendering;
using System.Drawing;
using System.Windows.Forms;
using Graphic3D.Examples;

namespace Graphic3D
{
    public class Window : GameWindow
    {
        /*private Vector3 cameraPosition = new Vector3(-5, 5, 20);
        private Vector3 cameraFront = Vector3.UnitZ;
        private Vector3 cameraUp = Vector3.UnitY;
        private float cameraSpeed = 0.1f;
         */
        private float yaw = -90f; // Ángulo de rotación en yaw
        private float pitch = 0f; // Ángulo de rotación en pitch

        private float angle = 0.0f;
        private Scene scene;

        //private Button saveButton, loadButton;

        public Window(int width = 800, int height = 500, string title = "Objecto 3D")
            : base(width, height, GraphicsMode.Default, title)
        {
            // tamano de la pantalla
            int screenWidth = DisplayDevice.Default.Width;
            int screenHeight = DisplayDevice.Default.Height;

            // centro de la pantalla
            int windowX = (screenWidth - Width) / 2;
            int windowY = (screenHeight - Height) / 2;
            Location = new Point(windowX, windowY);
        }

        protected override void OnLoad(EventArgs e)
        {

            GL.ClearColor(0.3f, 0.4f, 0.5f, 1f);
            GL.Enable(EnableCap.DepthTest);
            InitObjects();
            InitAnimation();
            base.OnLoad(e);
        }

        private Animation animation;
        private AnimationController animationController;
        private IObject obj;
        Models.Action action1;
        private void InitAnimation()
        {
            Part cube = new Part(StandardGeometry.CreateCube(4f, 4f, 4f,Color.Black));
            obj = new IObject(new Vertex(-35f, 10f, 10f));
            obj.addPart("cube", cube);
            obj.Translate();
            //VertexHelper.SetVertexToAbsObject(obj);
            action1 = new Models.Action(obj,4, new Vector3(-13, 13, 0), 60,startPosition : new Vector3(-35f, 10f, 10f), finalPosition:new Vector3(-10f,10f,2f), positionOffset: new Vector3(-20f, 0f, 0f), rotationAngle: new Vector3(25f, 0f, 0f), scaleFactor: new Vector3(1f, 1f, 1f));
            
            Models.Action action2 = new Models.Action(obj,4, new Vector3(-13, 13, 0), 60, startPosition: new Vector3(-10f, 10f, 2f), finalPosition: new Vector3(12f, 10.75f, 0f), positionOffset: new Vector3(-20f, 0f, 0f), rotationAngle: new Vector3(25f, 0f, 0f), scaleFactor: new Vector3(1f, 1f, 1f));
            
            animation = new Animation();

            animation.AddAction(action1);
            animation.AddAction(action2);
            animationController = new AnimationController(animation);
        }

        
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.LoadIdentity();
            
            GL.Translate(0.0f, 0.0f, -75.0f);
            //GL.Rotate(angle, 1.0, 0.0, 0.0);

            //animation.Update((float)e.Time);
            
            //action1.Update((float)e.Time,obj);
            //obj.Rotate(new Vertex(0f,0f,0f),angle,"x");
            obj.Draw();
            scene.Draw();

            angle += 1.0f;
            if (angle > 360)
                angle -= 360.0f;

            Context.SwapBuffers();
            base.OnRenderFrame(e);

        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Control && e.Key == Key.S) // Ctrl + S
            {
                SaveFile();
            }
            else if (e.Control && e.Key == Key.O) // Ctrl + O
            {
                LoadFile();
            }
            else if (e.Control && e.Key == Key.X) // Ctrl + P
            {
                animationController.Pause();
            }
            else if (e.Control && e.Key == Key.C) // Ctrl + R
            {
                animationController.Stop();
            }
            else if (e.Control && e.Key == Key.Z) // Ctrl + I
            {
                animationController.Start();
            }
        }

        

        protected override void OnResize(EventArgs e)
        {
            float aspectRatio = (float)Width / Height;
            float d = 50;

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), (float)Width / Height, 0.1f, 1000f);
            GL.LoadMatrix(ref projectionMatrix);
            //GL.Ortho(-d, d, -d, d, -d, d);
            
            /*if (aspectRatio >= 1.0f)
            {
                GL.Ortho(-d * aspectRatio, d * aspectRatio, -d, d, -d, d);
            }
            else
            {
                GL.Ortho(-d, d, -d / aspectRatio, d / aspectRatio, -d, d);
            }*/

            GL.MatrixMode(MatrixMode.Modelview);


            //Console.WriteLine($"e: {Width}  {Height}");

            base.OnResize(e);
        }

        private void SaveFile()
        {

            string filePath = null;

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON files (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;
                }
            }
            Console.WriteLine(filePath);
            if (!string.IsNullOrEmpty(filePath))
            {
                JsonHelper.SaveObjectToJsonFile<Scene>(scene, filePath);
                Console.WriteLine("JSON data saved successfully.");
            }
        }

        private void LoadFile()
        {
            // Console.WriteLine("load button click");
            string filePath = null;

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }
            Console.WriteLine(filePath);
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    scene = JsonHelper.LoadObjectFromJsonFile<Scene>(filePath);
                    //scene.Renderer = new OpenTKRenderer();
                    Console.WriteLine("JSON data loaded successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading JSON: {ex.Message}");
                }
            }
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            

            base.OnUpdateFrame(e);
        }


        private void InitObjects()
        {
            //---- -------tv
            Part pantalla = new Part(StandardGeometry.CreateCube(32f, 18f, 4f, Color.Gray));
            Part soporte = new Part(StandardGeometry.CreateCube(4f, 8f, 4f, Color.Black), new Vertex(0f, -13f, 0f));
            IObject tv = new IObject();
            tv.addPart("pantalla", pantalla);
            tv.addPart("soporte", soporte);

            //--- --------- florero
            Part cuello = new Part();
            cuello.AddFace("cuello", new Face(PlanarGeometry.createCurvedSurface(4f, 2f), Color.BlueViolet));
            Part cuerpo = new Part(StandardGeometry.CreateSphere(4f, 36), new Vertex(0f, -3.25f, 0f));
            IObject vase = new IObject(new Vertex(12f, 16f, 0));
            vase.addPart("cuello", cuello);
            vase.addPart("cuerpo", cuerpo);

            //--------- parlante
            Part diseno = new Part(new Vertex(0f, -2f, 3.25f));
            diseno.AddFace("1", new Face(PlanarGeometry.createCircle(3f, new Vertex(0f, 0f, 0f)), Color.DarkGray, MyPrimitiveType.TriangleFan));
            //Part diseno = new Part(StandardGeometry.CreateCube(6f, 6f, 1f, Color.DarkGray), new Vertex(0f, -1.5f, 3f));
            Part parlCuerpo = new Part(StandardGeometry.CreateCube(8f, 12f, 6f, Color.Black));
            IObject speaker = new IObject(new Vertex(-20f, -12.0f, -0.0f));
            speaker.addPart("diseno", diseno);
            speaker.addPart("cuerpo", parlCuerpo);

            scene = new Scene(new Vertex(0f, 0f, 0f));
            scene.AddObject("televisor", tv);
            scene.AddObject("parlante", speaker);
            scene.AddObject("florero", vase);

            scene.Translate();
        }


    }
    
}




