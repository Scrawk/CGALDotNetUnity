using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Numerics;
using Common.Core.Colors;
using Common.Geometry.Shapes;

#pragma warning disable 649

namespace Common.Unity.Utility
{

    [AddComponentMenu("Common/Utility/Image Plane")]
    public class ImagePlane : MonoBehaviour
    {

        [SerializeField]
        private Material m_material;

        [SerializeField]
        private float m_pixelScale = 1;

        [SerializeField]
        private bool m_pixelOffset = false;

        [SerializeField]
        private bool m_absolute = false;

        [SerializeField]
        private bool XY = false;

        [SerializeField]
        private Vector3 m_localPosition;

        [SerializeField]
        private FilterMode m_filterMode = FilterMode.Bilinear;

        private Texture2D m_tex;

        public GameObject Create(float[,] source, Box2f bounds)
        {
            m_tex = CreateTexture(source);
            return CreateGameObject(bounds, m_tex);
        }

        public GameObject Create(ColorRGB[,] source, Box2f bounds)
        {
            m_tex = CreateTexture(source);
            return CreateGameObject(bounds, m_tex);
        }

        public Color GetPixel(float u, float v)
        {
            if (m_tex == null)
                return Color.black;
            else
                return m_tex.GetPixelBilinear(u, v);
        }

        public Color GetPixel(Vector2d uv)
        {
            if (m_tex == null)
                return Color.black;
            else
                return m_tex.GetPixelBilinear((float)uv.x, (float)uv.y);
        }

        public Color GetPixel(int x, int y)
        {
            if (m_tex == null)
                return Color.black;
            else
                return m_tex.GetPixel(x, y);
        }

        private GameObject CreateGameObject(Box2f bounds, Texture2D tex)
        {
            var go = new GameObject("ImagePlane");

            go.transform.localPosition += m_localPosition;

            var filter = go.AddComponent<MeshFilter>();
            filter.sharedMesh = CreateMesh(tex, bounds);

            var renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = new Material(m_material);
            renderer.sharedMaterial.mainTexture = tex;

            return go;
        }

        private Texture2D CreateTexture(float[,] source)
        {
            int width = source.GetLength(0);
            int height = source.GetLength(1);

            var texture = new Texture2D(width, height);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = m_filterMode;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float v = source[x, y] * m_pixelScale;

                    if (m_absolute)
                        v = Mathf.Abs(v);

                    v = Mathf.Clamp01(v);
                    texture.SetPixel(x, y, new Color(v, v, v, 1));
                }
            }

            texture.Apply();

            return texture;
        }

        private Texture2D CreateTexture(ColorRGB[,] source)
        {
            int width = source.GetLength(0);
            int height = source.GetLength(1);

            var texture = new Texture2D(width, height);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = m_filterMode;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixel = source[x, y] * m_pixelScale;

                    if (m_absolute)
                    {
                        pixel.r = Mathf.Abs(pixel.r);
                        pixel.g = Mathf.Abs(pixel.g);
                        pixel.b = Mathf.Abs(pixel.b);
                    }

                    pixel = ColorRGB.Clamp(pixel, 0, 1);
                    texture.SetPixel(x, y, pixel.ToColor());
                }
            }

            texture.Apply();

            return texture;
        }

        private Mesh CreateMesh(Texture2D tex, Box2f bounds)
        {
            int width = tex.width;
            int height = tex.height;

            Mesh mesh = new Mesh();
            var size = bounds.Size;

            Vector3[] vertices = new Vector3[4];

            if (!XY)
            {
                vertices[0] = new Vector3(0, 0, 0);
                vertices[1] = new Vector3(size.x, 0, 0);
                vertices[2] = new Vector3(0, 0, size.y);
                vertices[3] = new Vector3(size.x, 0, size.y);
            }
            else
            {
                vertices[0] = new Vector3(0, 0, 0);
                vertices[1] = new Vector3(size.x, 0, 0);
                vertices[2] = new Vector3(0, size.y, 0);
                vertices[3] = new Vector3(size.x, size.y, 0);
            }
            

            int[] tris = new int[6]
            {
                0, 2, 1,
                2, 3, 1
            };

            float u0, u1, v0, v1;

            if (m_pixelOffset)
            {
                u0 = 0.5f / width;
                u1 = (width - 0.5f) / width;
                v0 = 0.5f / height;
                v1 = (height - 0.5f) / height;
            }
            else
            {
                u0 = 0;
                u1 = 1;
                v0 = 0;
                v1 = 1;
            }

            Vector2[] uv = new Vector2[4]
            {
                new Vector2(u0, v0),
                new Vector2(u1, v0),
                new Vector2(u0, v1),
                new Vector2(u1, v1)
            };

            mesh.vertices = vertices;
            mesh.triangles = tris;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

    }
}
