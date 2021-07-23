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
        POINT,
        POINT_CLICK,
        POLYGON
    }

    public abstract class InputBehaviour : MonoBehaviour
    {

        protected float SnapPoint { get; set; }

        protected float SnapDist { get; set; }

        protected float PointSize { get; private set; }

        protected INPUT_MODE Mode { get; private set; }

        private List<Point2d> InputPoints { get; set; }

        private List<BaseRenderer> ShapeRenderers { get; set; }

        private List<BaseRenderer> InputRenderers { get; set; }

        private List<BaseRenderer> PointRenderer { get; set; }

        private GridRenderer Grid { get; set; }

        protected virtual void Start()
        {
            SnapPoint = 0;
            SnapDist = 0.5f;
            PointSize = 0.2f;

            InputPoints = new List<Point2d>();
            ShapeRenderers = new List<BaseRenderer>();
            InputRenderers = new List<BaseRenderer>();
            PointRenderer = new List<BaseRenderer>();

            CreateInputRenderer();
            CreatePointRenderer();
            CreateGrid();

            Color inputLineColor = new Color32(20, 20, 20, 255);
            Color inputPointColor = new Color32(20, 20, 20, 255);
            Color gridAxixColor = new Color32(20, 20, 20, 255);
            Color gridLineColor = new Color32(180, 180, 180, 255);

            SetGridColor(gridLineColor, gridAxixColor);
            SetInputColor(inputLineColor, inputPointColor);
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

        protected void SetInputColor(Color lineColor, Color vertColor)
        {
            SetLineColor(InputRenderers, lineColor);
            SetPointColor(InputRenderers, vertColor);
        }

        protected void SetInputMode(INPUT_MODE mode)
        {
            ResetInput();
            Clear(InputRenderers);

            Mode = mode;

            if (mode != INPUT_MODE.POLYGON)
                SetLineEnable(InputRenderers, false);
            else
                SetLineEnable(InputRenderers, true);
        }


        protected virtual void OnInputComplete(List<Point2d> points)
        {

        }

        protected virtual void OnCleared()
        {

        }

        protected virtual void OnLeftClick(Point2d point)
        {

        }

        protected virtual void Update()
        {
            bool leftMouseClicked = Input.GetMouseButtonDown(0);
            Point2d point = GetMousePosition();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetInput();
                ClearClickPointRenderers();
                ClearShapeRenderers();
                OnCleared();
            }
            else
            {
                switch (Mode)
                {
                    case INPUT_MODE.POINT:
                        PointInputMode(point, leftMouseClicked);
                        break;

                    case INPUT_MODE.POINT_CLICK:
                        PointClickInputMode(point, leftMouseClicked);
                        break;

                    case INPUT_MODE.POLYGON:
                        PolygonInputMode(point, leftMouseClicked);
                        break;
                }
            }

        }

        private void PointInputMode(Point2d point, bool leftMouseClicked)
        {
            if(leftMouseClicked)
            {
                AddPoint(point);
                OnInputComplete(InputPoints);
                ResetInput();
            }
        }

        private void PointClickInputMode(Point2d point, bool leftMouseClicked)
        {
            if (leftMouseClicked)
            {
                OnLeftClick(point);
            }
        }

        private void PolygonInputMode(Point2d point, bool leftMouseClicked)
        {
            point = SnapToPolygon(point);

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
                        ResetInput();
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

        protected void DrawInput()
        {
            if (InputPoints.Count == 0) return;

            var points = ToVector2(InputPoints);
            SetPoints(InputRenderers, points);

            Draw(InputRenderers);
        }

        protected void DrawShapes()
        {
            foreach (var renderer in ShapeRenderers)
                renderer.Draw();
        }

        protected void DrawPoint()
        {
            Draw(PointRenderer);
        }

        protected void DrawGrid()
        {
            Grid.Draw();
        }

        protected void AddPolygon<K>(PolygonWithHoles2<K> polygon, Color lineColor, Color faceColor)
            where K : CGALKernel, new()
        {
            if (polygon == null) return;

            foreach (var poly in polygon.ToList())
                AddPolygon(poly, lineColor, faceColor);
        }

        protected void AddPolygon<K>(Polygon2<K> polygon, Color lineColor, Color faceColor)
            where K : CGALKernel, new()
        {
            if (polygon == null) return;

            var faceIndices = Triangulate(polygon);
            var lineIndices = PolygonIndices(polygon.Count);
            var points = polygon.ToArray();

            AddShape(points, faceIndices, lineIndices, lineColor, faceColor, LINE_MODE.LINES);
        }

        protected void AddTriangulation<K>(Triangulation2<K> triangulation, Color lineColor, Color faceColor)
            where K : CGALKernel, new()
        {
            if (triangulation == null) return;

            var points = new Point2d[triangulation.VertexCount];
            var indices = new int[triangulation.IndiceCount];

            triangulation.GetPoints(points);
            triangulation.GetIndices(indices);

            AddShape(points, indices, indices, lineColor, faceColor, LINE_MODE.TRIANGLES);
        }

        protected void AddShape(Point2d[] points, int[] faceIndices, int[] lineIndices, Color lineColor, Color faceColor, LINE_MODE lineMode)
        {
            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = faceColor;
            triangles.Load(ToVector2(points), faceIndices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;
            ShapeRenderers.Add(triangles);

            var lines = new SegmentRenderer();
            lines.LineMode = lineMode;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = lineColor;
            lines.Load(ToVector2(points), lineIndices);
            ShapeRenderers.Add(lines);

            var circles = new CircleRenderer();
            circles.Orientation = DRAW_ORIENTATION.XY;
            circles.DefaultColor = lineColor;
            circles.Fill = true;
            circles.DefaultRadius = PointSize * 0.5f;
            circles.Load(ToVector2(points));
            ShapeRenderers.Add(circles);
        }

        protected void SetPoint(Point2d point, Color color)
        {
            var p = new Vector2((float)point.x, (float)point.y);

            SetPoints(PointRenderer, new Vector2[] { p });
            SetPointColor(PointRenderer, color);
        }

        protected void ResetInput()
        {
            InputPoints.Clear();
            Clear(InputRenderers);
        }

        protected void ClearClickPointRenderers()
        {
            Clear(PointRenderer);
        }

        protected void ClearShapeRenderers()
        {
            Clear(ShapeRenderers);
        }

        private void CreatePointRenderer()
        {
            var pointRenderer = new CircleRenderer();
            pointRenderer.Orientation = DRAW_ORIENTATION.XY;
            pointRenderer.Fill = true;
            pointRenderer.DefaultRadius = PointSize * 0.5f;

            PointRenderer.Add(pointRenderer);
        }

        private void CreateInputRenderer()
        {
            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;

            var circles = new CircleRenderer();
            circles.Orientation = DRAW_ORIENTATION.XY;
            circles.DefaultRadius = PointSize * 0.5f;
            circles.Fill = true;

            InputRenderers.Add(lines);
            InputRenderers.Add(circles);
        }

        private void CreateGrid()
        {
            Grid = new GridRenderer();
            Grid.DrawAxis = false;
            Grid.Range = 100;
            Grid.PointSize = 0.1f;
            Grid.Create();
        }

        private void Clear(List<BaseRenderer> renderers)
        {
            foreach (var renderer in renderers)
                renderer.Clear();
        }

        private void Draw(List<BaseRenderer> renderers)
        {
            foreach (var renderer in renderers)
                renderer.Draw();
        }

        private void SetPoints(List<BaseRenderer> renderers, IList<Vector2> points)
        {
            Clear(renderers);

            foreach (var renderer in renderers)
            {
                if (renderer is VertexRenderer vr)
                    vr.Load(points);
                else if (renderer is SegmentRenderer sr)
                    sr.Load(points);
                else if (renderer is CircleRenderer cr)
                    cr.Load(points);
            }
        }

        private void SetPointSize(List<BaseRenderer> renderers, float size)
        {
            foreach (var renderer in renderers)
            {
                if (renderer is VertexRenderer vr)
                    vr.Size = size;
                else if (renderer is CircleRenderer cr)
                    cr.SetRadius(size * 0.5f);
            }
        }

        private void SetPointColor(List<BaseRenderer> renderers, Color color)
        {
            foreach (var renderer in renderers)
            {
                if (renderer is VertexRenderer vr)
                    vr.SetColor(color);
                else if (renderer is CircleRenderer cr)
                    cr.SetColor(color);
            }
        }

        private void SetLineColor(List<BaseRenderer> renderers, Color color)
        {
            foreach (var renderer in renderers)
            {
                if (renderer is SegmentRenderer sr)
                    sr.SetColor(color);
            }
        }

        private void SetFaceColor(List<BaseRenderer> renderers, Color color)
        {
            foreach (var renderer in renderers)
            {
                if (renderer is FaceRenderer fr)
                    fr.SetColor(color);
            }
        }

        private void SetLineEnable(List<BaseRenderer> renderers, bool enabled)
        {
            foreach (var renderer in renderers)
            {
                if (renderer is SegmentRenderer sr)
                    sr.Enabled = enabled;
            }
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

            var x = InputPoints[0].x - point.x;
            var y = InputPoints[0].y - point.y;

            var dist = Math.Sqrt(x * x + y * y);

            if (dist <= SnapDist)
            {
                point.x = InputPoints[0].x;
                point.y = InputPoints[0].y;
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

        private Vector2[] ToVector2<K>(Polygon2<K> poylgon, int count)
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

        private int[] PolygonIndices(int count)
        {
            int[] indices = new int[count * 2];

            for (int i = 0; i < count; i++)
            {
                indices[i * 2 + 0] = i;
                indices[i * 2 + 1] = MathUtil.Wrap(i + 1, count);
            }

            return indices;
        }

        private Vector2[] ToVector2(Point2d[] points)
        {
            return Array.ConvertAll(points, p => new Vector2((float)p.x, (float)p.y));
        }

        private List<Vector2> ToVector2(List<Point2d> points)
        {
            return points.ConvertAll(p => new Vector2((float)p.x, (float)p.y));
        }

        private int[] Triangulate<K>(Polygon2<K> polygon)
            where K : CGALKernel, new()
        {
            var points = new Point2d[polygon.Count];
            polygon.GetPoints(points);

            var tri = new Triangulation2<K>(points);

            var indices = new int[tri.IndiceCount];
            tri.GetIndices(indices);

            return indices;
        }

    }
}
