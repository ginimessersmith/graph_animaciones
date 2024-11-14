using Graphic3D.Rendering;
using Graphic3D.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Graphic3D.Models
{
    public class Scene
    {
        public Dictionary<string, IObject> Objects { get; set; } = new Dictionary<string, IObject>();
        public Vertex Center { get; private set; } = new Vertex(0f,0f,0f);
        private Vertex _center = new Vertex(0f, 0f, 0f);

        public Scene()
        {

        }

        public Scene(Dictionary<string, IObject> objects)
        {
            Objects = objects ?? throw new ArgumentNullException(nameof(objects));
            
        }

        public Scene( Vertex center)
        {
            Center = center;
        }

        public Scene(Dictionary<string, IObject> objects,Vertex center)
        {
            Objects = objects ?? throw new ArgumentNullException(nameof(objects));
            
            Center = center;
        }

        public void AddObject(string key, IObject o)
        {
            o.Center += Center;
            Objects.Add(key, o);
        }

        public IObject GetObject(string key)
        {
            return Objects.ContainsKey(key)? Objects[key] : null;
        }

        public void Translate(float x=0, float y=0, float z=0)
        {
            _center = Center + new Vertex(x, y, z);
            foreach (var o in Objects.Values)
            {
                o.Translate(x,y,z);
            }
        }

        public void Scale(float x, float y, float z)
        {
            foreach (var o in Objects.Values)
            {
                o.Scale(x, y, z);
            }
        }

        public void Rotate(Vertex center,float x, float y, float z)
        {
            foreach (var obj in Objects.Values)
            {
                obj.Rotate(center,x, y, z);
            }
        }

        public void Draw()
        {
            foreach (var o in Objects.Values)
            {
                o.Draw();
            }
        }

        

        public override string ToString()
        {
            return $"{Objects}";
        }
    }
}
