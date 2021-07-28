using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;

namespace CGALDotNetUnity.Polygons
{

    public class CreatePolygonExample : InputBehaviour
    {

        private Color lineColor  = new Color32(20, 20, 20, 255);

        private Color pointColor = new Color32(20, 20, 20, 255);

        private Color faceColor = new Color32(120, 120, 120, 128);

        private Polygon2<EEK> polygon;

        private CGAL_ORIENTED_SIDE orientedSide;

        private bool containsPoint;

        private bool isSimple;

        private Point2d? point;

        protected override void Start()
        {
            base.Start();

            SetInputMode(INPUT_MODE.POLYGON);
        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            polygon = new Polygon2<EEK>(points.ToArray());
            isSimple = polygon.IsSimple;

            var lineCol = isSimple ? lineColor : Color.red;
            var pointCol = isSimple ? pointColor : Color.red;

            AddPolygon<EEK>("", polygon, lineCol, pointCol, faceColor);

            SetInputMode(INPUT_MODE.POINT_CLICK);
        }

        protected override void OnCleared()
        {
            polygon = null;
            point = null;
            isSimple = false;
            SetInputMode(INPUT_MODE.POLYGON);
        }

        protected override void OnLeftClickDown(Point2d point)
        {
            if (isSimple && polygon != null)
            {
                this.point = point;
                orientedSide = polygon.OrientedSide(point);
                containsPoint = polygon.ContainsPoint(point);
                SetPoint(this.point.Value);
            }
        }

        private void OnPostRender()
        {
            DrawGrid();
            DrawShapes();
            DrawInput();
            DrawPoint();
        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;

            GUI.color = Color.black;

            if (polygon == null)
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
