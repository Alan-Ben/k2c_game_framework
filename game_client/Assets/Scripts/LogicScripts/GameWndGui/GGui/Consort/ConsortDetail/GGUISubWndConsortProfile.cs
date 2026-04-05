using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子简介子窗口
    /// </summary>
    public class GGUISubWndConsortProfile : _ANPGGUIBasicSubWnd<GGUISubMonoConsortProfile>
    {
        private GConsortRefObj _m_iConsortRefObj;//妃子配表

        public GGUISubWndConsortProfile(GGUISubMonoConsortProfile _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_consortRef"></param>
        public void setData(GConsortRefObj _consortRef)
        {
            _m_iConsortRefObj = _consortRef;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iConsortRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.consortName, _m_iConsortRefObj.transName);
            ALUGUICommon.setLabelTxt(wnd.consortTitle, TextTranslate.instance.getLanguage(_m_iConsortRefObj.consort_title));
            ALUGUICommon.setLabelTxt(wnd.birthplace, TextTranslate.instance.getLanguage(_m_iConsortRefObj.birthplace));
            ALUGUICommon.setLabelTxt(wnd.desc, GCommon.getItemDesc(ENPItemType.CONSORT, _m_iConsortRefObj.id));
        }
    }
}