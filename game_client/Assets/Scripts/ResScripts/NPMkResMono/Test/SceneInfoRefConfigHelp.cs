
using UnityEngine;

namespace GOE
{
    [ExecuteInEditMode]
    public class SceneInfoRefConfigHelp : MonoBehaviour
    {
        [ALHeader("这个场景的相机")]
        [ALHeader("用黄色表示相机看到的范围", "#ffff00")]
        public Camera previewCamera;
        [ALHeader("这个场景规划的位置")]
        public Vector3 cornerPos;
        [ALHeader("这个场景采用什么拖动方式")]
        public ESceneMoveType moveType;
        [ALHeader("这个场景的可视范围")]
        [ALHeader("用紫色表示", "#ff00ff")]
        public Rect dragMoveRect;
        [ALHeader("这个场景的柔和边缘尺寸")]
        [ALHeader("用浅蓝（绿）色表示", "#00ffff")]
        public Vector2 softBorderSize;

        [ALHeader("配置建议")][TextArea(11, 20)][ReadOnly]
        public string configSuggest;

        public void Update()
        {
            if (previewCamera == null)
            {
                configSuggest = "先拖一个 previewCamera 吧";
                return;
            }

            _ALogicPlane2DPosGetter getter = null;
            switch (moveType)
            {
                case ESceneMoveType.XY:
                    getter = new LogicPlane2DPosGetterXY(cornerPos);
                    break;
                case ESceneMoveType.XZ:
                    getter = new LogicPlane2DPosGetterXZ(cornerPos);
                    break;
                case ESceneMoveType.YZ:
                    getter = new LogicPlane2DPosGetterZY(cornerPos);
                    break;
            }

            if (getter == null)
            {
                configSuggest = "没有办法生成一个逻辑坐标获取器，通知程序查一下";
                return;
            }
            
            // 计算相机当前在移动平面上的显示范围
            if (!getter.intersectionWorldPointWithWorldRay(previewCamera.ViewportPointToRay(new Vector3(0, 0)), out Vector3 leftDown) ||
                !getter.intersectionWorldPointWithWorldRay(previewCamera.ViewportPointToRay(new Vector3(0, 1)), out Vector3 leftUp) ||
                !getter.intersectionWorldPointWithWorldRay(previewCamera.ViewportPointToRay(new Vector3(1, 1)), out Vector3 rightUp) ||
                !getter.intersectionWorldPointWithWorldRay(previewCamera.ViewportPointToRay(new Vector3(1, 0)), out Vector3 rightDown))
            {
                configSuggest = "相机当前的朝向和所选的 moveType 无法相交";
                return;
            }
            Vector3[] cameraViewRectPoints = new Vector3[] { leftDown, leftUp, rightUp, rightDown };

            // 绘制拖动可视范围
            Vector3[] dragMoveRectPoints = new Vector3[] { cornerPos, cornerPos, cornerPos, cornerPos };
            // 左下
            getter.setLogicXPos(ref dragMoveRectPoints[0], dragMoveRect.xMin);
            getter.setLogicYPos(ref dragMoveRectPoints[0], dragMoveRect.yMin);
            // 左上
            getter.setLogicXPos(ref dragMoveRectPoints[1], dragMoveRect.xMin);
            getter.setLogicYPos(ref dragMoveRectPoints[1], dragMoveRect.yMax);
            // 右上
            getter.setLogicXPos(ref dragMoveRectPoints[2], dragMoveRect.xMax);
            getter.setLogicYPos(ref dragMoveRectPoints[2], dragMoveRect.yMax);
            // 右下
            getter.setLogicXPos(ref dragMoveRectPoints[3], dragMoveRect.xMax);
            getter.setLogicYPos(ref dragMoveRectPoints[3], dragMoveRect.yMin);

            // 限制一下输入的值
            Vector2 cleanedSoftBorderSize = new Vector2(Mathf.Max(softBorderSize.x, 0), Mathf.Max(softBorderSize.y, 0));
            // 绘制拖动缓冲边缘
            Rect softBorderRect = dragMoveRect;
            softBorderRect.center -= cleanedSoftBorderSize;
            softBorderRect.width += cleanedSoftBorderSize.x * 2;
            softBorderRect.height += cleanedSoftBorderSize.y * 2;
            Vector3[] softBorderPoints = new Vector3[] { cornerPos, cornerPos, cornerPos, cornerPos };
            // 左下
            getter.setLogicXPos(ref softBorderPoints[0], softBorderRect.xMin);
            getter.setLogicYPos(ref softBorderPoints[0], softBorderRect.yMin);
            // 左上
            getter.setLogicXPos(ref softBorderPoints[1], softBorderRect.xMin);
            getter.setLogicYPos(ref softBorderPoints[1], softBorderRect.yMax);
            // 右上
            getter.setLogicXPos(ref softBorderPoints[2], softBorderRect.xMax);
            getter.setLogicYPos(ref softBorderPoints[2], softBorderRect.yMax);
            // 右下
            getter.setLogicXPos(ref softBorderPoints[3], softBorderRect.xMax);
            getter.setLogicYPos(ref softBorderPoints[3], softBorderRect.yMin);

            // 绘制
            DebugPlus.DrawPath(softBorderPoints, true, Color.cyan);
            DebugPlus.DrawPath(dragMoveRectPoints, true, Color.magenta);
            DebugPlus.DrawPath(cameraViewRectPoints, true, Color.yellow);

            // 如果 size 为 0 ，修正一个拖拽范围的配置，以便配置好的相机初始位置不会因为拖拽范围而发生改变
            Rect fixedDragMoveRect = dragMoveRect;
            if (fixedDragMoveRect is { width: <= 0, height: <= 0 })
            {
                fixedDragMoveRect.size = Vector2.zero;
                getter.intersectionWorldPointWithWorldRay(previewCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f)), out Vector3 centerPos);
                fixedDragMoveRect.position = getter.getLogicPos(centerPos);
            }
            Vector3 cameraPosition = previewCamera.transform.position - cornerPos;
            Vector3 cameraFocusPosition = previewCamera.transform.position + previewCamera.transform.forward * 10 - cornerPos;
            string isOrthographic = previewCamera.orthographic ? "TRUE" : "FALSE";
            configSuggest = "找到配置表中的对应字段，把后面的数值复制过去即可：\n" +
                            $"corner_pos : {cornerPos.x}:{cornerPos.y}:{cornerPos.z}\n" +
                            $"cameraPosition : {cameraPosition.x}:{cameraPosition.y}:{cameraPosition.z}\n" +
                            $"cameraFocusPosition : {cameraFocusPosition.x}:{cameraFocusPosition.y}:{cameraFocusPosition.z}\n" +
                            $"cameraFieldOfView : {previewCamera.fieldOfView}\n" +
                            $"isOrthographic : {isOrthographic}\n" +
                            $"orthographicSize : {previewCamera.orthographicSize}\n" +
                            $"clippingNear : {previewCamera.nearClipPlane}\n" +
                            $"clippingFar : {previewCamera.farClipPlane}\n" +
                            $"map_border_scale_size : {cleanedSoftBorderSize.x}:{cleanedSoftBorderSize.y}\n" +
                            $"map_drag_rect : {fixedDragMoveRect.xMin}:{fixedDragMoveRect.yMin}:{fixedDragMoveRect.width}:{fixedDragMoveRect.height}\n" +
                            $"move_type : {moveType.ToString()}";
        }
    }
}