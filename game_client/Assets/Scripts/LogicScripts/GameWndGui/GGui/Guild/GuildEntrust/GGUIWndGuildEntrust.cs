using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟杂物委托
    /// </summary>
    public class GGUIWndGuildEntrust : _ANPGGUIBasicResBarWnd<GGUIMonoGuildEntrust>
    {
        private static GGUIWndGuildEntrust _g_instance;
        public static GGUIWndGuildEntrust instance { get { return _g_instance ??= new GGUIWndGuildEntrust(); } }
        
        private NPGGuiWndTexture _m_wBannerImg;//委托banner
        private NPGGUIWndProgress _m_wEntrustProgress;//委托进度 
        private NPGGUIWndCommonToggleEx _m_wAutoDealToggle;//自动处理toggle
        private GGUIWndCommonLazyCDCountResume _m_wDealLazyCd;//处理cd
        private List<CommonUISfxObj> _m_lSfxObjList;//特效列表
        private NPGGUICommonTipDealerMgr _m_tipMgr;//tip管理器
        private List<CommonUISfxObj> _m_lMultipleSfxObjList;//暴击特效列表

        public GGUIWndGuildEntrust() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildEntrust.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildEntrust.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.entrustBannerImg != null)
                _m_wBannerImg = new NPGGuiWndTexture(wnd.entrustBannerImg);

            if (wnd.monoEntrustProgress != null)
                _m_wEntrustProgress = new NPGGUIWndProgress(wnd.monoEntrustProgress);

            if (wnd.monoAutoDealToggle != null)
            {
                _m_wAutoDealToggle = new NPGGUIWndCommonToggleEx(wnd.monoAutoDealToggle);
                _m_wAutoDealToggle.clickDelegate += _onAutoDealToggleClick;
            }

            if (wnd.monoDealLazyCd != null)
                _m_wDealLazyCd = new GGUIWndCommonLazyCDCountResume(wnd.monoDealLazyCd);

            //tip管理器
            if (wnd.multipleCenterTipParent != null)
                _m_tipMgr = new NPGGUICommonTipDealerMgr(wnd.multipleCenterTipParent);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnEntrustRewardPreview, _onBtnEntrustRewardPreviewClick);
            ALUGUICommon.combineBtnClick(wnd.btnDeal, _onDealBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnDealRecord, _onBtnDealRecordClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnEntrustRewardPreview, _onBtnEntrustRewardPreviewClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnDeal, _onDealBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnDealRecord, _onBtnDealRecordClick);
            }
            
            _m_wBannerImg?.discard();
            _m_wBannerImg = null;
            
            _m_wEntrustProgress?.discard();
            _m_wEntrustProgress = null;

            if (_m_wAutoDealToggle != null)
            {
                _m_wAutoDealToggle.clickDelegate -= _onAutoDealToggleClick;
                _m_wAutoDealToggle.discard();
                _m_wAutoDealToggle = null;    
            }
            
            _m_wDealLazyCd?.discard();
            _m_wDealLazyCd = null;

            if (_m_lSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }

            if (_m_lMultipleSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lMultipleSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lMultipleSfxObjList.Clear();
                _m_lMultipleSfxObjList = null;
            }

            _m_tipMgr?.clear();
            _m_tipMgr = null;
        }

        protected override void _onShowWnd()
        {
            if(_m_wAutoDealToggle != null)
            {
                _m_wAutoDealToggle.showWnd();
                _m_wAutoDealToggle.setSelected(AccountSettingMgr.instance.accountSetting.guildAutoDealEntrust);
            }

            //清空tip
            _m_tipMgr?.start();

            _refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_ENTRUST_INFO_CHG, _onEntrustInfoChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_ENTRUST_CHG_NEW, _onEntrustChgNew);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_ENTRUST_INFO_CHG, _onEntrustInfoChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_ENTRUST_CHG_NEW, _onEntrustChgNew);
            
            _m_wBannerImg?.hideWnd();
            _m_wEntrustProgress?.hideWnd();
            _m_wAutoDealToggle?.hideWnd();
            _m_wDealLazyCd?.hideWnd();
            _m_tipMgr?.clear();

            if (_m_lSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }

            if (_m_lMultipleSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lMultipleSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lMultipleSfxObjList.Clear();
                _m_lMultipleSfxObjList = null;
            }
        }

        protected override void _onReset()
        {
            _m_wBannerImg?.discardTexture();
            _m_wEntrustProgress?.resetWnd();
            _m_wAutoDealToggle?.resetWnd();
            _m_wDealLazyCd?.resetWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            GuildEntrustInfo guildEntrustInfo = NPPlayer.instance.guildComp.guildInfo?.guildEntrustInfo;
            if(wnd == null || guildEntrustInfo == null || guildEntrustInfo.entrustQualityRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtQuality, GCommon.addColorForRichText(TextTranslate.instance.getLanguage(guildEntrustInfo.entrustQualityRefObj.quality_name), guildEntrustInfo.entrustQualityRefObj.name_color));
            ALUGUICommon.setUIObjColor(wnd.changeColorList, guildEntrustInfo.entrustQualityRefObj.name_color);
            if (_m_wBannerImg != null)
            {
                _m_wBannerImg.showWnd();
                _m_wBannerImg.setTexture(guildEntrustInfo.entrustEventRefObj?.banner_tex);
            }

            ALUGUICommon.setLabelTxt(wnd.txtEntrustName, TextTranslate.instance.getLanguage(guildEntrustInfo.entrustEventRefObj?.name));
            ALUGUICommon.setLabelTxt(wnd.txtEntrustDesc, TextTranslate.instance.getLanguage(guildEntrustInfo.entrustEventRefObj?.desc));
            
            ALUGUICommon.setLabelTxt(wnd.txtPerDealGainSilver, guildEntrustInfo.getPerDealGainCoin().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));

            // 刷新委托进度
            _refreshEntrustProgress();
            
            // 处理事件消耗lazycd
            if(_m_wDealLazyCd != null)
            {
                _m_wDealLazyCd.showWnd();
                _m_wDealLazyCd.setInfo(GRefdataCoreMgr.instance.npGeneral.deal_entrust_lazy_cd_id);
            }
        }
        
        /// <summary>
        /// 刷新委托进度
        /// </summary>
        private void _refreshEntrustProgress()
        {
            GuildEntrustInfo guildEntrustInfo = NPPlayer.instance.guildComp.guildInfo?.guildEntrustInfo;
            if(wnd == null || guildEntrustInfo == null || guildEntrustInfo?.entrustQualityRefObj == null)
                return;

            if (_m_wEntrustProgress != null)
            {
                _m_wEntrustProgress.showWnd();
                _m_wEntrustProgress.setProgress(guildEntrustInfo.point, guildEntrustInfo.entrustQualityRefObj.count, EValueFormatType.NORMAL_NOT_LARGE_STR);
            }
        }

        /// <summary>
        /// 暴击倍数提示tip
        /// </summary>
        /// <param name="_multiple"></param>
        private void _showMultipleTip(long _multiple)
        {
            if (wnd == null || _m_tipMgr == null)
                return;

            NPCenterTipsRefObj tipsRef = GRefdataCoreMgr.instance.tipMap.getRef(wnd.multipleCenterTipID);
            if (tipsRef != null)
            {
                //超过1倍才显示tip
                if (_multiple <= 1)
                    return;

                //====显示tip====
                string tipStr = TextTranslate.instance.getLanguage(TransKeyConst.guild_entrustMultipleTip_num, _multiple);
                _m_tipMgr?.addTip(new NPTextTipDealer(new List<string>() {tipStr}, tipsRef, (_tipWnd) =>
                {
                    if (wnd == null || !isShow)
                        return;

                    //====播放暴击特效====
                    if (_m_lMultipleSfxObjList == null)
                        _m_lMultipleSfxObjList = new List<CommonUISfxObj>();

                    //先清除超出数量的特效
                    while (_m_lMultipleSfxObjList.Count >= wnd.maxSfxCount)
                    {
                        CommonUISfxObj oldSfxObj = _m_lMultipleSfxObjList[0];
                        oldSfxObj?.forceDiscard();
                        _m_lMultipleSfxObjList.RemoveAt(0);
                    }

                    //播放新的暴击特效
                    if (wnd.multipleSfxId > 0 && wnd.entrustSfxParent != null)
                    {
                        CommonUISfxObj newSfxObj = PlaySfxMgr.instance.playUISfx(wnd.multipleSfxId, wnd.entrustSfxParent);
                        _m_lMultipleSfxObjList.Add(newSfxObj);
                    }
                }));
            }
        }

        #region 点击按钮

        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_ENTRUST);
        }
        
        /// <summary>
        /// 自动处理Toggle点击
        /// </summary>
        private void _onAutoDealToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if(_toggle == null)
                return;

            bool isOn = !_toggle.isOn;
            AccountSettingMgr.instance.accountSetting.setGuildAutoDealEntrust(isOn);
         
            _m_wAutoDealToggle?.setSelected(isOn);
        }

        /// <summary>
        /// 委托奖励预览按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnEntrustRewardPreviewClick(GameObject _go)
        {
            // 展示奖励预览弹窗
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildEntrustRewardPreview.instance, () =>
            {
                GGUIWndGuildEntrustRewardPreview.instance.showWnd();
            });
        }
        
        /// <summary>
        /// 处理按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onDealBtnClick(GameObject _go)
        {
            _dealEntrust();
        }
        /// <summary>
        /// 处理委托
        /// </summary>
        private void _dealEntrust()
        {
            // 若处理需要的lazycd不足，则不处理
            if (!GCommon.lazycdEnough(GRefdataCoreMgr.instance.npGeneral.deal_entrust_lazy_cd_id, 1, true))
            {
                return;
            }
            
            // 每次处理委托获取的金币数量
            long perDealGainCoin = NPPlayer.instance.guildComp.guildInfo?.guildEntrustInfo?.getPerDealGainCoin() ?? 0;
            NPPlayer.instance.guildComp.reqGuildEntrust((_msg) =>
            {
                if(wnd == null || _msg == null || !isShow)
                    return;

                // GCommon.showRewardTip(ENPItemType.CURRENCY, (long) ECurrency.SILVER, perDealGainCoin);

                //当没有触发暴击时，播放普通特效
                if (_msg.getCritMul() == 1)
                {
                    if (_m_lSfxObjList == null)
                        _m_lSfxObjList = new List<CommonUISfxObj>();

                    //先清除超出数量的特效
                    while (_m_lSfxObjList.Count >= wnd.maxSfxCount)
                    {
                        CommonUISfxObj oldSfxObj = _m_lSfxObjList[0];
                        oldSfxObj?.forceDiscard();
                        _m_lSfxObjList.RemoveAt(0);
                    }

                    //播放处理成功特效
                    if (wnd.entrustSfxId > 0 && wnd.entrustSfxParent != null)
                    {
                        CommonUISfxObj newSfxObj = PlaySfxMgr.instance.playUISfx(wnd.entrustSfxId, wnd.entrustSfxParent);
                        _m_lSfxObjList.Add(newSfxObj);
                    }
                }

                //上浮倍数提示
                _showMultipleTip(_msg.getCritMul());

                // 若勾选了自动处理委托, 则继续处理
                if (AccountSettingMgr.instance.accountSetting.guildAutoDealEntrust)
                    _dealEntrust();
                
            }, null);
        }
        
        /// <summary>
        /// 处理记录按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnDealRecordClick(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildEntrustRecord.instance, () =>
            {
                GGUIWndGuildEntrustRecord.instance.showWnd();
            }, UINodeTagConst_Guild.C_GUILD_ENTRUST_RECORD);
        }

        #endregion

        #region 窗口消息

        /// <summary>
        /// 委托信息变化
        /// </summary>
        private void _onEntrustInfoChg()
        {
            _refreshEntrustProgress();
        }

        /// <summary>
        /// 更换新委托
        /// </summary>
        private void _onEntrustChgNew()
        {
            _refreshWnd();
        }

        #endregion
    }
}