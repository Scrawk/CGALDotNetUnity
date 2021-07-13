using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;

namespace Common.VisualTest
{

    public class CreatePolygonExample : Polygon2Input
    {

        private Polygon2 polygon;

        private CGAL_ORIENTED_SIDE orientedSide;

        private bool isSimple;

        private Point2d? point;

        protected override void OnPolygonComplete()
        {
            polygon = new Polygon2_EIK(Points.ToArray());
            isSimple = polygon.IsSimple();
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
            }
        }

        protected override void OnPostRender()
        {

            if (!MadePolygon)
            {
                base.OnPostRender();
            }
            else
            {
                DrawPolygon(polygon, Color.blue, Color.yellow);

                if (point != null)
                    DrawPoint(point.Value, Color.green, 0.02f);
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
                    GUI.Label(new Rect(10, 70, textLen, textHeight), "Is Convex = " + polygon.IsConvex());
                    GUI.Label(new Rect(10, 90, textLen, textHeight), "Area = " + polygon.Area());
                    GUI.Label(new Rect(10, 110, textLen, textHeight), "Orientation = " + polygon.Orientation());

                    if (point != null)
                        GUI.Label(new Rect(10, 130, textLen, textHeight), "Point oriented side = " + orientedSide);
                    else
                        GUI.Label(new Rect(10, 130, textLen, textHeight), "Click to test point oriented side.");
                }
            }

        }


    }
}
