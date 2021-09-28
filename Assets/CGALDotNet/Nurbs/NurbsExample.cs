using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Nurbs;
using CGALDotNet.Geometry;

namespace CGALDotNetUnity.Polygons
{

    public class NurbsExample : InputBehaviour
    {
        private Color redColor = new Color32(200, 80, 80, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private List<NurbsCurve2d> Curves;

        private Dictionary<string, CompositeRenderer> Renderers;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.NONE);
            Renderers = new Dictionary<string, CompositeRenderer>();
            Curves = new List<NurbsCurve2d>();

            CreateNurbs();
        }

        private void CreateNurbs()
        {

            //Curves.Add(NurbsMake.Arc(new Vector2d(-24, 0), 5, 0, 3 * Math.PI / 2));
            //Curves.Add(NurbsMake.Circle(new Vector2d(-12, 0), 5));
            //Curves.Add(NurbsMake.Ellipse(new Vector2d(0, 0), 5, 2));
            //Curves.Add(NurbsMake.EllipseArc(new Vector2d(12, 0), 5, 2, 0, 3 * Math.PI / 2));

            var points1 = new Vector2d[]
            {
                new Vector2d(43-24, 5),
                new Vector2d(48-24, -10),
                new Vector2d(51-24, 5)
            };

            //Curves.Add(NurbsMake.BezierCurve(points1));

            var points2 = new List<Vector2d>();
            //Curves[4].GetPoints(points2, 10);

           // var curve = NurbsMake.FromPoints(2, points2);
            //Curves.Add(curve.Translate(new Vector3d(12, 0, 0)));

            CreateRenderer();
        }

        private void CreateRenderer()
        {
            Renderers.Clear();

            for (int i = 0; i < Curves.Count; i++)
            {
                //Renderers["Curve"+i] = Draw().
                //    Outline(Curves[i], redColor, 100).
               //     PopRenderer();
            }
            
        }

        protected override void OnLeftClickDown(Point2d point)
        {

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


        }



    }
}
