using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地民意选择求助处理结果窗口
    /// </summary>
    public class GGUIWndMarsPopularWillChoiceHelpDealResult : _ANPGGUIBasicWnd<GGUIMonoMarsPopularWillChoiceHelpDealResult>
    {
        private static GGUIWndMarsPopularWillChoiceHelpDealResult _g_instance;
        public static GGUIWndMarsPopularWillChoiceHelpDealResult instance { get { return _g_instance ??= new GGUIWndMarsPopularWillChoiceHelpDealResult(); } }
        
        private MarsPeopleWillHelp_Choice _m_iHelp;
        private List<NPCommonCostItem> _m_rewardItemList;
        
        private NPGGuiWndTexture _m_wNpcTexture;
        private NPGGUIWndCommonItemContainer _m_wRewardItemContainer;
        
        public GGUIWndMarsPopularWillChoiceHelpDealResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsPopularWillChoiceHelpDealResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsPopularWillChoiceHelpDealResult.objName; } }
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

            // 注册确定按钮点击事件
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

            // 反注册确定按钮点击事件
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onClickSure);
            }
        }

        protected override void _onShowWnd()
        {
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
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
        public void setData(MarsPeopleWillHelp_Choice _help, List<NPCommon.NPCommon_ItemInfo> _rewardList)
        {
            _m_iHelp = _help;
            _m_rewardItemList = NPCommonCostItem.switchList(_rewardList);
            
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

            int chooseIdx = _m_iHelp.chooseIdx;
            
            // 设置处理结果描述文本 - 根据选择的选项获取对应的结果描述
            string resultDesc = choiceHelpRefObj.option_result_desc_list?.SafeGet(chooseIdx);
            ALUGUICommon.setLabelTxt(wnd.txtResultDesc, TextTranslate.instance.getLanguage(resultDesc));
            
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

            // 满意度变化值
            int satisfactionDegreeChange = choiceHelpRefObj.option_add_satisfaction_degree_list?.SafeGet(chooseIdx) ?? 0;
            
            ALUGUICommon.setGameObjEnable(wnd.satisfactionDegreeNoChgShowList, satisfactionDegreeChange == 0);
            ALUGUICommon.setGameObjEnable(wnd.satisfactionDegreeAddShowList, satisfactionDegreeChange > 0);
            ALUGUICommon.setGameObjEnable(wnd.satisfactionDegreeReduceShowList, satisfactionDegreeChange < 0);
            
            // 设置满意度变化文本 - 从选项对应的满意度列表中获取
            if (satisfactionDegreeChange != 0)
            {
                string satisfactionKey = satisfactionDegreeChange > 0 
                    ? wnd.txtSatisfactionDegreeAddKey 
                    : wnd.txtSatisfactionDegreeReduceKey;
                if (string.IsNullOrEmpty(satisfactionKey))
                    satisfactionKey = TransKeyConst.common_percentage_num;

                string satisfactionText = TextTranslate.instance.getLanguage(satisfactionKey, (satisfactionDegreeChange / 100).ToString("+#;-#;0"));
                ALUGUICommon.setLabelTxt(wnd.txtSatisfactionDegreeChange, satisfactionText);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtSatisfactionDegreeChange, "");
            }

            if (_m_wRewardItemContainer != null)
            {
                _m_wRewardItemContainer.showWnd();
                _m_wRewardItemContainer.showItemList(_m_rewardItemList);
            }
        }

        /// <summary>
        /// 点击确定按钮
        /// </summary>
        /// <param name="_go">按钮GameObject</param>
        private void _onClickSure(GameObject _go)
        {
            // 关闭结果展示窗口
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_POPULAR_WILL_CHOICE_HELP_DEAL_RESULT);
        }
    }
}