using System.Collections;
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
using CGALDotNet.Nurbs;
using CGALDotNet.Meshing;

namespace CGALDotNetUnity
{

    public class RendererBuilder
    {
        private const int POINT_SEGMENTS = 16;

        private const float POINT_SIZE = 0.2f;

        public static RendererBuilder Instance = new RendererBuilder();

        private CompositeRenderer m_renderer;

        private CompositeRenderer Renderer
        {
            get
            {
                if (m_renderer == null)
                    m_renderer = new CompositeRenderer();

                return m_renderer;
            }
            set
            {
                m_renderer = value;
            }
        }

        public RendererBuilder Faces(IList<Point2d> points, IList<int> indices, Color color)
        {
            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = color;
            triangles.Load(ToVector2(points), indices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            Renderer.Add(triangles);

            return Instance;
        }

        public RendererBuilder Faces(Polygon2<EEK> polygon, Color color)
        {
            var indices = new List<int>();
            polygon.Triangulate(indices);

            var points = new List<Point2d>();
            polygon.GetPoints(points);

            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = color;
            triangles.Load(ToVector2(points), indices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            Renderer.Add(triangles);

            return Instance;
        }

        public RendererBuilder Faces(PolygonWithHoles2<EEK> polygon, Color color)
        {
            var indices = new List<int>();
            polygon.Triangulate(indices);

            var points = new List<Point2d>();
            polygon.GetAllPoints(points);

            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = color;
            triangles.Load(ToVector2(points), indices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            Renderer.Add(triangles);

            return Instance;
        }

        public RendererBuilder Faces(BaseTriangulation2 tri, Color color)
        {
            var indices = new int[tri.IndiceCount];
            tri.GetIndices(indices, indices.Length);

            var points = new Point2d[tri.VertexCount];
            tri.GetPoints(points, points.Length);

            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = color;
            triangles.Load(ToVector2(points), indices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            Renderer.Add(triangles);

            return Instance;
        }

        public RendererBuilder Faces(ConformingTriangulation2 tri, Color color)
        {
            var indices = new int[tri.IndiceCount];
            tri.GetIndices(indices, indices.Length);

            var points = new Point2d[tri.VertexCount];
            tri.GetPoints(points, points.Length);

            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = color;
            triangles.Load(ToVector2(points), indices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            Renderer.Add(triangles);

            return Instance;
        }

        public RendererBuilder Faces(Triangle2d tri, Color color)
        {
            var points = new Point2d[]
            {
                tri.A, tri.B, tri.C
            };

            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = color;
            triangles.Load(ToVector2(points));
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            Renderer.Add(triangles);

            return Instance;
        }

        public RendererBuilder Faces(IList<Triangle2d> tri, Color color)
        {
            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = color;
            triangles.Load(ToVector2(tri));
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            Renderer.Add(triangles);

            return Instance;
        }

        public RendererBuilder Outline(Polygon2<EEK> polygon, Color color)
        {
            var lineIndices = BaseRenderer.PolygonIndices(polygon.Count);
            var points = polygon.ToArray();

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(points), lineIndices);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(Arrangement2 arr, Color color)
        {
            var edgeCount = arr.EdgeCount;

            var segments = new Segment2d[edgeCount];
            arr.GetSegments(segments, segments.Length);

            var indices = BaseRenderer.SegmentIndices(edgeCount);

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(segments), indices);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder TriangleOutline(Polygon2<EEK> polygon, Color color)
        {
            var indices = new List<int>();
            polygon.Triangulate(indices);

            var points = new List<Point2d>();
            polygon.GetPoints(points);

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.TRIANGLES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(points), indices);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder TriangleOutline(PolygonWithHoles2<EEK> polygon, Color color)
        {
            var indices = new List<int>();
            polygon.Triangulate(indices);

            var points = new List<Point2d>();
            polygon.GetAllPoints(points);

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.TRIANGLES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(points), indices);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(PolygonWithHoles2<EEK> polygon, Color color)
        {
            var count = polygon.PointCount(POLYGON_ELEMENT.BOUNDARY);
            var points = new Point2d[count];
            polygon.GetPoints(POLYGON_ELEMENT.BOUNDARY, points, points.Length);

            var indices = BaseRenderer.PolygonIndices(count);

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(points), indices);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(BaseTriangulation2 tri, Color color)
        {
            var indices = new int[tri.IndiceCount];
            tri.GetIndices(indices, indices.Length);

            var points = new Point2d[tri.VertexCount];
            tri.GetPoints(points, points.Length);

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.TRIANGLES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(points), indices);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(ConformingTriangulation2 tri, Color color)
        {
            var indices = new int[tri.IndiceCount];
            tri.GetIndices(indices, indices.Length);

            var points = new Point2d[tri.VertexCount];
            tri.GetPoints(points, points.Length);

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.TRIANGLES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(points), indices);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(Triangle2d tri, Color color)
        {
            var points = new Point2d[]
            {
                tri.A, tri.B, tri.C
            };

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(points));

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(IList<Point2d> points, Color color)
        {
            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(points));

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(BaseNurbsCurve2d curve, Color color, int samples)
        {
            var points = new List<Point2d>();
            curve.GetCartesianPoints(points, samples);

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(points));

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(IGeometry2d geometry, Color color)
        {

            switch (geometry)
            {
                case Line2d line:
                    return Outline(line, color);

                case Ray2d ray:
                    return Outline(ray, color);

                case Segment2d seg:
                    return Outline(seg ,color);

                case Box2d box:
                    return Outline(box, color);

            }

            return Instance;
        }

        public RendererBuilder Outline(Line2d line, Color lineColor)
        {
            double x1, y1, x2, y2;
            double len = -100;

            if (line.IsHorizontal)
            {
                x1 = -len;
                y1 = line.Y(x1);

                x2 = len;
                y2 = line.Y(x2);
            }
            else
            {
                y1 = -len;
                x1 = line.X(y1);

                y2 = len;
                x2 = line.X(y2);
            }

            var p1 = new Point2d(x1, y1);
            var p2 = new Point2d(x2, y2);

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = lineColor;
            lines.Load(ToVector2(new Point2d[] { p1, p2 }));

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(Ray2d ray, Color color)
        {
            return Outline(new Ray2d[] { ray }, color);
        }

        public RendererBuilder Outline(IList<Ray2d> rays, Color color)
        {
            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(rays), BaseRenderer.SegmentIndices(rays.Count));

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(Box2d box, Color color)
        {
            var points = box.GetCorners();

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(points), BaseRenderer.PolygonIndices(4));

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(Segment2d segment, Color color)
        {
            return Outline(new Segment2d[] { segment }, color);
        }

        public RendererBuilder Outline(IList<Segment2d> segments, Color color)
        {
            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(ToVector2(segments), BaseRenderer.SegmentIndices(segments.Count));

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(IList<Segment3d> segments, Color color)
        {
            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.DefaultColor = color;
            lines.Load(ToVector3(segments), BaseRenderer.SegmentIndices(segments.Count));

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Circles(IList<Circle2d> circles, Color lineCol, Color fillColor, bool filled = false, float size = POINT_SIZE)
        {

            var points = ToVector2(circles);
            var radius = ToRadius(circles);

            if (filled)
            {
                var pointBody = new CircleRenderer();
                pointBody.Orientation = DRAW_ORIENTATION.XY;
                pointBody.Segments = POINT_SEGMENTS;
                pointBody.DefaultColor = fillColor;
                pointBody.Fill = true;
                pointBody.DefaultRadius = size * 0.5f;
                pointBody.Load(points, radius);

                Renderer.Add(pointBody);
            }

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(points, radius);

            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Points(BaseTriangulation2 tri, Color lineCol, Color pointCol, float size = POINT_SIZE)
        {
            var points = new Point2d[tri.VertexCount];
            tri.GetPoints(points, points.Length);

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointCol;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            Renderer.Add(pointBody);
            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Points(ConformingTriangulation2 tri, Color lineCol, Color pointCol, float size = POINT_SIZE)
        {
            var points = new Point2d[tri.VertexCount];
            tri.GetPoints(points, points.Length);

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointCol;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            Renderer.Add(pointBody);
            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Points(Polygon2<EEK> polygon, Color lineCol, Color pointCol, float size = POINT_SIZE)
        {
            var points = polygon.ToArray();

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointCol;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            Renderer.Add(pointBody);
            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Points(PolygonWithHoles2<EEK> polygon, Color lineCol, Color pointCol, float size = POINT_SIZE)
        {
            var count = polygon.PointCount(POLYGON_ELEMENT.BOUNDARY);
            var points = new Point2d[count];
            polygon.GetPoints(POLYGON_ELEMENT.BOUNDARY, points, points.Length);

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointCol;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            Renderer.Add(pointBody);
            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Points(Arrangement2 arr, Color lineCol, Color pointCol, float size = POINT_SIZE)
        {
            var vertCount = arr.VertexCount;
            var points = new Point2d[vertCount];
            arr.GetPoints(points, points.Length);

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointCol;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            Renderer.Add(pointBody);
            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Points(Point2d point, Color lineCol, Color pointCol, float size = POINT_SIZE)
        {
            return Points(new Point2d[] { point }, lineCol, pointCol, size);
        }

        public RendererBuilder Points(IList<Point2d> points, Color lineCol, Color pointCol, float size = POINT_SIZE)
        {
            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointCol;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(ToVector2(points));

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(ToVector2(points));

            Renderer.Add(pointBody);
            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Points(Point3d[,] points, Color pointCol, float size)
        {
            var pointBody = new VertexRenderer(size);
            pointBody.DefaultColor = pointCol;
            pointBody.Load(ToVector3(points));

            Renderer.Add(pointBody);

            return Instance;
        }

        public RendererBuilder Points(Vector3d[,] points, Color pointCol, float size)
        {
            var pointBody = new VertexRenderer(size);
            pointBody.DefaultColor = pointCol;
            pointBody.Load(ToVector3(points));

            Renderer.Add(pointBody);

            return Instance;
        }

        public RendererBuilder Points(IList<Segment2d> segments, Color lineCol, Color pointCol, float size = POINT_SIZE)
        {
            var points = ToVector2(segments);

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointCol;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(points);

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(points);

            Renderer.Add(pointBody);
            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Tangents(BaseNurbsCurve2d curve, Color color, int samples)
        {
            var points = new List<Point2d>();
            curve.GetCartesianPoints(points, samples);

            var tangents = new List<Vector2d>();
            curve.GetTangents(tangents, samples);

            var renderer = new NormalRenderer();
            renderer.Orientation = DRAW_ORIENTATION.XY;
            renderer.DefaultColor = color;
            renderer.Load(ToVector2(points), ToVector2(tangents));

            Renderer.Add(renderer);

            return Instance;
        }

        public RendererBuilder Normals(BaseNurbsCurve2d curve, Color color, int samples)
        {
            var points = new List<Point2d>();
            curve.GetCartesianPoints(points, samples);

            var normals = new List<Vector2d>();
            curve.GetNormals(normals, samples);

            var normal = new NormalRenderer();
            normal.Orientation = DRAW_ORIENTATION.XY;
            normal.DefaultColor = color;
            normal.Load(ToVector2(points), ToVector2(normals));

            Renderer.Add(normal);

            return Instance;
        }

        public CompositeRenderer PopRenderer()
        {
            var renderer = Renderer;
            Renderer = null;
            return renderer;
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

        private static Vector3[] ToVector3(IList<Segment3d> segments)
        {
            var array = new Vector3[segments.Count * 2];

            for (int i = 0; i < segments.Count; i++)
            {
                var a = segments[i].A;
                var b = segments[i].B;
                array[i * 2 + 0] = new Vector3((float)a.x, (float)a.y, (float)a.z);
                array[i * 2 + 1] = new Vector3((float)b.x, (float)b.y, (float)b.z);
            }

            return array;
        }

        private static Vector2[] ToVector2(IList<Ray2d> rays)
        {
            var array = new Vector2[rays.Count * 2];

            for (int i = 0; i < rays.Count; i++)
            {
                var a = rays[i].Position;
                var b = a + (Point2d)rays[i].Direction;
                array[i * 2 + 0] = new Vector2((float)a.x, (float)a.y);
                array[i * 2 + 1] = new Vector2((float)b.x, (float)b.y);
            }

            return array;
        }

        private static Vector2[] ToVector2(IList<Point2d> points)
        {
            var array = new Vector2[points.Count];

            for (int i = 0; i < points.Count; i++)
                array[i] = new Vector2((float)points[i].x, (float)points[i].y);

            return array;
        }

        private static Vector2[] ToVector2(IList<Triangle2d> triangles)
        {
            var array = new Vector2[triangles.Count * 3];

            for (int i = 0; i < triangles.Count; i++)
            {
                var t = triangles[i];

                var a = new Vector2((float)t.A.x, (float)t.A.y);
                var b = new Vector2((float)t.B.x, (float)t.B.y);
                var c = new Vector2((float)t.C.x, (float)t.C.y);

                array[i * 3 + 0] = a;
                array[i * 3 + 1] = b;
                array[i * 3 + 2] = c;
            }
                
            return array;
        }

        private static Vector3[] ToVector3(Point3d[,] points)
        {
            var array = new Vector3[points.Length];

            int width = points.GetLength(0);
            int height = points.GetLength(1);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    array[i + j * width] = new Vector3((float)points[i,j].x, (float)points[i,j].y, (float)points[i, j].z);

            return array;
        }

        private static Vector3[] ToVector3(Vector3d[,] points)
        {
            var array = new Vector3[points.Length];

            int width = points.GetLength(0);
            int height = points.GetLength(1);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    array[i + j * width] = new Vector3((float)points[i, j].x, (float)points[i, j].y, (float)points[i, j].z);

            return array;
        }

        private static Vector2[] ToVector2(IList<Vector2d> points)
        {
            var array = new Vector2[points.Count];

            for (int i = 0; i < points.Count; i++)
                array[i] = new Vector2((float)points[i].x, (float)points[i].y);

            return array;
        }

        private static Vector2 ToVector2(Point2d point)
        {
            return new Vector2((float)point.x, (float)point.y);
        }

        private static Vector2[] ToVector2(IList<Circle2d> points)
        {
            var array = new Vector2[points.Count];

            for (int i = 0; i < points.Count; i++)
                array[i] = new Vector2((float)points[i].Center.x, (float)points[i].Center.y);

            return array;
        }

        private static float[] ToRadius(IList<Circle2d> circles)
        {
            var array = new float[circles.Count];

            for (int i = 0; i < circles.Count; i++)
                array[i] = (float)circles[i].Radius;

            return array;
        }

        private static List<Vector2> ToVector2(List<Segment2d> segments)
        {
            var list = new List<Vector2>();
            foreach (var seg in segments)
            {
                var a = new Vector2((float)seg.A.x, (float)seg.A.y);
                var b = new Vector2((float)seg.B.x, (float)seg.B.y);
                list.Add(a);
                list.Add(b);
            }

            return list;
        }
    }

}
