using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;
using Common.Unity.Utility;

namespace Common.VisualTest
{

    public class CreatePolygonWithHolesExample : Polygon2Input
    {

        private PolygonWithHoles2_EEK polygon;

        private Point2d? point;

        private bool containsPoint;

        protected override void Start()
        {
            base.Start();
            ConsoleRedirect.Redirect();
        }

        protected override void OnPolygonComplete()
        {
            if (polygon == null)
            {
                var input = CreateInputPolygon();

                ResetInput();
                ClearRenderers();

                if (input != null)
                {
                    polygon = input;
                }

                AddPolygon(polygon, Color.green, Color.yellow);
            }
            else if(polygon.HoleCount < 2)
            {
                var input = CreateInputHole();

                ResetInput();
                ClearRenderers();

                if (input != null)
                {
                    polygon.AddHole(input);

                    if(polygon.HoleCount == 2)
                        MadePolygon = true;
                }

                AddPolygon(polygon, Color.green, Color.yellow);
            }
     
        }

        protected override void OnLeftClick(Point2d point)
        {
            if (MadePolygon)
            {
                this.point = point;
                containsPoint = polygon.ContainsPoint(point);
                SetPoint(this.point.Value, Color.blue);
            }
        }

        private PolygonWithHoles2_EEK CreateInputPolygon()
        {
            var input = new Polygon2_EEK(Points.ToArray());

            if (input.IsSimple)
            {
                if (input.IsClockWise)
                    input.Reverse();

                return new PolygonWithHoles2_EEK(input);
            }
            else
            {
                return null;
            }
        }

        private Polygon2_EEK CreateInputHole()
        {
            var input = new Polygon2_EEK(Points.ToArray());

            if (input.IsSimple && polygon.ContainsPolygon(input))
            {
                if (input.IsCounterClockWise)
                    input.Reverse();

                return input;
            }
            else
            {
                return null;
            }
        }

        protected override void OnPolygonCleared()
        {
            polygon = null;
            point = null;
        }

        private void OnPostRender()
        {
            DrawInput();
            DrawPolygons();
            DrawPoint();
        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");

            if (polygon != null)
            { 
                GUI.Label(new Rect(10, 70, textLen, textHeight), "Add holes.");
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Holes = " + polygon.HoleCount);

                if (MadePolygon)
                {
                    if (point != null)
                        GUI.Label(new Rect(10, 110, textLen, textHeight), "Contains point = " + containsPoint);
                    else
                        GUI.Label(new Rect(10, 110, textLen, textHeight), "Click to test contains point.");
                }
            }

        }


    }
}
