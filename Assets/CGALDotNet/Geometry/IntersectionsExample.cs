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

            //SetInputMode(INPUT_MODE.POINT_CLICK);

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

        }

        protected override void OnLeftClickUp(Point2d point)
        {
            ClickedOn = null;
        }

        protected override void OnLeftDrag(Point2d point, Point2d delta)
        {
            //ClearShapeRenderers();

            ClickedOn = FindClikedOn(point);
            if(ClickedOn != null)
            {
                var t = (Vector2d)delta;
                var translate = Matrix3x3d.Translate(t);
                ClickedOn.Transform(translate);
            }
        }

        private void OnRenderObject()
        {
            DrawGrid();
            //DrawShapes();
            //DrawInput();
            //DrawPoint();
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


    }

}
