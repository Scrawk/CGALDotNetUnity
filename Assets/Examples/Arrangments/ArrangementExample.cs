using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Arrangements;

namespace CGALDotNetUnity.Arrangements
{

    public class ArrangementExample : InputBehaviour
    {

        public enum CLICK_MODE
        {
            CLICK_TO_ADD_POINT,
            CLICK_TO_ADD_EDGE,
            CLICK_TO_ADD_POLYGON,
            CLICK_TO_SELECT_VERTEX,
            CLICK_TO_SELECT_EDGE,
            CLICK_TO_SELECT_FACE
        }

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color redColor = new Color32(200, 80, 80, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        private Arrangement2<EEK> arrangement;

        private CLICK_MODE ClickMode = CLICK_MODE.CLICK_TO_ADD_POINT;

        private ArrFace2? SelectedFace;

        private ArrHalfedge2? SelectedEdge;

        private ArrVertex2? SelectedVertex;

        protected override void Start()
        {
            //Init base layer and enable snapping
            base.Start();
            SetInputMode(INPUT_MODE.POINT_CLICK);
            //Snap to each 0.5 unit on grid.
            SnapPoint = 0.5f;

            Renderers = new Dictionary<string, CompositeRenderer>();

            CreateArrangement();
        }

        /// <summary>
        /// Create the initial square arranggment.
        /// </summary>
        private void CreateArrangement()
        {
            arrangement = new Arrangement2<EEK>();
            var box = PolygonFactory<EEK>.CreateBox(-5, 5);

            arrangement.InsertPolygon(box, true);

            Renderers.Clear();
            BuildArrangementRenderer();
        }

        /// <summary>
        /// Add a segment or polygon dependinmg on input mode.
        /// </summary>
        /// <param name="points"></param>
        protected override void OnInputComplete(List<Point2d> points)
        {
            if(Mode == INPUT_MODE.SEGMENT)
            {
                var a = InputPoints[0];
                var b = InputPoints[1];

                AddEdge(a, b);
                InputPoints.Clear();
            }
            else if(Mode == INPUT_MODE.POLYGON)
            {
                AddPolygon(points);
                InputPoints.Clear();
            }
        }

        /// <summary>
        /// When left click down add point or select a object depending on mode.
        /// </summary>
        /// <param name="point"></param>
        protected override void OnLeftClickDown(Point2d point)
        {
            switch (ClickMode)
            {
                case CLICK_MODE.CLICK_TO_ADD_POINT:
                    AddPoint(point);
                    break;

                case CLICK_MODE.CLICK_TO_SELECT_FACE:
                    SelectFace(point);
                    break;

                case CLICK_MODE.CLICK_TO_SELECT_EDGE:
                    SelectEdge(point);
                    break;

                case CLICK_MODE.CLICK_TO_SELECT_VERTEX:
                    SelectVertex(point);
                    break;
            }

        }

        /// <summary>
        /// Add a single point.
        /// </summary>
        /// <param name="point"></param>
        private void AddPoint(Point2d point)
        {
            UnselectAll();

            //Insert into arrangment
            arrangement.InsertPoint(point);

            BuildArrangementRenderer();
            AddSnapTargets();
        }

        /// <summary>
        /// Add a edge.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void AddEdge(Point2d a, Point2d b)
        {
            UnselectAll();

            //Insert into arrangment
            //Always check for other intersections 
            arrangement.InsertSegment(a, b, false);

            //Build renderer and add snap targets
            BuildArrangementRenderer();
            AddSnapTargets();
        }

        /// <summary>
        /// Add a poygon.
        /// </summary>
        /// <param name="points"></param>
        private void AddPolygon(List<Point2d> points)
        {
            var polygon = new Polygon2<EEK>(points.ToArray());

            //Insert into arrangment
            //Always check for other intersections
             arrangement.InsertPolygon(polygon, false);

            //Build renderer and add snap targets
            BuildArrangementRenderer();
            AddSnapTargets();
        }

        /// <summary>
        /// All the points in arrangment should be snap targets.
        /// </summary>
        private void AddSnapTargets()
        {
            var points = new Point2d[arrangement.VertexCount];
            arrangement.GetPoints(points, points.Length);

            ClearSnapTargets();
            AddSnapTargets(points);
        }

        /// <summary>
        /// Try and select a face.
        /// </summary>
        /// <param name="point"></param>
        private void SelectFace(Point2d point)
        {
            UnselectAll();

            if (arrangement.LocateFace(point, out ArrFace2 face))
            {
                if (face.Index != -1)
                {
                    SelectedFace = face;
                    BuildSelectionRenderer();
                }
                else
                    Debug.Log(face);
            }
        }

        /// <summary>
        /// Try and select a edge.
        /// </summary>
        /// <param name="point"></param>
        private void SelectEdge(Point2d point)
        {
            UnselectAll();

            if (arrangement.LocateEdge(point, 0.5, out ArrHalfedge2 edge))
            {
                SelectedEdge = edge;
                BuildSelectionRenderer();
            }
        }

        /// <summary>
        /// Try and select a vertex.
        /// </summary>
        /// <param name="point"></param>
        private void SelectVertex(Point2d point)
        {
            UnselectAll();

            if (arrangement.LocateVertex(point, 0.5, out ArrVertex2 vert))
            {
                SelectedVertex = vert;
                BuildSelectionRenderer();
            }
        }

        /// <summary>
        /// Unselect all objects.
        /// </summary>
        private void UnselectAll()
        {
            SelectedFace = null;
            SelectedEdge = null;
            SelectedVertex = null;
            Renderers.Remove("Vertex");
            Renderers.Remove("Edge");
        }

        /// <summary>
        /// Build the arrangmentss renderer.
        /// </summary>
        private void BuildArrangementRenderer()
        {
            Renderers["Arrangement"] = Draw().
                //Faces(arrangement, faceColor).
                Outline(arrangement, lineColor).
                Points(arrangement, lineColor, pointColor).
                PopRenderer();
        }

        /// <summary>
        /// Build the renderer for any selected objects
        /// </summary>
        private void BuildSelectionRenderer()
        {
            if (SelectedVertex != null)
            {
                var point = SelectedVertex.Value.Point;
                Renderers["Vertex"] = Draw().Points(point, lineColor, redColor).PopRenderer();
            }

            if (SelectedEdge != null)
            {
                ArrVertex2 v1, v2;
                arrangement.GetVertex(SelectedEdge.Value.SourceIndex, out v1);
                arrangement.GetVertex(SelectedEdge.Value.TargetIndex, out v2);

                var seg = new Segment2d(v1.Point, v2.Point);
                Renderers["Edge"] = Draw().Outline(seg, redColor).PopRenderer();
            }
        }

        /// <summary>
        /// Clear and reset everthing.
        /// </summary>
        protected override void OnCleared()
        {
            UnselectAll();
            Renderers.Clear();
            CreateArrangement();
            ClearSnapTargets();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ClickMode = CGALEnum.Next(ClickMode);

                if (ClickMode == CLICK_MODE.CLICK_TO_ADD_EDGE)
                    SetInputMode(INPUT_MODE.SEGMENT);
                else if (ClickMode == CLICK_MODE.CLICK_TO_ADD_POLYGON)
                    SetInputMode(INPUT_MODE.POLYGON);
                else
                    SetInputMode(INPUT_MODE.POINT_CLICK);

            }

        }

        private void OnPostRender()
        {
            DrawGrid();
            DrawInput(lineColor, pointColor, PointSize);

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

        }

        protected void OnGUI()
        {
            int textLen = 1000;
            int textHeight = 25;
            GUI.color = Color.black;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Space to clear.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "Tab to change mode.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Current mode = " + ClickMode);

            if (SelectedVertex != null)
            {
                GUI.Label(new Rect(10, 70, textLen, textHeight), "Selected Vertex = " + SelectedVertex.Value);
            }
            else if (SelectedEdge != null)
            {
                var edge = SelectedEdge.Value;
                GUI.Label(new Rect(10, 70, textLen, textHeight), "Selected Edge = " + edge);

                ArrHalfedge2 twin;
                arrangement.GetHalfEdge(edge.TwinIndex, out twin);
                GUI.Label(new Rect(10, 90, textLen, textHeight), "Selected Twin = " + twin);
            }
            else if (SelectedFace != null)
            {
                GUI.Label(new Rect(10, 70, textLen, textHeight), "Selected Face = " + SelectedFace.Value);
            }
        }

    }
}
