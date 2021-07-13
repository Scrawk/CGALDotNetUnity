using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Common.Unity.Cameras
{
    public class OrthoCameraController : MonoBehaviour
    {
        private class CameraState
        {
            public Vector3 position;
            public float size;
        }

        [SerializeField]
        private UnityEvent m_cameraMovedEvent = new UnityEvent();

        [SerializeField]
        private float m_positionSpeed = 10f;

        [SerializeField]
        private float m_positionLerpTime = 0.2f;

        [SerializeField]
        private float m_zoomSpeed = 10f;

        [SerializeField]
        private float m_zoomMin = 1.0f;

        [SerializeField]
        private float m_zoomMax = 100.0f;

        [SerializeField]
        private float m_zoomLerpTime = 0.2f;

        private Camera m_camera;

        private CameraState m_targetCameraState = new CameraState();

        private CameraState m_interpolatingCameraState = new CameraState();

        private bool m_moved;

        private void OnEnable()
        {
            m_camera = GetComponent<Camera>();

            m_targetCameraState.position = transform.position;
            m_targetCameraState.size = m_camera.orthographicSize;

            m_interpolatingCameraState.position = transform.position;
            m_interpolatingCameraState.size = m_camera.orthographicSize;
        }

        private void Update()
        {
            m_moved = false;

            var translation = GetTranslationDirection();
            var scroll = GetScrollAmount();

            m_targetCameraState.position += translation;
            m_targetCameraState.size += scroll;
            m_targetCameraState.size = Mathf.Clamp(m_targetCameraState.size, m_zoomMin, m_zoomMax);

            LerpTowards(m_interpolatingCameraState, m_targetCameraState);

            transform.position = m_interpolatingCameraState.position;
            m_camera.orthographicSize = m_interpolatingCameraState.size;

            if (m_moved)
                m_cameraMovedEvent.Invoke();
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }

        private Vector3 GetTranslationDirection()
        {
            Vector3 direction = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                m_moved = true;
                direction += Vector3.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                m_moved = true;
                direction += Vector3.down;
            }
            if (Input.GetKey(KeyCode.A))
            {
                m_moved = true;
                direction += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                m_moved = true;
                direction += Vector3.right;
            }

            float scale = 1;
            if (m_camera.orthographic)
                scale = m_camera.orthographicSize / 10.0f;

            return direction * Time.deltaTime * m_positionSpeed * scale;
        }

        private float GetScrollAmount()
        {
            float delta = Input.mouseScrollDelta.y;

            if (delta != 0)
                m_moved = true;

            float scale = 1;
            if (m_camera.orthographic)
                scale = m_camera.orthographicSize / 10.0f;

            return Input.mouseScrollDelta.y * Time.deltaTime * -m_zoomSpeed * scale;
        }

        private void LerpTowards(CameraState state, CameraState target)
        {
            var positionLerp = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / m_positionLerpTime) * Time.deltaTime);
            var zoomLerp = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / m_zoomLerpTime) * Time.deltaTime);

            state.position = Vector3.Lerp(state.position, target.position, positionLerp);
            state.size = Mathf.Lerp(state.size, target.size, zoomLerp);
        }


    }

}