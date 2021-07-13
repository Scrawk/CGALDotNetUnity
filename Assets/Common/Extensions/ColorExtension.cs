using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Colors;
using Common.Core.Numerics;

namespace UnityEngine
{

    public static class ColorExtension
    {

        public static Color ToColor(this ColorRGBA c)
        {
            return new Color(c.r, c.g, c.b, c.a);
        }

        public static Color ToColor(this ColorRGB c)
        {
            return new Color(c.r, c.g, c.b, 1.0f);
        }

        public static ColorRGB ToColorRGB(this Color c)
        {
            return new ColorRGB(c.r, c.g, c.b);
        }

        public static ColorRGBA ToColorRGBA(this Color c)
        {
            return new ColorRGBA(c.r, c.g, c.b, c.a);
        }

        public static Color ToColor(this Vector3f v)
        {
            return new Color(v.x, v.y, v.z);
        }

        public static Color ToColor(this Vector3d v)
        {
            return new Color((float)v.x, (float)v.y, (float)v.z);
        }

    }

}
