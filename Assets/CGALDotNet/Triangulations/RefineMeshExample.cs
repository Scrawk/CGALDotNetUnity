using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;
using CGALDotNet.Triangulations;

namespace CGALDotNetUnity.Triangulations
{

    public class RefineMeshExample : InputBehaviour
    {
        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private PolygonWithHoles2<EEK> Polygon;

        private ConstrainedTriangulation2<EEK> Triangulation;

        private Dictionary<string, CompositeRenderer> Renderers;


        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();
            SetInputMode(INPUT_MODE.POINT_CLICK);

            MakeDounut();
            Refine();
        }

        private void MakeStar()
        {
            var star = PolygonFactory<EEK>.KochStar(20, 3);
            var circle = PolygonFactory<EEK>.FromCircle(3, 32);
            circle.Reverse();

            Polygon = new PolygonWithHoles2<EEK>(star);
            Polygon.AddHole(circle);
            //Polygon.Translate(new Point2d(-15, 0));

            //CreateRenderer("Polygon", Polygon);
        }

        private void MakeDounut()
        {
            Polygon = PolygonFactory<EEK>.FromDounut(10, 5, 16);
            Polygon.Translate(new Point2d(-15, 0));

            //CreateRenderer("Polygon", Polygon);
        }


        private void Refine()
        {
            Triangulation = new ConstrainedTriangulation2<EEK>();

            var boundary = Polygon.Copy(POLYGON_ELEMENT.BOUNDARY);
            var hole = Polygon.Copy(POLYGON_ELEMENT.HOLE, 0);

            Triangulation.InsertConstraint(boundary);
            Triangulation.InsertConstraint(hole);
            //Triangulation.Translate(new Point2d(30, 0));

            var seeds = new Point2d[1]
            {
                new Point2d(0,0)
            };

            Triangulation.Refine(0.125, 2, seeds);
            Triangulation.MakeDelaunay();

            //CreateRenderer(" Refined", Triangulation);
        }

        private void CreateRenderer(string name, PolygonWithHoles2<EEK> polygon)
        {
            Renderers[name] = Draw().
                Faces(polygon, faceColor).
                Outline(polygon, lineColor).
                Points(polygon, lineColor, pointColor, PointSize).
                PopRenderer();

            int holes = polygon.HoleCount;
            for (int i = 0; i < holes; i++)
            {
                var hole = polygon.Copy(POLYGON_ELEMENT.HOLE, i);

                Renderers[name + " Hole " + i] = Draw().
                Outline(hole, lineColor).
                Points(hole, lineColor, pointColor, PointSize).
                PopRenderer();
            }
        }

        private void CreateRenderer(string name, ConstrainedTriangulation2<EEK> triangulation)
        {
            Renderers[name] = Draw().
                Faces(triangulation, faceColor).
                Outline(triangulation, lineColor).
                Points(triangulation, lineColor, pointColor, PointSize).
                PopRenderer();
        }

        protected override void OnLeftClickDown(Point2d point)
        {
            Debug.Log("Clicked at " + point);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
            {

                Refine();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus))
            {

                Refine();
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

            GUI.Label(new Rect(10, 10, textLen, textHeight), "+/- to adjust simplification threshold.");
            //GUI.Label(new Rect(10, 30, textLen, textHeight), "Cost Function = " + Param.cost);
            //GUI.Label(new Rect(10, 50, textLen, textHeight), "Stop Function = " + Param.stop);
            //GUI.Label(new Rect(10, 70, textLen, textHeight), "Threshold = " + Param.threshold);

        }



    }
}
