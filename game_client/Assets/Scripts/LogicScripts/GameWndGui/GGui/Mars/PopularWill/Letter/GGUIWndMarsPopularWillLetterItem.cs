using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地民意信件项窗口
    /// </summary>
    public class GGUIWndMarsPopularWillLetterItem : _ANPGGUIBasicGridItemWnd<GGUIMonoMarsPopularWillLetterItem>
    {
        private _IMarsPeopleWillLetter _m_iLetter;
        
        private NPGGuiWndTexture _m_wNpcHead;
        
        public GGUIWndMarsPopularWillLetterItem(GGUIMonoMarsPopularWillLetterItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.npcHead != null)
                _m_wNpcHead = new NPGGuiWndTexture(wnd.npcHead);
            
            // 注册前往按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onGotoBtnClick);
        }

        protected override void _onDiscard()
        {
            _m_wNpcHead?.discard();
            _m_wNpcHead = null;
            
            _m_iLetter = null;
            
            // 反注册前往按钮点击事件
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onGotoBtnClick);
            }
        }

        protected override void _onShowWnd()
        {
            refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_LETTER_UPDATE, _onLetterInfoChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_LETTER_UPDATE, _onLetterInfoChg);
            
            _m_wNpcHead?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wNpcHead?.discardTexture();
        }
        
        protected override void _resetGridItem()
        {
            _m_wNpcHead?.discardTexture();
        }
        
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_letter">民意信件数据</param>
        public void setData(_IMarsPeopleWillLetter _letter)
        {
            _m_iLetter = _letter;
            refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_iLetter == null)
                return;

            MarsPeopleLetterRefObj letterRefObj = _m_iLetter.refObj;
            if (letterRefObj == null)
                return;
                
            NPNPCRefObj npcRefObj = _m_iLetter.npcRefObj;

            // 设置NPC头像
            if (_m_wNpcHead != null)
            {
                _m_wNpcHead.showWnd();
                _m_wNpcHead.setTexture(npcRefObj?.npcIcon);
            }

            // 设置NPC名字
            ALUGUICommon.setLabelTxt(wnd.txtNpcName, TextTranslate.instance.getLanguage(npcRefObj?.Name));

            // 设置信件内容
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(letterRefObj.Content));

            // 根据是否为抱怨显示不同界面元素
            ALUGUICommon.setGameObjEnable(wnd.isComplainShowGoList, letterRefObj.IsComplain);
            ALUGUICommon.setGameObjEnable(wnd.notComplainShowGoList, !letterRefObj.IsComplain);
            
            // 当需要处理 且 有前往效果 且 不满足已处理条件时 显示前往按钮
            ALUGUICommon.setGameObjEnable(wnd.btnGoto, letterRefObj.need_deal && letterRefObj.goToEffect != null && !letterRefObj.goToEffect.isEmpty && 
                                                       letterRefObj.resolveCond != null && !letterRefObj.resolveCond.isNoConditionOrEnable(null));
        }
        
        /// <summary>
        /// 前往按钮点击事件
        /// </summary>
        private void _onGotoBtnClick(GameObject _go)
        {
            if (_m_iLetter?.refObj == null)
                return;
            
            // 执行前往效果
            _m_iLetter.refObj.goToEffect?.dealEffect();
        }

        #region 窗口消息

        /// <summary>
        /// 信件信息变更
        /// </summary>
        private void _onLetterInfoChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is _IMarsPeopleWillLetter chgLetter) 
               || _m_iLetter == null || chgLetter.instanceId != _m_iLetter.instanceId)
                return;
            
            refreshWnd();
        }

        #endregion
    }
}