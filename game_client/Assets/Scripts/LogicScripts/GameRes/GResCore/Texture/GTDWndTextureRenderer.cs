using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GTDWndTextureRenderer : _ATDSceneBasicTexture
    {
        private Renderer _m_rRenderer;

        public GTDWndTextureRenderer(Renderer _rawImage)
        {
            _m_rRenderer = _rawImage;

            //设置默认图片
            if(null != _m_rRenderer && null != material)
                material.mainTexture = PLoginCommonInfo.instance.obj.defaultTexture;
        }
        
        /********
         * 显示窗口
         * */
        public void showWnd()
        {
            if(null == _m_rRenderer)
                return;
            
            _m_rRenderer.gameObject.SetActive(true);
        }
        public void hideWnd()
        {
            if(null == _m_rRenderer)
                return;

            //释放图片
            discardTexture();
            _m_rRenderer.gameObject.SetActive(false);
        }

        /********
         * 销毁实例化出来的对象
         * */
        public override void discard()
        {
            base.discard();

            _m_rRenderer = null;
        }
        
        /************
         * 获取对应图片显示的控件对象
         **/
        public sealed override Material material
        {
            get { return _m_rRenderer?.material; }
        }

        protected override Texture defaultTexture
        {
            get { return PLoginCommonInfo.instance.obj.defaultTexture; }
        }


        protected override _AUITextureCacheMgr textureCacheMgr
        {
            get { return GGUITextureCacheMgr.instance; }
        }
    }
}