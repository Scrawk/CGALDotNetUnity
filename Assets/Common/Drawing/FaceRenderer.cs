using UnityEngine;
using System;
using System.Collections.Generic;

namespace Common.Unity.Drawing
{

    public enum FACE_MODE { TRIANGLES = 3, QUADS = 4 };

    public class FaceRenderer : BaseRenderer
    {

        public FaceRenderer()
        {
            FaceMode = FACE_MODE.TRIANGLES;
            Orientation = DRAW_ORIENTATION.XY;
        }

        public FaceRenderer(FACE_MODE faceMode, DRAW_ORIENTATION orientation)
        {
            FaceMode = faceMode;
            Orientation = orientation;
        }

        public  FACE_MODE FaceMode = FACE_MODE.TRIANGLES;

        public void Load(Vector2 a, Vector2 b, Vector2 c)
        {
            SetFaceIndices(3, null);

            if (Orientation == DRAW_ORIENTATION.XY)
            {
                Vertices.Add(a.xy01());
                Vertices.Add(b.xy01());
                Vertices.Add(c.xy01());
            }
            else if (Orientation == DRAW_ORIENTATION.XZ)
            {
                Vertices.Add(a.x0y1());
                Vertices.Add(b.x0y1());
                Vertices.Add(c.x0y1());
            }

            Colors.Add(Color);
            Colors.Add(Color);
            Colors.Add(Color);
        }

        public  void Load(IList<Vector2> vertices, IList<int> indices = null)
        {
            SetFaceIndices(vertices.Count, indices);

            foreach (var v in vertices)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(v.xy01());
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(v.x0y1());
   
                Colors.Add(Color);
            }
        }

        public void Load(Vector3 a, Vector3 b, Vector3 c)
        {
            SetFaceIndices(3, null);

            Vertices.Add(a);
            Vertices.Add(b);
            Vertices.Add(c);

            Colors.Add(Color);
            Colors.Add(Color);
            Colors.Add(Color);
        }

        public void Load(IList<Vector3> vertices, IList<int> indices = null)
        {
            SetFaceIndices(vertices.Count, indices);

            foreach (var v in vertices)
            {
                Vertices.Add(v);
                Colors.Add(Color);
            }
        }

        public override void Draw(Camera camera, Matrix4x4 localToWorld)
        {

            switch (FaceMode)
            {
                case FACE_MODE.QUADS:
                    DrawVerticesAsQuads(camera, localToWorld);
                    break;

                case FACE_MODE.TRIANGLES:
                    DrawVerticesAsTriangles(camera, localToWorld);
                    break;
            }

        }

        private  void DrawVerticesAsQuads(Camera camera, Matrix4x4 localToWorld)
        {
            const int stride = 4;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix * localToWorld);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.QUADS);

            int vertexCount = Vertices.Count;

            for (int i = 0; i < Indices.Count / stride; i++)
            {
                int i0 = Indices[i * stride + 0];
                int i1 = Indices[i * stride + 1];
                int i2 = Indices[i * stride + 2];
                int i3 = Indices[i * stride + 3];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;
                if (i3 < 0 || i3 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);
                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);
            }

            GL.End();

            GL.PopMatrix();
        }

        private  void DrawVerticesAsTriangles(Camera camera, Matrix4x4 localToWorld)
        {
            const int stride = 3;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix * localToWorld);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.TRIANGLES);
            GL.Color(Color);

            int vertexCount = Vertices.Count;

            for (int i = 0; i < Indices.Count / stride; i++)
            {
                int i0 = Indices[i * stride + 0];
                int i1 = Indices[i * stride + 1];
                int i2 = Indices[i * stride + 2];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);
            }

            GL.End();

            GL.PopMatrix();
        }

    }

}