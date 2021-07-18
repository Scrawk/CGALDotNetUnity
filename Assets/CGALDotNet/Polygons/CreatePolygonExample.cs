using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;

namespace CGALDotNetUnity.Polygons
{

    public class CreatePolygonExample : Polygon2Input
    {

        private Polygon2 polygon;

        private CGAL_ORIENTED_SIDE orientedSide;

        private bool containsPoint;

        private bool isSimple;

        private Point2d? point;

        protected override void OnPolygonComplete()
        {
            polygon = new Polygon2<EEK>(Points.ToArray());
            isSimple = polygon.IsSimple;

            AddPolygon(polygon, isSimple ? Color.green : Color.red, Color.yellow);
        }

        protected override void OnPolygonCleared()
        {
            point = null;
            isSimple = false;
        }

        protected override void OnLeftClick(Point2d point)
        {
            if (MadePolygon && isSimple)
            {
                this.point = point;
                orientedSide = polygon.OrientedSide(point);
                containsPoint = polygon.ContainsPoint(point);
                SetPoint(this.point.Value, Color.blue);
            }
        }

        private void OnPostRender()
        {

            if (!MadePolygon)
            {
                DrawInput();
            }
            else
            {
                DrawPolygons();
                DrawPoint();
            }

        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;

            if (!MadePolygon)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
            }
            else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Count = " + polygon.Count);

                GUI.Label(new Rect(10, 50, textLen, textHeight), "Is Simple = " + isSimple);

                if (isSimple)
                {
                    GUI.Label(new Rect(10, 70, textLen, textHeight), "Is Convex = " + polygon.FindIfConvex());
                    GUI.Label(new Rect(10, 90, textLen, textHeight), "Area = " + polygon.FindArea());
                    GUI.Label(new Rect(10, 110, textLen, textHeight), "Orientation = " + polygon.FindOrientation());

                    if (point != null)
                    {
                        GUI.Label(new Rect(10, 130, textLen, textHeight), "Point oriented side = " + orientedSide);
                        GUI.Label(new Rect(10, 150, textLen, textHeight), "Contains point = " + containsPoint);
                    }
                    else
                        GUI.Label(new Rect(10, 130, textLen, textHeight), "Click to test point oriented side.");
                }
            }

        }


    }
}
