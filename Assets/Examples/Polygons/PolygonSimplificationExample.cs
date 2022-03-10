using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNetGeometry.Numerics;

namespace CGALDotNetUnity.Polygons
{

    public class PolygonSimplificationExample : InputBehaviour
    {
        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private PolygonWithHoles2<EIK> Polygon, SimplifiedPolygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        private PolygonSimplificationParams Param;

        protected override void Start()
        {
            //Init base layer and turn input off.
            base.Start();
            SetInputMode(INPUT_MODE.NONE);
            Renderers = new Dictionary<string, CompositeRenderer>();

            var star = PolygonFactory<EIK>.KochStar(20, 3);
            //var circle = PolygonFactory<EIK>.CreateCircle(3, 32);
            //circle.Reverse();

            //Create a polygon to show before simplication
            Polygon = new PolygonWithHoles2<EIK>(star);
            //Polygon.AddHole(circle);
            Polygon.Translate(new Point2d(-10, 0));
            CreateRenderer("Polygon", Polygon);

            //Set some of the simplification params
            Param = PolygonSimplificationParams.Default;
            Param.threshold = 100;
            Param.stop = POLYGON_SIMP_STOP_FUNC.BELOW_THRESHOLD;
            Param.cost = POLYGON_SIMP_COST_FUNC.SQUARE_DIST;

            //Perform the simplification
            Simplify();
        }

        /// <summary>
        /// Performs the simplification
        /// </summary>
        private void Simplify()
        {
            //Create a polygon to show after simplication
            SimplifiedPolygon = PolygonSimplification2<EIK>.Instance.Simplify(Polygon, Param);
            SimplifiedPolygon.Translate(new Point2d(20, 0));

            CreateRenderer("Simplified", SimplifiedPolygon);
        }

        /// <summary>
        /// Create the renderer that daws the polygon.
        /// This just ueses unitys GL to draw lines and points.
        /// Its not very fast 
        private void CreateRenderer(string name, PolygonWithHoles2<EIK> polygon)
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

        protected override void Update()
        {
            base.Update();

            if(Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
            {
                Param.threshold -= 1;

                if (Param.threshold < 1)
                    Param.threshold = 1;

                Simplify();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus))
            {
                Param.threshold += 1;

                Simplify();
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
            GUI.Label(new Rect(10, 30, textLen, textHeight), "Cost Function = " + Param.cost);
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Stop Function = " + Param.stop);
            GUI.Label(new Rect(10, 70, textLen, textHeight), "Threshold = " + Param.threshold);

        }



    }
}
