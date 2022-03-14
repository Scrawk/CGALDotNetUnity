using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using Common.Unity.Utility;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNet.Triangulations;

namespace CGALDotNetUnity.Polygons
{

    public class CreatePolygonExample : InputBehaviour
    {

        private Color redColor = new Color32(200, 80, 80, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Point2d? Point;

        private Polygon2<EIK> Polygon;

        private Dictionary<string, CompositeRenderer> Renderers;

        protected override void Start()
        {
            //Init base class
            base.Start();

            //Set the input mode to polygons.
            //Determines when OnInputComplete is called.
            SetInputMode(INPUT_MODE.POLYGON);

            //Create the place to store the renderers that draw the shapes.
            Renderers = new Dictionary<string, CompositeRenderer>();

            //ConsoleRedirect.Redirect();
            //Polygon = PolygonFactory<EIK>.CreateBox(-5, 5);
            //CreateRenderer("Polygon", Polygon);
        }

        /// <summary>
        /// This function is called once a set of at least 3 points has been 
        /// placed and the last point is placed on the first point.
        /// </summary>
        /// <param name="points"></param>
        protected override void OnInputComplete(List<Point2d> points)
        {
            //Create the polygon from the points.
            //We just use the EIK kernel as its the fastest.
            Polygon = new Polygon2<EIK>(points.ToArray());

            //Polygon must be simple to continue.
            if (Polygon.IsSimple)
            {
                //Polygons must be ccw.
                if (!Polygon.IsCounterClockWise)
                    Polygon.Reverse();

                //Create renderer to draw polygon.
                CreateRenderer("Polygon", Polygon);

                //Change input mode to click.
                //This will call OnLeftClickDown when left mouse clicked.
                SetInputMode(INPUT_MODE.POINT_CLICK);
            }
            else
            {
                CreateRenderer("Polygon", Polygon);
            }

            InputPoints.Clear();
        }

        /// <summary>
        /// Create the renderer that daws the polygon.
        /// This just ueses unitys GL to draw lines and points.
        /// Its not very fast and just used for demos.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="polygon"></param>
        private void CreateRenderer(string name, Polygon2 polygon)
        {
            if(polygon.IsSimple)
            {
                //var t = new List<Triangle2d>();
                //polygon.Triangulate(t);

                Renderers[name] = Draw().
                Faces(polygon, faceColor).
                Outline(polygon, lineColor).
                Points(polygon, lineColor, pointColor).
                PopRenderer();
            }
            else
            {
                //If not simple draw in red.
                Renderers[name] = Draw().
                Faces(polygon, redColor).
                Outline(polygon, redColor).
                Points(polygon, lineColor, pointColor).
                PopRenderer();
            }
        }

        private void CreateRenderer(string name, Polygon2 polygon, Color col)
        {
            Renderers[name] = Draw().
            Outline(polygon, col).
            PopRenderer();
        }

        private void CreateRenderer(string name, BaseTriangulation2 triangulation)
        {
            Renderers[name] = Draw().
            Outline(triangulation, lineColor).
            PopRenderer();
        }

        /// <summary>
        /// Called when scene is cleared.
        /// </summary>
        protected override void OnCleared()
        {
            Point = null;
            Polygon = null;
            Renderers.Clear();
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
            int textLen = 400;
            int textHeight = 25;
            GUI.color = Color.black;

            if (Polygon == null || !Polygon.IsSimple)
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");
            }
            else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Count = " + Polygon.Count);

                bool isSimple = Polygon.IsSimple;

                GUI.Label(new Rect(10, 50, textLen, textHeight), "Is Simple = " + isSimple);

                if (isSimple)
                {
                    GUI.Label(new Rect(10, 70, textLen, textHeight), "Is Convex = " + Polygon.FindIfConvex());
                    GUI.Label(new Rect(10, 90, textLen, textHeight), "Area = " + Polygon.FindArea());
                    GUI.Label(new Rect(10, 110, textLen, textHeight), "Orientation = " + Polygon.FindOrientation());

                    if (Point != null)
                    {
                        GUI.Label(new Rect(10, 130, textLen, textHeight), "Point oriented side = " + Polygon.OrientedSide(Point.Value));
                        GUI.Label(new Rect(10, 150, textLen, textHeight), "Point bounded side = " + Polygon.BoundedSide(Point.Value));
                        GUI.Label(new Rect(10, 170, textLen, textHeight), "Contains point = " + Polygon.ContainsPoint(Point.Value));
                    }
                    else
                        GUI.Label(new Rect(10, 130, textLen, textHeight), "Click to test point oriented side.");
                }
            }

        }



    }
}
