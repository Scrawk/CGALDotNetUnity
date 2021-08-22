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

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(20, 20, 255, 128);

        private Point2d[] points;

        private Polygon2<EEK> hull;

        List<Polygon2<EEK>> list = new List<Polygon2<EEK>>();

        List<Color> colors = new List<Color>();

        System.Random rnd = new System.Random(0);

        protected override void Start()
        {
            base.Start();

            int count = 50;
            int seed = 0;
            points = RandomPoints(count, seed);
            hull = ConvexHull2<EEK>.Instance.CreateHull(points, 0, count);

            //DrawGridAxis(true);

            hull = PolygonFactory<EEK>.KochStar(20, 2);

            list = new List<Polygon2<EEK>>();
            PolygonPartition2<EEK>.Instance.OptimalConvex(hull, list);

            for (int i = 0; i < list.Count; i++)
            {
                float r = rnd.NextFloat(0.5f, 1.0f);
                float g = rnd.NextFloat(0.5f, 1.0f);
                float b = rnd.NextFloat(0.5f, 1.0f);
                float a = 0.5f;

                colors.Add(new Color(r, g,  b, a));
            }
                

        }

        private void OnDestroy()
        {
            if (hull != null)
                hull.Dispose();

            foreach (var poly in list)
                poly.Dispose();

            hull = null;
            list.Clear();
        }

        private void OnPostRender()
        {
            DrawGrid();

            //CreatePolygon(hull, pointColor, pointColor, lineColor, 0.2f).Draw();
            //CreatePoints(points, pointColor, lineColor, 0.2f).Draw();

            for (int i = 0; i < list.Count; i++)
                CreatePolygon(list[i], colors[i]).Draw();
            
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
