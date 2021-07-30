using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Triangulations;

namespace CGALDotNetUnity.Triangulations
{

    public class TriangulationExample : InputBehaviour
    {

        private Color lineColor = new Color32(20, 20, 20, 255);

        private Color pointColor = new Color32(200, 80, 80, 255);

        private Color circumColor = new Color32(200, 80, 80, 255);

        private Color constraintColor = new Color32(200, 80, 80, 255);

        private Color faceColor = new Color32(120, 120, 120, 128);

        private ConstrainedTriangulation2 triangulation;

        protected override void Start()
        {
            base.Start();

            //SetInputMode(INPUT_MODE.POINT);
            //SetPointSize(0.5f);
            //SetInputColor(lineColor, pointColor);
            //EnableInputPointOutline(true, lineColor);

            triangulation = new ConstrainedTriangulation2<EEK>();

            var points = new Point2d[]
            {
                new Point2d(-5, -5),
                new Point2d(5, -5),
                new Point2d(5, 5),
                new Point2d(-5, 5),
            };

            triangulation.InsertPoints(points);

            triangulation.InsertSegmentConstraint(new Point2d(0, 0), new Point2d(2, -2));

            AddTraingulation();

        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            var p = points[0];
            triangulation.InsertPoint(p);

            AddTraingulation();
        }

        private void AddTraingulation()
        {
            //ClearShapeRenderers();
            //AddTriangulationFaces("", triangulation, lineColor, pointColor, faceColor);
            AddConstraints();
            //AddTriangulationPoints("", triangulation, lineColor, pointColor, faceColor);
            //EnableShapePointOutline(true, lineColor);
        }

        private void AddCircumcircles()
        {
            int count = triangulation.FaceCount;
            if (count > 0)
            {
                var triangles = new Triangle2d[count];
                triangulation.GetTriangles(triangles);

                var circumcenters = new Circle2d[count];
                for (int i = 0; i < count; i++)
                    circumcenters[i] = triangles[i].CircumCircle();

                //AddPoints("", circumcenters, PointSize, circumColor);
                //AddCircles("", circumcenters, circumColor, 64);
            }
        }

        private void AddConstraints()
        {
            int count = triangulation.ConstrainedEdgeCount;
            if (count > 0)
            {
                var segments = new Segment2d[count];
                triangulation.GetConstraints(segments);
                //AddSegments("", segments, constraintColor);
            }
        }

        protected override void OnCleared()
        {
            triangulation.Clear();
        }

        protected override void OnLeftClickDown(Point2d point)
        {

        }

        private void OnPostRender()
        {
            DrawGrid();
            //DrawShapes();
            //DrawInput();
            //DrawPoint();
        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;

            GUI.color = Color.black;

            /*
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
            */

        }


    }
}
