using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace UnityEngine
{

    public static class BoundsExtension 
    {

        public static Box2f ToBox2f(this Bounds bounds)
        {
            return new Box2f(bounds.min.ToVector2f(), bounds.max.ToVector2f());
        }

        public static Box2i ToBox2i(this Bounds bounds)
        {
            return new Box2i(bounds.min.ToVector2i(), bounds.max.ToVector2i());
        }

        public static Box3f ToBox3f(this Bounds bounds)
        {
            return new Box3f(bounds.min.ToVector3f(), bounds.max.ToVector3f());
        }

        public static Bounds ToBounds(this Box2f box)
        {
            var bounds = new Bounds();
            bounds.min = box.Min.ToVector2();
            bounds.max = box.Max.ToVector2();
            return bounds;
        }

        public static Bounds ToBounds(this Box3f box)
        {
            var bounds = new Bounds();
            bounds.min = box.Min.ToVector3();
            bounds.max = box.Max.ToVector3();
            return bounds;
        }

        public static BoundsInt ToBounds(this Box2i box)
        {
            var bounds = new BoundsInt();
            bounds.min = box.Min.ToVector3Int();
            bounds.max = box.Max.ToVector3Int();
            return bounds;
        }

        public static BoundsInt ToBounds(this Box3i box)
        {
            var bounds = new BoundsInt();
            bounds.min = box.Min.ToVector3Int();
            bounds.max = box.Max.ToVector3Int();
            return bounds;
        }

    }

}
