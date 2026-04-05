using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGuiWndTexture : _AGUIWndBasicTexture
    {
        private RawImage _m_riRawImageWnd;

        public NPGGuiWndTexture(RawImage _rawImage)
        {
            _m_riRawImageWnd = _rawImage;

            //设置默认图片
            if(null != _m_riRawImageWnd)
                _m_riRawImageWnd.texture = PLoginCommonInfo.instance.obj.defaultTexture;
        }
        
        /********
         * 显示窗口
         * */
        public void showWnd()
        {
            if(null == _m_riRawImageWnd)
                return;
            
            _m_riRawImageWnd.gameObject.SetActive(true);
        }
        public void hideWnd()
        {
            if(null == _m_riRawImageWnd)
                return;

            //释放图片
            discardTexture();
            _m_riRawImageWnd.gameObject.SetActive(false);
        }

        /********
         * 销毁实例化出来的对象
         * */
        public override void discard()
        {
            base.discard();

            _m_riRawImageWnd = null;
        }

        /************
         * 获取对应图片显示的控件对象
         **/
        public override RawImage rawImage
        {
            get { return _m_riRawImageWnd; }
        }

        protected override Texture defaultTexture
        {
            get { return PLoginCommonInfo.instance.obj.defaultTexture; }
        }

        protected override Material grayMat
        {
            get { return GGameCommonInfo.instance.obj.guiGrayMat; }
        }

        protected override _AUITextureCacheMgr textureCacheMgr
        {
            get { return GGUITextureCacheMgr.instance; }
        }
        
        public void setTexture(RenderTexture _renderTexture)
        {
            if(null == rawImage)
                return;
            discardTexture();
            rawImage.texture = _renderTexture;
        }
        
        public void setTexture(Texture2D _texture2D)
        {
            if (null == _texture2D)
                return;
            discardTexture();
            rawImage.texture = _texture2D;
        }
    }
}
