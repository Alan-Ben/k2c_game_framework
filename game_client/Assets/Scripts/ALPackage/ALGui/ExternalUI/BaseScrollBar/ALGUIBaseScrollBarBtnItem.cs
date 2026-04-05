using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /***********************************
     * scroll bar button item
     **/
    public class ALGUIBaseScrollBarBtnItem : ALGUIBaseMouseStatWnd
    {
        /** scroll bar item paint function delegate */
        public delegate void ScrollBarItemPainFunc(ALGUIBaseScrollBarBtnItem _item);

        /** 所在容器的对象 */
        private ALGUIBaseScrollBar _m_wParentScrollBar;
        /** pain function delegate */
        private ScrollBarItemPainFunc _m_dPainDelegate;

        public ALGUIBaseScrollBarBtnItem(ALGUIBaseScrollBar _scrollBar, ScrollBarItemPainFunc _painDelegate, ALGUIWndPositionStyle _posStyle)
            : base(_posStyle)
        {
            _m_wParentScrollBar = _scrollBar;
            _m_dPainDelegate = _painDelegate;

            ALGUIWndPrintActionDelegate = OnPain;
        }

        public ALGUIBaseScrollBar parent { get { return _m_wParentScrollBar; } }

        /******************
         * only draw the back ground picture
         **/
        public void OnPain(ALGUIBaseWnd _wnd)
        {
            //use the delegate to pain
            if (null != _m_dPainDelegate)
            {
                _m_dPainDelegate(this);
            }
        }
    }
}

#endif
