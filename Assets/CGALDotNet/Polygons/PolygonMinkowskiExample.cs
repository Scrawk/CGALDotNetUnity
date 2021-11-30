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

        private MINKOWSKI_DECOMPOSITION Decomposition = MINKOWSKI_DECOMPOSITION.SMALL_SIDE_ANGLE_BISECTOR;

        private Polygon2<EEK> Polygon, Shape;

        private PolygonWithHoles2<EEK> Sum;

        private Dictionary<string, CompositeRenderer> Renderers;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.POLYGON);
            Renderers = new Dictionary<string, CompositeRenderer>();

            CreateShape();
        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            Polygon = new Polygon2<EEK>(points.ToArray());

            if (Polygon.IsSimple)
            {
                if (!Polygon.IsCounterClockWise)
                    Polygon.Reverse();

                PerformMinkowskiSum();
            }
            else
            {
                Polygon = null;
            }

            InputPoints.Clear();
        }

        private void PerformMinkowskiSum()
        {
            if (Polygon == null || Shape == null)
            {
                OnCleared();
                return;
            }

            //Dont need the decomposite type. Default is fine.
            //Sum = PolygonMinkowski<EEK>.Instance.Sum(Polygon, Shape);

            float start = Time.realtimeSinceStartup;

            Sum = PolygonMinkowski<EEK>.Instance.Sum(Polygon, Shape, Decomposition);

            float end = Time.realtimeSinceStartup;

            Debug.Log(Decomposition + " Time = " + (end - start) * 1000 + " ms");

            CreateRenderer("Sum", Sum);
        }

        private void CreateShape()
        {
            //Shape = PolygonFactory<EEK>.CreateBox(-0.5, 0.5);
            Shape = PolygonFactory<EEK>.CreateCircle(0.5, 16);
        }

        private void CreateRenderer(string name, Polygon2<EEK> polygon)
        {
            Renderers[name] = Draw().
            Faces(polygon, faceColor).
            Outline(polygon, lineColor).
            //Points(polygon, lineColor, pointColor).
            PopRenderer();
        }

        private void CreateRenderer(string name, PolygonWithHoles2<EEK> polygon)
        {
            Renderers[name] = Draw().
            Faces(polygon, faceColor).
            Outline(polygon, lineColor).
            //Points(polygon, lineColor, pointColor).
            PopRenderer();
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

            if (Input.GetKeyDown(KeyCode.F1))
            {
                Decomposition = CGALEnum.Next(Decomposition);
                PerformMinkowskiSum();
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

            if (Polygon == null || !Polygon.IsSimple)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");

                GUI.Label(new Rect(10, 70, textLen, textHeight), "F1 to change decomposition type.");
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Current decomposition = " + Decomposition);
            }
            else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
            }

        }



    }
}
