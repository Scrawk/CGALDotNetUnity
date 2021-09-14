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

        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();

            var star = PolygonFactory<EEK>.KochStar(20, 3);
            var circle = PolygonFactory<EEK>.FromCircle(3, 32);
            circle.Reverse();

            Polygon = new PolygonWithHoles2<EEK>(star);
            Polygon.AddHole(circle);

            var param = PolygonSimplificationParams.Default;
            param.threshold = 50;
            param.stop = POLYGON_SIMP_STOP_FUNC.BELOW_THRESHOLD;
            param.cost = POLYGON_SIMP_COST_FUNC.SQUARE_DIST;
            param.elements = POLYGON_ELEMENT.ALL;

            SimplifiedPolygon = PolygonSimplification2<EEK>.Instance.Simplify(Polygon, param);

            Polygon.Translate(new Point2d(-15, 0));
            SimplifiedPolygon.Translate(new Point2d(15, 0));

            AddPolygon("Polygon", Polygon);
            AddPolygon("SimplifiedPolygon", SimplifiedPolygon);
        }

        private void AddPolygon(string name, PolygonWithHoles2<EEK> polygon, bool showTriangulation = false)
        {
            if (showTriangulation)
            {
                Renderers["Polygon " + name] = FromPolygonTriangulation(polygon, faceColor, lineColor);
            }
            else
            {
                Renderers["PolygonBody " + name] = FromPolygon(polygon, faceColor);
                Renderers["PolygonOutline " + name] = FromPolygon(polygon, lineColor, pointColor, PointSize);

                int holes = polygon.HoleCount;
                for (int i = 0; i < holes; i++)
                {
                    var hole = polygon.Copy(POLYGON_ELEMENT.HOLE, i);
                    Renderers["Hole " + i + " " + name] = FromPolygon(hole, lineColor, pointColor, PointSize);
                }
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
        }



    }
}
