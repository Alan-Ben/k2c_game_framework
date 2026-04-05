using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class NPAGuiWndTexture : _AALGuiWndBasicTexture
    {
        private RawImage _m_riRawImageWnd;

        public NPAGuiWndTexture(RawImage _rawImage)
        {
            _m_riRawImageWnd = _rawImage;

            //设置默认图片
            if(null != _m_riRawImageWnd)
                _m_riRawImageWnd.texture = PLoginCommonInfo.instance.obj.defaultTexture;
        }

        public RawImage rawImage { get { return _m_riRawImageWnd; } }

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
        protected override RawImage _getRawImage()
        {
            return _m_riRawImageWnd;
        }
        protected override Texture _getDefaultTexture()
        {
            return PLoginCommonInfo.instance.obj.defaultTexture;
        }

        protected override Texture _getFailTexture()
        {
            return PLoginCommonInfo.instance.obj.defaultTexture;
        }

        protected override Material _getGrayMat()
        {
            return GGameCommonInfo.instance.obj.guiGrayMat;
        }
        protected override _AALUITextureCacheMgr _getTextureCacheMgr()
        {
            return AGUITextureCacheMgr.instance;
        }
        
    }
}
