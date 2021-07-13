using System;
using System.Collections.Generic;
using UnityEngine;


namespace Common.Unity.Drawing
{
    [Flags]
    public enum DEBUG_DRAW_MODE
    {
        ON_DRAW_GIZMO = 1 << 0,
        ON_DRAW_GIZMO_SELECTED = 1 << 1,
        ON_RENDER_OBJECT = 1 << 2
    }

}
