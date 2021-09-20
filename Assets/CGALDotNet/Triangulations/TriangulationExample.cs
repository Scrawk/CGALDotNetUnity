using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using Common.Unity.Utility;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Circular;
using CGALDotNet.Triangulations;

namespace CGALDotNetUnity.Triangulations
{

    public class TriangulationExample : InputBehaviour
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

        private TRIANGULATION_TYPE Type = TRIANGULATION_TYPE.DELAUNAY;

        private TriVertex2? SelectedVertex;

        private TriFace2? SelectedFace;

        private TriEdge2? SelectedEdge;

        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();
            SetInputMode(INPUT_MODE.POINT_CLICK);

            CreateTriangulation(Type);
        }

        private void CreateTriangulation(TRIANGULATION_TYPE type)
        {
            switch (type)
            {
                case TRIANGULATION_TYPE.REGULAR:
                    triangulation = new Triangulation2<EEK>();
                    break;
                case TRIANGULATION_TYPE.DELAUNAY:
                    triangulation = new DelaunayTriangulation2<EEK>();
                    break;
                case TRIANGULATION_TYPE.CONSTRAINED:
                    triangulation = new ConstrainedTriangulation2<EEK>();
                    break;
            }

            triangulation.InsertPoint(new Point2d(-5, -5));
            triangulation.InsertPoint(new Point2d(5, -5));
            triangulation.InsertPoint(new Point2d(0, 5));

            Renderers.Clear();
            BuildTriangulationRenderer();
        }

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

        private void AddPoint(Point2d point)
        {
            UnselectAll();

            triangulation.InsertPoint(point);
            BuildTriangulationRenderer();
        }

        private void SelectPoint(Point2d point)
        {
            UnselectAll();

            if (triangulation.LocateVertex(point, out TriVertex2 vert))
            {
                if (Point2d.Distance(vert.Point, point) < 0.2)
                {
                    SelectedVertex = vert;
                    Renderers["Point"] = FromPoints(new Point2d[] { vert.Point }, lineColor, redColor, PointSize);
                }
            }
        }

        private void SelectEdge(Point2d point)
        {
            UnselectAll();

            if (triangulation.LocateEdge(point, out TriEdge2 edge, out Segment2d seg))
            {
                if (seg.Distance(point) < 0.2)
                {
                    SelectedEdge = edge;
                    Renderers["Segment"] = FromSegment(seg, redColor,  lineColor, redColor, PointSize);
                }
            }
        }

        private void SelectFace(Point2d point)
        {
            UnselectAll();

            if (triangulation.LocateFace(point, out TriFace2 face))
            {
                if(triangulation.GetTriangle(face.Index, out Triangle2d tri))
                {
                    SelectedFace = face;
                    Renderers["Face"] = FromTriangle(tri, redColor, lineColor, redColor, PointSize);
                }
                
            }
        }

        private void UnselectAll()
        {
            SelectedEdge = null;
            SelectedVertex = null;
            SelectedFace = null;

            Renderers.Remove("Point");
            Renderers.Remove("Edge");
            Renderers.Remove("Face");
        }

        private void BuildTriangulationRenderer()
        {
            Renderers["Triangulation"] = FromTriangulation(triangulation, faceColor, lineColor, pointColor, PointSize);
        }

        private void BuildCircumcircirles()
        {
            int count = triangulation.TriangleCount;
            var triangles = new Triangle2d[count];
            triangulation.GetTriangles(triangles);

            var circles = new Circle2d[count];
            for (int i = 0; i < count; i++)
                circles[i] = triangles[i].CircumCircle();

            Renderers["Circles"] = FromCircles(circles, redColor, lineColor, redColor, PointSize, 32);
        }

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
                    Renderers.Remove("Segment");
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
