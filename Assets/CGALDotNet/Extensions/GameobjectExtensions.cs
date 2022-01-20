using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;
using CGALDotNet.Polyhedra;
public static class CGALGameObjectExtensions
{

    public static Polyhedron3<K> ToCGALPolyhedron3<K>(this GameObject go) where K : CGALKernel, new()
    {
        var filter = go.GetComponent<MeshFilter>();
        if (filter == null)
        {
            Debug.Log("GameObject does not have a mesh filter.");
            return new Polyhedron3<K>();
        }

        return filter.sharedMesh.ToCGALPolyhedron3<K>();
    }

}


