using UnityEngine;
using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Unity.Drawing
{

    public class CircleRenderer : BaseRenderer
    {

        private  List<Vector4> m_line = new List<Vector4>();

        private  List<float> m_radii = new List<float>();

        public CircleRenderer()
        {

        }

        public CircleRenderer(DRAW_ORIENTATION orientation)
        {
            Orientation = orientation;
        }

        public int Segments = 16;

        public override void Clear()
        {
            base.Clear();
            m_radii.Clear();
        }

        public void Load(Vector2 position, float radius)
        {
            m_radii.Add(radius);

            if (Orientation == DRAW_ORIENTATION.XY)
                m_vertices.Add(position.xy01());
            else if (Orientation == DRAW_ORIENTATION.XZ)
                m_vertices.Add(position.x0y1());

            m_colors.Add(Color);
        }

        public void Load(Vector3 position, float radius)
        {
            m_radii.Add(radius);
            m_vertices.Add(position);
            m_colors.Add(Color);
        }

        public override void Draw(Camera camera, Matrix4x4 localToWorld)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);

            for (int j = 0; j < m_vertices.Count; j++)
            {
                Vector4 center = m_vertices[j];
                Color color = m_colors[j];
                float radius = m_radii[j];
                if (radius <= 0) continue;

                m_line.Clear();
                for (int i = 0; i < Segments; i++)
                {
                    double theta = 2.0 * Math.PI * i / Segments;
                    float x = radius * (float)Math.Cos(theta);
                    float y = radius * (float)Math.Sin(theta);

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
                    GL.Vertex(m_line[(i+1) % Segments]);
                }
            }

            GL.End();
            GL.PopMatrix();

        }

    }

}