using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游戏设置切换语言列表item
    /// </summary>
    public class NPGGUIWndGameSettingSwitchLanguageContainerItem : _ATALBasicUISubWnd<NPGGUIMonoGameSettingSwitchLanguageContainerItem>
    {
        private ENPLanguage _m_eLanguage;

        private Action<ENPLanguage> _m_aOnClickItem;
        
        public NPGGUIWndGameSettingSwitchLanguageContainerItem(NPGGUIMonoGameSettingSwitchLanguageContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_aOnClickItem = null;
        }

        protected override void _onReset()
        {
            _m_aOnClickItem = null;
        }

        protected override void _onDiscard()
        {
            _m_aOnClickItem = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        public void setInfo(ENPLanguage _language, bool _isSelect, Action<ENPLanguage> _onClickItem)
        {
            if (wnd == null)
                return;

            _m_eLanguage = _language;
            _m_aOnClickItem = _onClickItem;

            string languageName = TextTranslate.instance.getLanguage(string.Format("#1_{0}_name", _language.ToString()));
            if (_isSelect)
            {
                languageName = GCommon.addColorForRichText(languageName, wnd.curTextColor);
                ALUGUICommon.setGameObjEnable(wnd.goNotCurShowGOList,false);
                ALUGUICommon.setGameObjEnable(wnd.goCurShowGOList,true);
            }
            else
            {
                languageName = GCommon.addColorForRichText(languageName, wnd.notCurTextColor);
                ALUGUICommon.setGameObjEnable(wnd.goCurShowGOList, false);
                ALUGUICommon.setGameObjEnable(wnd.goNotCurShowGOList, true);
            }
            ALUGUICommon.setLabelTxt(wnd.texLanguage, languageName);
        }


        //响应选择语言
        private void _onClickItem(GameObject _go)
        {
            _m_aOnClickItem?.Invoke(_m_eLanguage);
        }
    }
}
