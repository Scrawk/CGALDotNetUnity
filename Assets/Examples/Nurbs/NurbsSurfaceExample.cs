using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Common.Unity.Drawing;

using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNet.Nurbs;

namespace CGALDotNetUnity.Nurbs
{
    /// <summary>
    /// WIP, not working atm.
    /// </summary>
    public class NurbsSurfaceExample : InputBehaviour
    {
        private Color redColor = new Color32(200, 80, 80, 255);

        private Color greenColor = new Color32(80, 200, 80, 255);

        private Color blueColor = new Color32(80, 80, 200, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        private GameObject Sphere;

        public Material material;

        protected override void Start()
        {
            base.Start();
            SetInputMode(INPUT_MODE.NONE);
            Renderers = new Dictionary<string, CompositeRenderer>();
     
            CreateNurbs();
        }

        private void CreateNurbs()
        {
            
            int degree_u = 3;
            int degree_v = 3;
            var knots_u = new List<double>() { 0, 0, 0, 0, 1, 1, 1, 1};
            var knots_v = new List<double>() { 0, 0, 0, 0, 1, 1, 1, 1};

            // 2D array of control points using tinynurbs::array2<T> container
            // Example from geometrictools.com/Documentation/NURBSCircleSphere.pdf

            var control_points = new Point3d[,]
            {
                { new Point3d(0, 0, 1), new Point3d(0, 0, 1), new Point3d(0, 0, 1), new Point3d(0, 0, 1) },
                {  new Point3d(2, 0, 1), new Point3d(2, 4, 1),  new Point3d(-2, 4, 1),  new Point3d(-2, 0, 1) },
                {  new Point3d(2, 0, -1), new Point3d(2, 4, -1), new Point3d(-2, 4, -1), new Point3d(-2, 0, -1) },
                {  new Point3d(0, 0, -1), new Point3d(0, 0, -1), new Point3d(0, 0, -1), new Point3d(0, 0, -1) }
            };

            var weights = new double[,] 
            {
                { 1.0 , 1.0 / 3.0, 1.0 / 3.0, 1.0 },
                { 1.0 / 3.0, 1.0 / 9.0, 1.0 / 9.0, 1.0 / 3.0 },
                { 1.0 / 3.0, 1.0 / 9.0, 1.0 / 9.0, 1.0 / 3.0 },
                { 1.0, 1.0 / 3.0, 1.0 / 3.0, 1.0 }
            };

            var srf = new RationalNurbsSurface3d(degree_u, degree_v, knots_u, knots_v, control_points, weights);

            //RationalNurbsSurface3d left, right;
            //RationalNurbsSurface3d.SplitV(srf, 0.25, out left, out right);

            //CreateRenderers(srf, 100, false);

            var mesh = CreateUnityMesh(srf, 100);

            Sphere = new GameObject();
            var filter = Sphere.AddComponent<MeshFilter>();
            var renderer = Sphere.AddComponent<MeshRenderer>();

            filter.mesh = mesh;
            renderer.material = material;
        }

        private Mesh CreateUnityMesh(RationalNurbsSurface3d srf, int samples)
        {
      
            var positions = new Vector3[samples * samples];
            var uv = new Vector2[samples * samples];
            var normals = new Vector3[samples * samples];
            var indices = new int[(samples * samples) * 6];

            for (int j = 0; j < samples; j++)
            {
                for (int i = 0; i < samples; i++)
                {
                    double u = i / (samples - 1.0);
                    double v = j / (samples - 1.0);

                    var p = srf.CartesianPoint(u, v);
                    var n = srf.Normal(u, v);

                    positions[i + j * samples] = new Vector3((float)p.x, (float)p.y, (float)p.z);
                    uv[i + j * samples] = new Vector2((float)u, (float)v);
                    normals[i + j * samples] = new Vector3((float)n.x, (float)n.y, (float)n.z);
                }
            }

            for (int j = 0; j < samples - 1; j++)
            {
                for (int i = 0; i < samples - 1; i++)
                {
                    indices[(i + j * samples) * 6 + 0] = i + j * samples;
                    indices[(i + j * samples) * 6 + 1] = (i+1) + j * samples;
                    indices[(i + j * samples) * 6 + 2] = i + (j+1) * samples;

                    indices[(i + j * samples) * 6 + 3] = (i+1) +  j* samples;
                    indices[(i + j * samples) * 6 + 4] = (i+1) + (j+1) * samples;
                    indices[(i + j * samples) * 6 + 5] = i + (j+1) * samples;
                }
            }

            var mesh = new Mesh();

            mesh.vertices = positions;
            mesh.uv = uv;
            mesh.normals = normals;
            mesh.triangles = indices;
            mesh.RecalculateBounds();
            //mesh.RecalculateNormals();
            //mesh.RecalculateTangents();

            return mesh;
        }

        private void CreateRenderers(RationalNurbsSurface3d srf, int samples, bool drawControl)
        {
            var points = new Point3d[samples, samples];
            srf.GetCartesianPoints(samples, points);

            if (drawControl)
            {
                var control = new List<Segment3d>();
                for (int j = 0; j < srf.Height; j++)
                {
                    for (int i = 0; i < srf.Width - 1; i++)
                    {
                        var a = srf.GetCartesianControlPoint(i, j);
                        var b = srf.GetCartesianControlPoint(i+ 1, j);

                        control.Add(new Segment3d(a, b));
                    }
                }

                Renderers["Surface"] = Draw().
                Points(points, redColor, 0.0025f).
                Outline(control, greenColor).
                PopRenderer();
            }
            else
            {
                Renderers["Surface"] = Draw().
                    Points(points, redColor, 0.0025f).
                    PopRenderer();
            }
        }

        private void OnPostRender()
        {
            foreach (var renderer in Renderers.Values)
                renderer.Draw();
        }


    }

}
