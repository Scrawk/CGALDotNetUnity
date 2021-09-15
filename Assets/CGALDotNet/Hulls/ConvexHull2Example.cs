using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;
using CGALDotNet.Hulls;

namespace CGALDotNetUnity.Polygons
{

    public class ConvexHull2Example : InputBehaviour
    {
        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private HULL_METHOD Method = HULL_METHOD.DEFAULT;

        private Polygon2<EEK> Hull;

        private int Seed = Guid.NewGuid().GetHashCode();

        private Dictionary<string, CompositeRenderer> Renderers;


        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();

            CreateHull();
        }

        private void CreateHull()
        {
            var points = CreatePoints(100, 20, 10);

            float start = Time.realtimeSinceStartup;

            Hull = ConvexHull2<EEK>.Instance.CreateHull(points, Method);

            float end = Time.realtimeSinceStartup;
            float ms= (end - start) * 1000;

            Debug.Log("Created hull in " + ms + "ms");

            Renderers["Hull"] = FromPolygon(Hull, faceColor, lineColor);
            Renderers["Points"] = FromPoints(points, lineColor, pointColor, PointSize);
        }

        private Point2d[] CreatePoints(int count, double xrange, double yrange)
        {
            var rnd = new System.Random(Seed);

            var points = new Point2d[count];

            for(int i = 0; i < count; i++)
            {
                double x = rnd.NextDouble(-xrange, xrange);
                double y = rnd.NextDouble(-yrange, yrange);
                points[i] = new Point2d(x, y);
            }

            return points;
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Method = Method.Next();
                CreateHull();
            }
            else if (Input.GetKeyDown(KeyCode.F1))
            {
                Seed = Guid.NewGuid().GetHashCode();
                CreateHull();
            }

        }

        private void OnPostRender()
        {
            DrawGrid();

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

        }

        protected void OnGUI()
        {
            int textLen = 400;
            int textHeight = 25;
            GUI.color = Color.black;

            GUI.Label(new Rect(10, 10, textLen, textHeight), "Tab to change hull method.");
            GUI.Label(new Rect(10, 30, textLen, textHeight), "F1 to change seed.");
            GUI.Label(new Rect(10, 50, textLen, textHeight), "Hull method = " + Method);
            GUI.Label(new Rect(10, 70, textLen, textHeight), "Seed = " + Seed);
        }



    }
}
