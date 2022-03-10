using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;

namespace CGALDotNetUnity.Polygons
{

    public class PolygonVisibilityExample : InputBehaviour
    {
        private Color regionColor = new Color32(200, 80, 80, 128);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Point2d? Point;

        private PolygonWithHoles2<EEK> Polygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        private bool AddHoles = true;

        protected override void Start()
        {
            //Init base class
            base.Start();

            //Set the input mode to polygons.
            //Determines when OnInputComplete is called.
            SetInputMode(INPUT_MODE.POLYGON);

            //Create the place to store the renderers that draw the shapes.
            Renderers = new Dictionary<string, CompositeRenderer>();
        }

        protected override void OnInputComplete(List<Point2d> points)
        {

            if (Polygon == null)
            {
                //Create the polygons boundary from the points.
                //We need to use EEK as a boolean op is used.
                var boundary = new Polygon2<EEK>(points.ToArray());

                //Polygon must be simple to continue.
                if (boundary.IsSimple)
                {
                    //The boundary must be ccw.
                    if (!boundary.IsCounterClockWise)
                        boundary.Reverse();

                    //Create the polygon
                    Polygon = new PolygonWithHoles2<EEK>(boundary);

                    //Create renderer to draw polygon.
                    CreateRenderer("Polygon", Polygon, faceColor);
                }
                else
                {
                    Debug.Log("Polygon was not simple.");
                }
            }
            else if (AddHoles)
            {
                //A polygon has already been created for the boundary
                //so this polygon must be a hole.
                var hole = new Polygon2<EEK>(points.ToArray());
                //holes must be cw.
                if (!hole.IsClockWise)
                    hole.Reverse();

                //Holes must be simple, cw and be contained in the
                //polygon and not intersect any other holes.
                if (PolygonWithHoles2.IsValidHole(Polygon, hole))
                {
                    Polygon.AddHole(hole);
                    int holes = Polygon.HoleCount;

                    //We need to recreate the polygon 
                    //renderer as well if a hole is added. 
                    CreateRenderer("Polygon", Polygon, faceColor);
                    //Create the holes renderer.
                    CreateRenderer("Hole" + holes, hole, faceColor);
                }
                else
                {
                    Debug.Log("Hole was not valid.");
                }
            }

            InputPoints.Clear();
        }

        /// <summary>
        /// Compute the visibilty from the point.
        /// </summary>
        /// <param name="point"></param>
        private void ComputeVisibility(Point2d point)
        {
            ClearRenderer("Polygon");
            ClearRenderer("Region");

            POLYGON_VISIBILITY method = POLYGON_VISIBILITY.TRIANGULAR_EXPANSION;

            PolygonWithHoles2<EEK> region;
            if(PolygonVisibility<EEK>.Instance.ComputeVisibility(method, point, Polygon, out region))
            {
                //Visibilty was successfully computed and is the region polygon
                CreateRenderer("Region", region, regionColor);

                //Take the difference with the original polygon for rendering.
                var results = new List<PolygonWithHoles2<EEK>>();
                PolygonBoolean2<EEK>.Instance.Op(POLYGON_BOOLEAN.DIFFERENCE, Polygon, region, results);

                for (int j = 0; j < results.Count; j++)
                {
                    CreateRenderer("Polygon" + j, results[j], faceColor);

                    for (int i = 0; i < results[j].HoleCount; i++)
                        CreateRenderer("PolygonHole" + j + " " + i, results[j].GetHole(i), faceColor);
                }
            }

        }

        private void ClearRenderer(string name)
        {
            var keys = new List<string>(Renderers.Keys);

            foreach (var key in keys)
            {
                if (key.Contains(name))
                    Renderers.Remove(key);
            }
        }

        private void CreateRenderer(string name, Polygon2<EEK> polygon, Color col)
        {
            Renderers[name] = Draw().
            Outline(polygon, lineColor).
            Points(polygon, lineColor, col).
            PopRenderer();
        }

        private void CreateRenderer(string name, PolygonWithHoles2<EEK> polygon, Color col)
        {
            Renderers[name] = Draw().
            Faces(polygon, col).
            Outline(polygon, lineColor).
            Points(polygon, lineColor, col).
            PopRenderer();
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
            ComputeVisibility(point);

            Point = point;
            Renderers["Point"] = Draw().
                Points(point, lineColor, regionColor).
                PopRenderer();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.F1))
            {
                AddHoles = false;
                SetInputMode(INPUT_MODE.POINT_CLICK);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                AddHoles = true;
                SetInputMode(INPUT_MODE.POLYGON);
            }
        }

        private void OnPostRender()
        {
            DrawGrid();

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

            DrawInput(lineColor, pointColor, PointSize);

        }

        protected void OnGUI()
        {
            int textLen = 1000;
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
                GUI.Label(new Rect(10, 70, textLen, textHeight), "F1 to stop adding holes and F2 to start adding holes again.");
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Holes must be simple and not intersect the polygon boundary or other holes.");
            }
            else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Click to compute visibility.");
            }

        }



    }
}
