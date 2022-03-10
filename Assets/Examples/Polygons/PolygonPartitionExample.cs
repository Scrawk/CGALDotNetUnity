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

    public class PolygonPartitionExample : InputBehaviour
    {

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Polygon2<EIK> Polygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        private POLYGON_PARTITION Op = POLYGON_PARTITION.OPTIMAL_CONVEX;

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
            //Create the polygon from the points.
            //We just use the EIK kernel as its the fastest.
            Polygon = new Polygon2<EIK>(points.ToArray());

            //Polygon must be simple to continue.
            if (Polygon.IsSimple)
            {
                //Polygons must be ccw.
                if (!Polygon.IsCounterClockWise)
                    Polygon.Reverse();

                //Perfom the partion algorithm
                PartitionPolygon();
            }

            InputPoints.Clear();
        }

        /// <summary>
        /// Called when scene cleared.
        /// </summary>
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

        /// <summary>
        /// Draw the renderers in post render.
        /// </summary>
        private void OnPostRender()
        {
            //This draws the grid
            DrawGrid();

            //This draws the polygon and the input point.
            foreach (var renderer in Renderers.Values)
                renderer.Draw();

            //This draws the line of points the user
            //is creating but they dont for a polygon yet.
            DrawInput(lineColor, pointColor, PointSize);
        }

        /// <summary>
        /// Perform the partition.
        /// </summary>
        private void PartitionPolygon()
        {
            if (Polygon == null) return;

            var results = new List<Polygon2<EIK>>();
            PolygonPartition2<EIK>.Instance.Partition(Op, Polygon, results);

            CreateRenderer(results);
        }

        /// <summary>
        /// Create the renderer that daws the polygon.
        /// This just ueses unitys GL to draw lies and points.
        /// Its not very fast and just used for demos.
        /// </summary>
        private void CreateRenderer(List<Polygon2<EIK>> list)
        {
            Renderers.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                var polygon = list[i];
                Renderers["Polygon " + i] = Draw().
                Faces(polygon, faceColor).
                Outline(polygon, lineColor).
                Points(polygon, lineColor, pointColor).
                PopRenderer();
            }
               
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
