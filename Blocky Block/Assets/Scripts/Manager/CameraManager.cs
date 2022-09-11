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
        [Space(10)]
        [Header("Move")]
        [SerializeField] float m_MoveSpeed = 5;
        [SerializeField] float m_DragPanSpeed = 0.07f;
        [Space(10)]
        [Header("Rotate")]
        [SerializeField] float m_RotateSpeed = 5f;
        [SerializeField] float m_RotatePanSpeed = 1;
        [SerializeField] float m_RotateYSpeed = 0.1f;
        [SerializeField] float m_MaxYRotation = 60;
        [SerializeField] float m_MinYRotation = 0f;
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
        private CinemachineComposer m_CMComposer;
        private int m_SquareMapSize;
        private Vector3 m_RealSize;
        private float m_InitY = 15;
        private Sequence zoomSequence;
        // Start is called before the first frame update
        void Start()
        {
            zoomSequence = DOTween.Sequence();
            m_CMTransposer = m_CMCam.GetCinemachineComponent<CinemachineTransposer>();
            m_CMComposer = m_CMCam.GetCinemachineComponent<CinemachineComposer>();

            GameEvents.SETUP_CAMERA += HandleSetupCamera;

            ToolEvents.ON_CURSOR_CHANGED += HandleCursorChanged;
            ToolEvents.ON_ZOOM_BUTTON_CLICKED += HandleZoomButtonClicked;
            ToolEvents.ON_RESET_BUTTON_CLICKED += HandleResetButtonClicked;
        }
        void Update()
        {
            
            
            switch (HandToolManager.Instance.CurrentCursor)
            {
                case Enums.CursorType.SELECTION:
                    HandleSelectionTool();
                    break;
                case Enums.CursorType.MOVE:
                    HandleMoveTool();
                    break;
                case Enums.CursorType.ROTATE:
                    HandleRotateTool();
                    break;
            }
        }
        void HandleSelectionTool()
        {
            #region MOVE
            if (Input.GetMouseButtonDown(2))
            {
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
                m_InputMove.z = moveDelta.y * m_DragPanSpeed;

                m_LastFramePosition = Input.mousePosition;
            }
            else
            {
                m_InputMove = Vector3.zero;
            }

            Vector3 moveDir = (m_CinemachinePivot.forward + m_CinemachinePivot.up * Mathf.Cos(m_CMCam.transform.eulerAngles.x)) * m_InputMove.z + m_CinemachinePivot.right * m_InputMove.x;
            // moveDir.Normalize();
            m_CinemachinePivot.position += -moveDir * m_MoveSpeed * Time.deltaTime;
            #endregion

            #region ROTATE
            if (Input.GetMouseButtonDown(1))
            {
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

            m_CinemachinePivot.eulerAngles += new Vector3(0, m_InputRotate.x * m_RotateSpeed * Time.deltaTime, 0);

            float rotateY = m_InputRotate.y * m_RotateYSpeed;
            m_CMTransposer.m_FollowOffset.y += rotateY;
            m_CMTransposer.m_FollowOffset.y = Mathf.Clamp(m_CMTransposer.m_FollowOffset.y, m_MinYRotation, m_MaxYRotation);
            #endregion
            
            #region ZOOM
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
        void HandleMoveTool()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.SetCursor(CursorManager.Instance.CursorData[CursorType.DRAGGING], Vector3.zero, CursorMode.Auto);
                m_DragPanMouseActive = true;
                m_LastFramePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                Cursor.SetCursor(CursorManager.Instance.CursorData[CursorType.MOVE], Vector3.zero, CursorMode.Auto);
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

            Vector3 moveDir = (m_CinemachinePivot.forward + m_CinemachinePivot.up * Mathf.Cos(Mathf.Abs(m_CMCam.transform.eulerAngles.x))) * m_InputMove.z + m_CinemachinePivot.right * m_InputMove.x;
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
                m_InputRotate.y = -rotateDelta.y * m_RotatePanSpeed;

                m_LastFrameRotation = Input.mousePosition;
            }
            else
            {
                m_InputRotate = Vector3.zero;
            }

            m_CinemachinePivot.eulerAngles += new Vector3(0, m_InputRotate.x * m_RotateSpeed * Time.deltaTime, 0);

            float rotateY = m_InputRotate.y * m_RotateYSpeed;
            m_CMTransposer.m_FollowOffset.y += rotateY;
            m_CMTransposer.m_FollowOffset.y = Mathf.Clamp(m_CMTransposer.m_FollowOffset.y, m_MinYRotation, m_MaxYRotation);
        }
        void OnDestroy()
        {
            GameEvents.SETUP_CAMERA -= HandleSetupCamera;

            ToolEvents.ON_ZOOM_BUTTON_CLICKED -= HandleZoomButtonClicked;
            ToolEvents.ON_RESET_BUTTON_CLICKED -= HandleResetButtonClicked;
            ToolEvents.ON_CURSOR_CHANGED += HandleCursorChanged;
        }
        void HandleCursorChanged(CursorType _type)
        {
            if (_type == CursorType.SELECTION)
            {
                m_DragPanMouseActive = false;
                m_RotatePanMouseActive = false;
            }
        }
        void HandleZoomButtonClicked(ZoomType _type)
        {
            int zoomFactor = _type == ZoomType.ZOOM_IN ? -1 : 1;
            float currentOrthosize = m_CMCam.m_Lens.OrthographicSize;
            currentOrthosize += zoomFactor * m_ZoomSpeed;

            DOTween.To(() => m_CMCam.m_Lens.OrthographicSize, value => m_CMCam.m_Lens.OrthographicSize = value, currentOrthosize, 1);
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
            DOTween.To(() => m_CMComposer.m_TrackedObjectOffset.x,
                       value => m_CMComposer.m_TrackedObjectOffset.x = value,
                       offset,
                       1);

            DOTween.To(() => m_CMTransposer.m_FollowOffset.x,
                       value => m_CMTransposer.m_FollowOffset.x = value,
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
            m_CMCam.m_Lens.OrthographicSize = GetPivotHeightAndCameraSize(_squareSize);

            m_CMComposer.m_TrackedObjectOffset.x = (_realSize.y - 1) / 2;
            m_CMTransposer.m_FollowOffset.x = (_realSize.y - 1) / 2;
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
