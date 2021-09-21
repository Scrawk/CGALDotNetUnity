using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using Common.Unity.Utility;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Triangulations;

namespace CGALDotNetUnity.Triangulations
{

    public class VoronoiExample : InputBehaviour
    {
        private Color blueColor = new Color32(80, 80, 200, 255);

        private Color redColor = new Color32(200, 80, 80, 255);

        private Color greenColor = new Color32(80, 200, 80, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        private DelaunayTriangulation2<EEK> triangulation;

        private bool DrawTriangulation = true;

        private bool DrawVoronoiSegments = true;

        private bool DrawVoronoiRays = false;

        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();

            int width = 20;
            int height = 20;
            int radius = 2;
            int samples = 1000;

            var points = CreateBoundaryPoints(width, height, radius);
            FillPoints(points, width, height, radius, samples);
            ExpandPoints(points, width, height, radius);
            TranslatePoints(points, width, height);

            triangulation = new DelaunayTriangulation2<EEK>();
            triangulation.InsertPoints(points.ToArray());

            CreateRenderers();
        }

        private void CreateRenderers()
        {
            Renderers.Clear();

            if(DrawTriangulation)
            {
                Renderers["Triangulation"] = Draw().Outline(triangulation, blueColor).PopRenderer();
            }
            
            if(DrawVoronoiSegments)
            {
                var segments = triangulation.GetVoronoiSegments();
                Renderers["Segments"] = Draw().Outline(segments, redColor).PopRenderer();
            }

            if(DrawVoronoiRays)
            {
                var rays = triangulation.GetVoronoiRays();
                Renderers["Rays"] = Draw().Outline(rays, redColor).PopRenderer();
            }
        }

        private List<Point2d> CreateBoundaryPoints(int width, int height, int radius)
        {
            var points = new List<Point2d>();

            points.Add(new Point2d(0, 0));
            points.Add(new Point2d(width, 0));
            points.Add(new Point2d(0, height));
            points.Add(new Point2d(width, height));

            for (int i = radius; i <= width - radius; i += radius)
            {
                points.Add(new Point2d(i, 0));
                points.Add(new Point2d(i, width));

                points.Add(new Point2d(0, i));
                points.Add(new Point2d(width, i));
            }

            return points;
        }

        private void FillPoints(List<Point2d> points, int width, int height, double radius, int samples)
        {

            for (int i = 0; i < samples; i++)
            {
                var point = new Point2d();
                point.x = UnityEngine.Random.Range(0, width);
                point.y = UnityEngine.Random.Range(0, height);

                if (!WithInRadius(point, points, radius))
                {
                    points.Add(point);
                }
            }
        }

        private List<Point2d> ExpandPoints(List<Point2d> points, int width, int height, int radius)
        {
            points.Add(new Point2d(-radius, -radius));
            points.Add(new Point2d(width + radius, -radius));
            points.Add(new Point2d(-radius, height + radius));
            points.Add(new Point2d(width + radius, height + radius));

            for (int i = 0; i <= width; i += radius)
            {
                points.Add(new Point2d(i, -radius));
                points.Add(new Point2d(i, width + radius));

                points.Add(new Point2d(-radius, i));
                points.Add(new Point2d(width + radius, i));
            }

            return points;
        }

        private bool WithInRadius(Point2d point, List<Point2d> points, double radius)
        {
            double radius2 = radius * radius;
            foreach (var p in points)
            {
                if (Point2d.SqrDistance(point, p) < radius2)
                    return true;
            }

            return false;
        }

        private void TranslatePoints(List<Point2d> points, int width, int height)
        {
            var translate = new Point2d(width * 0.5, height * 0.5);
            for (int i = 0; i < points.Count; i++)
                points[i] = points[i] - translate;
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.F1))
            {
                DrawTriangulation = !DrawTriangulation;
                CreateRenderers();
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                DrawVoronoiSegments = !DrawVoronoiSegments;
                CreateRenderers();
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                DrawVoronoiRays = !DrawVoronoiRays;
                CreateRenderers();
            }

        }

        private void OnPostRender()
        {
            DrawGrid();

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

        }

        protected void OnGUI()
        {
            int textLen = 1000;
            int textHeight = 25;
            GUI.color = Color.black;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "F1 to draw triangulation.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "F2 to draw voronoi segment.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "F3 to draw voronoi rays");

        }

    }
}
