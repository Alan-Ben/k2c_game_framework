using System;
using ALPackage;
using Common.GuildCooperateObj;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using System.Collections.Generic;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作属性据点详情
    /// </summary>
    public class GGUIWndGuildCooperateAttrPosDetail : _ANPGGUIBasicWnd<GGUIMonoGuildCooperateAttrPosDetail>
    {
        private static GGUIWndGuildCooperateAttrPosDetail _g_instance;
        public static GGUIWndGuildCooperateAttrPosDetail instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndGuildCooperateAttrPosDetail();
                return _g_instance;
            }
        }

        // 当前伙伴列表
        [NotNull] private List<HeroInfo> _m_lCurHeroList = new List<HeroInfo>();
        // 属性据点信息
        private GuildCooperatePropertyPointInfo _m_pointInfo;
        // 奖励据点配置
        private GuildCooperateAreaPosRefObj _m_areaPosRef;
        // 奖励据点图标
        private NPGGuiWndTexture _m_wPosIcon;
        // 属性图标
        private NPGGuiWndTexture _m_wAttrIcon;
        // 血量进度条
        private GGUISubWndCommonBlood _m_wHpProgress;
        // 筛选子窗口
        private GGUIWndHeroMainFilter _m_wHeroListFilter;
        // 伙伴列表容器
        private GGUIWndGuildCooperateSelectHeroContainer _m_wHeroListContainer;
        // 恢复消耗item
        private NPGGUIWndCommonItem _m_wRecoverCostItem;
        // 自动派遣开关
        private NPGGUIWndCommonToggleEx _m_wAutoDispatchToggle;
        // 上个已建设值
        private long _m_lLastValue;
        // 当前筛选类型
        private ESpecAttrType _m_eCurFilterType;
        // 是否正在处理派遣
        private bool _m_bIsDealingDispatch;
        // 自动派遣序列号
        private long _m_lAutoSerialize;
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;
        //进度条特效列表
        private List<CommonUISfxObj> _m_lProgressSfxObjList;
        //显示序列号
        private long _m_lShowSerialize;
        //建设值上浮tip管理器
        private NPGGUICommonTipDealerMgr _m_constructTipMgr;

        public GGUIWndGuildCooperateAttrPosDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildCooperateAttrPosDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildCooperateAttrPosDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_COOPERATE_PROPERTY_POINT_CHG, _onAttrPointChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_RESET, _onGuildCooperateReset);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_DISPATCH_HERO, _onSimulateClickDispatchHero);
            _m_lAutoSerialize = ALSerializeOpMgr.next();
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_constructTipMgr?.start();
            _checkIsReset();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_COOPERATE_PROPERTY_POINT_CHG, _onAttrPointChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_RESET, _onGuildCooperateReset);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_DISPATCH_HERO, _onSimulateClickDispatchHero);
            _m_wPosIcon?.hideWnd();
            _m_wAttrIcon?.hideWnd();
            _m_wHpProgress?.hideWnd();
            _m_wHeroListFilter?.hideWnd();
            _m_wHeroListContainer?.hideWnd();
            _m_wRecoverCostItem?.hideWnd();
            _m_wAutoDispatchToggle?.hideWnd();

            _m_bIsDealingDispatch = false;
            _m_lAutoSerialize = ALSerializeOpMgr.next();
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_constructTipMgr?.clear();

            if (_m_lSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
            if (_m_lProgressSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lProgressSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lProgressSfxObjList.Clear();
                _m_lProgressSfxObjList = null;
            }
        }

        protected override void _onReset()
        {
            _m_wPosIcon?.discardTexture();
            _m_wAttrIcon?.discardTexture();
            _m_wHpProgress?.resetWnd();
            _m_wHeroListFilter?.resetWnd();
            _m_wHeroListContainer?.resetWnd();
            _m_wRecoverCostItem?.resetWnd();
            _m_wAutoDispatchToggle?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wPosIcon?.discard();
            _m_wPosIcon = null;
            _m_wAttrIcon?.discard();
            _m_wAttrIcon = null;
            _m_wHpProgress?.discard();
            _m_wHpProgress = null;
            _m_wHeroListFilter?.discard();
            _m_wHeroListFilter = null;
            _m_wHeroListContainer?.discard();
            _m_wHeroListContainer = null;
            _m_wRecoverCostItem?.discard();
            _m_wRecoverCostItem = null;
            _m_wAutoDispatchToggle?.discard();
            _m_wAutoDispatchToggle = null;
            if (_m_lSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
            if (_m_lProgressSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lProgressSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lProgressSfxObjList.Clear();
                _m_lProgressSfxObjList = null;
            }

            _m_constructTipMgr?.clear();
            _m_constructTipMgr = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnDispatch, _onClickDispatch);
            ALUGUICommon.uncombineBtnClick(wnd.btnRecover, _onClickRecover);
            ALUGUICommon.uncombineBtnClick(wnd.btnStopAuto, _onClickStopAuto);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgPosIcon != null)
                _m_wPosIcon = new NPGGuiWndTexture(wnd.imgPosIcon);

            if (wnd.imgAttrIcon != null)
                _m_wAttrIcon = new NPGGuiWndTexture(wnd.imgAttrIcon);

            if (wnd.monoHP != null)
                _m_wHpProgress = new GGUISubWndCommonBlood(wnd.monoHP);

            if (wnd.monoFilter != null)
            {
                _m_wHeroListFilter = new GGUIWndHeroMainFilter(wnd.monoFilter);
                _m_wHeroListFilter.initState(ESpecAttrType.NONE);
                _m_wHeroListFilter.onFilterChanged += _onFilterChg;
            }

            if (wnd.monoHeroContainer != null)
            {
                _m_wHeroListContainer = new GGUIWndGuildCooperateSelectHeroContainer(wnd.monoHeroContainer);
                _m_wHeroListContainer.onItemIndexChanged += _onHeroListCurIndexChg;
            }

            if (wnd.monoRecoverItem != null)
                _m_wRecoverCostItem = new NPGGUIWndCommonItem(wnd.monoRecoverItem);

            if (wnd.monoAutoToggle != null)
            {
                _m_wAutoDispatchToggle = new NPGGUIWndCommonToggleEx(wnd.monoAutoToggle);
                _m_wAutoDispatchToggle.clickDelegate += _onClickAutoToggle;
            }

            if (wnd.constructCenterTipParent != null)
                _m_constructTipMgr = new NPGGUICommonTipDealerMgr(wnd.constructCenterTipParent);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnDispatch, _onClickDispatch);
            ALUGUICommon.combineBtnClick(wnd.btnRecover, _onClickRecover);
            ALUGUICommon.combineBtnClick(wnd.btnStopAuto, _onClickStopAuto);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(GuildCooperatePropertyPointInfo _info, GuildCooperateAreaPosRefObj _areaPosRef)
        {
            _m_pointInfo = _info;
            _m_areaPosRef = _areaPosRef;
            _m_lLastValue = _m_pointInfo?.hadAttackHp ?? 0;
            _m_eCurFilterType = _info != null ? _info.attr : ESpecAttrType.NONE;
            _m_bIsDealingDispatch = false;

            //检查当前筛选类型是否有可用顾问
            if (_m_eCurFilterType != ESpecAttrType.NONE)
            {
                bool canUse = false;
                NPPlayer.instance.heroComponent.dealAllHero(_heroInfo =>
                {
                    //属性符合并且顾问可用
                    if (_heroInfo != null && _heroInfo.specAttrType == _m_eCurFilterType && NPPlayer.instance.guildCooperateComp.isHeroCanUse(_heroInfo.id))
                        canUse = true;
                });
                //如果都顾问不可用，则默认选择全部
                if (!canUse)
                    _m_eCurFilterType = ESpecAttrType.NONE;
            }

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            _refreshBaseInfo();
            _refreshHpProgress();
            _refreshHeroList();
            _refreshStateShow();
            _refreshRecoverCost();
        }

        /// <summary>
        /// 刷新基础信息
        /// </summary>
        private void _refreshBaseInfo()
        {
            if (wnd == null || _m_pointInfo == null)
                return;

            BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) _m_pointInfo.attr);

            // 刷新属性据点名称
            ALUGUICommon.setLabelTxt(wnd.txtAttrPosName, _m_pointInfo.posName);
            // 派遣描述提示
            ALUGUICommon.setLabelTxt(wnd.txtDispatchDesc, TextTranslate.instance.getLanguage(TransKeyConst.guildCooperate_dispatchHeroDesc_str, basicAttrRef?.name));
            // 奖励据点图标
            _m_wPosIcon?.showWnd();
            _m_wPosIcon?.setTexture(_m_areaPosRef?.icon);
            //属性图标
            _m_wAttrIcon?.showWnd();
            _m_wAttrIcon?.setTexture(basicAttrRef?.icon);
            // 刷新筛选选择
            _m_wHeroListFilter?.showWnd();
            _m_wHeroListFilter?.initState(_m_eCurFilterType);
            _m_wHeroListFilter?.setShowTag(_m_pointInfo.attr);
            // 刷新自动派遣开关
            _m_wAutoDispatchToggle?.showWnd();
            _m_wAutoDispatchToggle?.setSelected(false, true);
            ALUGUICommon.setGameObjEnable(wnd.goAutoDispatchShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goAutoDispatchHideList, true);
        }

        /// <summary>
        /// 刷新血量进度条
        /// </summary>
        private void _refreshHpProgress()
        {
            if (wnd == null || _m_pointInfo == null || _m_wHpProgress == null)
                return;

            _m_wHpProgress.showWnd();
            _m_wHpProgress.setBloodData(_m_lLastValue, _m_pointInfo.totalHp);

            long cureValue = _m_pointInfo.hadAttackHp - _m_lLastValue;
            _m_lLastValue = _m_pointInfo.hadAttackHp;

            // 播放增加效果
            if (cureValue > 0)
                _m_wHpProgress.cure(cureValue);
        }

        /// <summary>
        /// 刷新伙伴列表
        /// </summary>
        private void _refreshHeroList()
        {
            if (_m_pointInfo == null)
                return;

            _m_lCurHeroList.Clear();
            NPPlayer.instance.heroComponent.dealAllHero(_heroInfo =>
            {
                if(_m_eCurFilterType == ESpecAttrType.NONE || (_heroInfo != null && _heroInfo.specAttrType == _m_eCurFilterType))
                    _m_lCurHeroList.Add(_heroInfo);
            });
            // 排序：可派遣 > 不可派遣，实力降序 > ID升序
            _m_lCurHeroList.Sort(_sortHeroList);
            // 获取可用的第一个伙伴索引
            int firstCanUseHeroIndex = 0;
            for (int i = 0; i < _m_lCurHeroList.Count; i++)
            {
                if (_m_lCurHeroList[i] != null && NPPlayer.instance.guildCooperateComp.isHeroCanUse(_m_lCurHeroList[i].id))
                {
                    firstCanUseHeroIndex = i;
                    break;
                }
            }

            _m_wHeroListContainer?.showWnd();
            _m_wHeroListContainer?.setInfo(_m_lCurHeroList, _m_pointInfo.attr, 0, _checkCanClickHeroItem);
            ALCommonActionMonoTask.addLaterMonoTask(() =>
            {
                //选中当前可用的第一个伙伴
                _m_wHeroListContainer?.fixToItemIndex(firstCanUseHeroIndex, false);
            });
        }

        /// <summary>
        /// 刷新状态显隐
        /// </summary>
        private void _refreshStateShow()
        {
            if (wnd == null)
                return;

            HeroInfo curHeroInfo = _m_wHeroListContainer?.curSelectedItem?.heroInfo;
            // 是否可以使用该伙伴
            bool canUse = curHeroInfo != null && NPPlayer.instance.guildCooperateComp.isHeroCanUse(curHeroInfo.id);
            // 是否可以恢复
            bool canRecover = NPPlayer.instance.fixedCdComp.getCount(GRefdataCoreMgr.instance.npGeneral.guild_cooperate_dispatch_time_reset_limit_fix_cd_id) > 0;

            wnd.setShowState(canUse, canRecover);
        }

        /// <summary>
        /// 刷新恢复消耗道具
        /// </summary>
        private void _refreshRecoverCost()
        {
            // 刷新恢复消耗
            _m_wRecoverCostItem?.showWnd();
            _m_wRecoverCostItem?.setItem(GRefdataCoreMgr.instance.npGeneral.guild_cooperate_dispatch_time_reset_cost);
        }

        /// <summary>
        /// 伙伴列表排序：可派遣 > 不可派遣，实力降序 > ID升序
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        private int _sortHeroList(HeroInfo _a, HeroInfo _b)
        {
            if (_a == null && _b == null) return 0;
            if (_a == null) return 1;
            if (_b == null) return -1;

            // 可派遣 > 不可派遣
            bool aCanUse = NPPlayer.instance.guildCooperateComp.isHeroCanUse(_a.id);
            bool bCanUse = NPPlayer.instance.guildCooperateComp.isHeroCanUse(_b.id);
            if (aCanUse != bCanUse) 
                return aCanUse ? -1 : 1;

            // 实力降序
            int powerComparison = _a.power.CompareTo(_b.power);
            if (powerComparison != 0) 
                return -powerComparison;

            // ID升序
            return _a.id.CompareTo(_b.id);
        }

        /// <summary>
        /// 检查是否可以点击伙伴列表，用于阻止派遣过程中点击
        /// </summary>
        /// <returns></returns>
        private bool _checkCanClickHeroItem()
        {
            return !_m_bIsDealingDispatch;
        }

        /// <summary>
        /// 检查是否重置了
        /// </summary>
        private void _checkIsReset()
        {
            // 如果刷新时间不一致说明发生了重置
            if (NPPlayer.instance.guildCooperateComp.recordCurShowRefreshTimeMs != NPPlayer.instance.guildCooperateComp.nextRefreshTimeMs)
                _onGuildCooperateReset();
        }

        /// <summary>
        /// 检查是否建设结束
        /// </summary>
        private bool _checkIsConstructFinish()
        {
            if (_m_pointInfo == null)
                return true;

            // 判断血量为是否0
            if (_m_pointInfo.leftHp <= 0)
            {
                // 停止自动派遣
                _dealStopAutoDispatch();

                // 弹窗完成提示
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildCooperateAttrPosFinish.instance, () =>
                {
                    GGUIWndGuildCooperateAttrPosFinish.instance.showWnd();
                    GGUIWndGuildCooperateAttrPosFinish.instance.setInfo(_m_pointInfo.attr, _m_areaPosRef);
                }, UINodeTagConst.C_GUIlD_COOPERATE_ATTR_POINT_FINISH);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 播放建设特效
        /// </summary>
        private void _playConstructSfx()
        {
            if (wnd == null || !isShow)
                return;

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
            if (wnd.sfxId > 0 && wnd.sfxParent != null)
            {
                CommonUISfxObj newSfxObj = PlaySfxMgr.instance.playUISfx(wnd.sfxId, wnd.sfxParent);
                _m_lSfxObjList.Add(newSfxObj);
            }
        }

        /// <summary>
        /// 播放进度条特效
        /// </summary>
        private void _playProgressSfx()
        {
            if (wnd == null || !isShow)
                return;

            if (_m_lProgressSfxObjList == null)
                _m_lProgressSfxObjList = new List<CommonUISfxObj>();

            //先清除超出数量的特效
            while (_m_lProgressSfxObjList.Count >= wnd.progressMaxSfxCount)
            {
                CommonUISfxObj oldSfxObj = _m_lProgressSfxObjList[0];
                oldSfxObj?.forceDiscard();
                _m_lProgressSfxObjList.RemoveAt(0);
            }

            //播放进度条特效
            if (wnd.progressSfxId > 0 && wnd.progressSfxParent != null)
            {
                CommonUISfxObj newSfxObj = PlaySfxMgr.instance.playUISfx(wnd.progressSfxId, wnd.progressSfxParent);
                _m_lProgressSfxObjList.Add(newSfxObj);
            }
        }

        #region 派遣表现流程

        /// <summary>
        /// 处理单个派遣表现流程
        /// </summary>
        private void _dealSingleDispatchProcess(List<NPCommon_ItemInfo> _itemList, Action _onAllDone = null)
        {
            // 先屏蔽列表拖拽
            if(_m_wHeroListContainer != null && _m_wHeroListContainer.wnd != null && _m_wHeroListContainer.wnd.scrollRect != null)
                _m_wHeroListContainer.wnd.scrollRect.enabled = false;
            // 设置处理派遣表现中
            _m_bIsDealingDispatch = true;

            ALProcess process = ALProcess.CreateProcess("guildCooperateDispatchHero");
            process
                .addProcess(() =>
                {
                    // ====奖励上浮提示====
                    if (_itemList == null || _itemList.Count == 0)
                        return;

                    NPCommonItem constructItem = GRefdataCoreMgr.instance.npGeneral.guild_cooperate_construction_common_item;
                    List<NPCommon_ItemInfo> otherItemList = null;

                    foreach (NPCommon_ItemInfo itemInfo in _itemList)
                    {
                        if (itemInfo == null)
                            continue;

                        if (constructItem != null && (int)constructItem.itemType == itemInfo.getItemType() && constructItem.itemId == itemInfo.getSubId())
                        {
                            // 建设值道具，单独展示
                            if (wnd != null && _m_constructTipMgr != null)
                            {
                                NPCenterTipsRefObj tipsRef = GRefdataCoreMgr.instance.tipMap.getRef(wnd.constructCenterTipID);
                                if (tipsRef != null)
                                {
                                    NPGTextureIndex icon = GCommon.getItemTexIcon((ENPItemType)itemInfo.getItemType(), itemInfo.getSubId());
                                    string text = GCommon.getCommaValueStr(itemInfo.getCount());
                                    _m_constructTipMgr.addTip(new NPIconTextTipDealer(icon, text, tipsRef, null));
                                }
                            }
                        }
                        else
                        {
                            if (otherItemList == null)
                                otherItemList = new List<NPCommon_ItemInfo>();
                            otherItemList.Add(itemInfo);
                        }
                    }
                    //其他道具使用通用tip展示
                    GCommon.showGainRewardTip(otherItemList);
                })
                .addProcess(() =>
                {
                    // ====延时播放特效====
                    if (wnd == null)
                        return;

                    long showSerialize = _m_lShowSerialize;
                    //延时播放建设特效
                    ALCommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        if (_m_lShowSerialize != showSerialize)
                            return;

                        //播放特效
                        _playConstructSfx();
                    }, wnd.delayShowSfxTime);

                    //延时播放进度条特效
                    ALCommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        if (_m_lShowSerialize != showSerialize)
                            return;

                        //刷新进度
                        _refreshHpProgress();
                        //播放进度条特效
                        _playProgressSfx();
                    }, wnd.delayShowProgressSfxTime);
                })
                .addDelegateProcess((_onDone) =>
                {
                    // ====播放派遣动画====
                    if (_m_wHeroListContainer != null && _m_wHeroListContainer.curSelectedItem != null)
                    {
                        _m_wHeroListContainer.curSelectedItem.playDispath(_onDone);
                    }
                    else
                        _onDone?.Invoke();
                })
                .addProcess(() =>
                {
                    // ====刷新伙伴列表====
                    _m_wHeroListContainer?.refreshAllItem();
                })
                .addDelegateProcess((_onDone) =>
                {
                    // ====移动伙伴列表到下一个====
                    //如果已经没有血量了，直接结束
                    if (_m_wHeroListContainer == null || _m_pointInfo == null || _m_pointInfo.leftHp <= 0)
                    {
                        _onDone?.Invoke();
                        return;
                    }

                    int nextIndex = _m_wHeroListContainer.currentItemIndex + 1;
                    if (nextIndex >= _m_lCurHeroList.Count)
                        nextIndex = _m_lCurHeroList.Count - 1;
                    _m_wHeroListContainer?.fixToItemIndex(nextIndex, true);
                    ALCommonActionMonoTask.addMonoTask(()=>
                    {
                        _refreshStateShow();
                        _onDone?.Invoke();
                    }, _m_wHeroListContainer.fixPosDuration);
                })
                .addProcess(() =>
                {
                    // ====检查是否建设完成====
                    _checkIsConstructFinish();
                })
                .addProcess(() =>
                {
                    // ====设置派遣完成====
                    // 恢复列表拖拽
                    if (_m_wHeroListContainer != null && _m_wHeroListContainer.wnd != null && _m_wHeroListContainer.wnd.scrollRect != null)
                        _m_wHeroListContainer.wnd.scrollRect.enabled = true;
                    _m_bIsDealingDispatch = false;
                    _onAllDone?.Invoke();
                })
                .deal();
        }

        /// <summary>
        /// 处理自动派遣表现流程
        /// </summary>
        private void _dealAutoDispatchProcess(List<NPCommon_ItemInfo> _itemList, long _autoSerialize)
        {
            ALProcess process = ALProcess.CreateProcess("guildCooperateDispatchHero");
            process
                .addDelegateProcess((_onDone) =>
                {
                    // ====单个派遣表现====
                    if (!_checkCanAutoDispatch(_autoSerialize))
                        return;

                    _dealSingleDispatchProcess(_itemList, _onDone);
                })
                .addDelegateProcess(_onDone =>
                {
                    // ====检查列表后面是否有可使用伙伴====
                    if (!_checkCanAutoDispatch(_autoSerialize))
                        return;

                    if (_m_wHeroListContainer == null)
                        return;

                    bool haveCanUseHero = false;
                    for (int i = _m_wHeroListContainer.currentItemIndex; i < _m_lCurHeroList.Count; i++)
                    {
                        if (NPPlayer.instance.guildCooperateComp.isHeroCanUse(_m_lCurHeroList[i].id))
                        {
                            haveCanUseHero = true;
                            break;
                        }
                    }

                    if (haveCanUseHero)
                    {
                        // 先屏蔽列表拖拽
                        if (_m_wHeroListContainer != null && _m_wHeroListContainer.wnd != null && _m_wHeroListContainer.wnd.scrollRect != null)
                            _m_wHeroListContainer.wnd.scrollRect.enabled = false;
                        // 设置处理派遣表现中
                        _m_bIsDealingDispatch = true;

                        _onDone?.Invoke();
                    }
                    else
                    {
                        // 没有可使用伙伴，停止自动派遣
                        _dealStopAutoDispatch();
                    }
                })
                .addDelegateProcess(_onDone =>
                {
                    // ====移动到可使用的伙伴位置====
                    if (!_checkCanAutoDispatch(_autoSerialize))
                        return;

                    _moveToCanDispatchHeroItem(_autoSerialize, _onDone);
                })
                .addProcess(() =>
                {
                    // ====点击派遣====
                    _m_bIsDealingDispatch = false;
                    _onClickDispatch(null);
                })
                .deal();
        }

        /// <summary>
        /// 移动列表到可派遣的伙伴item
        /// </summary>
        /// <param name="_autoSerialize"></param>
        /// <param name="_onDone"></param>
        private void _moveToCanDispatchHeroItem(long _autoSerialize, Action _onDone)
        {
            if (_m_lCurHeroList.Count <= 0 || _m_wHeroListContainer == null || _m_wHeroListContainer.currentItemIndex >= _m_lCurHeroList.Count || !_checkCanAutoDispatch(_autoSerialize))
                return;

            // 当前可派遣，直接返回
            if (NPPlayer.instance.guildCooperateComp.isHeroCanUse(_m_lCurHeroList[_m_wHeroListContainer.currentItemIndex].id))
            {
                _onDone?.Invoke();
                return;
            }

            // 还有下个数据，处理移动
            if (_m_wHeroListContainer.currentItemIndex + 1 < _m_lCurHeroList.Count)
            {
                _m_wHeroListContainer.fixToItemIndex(_m_wHeroListContainer.currentItemIndex + 1, true);
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (wnd == null || !isShow || !_checkCanAutoDispatch(_autoSerialize))
                        return;

                    _refreshStateShow();
                    _moveToCanDispatchHeroItem(_autoSerialize, _onDone);
                }, _m_wHeroListContainer.fixPosDuration);
            }
        }

        /// <summary>
        /// 检查是否可以自动派遣
        /// </summary>
        /// <param name="_autoSerialize"></param>
        private bool _checkCanAutoDispatch(long _autoSerialize)
        {
            if (_autoSerialize != _m_lAutoSerialize)
            {
                // 恢复列表拖拽
                if (_m_wHeroListContainer != null && _m_wHeroListContainer.wnd != null && _m_wHeroListContainer.wnd.scrollRect != null)
                    _m_wHeroListContainer.wnd.scrollRect.enabled = true;
                // 设置不在派遣中
                _m_bIsDealingDispatch = false;
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// 停止自动派遣
        /// </summary>
        private void _dealStopAutoDispatch()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goAutoDispatchHideList, true);
            ALUGUICommon.setGameObjEnable(wnd.goAutoDispatchShowList, false);
            _m_lAutoSerialize = ALSerializeOpMgr.next();

            // 恢复列表拖拽
            if (_m_wHeroListContainer != null && _m_wHeroListContainer.wnd != null && _m_wHeroListContainer.wnd.scrollRect != null)
                _m_wHeroListContainer.wnd.scrollRect.enabled = true;
        }

        #endregion

        #region 消息事件

        /// <summary>
        /// 属性据点信息变化
        /// </summary>
        /// <param name="_objects"></param>
        private void _onAttrPointChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 3 || _m_pointInfo == null)
                return;

            long areaId = (long)_objects[0];
            int rewardPointIndex = (int)_objects[1];
            int attrIndex = (int)_objects[2];
            if (_m_pointInfo.areaId == areaId && _m_pointInfo.rewardPointIndex == rewardPointIndex && _m_pointInfo.index == attrIndex)
            {
                //如果不是在处理派遣中，刷新进度和播放特效
                if (!_m_bIsDealingDispatch)
                {
                    //刷新进度
                    _refreshHpProgress();
                    //检查是否建设结束
                    _checkIsConstructFinish();
                }
            }
        }

        /// <summary>
        /// 公会协作重置
        /// </summary>
        private void _onGuildCooperateReset()
        {
            // 弹窗提示，确认后退出到联盟主界面
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.guildCooperate_resetNoticeContent_none),
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    QueueMgr.instance.QuitUntilCanStop(_node => _node.nodeTag == UINodeTagConst_Guild.C_GUILD_MAIN);
                },
                true,
                TransKeyConst.guildCooperate_resetNoticeTitle_none);
        }

        /// <summary>
        /// 模拟点击派遣
        /// </summary>
        private void _onSimulateClickDispatchHero()
        {
            _onClickDispatch(null);
        }

        #endregion

        #region 点击事件

        /// <summary>
        /// 筛选变更
        /// </summary>
        /// <param name="_type"></param>
        private void _onFilterChg(ESpecAttrType _type)
        {
            _m_eCurFilterType = _type;
            _dealStopAutoDispatch();
            _refreshHeroList();
            _refreshStateShow();
        }

        /// <summary>
        /// 伙伴列表选中索引变化
        /// </summary>
        private void _onHeroListCurIndexChg()
        {
            _refreshStateShow();
        }

        /// <summary>
        /// 点击自动派遣开关
        /// </summary>
        /// <param name="_toggle"></param>
        private void _onClickAutoToggle(NPGGUIWndCommonToggleEx _toggle)
        {
            _m_wAutoDispatchToggle?.setSelected(!_m_wAutoDispatchToggle.isOn, true);
            
            // 停止自动派遣
            if (_m_wAutoDispatchToggle != null && !_m_wAutoDispatchToggle.isOn && _m_bIsDealingDispatch)
                _dealStopAutoDispatch();
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_COOPERATE_ATTR_POINT_DETAIL);
        }

        /// <summary>
        /// 点击派遣
        /// </summary>
        private void _onClickDispatch(GameObject _go)
        {
            // 是否正在处理派遣中
            if (_m_bIsDealingDispatch)
                return;

            HeroInfo curHeroInfo = _m_wHeroListContainer?.curSelectedItem?.heroInfo;
            if (wnd == null || curHeroInfo == null || _m_pointInfo == null)
                return;

            // 是否可派遣
            if (!NPPlayer.instance.guildCooperateComp.isHeroCanUse(curHeroInfo.id))
                return;

            // 是否还有血量
            if (_m_pointInfo.leftHp <= 0)
                return;

            // 设置信息
            _m_bIsDealingDispatch = true;
            GuildCooperate_RewardPointPos rewardPos = new GuildCooperate_RewardPointPos();
            rewardPos.setAreaId(_m_pointInfo.areaId);
            rewardPos.setIndex(_m_pointInfo.rewardPointIndex);

            if (_m_wAutoDispatchToggle != null && _m_wAutoDispatchToggle.isOn)
            {
                // 自动派遣
                ALUGUICommon.setGameObjEnable(wnd.goAutoDispatchHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goAutoDispatchShowList, true);

                NPPlayer.instance.guildCooperateComp.reqAttackPropertyPoint(rewardPos, _m_pointInfo.index, curHeroInfo.id, (_isSuc, _msg) =>
                {
                    if (!_isSuc)
                        return;
                    _dealAutoDispatchProcess(_msg?.getItemList(), _m_lAutoSerialize);
                });
            }
            else
            {
                // 普通派遣
                ALUGUICommon.setGameObjEnable(wnd.goAutoDispatchHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goAutoDispatchShowList, false);

                NPPlayer.instance.guildCooperateComp.reqAttackPropertyPoint(rewardPos, _m_pointInfo.index, curHeroInfo.id, (_isSuc, _msg) =>
                {
                    if (!_isSuc)
                        return;
                    _dealSingleDispatchProcess(_msg?.getItemList());
                });
            }
        }

        /// <summary>
        /// 点击恢复
        /// </summary>
        private void _onClickRecover(GameObject _go)
        {
            // 是否正在处理派遣中
            if (_m_bIsDealingDispatch)
                return;

            HeroInfo curHeroInfo = _m_wHeroListContainer?.curSelectedItem?.heroInfo;
            if (curHeroInfo == null || _m_pointInfo == null || NPPlayer.instance.guildCooperateComp.isHeroCanUse(curHeroInfo.id))
                return;

            // 检查是否有恢复次数
            if (NPPlayer.instance.fixedCdComp.getCount(GRefdataCoreMgr.instance.npGeneral.guild_cooperate_dispatch_time_reset_limit_fix_cd_id) <= 0)
                return;

            // 检查道具是否足够
            if (!GCommon.isItemEnough(GRefdataCoreMgr.instance.npGeneral.guild_cooperate_dispatch_time_reset_cost, true))
                return;

            // 发送恢复请求
            GGUIWndGuildCooperateSelectHeroContainerItem curItem = _m_wHeroListContainer?.curSelectedItem;
            NPPlayer.instance.guildCooperateComp.reqAddHeroRecoveryCount(curHeroInfo.id,(_isSuc)=>
            {
                if (!_isSuc)
                    return;

                // 播放恢复动画，刷新列表
                curItem?.playRecover(null);
                _m_wHeroListContainer?.refreshAllItem();
                _refreshStateShow();
                _refreshRecoverCost();

                // 上浮提示恢复次数成功
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guildCooperate_recoverHeroSuc_none);
            });
        }

        /// <summary>
        /// 点击停止自动派遣
        /// </summary>
        private void _onClickStopAuto(GameObject _go)
        {
            _dealStopAutoDispatch();
        }

        #endregion
    }
}