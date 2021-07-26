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

        private const int POINT_SEGMENTS = 16;

        private const string POINT_OUTLINE_NAME = "pointOutline";

        protected float SnapPoint { get; set; }

        protected float SnapDist { get; set; }

        protected float PointSize { get; private set; }

        protected bool PointOutlineEnabled { get; private set; }

        protected INPUT_MODE Mode { get; private set; }

        protected Color LineColor { get; private set; }

        protected Color PointColor { get; private set; }

        protected Color PointOutlineColor { get; private set; }

        private List<Point2d> InputPoints { get; set; }

        private List<CompositeRenderer> ShapeRenderers { get; set; }

        private List<CompositeRenderer> InputRenderers { get; set; }

        private List<CompositeRenderer> PointRenderer { get; set; }

        private GridRenderer Grid { get; set; }

        protected virtual void Start()
        {
            SnapPoint = 0;
            SnapDist = 0.5f;
            PointSize = 0.2f;
            PointOutlineEnabled = false;

            InputPoints = new List<Point2d>();
            ShapeRenderers = new List<CompositeRenderer>();
            InputRenderers = new List<CompositeRenderer>();
            PointRenderer = new List<CompositeRenderer>();

            CreateInputRenderer();
            CreatePointRenderer();
            CreateGrid();

            LineColor = new Color32(20, 20, 20, 255);
            PointColor = new Color32(20, 20, 20, 255);
            PointOutlineColor = new Color32(20, 20, 20, 255);
            Color gridAxixColor = new Color32(20, 20, 20, 255);
            Color gridLineColor = new Color32(180, 180, 180, 255);

            SetGridColor(gridLineColor, gridAxixColor);
            SetInputColor(LineColor, PointColor);
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

        protected void SetInputColor(Color lineColor, Color pointColor)
        {
            LineColor = lineColor;
            PointColor = pointColor;
            PointOutlineColor = pointColor;

            SetLineColor(InputRenderers, lineColor);
            SetPointColor(InputRenderers, pointColor);
            SetPointColor(PointRenderer, pointColor);
        }

        protected void SetPointSize(float size)
        {
            PointSize = size;
            SetPointSize(InputRenderers, size);
            SetPointSize(ShapeRenderers, size);
            SetPointSize(PointRenderer, size);
        }

        protected void EnableInputPointOutline(bool enabled, Color color)
        {
            PointOutlineEnabled = enabled;
            PointOutlineColor = color;
            SetPointOutlineEnabled(InputRenderers, enabled);
            SetPointOutlineEnabled(PointRenderer, enabled);
            SetPointOutlineColor(InputRenderers, color);
            SetPointOutlineColor(PointRenderer, color);
        }

        protected void EnableShapePointOutline(bool enabled, Color color)
        {
            SetPointOutlineEnabled(ShapeRenderers, enabled);
            SetPointOutlineColor(ShapeRenderers, color);
        }

        protected void EnableShapePointOutline(string name, bool enabled, Color color)
        {
            SetPointOutlineEnabled(name, ShapeRenderers, enabled);
            SetPointOutlineColor(name, ShapeRenderers, color);
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

        protected void AddPolygon<K>(string name, PolygonWithHoles2<K> polygon, Color lineColor, Color pointColor, Color faceColor)
            where K : CGALKernel, new()
        {
            if (polygon == null) return;

            Point2d[] points;
            List<int> indices;
            Triangulate(polygon, out points, out indices);

            AddShapeFacesAndPoints(name, points, indices.ToArray(), pointColor, faceColor);

            foreach (var poly in polygon.ToList())
            {
                var lineIndices = BaseRenderer.PolygonIndices(poly.Count);
                points = poly.ToArray();
                AddLines(name, points, lineIndices, lineColor, LINE_MODE.LINES);
            }
                
        }

        protected void AddPolygon<K>(string name, Polygon2<K> polygon, Color lineColor, Color pointColor, Color faceColor)
            where K : CGALKernel, new()
        {
            if (polygon == null) return;

            var faceIndices = Triangulate(polygon);
            var lineIndices = BaseRenderer.PolygonIndices(polygon.Count);
            var points = polygon.ToArray();

            AddShape(name, points, faceIndices.ToArray(), lineIndices, lineColor, pointColor, faceColor, LINE_MODE.LINES);
        }

        protected void AddTriangulation(string name, BaseTriangulation2 triangulation, Color lineColor, Color pointColor, Color faceColor)
        {
            if (triangulation == null) return;

            var points = new Point2d[triangulation.VertexCount];
            var indices = new int[triangulation.IndiceCount];

            triangulation.GetPoints(points);
            triangulation.GetIndices(indices);

            AddShape(name, points, indices, indices, lineColor, pointColor, faceColor, LINE_MODE.TRIANGLES);
        }

        protected void AddTriangulationFaces(string name, BaseTriangulation2 triangulation, Color lineColor, Color pointColor, Color faceColor)
        {
            if (triangulation == null) return;

            var points = new Point2d[triangulation.VertexCount];
            var indices = new int[triangulation.IndiceCount];

            triangulation.GetPoints(points);
            triangulation.GetIndices(indices);

            AddShapeFacesAndLines(name, points, indices, indices, lineColor, faceColor, LINE_MODE.TRIANGLES);
        }

        protected void AddTriangulationPoints(string name, BaseTriangulation2 triangulation, Color lineColor, Color pointColor, Color faceColor)
        {
            if (triangulation == null) return;

            var points = new Point2d[triangulation.VertexCount];
            triangulation.GetPoints(points);

            AddPoints(name, points, PointSize, pointColor);
        }

        protected void AddSegments(string name, Segment2d[] segments, Color lineColor)
        {
            if (segments == null) return;

            var points = ToVector2(segments);

            AddLines(name, points, null, lineColor, LINE_MODE.LINES);
        }

        protected void AddShape(string name, Point2d[] points, int[] faceIndices, int[] lineIndices, Color lineColor, Color pointColor, Color faceColor, LINE_MODE lineMode)
        {
            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = faceColor;
            triangles.Load(ToVector2(points), faceIndices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            var lines = new SegmentRenderer();
            lines.LineMode = lineMode;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = lineColor;
            lines.Load(ToVector2(points), lineIndices);

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointColor;
            pointBody.Fill = true;
            pointBody.DefaultRadius = PointSize * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Name = POINT_OUTLINE_NAME;
            pointOutline.Enabled = false;
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = pointColor;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = PointSize * 0.5f;
            pointOutline.Load(ToVector2(points));

            var comp = new CompositeRenderer();
            comp.Name = name;
            comp.Add(triangles);
            comp.Add(lines);
            comp.Add(pointBody);
            comp.Add(pointOutline);

            ShapeRenderers.Add(comp);
        }

        protected void AddShapeFacesAndLines(string name, Point2d[] points, int[] faceIndices, int[] lineIndices, Color lineColor, Color faceColor, LINE_MODE lineMode)
        {
            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = faceColor;
            triangles.Load(ToVector2(points), faceIndices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            var lines = new SegmentRenderer();
            lines.LineMode = lineMode;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = lineColor;
            lines.Load(ToVector2(points), lineIndices);

            var comp = new CompositeRenderer();
            comp.Name = name;
            comp.Add(triangles);
            comp.Add(lines);

            ShapeRenderers.Add(comp);
        }

        protected void AddShapeFacesAndPoints(string name, Point2d[] points, int[] faceIndices, Color pointColor, Color faceColor)
        {
            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = faceColor;
            triangles.Load(ToVector2(points), faceIndices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointColor;
            pointBody.Fill = true;
            pointBody.DefaultRadius = PointSize * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Name = POINT_OUTLINE_NAME;
            pointOutline.Enabled = false;
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = pointColor;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = PointSize * 0.5f;
            pointOutline.Load(ToVector2(points));

            var comp = new CompositeRenderer();
            comp.Name = name;
            comp.Add(triangles);
            comp.Add(pointBody);
            comp.Add(pointOutline);

            ShapeRenderers.Add(comp);
        }

        protected void AddLines(string name, Point2d[] points, int[] lineIndices, Color lineColor, LINE_MODE lineMode)
        {
            var lines = new SegmentRenderer();
            lines.LineMode = lineMode;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = lineColor;
            lines.Load(ToVector2(points), lineIndices);

            var comp = new CompositeRenderer();
            comp.Name = name;
            comp.Add(lines);

            ShapeRenderers.Add(comp);
        }

        protected void AddLines(string name, Vector2[] points, int[] lineIndices, Color lineColor, LINE_MODE lineMode)
        {
            var lines = new SegmentRenderer();
            lines.LineMode = lineMode;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = lineColor;
            lines.Load(points, lineIndices);

            var comp = new CompositeRenderer();
            comp.Name = name;
            comp.Add(lines);

            ShapeRenderers.Add(comp);
        }

        protected void AddPoints(string name, Point2d[] points, float size, Color pointColor)
        {
            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointColor;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Name = POINT_OUTLINE_NAME;
            pointOutline.Enabled = false;
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = pointColor;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            var comp = new CompositeRenderer();
            comp.Name = name;
            comp.Add(pointBody);
            comp.Add(pointOutline);

            ShapeRenderers.Add(comp);
        }

        protected void AddPoints(string name, Circle2d[] points, float size, Color pointColor)
        {
            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointColor;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Name = POINT_OUTLINE_NAME;
            pointOutline.Enabled = false;
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = pointColor;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            var comp = new CompositeRenderer();
            comp.Name = name;
            comp.Add(pointBody);
            comp.Add(pointOutline);

            ShapeRenderers.Add(comp);
        }

        protected void AddCircles(string name, Circle2d[] circles, Color color, int segments = 16, bool fill = false)
        {
            var renderer = new CircleRenderer();
            renderer.Orientation = DRAW_ORIENTATION.XY;
            renderer.Segments = segments;
            renderer.DefaultColor = color;
            renderer.Fill = fill;
            renderer.Load(ToVector2(circles), ToFloat(circles));

            var comp = new CompositeRenderer();
            comp.Name = name;
            comp.Add(renderer);

            ShapeRenderers.Add(comp);
        }

        protected void SetPoint(Point2d point)
        {
            var p = new Vector2((float)point.x, (float)point.y);

            SetPoints(PointRenderer, new Vector2[] { p });
            SetPointColor(PointRenderer, PointColor);
            SetPointOutlineEnabled(PointRenderer, PointOutlineEnabled);
            SetPointOutlineColor(PointRenderer, PointOutlineColor);
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
            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultRadius = PointSize * 0.5f;
            pointBody.Fill = true;

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Name = POINT_OUTLINE_NAME;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultRadius = PointSize * 0.5f;
            pointOutline.Fill = false;
            pointOutline.Enabled = false;

            var comp = new CompositeRenderer();
            comp.Add(pointBody);
            comp.Add(pointOutline);

            PointRenderer.Add(comp);
        }

        private void CreateInputRenderer()
        {
            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultRadius = PointSize * 0.5f;
            pointBody.Fill = true;

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Name = POINT_OUTLINE_NAME;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultRadius = PointSize * 0.5f;
            pointOutline.Fill = false;
            pointOutline.Enabled = false;

            var comp = new CompositeRenderer();
            comp.Add(lines);
            comp.Add(pointBody);
            comp.Add(pointOutline);

            InputRenderers.Add(comp);
        }

        private void CreateGrid()
        {
            Grid = new GridRenderer();
            Grid.DrawAxis = false;
            Grid.Range = 100;
            Grid.PointSize = 0.1f;
            Grid.Create();
        }

        private void Clear(List<CompositeRenderer> renderers)
        {
            foreach (var renderer in renderers)
                renderer.ClearRenderers();
        }

        private void Draw(List<CompositeRenderer> renderers)
        {
            foreach (var renderer in renderers)
                renderer.Draw();
        }

        private void SetPoints(List<CompositeRenderer> renderers, IList<Vector2> points)
        {
            Clear(renderers);

            foreach (var renderer in renderers)
                renderer.Load(points);
        }

        private void SetPointSize(List<CompositeRenderer> renderers, float size)
        {
            foreach (var renderer in renderers)
                renderer.SetSize(size);

            foreach (var renderer in renderers)
                renderer.SetRadius(size * 0.5f);
        }

        private void SetPointColor(List<CompositeRenderer> renderers, Color color)
        {
            foreach (var renderer in renderers)
                renderer.SetColor<VertexRenderer>(color);

            foreach (var renderer in renderers)
                renderer.SetColor<CircleRenderer>(color);
        }

        private void SetPointOutlineEnabled(List<CompositeRenderer> renderers, bool enabled)
        {
            foreach (var renderer in renderers)
                renderer.SetEnabled<CircleRenderer>(enabled, POINT_OUTLINE_NAME);
        }

        private void SetPointOutlineColor(List<CompositeRenderer> renderers, Color color)
        {
            foreach (var renderer in renderers)
                renderer.SetColor<CircleRenderer>(color, POINT_OUTLINE_NAME);
        }

        private void SetPointOutlineEnabled(string name, List<CompositeRenderer> renderers, bool enabled)
        {
            foreach (var renderer in renderers)
            {
                if(renderer.Name == name)
                    renderer.SetEnabled<CircleRenderer>(enabled, POINT_OUTLINE_NAME);
            }  
        }

        private void SetPointOutlineColor(string name, List<CompositeRenderer> renderers, Color color)
        {
            foreach (var renderer in renderers)
            {
                if (renderer.Name == name)
                    renderer.SetColor<CircleRenderer>(color, POINT_OUTLINE_NAME);
            }  
        }

        private void SetLineColor(List<CompositeRenderer> renderers, Color color)
        {
            foreach (var renderer in renderers)
                renderer.SetColor<SegmentRenderer>(color);
        }

        private void SetFaceColor(List<CompositeRenderer> renderers, Color color)
        {
            foreach (var renderer in renderers)
                renderer.SetColor<FaceRenderer>(color);
        }

        private void SetLineEnable(List<CompositeRenderer> renderers, bool enabled)
        {
            foreach (var renderer in renderers)
                renderer.SetEnabled<SegmentRenderer>(enabled);
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

        private Vector2[] ToVector2(Segment2d[] segments)
        {
            var array = new Vector2[segments.Length * 2];

            for (int i = 0; i < segments.Length; i++)
            {
                var a = segments[i].A;
                var b = segments[i].B;
                array[i * 2 + 0] = new Vector2((float)a.x, (float)a.y);
                array[i * 2 + 1] = new Vector2((float)b.x, (float)b.y);
            }

            return array;
        }

        private Vector2[] ToVector2(Point2d[] points)
        {
            return Array.ConvertAll(points, p => new Vector2((float)p.x, (float)p.y));
        }

        private Vector2[] ToVector2(Circle2d[] circles)
        {
            return Array.ConvertAll(circles, c => new Vector2((float)c.Center.x, (float)c.Center.y));
        }

        private float[] ToFloat(Circle2d[] circles)
        {
            return Array.ConvertAll(circles, c => (float)c.Radius);
        }

        private List<Vector2> ToVector2(List<Point2d> points)
        {
            return points.ConvertAll(p => new Vector2((float)p.x, (float)p.y));
        }

        private List<int> Triangulate<K>(Polygon2<K> polygon)
            where K : CGALKernel, new()
        {
            var tri = new ConstrainedTriangulation2<K>(polygon);
            var indices = new List<int>();
            tri.GetPolygonIndices(polygon, indices);
            return indices;
        }

        private void Triangulate<K>(PolygonWithHoles2<K> polygon, out Point2d[] points, out List<int> indices)
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
