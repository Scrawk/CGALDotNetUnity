using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Unity.Drawing
{

    public class GridRenderer
    {

        private SegmentRenderer m_vertical, m_horizontal;

        private SegmentRenderer m_xaxis, m_yaxis;

        private VertexRenderer m_vertices;

        private Color32 m_lineColor = new Color32(180, 180, 180, 255);

        private Color32 m_axisColor = new Color32(64, 64, 64, 255);

        public int Range = 50;

        public float PointSize = 0.1f;

        public float Spacing = 1.0f;

        public bool DrawAxis = true;

        public bool ScaleOnZoom;

        public Color32 LineColor
        {
            get => m_lineColor;
            set => SetLineColor(value);
        }

        public Color32 AxisColor
        {
            get => m_axisColor;
            set => SetAxisColor(value);
        }

        public void Create()
        {

            m_vertical = new SegmentRenderer(DRAW_ORIENTATION.XY);
            m_vertical.DefaultColor = LineColor;
            m_vertical.Load(new Vector2(0, -Range), new Vector2(0, Range));

            m_horizontal = new SegmentRenderer(DRAW_ORIENTATION.XY);
            m_horizontal.DefaultColor = LineColor;
            m_horizontal.Load(new Vector2(-Range, 0), new Vector2(Range, 0));

            m_yaxis = new SegmentRenderer(DRAW_ORIENTATION.XY);
            m_yaxis.DefaultColor = AxisColor;
            m_yaxis.Load(new Vector2(0, -Range), new Vector2(0, Range));

            m_xaxis = new SegmentRenderer(DRAW_ORIENTATION.XY);
            m_xaxis.DefaultColor = AxisColor;
            m_xaxis.Load(new Vector2(-Range, 0), new Vector2(Range, 0));

            m_vertices = new VertexRenderer(PointSize, DRAW_ORIENTATION.XY);
            m_vertices.DefaultColor = AxisColor;

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
                var m = Matrix4x4.Translate(new Vector3((float)i, 0, 0));
                m_vertical.Draw(cam, m);

                m = Matrix4x4.Translate(new Vector3(0, (float)i, 0));
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

        private void SetLineColor(Color color)
        {
            m_lineColor = color;
            m_vertical.SetColor(color);
            m_horizontal.SetColor(color);
        }

        private void SetAxisColor(Color color)
        {
            m_axisColor = color;
            m_xaxis.SetColor(color);
            m_yaxis.SetColor(color);
        }

    }

}
