using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Numerics;

namespace Common.Unity.Drawing
{

    public class GridRenderer
    {

        private SegmentRenderer m_vertical, m_horizontal;

        private SegmentRenderer m_xaxis, m_yaxis;

        private VertexRenderer m_vertices;

        public int Range = 50;

        public float PointSize = 0.1f;

        public float Spacing = 1.0f;

        public Color32 BackgroundColor = new Color32(180, 180, 180, 255);

        public Color32 AxisColor = new Color32(64, 64, 64, 255);

        public Color32 PointColor = new Color32(32, 32, 32, 255);

        public bool DrawAxis = true;

        public bool ScaleOnZoom;

        public void Create()
        {

            m_vertical = new SegmentRenderer(LINE_MODE.LINES, DRAW_ORIENTATION.XY);
            m_vertical.Color = BackgroundColor;
            m_vertical.Load(new Vector2(0, -Range), new Vector2(0, Range));

            m_horizontal = new SegmentRenderer(LINE_MODE.LINES, DRAW_ORIENTATION.XY);
            m_horizontal.Color = BackgroundColor;
            m_horizontal.Load(new Vector2(-Range, 0), new Vector2(Range, 0));

            m_yaxis = new SegmentRenderer(LINE_MODE.LINES, DRAW_ORIENTATION.XY);
            m_yaxis.Color = AxisColor;
            m_yaxis.Load(new Vector2(0, -Range), new Vector2(0, Range));

            m_xaxis = new SegmentRenderer(LINE_MODE.LINES, DRAW_ORIENTATION.XY);
            m_xaxis.Color = AxisColor;
            m_xaxis.Load(new Vector2(-Range, 0), new Vector2(Range, 0));

            m_vertices = new VertexRenderer(PointSize, DRAW_ORIENTATION.XY);
            m_vertices.Color = PointColor;

            for (int i = -Range; i < Range; i++)
            {
                m_vertices.Load(new Vector2(i, 0));
                m_vertices.Load(new Vector2(0, i));
            }
        }

        public void Draw()
        {
            Draw(Camera.current);
        }

        public void Draw(Camera cam)
        {

            for (double i = -Range; i < Range; i += Spacing)
            {
                var m = Matrix4x4f.Translate(new Vector3f(i, 0, 0));
                m_vertical.Draw(cam, m);

                m = Matrix4x4f.Translate(new Vector3f(0, i, 0));
                m_horizontal.Draw(cam, m);
            }

            if (DrawAxis)
            {
                m_vertices.ScaleOnZoom = ScaleOnZoom;

                m_xaxis.Draw(cam);
                m_yaxis.Draw(cam);
                m_vertices.Draw(cam);
            }
        }

    }

}
