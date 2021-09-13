using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using Common.Unity.Utility;

using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;

namespace CGALDotNetUnity.Polygons
{


    public class PolygonBooleanExample : InputBehaviour
    {
        private Color redColor = new Color32(200, 80, 80, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private List<PolygonWithHoles2<EEK>> Polygons;

        private Dictionary<string, CompositeRenderer> Renderers;

        private POLYGON_BOOLEAN Op = POLYGON_BOOLEAN.JOIN;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.POLYGON);
            Renderers = new Dictionary<string, CompositeRenderer>();
            Polygons = new List<PolygonWithHoles2<EEK>>();

            ConsoleRedirect.Redirect();
        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            if (Polygons.Count == 0)
            {
                var boundary = new Polygon2<EEK>(points.ToArray());

                if (boundary.IsSimple)
                {
                    if (!boundary.IsCounterClockWise)
                        boundary.Reverse();

                    var polygon = new PolygonWithHoles2<EEK>(boundary);
                    Polygons.Add(polygon);

                    for(int i = 0; i < Polygons.Count; i++)
                        AddPolygon(i, Polygons[i]);

                }
            }
            else
            {
                var polygon = new Polygon2<EEK>(points.ToArray());

                if(polygon.IsSimple)
                {
                    if (!polygon.IsCounterClockWise)
                        polygon.Reverse();

                    var tmp = new List<PolygonWithHoles2<EEK>>(Polygons);
                    Polygons.Clear();

                    foreach(var poly in tmp)
                    {
                        if(PolygonBoolean2<EEK>.Instance.Op(Op, polygon, poly, Polygons))
                        {
                            for (int i = 0; i < Polygons.Count; i++)
                                AddPolygon(i, Polygons[i]);

                            break;
                        }
                    }
                }

            }

            InputPoints.Clear();
        }

        private void AddPolygon(int id, PolygonWithHoles2<EEK> polygon)
        {
            //Renderers["PolygonBody"+id] = FromPolygon(polygon, faceColor);
            //Renderers["PolygonOutline" + id] = FromPolygon(polygon, lineColor, pointColor, PointSize);

            Renderers["PolygonBody" + id] = FromPolygon(polygon, faceColor, lineColor);

            int holes = polygon.HoleCount;
            for(int i = 0; i < holes; i++)
            {
                //var hole = polygon.Copy(POLYGON_ELEMENT.HOLE, i);
                //Renderers["Hole " + i + " " + id] = FromPolygon(hole, lineColor, pointColor, PointSize);
            }
                
        }

        protected override void OnCleared()
        {
            Polygons.Clear();
            Renderers.Clear();
            InputPoints.Clear();
            SetInputMode(INPUT_MODE.POLYGON);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Op = NextOp(Op);
            }

        }

        private POLYGON_BOOLEAN NextOp(POLYGON_BOOLEAN op)
        {
            switch (op)
            {
                case POLYGON_BOOLEAN.JOIN:
                    return POLYGON_BOOLEAN.INTERSECT;

                case POLYGON_BOOLEAN.INTERSECT:
                    return POLYGON_BOOLEAN.DIFFERENCE;

                case POLYGON_BOOLEAN.DIFFERENCE:
                    return POLYGON_BOOLEAN.SYMMETRIC_DIFFERENCE;

                case POLYGON_BOOLEAN.SYMMETRIC_DIFFERENCE:
                    return POLYGON_BOOLEAN.JOIN;
            }

            return op;
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

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");

            GUI.Label(new Rect(10, 70, textLen, textHeight), "Tab to change boolean op.");
            GUI.Label(new Rect(10, 90, textLen, textHeight), "Current op = " + Op);
        }

    }
}
