using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 以 https://www.teambition.com/task/630efc1374b8c10040a8ade7 显示方式的tip
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _ATNPGGUIWndCommonItemToolTipCheckPosition<T> : _ATNPGGUIWndCommonItemToolTip<T>
        where T : NPGGUIMonoCommonToolTip
    {
        protected _ATNPGGUIWndCommonItemToolTipCheckPosition(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }
        
        public override void setPos(RectTransform _targetTransRoot, float _intervalX, float _intervalY)
        {
            if (wnd == null)
                return;

            //横向跟随不处理
            if (wnd.isHorizontalFollow)
            {
                base.setPos(_targetTransRoot, _intervalX, _intervalY);
                return;
            }

            //目标点的世界坐标
            Vector3 centerWorldPos = _targetTransRoot.transform.TransformPoint(_targetTransRoot.rect.center);
            //目标点的屏幕坐标
            Vector2 centerScreenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.fullCanvas.worldCamera, centerWorldPos);
            //目标点的UGUI坐标，这里计算出的uiPos是以中心为原点的坐标，即(0,0)是屏幕中心
            Vector2 uiPos;
            bool isInAdjustScreen = MainCameraMono.selfInstance.isAdjustScreenCanvasChildRect(_targetTransRoot, out Canvas inCanvas);
            RectTransform rectTrans = isInAdjustScreen && inCanvas != null ? (RectTransform)inCanvas.transform : Game.instance.mainCamera.uiRootRectTrans;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTrans,
                centerScreenPos,
                Game.instance.mainCamera.uiCamera,
                out uiPos);
            float showAreaWidth = isInAdjustScreen ? MainCameraMono.selfInstance.adjustScreenCanvasWidthUGUI : Game.instance.mainCamera.uiRootRectTrans.rect.width;
            float showAreaHeight = isInAdjustScreen ? MainCameraMono.selfInstance.adjustScreenCanvasHeightUGUI : Game.instance.mainCamera.uiRootRectTrans.rect.height;

            float selfWidth = getSelfWidth();//自身宽度
            float selfHeight = getSelfHeight();//自身高度
            bool ignoreTargetSize = false;//是否需要无视点击载体的大小，以配置优先
            //只有在下方显示时需要做左右处理
            //只有忽略点击载体大小的情况下才需要特殊处理
            if (null != wnd && wnd.ignoreClickRectSize && showAreaHeight / 2 > Mathf.Abs(uiPos.y - selfHeight - _intervalY))
            {
                //优先级：右 > 左 > 正下方
                if (showAreaWidth / 2 > uiPos.x + selfWidth + _intervalX)
                {
                    _intervalX += selfWidth / 2;
                    ignoreTargetSize = true;
                }
                else if (-showAreaWidth / 2 < uiPos.x - selfWidth - _intervalX)
                {
                    _intervalX -= selfWidth / 2;
                    ignoreTargetSize = true;
                }
            }
            base.setPos(_targetTransRoot, _intervalX, _intervalY, wnd.isDefaultDownLeft, wnd.ignoreClickRectSize && ignoreTargetSize);
        }
    }
}