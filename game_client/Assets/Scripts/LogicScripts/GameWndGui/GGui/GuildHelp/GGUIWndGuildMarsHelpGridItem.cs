using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟火星互助详情
    /// </summary>
    public class GGUIWndGuildMarsHelpGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildMarsHelpGridItem>
    {
        private _IGuildMarsHelpShow _m_data;
        
        // <AutoGen:WndDeclaration>
        private NPGGUIWndPlayerIcon _m_playIconWnd;
        private GGUIWndLongProgress _m_helpProgressWnd;  // 帮助进度
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildMarsHelpGridItem(GGUIMonoGuildMarsHelpGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_helpProgressWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_helpProgressWnd?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_playIconWnd?.discard();
            _m_playIconWnd = null;
            _m_helpProgressWnd?.discard();
            _m_helpProgressWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.playIcon != null)
                _m_playIconWnd = new NPGGUIWndPlayerIcon(wnd.playIcon);
            if (wnd.helpProgress != null)
                _m_helpProgressWnd = new GGUIWndLongProgress(wnd.helpProgress);
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(_IGuildMarsHelpShow _data)
        {
            _m_data = _data;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || _m_data == null)
                return;
            if(_m_playIconWnd != null)
            {
                _m_playIconWnd.showWnd();
                if(_m_data.isMyHelp)
                    _m_playIconWnd.setSelfInfo();
                else
                    _m_playIconWnd.setPlayer(_m_data.senderCid);
            }
            ALUGUICommon.setLabelTxt(wnd.txtHelpDetal, _m_data.getDetailStr());
            if(_m_helpProgressWnd != null)
            {
                string _getCommonSliderTxtStr(string _cur, string _max)
                {
                    return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _cur, _max);
                }
                _m_helpProgressWnd.showWnd();
                _m_helpProgressWnd.initSld(0, _m_data.dealLimit, _getCommonSliderTxtStr, EValueFormatType.NORMAL);
                _m_helpProgressWnd.setNowValue(_m_data.dealedCount);
            }
            ALUGUICommon.setLabelTxt(wnd.txtReduceTime, _m_data.getReduceTimeStr());
            ALUGUICommon.setGameObjEnable(wnd.myHelpShowList, _m_data.isMyHelp);
            ALUGUICommon.setGameObjEnable(wnd.myHelpHideList, !_m_data.isMyHelp);
            if (wnd.moreInfoShowAnimation != null)
            {
                if (_m_data.isMyHelp)
                {
                    wnd.moreInfoShowAnimation.Play(wnd.moreInfoShowAnimationName);
                }
                else
                {
                    wnd.moreInfoShowAnimation.Sample(wnd.moreInfoShowAnimationName, 0);
                }
            }
         
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
