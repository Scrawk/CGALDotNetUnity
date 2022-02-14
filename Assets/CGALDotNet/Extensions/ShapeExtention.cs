using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNetGeometry.Numerics;

namespace CGALDotNetGeometry.Shapes
{

    public static class ShapeExtension
    {
        public static Ray ToUnityRay(this Ray2d ray)
        {
            return new Ray(ray.Position.ToUnityVector3(), ray.Direction.ToUnityVector3());
        }

        public static Bounds ToUnityBounds(this Box3d box)
        {
            return new Bounds(box.Center.ToUnityVector3(), box.Size.ToUnityVector3());
        }

        public static Ray3d ToCGALRay3d(this Ray ray)
        {
            return new Ray3d(ray.origin.ToCGALPoint3d(), ray.direction.ToCGALVector3d());
        }

        public static Box3d ToCGALBox3d(this Bounds bounds)
        {
            return new Box3d(bounds.min.ToCGALPoint3d(), bounds.max.ToCGALPoint3d());
        }

    }

}
