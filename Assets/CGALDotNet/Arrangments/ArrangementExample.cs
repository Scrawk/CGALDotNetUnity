using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using Common.Unity.Drawing;
using Common.Unity.Utility;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;
using CGALDotNet.Arrangements;
using CGALDotNet.DCEL;

namespace CGALDotNetUnity.Arrangements
{

    public class ArrangementExample : InputBehaviour
    {

        public enum CLICK_MODE
        {
            CLICK_TO_ADD_POINT,
            CLICK_TO_ADD_EDGE,
            CLICK_TO_ADD_POLYGON,
            CLICK_TO_SELECT_POINT,
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

        private ArrVertex2? SelectedVertex;

        private ArrFace2? SelectedFace;

        private ArrHalfEdge2? SelectedEdge;

        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();
            SetInputMode(INPUT_MODE.POINT_CLICK);

            CreateArrangement();
        }

        private void CreateArrangement()
        {
            //ConsoleRedirect.Redirect();

            arrangement = new Arrangement2<EEK>();
            var box = PolygonFactory<EEK>.FromBox(-5, 5);

            arrangement.InsertPolygon(box, true);

            //var builder = new StringBuilder();
            //arrangement.Print(builder, true);
            //Debug.Log(builder);

            Renderers.Clear();
            BuildArrangementRenderer();
        }

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

            arrangement.InsertPoint(point);
            BuildArrangementRenderer();
        }

        private void AddEdge(Point2d a, Point2d b)
        {
            UnselectAll();

            arrangement.InsertSegment(a, b, false);
            BuildArrangementRenderer();
        }

        private void AddPolygon(List<Point2d> points)
        {
            var polygon = new Polygon2<EEK>(points.ToArray());

            arrangement.InsertPolygon(polygon, false);
            BuildArrangementRenderer();
        }

        private void SelectPoint(Point2d point)
        {
            UnselectAll();

        }

        private void SelectEdge(Point2d point)
        {
            UnselectAll();

        }

        private void SelectFace(Point2d point)
        {
            UnselectAll();

            if (arrangement.LocateFace(point, out ArrFace2 face))
            {
                if(face.Index != -1)
                    SelectedFace = face;
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

        private void BuildArrangementRenderer()
        {
            var mesh = new DCELMesh();
            mesh.FromArrangement(arrangement);

            int index = 0;
            foreach(var face in mesh.EnumerateFaces())
            {
                var points = new List<Point2d>();
                face.GetPoints(points);

                var polygon = new Polygon2<EEK>(points.ToArray());
                if (polygon.IsCounterClockWise)
                    polygon.Reverse();

                Renderers["Face"+index] = FromPolygonTriangulation(polygon, faceColor, lineColor);

                index++;
            }

            Renderers["Arrangement"] = FromArrangement(arrangement, lineColor, pointColor, PointSize);
        }

        protected override void OnCleared()
        {
            UnselectAll();
            CreateArrangement();
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
            else if (Input.GetKeyDown(KeyCode.F1))
            {
          
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
                GUI.Label(new Rect(10, 130, textLen, textHeight), "Selected Vertex = " + SelectedVertex.Value);
                GUI.Label(new Rect(10, 150, textLen, textHeight), "Press delete to remove selected vertex.");
            }
            else if (SelectedEdge != null)
            {
                GUI.Label(new Rect(10, 130, textLen, textHeight), "Selected Edge = " + SelectedEdge.Value);
            }
            else if (SelectedFace != null)
            {
                GUI.Label(new Rect(10, 190, textLen, textHeight), "Selected Face = " + SelectedFace.Value);
            }
            
        }



    }
}
