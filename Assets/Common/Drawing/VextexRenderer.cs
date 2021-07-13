using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Numerics;
using Common.Core.Colors;

namespace Common.Unity.Drawing
{

    public class VertexRenderer : BaseRenderer
    {

        public VertexRenderer(float size)
        {
            Size = size;
            Orientation = DRAW_ORIENTATION.XY;
        }

        public VertexRenderer(float size, DRAW_ORIENTATION orientation)
        {
            Size = size;
            Orientation = orientation;
        }

        public float Size = 0.1f;

        public void Load(Vector2 vertex)
        {
            var v = vertex;

            if (Orientation == DRAW_ORIENTATION.XY)
                m_vertices.Add(v);
            else if (Orientation == DRAW_ORIENTATION.XZ)
                m_vertices.Add(new Vector4(v.x, 0, v.y, 1));

            m_colors.Add(Color);
        }

        public void Load(Vector2 vertex, Color color)
        {
            var v = vertex;

            if (Orientation == DRAW_ORIENTATION.XY)
                m_vertices.Add(v);
            else if (Orientation == DRAW_ORIENTATION.XZ)
                m_vertices.Add(new Vector4(v.x, 0, v.y, 1));

            m_colors.Add(color);
        }

        public void Load(IList<Vector2> vertices)
        {
            for(int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];

                if (Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_vertices.Add(new Vector4(v.x, 0, v.y, 1));

                m_colors.Add(Color);
            }
        }

        public void Load(IList<Vector2> vertices, IList<Color> colors)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];

                if (Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_vertices.Add(new Vector4(v.x, 0, v.y, 1));

                m_colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector2> vertices, Color color)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];

                if (Orientation == DRAW_ORIENTATION.XY)
                    m_vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_vertices.Add(new Vector4(v.x, 0, v.y, 1));

                m_colors.Add(color);
            }
        }

        public void Load(Vector3 vertex)
        {
            m_vertices.Add(vertex);
            m_colors.Add(Color);
        }

        public void Load(Vector3 vertex, Color color)
        {
            m_vertices.Add(vertex);
            m_colors.Add(color);
        }

        public  void Load(IList<Vector3> vertices)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                m_vertices.Add(vertices[i]);
                m_colors.Add(Color);
            }
        }

        public void Load(IList<Vector3> vertices, IList<Color> colors)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                m_vertices.Add(vertices[i]);
                m_colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector3> vertices, Color color)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                m_vertices.Add(vertices[i]);
                m_colors.Add(color);
            }
        }

        public void Load(Vector4 vertex)
        {
            m_vertices.Add(vertex);
            m_colors.Add(Color);
        }

        public void Load(Vector4 vertex, Color color)
        {
            m_vertices.Add(vertex);
            m_colors.Add(color);
        }

        public void Load(IList<Vector4> vertices)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                m_vertices.Add(vertices[i]);
                m_colors.Add(Color);
            }
        }

        public void Load(IList<Vector4> vertices, IList<Color> colors)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                m_vertices.Add(vertices[i]);
                m_colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector4> vertices, Color color)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                m_vertices.Add(vertices[i]);
                m_colors.Add(color);
            }
        }

        public override void Draw(Camera camera, Matrix4x4 localToWorld)
        {
            float size = Size;

            if (camera.orthographic && ScaleOnZoom)
                size *= camera.orthographicSize / 10.0f;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.QUADS);

            switch (Orientation)
            {
                case DRAW_ORIENTATION.XY:
                    DrawXY(size);
                    break;

                case DRAW_ORIENTATION.XZ:
                    DrawXZ(size);
                    break;
            }

            GL.End();

            GL.PopMatrix();
        }

        private  void DrawXY(float size)
        {
            float half = size * 0.5f;
            for (int i = 0; i < m_vertices.Count; i++)
            {
                float x = m_vertices[i].x;
                float y = m_vertices[i].y;
                float z = m_vertices[i].z;
                Color color = m_colors[i];

                GL.Color(color);
                GL.Vertex3(x + half, y + half, z);
                GL.Vertex3(x + half, y - half, z);
                GL.Vertex3(x - half, y - half, z);
                GL.Vertex3(x - half, y + half, z);
            }
        }

        private  void DrawXZ(float size)
        {
            float half = size * 0.5f;
            for (int i = 0; i < m_vertices.Count; i++)
            {
                float x = m_vertices[i].x;
                float y = m_vertices[i].y;
                float z = m_vertices[i].z;
                Color color = m_colors[i];

                GL.Color(color);
                GL.Vertex3(x + half, y, z + half);
                GL.Vertex3(x + half, y, z - half);
                GL.Vertex3(x - half, y, z - half);
                GL.Vertex3(x - half, y, z + half);
            }
        }
      
    }

}
