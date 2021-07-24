using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;
using System.Collections.Generic;
using UnityEngine;

namespace CGALDotNetUnity.Polygons
{

    public class CreatePolygonWithHolesExample : InputBehaviour
    {

        private Color lineColor = new Color32(20, 20, 20, 255);

        private Color pointColor = new Color32(20, 20, 20, 255);

        private Color faceColor = new Color32(120, 120, 120, 128);

        private PolygonWithHoles2<EEK> polygon;

        private Point2d? point;

        private bool containsPoint;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.POLYGON);
        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            if (polygon == null)
            {
                var input = CreateInputPolygon(points);

                ResetInput();
                ClearShapeRenderers();

                if (input != null)
                {
                    polygon = input;
                }

                AddPolygon("", polygon, lineColor, pointColor, faceColor);
            }
            else if(polygon.HoleCount < 2)
            {
                var input = CreateInputHole(points);

                ResetInput();
                ClearShapeRenderers();

                if (input != null)
                {
                    polygon.AddHole(input);

                    if (polygon.HoleCount == 2)
                        SetInputMode(INPUT_MODE.POINT_CLICK);
                }

                AddPolygon("", polygon, lineColor, pointColor, faceColor);
            }
     
        }

        protected override void OnLeftClick(Point2d point)
        {
            if (polygon != null)
            {
                this.point = point;
                containsPoint = polygon.ContainsPoint(point);
                SetPoint(this.point.Value);
            }
        }

        private PolygonWithHoles2<EEK> CreateInputPolygon(List<Point2d> points)
        {
            var input = new Polygon2<EEK>(points.ToArray());

            if (input.IsSimple)
            {
                if (input.IsClockWise)
                    input.Reverse();

                return new PolygonWithHoles2<EEK>(input);
            }
            else
            {
                return null;
            }
        }

        private Polygon2<EEK> CreateInputHole(List<Point2d> points)
        {
            var input = new Polygon2<EEK>(points.ToArray());

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

        protected override void OnCleared()
        {
            polygon = null;
            point = null;
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

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");

            if (polygon != null)
            { 
                GUI.Label(new Rect(10, 70, textLen, textHeight), "Add holes.");
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Holes = " + polygon.HoleCount);

                if (Mode == INPUT_MODE.POINT_CLICK)
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
