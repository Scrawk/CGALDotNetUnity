using System;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Hulls;
using CGALDotNet.Polygons;

namespace CGALDotNetUnity.Polygons
{

    public class CreateHullExample : InputBehaviour
    {

        private Color pointColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(20, 20, 255, 128);

        private Point2d[] points;

        private Polygon2<EEK> hull;

        protected override void Start()
        {
            base.Start();

            int count = 50;
            int seed = 0;
            points = RandomPoints(count, seed);
            hull = ConvexHull2<EEK>.Instance.CreateHull(points, 0, count);

            DrawGridAxis(true);

        }

        private void OnPostRender()
        {
            DrawGrid();

           CreatePolygon(hull, pointColor, pointColor, lineColor, 0.2f).Draw();
           CreatePoints(points, pointColor, lineColor, 0.2f).Draw();
        }

        private Point2d[] RandomPoints(int count, int seed)
        {
            var points = new Point2d[count];

            var rnd = new System.Random(seed);

            for (int i = 0; i < count; i++)
            {
                double x = rnd.NextDouble(-10, 10);
                double y = rnd.NextDouble(-10, 10);

                points[i] = new Point2d(x, y);
            }

            return points;
        }


    }
}
