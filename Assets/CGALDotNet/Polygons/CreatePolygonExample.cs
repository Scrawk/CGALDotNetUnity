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
        private Color pointColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(20, 20, 255, 128);

        private Polygon2<EEK> polygon;

        private CGAL_ORIENTED_SIDE orientedSide;

        private bool containsPoint;

        private bool isSimple;

        private Point2d? point;

        protected override void Start()
        {
            base.Start();
            DrawGridAxis(true);
            SetInputMode(INPUT_MODE.POLYGON);
        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            polygon = new Polygon2<EEK>(points.ToArray());
            isSimple = polygon.IsSimple;
            InputPoints.Clear();
        }

        protected override void OnCleared()
        {
            polygon = null;
            point = null;
            isSimple = false;
            SetInputMode(INPUT_MODE.POLYGON);
            InputPoints.Clear();
        }

        protected override void OnLeftClickDown(Point2d point)
        {
            if (isSimple && polygon != null)
            {
                this.point = point;
                orientedSide = polygon.OrientedSide(point);
                containsPoint = polygon.ContainsPoint(point);
            }
        }

        private void OnPostRender()
        {
            DrawGrid();

            CreatePoints(InputPoints.ToArray(), null, pointColor, lineColor, 0.2f).Draw();

            if(point != null)
                CreatePoints(new Point2d[] { point.Value }, pointColor, lineColor, 0.2f).Draw();
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
