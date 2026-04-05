using ALPackage;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 对话框子窗体
    /// </summary>
    public partial class NPGGUIWndDialogBox : _ATALBasicLoadPrefabSubUIWnd<NPGGUIMonoDialogBox>
    {
        /// <summary>
        /// 对话状态，WAIT => PLAYING => END => WAIT => ……
        /// </summary>
        public enum ENPDialogBoxState
        {
            WAIT,//等待
            PLAYING,//播放中
            END,//播放结束
        }

        [NotNull] private NPDialogueTextTagDealer _m_tagDealer;//标签处理器
        [NotNull] private _TALStateMachine<_ANPDialogBoxState, ENPDialogBoxState> _m_stateMachine;//状态机
        private ALCommonEnableTaskController _m_tickTask;//状态机tick任务
        private NPGGUIWndDialogueOptionContainer _m_wOptionContainer;//回应选项容器

        private long _m_lResPathId;//资源路径id
        private NPGGuiWndTexture _m_icon;//头像图标
        private NPDialogueSentenceRefObj _m_refObj;//对话配置
        private Action _m_aOnShowEnd;//对话结束调用
        private Action _m_aOnClickNext;//点击下一句调用
        private Action<NPGGUIWndDialogueOptionItem> _m_aOnClickOption;//点击对话选项
        private Action _m_aOnTextChg;//文本变化时调用
        
        private long _m_serialId = 0;//序列号
        private bool _m_isMaskClickNext = false;//是否屏蔽点击下一句

        public NPGGUIWndDialogBox(long _resPathId, Transform _parent) : base(_parent)
        {
            _m_lResPathId = _resPathId;
            _m_tagDealer = new NPDialogueTextTagDealer();
            _m_stateMachine = new _TALStateMachine<_ANPDialogBoxState, ENPDialogBoxState>(new NPDialogBoxStateFactory(this), new NPDialogBoxState_Wait(this));
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lResPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lResPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        public long resPathId { get { return _m_lResPathId; } }

        protected override void _onShowWnd()
        {
            _m_isMaskClickNext = false;
            _m_serialId++;
        }

        protected override void _onHideWnd()
        {
            _m_stateMachine.setState(typeof(NPDialogBoxState_Wait));
            _m_icon?.hideWnd();
            
            _m_isMaskClickNext = false;
            _m_serialId++;
        }

        protected override void _onReset()
        {
            _m_icon?.discardTexture();
            _m_wOptionContainer?.resetWnd();
            _m_tagDealer.clear();

            _m_refObj = null;
            _m_aOnShowEnd = null;
            _m_aOnClickNext = null;
            _m_aOnClickOption = null;
            _m_aOnTextChg = null;
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_icon?.discard();
            _m_icon = null;

            if (_m_wOptionContainer != null)
            {
                _m_wOptionContainer.onSelectItem -= _onSelectOptionItem;
                _m_wOptionContainer.discard();
            }
            _m_wOptionContainer = null;

            _m_tagDealer.clear();

            _m_refObj = null;
            _m_aOnShowEnd = null;
            _m_aOnClickNext = null;
            _m_aOnClickOption = null;
            _m_aOnTextChg = null;
            
            _m_isMaskClickNext = false;
            _m_serialId++;

            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickBtnNext);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //选项容器
            if (wnd.monoOptionContainer != null)
            {
                _m_wOptionContainer = new NPGGUIWndDialogueOptionContainer(wnd.monoOptionContainer);
                _m_wOptionContainer.onSelectItem += _onSelectOptionItem;
                _m_wOptionContainer.hideWnd();
            }

            //发言者头像
            if (wnd.imgIcon != null)
                _m_icon = new NPGGuiWndTexture(wnd.imgIcon);

            _m_isMaskClickNext = false;
            _m_serialId++;
            
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickBtnNext);
        }


        #region 点击事件

        /// <summary>
        /// 点击继续按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnNext(GameObject _go)
        {
            if (wnd == null || _m_refObj == null)
                return;

            if(_m_isMaskClickNext)
                return;
            
            //等待状态下不处理点击，只有外部调用playSentence才会再次进入播放状态
            switch (_m_stateMachine.curState.state)
            {
                //播放状态，显示全部文本
                case ENPDialogBoxState.PLAYING:
                    _showAllContentText();
                    break;

                //播放结束状态，执行点击回调，并重新进入等待状态
                case ENPDialogBoxState.END:
                    _m_stateMachine.changeState(typeof(NPDialogBoxState_Wait));
                    _m_aOnClickNext?.Invoke();
                    break;
            }
        }

        #endregion


        #region 窗体事件

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_refObj == null)
                return;

            //是否可以设置玩家名字
            bool canSetPlayerName = GRefdataCoreMgr.instance.npGeneral.dialogue_set_self_name_cond != null &&
                                    !GRefdataCoreMgr.instance.npGeneral.dialogue_set_self_name_cond.isEmpty &&
                                    GRefdataCoreMgr.instance.npGeneral.dialogue_set_self_name_cond.IsEnable(null);

            //发言者名称
            if (_m_refObj.isSelf && canSetPlayerName)
                ALUGUICommon.setLabelTxt(wnd.txtName, NPPlayer.instance.playerInfo.PlayerName);
            else if(_m_refObj.isSelf && !canSetPlayerName)
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(TransKeyConst.common_you_none));
            else
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_refObj.sender_name,_m_refObj.senderNameArgs));

            //发言者头像图标
            if (_m_icon != null)
            {
                //如果是自己读取自己的图标
                if (_m_refObj.isSelf)
                {
                    NPPlayerIconItem selfIcon = NPPlayer.instance.playerInfo.curIcon;
                    if (selfIcon != null)
                    {
                        _m_icon.showWnd();
                        _m_icon.setTexture(GCommon.getItemTexIcon(ENPItemType.ICON, selfIcon.refId));
                    }
                    else
                        _m_icon.hideWnd();
                }
                else if (_m_refObj.senderIcon != null && _m_refObj.senderIcon.isValid())
                {
                    _m_icon.showWnd();
                    _m_icon.setTexture(_m_refObj.senderIcon);
                }
                else
                    _m_icon.hideWnd();
            }
        }

        /// <summary>
        /// 直接显示所有对话内容
        /// </summary>
        private void _showAllContentText()
        {
            if (wnd == null || _m_refObj == null)
                return;

            //显示对话文本
            ALUGUICommon.setLabelTxt(wnd.txtContent, _m_refObj.getContentStr());

            _dealRightStyleShow();
            //对话结束
            _onDialogueShowEnd();
        }

        /// <summary>
        /// 一句对话结束
        /// </summary>
        private void _onDialogueShowEnd()
        {
            if (wnd != null && wnd.txtContent != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.txtContent.rectTransform);
            _m_stateMachine.changeState(typeof(NPDialogBoxState_End));
        }

        /// <summary>
        /// 处理右边对话样式文本布局
        /// </summary>
        private void _dealRightStyleShow()
        {
            if (wnd == null || wnd.txtContent == null || wnd.monoSizeFix == null)
                return;

            if (wnd.txtContent.preferredWidth > wnd.monoSizeFix.maxPreferredWidth)
                wnd.txtContent.alignment = TextAnchor.UpperLeft;
            else
                wnd.txtContent.alignment = TextAnchor.UpperRight;
        }

        /// <summary>
        /// 文本内容变化，入参为不带标签的文本
        /// </summary>
        /// <param name="_text"></param>
        private void _onContentTextChg(string _text)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtContent, _m_tagDealer.applyTag(_text));
            _dealRightStyleShow();
            _m_aOnTextChg?.Invoke();
        }

        /// <summary>
        /// 选中对话选项
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onSelectOptionItem(NPGGUIWndDialogueOptionItem _itemWnd)
        {
            if (_itemWnd == null)
                return;

            //隐藏选项
            ALUGUICommon.setGameObjEnable(wnd.goHaveOptionShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goHaveOptionHideList, true);

            //选择选项之后隐藏选项窗体
            _m_wOptionContainer?.hideWnd();

            _m_aOnClickOption?.Invoke(_itemWnd);
        }

        #endregion


        #region State

        /// <summary>
        /// 进入播放状态
        /// </summary>
        private void _onEnterPlayingState()
        {
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_stateMachineTick, 0);
        }

        /// <summary>
        /// 退出播放状态
        /// </summary>
        private void _onExitPlayingState()
        {
            _m_tickTask.setDisable();
        }

        /// <summary>
        /// 进入结束状态
        /// </summary>
        private void _onEnterEndState()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goListShowOnSentenceOver, true);
            _m_aOnShowEnd?.Invoke();
        }

        /// <summary>
        /// 状态机tick
        /// </summary>
        private void _stateMachineTick()
        {
            _m_stateMachine.tick(Time.deltaTime);
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 强制设置对话状态结束
        /// </summary>
        public void setSentenceStateFinal()
        {
            _m_stateMachine.setState(typeof(NPDialogBoxState_Wait));
        }

        /// <summary>
        /// 直接显示所有对话内容
        /// </summary>
        public void setSentenceShowAllContentText()
        {
            _showAllContentText();
        }

        /// <summary>
        /// 播放句子
        /// </summary>
        /// <param name="_refObj"></param>
        /// <param name="_onShowEnd"></param>
        /// <param name="_onClickNext"></param>
        public void playSentence(NPDialogueSentenceRefObj _refObj, Action _onShowEnd, Action _onClickNext, Action _onTextChg = null)
        {
            if (_refObj == null || wnd == null)
                return;

            _m_refObj = _refObj;
            _m_aOnShowEnd = _onShowEnd;
            _m_aOnClickNext = _onClickNext;
            _m_aOnTextChg = _onTextChg;

            //初始化标签信息，并获取返回去除标签后的文本内容
            string realStr = _m_tagDealer.initTagInfo(_m_refObj.getContentStr());

            //刷新名称等显示
            _refreshWnd();

            //先清空文本
            ALUGUICommon.setLabelTxt(wnd.txtContent, string.Empty);
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnSentenceOver, false);

            //隐藏选项
            ALUGUICommon.setGameObjEnable(wnd.goHaveOptionShowList,false);
            ALUGUICommon.setGameObjEnable(wnd.goHaveOptionHideList,true);

            //进入播放状态
            _m_stateMachine.changeState<NPDialogBoxState_Playing>((_state) => { _state.setContent(realStr);});
            
            //计时屏蔽点击时间
            if (_m_refObj != null && _m_refObj.skipTimeMs > 0)
            {
                _m_serialId++;
                long serialId = _m_serialId;
                
                _m_isMaskClickNext = true;
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if(serialId != _m_serialId)
                        return;
                    
                    _m_isMaskClickNext = false;
                }, _m_refObj.skipTimeMs / 1000f);
            }
            else
            {
                _m_isMaskClickNext = false;
            }
        }

        /// <summary>
        /// 展示对话选项
        /// </summary>
        /// <param name="_optionRefList"></param>
        public void showOptionList(List<NPDialogueResponseOptionRefObj> _optionRefList, Action<NPGGUIWndDialogueOptionItem> _onClickOption)
        {
            if (wnd == null || _optionRefList == null)
                return;

            _m_aOnClickOption = _onClickOption;

            if (_m_wOptionContainer != null && _optionRefList.Count > 0)
            {
                //显示选项
                ALUGUICommon.setGameObjEnable(wnd.goHaveOptionHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goHaveOptionShowList, true);

                _m_wOptionContainer.showWnd();
                _m_wOptionContainer.showItemList(_optionRefList);
            }
            else
            {
                //隐藏选项
                ALUGUICommon.setGameObjEnable(wnd.goHaveOptionShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goHaveOptionHideList, true);
            }
        }

        /// <summary>
        /// 获取对话框当前状态
        /// </summary>
        /// <returns></returns>
        public ENPDialogBoxState getDialogBoxState()
        {
            return _m_stateMachine.curState.state;
        }

        #endregion
    }
}
