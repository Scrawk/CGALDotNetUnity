using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;
using CGALDotNet.Circular;
using CGALDotNet.Triangulations;
using CGALDotNet.Arrangements;

namespace CGALDotNetUnity
{

    public enum INPUT_MODE
    {
        NONE,
        POINT,
        POINT_CLICK,
        SEGMENT,
        POLYGON
    }

    public abstract class InputBehaviour : MonoBehaviour
    {

        private const int POINT_SEGMENTS = 16;

        protected float SnapPoint { get; set; }

        protected float SnapDist { get; set; }

        protected float PointSize { get; set; }

        protected bool EnablePointOutline { get; set; }

        protected INPUT_MODE Mode { get; private set; }

        private GridRenderer Grid { get; set; }

        public List<Point2d> InputPoints { get; set; }

        private List<Point2d> SnapTargets { get; set; }

        private Point2d? PreviousPoint { get; set; }

        private CompositeRenderer InputRenderer;

        protected virtual void Start()
        {
            SnapPoint = 0;
            SnapDist = 0.5f;
            PointSize = 0.2f;

            InputPoints = new List<Point2d>();

            Color gridAxixColor = new Color32(20, 20, 20, 255);
            Color gridLineColor = new Color32(180, 180, 180, 255);

            CreateGrid();
            SetGridColor(gridLineColor, gridAxixColor);
        }

        protected void SetInputMode(INPUT_MODE mode)
        {
            Mode = mode;
        }

        protected void SetGridColor(Color lineColor, Color axisColor)
        {
            Grid.LineColor = lineColor;
            Grid.AxisColor = axisColor;
        }

        protected virtual void OnInputComplete(List<Point2d> points)
        {
            
        }

        protected virtual void OnCleared()
        {
           
        }

        protected virtual void OnLeftClickDown(Point2d point)
        {
            
        }

        protected virtual void OnLeftClickUp(Point2d point)
        {
           
        }

        protected virtual void OnLeftDrag(Point2d point, Point2d delta)
        {
           
        }

        protected virtual void Update()
        {
            bool leftMouseDown = Input.GetMouseButtonDown(0);
            bool leftMouseUp = Input.GetMouseButtonUp(0);
            bool leftMouseDragged = Input.GetMouseButton(0);

            Point2d point = GetMousePosition();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnCleared();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                InputPoints.Clear();
            }
            else
            {
                if (leftMouseUp)
                {
                    OnLeftClickUp(point);
                }
                else
                {
                    switch (Mode)
                    {
                        case INPUT_MODE.NONE:
                            break;

                        case INPUT_MODE.POINT:
                            PointInputMode(point, leftMouseDown);
                            break;

                        case INPUT_MODE.POINT_CLICK:
                            PointClickInputMode(point, leftMouseDown, leftMouseDragged);
                            break;

                        case INPUT_MODE.SEGMENT:
                            SegmentInputMode(point, leftMouseDown);
                            break;

                        case INPUT_MODE.POLYGON:
                            PolygonInputMode(point, leftMouseDown);
                            break;
                    }
                }
            }

            PreviousPoint = point;
        }

        private void PointInputMode(Point2d point, bool leftMouseClicked)
        {
            point = SnapToTargets(point);

            if (leftMouseClicked)
            {
                AddPoint(point);
            }
        }

        private void PointClickInputMode(Point2d point, bool leftMouseClicked, bool leftMouseDragged)
        {
            point = SnapToTargets(point);

            if (leftMouseClicked)
            {
                OnLeftClickDown(point);
            }
            else if(leftMouseDragged && PreviousPoint != null)
            {
                var delta = point - PreviousPoint.Value;
                OnLeftDrag(point, delta);
            }
        }

        private void SegmentInputMode(Point2d point, bool leftMouseClicked)
        {
            point = SnapToTargets(point);

            if (leftMouseClicked)
            {
                if (InputPoints.Count == 0)
                {
                    AddPoint(point);
                    AddPoint(point);
                }

                if (InputPoints.Count == 2 && LastPointDistance() > SnapDist)
                {
                    OnInputComplete(InputPoints);
                }
            }
            else
            {
                MoveLastPoint(point);
            }
        }

        private void PolygonInputMode(Point2d point, bool leftMouseClicked)
        {
            point = SnapToPolygon(point);
            point = SnapToTargets(point);

            if (leftMouseClicked)
            {
                if (InputPoints.Count == 0)
                {
                    AddPoint(point);
                    AddPoint(point);
                }
                else
                {
                    if (PolygonClosed())
                    {
                        ClosePolygon();
                        OnInputComplete(InputPoints);
                    }
                    else
                    {
                        AddPoint(point);
                    }
                }
            }
            else
            {
                MoveLastPoint(point);
            }
        }

        protected void DrawGrid(bool drawAxis = false)
        {
            Grid.DrawAxis = drawAxis;
            Grid.Draw();
        }

        protected void DrawInput(Color lineColor, Color pointColor, float pointSize)
        {
            if (InputPoints == null || InputPoints.Count == 0)
                return;

            InputRenderer = Draw().
                Outline(InputPoints, lineColor).
                Points(InputPoints, lineColor, pointColor, PointSize).
                PopRenderer();

            InputRenderer.Draw();
        }

        protected void AddSnapTarget(Point2d point)
        {
            SnapTargets.Add(point);
        }

        protected void AddSnapTargets(IList<Point2d> points)
        {
            if (SnapTargets == null)
                SnapTargets = new List<Point2d>();

            SnapTargets.AddRange(points);
        }

        protected void ClearSnapTargets()
        {
            SnapTargets.Clear();
        }

        private void CreateGrid()
        {
            Grid = new GridRenderer();
            Grid.DrawAxis = false;
            Grid.Range = 100;
            Grid.PointSize = 0.1f;
            Grid.Create();
        }

        protected RendererBuilder Draw()
        {
            return RendererBuilder.Instance;
        }

        private Point2d GetMousePosition()
        {
            Vector3 p = Input.mousePosition;

            Camera cam = GetComponent<Camera>();
            p = cam.ScreenToWorldPoint(p);

            if (SnapPoint > 0.0f)
            {
                p.x = Mathf.Round(p.x * SnapPoint) / SnapPoint;
                p.y = Mathf.Round(p.y * SnapPoint) / SnapPoint;
            }

            return new Point2d(p.x, p.y);
        }

        private double LastPointDistance()
        {
            if (InputPoints.Count < 2)
                return 0;

            int i = InputPoints.Count - 2;
            int j = InputPoints.Count - 1;
            return Point2d.Distance(InputPoints[i], InputPoints[j]);
        }

        private void AddPoint(Point2d point)
        {
            InputPoints.Add(point);
        }

        private void MoveLastPoint(Point2d point)
        {
            int count = InputPoints.Count;
            if (count == 0) return;

            InputPoints[count - 1] = point;
        }

        private Point2d SnapToPolygon(Point2d point)
        {
            int count = InputPoints.Count;
            if (count < 4) return point;

            var p = InputPoints[0] - point;

            if (p.Magnitude <= SnapDist)
                point = InputPoints[0];

            return point;
        }

        private Point2d SnapToTargets(Point2d point)
        {
            if (SnapTargets == null)
                return point;

            foreach(var p in SnapTargets)
            {
                var q = p - point;

                if (q.Magnitude <= SnapDist)
                {
                    point = p;
                    return point;
                }
            }

            return point;
        }

        private bool PolygonClosed()
        {
            int count = InputPoints.Count;
            if (count < 3) return false;

            var x = InputPoints[0].x - InputPoints[count - 1].x;
            var y = InputPoints[0].y - InputPoints[count - 1].y;

            return Math.Sqrt(x * x + y * y) <= SnapDist;
        }

        private void ClosePolygon()
        {
            int count = InputPoints.Count;
            if (count < 3) return;

            InputPoints.RemoveAt(count - 1);
        }

    }
}
