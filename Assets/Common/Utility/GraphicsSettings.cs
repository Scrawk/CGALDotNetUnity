using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Unity.Utility
{
    [ExecuteInEditMode]
    [AddComponentMenu("Common/Utility/Graphics Settings")]
    public class GraphicsSettings : MonoBehaviour
    {

        public int vSyncCount = 1;

        public int targetFrameRate = 60;

        private void Update()
        {
            QualitySettings.vSyncCount = vSyncCount;
            Application.targetFrameRate = targetFrameRate;
        }

    }

}
