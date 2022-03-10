using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNet;
using CGALDotNet.Polygons;

namespace CGALDotNetUnity.Polygons
{

    public class CreatePolygonWithHolesExample : InputBehaviour
    {
        private Color redColor = new Color32(200, 80, 80, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Point2d? Point;

        private PolygonWithHoles2<EIK> Polygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        private bool AddHoles = true;

        protected override void Start()
        {
            //Init base class
            base.Start();

            //Set the input mode to polygons.
            //Determines when OnInputComplete is called.
            SetInputMode(INPUT_MODE.POLYGON);

            //Create the place to store the renderers that draw the shapes.
            Renderers = new Dictionary<string, CompositeRenderer>();
        }

        /// <summary>
        /// Creates a interesting polygon to use as a demo or debuging.
        /// </summary>
        /// <returns></returns>
        public static PolygonWithHoles2<EIK> CreateKochStar()
        {
            var koch = PolygonFactory<EIK>.KochStar(30, 3);
            var hole = PolygonFactory<EIK>.CreateCircle(5, 32);
            hole.Reverse();

            var polygon = new PolygonWithHoles2<EIK>(koch);
            polygon.AddHole(hole);

            return polygon;
        }

        /// <summary>
        /// Creates a interesting polygon to use as a demo or debuging.
        /// </summary>
        /// <returns></returns>
        public static PolygonWithHoles2<EEK> CreateRoom()
        {
            var room = PolygonFactory<EEK>.CreateBox(-15, 15);

            var hole1 = PolygonFactory<EEK>.CreateBox(5, 10, false);
            var hole2 = PolygonFactory<EEK>.CreateBox(-10, -5, false);
            var hole3 = PolygonFactory<EEK>.CreateBox(new Point2d(-10,5), new Point2d(-5,10), false);

            var polygon = new PolygonWithHoles2<EEK>(room);
            polygon.AddHole(hole1);
            polygon.AddHole(hole2);
            polygon.AddHole(hole3);

            return polygon;
        }

        protected override void OnInputComplete(List<Point2d> points)
        {

            if(Polygon == null)
            {
                //Create the polygons boundary from the points.
                //We just use the EIK kernel as its the fastest.
                var boundary = new Polygon2<EIK>(points.ToArray());

                //Polygon must be simple to continue.
                if (boundary.IsSimple)
                {
                    //The boundary must be ccw.
                    if (!boundary.IsCounterClockWise)
                        boundary.Reverse();

                    Polygon = new PolygonWithHoles2<EIK>(boundary);

                    //Create renderer to draw polygon.
                    CreateRenderer("Polygon", Polygon);
                }
                else
                {
                    Debug.Log("Polygon was not simple.");
                }
            }
            else if(AddHoles)
            {
                //A polygon has already been created for the boundary
                //so this polygon must be a hole.
                var hole = new Polygon2<EIK>(points.ToArray());

                //holes must be cw.
                if (!hole.IsClockWise)
                    hole.Reverse();

                //Holes must be simple, cw and be contained in the
                //polygon and not intersect any other holes.
                if (PolygonWithHoles2.IsValidHole(Polygon, hole))
                {
                    Polygon.AddHole(hole);
                    int holes = Polygon.HoleCount;

                    //We need to recreate the polygon 
                    //renderer as well if a hole is added.  
                    CreateRenderer("Polygon", Polygon);

                    //Create the holes renderer.
                    CreateRenderer("Hole" + holes, hole);
                }
                else
                {
                    Debug.Log("Hole was not valid.");
                }
            }

            InputPoints.Clear();
        }

        /// <summary>
        /// Create the renderer that daws the polygon.
        /// This just ueses unitys GL to draw lies and points.
        /// Its not very fast and just used for demos.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="polygon"></param>
        private void CreateRenderer(string name, PolygonWithHoles2<EIK> polygon)
        {
            Renderers[name] = Draw().
            Faces(polygon, faceColor).
            Outline(polygon, lineColor).
            Points(polygon, lineColor, pointColor, PointSize).
            PopRenderer();
        }

        private void CreateRenderer(string name, Polygon2<EIK> hole)
        {
            Renderers[name] = Draw().
            Outline(hole, lineColor).
            Points(hole, lineColor, pointColor, PointSize).
            PopRenderer();
        }

        protected override void OnCleared()
        {
            Point = null;
            Polygon = null;
            Renderers.Clear();
            AddHoles = true;
            InputPoints.Clear();
            SetInputMode(INPUT_MODE.POLYGON);
        }

        /// <summary>
        /// Called when input mode is changed to point.
        /// </summary>
        /// <param name="point"></param>
        protected override void OnLeftClickDown(Point2d point)
        {
            //save the point.
            Point = point;

            //Create render to draw point.
            Renderers["Point"] = Draw().
                Points(Point.Value, lineColor, redColor, PointSize).
                PopRenderer();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.F1))
            {
                AddHoles = false;
                SetInputMode(INPUT_MODE.POINT_CLICK);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                AddHoles = true;
                SetInputMode(INPUT_MODE.POLYGON);
            }
        }

        /// <summary>
        /// Draw the renderers in post render.
        /// </summary>
        private void OnPostRender()
        {
            //This draws the grid
            DrawGrid();

            //This draws the polygon and the input point.
            foreach (var renderer in Renderers.Values)
                renderer.Draw();

            //This draws the line of points the user
            //is creating but they dont for a polygon yet.
            DrawInput(lineColor, pointColor, PointSize);
        }

        protected void OnGUI()
        {
            int textLen = 1000;
            int textHeight = 25;
            GUI.color = Color.black;

            if (Polygon == null)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
            }
            else if (AddHoles)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Add holes to polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
                GUI.Label(new Rect(10, 70, textLen, textHeight), "F1 to stop adding holes and show the polygons properties.");
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Holes must be simple and not intersect the polygon boundary or other holes.");
            }
            else
            {

                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "F2 to start adding holes again.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Hole Count = " + Polygon.HoleCount);

                bool isSimple = Polygon.FindIfSimple(POLYGON_ELEMENT.BOUNDARY);
                GUI.Label(new Rect(10, 70, textLen, textHeight), "Is Simple = " + isSimple);

                if (isSimple)
                {
                    GUI.Label(new Rect(10, 90, textLen, textHeight), "Is Convex = " + Polygon.FindIfConvex(POLYGON_ELEMENT.BOUNDARY));
                    GUI.Label(new Rect(10, 110, textLen, textHeight), "Area = " + Polygon.FindArea(POLYGON_ELEMENT.BOUNDARY));
                    GUI.Label(new Rect(10, 130, textLen, textHeight), "Orientation = " + Polygon.FindOrientation(POLYGON_ELEMENT.BOUNDARY));

                    if (Point != null)
                    {
                        GUI.Label(new Rect(10, 150, textLen, textHeight), "Contains point = " + Polygon.ContainsPoint(Point.Value));
                    }
                    else
                        GUI.Label(new Rect(10, 150, textLen, textHeight), "Click to test contains point.");
                }

            }

        }



    }
}
