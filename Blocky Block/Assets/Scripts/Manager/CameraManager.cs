using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Configurations;

namespace BlockyBlock.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] Camera m_MainCamera;
        [SerializeField] Transform m_CameraPivot;
        [SerializeField] CameraConfig m_CameraConfig;
        [SerializeField] float m_SizeBuffer = 1;
        Vector3 IsometricCameraRotation = new Vector3(45, 45, 0);
        Vector3 IsometricCameraInitPosition = new Vector3(0, 0, -10);
        void Awake()
        {
            m_CameraPivot.eulerAngles = IsometricCameraRotation;
            m_MainCamera.transform.position = IsometricCameraInitPosition;
        }
        // Start is called before the first frame update
        void Start()
        {
            GameEvents.SETUP_CAMERA += HandleSetupCamera;
        }
        void OnDestroy()
        {
            GameEvents.SETUP_CAMERA -= HandleSetupCamera;
        }
        void HandleSetupCamera(int _squareSize)
        {
            Vector3 heightNsize = GetPivotHeightAndCameraSize(_squareSize);

            Vector2 cameraPivotXZ = m_CameraConfig.CameraDatas[_squareSize];
            m_MainCamera.orthographicSize = heightNsize.y;
            m_CameraPivot.position = new Vector3(cameraPivotXZ.x, heightNsize.x, cameraPivotXZ.y);
            m_MainCamera.transform.localPosition = IsometricCameraInitPosition;
        }
        Vector3 GetPivotHeightAndCameraSize(int _ms)
        {
            float rs = ((2.0f * (float)_ms + 1.0f) * Mathf.Sqrt(2.0f)) / 2.0f;
            float camHeight = (rs / 2.0f) - Mathf.Sqrt(2.0f);
            float camSize = (rs / (2 * Mathf.Sqrt(2.0f))) + m_SizeBuffer;
            float camHorizontal = ((float)_ms * Mathf.Sqrt(2.0f)) / 2.0f;
            return new Vector3(camHeight, camSize, camHorizontal);
        }
        void OnDrawGizmos()
        {
            float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
            float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(verticalWidthSeen, verticalHeightSeen, 0));
        }
    }
}
