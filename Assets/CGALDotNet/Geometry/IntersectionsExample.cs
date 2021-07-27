using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Geometry;

namespace CGALDotNetUnity.Geometry
{
    public class IntersectionsExample : InputBehaviour
    {

        private Color shapeColor = new Color32(80, 80, 200, 128);

        private Color intersectionColor = new Color32(200, 80, 00, 128);


        private List<IGeometry2d> Geometry;

        protected override void Start()
        {
            base.Start();

            SetInputMode(INPUT_MODE.NONE);

            var box1 = new Box2d(-5, -5, 5, 5);
            var box2 = new Box2d(-10, -10, 0, 0);
            var line = new Line2d(new Point2d(0, 0), new Point2d(1, 0));

            Geometry = new List<IGeometry2d>();
            Geometry.Add(box1);
            Geometry.Add(box2);
            Geometry.Add(line);

            AddBox("", box1, shapeColor, shapeColor);
            AddBox("", box2, shapeColor, shapeColor);
            AddLine("", line, shapeColor);

            int count = Geometry.Count;

            var check = new bool[count, count];

            for(int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (check[i, j]) continue;
                    check[i, j] = true;
                    check[j, i] = true;

                    if (Geometry[i] == Geometry[j]) continue;

                    var result = CGALIntersections.Intersection(Geometry[i], Geometry[j]);

                    AddIntersection(result);
                }
            }

        }

        private void OnRenderObject()
        {
            DrawGrid();
            DrawShapes();
            DrawInput();
            DrawPoint();
        }

        private void AddIntersection(IntersectionResult2d result)
        {
            switch (result.type)
            {
                case INTERSECTION_RESULT_2D.POINT2:
                    break;

                case INTERSECTION_RESULT_2D.LINE2:
                    AddLine("", result.Line, intersectionColor);
                    break;

                case INTERSECTION_RESULT_2D.RAY2:
                    break;

                case INTERSECTION_RESULT_2D.SEGMENT2:
                    break;

                case INTERSECTION_RESULT_2D.BOX2:
                    AddBox("", result.Box, intersectionColor, intersectionColor);
                    break;

                case INTERSECTION_RESULT_2D.TRIANGLE2:
                    break;
            }
        }

    }

}
