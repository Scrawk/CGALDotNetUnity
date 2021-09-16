using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Triangulations;

namespace CGALDotNetUnity.Triangulations
{

    public class TriangulationExample : InputBehaviour
    {
        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color redColor = new Color32(200, 80, 80, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        private Triangulation2<EEK> Triangulation;


        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();
            SetInputMode(INPUT_MODE.POINT_CLICK);

            Triangulation = new Triangulation2<EEK>();
            Triangulation.InsertPoint(new Point2d(-5, -5));
            Triangulation.InsertPoint(new Point2d(5, -5));
            Triangulation.InsertPoint(new Point2d(0, 5));

            Renderers["Triangulation"] = FromTriangulation(Triangulation, faceColor, lineColor, pointColor, PointSize);
        }

        protected override void OnLeftClickDown(Point2d point)
        {

            if(Triangulation.LocateEdge(point, out TriEdge2 edge, out Segment2d seg))
            {
                Debug.Log(edge);

                Renderers["Segment"] = FromPoints(new Point2d[] { seg.A, seg.B }, lineColor, redColor, PointSize);
            }

        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Tab))
            {

            }
            else if (Input.GetKeyDown(KeyCode.F1))
            {
     
            }

        }

        private void OnPostRender()
        {
            DrawGrid();

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;
            GUI.color = Color.black;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Tab to change hull method.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "F1 to change seed.");

        }



    }
}
