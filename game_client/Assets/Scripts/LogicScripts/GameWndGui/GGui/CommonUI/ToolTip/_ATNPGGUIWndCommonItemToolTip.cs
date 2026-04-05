using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GOE
{
    /// <summary>
    /// 通用的提示跟随子窗口基类，可以用于各种小icon点击长按查看说明
    /// </summary>
    public abstract class _ATNPGGUIWndCommonItemToolTip<T> : _ANPGGUIBasicWnd<T>
        where T : NPGGUIMonoCommonToolTip
    {
        //跟随目标的物体
        private RectTransform _m_targetTransRoot;

        //额外X偏移间距
        private float _m_intervalX;
        //额外Y偏移间距
        private float _m_intervalY;

        private string _m_sAssetPath;
        private string _m_sAssetName;

        public _ATNPGGUIWndCommonItemToolTip(string _assetPath, string _assetName) : base(EALUIWndLayer.NOTICE)
        {
            _m_sAssetPath = _assetPath;
            _m_sAssetName = _assetName;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sAssetName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {

        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickClose(GameObject _gameObject)
        {
            _onClose();
        }


        protected abstract void _onClose();

        /// <summary>
        /// 获取自身高度
        /// 简单对象是固定高度的，直接自身prefab高度
        /// </summary>
        /// <returns></returns>
        protected virtual float getSelfHeight()
        {
            if (null == rectTransform)
                return 0;

            return rectTransform.rect.height;
        }
        /// <summary>
        /// 获取自身宽度
        /// 简单对象是固定高度的，直接自身prefab高度
        /// </summary>
        /// <returns></returns>
        protected virtual float getSelfWidth()
        {
            if (null == rectTransform)
                return 0;

            return rectTransform.rect.width;
        }


        public virtual void setPos(RectTransform _targetTransRoot, float _interval)
        {
            if (null == _targetTransRoot || null == _targetTransRoot.transform)
                return;

            if (wnd.isHorizontalFollow) // 横向跟随
            {
                _m_intervalX = _interval;
                _m_intervalY = 0;
            }
            else
            {
                _m_intervalY = _interval;
                _m_intervalX = 0;
            }

            setPos(_targetTransRoot, _m_intervalX, _m_intervalY);
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        public virtual void setPos(RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            if(wnd == null)
                return;
            
            setPos(_targetTransRoot, _intervalX, _intervalY, wnd.isDefaultDownLeft, wnd.ignoreClickRectSize);
        }

        /// <summary>
        /// /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="_targetTransRoot"></param>
        /// <param name="_intervalX"></param>
        /// <param name="_intervalY"></param>
        /// <param name="_isDefaultDownLeft">是否向左/下跟随</param>
        /// <param name="_ignoreTargetSize">是否无视点击目标的大小</param>
        public virtual void setPos(RectTransform _targetTransRoot, float _intervalX, float _intervalY,bool _isDefaultDownLeft,bool _ignoreTargetSize)
        {
            if (null == _targetTransRoot || null == _targetTransRoot.transform)
                return;

            _m_targetTransRoot = _targetTransRoot;
            _m_intervalX = _intervalX;
            _m_intervalY = _intervalY;

            //目标点的世界坐标
            Vector3 centerWorldPos = _m_targetTransRoot.transform.TransformPoint(_m_targetTransRoot.rect.center);
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

            bool isUp = false;
            bool isLeft = false;
            Vector2 tempPos = uiPos;

            //跟随GO的高度
            float followHeight = 0;
            //跟随GO的宽度
            float followWidth = 0;
            if (wnd != null && wnd.goFollow != null)
            {
                RectTransform followRectTransform = (RectTransform)wnd.goFollow.transform;
                if (followRectTransform != null)
                {
                    followHeight = followRectTransform.rect.height;
                    followWidth = followRectTransform.rect.width;
                }
            }

            //判断是不是在目标上面显示，默认在目标上面显示
            if (wnd.isHorizontalFollow) // 横向跟随
            {
                float tragetTransSize = _ignoreTargetSize ? 0 : _m_targetTransRoot.rect.width;
                //公式 = 目标坐标 + 目标宽度一半 + 额外偏移量 + 自身宽度 + 跟随GO的宽度
                if (!_isDefaultDownLeft)
                {
                    //目标右侧足够显示，向右跟随
                    if (showAreaWidth / 2 > uiPos.x + getSelfWidth() + _m_intervalX + tragetTransSize / 2 + followWidth)
                    {
                        tempPos.x = tempPos.x + _m_intervalX + tragetTransSize / 2 + getSelfWidth() * rectTransform.pivot.x + followWidth;
                    }
                    //否则向左跟随
                    else
                    {
                        isLeft = true;
                        tempPos.x = tempPos.x - _m_intervalX - tragetTransSize / 2 - getSelfWidth() * (1 - rectTransform.pivot.x) - followWidth;
                    }
                }
                else
                {
                    //目标左侧足够显示，向左跟随
                    if (-showAreaWidth / 2 < uiPos.x - getSelfWidth() - _m_intervalX - tragetTransSize / 2 - followWidth)
                    {
                        isLeft = true;
                        tempPos.x = tempPos.x - _m_intervalX - tragetTransSize / 2 - getSelfWidth() * (1 - rectTransform.pivot.x) - followWidth;
                    }
                    //否则向右跟随
                    else
                    {
                        tempPos.x = tempPos.x + _m_intervalX + tragetTransSize / 2 + getSelfWidth() * rectTransform.pivot.x + followWidth;
                    }
                }

                //设置Y偏移
                tempPos.y = tempPos.y + _m_intervalY;

                //修正坐标
                if (tempPos.y < 0)
                {
                    if (tempPos.y - rectTransform.rect.height / 2 < -showAreaHeight / 2f)
                        tempPos.y = rectTransform.rect.height / 2 - showAreaHeight / 2f;
                }
                else
                {
                    if (tempPos.y + rectTransform.rect.height / 2 > showAreaHeight / 2f)
                        tempPos.y = showAreaHeight / 2f - rectTransform.rect.height / 2f;
                }
                if (tempPos.x < 0)
                {
                    if (tempPos.x - rectTransform.rect.width / 2 < -showAreaWidth / 2f)
                        tempPos.x = rectTransform.rect.width / 2 - showAreaWidth / 2f;

                }
                else
                {
                    if (tempPos.x + rectTransform.rect.width / 2 > showAreaWidth / 2f)
                        tempPos.x = showAreaWidth / 2f - rectTransform.rect.width / 2;
                }
            }
            else
            {
                float tragetTransSize = _ignoreTargetSize ? 0 : _m_targetTransRoot.rect.height;

                //公式 = 目标坐标 + 目标高度一半 + 额外偏移量 + 自身高度 + 跟随GO的高度
                if (!_isDefaultDownLeft)
                {
                    //目标上方足够显示，向上跟随
                    if (showAreaHeight / 2 > uiPos.y + getSelfHeight() + _m_intervalY + tragetTransSize / 2 + followHeight)
                    {
                        isUp = true;
                        tempPos.y = tempPos.y + _m_intervalY + tragetTransSize / 2 + getSelfHeight() * rectTransform.pivot.y + followHeight;
                    }
                    //否则向下跟随
                    else
                    {
                        tempPos.y = tempPos.y - _m_intervalY - tragetTransSize / 2 - getSelfHeight() * (1 - rectTransform.pivot.y) - followHeight;
                    }
                }
                else
                {
                    //目标下方足够显示，向下跟随
                    if (-showAreaHeight / 2 < uiPos.y - getSelfHeight() - _m_intervalY - tragetTransSize / 2 - followHeight)
                    {
                        tempPos.y = tempPos.y - _m_intervalY - tragetTransSize / 2 - getSelfHeight() * (1 - rectTransform.pivot.y) - followHeight;
                    }
                    //否则向上跟随
                    else
                    {
                        isUp = true;
                        tempPos.y = tempPos.y + _m_intervalY + tragetTransSize / 2 + getSelfHeight() * rectTransform.pivot.y + followHeight;
                    }
                }

                //设置X偏移
                tempPos.x = tempPos.x + _m_intervalX;

                //修正坐标
                if (tempPos.y < 0)
                {
                    if (tempPos.y - rectTransform.rect.height / 2 < -showAreaHeight / 2f)
                        tempPos.y = rectTransform.rect.height / 2 - showAreaHeight / 2f;
                }
                else
                {
                    if (tempPos.y + rectTransform.rect.height / 2 > showAreaHeight / 2f)
                        tempPos.y = showAreaHeight / 2f - rectTransform.rect.height / 2f;
                }
                if (tempPos.x < 0)
                {
                    if (tempPos.x - rectTransform.rect.width / 2 < -showAreaWidth / 2f)
                        tempPos.x = rectTransform.rect.width / 2 - showAreaWidth / 2f;
                }
                else
                {
                    if (tempPos.x + rectTransform.rect.width / 2 > showAreaWidth / 2f)
                        tempPos.x = showAreaWidth / 2f - rectTransform.rect.width / 2;
                }
            }
            ALUGUICommon.setUIPos(rectTransform, tempPos);

            //设置跟随go的指针的位置
            _setFollowPointPos(centerWorldPos, isUp, isLeft);
        }
        
        /// <summary>
        /// 设置跟随go的指针的位置
        /// </summary>
        public void _setFollowPointPos(Vector3 _centerWorldPos,bool _isUp, bool _isLeft)
        {
            if (wnd == null || wnd.goFollow == null || _m_targetTransRoot == null || rectTransform == null)
                return;

            RectTransform followRectTransform = (RectTransform) wnd.goFollow.transform;
            if (followRectTransform == null)
                return;

            // Scale会影响到计算，把scale设回来再运算
            Vector3 originScale = wnd.transform.localScale;
            wnd.transform.localScale = Vector3.one;
            Vector3 tempPos = wnd.goFollow.transform.position;

            //先设置跟随点需要跟随的方向的坐标位置
            if (wnd.isHorizontalFollow)
            {
                tempPos.y = _centerWorldPos.y;
                wnd.goFollow.transform.position = tempPos;
            }
            else
            {
                tempPos.x = _centerWorldPos.x;
                wnd.goFollow.transform.position = tempPos;
            }
            Vector3 tempLocalPos = wnd.goFollow.transform.localPosition;

            //设置依附到tooltip的边的位置
            if (wnd.isHorizontalFollow)
            {
                //横向跟随
                if (_isLeft)
                {
                    tempLocalPos.x = rectTransform.rect.width / 2 + followRectTransform.rect.width * followRectTransform.pivot.x;
                    followRectTransform.eulerAngles = new Vector3(0, 0, 270);
                }
                else
                {
                    tempLocalPos.x = -rectTransform.rect.width / 2 - followRectTransform.rect.width * followRectTransform.pivot.x;
                    followRectTransform.eulerAngles = new Vector3(0, 0, 90);
                }
            }
            else
            {
                //竖向跟随
                if (_isUp)
                {
                    tempLocalPos.y = -rectTransform.rect.height / 2 - followRectTransform.rect.height * followRectTransform.pivot.y;
                    followRectTransform.eulerAngles = new Vector3(0, 0, 180);
                }
                else
                {
                    tempLocalPos.y = rectTransform.rect.height / 2 + followRectTransform.rect.height * followRectTransform.pivot.y;
                    followRectTransform.eulerAngles = new Vector3(0, 0, 0);
                }
            }

            wnd.goFollow.transform.localPosition = tempLocalPos;
            wnd.transform.localScale = originScale;
        }
    }

}