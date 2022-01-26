using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;
using CGALDotNet.Triangulations;
using CGALDotNet.Meshing;

namespace CGALDotNetUnity.Polygons
{

    public class CreatePolygonExample : InputBehaviour
    {

        private Color redColor = new Color32(200, 80, 80, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Point2d? Point;

        private Polygon2<EEK> Polygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.POLYGON);
            Renderers = new Dictionary<string, CompositeRenderer>();
        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            Polygon = new Polygon2<EEK>(points.ToArray());

            if (Polygon.IsSimple)
            {
                if (!Polygon.IsCounterClockWise)
                    Polygon.Reverse();

                SetInputMode(INPUT_MODE.POINT_CLICK);
                CreateRenderer("Polygon", Polygon);

                //Refine(Polygon);
            }
            else
            {
                CreateRenderer("Polygon", Polygon);
            }

            InputPoints.Clear();
        }

        private void Refine(Polygon2<EEK> polygon)
        {
            var tri = polygon.Refine(2);
            CreateRenderer("Refined", tri);
            CreateRenderer("Hull", tri.ComputeHull(), Color.yellow);
        }

        private void CreateRenderer(string name, Polygon2 polygon)
        {
            if(polygon.IsSimple)
            {
                Renderers[name] = Draw().
                Faces(polygon, faceColor).
                Outline(polygon, lineColor).
                Points(polygon, lineColor, pointColor).
                PopRenderer();
            }
            else
            {
                Renderers[name] = Draw().
                Faces(polygon, redColor).
                Outline(polygon, redColor).
                Points(polygon, lineColor, pointColor).
                PopRenderer();
            }
        }

        private void CreateRenderer(string name, Polygon2 polygon, Color col)
        {
            Renderers[name] = Draw().
            Outline(polygon, col).
            PopRenderer();
        }

        private void CreateRenderer(string name, BaseTriangulation2 triangulation)
        {
            Renderers[name] = Draw().
            Outline(triangulation, lineColor).
            PopRenderer();
        }

        protected override void OnCleared()
        {
            Point = null;
            Polygon = null;
            Renderers.Clear();
            InputPoints.Clear();
            SetInputMode(INPUT_MODE.POLYGON);
        }

        protected override void OnLeftClickDown(Point2d point)
        {
            Point = point;

            var array = new Point2d[] { Point.Value };

            Renderers["Point"] = Draw().
                Points(array, lineColor, redColor, PointSize).
                PopRenderer();
        }

        private void OnPostRender()
        {
            DrawGrid();
            DrawInput(lineColor, pointColor, PointSize);

            foreach (var renderer in Renderers.Values)
                renderer.Draw();
            
        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;
            GUI.color = Color.black;

            if (Polygon == null || !Polygon.IsSimple)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
            }
            else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Count = " + Polygon.Count);

                bool isSimple = Polygon.IsSimple;

                GUI.Label(new Rect(10, 50, textLen, textHeight), "Is Simple = " + isSimple);

                if (isSimple)
                {
                    GUI.Label(new Rect(10, 70, textLen, textHeight), "Is Convex = " + Polygon.FindIfConvex());
                    GUI.Label(new Rect(10, 90, textLen, textHeight), "Area = " + Polygon.FindArea());
                    GUI.Label(new Rect(10, 110, textLen, textHeight), "Orientation = " + Polygon.FindOrientation());

                    if (Point != null)
                    {
                        GUI.Label(new Rect(10, 130, textLen, textHeight), "Point oriented side = " + Polygon.OrientedSide(Point.Value));
                        GUI.Label(new Rect(10, 150, textLen, textHeight), "Point bounded side = " + Polygon.BoundedSide(Point.Value));
                        GUI.Label(new Rect(10, 170, textLen, textHeight), "Contains point = " + Polygon.ContainsPoint(Point.Value));
                    }
                    else
                        GUI.Label(new Rect(10, 130, textLen, textHeight), "Click to test point oriented side.");
                }
            }

        }



    }
}
