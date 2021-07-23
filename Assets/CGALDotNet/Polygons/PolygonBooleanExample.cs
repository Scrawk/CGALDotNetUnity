using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;
using Common.Unity.Utility;

namespace CGALDotNetUnity.Polygons
{

    public class PolygonBooleanExample : InputBehaviour
    {

        private Color outLineColor = new Color32(20, 20, 20, 255);

        private Color faceColor = new Color32(120, 120, 120, 128);

        private Polygon2<EEK> polygon1, polygon2;

        private List<PolygonWithHoles2<EEK>> result;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.POLYGON);
            ConsoleRedirect.Redirect();
            result = new List<PolygonWithHoles2<EEK>>();
        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            if (polygon1 == null)
            {
                var input = CreateInputPolygon(points);

                ResetInput();
                ClearShapeRenderers();

                if (input != null)
                {
                    polygon1 = input;
                    AddPolygon(polygon1, outLineColor, faceColor);
                }
            }
            else if (polygon2 == null)
            {
                var input = CreateInputPolygon(points);

                ResetInput();
                ClearShapeRenderers();

                if (input != null)
                {
                    polygon2 = input;

                    result.Clear();
                    PolygonBoolean2<EEK>.Intersect(polygon1, polygon2, result);

                    foreach (var pwh in result)
                    {
                        var polygons = pwh.ToList();

                        foreach (var poly in polygons)
                        {
                            poly.Print();
                            AddPolygon(poly, outLineColor, faceColor);
                        }
                    }
                }
  
            }
            else
            {
                ResetInput();
            }
        }

        private Polygon2<EEK> CreateInputPolygon(List<Point2d> points)
        {
            if (points == null) 
                return null;

            var input = new Polygon2<EEK>(points.ToArray());

            if (input.IsSimple)
            {
                if (input.IsClockWise)
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
            polygon1 = null;
            polygon2 = null;
        }

        private void OnPostRender()
        {
            DrawGrid();
            DrawShapes();
            DrawInput();
        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;
            GUI.color = Color.black;

            //if (!MadeInput)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
            }
            //else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                //GUI.Label(new Rect(10, 30, textLen, textHeight), "Count = " + polygon.Count);

                //GUI.Label(new Rect(10, 50, textLen, textHeight), "Is Simple = " + isSimple);

                /*
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
                */
            }

        }


    }
}
