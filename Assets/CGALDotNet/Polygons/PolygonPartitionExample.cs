using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;

namespace CGALDotNetUnity.Polygons
{

    public class PolygonPartitionExample : InputBehaviour
    {

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Polygon2<EEK> Polygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        private POLYGON_PARTITION Op = POLYGON_PARTITION.OPTIMAL_CONVEX;

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

                PartitionPolygon();
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

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Op = CGALEnum.Next(Op);
                PartitionPolygon();
            }
        }

        private void OnPostRender()
        {
            DrawGrid();
            DrawInput(lineColor, pointColor, PointSize);

            foreach (var renderer in Renderers.Values)
                renderer.Draw();
        }

        private void PartitionPolygon()
        {
            if (Polygon == null) return;

            var results = new List<Polygon2<EEK>>();
            PolygonPartition2<EEK>.Instance.Partition(Op, Polygon, results);

            Renderers.Clear();
            for (int i = 0; i < results.Count; i++)
                Renderers["Polygon" + i] = FromPolygon(results[i], faceColor, lineColor, pointColor, PointSize);
        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;
            GUI.color = Color.black;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
            GUI.Label(new Rect(10, 70, textLen, textHeight), "Tab to change partition type.");
            GUI.Label(new Rect(10, 90, textLen, textHeight), "Current op = " + Op);
        }

    }
}
