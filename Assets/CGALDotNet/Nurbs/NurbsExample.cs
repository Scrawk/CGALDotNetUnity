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

        private Color greenColor = new Color32(80, 200, 80, 255);

        private Color blueColor = new Color32(80, 80, 200, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private List<BaseNurbsCurve2d> Curves;

        private Dictionary<string, CompositeRenderer> Renderers;

        private bool DrawNormals;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.NONE);
            Renderers = new Dictionary<string, CompositeRenderer>();
            Curves = new List<BaseNurbsCurve2d>();

            CreateNurbs();
        }

        private void CreateNurbs()
        {

            Curves.Add(NurbsMake.Arc(new Point2d(-24, 0), 5, 0, 3 * Math.PI / 2));
            Curves.Add(NurbsMake.Circle(new Point2d(-12, 0), 5));
            Curves.Add(NurbsMake.Ellipse(new Point2d(0, 0), 5, 2));
            Curves.Add(NurbsMake.EllipseArc(new Point2d(12, 0), 5, 2, 0, 3 * Math.PI / 2));

            var points1 = new Point2d[]
            {
                new Point2d(-5.5, 5),
                new Point2d(0.5, -10),
                new Point2d(3.5, 5)
            };

            var bezier = NurbsMake.BezierCurve(points1);
            bezier.Translate(new Point2d(24, 0));
            Curves.Add(bezier);

            var points2 = new List<Point2d>();
            bezier.GetCartesianPoints(points2, 10);

           var curve = NurbsMake.FromPoints(2, points2);
            curve.Translate(new Point2d(12, 0));
            Curves.Add(curve);

            CreateRenderer();
        }

        private void CreateRenderer()
        {
            Renderers.Clear();

            for (int i = 0; i < Curves.Count; i++)
            {
                if(DrawNormals)
                {
                    Renderers["Curve" + i] = Draw().
                        Outline(Curves[i], redColor, 100).
                        Tangents(Curves[i], greenColor, 10).
                        Normals(Curves[i], blueColor, 10).
                        PopRenderer();
                }
                else
                {
                    Renderers["Curve" + i] = Draw().
                        Outline(Curves[i], redColor, 100).
                        PopRenderer();
                }

            }
            
        }
        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.F1))
            {
                DrawNormals = !DrawNormals;
                CreateRenderer();
            }

        }

        private void OnPostRender()
        {
            DrawGrid(true);

            DrawInput(lineColor, pointColor, PointSize);

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;
            GUI.color = Color.black;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "F1 to draw tangents and normals.");
        }



    }
}
