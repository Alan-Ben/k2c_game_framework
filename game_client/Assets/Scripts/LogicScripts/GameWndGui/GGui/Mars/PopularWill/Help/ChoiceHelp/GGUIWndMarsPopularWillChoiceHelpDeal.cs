using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地民意选择求助处理窗口
    /// </summary>
    public class GGUIWndMarsPopularWillChoiceHelpDeal : _ANPGGUIBasicWnd<GGUIMonoMarsPopularWillChoiceHelpDeal>
    {
        private static GGUIWndMarsPopularWillChoiceHelpDeal _g_instance;
        public static GGUIWndMarsPopularWillChoiceHelpDeal instance { get { return _g_instance ??= new GGUIWndMarsPopularWillChoiceHelpDeal(); } }
        
        private MarsPeopleWillHelp_Choice _m_iHelp;
        
        private NPGGuiWndTexture _m_wNpcTexture;
        private GGUIWndMarsPopularWillChoiceHelpOptionItemContainer _m_wBeforeDealOptionContainer;
        private GGUIWndMarsPopularWillChoiceHelpOptionItemContainer _m_wAfterDealOptionContainer;
        
        public GGUIWndMarsPopularWillChoiceHelpDeal() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsPopularWillChoiceHelpDeal.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsPopularWillChoiceHelpDeal.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化NPC头像窗口
            if (wnd.npcImg != null)
                _m_wNpcTexture = new NPGGuiWndTexture(wnd.npcImg);

            // 初始化处理前选项容器
            if (wnd.monoBeforeDealChoiceHelpOptionItemContainer != null)
            {
                _m_wBeforeDealOptionContainer = new GGUIWndMarsPopularWillChoiceHelpOptionItemContainer(wnd.monoBeforeDealChoiceHelpOptionItemContainer);
                _m_wBeforeDealOptionContainer.onClickOption += _onClickOption;
            }

            // 初始化处理后选项容器
            if (wnd.monoAfterDealChoiceHelpOptionItemContainer != null)
            {
                _m_wAfterDealOptionContainer = new GGUIWndMarsPopularWillChoiceHelpOptionItemContainer(wnd.monoAfterDealChoiceHelpOptionItemContainer);
                _m_wAfterDealOptionContainer.onClickOption += _onClickOption;
            }

            // 注册确认按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onClickSure);
        }

        protected override void _onDiscard()
        {
            // 销毁NPC头像窗口
            _m_wNpcTexture?.discard();
            _m_wNpcTexture = null;

            // 销毁选项容器
            if (_m_wBeforeDealOptionContainer != null)
            {
                _m_wBeforeDealOptionContainer.onClickOption -= _onClickOption;
                _m_wBeforeDealOptionContainer.discard();
                _m_wBeforeDealOptionContainer = null;    
            }

            if (_m_wAfterDealOptionContainer != null)
            {
                _m_wAfterDealOptionContainer.onClickOption -= _onClickOption;
                _m_wAfterDealOptionContainer.discard();
                _m_wAfterDealOptionContainer = null;
            }
            
            _m_iHelp = null;

            // 反注册确认按钮点击事件
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onClickSure);
            }
        }

        protected override void _onShowWnd()
        {
            refreshWnd();
            
            // 注册求助相关消息监听
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_HELP_UPDATE, _onHelpUpdate);
        }

        protected override void _onHideWnd()
        {
            // 反注册消息监听
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_HELP_UPDATE, _onHelpUpdate);
            
            // 隐藏子窗口
            _m_wNpcTexture?.hideWnd();
            _m_wBeforeDealOptionContainer?.hideWnd();
            _m_wAfterDealOptionContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置子窗口
            _m_wNpcTexture?.discardTexture();
            _m_wBeforeDealOptionContainer?.resetWnd();
            _m_wAfterDealOptionContainer?.resetWnd();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_help">帮助数据</param>
        public void setData(_IMarsPeopleWillHelp _help)
        {
            _m_iHelp = _help as MarsPeopleWillHelp_Choice;
            
            refreshWnd();
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_iHelp == null)
                return;

            MarsPeopleHelpRefObj helpRefObj = _m_iHelp.refObj;
            MarsPeopleChoiceHelpRefObj choiceHelpRefObj = _m_iHelp.choiceHelpRefObj;
            if (helpRefObj == null || choiceHelpRefObj == null)
                return;

            // 设置描述文本
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(helpRefObj.desc));
            
            NPNPCRefObj npcRefObj = _m_iHelp.npcRefObj;
            if (npcRefObj != null)
            {
                // 设置NPC头像
                if (_m_wNpcTexture != null)
                {
                    _m_wNpcTexture.showWnd();
                    _m_wNpcTexture.setTexture(npcRefObj.npcIcon);
                }

                // 设置NPC名字
                ALUGUICommon.setLabelTxt(wnd.txtNpcName, TextTranslate.instance.getLanguage(npcRefObj.Name));
            }

            // 根据求助状态显示不同的选项容器
            if (_m_iHelp.state == EMarsPopularWillHelpState.WAIT_HANDLE)
            {
                _m_wAfterDealOptionContainer?.hideWnd();

                if (_m_wBeforeDealOptionContainer != null)
                {
                    _m_wBeforeDealOptionContainer.showWnd();
                    _m_wBeforeDealOptionContainer.setData(choiceHelpRefObj.option_desc_list, _m_iHelp.chooseIdx);       
                }
            }
            else
            {
                // 已处理状态：显示处理后容器，显示已选择的选项
                _m_wBeforeDealOptionContainer?.hideWnd();

                if (_m_wAfterDealOptionContainer != null)
                {
                    _m_wAfterDealOptionContainer.showWnd();
                    _m_wAfterDealOptionContainer.setData(choiceHelpRefObj.option_desc_list, _m_iHelp.chooseIdx);
                }
            }

            // 设置状态显示
            if(wnd.stateShowList != null)
                NPCommonEnumStatMutexShowInfo<EMarsPopularWillHelpState>.setStatEx(wnd.stateShowList, _m_iHelp.state);
        }

        /// <summary>
        /// 选项点击事件
        /// </summary>
        /// <param name="optionItem">点击的选项</param>
        private void _onClickOption(GGUIWndMarsPopularWillChoiceHelpOptionItem optionItem)
        {
            if (optionItem == null || _m_iHelp == null || _m_iHelp.state != EMarsPopularWillHelpState.WAIT_HANDLE)
                return;
            
            NPPlayer.instance.marsComp.peopleSubComponent.reqDealChoiceHelp(_m_iHelp.instanceId, optionItem.index, (_isSucc, _msg) =>
            {
                if (_isSucc && _msg != null)
                {
                    GGUIWndMarsPopularWillChoiceHelpDealResult.instance.setData(_m_iHelp, _msg.getItemList());
                    // 处理成功，显示结果窗口
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsPopularWillChoiceHelpDealResult.instance, () =>
                    {
                        GGUIWndMarsPopularWillChoiceHelpDealResult.instance.showWnd();
                    }, UINodeTagConst.C_MARS_POPULAR_WILL_CHOICE_HELP_DEAL_RESULT);
                    
                    // 关闭当前窗口
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_POPULAR_WILL_CHOICE_HELP_DEAL);
                }
                else
                {
                    // 处理失败，显示错误提示
                    Debug.LogError("[GGUIWndMarsPopularWillChoiceHelpDeal] 处理选择求助失败");
                }
            });
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go">按钮GameObject</param>
        private void _onClickSure(GameObject _go)
        {
            // 直接关闭窗口
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_POPULAR_WILL_CHOICE_HELP_DEAL);
        }
        
        #region 消息处理
        
        /// <summary>
        /// 求助信息更新
        /// </summary>
        /// <param name="_objs">消息参数</param>
        private void _onHelpUpdate(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is _IMarsPeopleWillHelp chgHelpInfo) 
               || _m_iHelp == null || chgHelpInfo.instanceId != _m_iHelp.instanceId)
                return;
            
            // 更新本地数据
            _m_iHelp = chgHelpInfo as MarsPeopleWillHelp_Choice;
            
            // 刷新窗口显示
            refreshWnd();
        }
        
        #endregion
    }
}