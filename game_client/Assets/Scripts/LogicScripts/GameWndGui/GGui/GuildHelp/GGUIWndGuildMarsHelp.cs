using ALPackage;
using Common.GuildEnum;
using Common.GuildObj;
using Common.PrivilegeCardEnum;
using CommonEnum;
using GC2GS.p013_HeroOp;
using GC2GS.p032_GuildOp;
using GS2GC.p013_HeroOp;
using GS2GC.p032_GuildOp;
using NPEnum;
using System;
using System.Collections.Generic;
using GC2GS.p042_GuildRelatedOp;
using GS2GC.p042_GuildRelatedOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟火星互助详情
    /// </summary>
    public class GGUIWndGuildMarsHelp : _ATALBasicUIWnd<GGUIMonoGuildMarsHelp>
    {
        private static GGUIWndGuildMarsHelp _g_instance = new GGUIWndGuildMarsHelp();
    
        public static GGUIWndGuildMarsHelp instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildMarsHelp();
                return _g_instance;
            }
        }

        //权益卡类型
        private EPrivilegeCardType _m_eCardType;
        private GGUIWndGuildMarsHelpGrid _m_itemGridWnd;
        private List<_IGuildMarsHelpShow> _m_itemDataList = new List<_IGuildMarsHelpShow>();
        //定时任务
        private ALCommonEnableTaskController _m_cdTask;
        //显示序列号
        private long _m_lShowSerialize;

        // <AutoGen:WndDeclaration>
        private GGUIWndLongProgress _m_rewardProgressWnd;  // 奖励进度
        // </AutoGen:WndDeclaration>

        public GGUIWndGuildMarsHelp() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildMarsHelp.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildMarsHelp.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch { get => true; }

        protected override void _onShowWnd()
        {
            //获取对应权益卡类型
            GRefdataCoreMgr.instance.privilegeCardRefCore.dealAllRef(_ref =>
            {
                if (_m_eCardType == EPrivilegeCardType.NONE && _ref != null)
                {
                    GiftPackRefObj giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_ref.gift_pack_id);
                    if (giftPackRef != null && giftPackRef.item_list != null)
                    {
                        for (int i = 0; i < giftPackRef.item_list.Count; i++)
                        {
                            if (giftPackRef.item_list[i] != null &&
                                giftPackRef.item_list[i].getItemType() == ENPItemType.BUFF &&
                                giftPackRef.item_list[i].subId == GRefdataCoreMgr.instance.npGeneral.guild_mars_help_auto_deal_buff_id)
                            {
                                _m_eCardType = _ref.privilege_card_type;
                                break;
                            }
                        }
                    }
                }
            });

            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
            _startCheck();
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG, _onCanDealCountChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
        }
    
        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _stopCheck();
            _m_rewardProgressWnd?.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG, _onCanDealCountChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
        }
    
        protected override void _onReset()
        {
            _m_rewardProgressWnd?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            _m_rewardProgressWnd?.discard();
            _m_rewardProgressWnd = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnHelp, _onBtnHelpClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrivilegeTip, _onBtnPrivilegeTipClick);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemGrid != null)
                _m_itemGridWnd = new GGUIWndGuildMarsHelpGrid(wnd.itemGrid);
            
            if (wnd.rewardProgress != null)
                _m_rewardProgressWnd = new GGUIWndLongProgress(wnd.rewardProgress);
            
            ALUGUICommon.combineBtnClick(wnd.btnHelp, _onBtnHelpClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnPrivilegeTip, _onBtnPrivilegeTipClick);
        }

        private void _onBtnHelpClick(GameObject _obj)
        {
            NPPlayer.instance.guildMarsHelpComp.reqDealMarsHelp(_suc =>
            {
                _refreshWnd();
            });
        }
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_MARS_HELP);
        }

        private void _onBtnPrivilegeTipClick(GameObject _obj)
        {
            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                TextTranslate.instance.getLanguage(TransKeyConst.guild_autoMarsHelpTipDesc_none),
                (RectTransform)_obj.transform, wnd.privilegeTipInterval.x, wnd.privilegeTipInterval.y));
        }

        public _IGuildMarsHelpShow createMyHelpShowInfo(Guild_MarsHelpInfo _helpInfo)
        {
            switch (_helpInfo.getObjType())
            {
                case EGuildMarsHelpObjType.BUILDING_QUEUE:
                    return new GuildMarsHelpShow_BuildingQueue(true, 0, _helpInfo.getDealLimit(), _helpInfo.getDealedCount(), _helpInfo.getDealSecs(), _helpInfo.getExt());
                    break;
                case EGuildMarsHelpObjType.TECH_UP:
                    return new GuildMarsHelpShow_TechUp(true, 0, _helpInfo.getDealLimit(), _helpInfo.getDealedCount(), _helpInfo.getDealSecs(), _helpInfo.getExt());
                    break;
                case EGuildMarsHelpObjType.TEAM_REPAIR:
                    return new GuildMarsHelpShow_TeamRepair(true, 0, _helpInfo.getDealLimit(), _helpInfo.getDealedCount(), _helpInfo.getDealSecs(), _helpInfo.getExt());
                    break;
            }

            return null;
        }
        public _IGuildMarsHelpShow createHelpShowInfo(Guild_MarsHelpShowInfo _helpInfo)
        {
            switch (_helpInfo.getObjType())
            {
                case EGuildMarsHelpObjType.BUILDING_QUEUE:
                    return new GuildMarsHelpShow_BuildingQueue(false, _helpInfo.getSenderCid(), _helpInfo.getDealLimit(), _helpInfo.getDealedCount(), 0, _helpInfo.getExt());
                    break;
                case EGuildMarsHelpObjType.TECH_UP:
                    return new GuildMarsHelpShow_TechUp(false, _helpInfo.getSenderCid(), _helpInfo.getDealLimit(), _helpInfo.getDealedCount(), 0, _helpInfo.getExt());
                    break;
                case EGuildMarsHelpObjType.TEAM_REPAIR:
                    return new GuildMarsHelpShow_TeamRepair(false, _helpInfo.getSenderCid(), _helpInfo.getDealLimit(), _helpInfo.getDealedCount(), 0, _helpInfo.getExt());
                    break;
            }

            return null;
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            if(_m_itemDataList == null)
                _m_itemDataList = new List<_IGuildMarsHelpShow>();
            _m_itemDataList.Clear();
            NPPlayer.instance.guildMarsHelpComp.reqMarsHelpList(_msg =>
            {
                _m_itemDataList.Clear();
                foreach (Guild_MarsHelpInfo myHelp in _msg.getMyHelpList())
                {
                    _m_itemDataList.Add(createMyHelpShowInfo(myHelp));
                }
                foreach (Guild_MarsHelpShowInfo helpShowInfo in _msg.getHelpList())
                {
                    _m_itemDataList.Add(createHelpShowInfo(helpShowInfo));
                }
                _m_itemGridWnd?.showWnd();
                _m_itemGridWnd?.showItemList(_m_itemDataList);
                if(_m_rewardProgressWnd != null)
                {
                    string _getCommonSliderTxtStr(string _cur, string _max)
                    {
                        return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _cur, _max);
                    }
                    _m_rewardProgressWnd.showWnd();
                    NPCommonCostItem costItem = GRefdataCoreMgr.instance.npGeneral.guild_mars_help_deal_reward_item;
                    NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral
                        .guild_mars_help_deal_reward_fixed_cd_id);

                    long maxCount = 0;
                    long curCount = 0;
                   
                    if (costItem != null)
                    {
                        if (fixedCdInfo != null)
                        {
                            maxCount = fixedCdInfo.getMaxCount() * costItem.count;
                            curCount = costItem.count * fixedCdInfo.getCount();
                        }
                    }
                    _m_rewardProgressWnd.initSld(0, maxCount, _getCommonSliderTxtStr, EValueFormatType.NORMAL);
                    _m_rewardProgressWnd.setNowValue(maxCount - curCount);
                }
            });
            
            
            ALUGUICommon.setGameObjEnable(wnd.hasHelpToDealShowGos, NPPlayer.instance.guildComp.isJoinGuild() && NPPlayer.instance.guildMarsHelpComp.canDealCount > 0);

            //刷新特权展示
            _refreshPrivilegeShow();
        }

        //刷新特权展示
        private void _refreshPrivilegeShow()
        {
            if (wnd == null)
                return;

            //特权显隐
            NPPlayerBuffInfo buffInfo = NPPlayer.instance.playerBuffComp.lookup(GRefdataCoreMgr.instance.npGeneral.guild_mars_help_auto_deal_buff_id);
            bool havePrivilege = buffInfo != null && buffInfo.layer > 0;
            ALUGUICommon.setGameObjEnable(wnd.goActivePrivilegeHideList,!havePrivilege);
            ALUGUICommon.setGameObjEnable(wnd.goActivePrivilegeShowList,havePrivilege);

            //请求自动互助次数
            ALUGUICommon.setLabelTxt(wnd.txtAutoHelpCount, TextTranslate.instance.getLanguage(TransKeyConst.guild_todayAutoMarsHelpCount_num, 0));
            if (havePrivilege)
            {
                long curSerialize = _m_lShowSerialize;
                NPGSClientListener.sendRequestByLog(new GC2GS_042_009_ReqMarsHelpAutoDailyRecord(),
                    new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_042_009_RetMarsHelpAutoDailyRecord>(_msg =>
                    {
                        if (wnd == null || !isShow || curSerialize != _m_lShowSerialize)
                            return;

                        if(_msg != null)
                            ALUGUICommon.setLabelTxt(wnd.txtAutoHelpCount, TextTranslate.instance.getLanguage(TransKeyConst.guild_todayAutoMarsHelpCount_num,_msg.getCount()));
                    }));
            }
        }

        #region 倒计时

        /// <summary>
        /// 开启定时检查
        /// </summary>
        private void _startCheck()
        {
            _m_cdTask.setDisable();
            _m_cdTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCD, 0.2f);
        }

        /// <summary>
        /// 关闭定时检查
        /// </summary>
        private void _stopCheck()
        {
            _m_cdTask.setDisable();
        }

        /// <summary>
        /// 倒计时检查
        /// </summary>
        private void _tickCD()
        {
            if (wnd == null)
                return;

            NPPlayerBuffInfo buffInfo = NPPlayer.instance.playerBuffComp.lookup(GRefdataCoreMgr.instance.npGeneral.guild_mars_help_auto_deal_buff_id);
            if (buffInfo == null || buffInfo.layer <= 0)
            {
                _stopCheck();
                _refreshPrivilegeShow();
                return;
            }

            ALUGUICommon.setLabelTxt(wnd.txtPrivilegeCardLeftTime, TextTranslate.instance.getLanguage(TransKeyConst.guild_autoMarsHelpLeftTime_str, buffInfo.getLeftTimeStr()));
        }

        #endregion

        private void _onCanDealCountChg()
        {
            _refreshWnd();
        }

        private void _onCrossDay()
        {
            if (wnd == null)
                return;

            //跨天后重置显示
            ALUGUICommon.setLabelTxt(wnd.txtAutoHelpCount, TextTranslate.instance.getLanguage(TransKeyConst.guild_todayAutoMarsHelpCount_num, 0));
        }

        // <AutoGen:Method>

        // </AutoGen:Method>
    }
}