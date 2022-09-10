using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Tools;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using DG.Tweening;
using Cinemachine;

namespace BlockyBlock.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera m_CMCam;
        [SerializeField] float m_SizeBuffer = 1;
        [SerializeField] Transform m_CinemachinePivot;
        [Header("Move")]
        [SerializeField] float m_MoveSpeed = 1;
        [SerializeField] float m_DragPanSpeed = 1;
        [Space(10)]
        [Header("Rotate")]
        [SerializeField] float m_RotateSpeed = 50f;
        [SerializeField] float m_RotatePanSpeed = 1;
        [Space(10)]
        [Header("Zoom")]
        [SerializeField] float m_ZoomSpeed = 1;
        private bool m_RotatePanMouseActive;
        private Vector2 m_LastFrameRotation;
        private Vector3 m_InputRotate;
        private bool m_DragPanMouseActive;
        private Vector2 m_LastFramePosition;
        private Vector3 m_InputMove;
        // Start is called before the first frame update
        void Start()
        {
            GameEvents.SETUP_CAMERA += HandleSetupCamera;
            ToolEvents.ON_ZOOM_BUTTON_CLICKED += HandleZoomButtonClicked;
        }
        void Update()
        {
            switch (HandToolManager.Instance.CurrentCursor)
            {
                case Enums.CursorType.SELECTION:
                    break;
                case Enums.CursorType.MOVE:
                    HandleMoveTool();
                    break;
                case Enums.CursorType.ROTATE:
                    HandleRotateTool();
                    break;
            }
        }
        void HandleMoveTool()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_DragPanMouseActive = true;
                m_LastFramePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_DragPanMouseActive = false;
            }
            if (m_DragPanMouseActive)
            {
                Vector2 moveDelta = (Vector2)Input.mousePosition - m_LastFramePosition;
                
                m_InputMove.x = moveDelta.x * m_DragPanSpeed;
                m_InputMove.z = moveDelta.y * m_DragPanSpeed;

                m_LastFramePosition = Input.mousePosition;
            }
            else
            {
                m_InputMove = Vector3.zero;
            }

            Vector3 moveDir = m_CinemachinePivot.forward * m_InputMove.z + m_CinemachinePivot.right * m_InputMove.x;
            m_CinemachinePivot.position += -moveDir * m_MoveSpeed * Time.deltaTime;
        }
        void HandleRotateTool()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_RotatePanMouseActive = true;
                m_LastFrameRotation = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_RotatePanMouseActive = false;
            }
            if (m_RotatePanMouseActive)
            {
                Vector2 rotateDelta = (Vector2)Input.mousePosition - m_LastFrameRotation;

                m_InputRotate.x = rotateDelta.x * m_RotatePanSpeed;
                m_LastFrameRotation = Input.mousePosition;
            }
            else
            {
                m_InputRotate = Vector3.zero;
            }

            m_CinemachinePivot.eulerAngles += new Vector3(0, m_InputRotate.x * m_RotateSpeed * Time.deltaTime, 0);
        }
        void OnDestroy()
        {
            GameEvents.SETUP_CAMERA -= HandleSetupCamera;
            ToolEvents.ON_ZOOM_BUTTON_CLICKED -= HandleZoomButtonClicked;
        }
        void HandleZoomButtonClicked(ZoomType _type)
        {
            int zoomFactor = _type == ZoomType.ZOOM_IN ? -1 : 1;
            float currentOrthosize = m_CMCam.m_Lens.OrthographicSize;
            currentOrthosize += zoomFactor * m_ZoomSpeed;

            m_CMCam.m_Lens.OrthographicSize = currentOrthosize;
        }
        void HandleSetupCamera(int _squareSize, Vector2 _realSize)
        {
            m_CinemachinePivot.position = new Vector3((_realSize.x - 1) / 2, 0, (_realSize.y - 1) / 2);
            m_CMCam.m_Lens.OrthographicSize = GetPivotHeightAndCameraSize(_squareSize);
        }
        float GetPivotHeightAndCameraSize(int _ms)
        {
            float rs = ((2.0f * (float)_ms + 1.0f) * Mathf.Sqrt(2.0f)) / 2.0f;
            float camSize = (rs / (2 * Mathf.Sqrt(2.0f))) + m_SizeBuffer;
            return camSize;
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
