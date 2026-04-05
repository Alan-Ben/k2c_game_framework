using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 截图界面
    /// </summary>
    public abstract class _AGGUIWndScreenShot<T_MONO> : _ANPGGUIBasicWnd<T_MONO>
    where T_MONO : _AGGUIMonoScreenShot
    {

        protected _AGGUIWndScreenShot(EALUIWndLayer _layer) : base(_layer)
        {
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            ALUGUICommon.uncombineBtnClick(wnd.btnBack, _onBtnBackClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnShot, _onBtnShareClicked);
            
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnBack, _onBtnBackClicked);
            ALUGUICommon.combineBtnClick(wnd.btnShot, _onBtnShareClicked);
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            _onRefreshWndEx();
        }

        protected abstract void _onRefreshWndEx();

        private void _onBtnShareClicked(GameObject obj)
        {
            if (null == wnd)
            {
                ALLog.Error("wnd is null");
                return;
            }
            
            byte[] _shareImageBytes = null;
            Vector3[] corners = new Vector3[4];
            wnd.showRectTransform.GetWorldCorners(corners);
            Vector3 bottomLeft = corners[0];
            Vector3 topRight = corners[2];

            Vector2 bl =  Game.instance.mainCamera.uiCamera.WorldToViewportPoint(bottomLeft);
            Vector2 tr = Game.instance.mainCamera.uiCamera.WorldToViewportPoint(topRight);
            
            Vector4 viewClip = new Vector4(tr.x - bl.x, tr.y - bl.y, bl.x, bl.y);
            
            Rect returnRect = _getShotRect(wnd.showClothesImageSize.x, wnd.showClothesImageSize.y);
            Vector2 shotSize = new Vector2(returnRect.x * 2 + returnRect.width, returnRect.y * 2 + returnRect.height);
            
            ScreenShotUtil.getMainCameraRenderTexture(shotSize, viewClip, (_renderTexture) =>
            {
                Vector2Int shareRTSize = new Vector2Int(Mathf.CeilToInt(wnd.shareClothesImageSize.x), Mathf.CeilToInt(wnd.shareClothesImageSize.y));

                _onScreenShot(_renderTexture, shareRTSize, ScreenShotUtil.releaseMainCameraRenderTexture);
            });
          
        }

        /// <summary>
        /// 获取完整的显示尺寸,
        /// </summary>
        /// <param name="_showWidth"></param>
        /// <param name="_showHeight"></param>
        /// <returns></returns>
        private Vector2 _getShowMaxSize(float _showWidth, float _showHeight)
        {
            float fullWidth = MainCameraMono.selfInstance.fullCanvasScaler.referenceResolution.x;
            float fullHeight = MainCameraMono.selfInstance.fullCanvasScaler.referenceResolution.y;
            float widthScale =  fullWidth / _showWidth;
            float heightScale = fullHeight / _showHeight;
            float targetScale = widthScale < heightScale ? widthScale : heightScale;
            float rectSpaceScale = wnd.showRectSpaceScale;//距离边框的最小比例
            rectSpaceScale *= 2;
            float fullShowWidth = targetScale * _showWidth * (1 - rectSpaceScale);
            float fullShowHeight = targetScale * _showHeight * (1 - rectSpaceScale);
            return new Vector2(fullShowWidth, fullShowHeight);
        }
        
        /// <summary>
        /// 获得截图的区域
        /// </summary>
        /// <param name="_showWidth"></param>
        /// <param name="_showHeight"></param>
        /// <returns></returns>
        private Rect _getShotRect(float _showWidth, float _showHeight)
        {
            float fullWidth = MainCameraMono.selfInstance.fullCanvasScaler.referenceResolution.x;
            float fullHeight = MainCameraMono.selfInstance.fullCanvasScaler.referenceResolution.y;
            Vector2 fullShowSize = _getShowMaxSize(_showWidth, _showHeight);
            float posX = ((fullWidth / fullShowSize.x) * _showWidth - _showWidth) / 2; //(((fullWidth - fullShowSize.x) / 2) / fullWidth) * _showWidth;
            float posY = ((fullHeight / fullShowSize.y) * _showHeight - _showHeight) / 2;;
            
            
            Rect returnRect = new Rect(posX, posY, _showWidth, _showHeight);


            return returnRect;
        }
        

        protected abstract void _onScreenShot(RenderTexture _showRenderTexture, Vector2Int _shareImageSize, Action _onComplete);

        /// <summary>
        /// 点击返回
        /// </summary>
        /// <param name="_"></param>
        protected void _onBtnBackClicked(GameObject _)
        {
            _onClickedBtnBack();
        }

        protected abstract void _onClickedBtnBack();
    }
}