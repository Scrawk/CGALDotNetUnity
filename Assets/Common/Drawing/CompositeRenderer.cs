using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Unity.Drawing
{

    public class CompositeRenderer
    {

        public CompositeRenderer()
        {
            Renderers = new List<BaseRenderer>();
        }

        public string Name { get; set; }

        private List<BaseRenderer> Renderers { get; set; }

        public void Add(BaseRenderer renderer)
        {
            Renderers.Add(renderer);
        }

        public bool Remove(BaseRenderer renderer)
        {
            return Renderers.Remove(renderer);
        }

        public void Draw()
        {
            for (int i = 0; i < Renderers.Count; i++)
                Renderers[i].Draw();
        }

        public void Clear()
        {
            Renderers.Clear();
        }

        public void ClearRenderers()
        {
            for (int i = 0; i < Renderers.Count; i++)
                Renderers[i].Clear();
        }

        public void Translate(Vector3 translation, string name = "")
        {
            Matrix4x4 mat = Matrix4x4.Translate(translation);
            for (int i = 0; i < Renderers.Count; i++)
            {
                if (name == "" || Renderers[i].Name == name)
                    Renderers[i].LocalToWorld = mat;
            }
        }

        public void SetLocalToWorld(Matrix4x4 mat, string name = "")
        {
            for (int i = 0; i < Renderers.Count; i++)
            {
                if (name == "" || Renderers[i].Name == name)
                    Renderers[i].LocalToWorld = mat;
            }
        }

        /*
        public void Load(IList<Vector2> points, string name = "")
        {
            for (int i = 0; i < Renderers.Count; i++)
            {
                if (name == "" || Renderers[i].Name == name)
                    Renderers[i].Load(points);
            }
        }
        */

        public void SetColor(Color color, string name = "")
        {
            for (int i = 0; i < Renderers.Count; i++)
            {
                if (name == "" || Renderers[i].Name == name)
                    Renderers[i].SetColor(color);
            }
        }

        public void SetColor<T>(Color color, string name = "") where T : BaseRenderer
        {
            for (int i = 0; i < Renderers.Count; i++)
            {
                if(Renderers[i] is T r && (name == "" || r.Name == name))
                    r.SetColor(color);
            }
        }

        public void SetEnabled(bool enabled, string name = "")
        {
            for (int i = 0; i < Renderers.Count; i++)
            {
                if (name == "" || Renderers[i].Name == name)
                    Renderers[i].Enabled = enabled;
            }
        }

        public void SetEnabled<T>(bool enabled, string name = "") where T : BaseRenderer
        {
            for (int i = 0; i < Renderers.Count; i++)
            {
                if (Renderers[i] is T r && (name == "" || r.Name == name))
                    r.Enabled = enabled;
            }
        }

        public void SetSize(float size, string name = "") 
        {
            for (int i = 0; i < Renderers.Count; i++)
            {
                if (Renderers[i] is VertexRenderer r && (name == "" || r.Name == name))
                    r.Size = size;
            }
        }

        public void SetRadius(float radius, string name = "")
        {
            for (int i = 0; i < Renderers.Count; i++)
            {
                if (Renderers[i] is CircleRenderer r && (name == "" || r.Name == name))
                    r.SetRadius(radius);
            }
        }

        public void SetFill(bool fill, string name = "")
        {
            for (int i = 0; i < Renderers.Count; i++)
            {
                if (Renderers[i] is CircleRenderer r && (name == "" || r.Name == name))
                    r.Fill = fill;
            }
        }

    }
}
