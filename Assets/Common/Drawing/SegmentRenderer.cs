using UnityEngine;
using System;
using System.Collections.Generic;

namespace Common.Unity.Drawing
{

    public enum LINE_MODE { LINES = 2, TRIANGLES = 3, TETRAHEDRON = 4 };

    public class SegmentRenderer : BaseRenderer
    {

        public SegmentRenderer()
        {
            LineMode = LINE_MODE.LINES;
            Orientation = DRAW_ORIENTATION.XY;
        }

        public SegmentRenderer(LINE_MODE lineMode, DRAW_ORIENTATION orientation)
        {
            LineMode = lineMode;
            Orientation = orientation;
        }

        public LINE_MODE LineMode { get; set; }

        public override void Load(IList<Vector2> vertices)
        {
            Load(vertices, null);
        }

        public void Load(IList<Vector2> vertices, IList<int> indices)
        {
            SetSegmentIndices(vertices.Count, indices);

            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];

                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(v.x, 0, v.y, 1));

                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector2> vertices, IList<Color> colors, IList<int> indices = null)
        {
            SetSegmentIndices(vertices.Count, indices);

            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];

                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(v.x, 0, v.y, 1));

                Colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector2> vertices, Color color, IList<int> indices = null)
        {
            SetSegmentIndices(vertices.Count, indices);

            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];

                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(v.x, 0, v.y, 1));

                Colors.Add(color);
            }
        }

        public void Load(Vector2 a, Vector2 b)
        {
            SetSegmentIndices(2, null);

            if (Orientation == DRAW_ORIENTATION.XY)
            {
                Vertices.Add(a);
                Vertices.Add(b);
            }
            else if (Orientation == DRAW_ORIENTATION.XZ)
            {
                Vertices.Add(new Vector4(a.x, 0, a.y, 1));
                Vertices.Add(new Vector4(b.x, 0, b.y, 1));
            }

            Colors.Add(DefaultColor);
            Colors.Add(DefaultColor);
        }

        public void Load(Vector2 a, Vector2 b, Color color)
        {
            SetSegmentIndices(2, null);

            if (Orientation == DRAW_ORIENTATION.XY)
            {
                Vertices.Add(a);
                Vertices.Add(b);
            }
            else if (Orientation == DRAW_ORIENTATION.XZ)
            {
                Vertices.Add(new Vector4(a.x, 0, a.y, 1));
                Vertices.Add(new Vector4(b.x, 0, b.y, 1));
            }

            Colors.Add(color);
            Colors.Add(color);
        }

        public override void Load(IList<Vector3> vertices)
        {
            Load(vertices, null);
        }

        public void Load(IList<Vector3> vertices, IList<int> indices)
        {
            SetSegmentIndices(vertices.Count, indices);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector3> vertices, IList<Color> colors, IList<int> indices = null)
        {
            SetSegmentIndices(vertices.Count, indices);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector3> vertices, Color color, IList<int> indices = null)
        {
            SetSegmentIndices(vertices.Count, indices);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(color);
            }
        }

        public void Load(Vector3 a, Vector3 b)
        {
            SetSegmentIndices(2, null);

            Vertices.Add(a);
            Colors.Add(DefaultColor);
            Vertices.Add(b);
            Colors.Add(DefaultColor);
        }

        public void Load(Vector3 a, Vector3 b, Color color)
        {
            SetSegmentIndices(2, null);

            Vertices.Add(a);
            Colors.Add(color);
            Vertices.Add(b);
            Colors.Add(color);
        }

        public void Load(IList<Vector4> vertices, IList<int> indices = null)
        {
            SetSegmentIndices(vertices.Count, indices);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector4> vertices, IList<Color> colors, IList<int> indices = null)
        {
            SetSegmentIndices(vertices.Count, indices);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector4> vertices, Color color, IList<int> indices = null)
        {
            SetSegmentIndices(vertices.Count, indices);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(color);
            }
        }

        public void Load(Vector4 a, Vector4 b)
        {
            SetSegmentIndices(2, null);

            Vertices.Add(a);
            Colors.Add(DefaultColor);
            Vertices.Add(b);
            Colors.Add(DefaultColor);
        }

        public void Load(Vector4 a, Vector4 b, Color color)
        {
            SetSegmentIndices(2, null);

            Vertices.Add(a);
            Colors.Add(color);
            Vertices.Add(b);
            Colors.Add(color);
        }

        protected override void OnDraw(Camera camera, Matrix4x4 localToWorld)
        {
            switch (LineMode)
            {
                case LINE_MODE.LINES:
                    DrawVerticesAsLines(camera, localToWorld);
                    break;

                case LINE_MODE.TRIANGLES:
                    DrawVerticesAsTriangles(camera, localToWorld);
                    break;

                case LINE_MODE.TETRAHEDRON:
                    DrawVerticesAsTetrahedron(camera, localToWorld);
                    break;
            }
        }

        private void DrawVerticesAsLines(Camera camera, Matrix4x4 localToWorld)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);

            int vertexCount = Vertices.Count;

            for (int i = 0; i < Indices.Count / 2; i++)
            {
                int i0 = Indices[i * 2 + 0];
                int i1 = Indices[i * 2 + 1];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
            }

            GL.End();

            GL.PopMatrix();
        }

        private void DrawVerticesAsTriangles(Camera camera, Matrix4x4 localToWorld)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix * localToWorld);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(DefaultColor);

            int vertexCount = Vertices.Count;

            for (int i = 0; i < Indices.Count / 3; i++)
            {
                int i0 = Indices[i * 3 + 0];
                int i1 = Indices[i * 3 + 1];
                int i2 = Indices[i * 3 + 2];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);

                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
            }

            GL.End();

            GL.PopMatrix();
        }

        private void DrawVerticesAsTetrahedron(Camera camera, Matrix4x4 localToWorld)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix * localToWorld);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(DefaultColor);

            int vertexCount = Vertices.Count;

            for (int i = 0; i < Indices.Count / 4; i++)
            {
                int i0 = Indices[i * 4 + 0];
                int i1 = Indices[i * 4 + 1];
                int i2 = Indices[i * 4 + 2];
                int i3 = Indices[i * 4 + 3];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;
                if (i3 < 0 || i3 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);

                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);

                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);

                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);
            }

            GL.End();

            GL.PopMatrix();
        }

    }

}