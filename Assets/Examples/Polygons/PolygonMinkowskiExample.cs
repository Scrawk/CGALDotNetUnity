using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using Common.Unity.Utility;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNet;
using CGALDotNet.Polygons;

namespace CGALDotNetUnity.Polygons
{


    public class PolygonMinkowskiExample : InputBehaviour
    {
        private Color sumColor = new Color32(200, 80, 80, 128);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private MINKOWSKI_DECOMPOSITION_PWH Decomposition = MINKOWSKI_DECOMPOSITION_PWH.TRIANGULATION;

        private PolygonWithHoles2<EEK> Polygon;

        private Polygon2<EEK> Shape;

        private PolygonWithHoles2<EEK> Sum;

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

            //Create the shape polygon thats going to
            //be used to perform the Minkowski sum.
            CreateShape();

            //Polygon = CreatePolygonWithHolesExample.CreateRoom();
            //PerformMinkowskiSum();
        }

        /// <summary>
        /// This function is called once a set of at least 3 points has been 
        /// placed and the last point is placed on the first point.
        /// </summary>
        /// <param name="points"></param>
        protected override void OnInputComplete(List<Point2d> points)
        {
            if (Polygon == null)
            {
                //Create the polygons boundary from the points.
                //We need to use the EEK kerel as I am doing some
                //boolean ops for the rendering.
                var boundary = new Polygon2<EEK>(points.ToArray());

                //Polygon must be simple to continue.
                if (boundary.IsSimple)
                {
                    //The boundary must be ccw.
                    if (!boundary.IsCounterClockWise)
                        boundary.Reverse();

                    //Create the polygon.
                    Polygon = new PolygonWithHoles2<EEK>(boundary);

                    //Create renderer to draw polygon.
                    CreateRenderer("Polygon", Polygon, faceColor);
                }
                else
                {
                    Debug.Log("Polygon was not simple.");
                }
            }
            else if (AddHoles)
            {
                //The poylgon has beed create so now we can add some holes
                var hole = new Polygon2<EEK>(points.ToArray());
                
                //Hole must be cw
                if (!hole.IsClockWise)
                    hole.Reverse();


                if (PolygonWithHoles2.IsValidHole(Polygon, hole))
                {
                    //Hole is valid. Add the holes and recreate the renderer.

                    Polygon.AddHole(hole);
                    int holes = Polygon.HoleCount;

                    CreateRenderer("Polygon", Polygon, faceColor);
                    CreateRenderer("Hole" + holes, hole, faceColor);
                }
                else
                {
                    Debug.Log("Hole was not valid.");
                }
            }

            InputPoints.Clear();
        }

        /// <summary>
        /// Create the shape to use for the Minkowski sum.
        /// Can be any poygon shape.
        /// </summary>
        private void CreateShape()
        {
            //Shape = PolygonFactory<EEK>.CreateBox(-0.5, 0.5);
            Shape = PolygonFactory<EEK>.CreateCircle(0.5, 16);
        }

        private void CreateRenderer(string name, Polygon2<EEK> polygon, Color col)
        {
            Renderers[name] = Draw().
            Outline(polygon, lineColor).
            Points(polygon, lineColor, col).
            PopRenderer();
        }

        private void CreateRenderer(string name, PolygonWithHoles2<EEK> polygon, Color col)
        {
            Renderers[name] = Draw().
            Faces(polygon, col).
            Outline(polygon, lineColor).
            Points(polygon, lineColor, col).
            PopRenderer();
        }

        protected override void OnCleared()
        {
            Polygon = null;
            Renderers.Clear();
            InputPoints.Clear();
            SetInputMode(INPUT_MODE.POLYGON);
        }

        protected override void Update()
        {
            base.Update();

            if (Polygon == null) return;

            if (Input.GetKeyDown(KeyCode.F1))
            {
                AddHoles = false;
                PerformMinkowskiSum();
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                AddHoles = true;
                ClearRenderer("Sum");
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                Decomposition = CGALEnum.Next(Decomposition);
                PerformMinkowskiSum();
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

        /// <summary>
        /// Perfoms the Minkowski sum.
        /// </summary>
        private void PerformMinkowskiSum()
        {
            ClearRenderer("Polygon");
            ClearRenderer("Sum");

            //Get the instance
            var minkowski = PolygonMinkowski<EEK>.Instance;
            var boo = PolygonBoolean2<EEK>.Instance;

            //Compute the minkowski sum.
            Sum = minkowski.Sum(Decomposition, Polygon, Shape);

            //This is just for rendering.
            //Take the intersection of the sum and the input polygon.
            var results = new List<PolygonWithHoles2<EEK>>();
            boo.Op(POLYGON_BOOLEAN.DIFFERENCE, Sum, Polygon, results);

            //Create the renderers
            for(int j = 0; j < results.Count; j++)
            {
                CreateRenderer("Sum"+j, results[j], sumColor);

                for(int i = 0; i < results[j].HoleCount; i++)
                    CreateRenderer("SumHole" + j + " " + i, results[j].GetHole(i), sumColor);   
            }
            
            CreateRenderer("Polygon", Polygon, faceColor);

            for (int i = 0; i < Polygon.HoleCount; i++)
               CreateRenderer("PolygonHole" + i, Polygon.GetHole(i), faceColor);

        }

        /// <summary>
        /// Clear the renderer with the name.
        /// </summary>
        /// <param name="name"></param>
        private void ClearRenderer(string name)
        {
            var keys = new List<string>(Renderers.Keys);

            foreach (var key in keys)
            {
                if (key.Contains(name))
                    Renderers.Remove(key);
            }
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
                GUI.Label(new Rect(10, 70, textLen, textHeight), "F1 to stop adding holes and show the minkowski sum.");
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Holes must be simple and not intersect the polygon boundary or other holes.");
            }
            else
            {
                GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear polygon.");
                GUI.Label(new Rect(10, 30, textLen, textHeight), "Tab to change decomposition mode");
                GUI.Label(new Rect(10, 50, textLen, textHeight), "Decomposition = " + Decomposition);
                GUI.Label(new Rect(10, 70, textLen, textHeight), "F2 to start adding holes again.");
            }

        }



    }
}
