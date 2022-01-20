using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet.Geometry;
using Common.Unity.Drawing;

namespace CGALDotNet.Polyhedra
{
    public static class Polyhedron3Extension
    {
        public static GameObject ToUnityMesh(this Polyhedron3 poly, string name, Material material, bool splitFaces = true)
        {
            if (!poly.IsValid)
            {
                Debug.Log("Polyhedron3 is not valid");
                return new GameObject(name);
            }
                
            if (!poly.IsTriangle)
                poly.Triangulate();

            int count = poly.VertexCount;
            if(count == 0)
            {
                Debug.Log("Polyhedron3 is empty");
                return new GameObject(name);
            }

            var points = new Point3d[count];
            var indices = new int[poly.FaceCount * 3];   
            poly.GetPoints(points, points.Length);
            poly.GetTriangleIndices(indices, indices.Length);

            //if (!points.IsFinite())
            //{
            //    Debug.Log("Polyhedron3 points are not finite.");
            //    return new GameObject(name);
            //}

            Mesh mesh;

            if(splitFaces)
            {
                ExtensionHelper.SplitFaces(points, indices, out Point3d[] splitPoints, out int[] spliIndices);
                mesh = ExtensionHelper.CreateMesh(splitPoints, spliIndices);
            }
            else
            {
                mesh = ExtensionHelper.CreateMesh(points, indices);
            }

            return ExtensionHelper.CreateGameobject(name, mesh, Vector3.zero, material);
        }

        public static CompositeRenderer CreateWireframeRenderer(this Polyhedron3 poly, Color col)
        {
            var renderer = new CompositeRenderer();
    
            var primatives = poly.GetPrimativeCount();
            var points = new Point3d[poly.VertexCount];
            poly.GetPoints(points, points.Length);

            var vectors = points.ToUnityVector3();

            if (primatives.three > 0)
            {
                var triangles = new int[primatives.three * 3];
                poly.GetTriangleIndices(triangles, triangles.Length);

                var triangleRenderer = new SegmentRenderer();
                triangleRenderer.DefaultColor = col;
                triangleRenderer.LineMode = LINE_MODE.TRIANGLES;
                triangleRenderer.Load(vectors, triangles);

                renderer.Add(triangleRenderer);
            }

            if (primatives.four > 0)
            {
                var quads = new int[primatives.four * 4];
                poly.GetQuadIndices(quads, quads.Length);

                var quadRenderer = new SegmentRenderer();
                quadRenderer.DefaultColor = col;
                quadRenderer.LineMode = LINE_MODE.QUADS;
                quadRenderer.Load(vectors, quads);

                renderer.Add(quadRenderer);
            }

            return renderer;
        }

        public static NormalRenderer CreateVertexNormalRenderer(this Polyhedron3 poly, Color col, float len)
        {
            var points = new Point3d[poly.VertexCount];
            poly.GetPoints(points, points.Length);

            var vertNormals = new Vector3d[poly.VertexCount];
            poly.ComputeVertexNormals();
            poly.GetVertexNormals(vertNormals, vertNormals.Length);

            var upoints = points.ToUnityVector3();
            var vnormals = vertNormals.ToUnityVector3();

            var renderer = new NormalRenderer();
            renderer.DefaultColor = col;
            renderer.Length = len;
            renderer.Load(upoints, vnormals);

            return renderer;
        }

        public static NormalRenderer CreateFaceNormalRenderer(this Polyhedron3 poly, Color col, float len)
        {
            var centroids = new Point3d[poly.FaceCount];
            poly.GetCentroids(centroids, centroids.Length);

            var faceNormals = new Vector3d[poly.FaceCount];
            poly.ComputeFaceNormals();
            poly.GetFaceNormals(faceNormals, faceNormals.Length);

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
