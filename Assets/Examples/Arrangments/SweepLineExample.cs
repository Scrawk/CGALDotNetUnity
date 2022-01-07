using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using Common.Unity.Drawing;
using Common.Unity.Utility;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;
using CGALDotNet.Arrangements;

namespace CGALDotNetUnity.Arrangements
{

    public class SweepLineExample : InputBehaviour
    {

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color redColor = new Color32(200, 80, 80, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        private Segment2d[] SubCurves;

        private Point2d[] Points;

        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();
            SetInputMode(INPUT_MODE.NONE);

            var segments = new Segment2d[]
            {
                new Segment2d(new Point2d(1,5), new Point2d(8,5)),
                new Segment2d(new Point2d(1,1), new Point2d(8,8)),
                new Segment2d(new Point2d(3,1), new Point2d(3,8)),
                new Segment2d(new Point2d(8,5), new Point2d(8,8))
            };

            SubCurves = SweepLine<EEK>.Instance.ComputeSubcurves(segments, segments.Length);
            Points = SweepLine<EEK>.Instance.ComputeIntersectionPoints(segments, segments.Length);

            Renderers["Segments"] = Draw().
                Outline(SubCurves, lineColor).
                Points(SubCurves, lineColor, pointColor).
                PopRenderer();

            Renderers["Points"] = Draw().
                Points(Points, lineColor, redColor).
                PopRenderer();
            
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
            int textLen = 1000;
            int textHeight = 25;
            GUI.color = Color.black;

            int subcurves = SubCurves.Length;
            int points = Points.Length;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "There are " + subcurves + " sub curves.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "There are " + points +  " intersection points.");
        }

    }
}
