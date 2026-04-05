using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndConsortChatMsgDetail : _ATALBasicUIWnd<GGUIMonoConsortChatMsgDetail>
    {
        private static GGUIWndConsortChatMsgDetail _g_instance = new GGUIWndConsortChatMsgDetail();
    
        public static GGUIWndConsortChatMsgDetail instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndConsortChatMsgDetail();
                return _g_instance;
            }
        }

        private long _m_consortId; 
        
        private ConsortPresetChatInfo _m_presetChatInfo; // 预设聊天信息
        private GGUIWndConsortChatMsgList _m_wMsgList; // 消息列表窗口
        private GGUIWndConsortChatOptionItemContainer _wOptionItemContainer; // 选项容器窗口
        //消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;
        
        private ALCommonEnableTaskController _m_consortChatTick;

    
        public GGUIWndConsortChatMsgDetail() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoConsortChatMsgDetail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoConsortChatMsgDetail.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            _m_wMsgList?.showWnd();
            _refreshWnd();
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_CHAT_PRESET_CHAT_CHG, _onChatMsgChange);

        }
    
        protected override void _onHideWnd()
        {        
            _m_wMsgList?.hideWnd();
            _m_consortChatTick.setDisable();
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_CHAT_PRESET_CHAT_CHG, _onChatMsgChange);

        }
    
        protected override void _onReset()
        {
            _m_wMsgList?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_wMsgList?.discard();
            _m_wMsgList = null;
            
            if (_wOptionItemContainer != null)
            {
                _wOptionItemContainer.discard();
                _wOptionItemContainer = null;
            }
            _m_presetChatInfo = null;
            
            if (wnd == null) return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnSend, _onBtnSendClick);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.msgList != null)
            {
                _m_wMsgList = new GGUIWndConsortChatMsgList(wnd.msgList);
            }
            if (wnd.optionItemContainer != null)
            {
                _wOptionItemContainer = new GGUIWndConsortChatOptionItemContainer(wnd.optionItemContainer);
            }
            if(wnd.inputField != null)
                wnd.inputField.characterLimit = GRefdataCoreMgr.instance.npGeneral.consort_chat_ai_one_msg_max_word_count;

            if (wnd.monoAISendCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoAISendCostItem);
            ALUGUICommon.combineBtnClick(wnd.btnSend, _onBtnSendClick);
        }

        public void setInfo(long _consortId)
        {
            _m_consortId = _consortId;
            if (_m_consortId <= 0)
                return;
      
            _m_presetChatInfo = NPPlayer.instance.consortChatComp.getConsortPresetChatInfo(_m_consortId);
          
            _refreshWnd();
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if (_m_consortId <= 0)
                return;
            
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_consortId);
            ALUGUICommon.setLabelTxt(wnd.txtConsortName, consortInfo?.consortTransName);
            
            if (_m_presetChatInfo != null)
            {
                if (!_m_presetChatInfo.isLoaded)
                    _m_presetChatInfo.load();
                ALUGUICommon.setGameObjEnable(wnd.stateAIChatShows, false);
                ALUGUICommon.setGameObjEnable(wnd.stateNormalChatShows, true);
                ALUGUICommon.setGameObjEnable(wnd.stateOptionChatShows, false);
                _m_presetChatInfo.regLoadDoneDelegate(() =>
                {
                    _m_wMsgList?.setShowData(_m_presetChatInfo, _m_consortId);
                    _refreshPage();
                });
            }
        }

        private void _refreshPage()
        {
            if (_m_presetChatInfo != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.lockAIChatShows, !_m_presetChatInfo.hasUnlockAIChat);
                ALUGUICommon.setLabelTxt(wnd.txtLockAITip, _m_presetChatInfo.unlockAIChatTip);

                AccountSettingMgr.instance.consortAIChatSaverMgr.setRead(_m_presetChatInfo.consortId);
                if (_m_presetChatInfo.needShowAIChat)
                {
                    _refreshPageAIChat();
                }
                else
                {
                    _refreshPagePresetChat();
                }
            }
        }


        /// <summary>
        /// 刷新预设聊天界面
        /// </summary>
        private void _refreshPagePresetChat()
        {
           
            _m_consortChatTick.setDisable();
            _m_consortChatTick = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_refreshOption);
            
            ALUGUICommon.setGameObjEnable(wnd.stateAIChatShows, false);
            ALUGUICommon.setGameObjEnable(wnd.stateNormalChatShows, true);
            ALUGUICommon.setGameObjEnable(wnd.stateOptionChatShows, false);
            _refreshOption();
        }
        
        private void _refreshOption()
        {
            if (_m_presetChatInfo == null || wnd == null)
                return;
            
            bool needShowOption = _m_presetChatInfo.needShowOption(out long dialogueId, out ConsortChatDialogueSentenceRefObj sentence);
            if (_wOptionItemContainer != null)
            {
                if (needShowOption && !_wOptionItemContainer.isShow)
                {
                    _wOptionItemContainer.showWnd();
                    _wOptionItemContainer.showItemList(_m_presetChatInfo.consortId, dialogueId, sentence, _onChooseOption);
                    _m_wMsgList?.setSeeNewest();
                }
                else
                {
                    _wOptionItemContainer.hideWnd();
                }
            }
            
            
            ALUGUICommon.setGameObjEnable(wnd.stateNormalChatShows, !needShowOption);
            ALUGUICommon.setGameObjEnable(wnd.stateOptionChatShows, needShowOption);
            if (_m_presetChatInfo != null && _m_presetChatInfo.needShowAIChat)
            {
                _refreshPage();
            }
        }

        private void _onChooseOption(long _dialogueId, long _sentenceId, long _optionId)
        {
            if (_m_presetChatInfo != null)
            {
                int serialize = MainCameraMono.selfInstance.openAllInputMask();

                NPPlayer.instance.consortChatComp.reqSendChooseDialogueOption(_m_consortId,
                    _dialogueId, _sentenceId, _optionId, _b =>
                    {
                        MainCameraMono.selfInstance.closeAllInputMask(serialize);
                        _refreshOption();
                    });
                
            }
        }

        #region Page AI 妃子AI聊天
        
        private void _refreshPageAIChat()
        {
            _refreshAIChatCost();
            
            ALUGUICommon.setGameObjEnable(wnd.stateNormalChatShows, false);
            ALUGUICommon.setGameObjEnable(wnd.stateOptionChatShows, false);
            ALUGUICommon.setGameObjEnable(wnd.stateAIChatShows, true);
        }

        /// <summary>
        /// 刷新AI聊天的消耗信息
        /// </summary>
        private void _refreshAIChatCost()
        {
            if (wnd == null)
                return;

            NPPlayerFixedCDInfo cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.consort_chat_ai_send_fixed_cd_id);

            if (cdInfo != null && cdInfo.getCount() > 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtAiChatLeftFreeTimes,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, cdInfo.getCount(),
                        cdInfo.getMaxCount()));
                ALUGUICommon.setGameObjEnable(wnd.aiCostPriceShows, false);
                ALUGUICommon.setGameObjEnable(wnd.aiFreeTimesShows, true);
            }
            else
            {
                if (_m_wCostItem != null)
                {         
                    long timePriceTypeId = GRefdataCoreMgr.instance.npGeneral.consort_chat_ai_send_time_price_id;
                    long todayAlreadyBuyCount = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.DAY_HAD_SEND_PAY_AI_TIMES);

                    TimesPriceRefObj costItemPrice = GRefdataCoreMgr.instance.getTimesPriceRefObj(timePriceTypeId, todayAlreadyBuyCount + 1);

                    if (costItemPrice != null)
                    {
                        _m_wCostItem.showWnd();
                        _m_wCostItem.setItem(new NPCommonCostItem(costItemPrice.item, costItemPrice.cost_item_formula.CalculateVariableResult(null)));
                    }
                }
                ALUGUICommon.setGameObjEnable(wnd.aiFreeTimesShows, false);
                ALUGUICommon.setGameObjEnable(wnd.aiCostPriceShows, true);

            }

        }
        
        /// <summary>
        /// AI聊天消息发生变化
        /// </summary>
        private void _onChatMsgChange(params object[] _objs)
        {
            if (_objs == null || _objs[0] is not long consortId)
                return;
            // 如果是当前打开得妃子ai聊天。则设置为已读
            if(consortId == _m_consortId && isShow)
                AccountSettingMgr.instance.consortAIChatSaverMgr.setRead(consortId);
        }
        
        /// <summary>
        /// 点击发送消息按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnSendClick(GameObject _)
        {
            if (wnd == null ||  wnd.inputField == null)
                return;
            string output = wnd.inputField.text;
            if (string.IsNullOrEmpty(output))
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_input_empty_tips));
                return;
            }
            
            if(_m_presetChatInfo!= null && _m_presetChatInfo.needShowAIChat)
            {
                _m_presetChatInfo.reqSendAIChatMsg(output, _suc => { _refreshAIChatCost();});
            }
            
            // 发送后清空文字，并刷新
            wnd.inputField.text = string.Empty;
        }
        #endregion

      
    }
}