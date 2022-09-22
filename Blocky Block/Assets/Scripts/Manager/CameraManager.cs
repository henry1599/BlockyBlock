using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Tools;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using DG.Tweening;
using Cinemachine;
using Helpers;

namespace BlockyBlock.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera m_CMCam;
        [SerializeField] float m_SizeBuffer = 1;
        [SerializeField] CinemachineCameraOffset m_CMOffset;
        [SerializeField] Transform m_CinemachinePivot;
        [Space(10)]
        [Header("Move")]
        [SerializeField] float m_MoveSpeed = 5;
        [SerializeField] float m_DragPanSpeed = 0.07f;
        [Space(10)]
        [Header("Rotate")]
        [SerializeField] float m_RotateSpeed = 5f;
        [SerializeField] float m_RotatePanSpeed = 1;
        [SerializeField] float m_RotateYSpeed = 0.1f;
        [SerializeField] float m_MaxXRotation = 30;
        [SerializeField] float m_MinXRotation = -55f;
        [Space(10)]
        [Header("Zoom")]
        [SerializeField] float m_ZoomSpeed = 1;
        [SerializeField] float m_ZoomScrollBuffer = 1;
        [SerializeField] float m_ZoomInterval = 0.5f;
        private bool m_RotatePanMouseActive;
        private Vector2 m_LastFrameRotation;
        private Vector3 m_InputRotate;
        private bool m_DragPanMouseActive;
        private Vector2 m_LastFramePosition;
        private Vector3 m_InputMove;
        private CinemachineTransposer m_CMTransposer;
        private int m_SquareMapSize;
        private Vector3 m_RealSize;
        private float m_InitY = 15;
        private Sequence zoomSequence;
        // Start is called before the first frame update
        void Start()
        {
            zoomSequence = DOTween.Sequence();
            m_CMTransposer = m_CMCam.GetCinemachineComponent<CinemachineTransposer>();

            GameEvents.SETUP_CAMERA += HandleSetupCamera;

            ToolEvents.ON_RESET_BUTTON_CLICKED += HandleResetButtonClicked;
        }
        void Update()
        {
            HandleSelectionTool();
        }
        void HandleSelectionTool()
        {
            #region MOVE
            if (Input.GetMouseButtonDown(2))
            {
                m_RotatePanMouseActive = false;
                Cursor.SetCursor(CursorManager.Instance.CursorData[CursorType.DRAGGING], Vector3.zero, CursorMode.Auto);
                m_DragPanMouseActive = true;
                m_LastFramePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(2))
            {
                Cursor.SetCursor(CursorManager.Instance.CursorData[CursorType.SELECTION], Vector3.zero, CursorMode.Auto);
                m_DragPanMouseActive = false;
            }
            if (m_DragPanMouseActive)
            {
                Vector2 moveDelta = (Vector2)Input.mousePosition - m_LastFramePosition;
                
                m_InputMove.x = moveDelta.x * m_DragPanSpeed;
                m_InputMove.y = moveDelta.y * m_DragPanSpeed;
                m_InputMove.z = 0;

                m_LastFramePosition = Input.mousePosition;
            }
            else
            {
                m_InputMove = Vector3.zero;
            }

            // Vector3 moveDir = (m_CinemachinePivot.forward) * m_InputMove.z + m_CinemachinePivot.right * m_InputMove.x;
            // m_CMOffset.m_Offset += new Vector3(-moveDir.x * m_MoveSpeed * Time.deltaTime, -moveDir.z * m_MoveSpeed * Time.deltaTime, 0);
            m_CMOffset.m_Offset += -m_InputMove * m_MoveSpeed * Time.deltaTime;
            #endregion

            #region ROTATE
            if (Input.GetMouseButtonDown(1))
            {
                m_DragPanMouseActive = false;
                Cursor.SetCursor(CursorManager.Instance.CursorData[CursorType.ROTATE], Vector3.zero, CursorMode.Auto);
                m_RotatePanMouseActive = true;
                m_LastFrameRotation = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(1))
            {
                Cursor.SetCursor(CursorManager.Instance.CursorData[CursorType.SELECTION], Vector3.zero, CursorMode.Auto);
                m_RotatePanMouseActive = false;
            }
            if (m_RotatePanMouseActive)
            {
                Vector2 rotateDelta = (Vector2)Input.mousePosition - m_LastFrameRotation;

                m_InputRotate.x = rotateDelta.x * m_RotatePanSpeed;
                m_InputRotate.y = -rotateDelta.y * m_RotatePanSpeed;

                m_LastFrameRotation = Input.mousePosition;
            }
            else
            {
                m_InputRotate = Vector3.zero;
            }

            Vector3 angle = m_CinemachinePivot.eulerAngles;
            angle += new Vector3(m_InputRotate.y * m_RotateSpeed * Time.deltaTime, m_InputRotate.x * m_RotateSpeed * Time.deltaTime, 0);
            
            m_CinemachinePivot.localRotation = Quaternion.Euler(angle);
            #endregion
            
            #region ZOOM
            if (Helper.isOverUI())
            {
                return;
            }
            int zoomFactor = Input.mouseScrollDelta.y > 0 ? -1 : Input.mouseScrollDelta.y < 0 ? 1 : 0;
            if (zoomFactor == 0)
            {
                return;
            }
            float currentOrthosize = m_CMCam.m_Lens.OrthographicSize;
            currentOrthosize += zoomFactor * m_ZoomSpeed * m_ZoomScrollBuffer;

            Tween zoomTween = DOTween.To(() => m_CMCam.m_Lens.OrthographicSize, value => m_CMCam.m_Lens.OrthographicSize = value, currentOrthosize, m_ZoomInterval);
            zoomSequence.Append(zoomTween);
            #endregion
        }
        void OnDestroy()
        {
            GameEvents.SETUP_CAMERA -= HandleSetupCamera;
            ToolEvents.ON_RESET_BUTTON_CLICKED -= HandleResetButtonClicked;
        }
        void HandleResetButtonClicked()
        {
            Vector3 cameraPivot = new Vector3((m_RealSize.y - 1) / 2, 0, (m_RealSize.x - 1) / 2);
            DOTween.To(() => m_CinemachinePivot.position,
                       value => m_CinemachinePivot.position = value,
                       cameraPivot,
                       1);

            float orthoSize = GetPivotHeightAndCameraSize(m_SquareMapSize);
            DOTween.To(() => m_CMCam.m_Lens.OrthographicSize,
                       value => m_CMCam.m_Lens.OrthographicSize = value,
                       orthoSize,
                       1);

            float offset = (m_RealSize.y - 1) / 2;
            DOTween.To(() => m_CMOffset.m_Offset.x,
                       value => m_CMOffset.m_Offset.x = value,
                       offset,
                       1);
                       
            DOTween.To(() => m_CMTransposer.m_FollowOffset.y,
                       value => m_CMTransposer.m_FollowOffset.y = value,
                       m_InitY,
                       1);

            m_CinemachinePivot
                .DORotate(
                    Vector3.zero,
                    1
                )
                .SetEase(Ease.InOutSine);
        }
        void HandleSetupCamera(int _squareSize, Vector2 _realSize)
        {
            m_SquareMapSize = _squareSize;
            m_RealSize = _realSize;

            m_CinemachinePivot.position = new Vector3((_realSize.y - 1) / 2, 0, (_realSize.x - 1) / 2);
            m_CinemachinePivot.eulerAngles = Vector3.zero;
            m_CMCam.m_Lens.OrthographicSize = GetPivotHeightAndCameraSize(_squareSize);

            m_CMOffset.m_Offset.x = (_realSize.y - 1) / 2;
        }
        float GetPivotHeightAndCameraSize(int _ms)
        {
            float rs = ((2.0f * (float)_ms + 1.0f) * Mathf.Sqrt(2.0f)) / 2.0f;
            float camSize = (rs / (2 * Mathf.Sqrt(2.0f))) + m_SizeBuffer;
            return camSize;
        }
    }
}
