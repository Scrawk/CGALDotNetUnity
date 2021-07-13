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

        private PolygonWithHoles2_EEK result;

        private void Start()
        {
            ConsoleRedirect.Redirect();
        }

        protected override void OnPolygonComplete()
        {
            if (polygon1 == null)
            {
                polygon1 = new Polygon2_EEK(Points.ToArray());
                ResetInput();
            }
            else if (polygon2 == null)
            {
                polygon2 = new Polygon2_EEK(Points.ToArray());

                PolygonBoolean2.Join(polygon1, polygon2, out result);

                if (result != null)
                    result.Print();
  
            }
        }

        protected override void OnPolygonCleared()
        {
            polygon1 = null;
            polygon2 = null;
        }

        protected override void OnPostRender()
        {
            base.OnPostRender();

            DrawPolygon(polygon1, Color.blue, Color.yellow);
            DrawPolygon(polygon2, Color.green, Color.yellow);
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
