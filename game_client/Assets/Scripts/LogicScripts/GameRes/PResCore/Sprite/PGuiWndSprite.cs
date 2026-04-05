using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class PGuiWndSprite : _APGuiWndBasicSprite
    {
        private Image _m_riImageWnd;

        public PGuiWndSprite(Image _image)
        {
            _m_riImageWnd = _image;
        }

        public Image image { get { return _m_riImageWnd; } }

        /********
         * 显示窗口
         * */
        public void showWnd()
        {
            if(null == _m_riImageWnd)
                return;

            _m_riImageWnd.gameObject.SetActive(true);
        }
        public void hideWnd()
        {
            if(null == _m_riImageWnd)
                return;

            //释放图片
            discardTexture();
            _m_riImageWnd.gameObject.SetActive(false);
        }

        /********
         * 销毁实例化出来的对象
         * */
        public override void discard()
        {
            base.discard();

            _m_riImageWnd = null;
        }

        /************
         * 获取对应图片显示的控件对象
         **/
        protected override Image _getImage()
        {
            return _m_riImageWnd;
        }

        //重写方法，加入默认图标展示
        public new void setTexture(NPPSpriteIndex _spriteIndex)
        {
            base.setTexture(_spriteIndex, _setDefaultTexture);
        }
        
        private void _setDefaultTexture(_ATALObjResObj<Sprite> _resObj)
        {
            //如果没有加载到对应的资源图标，默认展示spt_99_1图标
            if (_m_riImageWnd != null && _m_riImageWnd.sprite == null)
                setTexture(99,1);
        }
    }
}
