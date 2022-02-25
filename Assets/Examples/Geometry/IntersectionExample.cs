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

    /// <summary>
    /// Examples of intersection between CGALDotNet shape and geometry objects.
    /// It is reconmended to use the EEK kernel for intersections.
    /// </summary>
    public class IntersectionExample : InputBehaviour
    {
        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color intersectionColor = new Color32(200, 80, 80, 128);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        private bool isCreateed = false;

        private int buildStamp;

        protected override void Start()
        {
            CreateDemo();
        }

        private void OnEnable()
        {
            CreateDemo();
        }

        /// <summary>
        /// Create the scene objects.
        /// </summary>
        private void CreateDemo()
        {
            //If already created return.
            if (isCreateed) return;
            isCreateed = true;

            //Init the base input
            base.Start();

            Renderers = new Dictionary<string, CompositeRenderer>();

            //Shape examples

            CreateIntersection(
                new Box2d(-2.5, 2.5), 
                new Triangle2d(new Point2d(0, 0), new Point2d(4, 0), new Point2d(0, 4)),
                new Point2d(-30, 20)
                );

            CreateIntersection(
                new Box2d(-2.5, 2.5),
                new Box2d(0, 4),
                new Point2d(-20, 20)
                );

            CreateIntersection(
                new Box2d(-2.5, 2.5),
                new Ray2d(new Point2d(-5, 0), new Vector2d(10,0)),
                new Point2d(-10, 20)
                );

            CreateIntersection(
                new Box2d(-2.5, 2.5),
                new Segment2d(new Point2d(-5, 0), new Point2d(5, 0)),
                new Point2d(-30, 10)
                );

            CreateIntersection(
                new Box2d(-2.5, 2.5),
                PolygonFactory<EEK>.CreateCircle(new Point2d(2, 2), 2, 6),
                new Point2d(-20, 10)
                );

            CreateIntersection(
                new Box2<EEK>(-2.5, 2.5),
                PolygonFactory<EEK>.KochStar(new Point2d(-2, -2), 4, 1),
                new Point2d(-10, 10)
                );

            //EEK Geometry examples

            CreateIntersection(
                new Box2<EEK>(-2.5, 2.5),
                new Triangle2<EEK>(new Point2d(0, 0), new Point2d(4, 0), new Point2d(0, 4)),
                new Point2d(30, 20)
                );

            CreateIntersection(
                new Box2<EEK>(-2.5, 2.5),
                new Box2<EEK>(0, 4),
                new Point2d(20, 20)
                );

            CreateIntersection(
                new Box2<EEK>(-2.5, 2.5),
                new Ray2<EEK>(new Point2d(-5, 0), new Vector2d(10, 0)),
                new Point2d(10, 20)
                );

            CreateIntersection(
                new Box2<EEK>(-2.5, 2.5),
                new Segment2<EEK>(new Point2d(-5, 0), new Point2d(5, 0)),
                new Point2d(30, 10)
                );

            CreateIntersection(
                new Box2<EEK>(-2.5, 2.5),
                PolygonFactory<EEK>.CreateCircle(new Point2d(2, 2), 2, 6),
                new Point2d(20, 10)
                );

            CreateIntersection(
                new Box2<EEK>(-2.5, 2.5),
                PolygonFactory<EEK>.KochStar(new Point2d(-2, -2), 4, 1),
                new Point2d(10, 10)
            );
        }

        /// <summary>
        /// Create th intersetion between a box and triangle.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="tri">The trinagle.</param>
        /// <param name="translate">THe objects positions.</param>
        private void CreateIntersection(Box2d box, Triangle2d tri, Point2d translate)
        {
            box += translate;
            tri += translate;

            //To draw the shape its easest to convert shape to a polygon.
            var boxPoly = PolygonFactory<EEK>.CreateBox(box);
            DrawPolygon(boxPoly, faceColor, lineColor);

            //To draw the shape its easest to convert shape to a polygon.
            var triPoly = PolygonFactory<EEK>.CreateTriangle(tri);
            DrawPolygon(triPoly, faceColor, lineColor);

            //Test intersection.
            var result = CGALIntersections.Intersection(box, tri);

            if (result.Hit)
            {
                //If there was a hit get the intersection shape and draw it.
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(Box2d box, Box2d box2, Point2d translate)
        {
            box += translate;
            box2 += translate;

            var boxPoly = PolygonFactory<EEK>.CreateBox(box);
            DrawPolygon(boxPoly, faceColor, lineColor);

            var boxPoly2 = PolygonFactory<EEK>.CreateBox(box2);
            DrawPolygon(boxPoly2, faceColor, lineColor);

            var result = CGALIntersections.Intersection(box, box2);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(Box2d box, Ray2d ray, Point2d translate)
        {
            box += translate;
            ray.Position += translate;

            var boxPoly = PolygonFactory<EEK>.CreateBox(box);
            DrawPolygon(boxPoly, faceColor, lineColor);

            //only need to draw ray as a outline.
            Renderers["Shape:" + (buildStamp++)] = Draw().
            Outline(ray, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, ray);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(Box2d box, Segment2d seg, Point2d translate)
        {
            box += translate;
            seg += translate;

            var boxPoly = PolygonFactory<EEK>.CreateBox(box);
            DrawPolygon(boxPoly, faceColor, lineColor);

            //only need to draw segment as a outline.
            Renderers["Shape:" + (buildStamp++)] = Draw().
            Outline(seg, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, seg);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(Box2<EEK> box, Triangle2<EEK> tri, Point2d translate)
        {
            box.Translate(translate);
            tri.Translate(translate);

            var boxPoly = PolygonFactory<EEK>.CreateBox(box.Shape);
            DrawPolygon(boxPoly, faceColor, lineColor);

            var triPoly = PolygonFactory<EEK>.CreateTriangle(tri.Shape);
            DrawPolygon(triPoly, faceColor, lineColor);

            var result = CGALIntersections.Intersection(box, tri);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(Box2<EEK> box, Box2<EEK> box2, Point2d translate)
        {
            box.Translate(translate);
            box2.Translate(translate);

            var boxPoly = PolygonFactory<EEK>.CreateBox(box.Shape);
            DrawPolygon(boxPoly, faceColor, lineColor);

            var boxPoly2 = PolygonFactory<EEK>.CreateBox(box2.Shape);
            DrawPolygon(boxPoly2, faceColor, lineColor);

            var result = CGALIntersections.Intersection(box, box2);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(Box2<EEK> box, Ray2<EEK> ray, Point2d translate)
        {
            box.Translate(translate);
            ray.Translate(translate);

            var boxPoly = PolygonFactory<EEK>.CreateBox(box.Shape);
            DrawPolygon(boxPoly, faceColor, lineColor);

            Renderers["Shape:" + (buildStamp++)] = Draw().
            Outline(ray.Shape, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, ray);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(Box2<EEK> box, Segment2<EEK> seg, Point2d translate)
        {
            box.Translate(translate);
            seg.Translate(translate);

            var boxPoly = PolygonFactory<EEK>.CreateBox(box.Shape);
            DrawPolygon(boxPoly, faceColor, lineColor);

            Renderers["Shape:" + (buildStamp++)] = Draw().
            Outline(seg.Shape, lineColor).
            PopRenderer();

            var result = CGALIntersections.Intersection(box, seg);
            if (result.Hit)
            {
                CreateRenderer(result, intersectionColor);
            }
        }

        private void CreateIntersection(Box2d box, Polygon2<EEK> polygon, Point2d translate)
        {
            box += translate;
            polygon.Translate(translate);

            var boxPoly = PolygonFactory<EEK>.CreateBox(box);
            DrawPolygon(boxPoly, faceColor, lineColor);
            DrawPolygon(polygon, faceColor, lineColor);

            var results = new List<PolygonWithHoles2<EEK>>();
            if (polygon.Intersection(boxPoly, results))
            {
                foreach (var poly in results)
                {
                    DrawPolygon(poly, intersectionColor);
                }
            }
        }

        private void CreateIntersection(Box2<EEK> box, Polygon2<EEK> polygon, Point2d translate)
        {
            box.Translate(translate);
            polygon.Translate(translate);

            var boxPoly = PolygonFactory<EEK>.CreateBox(box.Shape);
            DrawPolygon(boxPoly, faceColor, lineColor);
            DrawPolygon(polygon, faceColor, lineColor);

            var results = new List<PolygonWithHoles2<EEK>>();
            if (polygon.Intersection(boxPoly, results))
            {
                foreach (var poly in results)
                {
                    DrawPolygon(poly, intersectionColor);
                }
            }
        }

        /// <summary>
        /// Create the intersect shape and draw it.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="col"></param>
        private void CreateRenderer(IntersectionResult2d result, Color col)
        {

            if (result.Type == INTERSECTION_RESULT_2D.POLYGON2)
            {
                //Intersection was a polygon.
                var polygon = result.Polygon<EEK>();
                DrawPolygon(polygon, intersectionColor);
            }
            else if(result.Type == INTERSECTION_RESULT_2D.POINT2)
            {
                //Intersection was a point.
                var point = result.Point;

                Renderers["Intersection:" + (buildStamp++)] = Draw().
                    Points(point, col, col).
                    PopRenderer();
            }
            else if (result.Type == INTERSECTION_RESULT_2D.LINE2)
            {
                //Intersection was a line.
                var line = result.Line;

                Renderers["Intersection:" + (buildStamp++)] = Draw().
                    Outline(line, col).
                    PopRenderer();
            }
            else if (result.Type == INTERSECTION_RESULT_2D.RAY2)
            {
                //Intersection was a ray.
                var ray = result.Ray;

                Renderers["Intersection:" + (buildStamp++)] = Draw().
                    Outline(ray, col).
                    PopRenderer();
            }
            else if (result.Type == INTERSECTION_RESULT_2D.SEGMENT2)
            {
                //Intersection was a segment.
                var segment = result.Segment;

                Renderers["Intersection:" + (buildStamp++)] = Draw().
                    Outline(segment, col).
                    PopRenderer();
            }
            else if (result.Type == INTERSECTION_RESULT_2D.BOX2)
            {
                //Intersection was a box, convert to polygon to draw.
                var polygon = PolygonFactory<EEK>.CreateBox(result.Box);
                DrawPolygon(polygon, intersectionColor);
            }
            else if (result.Type == INTERSECTION_RESULT_2D.TRIANGLE2)
            {
                //Intersection was a triangle, convert to polygon to draw.
                var polygon = PolygonFactory<EEK>.CreateTriangle(result.Triangle);
                DrawPolygon(polygon, intersectionColor);
            }
        }

        /// <summary>
        /// Add a polygon renderer.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="col">The polygons color.</param>
        private void DrawPolygon(Polygon2<EEK> polygon, Color col)
        {
            Renderers["Polygon:" + (buildStamp++)] = Draw().
            Faces(polygon, col).
            Outline(polygon, col).
            PopRenderer();
        }

        /// <summary>
        /// Add a polygon renderer.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="faceCol">The polygons face color.</param>
        /// <param name="outlineCol">The polygons outlineCol color.</param>
        private void DrawPolygon(Polygon2<EEK> polygon, Color faceCol, Color outlineCol)
        {
            Renderers["Polygon:" + (buildStamp++)] = Draw().
            Faces(polygon, faceCol).
            Outline(polygon, outlineCol).
            PopRenderer();
        }

        /// <summary>
        /// Add a polygon renderer.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="col">The polygons color.</param>
        private void DrawPolygon(PolygonWithHoles2<EEK> polygon, Color col)
        {
            Renderers["Polygon:" + (buildStamp++)] = Draw().
            Faces(polygon, col).
            Outline(polygon, col).
            PopRenderer();
        }

        /// <summary>
        /// Draw the scene.
        /// </summary>
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
