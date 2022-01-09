using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Polygons;
using CGALDotNet.Geometry;

namespace CGALDotNetUnity.Geometry
{

    public class IntersectionExample : InputBehaviour
    {
        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color intersectionColor = new Color32(200, 80, 80, 128);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private List<IGeometry2d> Geometry;

        private Dictionary<string, CompositeRenderer> Renderers;

        protected override void Start()
        {
            base.Start();
            Renderers = new Dictionary<string, CompositeRenderer>();
            Geometry = new List<IGeometry2d>();

            //var point = new Point2d();
            //var line = new Line2d(new Point2d(0, 0), new Point2d(1,1));
            //var segment = new Segment2d(new Point2d(-10, 0), new Point2d(10, 0));
            var box = new Box2d(-5, 5);
            var triangle = new Triangle2d(new Point2d(1, 1), new Point2d(9, 1), new Point2d(1, 9));
            //var ray = new Ray2d(new Point2d(0,0), new Vector2d(0, 1));

            //Geometry.Add(point);
            Geometry.Add(box);
            //Geometry.Add(line);
            //Geometry.Add(segment);
            Geometry.Add(triangle);
            //Geometry.Add(ray);

            foreach(var geometry in Geometry)
            {
                CreateRenderer(geometry);
            }

            var set = new HashSet<ValueTuple<IGeometry2d, IGeometry2d>>();

            foreach (var geo1 in Geometry)
            {
                foreach(var geo2 in Geometry)
                {
                    if (geo1 == geo2) continue;

                    var ab = (geo1, geo2);
                    var ba = (geo2, geo1);

                    if (set.Contains(ab)) continue;

                    set.Add(ab);
                    set.Add(ba);

                    var result = CGALIntersections.Intersection(geo1, geo2);
                   if(result.Hit)
                    {
                        CreateRenderer(result, intersectionColor);
                    }
        
                }
            }
        }

        private void CreateRenderer( IntersectionResult2d result, Color col)
        {
            int count = Renderers.Count;

            if (result.Type == INTERSECTION_RESULT_2D.POLYGON2)
            {
                var polygon = result.Polygon<EEK>();

                Renderers["Geometry" + count] = Draw().
                Faces(polygon, col).
                Outline(polygon, col).
                PopRenderer();
            }
            else if(result.Type == INTERSECTION_RESULT_2D.POINT2)
            {
                var point = result.Point;

                Renderers["Geometry" + count] = Draw().
                    Points(point, col, col).
                    PopRenderer();
            }
            else 
            {
                var geo = result.Geometry;
                Renderers["Geometry" + count] = Draw().
                    Outline(geo, col).
                    PopRenderer();
            }
        }

        private void CreateRenderer(IGeometry2d geometry)
        {
            int count = Renderers.Count;

            if (geometry is Box2d box)
            {
                var polygon = PolygonFactory<EEK>.CreateBox(box);

                Renderers["Geometry" + count] = Draw().
                Faces(polygon, faceColor).
                Outline(polygon, lineColor).
                PopRenderer();
            }
            else if (geometry is Triangle2d tri)
            {
                var polygon = PolygonFactory<EEK>.CreateTriangle(tri);

                Renderers["Geometry" + count] = Draw().
                Faces(polygon, faceColor).
                Outline(polygon, lineColor).
                PopRenderer();
            }
            else if (geometry is Point2d point)
            {
                Renderers["Geometry" + count] = Draw().
                    Points(point, lineColor, pointColor).
                    PopRenderer();
            }
            else
            {
                Renderers["Geometry" + count] = Draw().
                    Outline(geometry, lineColor).
                    PopRenderer();
            }
        }


        private void OnPostRender()
        {
            DrawGrid();

            foreach (var renderer in Renderers.Values)
                renderer.Draw();

        }

    }
}
