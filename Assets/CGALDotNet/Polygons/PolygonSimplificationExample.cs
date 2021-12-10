using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;

namespace CGALDotNetUnity.Polygons
{

    public class PolygonSimplificationExample : InputBehaviour
    {
        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private PolygonWithHoles2<EEK> Polygon, SimplifiedPolygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        private PolygonSimplificationParams Param;

        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();

            var star = PolygonFactory<EEK>.KochStar(20, 3);
            var circle = PolygonFactory<EEK>.CreateCircle(3, 32);
            circle.Reverse();

            Polygon = new PolygonWithHoles2<EEK>(star);
            //Polygon.AddHole(circle);
            Polygon.Translate(new Point2d(-10, 0));
            CreateRenderer("Polygon", Polygon);

            Param = PolygonSimplificationParams.Default;
            Param.threshold = 100;
            Param.stop = POLYGON_SIMP_STOP_FUNC.BELOW_THRESHOLD;
            Param.cost = POLYGON_SIMP_COST_FUNC.SQUARE_DIST;

            Simplify();
        }

        private void Simplify()
        {
            SimplifiedPolygon = PolygonSimplification2<EEK>.Instance.Simplify(Polygon, Param);
            SimplifiedPolygon.Translate(new Point2d(20, 0));

            CreateRenderer("Simplified", SimplifiedPolygon);
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
