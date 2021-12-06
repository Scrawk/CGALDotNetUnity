using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;

namespace CGALDotNetUnity.Polygons
{

    public class PolygonOffsetExample : InputBehaviour
    {

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color skeletonColor = new Color32(200, 80, 80, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Polygon2<EEK> Polygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        private double offset = 0.5;

        private bool ShowSkeleton;

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

                PolygonOffset();
            }

            InputPoints.Clear();
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

            if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
            {
                offset -= 0.1;
                if (offset < 0)
                    offset = 0;

                PolygonOffset();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus))
            {
                offset += 0.1;

                PolygonOffset();
            }
            else if(Input.GetKeyDown(KeyCode.F1))
            {
                ShowSkeleton = !ShowSkeleton;
                PolygonOffset();
            }
        }

        private void OnPostRender()
        {
            DrawGrid();
            DrawInput(lineColor, pointColor, PointSize);

            foreach (var renderer in Renderers.Values)
                renderer.Draw();
        }

        private void PolygonOffset()
        {
            if (Polygon == null) return;

            Polygon2<EEK> interior, exterior;
            if (PolygonOffset2<EEK>.Instance.CreateInteriorOffset(Polygon, offset, out interior))
            {
                CreateRenderer("Interior", Polygon, interior);
            }
                

            if(PolygonOffset2<EEK>.Instance.CreateExteriorOffset(Polygon, offset, out exterior))
            {
                CreateRenderer("Exterior", Polygon, exterior);
            }

            if (ShowSkeleton)
            {
                var skeleton = new List<Segment2d>();
                PolygonOffset2<EEK>.Instance.CreateInteriorSkeleton(Polygon, false, skeleton);
                CreateRenderer("InteriorSkeleton", skeleton);

                //skeleton.Clear();
                //PolygonOffset2<EEK>.Instance.CreateExteriorSkeleton(Polygon, 1, false, skeleton);
                //CreateRenderer("ExteriorSkeleton", skeleton);
            }
            else
            {
                Renderers.Remove("InteriorSkeleton");
            }
        }

        private void CreateRenderer(string name, Polygon2<EEK> polygon, Polygon2<EEK> offset)
        {
            Renderers["Polygon"] = Draw().
                Faces(polygon, faceColor).
                Outline(polygon, lineColor).
                Points(polygon, lineColor, pointColor).
                PopRenderer();


            Renderers[name] = Draw().
                //Faces(poly, faceColor).
                Outline(offset, lineColor).
                Points(offset, lineColor, pointColor).
                PopRenderer();
        }

        private void CreateRenderer(string name, List<Segment2d> skeleton)
        {
            Renderers[name] = Draw().
                Outline(skeleton, skeletonColor).
                Points(skeleton, lineColor, skeletonColor).
                PopRenderer();

        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;
            GUI.color = Color.black;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
            GUI.Label(new Rect(10, 70, textLen, textHeight), "-/+ to adjust offset amount");
            GUI.Label(new Rect(10, 90, textLen, textHeight), "Offset amount =" + offset);
            GUI.Label(new Rect(10, 110, textLen, textHeight), "F1 to toggle skeleton");


        }

    }
}
