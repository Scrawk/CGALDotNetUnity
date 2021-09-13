using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;

namespace CGALDotNetUnity.Polygons
{

    public class CreatePolygonWithHolesExample : InputBehaviour
    {
        private Color redColor = new Color32(200, 80, 80, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Point2d? Point;

        private PolygonWithHoles2<EEK> Polygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        private bool AddHoles = true;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.POLYGON);
            Renderers = new Dictionary<string, CompositeRenderer>();
        }

        protected override void OnInputComplete(List<Point2d> points)
        {

            if(Polygon == null)
            {
                var boundary = new Polygon2<EEK>(points.ToArray());

                if (boundary.IsSimple)
                {
                    if (!boundary.IsCounterClockWise)
                        boundary.Reverse();

                    Polygon = new PolygonWithHoles2<EEK>(boundary);

                    Renderers["PolygonOutline"] = FromPolygon(Polygon, faceColor, lineColor, pointColor, PointSize);
                }
            }
            else if(AddHoles)
            {
                var hole = new Polygon2<EEK>(points.ToArray());

                if (!hole.IsClockWise)
                    hole.Reverse();

                if (PolygonWithHoles2.IsValidHole(Polygon, hole))
                {
                    Polygon.AddHole(hole);

                    Renderers["PolygonBody"] = FromPolygon(Polygon, faceColor);
                    Renderers["PolygonOutline"] = FromPolygon(Polygon, lineColor, pointColor, PointSize);

                    int holes = Polygon.HoleCount;
                    Renderers["Hole " + holes] = FromPolygon(hole, lineColor, pointColor, PointSize);
                }
                else
                {
                    Debug.Log("Hole was not valid.");
                }
            }

            InputPoints.Clear();
        }

        protected override void OnCleared()
        {
            Point = null;
            Polygon = null;
            Renderers.Clear();
            AddHoles = true;
            InputPoints.Clear();
            SetInputMode(INPUT_MODE.POLYGON);
        }

        protected override void OnLeftClickDown(Point2d point)
        {
            Point = point;
            Renderers["Point"] = FromPoints(new Point2d[] { Point.Value }, lineColor, redColor, PointSize);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.F1))
            {
                AddHoles = true;
                SetInputMode(INPUT_MODE.POLYGON);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                AddHoles = false;
                SetInputMode(INPUT_MODE.POINT_CLICK);
            }
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

            if (Polygon == null)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
            }
            else if (AddHoles)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Add holes to polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
                GUI.Label(new Rect(10, 70, textLen, textHeight), "F2 to stop adding holes and F1 to start adding holes again.");
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Holes must be simple and not intersect the polygon boundary or other holes.");
            }
            else
            {

                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Hole Count = " + Polygon.HoleCount);

                bool isSimple = Polygon.FindIfSimple(POLYGON_ELEMENT.BOUNDARY);
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Is Simple = " + isSimple);

                if (isSimple)
                {
                    GUI.Label(new Rect(10, 70, textLen, textHeight), "Is Convex = " + Polygon.FindIfConvex(POLYGON_ELEMENT.BOUNDARY));
                    GUI.Label(new Rect(10, 90, textLen, textHeight), "Area = " + Polygon.FindArea(POLYGON_ELEMENT.BOUNDARY));
                    GUI.Label(new Rect(10, 110, textLen, textHeight), "Orientation = " + Polygon.FindOrientation(POLYGON_ELEMENT.BOUNDARY));

                    if (Point != null)
                    {
                        GUI.Label(new Rect(10, 150, textLen, textHeight), "Contains point = " + Polygon.ContainsPoint(Point.Value));
                    }
                    else
                        GUI.Label(new Rect(10, 130, textLen, textHeight), "Click to test contains point.");
                }

            }

        }



    }
}
