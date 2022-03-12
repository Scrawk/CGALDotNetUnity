using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNet.Triangulations;

namespace CGALDotNetUnity.Triangulations
{

    public class Triangulation2Example : InputBehaviour
    {

        public enum CLICK_MODE
        {
            CLICK_TO_ADD_POINT,
            CLICK_TO_SELECT_POINT,
            CLICK_TO_SELECT_EDGE,
            CLICK_TO_SELECT_FACE
        }

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color redColor = new Color32(200, 80, 80, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        private BaseTriangulation2 triangulation;

        private CLICK_MODE ClickMode = CLICK_MODE.CLICK_TO_ADD_POINT;

        private TRIANGULATION2 Type = TRIANGULATION2.CONSTRAINED_DELAUNAY;

        private TriVertex2? SelectedVertex;

        private TriFace2? SelectedFace;

        private TriEdge2? SelectedEdge;

        private Triangle2d SelectedTriangle;

        protected override void Start()
        {
            //Init the base class.
            base.Start();

            //Create the renderers to draw the triangulations.
            Renderers = new Dictionary<string, CompositeRenderer>();

            //Set input mode to point. This will call OnLeftClickDown when mouse pressed.
            SetInputMode(INPUT_MODE.POINT_CLICK);

            //Create the triangulation.
            CreateTriangulation(Type);
        }

        /// <summary>
        /// Create the trianglation given the type enum
        /// and add 3 points to make a triangle.
        /// </summary>
        /// <param name="type">THe type to create</param>
        private void CreateTriangulation(TRIANGULATION2 type)
        {
            switch (type)
            {
                case TRIANGULATION2.TRIANGULATION:
                    triangulation = new Triangulation2<EEK>();
                    break;
                case TRIANGULATION2.DELAUNAY:
                    triangulation = new DelaunayTriangulation2<EEK>();
                    break;
                case TRIANGULATION2.CONSTRAINED:
                    triangulation = new ConstrainedTriangulation2<EEK>();
                    break;
                case TRIANGULATION2.CONSTRAINED_DELAUNAY:
                    triangulation = new ConstrainedDelaunayTriangulation2<EEK>();
                    break; ;
            }

            triangulation.Insert(new Point2d(-5, -5));
            triangulation.Insert(new Point2d(5, -5));
            triangulation.Insert(new Point2d(0, 5));

            Renderers.Clear();
            BuildTriangulationRenderer();
        }

        /// <summary>
        /// Left click was down.
        /// Could add a new point or select a element.
        /// </summary>
        /// <param name="point"></param>
        protected override void OnLeftClickDown(Point2d point)
        {

            switch (ClickMode)
            {
                case CLICK_MODE.CLICK_TO_ADD_POINT:
                    AddPoint(point);
                    break;

                case CLICK_MODE.CLICK_TO_SELECT_POINT:
                    SelectPoint(point);
                    break;

                case CLICK_MODE.CLICK_TO_SELECT_EDGE:
                    SelectEdge(point);
                    break;

                case CLICK_MODE.CLICK_TO_SELECT_FACE:
                    SelectFace(point);
                    break;
            }

        }

        /// <summary>
        /// Add a new point to triangulation
        /// </summary>
        /// <param name="point"></param>
        private void AddPoint(Point2d point)
        {
            UnselectAll();

            triangulation.Insert(point);
            //Must rebuild renderer.
            BuildTriangulationRenderer();
        }

        /// <summary>
        /// Try to select a point in triagulation.
        /// </summary>
        /// <param name="point">The left click point</param>
        private void SelectPoint(Point2d point)
        {
            UnselectAll();

            //If selection is with this distance
            //to element it counts as being cliked on.
            double radius = 0.5;

            if (triangulation.LocateVertex(point, radius, out TriVertex2 vert))
            {
                SelectedVertex = vert;
                //Must rebuild renderer.
                BuildSelectionRenderer();
            }
        }

        /// <summary>
        /// Try to select a edge in triagulation.
        /// </summary>
        /// <param name="point">The left click point</param>
        private void SelectEdge(Point2d point)
        {
            UnselectAll();

            //If selection is with this distance
            //to element it counts as being cliked on.
            double radius = 0.5;

            if (triangulation.LocateEdge(point, radius, out TriEdge2 edge))
            {
                SelectedEdge = edge;
                //Must rebuild renderer.
                BuildSelectionRenderer();
            }
        }

        /// <summary>
        /// Try to select a face in triagulation.
        /// </summary>
        /// <param name="point">The left click point</param>
        private void SelectFace(Point2d point)
        {
            UnselectAll();

            if (triangulation.LocateFace(point, out TriFace2 face))
            {
                if(triangulation.GetTriangle(face.Index, out Triangle2d tri))
                {
                    SelectedFace = face;
                    SelectedTriangle = tri;
                    //Must rebuild renderer.
                    BuildSelectionRenderer();
                }
                
            }
        }

        /// <summary>
        /// Unselects any elements that have been clicked on.
        /// </summary>
        private void UnselectAll()
        {
            SelectedEdge = null;
            SelectedVertex = null;
            SelectedFace = null;

            Renderers.Remove("Point");
            Renderers.Remove("Edge");
            Renderers.Remove("Face");
        }

        /// <summary>
        /// Builds the triangulation renderer
        /// </summary>
        private void BuildTriangulationRenderer()
        {
            Renderers["Triangulation"] = Draw().
                Faces(triangulation, faceColor).
                Outline(triangulation, lineColor).
                Points(triangulation, lineColor, pointColor).
                PopRenderer();

            if(triangulation is ConstrainedTriangulation2<EEK> tri)
            {
                int count = tri.ConstrainedEdgeCount;
                if (count == 0) return;

                var segments = new Segment2d[count];
                tri.GetConstraints(segments, segments.Length);

                Renderers["Segment"] = Draw().
                    Outline(segments, redColor).
                    PopRenderer();
            }
        }

        /// <summary>
        /// Builds the renderer for any selected elements
        /// </summary>
        private void BuildSelectionRenderer()
        {
            if(SelectedVertex != null)
            {
                var point = SelectedVertex.Value.Point;
                Renderers["Point"] = Draw().Points(point, lineColor, redColor).PopRenderer();
            }

            if (SelectedEdge != null)
            {
                var seg = SelectedEdge.Value.Segment;
                Renderers["Edge"] = Draw().Outline(seg, redColor).PopRenderer();
            }

            if (SelectedFace != null)
            {
                var tri = SelectedTriangle;
                Renderers["Face"] = Draw().Faces(tri, redColor).PopRenderer();
            }
        }

        /// <summary>
        /// Builds the Circumcircirles renderers. 
        /// The Circumcircirles are the circles that 
        /// pass through the triangle vertices and
        /// are important in delaunay triangulation.
        /// </summary>
        private void BuildCircumcircirles()
        {
            int count = triangulation.TriangleCount;
            var triangles = new Triangle2d[count];
            triangulation.GetTriangles(triangles, triangles.Length);

            var circles = new Circle2d[count];
            for (int i = 0; i < count; i++)
                circles[i] = triangles[i].CircumCircle;

            Renderers["Circles"] = Draw().
                Circles(circles, redColor, redColor, false, 64).
                PopRenderer();
        }

        /// <summary>
        /// Clear everything when space bar pressed.
        /// </summary>
        protected override void OnCleared()
        {
            UnselectAll();
            CreateTriangulation(Type);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ClickMode = CGALEnum.Next(ClickMode);
            }
            else if (Input.GetKeyDown(KeyCode.F1))
            {
                Type = CGALEnum.Next(Type);
                OnCleared();
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                if (Renderers.ContainsKey("Circles"))
                    Renderers.Remove("Circles");
                else
                    BuildCircumcircirles();
            
            }
            else if(Input.GetKeyDown(KeyCode.Delete))
            {
                if (SelectedVertex != null)
                {
                    triangulation.RemoveVertex(SelectedVertex.Value.Index);

                    SelectedVertex = null;
                    Renderers.Remove("Point");
                    BuildTriangulationRenderer();
                }  
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                if(SelectedEdge != null)
                {
                    var e = SelectedEdge.Value;
                    triangulation.FlipEdge(e.FaceIndex, e.NeighbourIndex);

                    SelectedEdge = null;
                    Renderers.Remove("Edge");
                    BuildTriangulationRenderer();

                }
            }

        }

        private void OnPostRender()
        {
            DrawGrid();

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

        }

        protected void OnGUI()
        {
            int textLen = 1000;
            int textHeight = 25;
            GUI.color = Color.black;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "F1 to change triangulation type.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Current triangulation type = " + Type);
            GUI.Label(new Rect(10, 70, textLen, textHeight), "F2 to toggle circumcircles.");
            GUI.Label(new Rect(10, 90, textLen, textHeight), "Tab to change mode.");
            GUI.Label(new Rect(10, 110, textLen, textHeight), "Current mode = " + ClickMode);

            if(SelectedVertex != null)
            {
                GUI.Label(new Rect(10, 130, textLen, textHeight), "Selected Vertex = " + SelectedVertex.Value);
                GUI.Label(new Rect(10, 150, textLen, textHeight), "Press delete to remove selected vertex.");
            }
            else if (SelectedEdge != null)
            {
                GUI.Label(new Rect(10, 130, textLen, textHeight), "Selected Edge = " + SelectedEdge.Value);
                GUI.Label(new Rect(10, 150, textLen, textHeight), "Press F3 to flip selected edge.");
                GUI.Label(new Rect(10, 170, textLen, textHeight), "Warning - May result in invalid triangulation.");
            }
            else if (SelectedFace != null)
            {
                GUI.Label(new Rect(10, 190, textLen, textHeight), "Selected Face = " + SelectedFace.Value);
            }

        }



    }
}
