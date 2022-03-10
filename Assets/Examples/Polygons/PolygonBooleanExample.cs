using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using Common.Unity.Utility;

using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;

namespace CGALDotNetUnity.Polygons
{


    public class PolygonBooleanExample : InputBehaviour
    {
        private Color redColor = new Color32(200, 80, 80, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private List<PolygonWithHoles2<EEK>> Polygons;

        private Dictionary<string, CompositeRenderer> Renderers;

        private POLYGON_BOOLEAN Op = POLYGON_BOOLEAN.JOIN;

        protected override void Start()
        {
            //Init base class
            base.Start();

            //Set the input mode to polygons.
            //Determines when OnInputComplete is called.
            SetInputMode(INPUT_MODE.POLYGON);

            //Create the place to store the renderers that draw the shapes.
            Renderers = new Dictionary<string, CompositeRenderer>();

            //Create a list to hold the polygons.
            //Kernel should be EEK for boolean ops. 
            //If EIK is used precision will be a issue.
            Polygons = new List<PolygonWithHoles2<EEK>>();
        }

        /// <summary>
        /// This function is called once a set of at least 3 points has been 
        /// placed and the last point is placed on the first point.
        /// </summary>
        /// <param name="points"></param>
        protected override void OnInputComplete(List<Point2d> points)
        {
            
            if (Polygons.Count == 0)
            {
                //if this is the first polygon create the boundary.
                var boundary = new Polygon2<EEK>(points.ToArray());

                if (boundary.IsSimple)
                {
                    //Must be simple and ccw.
                    if (!boundary.IsCounterClockWise)
                        boundary.Reverse();

                    //If boundary is valid create a polygon with hole object.
                    var polygon = new PolygonWithHoles2<EEK>(boundary);
                    Polygons.Add(polygon);

                    //As polygons are added rebuild renderers
                    for(int i = 0; i < Polygons.Count; i++)
                        CreateRenderer(i, Polygons[i]);

                }
            }
            else
            {
                //This is not the first polygon created.
                var polygon = new Polygon2<EEK>(points.ToArray());

                if(polygon.IsSimple)
                {
                    //Must be simple and ccw.
                    if (!polygon.IsCounterClockWise)
                        polygon.Reverse();

                    //create a tmp list of the polygons aready made
                    var tmp = new List<PolygonWithHoles2<EEK>>(Polygons);
                    Polygons.Clear();

                    foreach(var poly in tmp)
                    {
                        //for each polygon created perform
                        //the boolean op agaist the new polygon.

                        //you can use the boolean instance 
                        //to save creating a new object.
                        var boo = PolygonBoolean2<EEK>.Instance;

                        //Checking input can be disabled if we know input is good
                        //boo.CheckInput = false;

                        if (boo.Op(Op, polygon, poly, Polygons))
                        {
                            //If the op was successfull the results
                            //will have been writen into the Polygon list.

                            //Recreate the renderers to draw the polygons
                            for (int i = 0; i < Polygons.Count; i++)
                                CreateRenderer(i, Polygons[i]);

                            break;
                        }
                    }
                }

            }

            InputPoints.Clear();
        }

        /// <summary>
        /// Create the renderer that daws the polygon.
        /// This just ueses unitys GL to draw lies and points.
        /// Its not very fast and just used for demos.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="polygon"></param>
        private void CreateRenderer(int id, PolygonWithHoles2<EEK> polygon)
        {
            Renderers["Polygon " + id] = Draw().
                Faces(polygon, faceColor).
                Outline(polygon, lineColor).
                Points(polygon, lineColor, pointColor, PointSize).
                PopRenderer();

            int holes = polygon.HoleCount;
            for (int i = 0; i < holes; i++)
            {
                var hole = polygon.Copy(POLYGON_ELEMENT.HOLE, i);

                Renderers["Hole " + i + " " + id] = Draw().
                Outline(hole, lineColor).
                Points(hole, lineColor, pointColor, PointSize).
                PopRenderer();
            }
        }

        /// <summary>
        /// Called when scene is cleared.
        /// </summary>
        protected override void OnCleared()
        {
            Polygons.Clear();
            Renderers.Clear();
            InputPoints.Clear();
            SetInputMode(INPUT_MODE.POLYGON);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                //switch the op as tab pressed.
                Op = CGALEnum.Next(Op);

                //Triangulation polygons for symetric difference
                //not working at the moment so skip.
                if (Op == POLYGON_BOOLEAN.SYMMETRIC_DIFFERENCE)
                    Op = POLYGON_BOOLEAN.JOIN;
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
            int textLen = 400;
            int textHeight = 25;
            GUI.color = Color.black;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "Left click to place point.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Click on first point to close polygon.");

            GUI.Label(new Rect(10, 70, textLen, textHeight), "Tab to change boolean op.");
            GUI.Label(new Rect(10, 90, textLen, textHeight), "Current op = " + Op);
        }

    }
}
