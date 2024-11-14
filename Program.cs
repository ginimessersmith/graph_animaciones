using Graphic3D.Models;
using Graphic3D.Rendering;
using Graphic3D.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphic3D
{
    static class Program
    {

        [STAThread]
        static void Main(string[] args)
        {

            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/
            using (Window game = new Window())
            {
                game.Run();
            }


        }

        static void test1()
        {
            string currentDirectory = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
            Console.WriteLine(currentDirectory);
            string current = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine(current);

            string filePath = Path.Combine(currentDirectory, "Program.cs");
            Console.WriteLine(filePath);
            string directoryPath = Path.GetDirectoryName(filePath);
            Console.WriteLine(directoryPath);
            /*string currentDirectory = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
            string json = File.ReadAllText(currentDirectory + "\\scene.json");
            var scene = JsonConvert.DeserializeObject<Scene>(json);
            Console.WriteLine(scene);*/
        }
    }
}
