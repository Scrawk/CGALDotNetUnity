using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNetGeometry.Marching;
using CGALDotNet.Triangulations;

namespace CGALDotNetUnity.Marching
{

    public class MarchingSquaresExample : InputBehaviour
    {
        private Color redColor = new Color32(200, 80, 80, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.NONE);
            Renderers = new Dictionary<string, CompositeRenderer>();

            PerformMarching();
        }

        private void PerformMarching()
        {
            int size = 20;
            int half = size / 2;
            Point2d translate = new Point2d(-half);
            
            var ms = new MarchingSquares();
            var tri = new ConstrainedTriangulation2<EEK>();

            var vertices = new List<Point2d>();
            var indices = new List<int>();

            ms.Generate(UnionSDF, size + 1, size+ 1, vertices, indices);

            var segments = new List<Segment2d>();
            for(int i = 0; i < indices.Count/2; i++)
            {
                int i0 = i * 2 + 0;
                int i1 = i * 2 + 1;

                var a = vertices[indices[i0]] + translate;
                var b = vertices[indices[i1]] + translate;

                segments.Add(new Segment2d(a, b));

                tri.InsertConstraint(a, b);
            }

            CreateRenderer(tri, translate);

            //Renderers["Tri"] = Draw().Outline(tri, lineColor).PopRenderer();
            //Renderers["Bounds"] = Draw().Outline(new Box2d(-half, half), lineColor).PopRenderer();
            Renderers["Segments"] = Draw().Outline(segments, lineColor).PopRenderer();
        }

        private double UnionSDF(Point2d point)
        {
            double sdf1 = CircleSDF(point);
            double sdf2 = BoxSDF(point);

            return Math.Min(sdf1, sdf2);
        }

        private double CircleSDF(Point2d point)
        {
            double radius = 5;
            Point2d Center = new Point2d(10);

            point = point - Center;
            return point.Magnitude - radius;
        }

        private double BoxSDF(Point2d point)
        {
            Point2d Min = new Point2d(2);
            Point2d Max = new Point2d(10);
            Point2d size = new Point2d(8);

            Point2d p = point - (Min+Max)*0.5;
            p.x = Math.Abs(p.x);
            p.y = Math.Abs(p.y);

            Point2d d = p - size * 0.5;
            Point2d max = Point2d.Max(d, 0);

            return max.Magnitude + Math.Min(Math.Max(d.x, d.y), 0.0);
        }

        private void CreateRenderer(ConstrainedTriangulation2<EEK> tri, Point2d translate)
        {
            var points = new Point2d[tri.VertexCount];
            tri.GetPoints(points, points.Length);

            var indices = new int[tri.IndiceCount];
            tri.GetIndices(indices, indices.Length);

            var indices2 = new List<int>();

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int i0 = i * 3 + 0;
                int i1 = i * 3 + 1;
                int i2 = i * 3 + 2;

                var a = points[indices[i0]] - translate;
                var b = points[indices[i1]] - translate;
                var c = points[indices[i2]] - translate;

                var center = (a + b + c) / 3.0;

                if(UnionSDF(center) < 0)
                {
                    indices2.Add(indices[i0]);
                    indices2.Add(indices[i1]);
                    indices2.Add(indices[i2]);
                }

            }

            Renderers["Trianguation"] = Draw().
                Faces(points, indices2, faceColor).
                PopRenderer();

        }

        private void OnPostRender()
        {
            DrawGrid();

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

        }

    }
}
