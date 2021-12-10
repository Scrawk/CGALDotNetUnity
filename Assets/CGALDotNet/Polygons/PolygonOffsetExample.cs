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

        private Color skeletonColor = new Color32(200, 80, 80, 255);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private PolygonWithHoles2<EEK> Polygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        private double Offset = 0.5;

        private bool AddHoles = true;

        private bool ShowSkeleton;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.POLYGON);
            Renderers = new Dictionary<string, CompositeRenderer>();
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

                    CreateRenderer("Polygon", Polygon);
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

                    CreateRenderer("Polygon", Polygon);
                    CreateRenderer("Hole" + holes, hole);
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
            Polygon = null;
            AddHoles = false;
            ShowSkeleton = false;
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
                CreateOffset();
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                AddHoles = true;
                ClearOffsets();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
            {
                Offset -= 0.1;
                if (Offset < 0)
                    Offset = 0;

                CreateOffset();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus))
            {
                Offset += 0.1;

                CreateOffset();
            }
            else if(Input.GetKeyDown(KeyCode.F3))
            {
                ShowSkeleton = !ShowSkeleton;
                CreateOffset();
            }
        }

        private void OnPostRender()
        {
            DrawGrid();
            
            foreach (var renderer in Renderers.Values)
                renderer.Draw();

            DrawInput(lineColor, pointColor, PointSize);
        }

        private void CreateOffset()
        {
            if (Polygon == null) return;

            var interior = new List<Polygon2<EEK>>();
            PolygonOffset2<EEK>.Instance.CreateInteriorOffset(Polygon, Offset, interior);

            for(int i = 0; i < interior.Count; i++)
                CreateRenderer("InteriorOffset" + i, interior[i]);

            var exterior = new List<Polygon2<EEK>>();
            PolygonOffset2<EEK>.Instance.CreateExteriorOffset(Polygon, Offset, exterior);

            //The first polygon is the bounding box so ignore.
            for (int i = 1; i < exterior.Count; i++)
                CreateRenderer("ExteriorOffset" + i, exterior[i]);

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
                ClearSkeletons();
            }
           
        }

        private void ClearOffsets()
        {
            var keys = new List<string>(Renderers.Keys);

            foreach(var key in keys)
            {
                if(key.Contains("Offset"))
                    Renderers.Remove(key);
            }
        }

        private void ClearSkeletons()
        {
            var keys = new List<string>(Renderers.Keys);

            foreach (var key in keys)
            {
                if (key.Contains("Skeleton"))
                    Renderers.Remove(key);
            }
        }

        private void CreateRenderer(string name, PolygonWithHoles2<EEK> pwh)
        {
            Renderers[name] = Draw().
            Faces(pwh, faceColor).
            Outline(pwh, lineColor).
            Points(pwh, lineColor, pointColor, PointSize).
            PopRenderer();
        }

        private void CreateRenderer(string name, Polygon2<EEK> polygon)
        {
            Renderers[name] = Draw().
            Outline(polygon, lineColor).
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
                GUI.Label(new Rect(10, 70, textLen, textHeight), "F1 to stop adding holes and show the offsets.");
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Holes must be simple and not intersect the polygon boundary or other holes.");
            }
            else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "-/+ to adjust offset amount");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Offset amount = " + Offset);
                GUI.Label(new Rect(10, 70, textLen, textHeight), "F2 to start adding holes again.");
                GUI.Label(new Rect(10, 90, textLen, textHeight), "F3 to toggle skeleton");
            }


        }

    }
}
