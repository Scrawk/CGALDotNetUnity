using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polyhedra;
using CGALDotNet.Processing;
using CGALDotNet.Polylines;

using Common.Unity.Drawing;

namespace CGALDotNetUnity.Processing
{

    public class SlicerExample : MonoBehaviour
    {
        public Material material;

        public Color lineColor = Color.black;

        public string file;

        private Polyhedron3<EEK> poly;

        private GameObject m_object;

        private SegmentRenderer m_renderer;

        private void Start()
        {
            //Used for debuging.
            //dont recommend loading meshes like this.
            string filename = Application.dataPath + "/Examples/Data/" + file;

            var split = filename.Split('/', '.');
            int i = split.Length - 2;
            var name = i > 0 ? split[i] : "Mesh";

            poly = new Polyhedron3<EEK>();
            poly.ReadOFF(filename);
            //poly.Triangulate();

            m_object = CreateGameobject(name, poly);

            var bounds = LookAt(m_object);

            var lines = Slice(poly, bounds, 20);

            m_renderer = CreateLineRenderer(lines, lineColor);
        }

        private Bounds LookAt(GameObject go)
        {
            var filter = go.GetComponent<MeshFilter>();
            if (filter == null) return new Bounds();

            var bounds = filter.sharedMesh.bounds;
            var size = bounds.size;
            var center = bounds.center;

            Camera.main.transform.position = center + new Vector3(0, 0, size.z * 5);
            Camera.main.transform.LookAt(center, Vector3.up);

            return bounds;
        }

        private List<Polyline3<EEK>> Slice(Polyhedron3<EEK> poly, Bounds bounds, int count)
        {
            var size = bounds.size;
            var half = size * 0.5f;
            half.x = 0;
            half.z = 0;

            var center = bounds.center;

            var start = (center + half).ToCGALPoint3d();
            var end = (center - half).ToCGALPoint3d();
            var increment = (end - start).Magnitude / count;

            var slicer = MeshProcessingSlicer<EEK>.Instance;

            var lines = new List<Polyline3<EEK>>();
            slicer.Slice(poly, start, end, increment, lines);

            return lines;
        }

        private GameObject CreateGameobject(string name, Polyhedron3<EEK> poly)
        {
            return poly.ToUnityMesh(name, material, false);
        }

        private SegmentRenderer CreateLineRenderer(List<Polyline3<EEK>> lines, Color color)
        {
            var renderer = new SegmentRenderer();
            renderer.DefaultColor = color;

            foreach(var line in lines)
            {
                var points = line.ToArray().ToUnityVector3();
                renderer.Load(points, null, LINE_MODE.LINES);
            }

            return renderer;
        }

        private void OnRenderObject()
        {
            if(lineColor != m_renderer.DefaultColor)
                m_renderer.SetColor(lineColor);

            m_renderer.Draw();
        }

    }

}
