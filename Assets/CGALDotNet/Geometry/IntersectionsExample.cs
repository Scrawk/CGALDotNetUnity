using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;

namespace CGALDotNetUnity.Geometry
{
    public class IntersectionsExample : InputBehaviour
    {

        private Color shapeColor = new Color32(80, 80, 200, 128);

        private Color intersectionColor = new Color32(200, 80, 00, 128);

        private List<IGeometry2d> Geometry;

        private IGeometry2d ClickedOn;

        protected override void Start()
        {
            base.Start();

            SetInputMode(INPUT_MODE.POINT_CLICK);

            var box = new Box2d(-7, -5, -2, 5);

            var triangle = new Triangle2d(new Point2d(2, -5), new Point2d(7, -5), new Point2d(2, 5));

            var line = new Line2d(new Point2d(0, 10), new Point2d(1, 10));

            var segment = new Segment2d(new Point2d(-10, -10), new Point2d(10, -10));

            var ray = new Ray2d(new Point2d(0, 5), new Vector2d(1, 0));

            Geometry = new List<IGeometry2d>();
            Geometry.Add(box);
            Geometry.Add(triangle);
            Geometry.Add(line);
            Geometry.Add(segment);
            Geometry.Add(ray);

            AddGeometry();
            AddIntersections();
        }

        protected override void OnLeftClickUp(Point2d point)
        {
            ClickedOn = null;
        }

        protected override void OnLeftDrag(Point2d point, Point2d delta)
        {
            ClearShapeRenderers();

            ClickedOn = FindClikedOn(point);
            if(ClickedOn != null)
            {
                var t = (Vector2d)delta;
                var translate = Matrix3x3d.Translate(t);
                ClickedOn.Transform(translate);
            }

            AddGeometry();
            AddIntersections();
        }

        private void OnRenderObject()
        {
            DrawGrid();
            DrawShapes();
            DrawInput();
            DrawPoint();
        }

        private IGeometry2d FindClikedOn(Point2d point)
        {
            if(ClickedOn != null)
                    return ClickedOn;

            foreach (var geo in Geometry)
            {
                if (CGALIntersections.DoIntersect(point, geo))
                    return geo;
            }

            return null;
        }

        private void AddGeometry()
        {
            foreach (var geometry in Geometry)
                AddGeometry("", geometry, shapeColor, 0.1f);
        }

        private void AddIntersections()
        {
            int count = Geometry.Count;

            var check = new bool[count, count];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (check[i, j]) continue;
                    check[i, j] = true;
                    check[j, i] = true;

                    if (Geometry[i] == Geometry[j]) continue;

                    var result = CGALIntersections.Intersection(Geometry[i], Geometry[j]);

                    AddIntersection(result);
                }
            }
        }

        private void AddIntersection(IntersectionResult2d result)
        {
            switch (result.type)
            {
                case INTERSECTION_RESULT_2D.POINT2:
                    AddPoint("", result.Point, 0.1f, intersectionColor);
                    break;

                case INTERSECTION_RESULT_2D.LINE2:
                    AddLine("", result.Line, intersectionColor);
                    break;

                case INTERSECTION_RESULT_2D.RAY2:
                    AddRay("", result.Ray, intersectionColor);
                    break;

                case INTERSECTION_RESULT_2D.SEGMENT2:
                    AddSegment("", result.Segment, intersectionColor);
                    break;

                case INTERSECTION_RESULT_2D.TRIANGLE2:
                    AddTriangle("", result.Triangle, intersectionColor, intersectionColor);
                    break;

                case INTERSECTION_RESULT_2D.BOX2:
                    AddBox("", result.Box, intersectionColor, intersectionColor);
                    break;

                case INTERSECTION_RESULT_2D.POLYGON2:
                    var polygon = new Polygon2<EEK>(result.Polygon);
                    AddPolygon("", polygon, intersectionColor, intersectionColor);
                    break;
            }
        }

    }

}
