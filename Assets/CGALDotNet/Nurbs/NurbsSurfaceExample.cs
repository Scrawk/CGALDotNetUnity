using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Common.Unity.Drawing;

using CGALDotNet.Geometry;
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

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Dictionary<string, CompositeRenderer> Renderers;

        private bool DrawNormals, DrawControlPoints;

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

            //var split = RationalNurbsSurface3d.SplitV(srf, 0.25);

            //CreateRenderers(srf, 100);
        }

        private void CreateRenderers(RationalNurbsSurface3d surface, int samples)
        {
            var points = new Point3d[samples, samples];
            surface.GetCartesianPoints(samples, points);

            Renderers["Surface"] = Draw().
                Points(points, redColor, 0.0025f).
                PopRenderer();
        }

        private void OnPostRender()
        {
            foreach (var renderer in Renderers.Values)
                renderer.Draw();
        }

    }

}
