using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱
    /// </summary>
    public class GGUIWndGuildBoxGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildBoxGridItem>
    {
        private GuildBoxInfo _m_guildBoxInfo;
        private string _m_sPlayerName;
        private NPGGUIWndCommonItem _m_itemWndWnd;  // 物品
        private NPGGuiWndTexture _m_imgBoxIconWnd;
        private long _m_playerInfoSerializeOp;
        private Action<int> _m_onScorllList;
        private Action<long, string> _m_onGetName;
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildBoxGridItem(GGUIMonoGuildBoxGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_playerInfoSerializeOp = ALSerializeOpMgr.next();
        }
    
        protected override void _onReset()
        {
            _m_playerInfoSerializeOp = ALSerializeOpMgr.next();
        }
    
        protected override void _onDiscard()
        {
            _m_itemWndWnd?.discard();
            _m_itemWndWnd = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnGet, _onClickbtnGet);
                ALUGUICommon.uncombineBtnClick(wnd.btnBoxDetail, _onClickbtnBoxDetail);
            }
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.itemWnd != null)
                _m_itemWndWnd = new NPGGUIWndCommonItem(wnd.itemWnd);
            
            if (wnd.imgBoxIcon != null)
                _m_imgBoxIconWnd = new NPGGuiWndTexture(wnd.imgBoxIcon);
            ALUGUICommon.combineBtnClick(wnd.btnGet, _onClickbtnGet);
            ALUGUICommon.combineBtnClick(wnd.btnBoxDetail, _onClickbtnBoxDetail);
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(GuildBoxInfo _data, string _playerName, Action<int> _onScrollList, Action<long,string> _onGetName)
        {
            _m_guildBoxInfo = _data;
            _m_sPlayerName = _playerName;
            _m_onScorllList = _onScrollList;
            _m_onGetName = _onGetName;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新倒计时
        /// </summary>
        public void refreshCD()
        {
            if (_m_guildBoxInfo == null || wnd == null)
                return;

            long leftTimeMs = _m_guildBoxInfo.endTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            ALUGUICommon.setUIObjColor(wnd.txtTime, leftTimeMs < wnd.redColorTextTimeS * 1000 ? wnd.txtTimeColorRed : wnd.txtTimeColorCommon);
            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.millisecondsToTime_hms(leftTimeMs));
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (_m_guildBoxInfo == null)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtScore, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_guildBoxInfo.guildBoxRefObj.gain_guild_active_point));
            ALUGUICommon.setLabelTxt(wnd.txtName,  GCommon.getItemName(ENPItemType.GUILD_BOX,  _m_guildBoxInfo.guildBoxRefObj.id));
            _m_playerInfoSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_playerInfoSerializeOp;

            if (_m_guildBoxInfo.senderCid <= 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtDetail, TextTranslate.instance.getLanguage( _m_guildBoxInfo.guildBoxRefObj.box_source_desc, TextTranslate.instance.getLanguage(TransKeyConst.guild_box_anonymity_name)));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtDetail, TextTranslate.instance.getLanguage(_m_guildBoxInfo.guildBoxRefObj.box_source_desc, _m_sPlayerName));
                if (string.IsNullOrEmpty(_m_sPlayerName))
                {
                    GCommon.reqPlayerInfo(_m_guildBoxInfo.senderCid, _info =>
                    {
                        if (serializeOp != _m_playerInfoSerializeOp || wnd == null)
                            return;
                        _m_onGetName?.Invoke(_m_guildBoxInfo.senderCid, _info?.name);
                        ALUGUICommon.setLabelTxt(wnd.txtDetail, TextTranslate.instance.getLanguage(_m_guildBoxInfo.guildBoxRefObj.box_source_desc, _info?.name));
                    });
                }
            }
         
            bool alreadyGetBox = _m_guildBoxInfo.isArealdyGain;
            ALUGUICommon.setGameObjEnable(wnd.alreadyGetShowList, alreadyGetBox);
            ALUGUICommon.setGameObjEnable(wnd.alreadyGetHideList, !alreadyGetBox);
            _m_itemWndWnd?.setItem(_m_guildBoxInfo.rewardItem);
            _m_imgBoxIconWnd?.setTexture(_m_guildBoxInfo.guildBoxRefObj.box_icon);
            refreshCD();
        }

        /// <summary>
        /// 处理领奖完成显示
        /// </summary>
        private void _dealShowGetReward()
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addProcess(_refreshWnd)
                .addDelegateProcess(_showParticle)
                .addProcess(() =>
                {
                    _m_onScorllList?.Invoke(itemIdx);
                })
                .deal();
        }

        /// <summary>
        /// 展示粒子
        /// </summary>
        private void _showParticle(Action _onDone)
        {
            if (wnd == null || _m_guildBoxInfo == null || _m_guildBoxInfo.guildBoxRefObj == null)
                return;

            //奖励道具
            NPCommonCostItem costItem = _m_guildBoxInfo.rewardItem;
            if (costItem != null)
                GCommon.showItemParticle(costItem.toCommon_ItemInfo(), wnd.itemParticleStartRectTransform, wnd.itemParticleId);

            //积分道具
            NPCommonItem activePointItem = GRefdataCoreMgr.instance.npGeneral.guild_box_active_point_item;
            NPCommonCostItem activePointCostItem = new NPCommonCostItem(activePointItem, 1);
            GCommon.showItemParticle(activePointCostItem.toCommon_ItemInfo(), wnd.activePointParticleStartRectTransform, wnd.activePointParticleId,null,true);

            ALCommonActionMonoTask.addMonoTask(_onDone, wnd.delayShowListScroll);
        }

        // <AutoGen:Method>
        
        // 领取按钮点击事件
        private void _onClickbtnGet(GameObject go)
        {
            if(_m_guildBoxInfo == null)
                return;
            if (_m_guildBoxInfo.endTimeMs - FpsAndPingMgr.instance.serverTimeTag < 0)
                return;
            NPPlayer.instance.guildBoxComp.reqGainGuildRewardBox(_m_guildBoxInfo.boxType,  _m_guildBoxInfo.instanceId,
                (_suc) =>
                {
                    if (wnd == null || !isShow || !_suc)
                        return;

                    _dealShowGetReward();
                });
        }
        private void _onClickbtnBoxDetail(GameObject go)
        {
            if (_m_guildBoxInfo != null && _m_guildBoxInfo.guildBoxRefObj != null) 
                GCommon.showItemDetail(ENPItemType.GUILD_BOX, _m_guildBoxInfo.guildBoxRefObj.id);
        }
        // </AutoGen:Method>
    }
}
