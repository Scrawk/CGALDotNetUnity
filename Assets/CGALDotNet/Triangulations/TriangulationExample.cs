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

        private Color outLineColor = new Color32(20, 20, 20, 255);

        private Color faceColor = new Color32(120, 120, 120, 128);

        private Triangulation2<EEK> triangulation;

        protected override void Start()
        {
            base.Start();

            SetInputMode(INPUT_MODE.POINT);

            triangulation = new Triangulation2<EEK>();
        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            
            var p = points[0];
            triangulation.InsertPoint(p);

            ClearShapeRenderers();
            AddTriangulation(triangulation, outLineColor, faceColor);
        }

        protected override void OnCleared()
        {
            triangulation.Clear();
        }

        protected override void OnLeftClick(Point2d point)
        {

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
