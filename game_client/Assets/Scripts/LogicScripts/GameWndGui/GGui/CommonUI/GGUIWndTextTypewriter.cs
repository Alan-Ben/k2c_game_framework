using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTextTypewriter : _ANPGGUIBasicSubWnd<GGUIMonoTextTypewriter>
    {
        private ALCommonEnableTaskController _m_tickTask;//文本打字机效果任务
        
        private NPDialogueTextTagDealer _m_tagDealer;//标签处理器
        private string _m_sOriContent;//原内容文本
        private string _m_sRemoveTagContent;//去掉标签打字机效果用的内容文本
        
        private float _m_fIntervalMs;//字符显示间隔时间（毫秒）
        private long _m_fTypewriterShowStartTimeMs;//打字机文本开始显示的时间
        private int _m_iPreShowLength;//上次显示的长度
        private bool _m_bTypewriterIsShowing;//是否正在显示打字机效果

        private Action _m_aOnShowTextNumChg;//当显示的文本数量发生变化时回调(打字机效果时调用)
        private Action _m_aShowDone;//显示完成回调

        public GGUIWndTextTypewriter(GGUIMonoTextTypewriter _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            _m_fIntervalMs = wnd == null ? 0 : wnd.perCharShowIntervalMs;
            if (wnd != null)
            {
                ALUGUICommon.combineBtnClick(wnd.btnShowAllText, _showAllBtnClick);
            }
        }
        
        protected override void _onDiscard()
        {
            _typewriterShowDone();
            
            _m_tagDealer?.clear();
            _m_tagDealer = null;
            
            if(wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnShowAllText, _showAllBtnClick);
            }
        }
        
        protected override void _onShowWnd()
        {
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnShowAllText, false);
            }
        }

        protected override void _onHideWnd()
        {
            _typewriterShowDone();
            
            _m_tagDealer?.clear();
        }

        protected override void _onReset()
        {
            _typewriterShowDone();
            
            _m_tagDealer?.clear();
        }
        
        /// <summary>
        /// 直接显示文本
        /// </summary>
        public void showText(string _content)
        {
            if (wnd == null)
                return;
            
            if (_m_bTypewriterIsShowing)//若还有打字机效果在显示, 直接完成
                _typewriterShowDone();

            ALUGUICommon.setGameObjEnable(wnd.btnShowAllText, false);
            ALUGUICommon.setLabelTxt(wnd.txt, _content);
        }

        /// <summary>
        ///  显示打字机效果
        /// </summary>
        /// <param name="_content"></param>
        /// <param name="_showDone"></param>
        public void showTextTypewriter(string _content, Action _onShowTextNumChg = null, Action _showDone = null)
        {
            if (wnd == null)
                return;
            
            if (_m_bTypewriterIsShowing)//若还有打字机效果在显示, 直接完成
                _typewriterShowDone();

            // 设置文本内容为空
            ALUGUICommon.setLabelTxt(wnd.txt, string.Empty);

            // 打字机每个字符显示间隔 <= 0, 直接显示文本
            if (_m_fIntervalMs < 0 || Mathf.Approximately(_m_fIntervalMs, 0))
            {
                ALUGUICommon.setLabelTxt(wnd.txt, _content);
                _onShowTextNumChg?.Invoke();
                _showDone?.Invoke();
            }
            else
            {
                _m_sOriContent = _content;
                if(_m_tagDealer == null)
                    _m_tagDealer = new NPDialogueTextTagDealer();
                _m_sRemoveTagContent = _m_tagDealer.initTagInfo(_m_sOriContent);
                
                _m_aOnShowTextNumChg = _onShowTextNumChg;
                _m_aShowDone = _showDone;

                _startTypewriterShow();//开始显示打字机效果
            }
        }

        /// <summary>
        /// 开始显示打字机效果
        /// </summary>
        private void _startTypewriterShow()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.btnShowAllText, true);
            _m_bTypewriterIsShowing = true;
            _m_fTypewriterShowStartTimeMs = TimeUtil.getTimeStampMill();//记录打字机开始显示时间
            _m_iPreShowLength = -1;
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tickAction, 0);
        }

        private void _tickAction()
        {
            if (wnd == null || string.IsNullOrEmpty(_m_sRemoveTagContent) || _m_fIntervalMs < 0 || Mathf.Approximately(_m_fIntervalMs, 0))
            {
                _typewriterShowDone();
                return;
            }

            long typewriterShowPassTimeMs = TimeUtil.getTimeStampMill() - _m_fTypewriterShowStartTimeMs;//开始显示打字机效果经过的时间
            int curShowLength = (int)(typewriterShowPassTimeMs / _m_fIntervalMs);//本次需要显示的文本长度
            if (curShowLength >= _m_sRemoveTagContent.Length)//若本次显示的文本长度超过了文本总长度
            {
                _typewriterShowDone();
                return;
            }

            if (curShowLength >= 0 && curShowLength != _m_iPreShowLength)//若本次显示的文本长度与上次不同
            {
                string subStr = _m_sRemoveTagContent.Substring(0, curShowLength);
                ALUGUICommon.setLabelTxt(wnd.txt, _m_tagDealer?.applyTag(subStr) ?? subStr);
                _m_aOnShowTextNumChg?.Invoke();
                _m_iPreShowLength = curShowLength;
            }
        }
        
        /// <summary>
        /// 打字机效果显示完成
        /// </summary>
        private void _typewriterShowDone()
        {
            if (!_m_bTypewriterIsShowing)//若当前没有打字机效果在显示, 直接返回
                return;
            
            _m_tickTask.setDisable();

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnShowAllText, false);
                ALUGUICommon.setLabelTxt(wnd.txt, _m_sOriContent);
            }

            _m_bTypewriterIsShowing = false;

            Action showDone = _m_aShowDone;
            _m_aShowDone = null;
            showDone?.Invoke();
        }

        /// <summary>
        /// 显示所有文本内容按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _showAllBtnClick(GameObject _go)
        {
            _typewriterShowDone();
        }

        /// <summary>
        /// 外部调用，设置打字机效果显示完成
        /// </summary>
        public void setTypewriterShowDone()
        {
            _typewriterShowDone();
        }
    }
}