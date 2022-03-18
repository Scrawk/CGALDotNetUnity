using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGALDotNetGeometry.Numerics
{

    public static class QuaternionExtension
    {

        public static Quaternion ToUnityQuaternion(this Quaternion3f quat)
        {
            return new Quaternion(quat.x, quat.y, quat.z, quat.w);
        }

        public static Quaternion ToUnityQuaternion(this Quaternion3d quat)
        {
            return new Quaternion((float)quat.x, (float)quat.y, (float)quat.z, (float)quat.w);
        }

        public static Quaternion3d ToCGALQuaternion3d(this Quaternion quat)
        {
            return new Quaternion3d(quat.x, quat.y, quat.z, quat.w);
        }

        public static Quaternion3f ToCGALQuaternion3f(this Quaternion quat)
        {
            return new Quaternion3f(quat.x, quat.y, quat.z, quat.w);
        }

    }

}
