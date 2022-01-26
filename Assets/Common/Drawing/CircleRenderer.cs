using UnityEngine;
using System;
using System.Collections.Generic;

namespace Common.Unity.Drawing
{

    public class CircleRenderer : BaseRenderer
    {

        private  List<Vector4> m_line = new List<Vector4>();

        private  List<float> m_radii = new List<float>();

        public CircleRenderer()
        {

        }

        public CircleRenderer(DRAW_ORIENTATION orientation, bool fill, float radius)
        {
            Orientation = orientation;
            Fill = fill;
            DefaultRadius = radius;
        }

        public int Segments = 16;

        public bool Fill = false;

        public float DefaultRadius = 0.1f;

        public override void Clear()
        {
            base.Clear();
            m_radii.Clear();
        }

        public void SetRadius(float radius)
        {
            DefaultRadius = radius;
            for (int i = 0; i < m_radii.Count; i++)
                m_radii[i] = radius;
        }

        public void Load(Vector2 position)
        {
            Load(position, DefaultRadius);
        }

        public void Load(Vector2 position, float radius)
        {
            m_radii.Add(radius);

            float x = position.x;
            float y = position.y;

            if (Orientation == DRAW_ORIENTATION.XY)
                Vertices.Add(new Vector4(x, y, 0, 1));
            else if (Orientation == DRAW_ORIENTATION.XZ)
                Vertices.Add(new Vector4(x, 0, y, 1));

            Colors.Add(DefaultColor);
        }

        public void Load(IList<Vector2> positions)
        {
            Load(positions, DefaultRadius);
        }

        public void Load(IList<Vector2> positions, float radius)
        {

            for (int i = 0; i < positions.Count; i++)
            {
                m_radii.Add(radius);

                float x = positions[i].x;
                float y = positions[i].y;

                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(new Vector4(x, y, 0, 1));
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(x, y, 0, 1));

                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector2> positions, IList<float> radius)
        {

            for (int i = 0; i < positions.Count; i++)
            {
                m_radii.Add(radius[i]);

                float x = positions[i].x;
                float y = positions[i].y;

                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(new Vector4(x, y, 0, 1));
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(x, 0, y, 1));

                Colors.Add(DefaultColor);
            }
        }

        public void Load(Vector3 position)
        {
            Load(position, DefaultRadius);
        }

        public void Load(Vector3 position, float radius)
        {
            m_radii.Add(radius);
            Vertices.Add(position);
            Colors.Add(DefaultColor);
        }

        public void Load(IList<Vector3> positions)
        {
            Load(positions, DefaultRadius);
        }

        public void Load(IList<Vector3> positions, float radius)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                m_radii.Add(radius);
                Vertices.Add(positions[i]);
                Colors.Add(DefaultColor);
            }
        }

        protected override void OnDraw(Camera camera, Matrix4x4 localToWorld)
        {
            if (Segments < 3) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(Fill ? GL.TRIANGLES : GL.LINES);

            for (int i = 0; i < Vertices.Count; i++)
            {
                DrawCircle(i);
            }

            GL.End();
            GL.PopMatrix();

        }

        private void DrawCircle(int index)
        {
            Vector4 center = Vertices[index];
            Color color = Colors[index];
            float radius = m_radii[index];
            if (radius <= 0) return;

            m_line.Clear();
            for (int i = 0; i < Segments; i++)
            {
                float theta = 2.0f * Mathf.PI * i / Segments;
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);

                if (Orientation == DRAW_ORIENTATION.XY)
                    m_line.Add(center + new Vector4(x, y, 0, 1));
                else
                    m_line.Add(center + new Vector4(x, 0, y, 1));
            }

            for (int i = 0; i < Segments; i++)
            {
                GL.Color(color);
                GL.Vertex(m_line[i]);
                GL.Color(color);
                GL.Vertex(m_line[(i + 1) % Segments]);

                if(Fill)
                {
                    GL.Color(color);
                    GL.Vertex(center);
                }
            }
        }


    }

}