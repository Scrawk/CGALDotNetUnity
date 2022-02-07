using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;

namespace CGALDotNetUnity.Geometry
{

    public class IntersectionExample : InputBehaviour
    {
        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color intersectionColor = new Color32(200, 80, 80, 128);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();

            //var point = new Point2d();
            //var line = new Line2d(new Point2d(0, 0), new Point2d(1,1));
            //var segment = new Segment2d(new Point2d(-10, 0), new Point2d(10, 0));
            var box = new Box2d(-5, 5);
            var triangle = new Triangle2d(new Point2d(1, 1), new Point2d(9, 1), new Point2d(1, 9));
            //var ray = new Ray2d(new Point2d(0,0), new Vector2d(0, 1));

            var boxPoly = PolygonFactory<EEK>.CreateBox(box);

            Renderers["Box"] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            var triPoly = PolygonFactory<EEK>.CreateTriangle(triangle);

            Renderers["Triangle"] = Draw().
            Faces(triPoly, faceColor).
            Outline(triPoly, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, triangle);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }


        }

        private void CreateRenderer(IntersectionResult2d result, Color col)
        {
            int count = Renderers.Count;

            if (result.Type == INTERSECTION_RESULT_2D.POLYGON2)
            {
                var polygon = result.Polygon<EEK>();

                Renderers["Intersection" + count] = Draw().
                Faces(polygon, col).
                Outline(polygon, col).
                PopRenderer();
            }
            else if(result.Type == INTERSECTION_RESULT_2D.POINT2)
            {
                var point = result.Point;

                Renderers["Intersection" + count] = Draw().
                    Points(point, col, col).
                    PopRenderer();
            }
            else if (result.Type == INTERSECTION_RESULT_2D.LINE2)
            {
                var geo = result.Line;
                Renderers["Intersection" + count] = Draw().
                    Outline(geo, col).
                    PopRenderer();
            }
            else if (result.Type == INTERSECTION_RESULT_2D.RAY2)
            {
                var geo = result.Ray;
                Renderers["Intersection" + count] = Draw().
                    Outline(geo, col).
                    PopRenderer();
            }
            else if (result.Type == INTERSECTION_RESULT_2D.SEGMENT2)
            {
                var geo = result.Segment;
                Renderers["Intersection" + count] = Draw().
                    Outline(geo, col).
                    PopRenderer();
            }
            else if (result.Type == INTERSECTION_RESULT_2D.BOX2)
            {
                var polygon = PolygonFactory<EEK>.CreateBox(result.Box);

                Renderers["Intersection" + count] = Draw().
                Faces(polygon, col).
                Outline(polygon, col).
                PopRenderer();
            }
            else if (result.Type == INTERSECTION_RESULT_2D.TRIANGLE2)
            {
                var polygon = PolygonFactory<EEK>.CreateTriangle(result.Triangle);

                Renderers["Intersection" + count] = Draw().
                Faces(polygon, col).
                Outline(polygon, col).
                PopRenderer();
            }
        }

        private void OnPostRender()
        {
            DrawGrid();

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

        }

    }
}
