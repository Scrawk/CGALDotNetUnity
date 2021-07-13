using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Numerics;

namespace UnityEngine
{

    public static class QuaternionExtension
    {

        public static Quaternion ToQuaternion(this Quaternion3f q)
        {
            return new Quaternion(q.x, q.y, q.z, q.w);
        }

        public static Quaternion ToQuaternion(this Quaternion3d q)
        {
            return new Quaternion((float)q.x, (float)q.y, (float)q.z, (float)q.w);
        }

        public static Quaternion3f ToQuaternion3f(this Quaternion q)
        {
            return new Quaternion3f(q.x, q.y, q.z, q.w);
        }

        public static Quaternion3d ToQuaternion3d(this Quaternion q)
        {
            return new Quaternion3d(q.x, q.y, q.z, q.w);
        }

    }

}
