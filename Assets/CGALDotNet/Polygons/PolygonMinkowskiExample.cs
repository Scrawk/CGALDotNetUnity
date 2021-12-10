using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;

namespace CGALDotNetUnity.Polygons
{


    public class PolygonMinkowskiExample : InputBehaviour
    {
        private Color redColor = new Color32(200, 80, 80, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private MINKOWSKI_DECOMPOSITION_PWH Decomposition = MINKOWSKI_DECOMPOSITION_PWH.TRIANGULATION;

        private PolygonWithHoles2<EEK> Polygon;

        private Polygon2<EEK> Shape;

        private PolygonWithHoles2<EEK> Sum;

        private Dictionary<string, CompositeRenderer> Renderers;

        private bool AddHoles = true;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.POLYGON);
            Renderers = new Dictionary<string, CompositeRenderer>();

            CreateShape();
        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            if (Polygon == null)
            {
                var boundary = new Polygon2<EEK>(points.ToArray());

                if (boundary.IsSimple)
                {
                    if (!boundary.IsCounterClockWise)
                        boundary.Reverse();

                    Polygon = new PolygonWithHoles2<EEK>(boundary);

                    CreateRenderer("Polygon", Polygon, false);
                }
                else
                {
                    Debug.Log("Polygon was not simple.");
                }
            }
            else if (AddHoles)
            {
                var hole = new Polygon2<EEK>(points.ToArray());

                if (!hole.IsClockWise)
                    hole.Reverse();

                if (PolygonWithHoles2.IsValidHole(Polygon, hole))
                {
                    Polygon.AddHole(hole);
                    int holes = Polygon.HoleCount;

                    CreateRenderer("Polygon", Polygon, false);
                    CreateRenderer("Hole" + holes, hole);
                }
                else
                {
                    Debug.Log("Hole was not valid.");
                }
            }

            InputPoints.Clear();
        }

        private void CreateShape()
        {
            //Shape = PolygonFactory<EEK>.CreateBox(-0.5, 0.5);
            Shape = PolygonFactory<EEK>.CreateCircle(0.5, 16);
        }

        private void CreateRenderer(string name, Polygon2<EEK> polygon)
        {
            Renderers[name] = Draw().
            Outline(polygon, lineColor).
            Points(polygon, lineColor, pointColor).
            PopRenderer();
        }

        private void CreateRenderer(string name, PolygonWithHoles2<EEK> polygon, bool outlineOnly)
        {
            if (outlineOnly)
            {
                Renderers[name] = Draw().
                Outline(polygon, lineColor).
                Points(polygon, lineColor, pointColor).
                PopRenderer();
            }
            else
            {
                Renderers[name] = Draw().
                Faces(polygon, faceColor).
                Outline(polygon, lineColor).
                Points(polygon, lineColor, pointColor).
                PopRenderer();
            }
        }

        protected override void OnCleared()
        {
            Polygon = null;
            Renderers.Clear();
            InputPoints.Clear();
            SetInputMode(INPUT_MODE.POLYGON);
        }

        protected override void Update()
        {
            base.Update();

            if (Polygon == null) return;

            if (Input.GetKeyDown(KeyCode.F1))
            {
                AddHoles = false;
                PerformMinkowskiSum();
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                AddHoles = true;
                ClearMinkowskiSum();
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                Decomposition = CGALEnum.Next(Decomposition);
                PerformMinkowskiSum();
            }
        }

        private void OnPostRender()
        {
            DrawGrid();

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

            DrawInput(lineColor, pointColor, PointSize);

        }

        private void PerformMinkowskiSum()
        {
            Sum = PolygonMinkowski<EEK>.Instance.Sum(Decomposition, Polygon, Shape);
            CreateRenderer("Sum", Sum, true);

            for(int i = 0; i < Sum.HoleCount; i++)
                CreateRenderer("SumHole"+i, Sum.GetHole(i));

        }

        private void ClearMinkowskiSum()
        {
            var keys = new List<string>(Renderers.Keys);

            foreach (var key in keys)
            {
                if (key.Contains("Sum"))
                    Renderers.Remove(key);
            }
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
                GUI.Label(new Rect(10, 70, textLen, textHeight), "F1 to stop adding holes and show the minkowski sum.");
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Holes must be simple and not intersect the polygon boundary or other holes.");
            }
            else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Tab to change decomposition mode");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Decomposition = " + Decomposition);
                GUI.Label(new Rect(10, 70, textLen, textHeight), "F2 to start adding holes again.");
            }

        }



    }
}
