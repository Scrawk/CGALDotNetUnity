using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using Common.Unity.Drawing;
using System.Collections.Generic;
using UnityEngine;

namespace CGALDotNetUnity.Geometry
{

    public class IntersectionExample : InputBehaviour
    {
        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color intersectionColor = new Color32(200, 80, 80, 128);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        private bool isCreateed = false;

        protected override void Start()
        {
            CreateDemo();
        }

        private void OnEnable()
        {
            CreateDemo();
        }

        private void CreateDemo()
        {
            if (isCreateed) return;
            isCreateed = true;

            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();

            //Shape examples

            CreateIntersection(
                "BoxTriangle",
                new Box2d(-2.5, 2.5), 
                new Triangle2d(new Point2d(0, 0), new Point2d(4, 0), new Point2d(0, 4)),
                new Point2d(-30, 20)
                );

            CreateIntersection(
                "BoxBox",
                new Box2d(-2.5, 2.5),
                new Box2d(0, 4),
                new Point2d(-20, 20)
                );

            CreateIntersection(
                "BoxRay",
                new Box2d(-2.5, 2.5),
                new Ray2d(new Point2d(-5, 0), new Vector2d(10,0)),
                new Point2d(-10, 20)
                );

            CreateIntersection(
                "BoxSegment",
                new Box2d(-2.5, 2.5),
                new Segment2d(new Point2d(-5, 0), new Point2d(5, 0)),
                new Point2d(-30, 10)
                );

            CreateIntersection(
                "BoxPolygon",
                new Box2d(-2.5, 2.5),
                PolygonFactory<EIK>.CreateCircle(new Point2d(2, 2), 2, 6),
                new Point2d(-20, 10)
                );

            //EIK Geometry examples

            CreateIntersection(
                "BoxTriangle<EIK>",
                new Box2<EIK>(-2.5, 2.5),
                new Triangle2<EIK>(new Point2d(0, 0), new Point2d(4, 0), new Point2d(0, 4)),
                new Point2d(30, 20)
                );

            CreateIntersection(
                "BoxBox<EIK>",
                new Box2<EIK>(-2.5, 2.5),
                new Box2<EIK>(0, 4),
                new Point2d(20, 20)
                );

            CreateIntersection(
                "BoxRay<EIK>",
                new Box2<EIK>(-2.5, 2.5),
                new Ray2<EIK>(new Point2d(-5, 0), new Vector2d(10, 0)),
                new Point2d(10, 20)
                );

            CreateIntersection(
                "BoxSegment<EIK>",
                new Box2<EIK>(-2.5, 2.5),
                new Segment2<EIK>(new Point2d(-5, 0), new Point2d(5, 0)),
                new Point2d(30, 10)
                );

            CreateIntersection(
                "BoxPolygon<EIK>",
                new Box2<EIK>(-2.5, 2.5),
                PolygonFactory<EIK>.CreateCircle(new Point2d(2, 2), 2, 6),
                new Point2d(20, 10)
                );
        }

        private void CreateIntersection(string name, Box2d box, Triangle2d tri, Point2d translate)
        {
            box += translate;
            tri += translate;

            var boxPoly = PolygonFactory<EIK>.CreateBox(box);
            Renderers["Box:" + name] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            var triPoly = PolygonFactory<EIK>.CreateTriangle(tri);
            Renderers["Triangle:" + name] = Draw().
            Faces(triPoly, faceColor).
            Outline(triPoly, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, tri);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(string name, Box2d box, Box2d box2, Point2d translate)
        {
            box += translate;
            box2 += translate;

            var boxPoly = PolygonFactory<EIK>.CreateBox(box);
            Renderers["Box:" + name] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            var boxPoly2 = PolygonFactory<EIK>.CreateBox(box2);
            Renderers["Box2:" + name] = Draw().
            Faces(boxPoly2, faceColor).
            Outline(boxPoly2, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, box2);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(string name, Box2d box, Ray2d ray, Point2d translate)
        {
            box += translate;
            ray.Position += translate;

            var boxPoly = PolygonFactory<EIK>.CreateBox(box);
            Renderers["Box:" + name] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            Renderers["Ray:" + name] = Draw().
            Outline(ray, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, ray);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(string name, Box2d box, Segment2d seg, Point2d translate)
        {
            box += translate;
            seg += translate;

            var boxPoly = PolygonFactory<EIK>.CreateBox(box);
            Renderers["Box:" + name] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            Renderers["Seg:" + name] = Draw().
            Outline(seg, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, seg);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(string name, Box2<EIK> box, Triangle2<EIK> tri, Point2d translate)
        {
            box.Translate(translate);
            tri.Translate(translate);

            var boxPoly = PolygonFactory<EIK>.CreateBox(box.Shape);
            Renderers["Box<EIK>:" + name] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            var triPoly = PolygonFactory<EIK>.CreateTriangle(tri.Shape);
            Renderers["Triangle<EIK>:" + name] = Draw().
            Faces(triPoly, faceColor).
            Outline(triPoly, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, tri);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(string name, Box2<EIK> box, Box2<EIK> box2, Point2d translate)
        {
            box.Translate(translate);
            box2.Translate(translate);

            var boxPoly = PolygonFactory<EIK>.CreateBox(box.Shape);
            Renderers["Box<EIK>:" + name] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            var boxPoly2 = PolygonFactory<EIK>.CreateBox(box2.Shape);
            Renderers["Box2<EIK>:" + name] = Draw().
            Faces(boxPoly2, faceColor).
            Outline(boxPoly2, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, box2);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(string name, Box2<EIK> box, Ray2<EIK> ray, Point2d translate)
        {
            box.Translate(translate);
            ray.Translate(translate);

            var boxPoly = PolygonFactory<EIK>.CreateBox(box.Shape);
            Renderers["Box<EIK>:" + name] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            Renderers["Ray<EIK>:" + name] = Draw().
            Outline(ray.Shape, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, ray);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(string name, Box2<EIK> box, Segment2<EIK> seg, Point2d translate)
        {
            box.Translate(translate);
            seg.Translate(translate);

            var boxPoly = PolygonFactory<EIK>.CreateBox(box.Shape);
            Renderers["Box<EIK>:" + name] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            Renderers["Seg<EIK>:" + name] = Draw().
            Outline(seg.Shape, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, seg);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(string name, Box2d box, Polygon2<EIK> polygon, Point2d translate)
        {
            box += translate;
            polygon.Translate(translate);

            var boxPoly = PolygonFactory<EIK>.CreateBox(box);
            Renderers["Box:" + name] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            Renderers["Polygon<EIK>:" + name] = Draw().
            Faces(polygon, faceColor).
            Outline(polygon, lineColor).
            PopRenderer();

            var results = new List<PolygonWithHoles2<EIK>>();
            if (polygon.Intersection(boxPoly, results))
            {
                int count = Renderers.Count;
                foreach (var poly in results)
                {
                    Renderers["Intersection" + (count++)] = Draw().
                    Faces(poly, intersectionColor).
                    Outline(poly, intersectionColor).
                    PopRenderer();
                }
            }
        }

        private void CreateIntersection(string name, Box2<EIK> box, Polygon2<EIK> polygon, Point2d translate)
        {
            box.Translate(translate);
            polygon.Translate(translate);

            var boxPoly = PolygonFactory<EIK>.CreateBox(box.Shape);
            Renderers["Box<EIK>:" + name] = Draw().
            Faces(boxPoly, faceColor).
            Outline(boxPoly, lineColor).
            PopRenderer();

            Renderers["Polygon<EIK>:" + name] = Draw().
            Faces(polygon, faceColor).
            Outline(polygon, lineColor).
            PopRenderer();

            var results = new List<PolygonWithHoles2<EIK>>();
            if (polygon.Intersection(boxPoly, results))
            {
                int count = Renderers.Count;
                foreach (var poly in results)
                {
                    Renderers["Intersection" + (count++)] = Draw().
                    Faces(poly, intersectionColor).
                    Outline(poly, intersectionColor).
                    PopRenderer();
                }
            }
        }

        private void CreateRenderer(IntersectionResult2d result, Color col)
        {
            int count = Renderers.Count;

            if (result.Type == INTERSECTION_RESULT_2D.POLYGON2)
            {
                var polygon = result.Polygon<EIK>();

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
            DrawGrid(true);

            if (Renderers != null)
            {
                foreach (var renderer in Renderers.Values)
                    renderer.Draw();
            }


        }

    }
}
