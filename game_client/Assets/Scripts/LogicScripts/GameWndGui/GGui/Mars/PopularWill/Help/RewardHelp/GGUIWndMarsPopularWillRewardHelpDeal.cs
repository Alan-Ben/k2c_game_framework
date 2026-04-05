using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地民意奖励帮助处理窗口
    /// </summary>
    public class GGUIWndMarsPopularWillRewardHelpDeal : _ANPGGUIBasicWnd<GGUIMonoMarsPopularWillRewardHelpDeal>
    {
        private static GGUIWndMarsPopularWillRewardHelpDeal _g_instance;
        public static GGUIWndMarsPopularWillRewardHelpDeal instance { get { return _g_instance ??= new GGUIWndMarsPopularWillRewardHelpDeal(); } }

        private MarsPeopleWillHelp_Reward _m_iHelp;
        
        private NPGGuiWndTexture _m_wNpcTexture;
        private NPGGUIWndCommonItemContainer _m_wRewardItemContainer;
        
        public GGUIWndMarsPopularWillRewardHelpDeal() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsPopularWillRewardHelpDeal.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsPopularWillRewardHelpDeal.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化NPC头像窗口
            if (wnd.npcImg != null)
                _m_wNpcTexture = new NPGGuiWndTexture(wnd.npcImg);

            // 初始化奖励物品容器
            if (wnd.rewardItemContainer != null)
                _m_wRewardItemContainer = new NPGGUIWndCommonItemContainer(wnd.rewardItemContainer);

            // 注册确认按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onClickSure);
        }

        protected override void _onDiscard()
        {
            // 销毁NPC头像窗口
            _m_wNpcTexture?.discard();
            _m_wNpcTexture = null;

            // 销毁奖励物品容器
            _m_wRewardItemContainer?.discard();
            _m_wRewardItemContainer = null;

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
            _m_wRewardItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置子窗口
            _m_wNpcTexture?.discardTexture();
            _m_wRewardItemContainer?.resetWnd();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_help">帮助数据</param>
        public void setData(_IMarsPeopleWillHelp _help)
        {
            _m_iHelp = _help as MarsPeopleWillHelp_Reward;
            
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
            MarsPeopleRewardHelpRefObj rewardHelpRefObj = _m_iHelp.rewardHelpRefObj;
            if (helpRefObj == null || rewardHelpRefObj == null)
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

            ALUGUICommon.setGameObjEnable(wnd.satisfactionDegreeNoChgShowList, rewardHelpRefObj.add_satisfaction_degree == 0);
            ALUGUICommon.setGameObjEnable(wnd.satisfactionDegreeAddShowList, rewardHelpRefObj.add_satisfaction_degree > 0);
            ALUGUICommon.setGameObjEnable(wnd.satisfactionDegreeReduceShowList, rewardHelpRefObj.add_satisfaction_degree < 0);
            
            // 设置满意度变化文本
            if (rewardHelpRefObj.add_satisfaction_degree != 0)
            {
                string satisfactionKey = rewardHelpRefObj.add_satisfaction_degree > 0 
                    ? wnd.txtSatisfactionDegreeAddKey 
                    : wnd.txtSatisfactionDegreeReduceKey;
                if (string.IsNullOrEmpty(satisfactionKey))
                    satisfactionKey = TransKeyConst.common_percentage_num;
                
                string satisfactionText = TextTranslate.instance.getLanguage(satisfactionKey, (rewardHelpRefObj.add_satisfaction_degree / 100).ToString("+#;-#;0"));
                ALUGUICommon.setLabelTxt(wnd.txtSatisfactionDegreeChange, satisfactionText);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtSatisfactionDegreeChange, "");
            }

            // 设置奖励物品
            if (_m_wRewardItemContainer != null && rewardHelpRefObj.reward_list != null && rewardHelpRefObj.reward_list.Count > 0)
            {
                _m_wRewardItemContainer.showWnd();
                _m_wRewardItemContainer.showItemList(rewardHelpRefObj.reward_list);
            }
            else
            {
                _m_wRewardItemContainer?.hideWnd();
            }

            // 设置状态显示
            setStat(_m_iHelp.state);
        }

        /// <summary>
        /// 设置状态显示
        /// </summary>
        /// <param name="_state">状态类型</param>
        public void setStat(EMarsPopularWillHelpState _state)
        {
            if (wnd == null || wnd.stateShowList == null)
                return;

            NPCommonEnumStatMutexShowInfo<EMarsPopularWillHelpState>.setStatEx(wnd.stateShowList, _state);
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go">按钮GameObject</param>
        private void _onClickSure(GameObject _go)
        {
            // 根据求助状态进行不同操作
            if (_m_iHelp != null && _m_iHelp.state == EMarsPopularWillHelpState.WAIT_HANDLE)
            {
                // 未处理的求助，向服务器请求处理
                NPPlayer.instance.marsComp?.peopleSubComponent?.reqDealRewardHelp(_m_iHelp.instanceId, (_isSucc, _msg) =>
                {
                    if (_isSucc)
                    {
                        // 处理成功，关闭窗口
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_POPULAR_WILL_REWARD_HELP_DEAL);
                    }
                    else
                    {
                        // 处理失败，可以在这里显示错误提示
                        Debug.LogError_EditorOnly("[GGUIWndMarsPopularWillRewardHelpDeal] 处理奖励求助失败");
                    }
                });
            }
            else
            {
                // 已处理的求助，直接关闭窗口
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_POPULAR_WILL_REWARD_HELP_DEAL);
            }
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
            _m_iHelp = chgHelpInfo as MarsPeopleWillHelp_Reward;
            
            // 刷新窗口显示
            refreshWnd();
        }
        
        #endregion
    }
}