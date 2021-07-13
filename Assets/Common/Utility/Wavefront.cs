using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;

namespace Common.Unity.Utility
{

    public static class Wavefront
    {

        public static Mesh Import(string fileName)
        {

            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            string[] space = new string[] { " " };
            string[] slash = new string[] { "/" };

            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {

                    if (line[0] == 'f')
                    {
                        string[] elements = line.Split(space, StringSplitOptions.None);

                        string[] face1 = elements[1].Split(slash, StringSplitOptions.None);
                        string[] face2 = elements[2].Split(slash, StringSplitOptions.None);
                        string[] face3 = elements[3].Split(slash, StringSplitOptions.None);

                        indices.Add(int.Parse(face1[0]) - 1);
                        indices.Add(int.Parse(face2[1]) - 1);
                        indices.Add(int.Parse(face3[2]) - 1);

                    }
                    else if (line[0] == 'v')
                    {
                        string[] elements = line.Split(space, StringSplitOptions.None);

                        if (line[1] == ' ')
                        {
                            Vector3 v;
                            v.x = float.Parse(elements[1]);
                            v.y = float.Parse(elements[2]);
                            v.z = float.Parse(elements[3]);

                            positions.Add(v);
                        }
                        else if (line[1] == 'n')
                        {
                            Vector3 v;
                            v.x = float.Parse(elements[1]);
                            v.y = float.Parse(elements[2]);
                            v.z = float.Parse(elements[3]);

                            normals.Add(v);
                        }
                        else if (line[1] == 't')
                        {
                            Vector2 v;
                            v.x = float.Parse(elements[1]);
                            v.y = float.Parse(elements[2]);
               
                            uvs.Add(v);
                        }

                    }

                }

            }

            Mesh mesh = new Mesh();
            mesh.SetVertices(positions);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(indices, 0);

            return mesh;
        }

        public static void Export(string fileName, Mesh mesh)
        {
            Export(fileName, mesh.vertices, mesh.normals, mesh.uv, mesh.triangles);
        }

        public static void Export(string fileName, IList<Vector3> positions, IList<Vector3> normals, IList<Vector2> uv0, IList<int> indices)
        {
            if (positions == null)
                throw new NullReferenceException("Positions can not be null.");

            StringBuilder sb = new StringBuilder();

            foreach (Vector3 v in positions)
            {
                sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
            }
            sb.Append("\n");

            if (normals != null)
            {
                foreach (Vector3 v in normals)
                {
                    sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
                }
                sb.Append("\n");
            }

            if (uv0 != null)
            {
                foreach (Vector2 v in uv0)
                {
                    sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
                }
                sb.Append("\n");
            }

            for (int i = 0; i < indices.Count; i += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", indices[i] + 1, indices[i + 1] + 1, indices[i + 2] + 1));
            }

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(sb.ToString());
            }
        }


    }


}