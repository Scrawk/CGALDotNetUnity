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

    public class PolygonBooleanExample : Polygon2Input
    {

        private Polygon2_EEK polygon1, polygon2;

        private List<PolygonWithHoles2_EEK> result;

        protected override void Start()
        {
            base.Start();
            ConsoleRedirect.Redirect();
            result = new List<PolygonWithHoles2_EEK>();
        }

        protected override void OnPolygonComplete()
        {
            if (polygon1 == null)
            {
                var input = CreateInputPolygon();

                ResetInput();
                ClearRenderers();

                if (input != null)
                {
                    polygon1 = input;
                    AddPolygon(polygon1, Color.green, Color.yellow);
                }
            }
            else if (polygon2 == null)
            {
                var input = CreateInputPolygon();

                ResetInput();
                ClearRenderers();

                if (input != null)
                {
                    polygon2 = input;

                    result.Clear();
                    PolygonBoolean2.Intersect(polygon1, polygon2, result);

                    foreach (var pwh in result)
                    {
                        var polygons = pwh.ToList();

                        foreach (var poly in polygons)
                        {
                            poly.Print();
                            AddPolygon(poly, Color.green, Color.yellow);
                        }
                    }
                }
  
            }
            else
            {
                ResetInput();
            }
        }

        private Polygon2_EEK CreateInputPolygon()
        {
            var input = new Polygon2_EEK(Points.ToArray());

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

        protected override void OnPolygonCleared()
        {
            polygon1 = null;
            polygon2 = null;
        }

        private void OnPostRender()
        {
            DrawInput();
            DrawPolygons();
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
