using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;
using CGALDotNet.Triangulations;

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

        protected void DrawGridAxis(bool draw)
        {
            Grid.DrawAxis = draw;
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

        protected void DrawGrid()
        {
            Grid.Draw();
        }

        protected void AddSnapTargets(IList<Point2d> points)
        {
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

        protected static CompositeRenderer CreatePolygon(Polygon2<EEK> polygon, Color pointColor, Color faceColor, Color lineColor, float pointSize)
        {
            var faceIndices = Triangulate(polygon);
            var lineIndices = BaseRenderer.PolygonIndices(polygon.Count);
            var points = polygon.ToArray();

            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = faceColor;
            triangles.Load(ToVector2(points), faceIndices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = lineColor;
            lines.Load(ToVector2(points), lineIndices);

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointColor;
            pointBody.Fill = true;
            pointBody.DefaultRadius = pointSize * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Enabled = false;
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = pointColor;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = pointSize * 0.5f;
            pointOutline.Load(ToVector2(points));

            var comp = new CompositeRenderer();
            comp.Add(triangles);
            comp.Add(lines);
            comp.Add(pointBody);
            comp.Add(pointOutline);

            return comp;
        }

        protected static CompositeRenderer CreatePoints(Point2d[] points, int[] lineIndices, Color lineColor, Color pointColor, float size)
        {
            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = lineColor;
            lines.Load(ToVector2(points), lineIndices);

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointColor;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Enabled = true;
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = pointColor;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            var comp = new CompositeRenderer();
            comp.Add(lines);
            comp.Add(pointBody);
            comp.Add(pointOutline);

            return comp;
        }

        protected static CompositeRenderer CreatePoints(Circle2d[] points, Color pointColor, float size)
        {
            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointColor;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Enabled = true;
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = pointColor;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            var comp = new CompositeRenderer();
            comp.Add(pointBody);
            comp.Add(pointOutline);

            return comp;
        }

        protected static CompositeRenderer CreatePoints(Point2d[] points, Color pointColor, Color outlineColor, float size)
        {
            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointColor;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Enabled = true;
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = outlineColor;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            var comp = new CompositeRenderer();
            comp.Add(pointBody);
            comp.Add(pointOutline);

            return comp;
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

        private static Vector2[] ToVector2<K>(Polygon2<K> poylgon, int count)
            where K : CGALKernel, new()
        {
            var array = new Vector2[count];

            for(int i = 0; i < count; i++)
            {
                var p = poylgon[i];
                array[i] = new Vector2((float)p.x, (float)p.y);
            }

            return array;
        }

        private static Vector2[] ToVector2(IList<Segment2d> segments)
        {
            var array = new Vector2[segments.Count * 2];

            for (int i = 0; i < segments.Count; i++)
            {
                var a = segments[i].A;
                var b = segments[i].B;
                array[i * 2 + 0] = new Vector2((float)a.x, (float)a.y);
                array[i * 2 + 1] = new Vector2((float)b.x, (float)b.y);
            }

            return array;
        }

        private static Vector2[] ToVector2(Point2d[] points)
        {
            return Array.ConvertAll(points, p => new Vector2((float)p.x, (float)p.y));
        }

        private static Vector2[] ToVector2(Circle2d[] circles)
        {
            return Array.ConvertAll(circles, c => new Vector2((float)c.Center.x, (float)c.Center.y));
        }

        private static float[] ToFloat(Circle2d[] circles)
        {
            return Array.ConvertAll(circles, c => (float)c.Radius);
        }

        private static List<Vector2> ToVector2(List<Point2d> points)
        {
            return points.ConvertAll(p => new Vector2((float)p.x, (float)p.y));
        }

        private static List<int> Triangulate<K>(Polygon2<K> polygon)
            where K : CGALKernel, new()
        {
            var tri = new ConstrainedTriangulation2<K>(polygon);
            var indices = new List<int>();
            tri.GetPolygonIndices(polygon, indices);
            return indices;
        }

        private static void Triangulate<K>(PolygonWithHoles2<K> polygon, out Point2d[] points, out List<int> indices)
            where K : CGALKernel, new()
        {
            var tri = new ConstrainedTriangulation2<K>(polygon);

            int count = tri.VertexCount;
            points = new Point2d[count];
            tri.GetPoints(points);

            indices = new List<int>();
            tri.GetPolygonIndices(polygon, indices);

        }

    }
}
