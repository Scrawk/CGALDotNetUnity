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
using CGALDotNet.Polyhedra;

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
            triangles.Load(points.ToUnityVector2(), indices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            Renderer.Add(triangles);

            return Instance;
        }

        public RendererBuilder Faces(Polygon2 polygon, Color color)
        {
            var indices = new List<int>();
            polygon.Triangulate(indices);

            var points = new List<Point2d>();
            polygon.GetPoints(points);

            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = color;
            triangles.Load(points.ToUnityVector2(), indices);
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            Renderer.Add(triangles);

            return Instance;
        }

        public RendererBuilder Faces(PolygonWithHoles2 polygon, Color color)
        {
            var indices = new List<int>();
            polygon.Triangulate(indices);

            var points = new List<Point2d>();
            polygon.GetAllPoints(points);

            var triangles = new FaceRenderer();
            triangles.FaceMode = FACE_MODE.TRIANGLES;
            triangles.Orientation = DRAW_ORIENTATION.XY;
            triangles.DefaultColor = color;
            triangles.Load(points.ToUnityVector2(), indices);
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
            triangles.Load(points.ToUnityVector2(), indices);
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
            triangles.Load(points.ToUnityVector2(), indices);
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
            triangles.Load(points.ToUnityVector2());
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
            triangles.Load(tri.ToUnityVector2());
            triangles.ZWrite = false;
            triangles.SrcBlend = BlendMode.One;

            Renderer.Add(triangles);

            return Instance;
        }

        public RendererBuilder Outline(Polygon2 polygon, Color color)
        {
            var lineIndices = BaseRenderer.PolygonIndices(polygon.Count);
            var points = polygon.ToArray();

            var lines = new SegmentRenderer();
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(points.ToUnityVector2(), lineIndices, LINE_MODE.LINES);

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
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(segments.ToUnityVector2(), indices, LINE_MODE.LINES);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder TriangleOutline(Polygon2 polygon, Color color)
        {
            var indices = new List<int>();
            polygon.Triangulate(indices);

            var points = new List<Point2d>();
            polygon.GetPoints(points);

            var lines = new SegmentRenderer();
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(points.ToUnityVector2(), indices, LINE_MODE.TRIANGLES);

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
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(points.ToUnityVector2(), indices, LINE_MODE.TRIANGLES);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(PolygonWithHoles2 polygon, Color color)
        {
            var count = polygon.PointCount(POLYGON_ELEMENT.BOUNDARY);
            var points = new Point2d[count];
            polygon.GetPoints(POLYGON_ELEMENT.BOUNDARY, points, points.Length);

            var indices = BaseRenderer.PolygonIndices(count);

            var lines = new SegmentRenderer();
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(points.ToUnityVector2(), indices, LINE_MODE.LINES);

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
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(points.ToUnityVector2(), indices, LINE_MODE.TRIANGLES);

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
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(points.ToUnityVector2(), indices, LINE_MODE.TRIANGLES);

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
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(points.ToUnityVector2(), null, LINE_MODE.LINES);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(IList<Point2d> points, Color color)
        {
            var lines = new SegmentRenderer();
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(points.ToUnityVector2(), null, LINE_MODE.LINES);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(BaseNurbsCurve2d curve, Color color, int samples)
        {
            var points = new List<Point2d>();
            curve.GetCartesianPoints(points, samples);

            var lines = new SegmentRenderer();
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(points.ToUnityVector2(), null, LINE_MODE.LINES);

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
            var points = new Point2d[] { p1, p2 };

            var lines = new SegmentRenderer();
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = lineColor;
            lines.Load(points.ToUnityVector2(), null, LINE_MODE.LINES);

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
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(rays.ToUnityVector2(), BaseRenderer.SegmentIndices(rays.Count), LINE_MODE.LINES);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(Box2d box, Color color)
        {
            var points = box.GetCorners();

            var lines = new SegmentRenderer();
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(points.ToUnityVector2(), BaseRenderer.PolygonIndices(4), LINE_MODE.LINES);

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
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.DefaultColor = color;
            lines.Load(segments.ToUnityVector2(), BaseRenderer.SegmentIndices(segments.Count), LINE_MODE.LINES);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Outline(IList<Segment3d> segments, Color color)
        {
            var lines = new SegmentRenderer();
            lines.DefaultColor = color;
            lines.Load(segments.ToUnityVector3(), BaseRenderer.SegmentIndices(segments.Count), LINE_MODE.LINES);

            Renderer.Add(lines);

            return Instance;
        }

        public RendererBuilder Circles(IList<Circle2d> circles, Color lineCol, Color fillColor, bool filled = false, float size = POINT_SIZE)
        {

            var points = circles.ToUnityVector2();
            var radius = circles.ToRadius();

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
            pointBody.Load(points.ToUnityVector2());

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(points.ToUnityVector2());

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
            pointBody.Load(points.ToUnityVector2());

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(points.ToUnityVector2());

            Renderer.Add(pointBody);
            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Points(Polygon2 polygon, Color lineCol, Color pointCol, float size = POINT_SIZE)
        {
            var points = polygon.ToArray();

            var pointBody = new CircleRenderer();
            pointBody.Orientation = DRAW_ORIENTATION.XY;
            pointBody.Segments = POINT_SEGMENTS;
            pointBody.DefaultColor = pointCol;
            pointBody.Fill = true;
            pointBody.DefaultRadius = size * 0.5f;
            pointBody.Load(points.ToUnityVector2());

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(points.ToUnityVector2());

            Renderer.Add(pointBody);
            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Points(PolygonWithHoles2 polygon, Color lineCol, Color pointCol, float size = POINT_SIZE)
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
            pointBody.Load(points.ToUnityVector2());

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(points.ToUnityVector2());

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
            pointBody.Load(points.ToUnityVector2());

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(points.ToUnityVector2());

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
            pointBody.Load(points.ToUnityVector2());

            var pointOutline = new CircleRenderer();
            pointOutline.Orientation = DRAW_ORIENTATION.XY;
            pointOutline.Segments = POINT_SEGMENTS;
            pointOutline.DefaultColor = lineCol;
            pointOutline.Fill = false;
            pointOutline.DefaultRadius = size * 0.5f;
            pointOutline.Load(points.ToUnityVector2());

            Renderer.Add(pointBody);
            Renderer.Add(pointOutline);

            return Instance;
        }

        public RendererBuilder Points(Point3d[] points, Color pointCol, float size)
        {
            var pointBody = new VertexRenderer(size);
            pointBody.DefaultColor = pointCol;
            pointBody.Load(points.ToUnityVector3());

            Renderer.Add(pointBody);

            return Instance;
        }

        public RendererBuilder Points(Point3d[,] points, Color pointCol, float size)
        {
            var pointBody = new VertexRenderer(size);
            pointBody.DefaultColor = pointCol;
            pointBody.Load(points.ToUnityVector3());

            Renderer.Add(pointBody);

            return Instance;
        }

        public RendererBuilder Points(Vector3d[,] points, Color pointCol, float size)
        {
            var pointBody = new VertexRenderer(size);
            pointBody.DefaultColor = pointCol;
            pointBody.Load(points.ToUnityVector3());

            Renderer.Add(pointBody);

            return Instance;
        }

        public RendererBuilder Points(IList<Segment2d> segments, Color lineCol, Color pointCol, float size = POINT_SIZE)
        {
            var points = segments.ToUnityVector2();

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
            renderer.Load(points.ToUnityVector2(), tangents.ToUnityVector2());

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
            normal.Load(points.ToUnityVector2(), normals.ToUnityVector2());

            Renderer.Add(normal);

            return Instance;
        }

        public CompositeRenderer PopRenderer()
        {
            var renderer = Renderer;
            Renderer = null;
            return renderer;
        }

        public static SegmentRenderer CreateWireframeRenderer(SurfaceMesh3 mesh, Color col)
        {
            var renderer = new SegmentRenderer();
            renderer.DefaultColor = col;

            var faceVertCount = mesh.GetFaceVertexCount();
            var points = new Point3d[mesh.VertexCount];
            mesh.GetPoints(points, points.Length);

            var vectors = points.ToUnityVector3();

            if (faceVertCount.triangles > 0)
            {
                var triangles = new int[faceVertCount.triangles * 3];
                mesh.GetTriangleIndices(triangles, triangles.Length);

                renderer.Load(vectors, triangles, LINE_MODE.TRIANGLES);
            }

            if (faceVertCount.quads > 0)
            {
                var quads = new int[faceVertCount.quads * 4];
                mesh.GetQuadIndices(quads, quads.Length);

                renderer.Load(vectors, quads, LINE_MODE.QUADS);
            }

            return renderer;
        }

        public static SegmentRenderer CreateWireframeRenderer(Polyhedron3 poly, Color col)
        {
            var renderer = new SegmentRenderer();
            renderer.DefaultColor = col;

            var faceVertCount = poly.GetFaceVertexCount();
            var points = new Point3d[poly.VertexCount];
            poly.GetPoints(points, points.Length);

            var vectors = points.ToUnityVector3();

            var indices = faceVertCount.Indices();
            poly.GetPolygonalIndices(ref indices);

            if (faceVertCount.triangles > 0)
            {
                renderer.Load(vectors, indices.triangles, LINE_MODE.TRIANGLES);
            }

            if (faceVertCount.quads > 0)
            {
                renderer.Load(vectors, indices.quads, LINE_MODE.QUADS);
            }

            if (faceVertCount.pentagons > 0)
            {
                renderer.Load(vectors, indices.pentagons, LINE_MODE.PENTAGONS);
            }

            if (faceVertCount.hexagons > 0)
            {
                renderer.Load(vectors, indices.hexagons, LINE_MODE.HEXAGONS);
            }

            return renderer;
        }

        public static NormalRenderer CreateVertexNormalRenderer(IMesh imesh, Color col, float len)
        {
            var points = new Point3d[imesh.VertexCount];
            imesh.GetPoints(points, points.Length);

            var vertNormals = new Vector3d[imesh.VertexCount];
            imesh.ComputeVertexNormals();
            imesh.GetVertexNormals(vertNormals, vertNormals.Length);

            var upoints = points.ToUnityVector3();
            var vnormals = vertNormals.ToUnityVector3();

            var renderer = new NormalRenderer();
            renderer.DefaultColor = col;
            renderer.Length = len;
            renderer.Load(upoints, vnormals);

            return renderer;
        }

        public static NormalRenderer CreateFaceNormalRenderer(IMesh imesh, Color col, float len)
        {
            var centroids = new Point3d[imesh.FaceCount];
            imesh.GetCentroids(centroids, centroids.Length);

            var faceNormals = new Vector3d[imesh.FaceCount];
            imesh.ComputeFaceNormals();
            imesh.GetFaceNormals(faceNormals, faceNormals.Length);

            var ucentroids = centroids.ToUnityVector3();
            var fnormals = faceNormals.ToUnityVector3();

            var renderer = new NormalRenderer();
            renderer.DefaultColor = col;
            renderer.Length = len;
            renderer.Load(ucentroids, fnormals);

            return renderer;
        }

    }

}
