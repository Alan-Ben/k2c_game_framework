using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /***********************************
     * scroll bar button item
     **/
    public class ALGUIBaseScrollBarDragBtn : ALGUIBaseMouseStatWnd
    {
        /** 所在容器的对象 */
        private ALGUIBaseScrollBar _m_wParentScrollBar;

        public ALGUIBaseScrollBarDragBtn(ALGUIBaseScrollBar _scrollBar, ALGUIWndPositionStyle _posStyle)
            : base(_posStyle)
        {
            _m_wParentScrollBar = _scrollBar;

            ALGUIWndPrintActionDelegate = OnPain;
        }

        /******************
         * only draw the back ground picture
         **/
        public void OnPain(ALGUIBaseWnd _wnd)
        {
            //use the parent's function to paint the gui
            _m_wParentScrollBar._onPainDragBtn(this);
        }
    }
}

#endif
