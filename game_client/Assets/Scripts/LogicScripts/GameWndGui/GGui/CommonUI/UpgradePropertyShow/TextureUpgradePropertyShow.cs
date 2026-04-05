
using UnityEngine.UI;

namespace GOE
{
    public class TextureUpgradePropertyShow : _AUpgradePropertyShow<RawImage, NPGTextureIndex>
    {
        private NPGGuiWndTexture _m_textureCurrent;
        private NPGGuiWndTexture _m_textureNext;
        
        
        public TextureUpgradePropertyShow(CommonUpgradePropertyShow<RawImage> _propertyShow) 
            : base(_propertyShow)
        {
            if (_propertyShow == null)
                return;
            
            if (_propertyShow.current != null)
                _m_textureCurrent = new NPGGuiWndTexture(_propertyShow.current);
            if (_propertyShow.next != null)
                _m_textureNext = new NPGGuiWndTexture(_propertyShow.next);
        }


        public void showWnd()
        {
            _m_textureCurrent?.showWnd();
            _m_textureNext?.showWnd();
        }
        public void hideWnd()
        {
            _m_textureCurrent?.hideWnd();
            _m_textureNext?.hideWnd();
        }
        public void discardTexture()
        {
            _m_textureCurrent?.discardTexture();
            _m_textureNext?.discardTexture();
        }
        public void discard()
        {
            _m_textureCurrent?.discard();
            _m_textureNext?.discard();
            
            _m_textureCurrent = null;
            _m_textureNext = null;
        }
        
        
        protected override void _setValue(RawImage _property, NPGTextureIndex _value)
        {
            if (_m_textureCurrent != null && _property == _m_textureCurrent.rawImage)
                _m_textureCurrent.setTexture(_value);
            if (_m_textureNext != null && _property == _m_textureNext.rawImage)
                _m_textureNext.setTexture(_value);
        }
    }
}