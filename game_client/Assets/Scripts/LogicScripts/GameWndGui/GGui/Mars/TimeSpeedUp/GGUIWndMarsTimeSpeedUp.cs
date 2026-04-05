using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using Common.MarsEnum;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;
using UnityEngine;
using Object = System.Object;

namespace GOE
{
    public interface _IMarsTimeSpeedUpObject
    {
        /// <summary>
        /// 时间类型
        /// </summary>
        EMarsBagItemUseTimeType timeType { get; }

        /// <summary>
        /// 对象id
        /// </summary>
        long timeObjId { get; }
        
        /// <summary>
        /// 剩余时间（毫秒）
        /// </summary>
        long remainTimeMs { get; }
        
        /// <summary>
        /// 未经过道具、盟友帮助减少的总时间（毫秒, 但是经过了玩家属性ENPPlayerPropertyType中的各种减免时间）
        /// </summary>
        long beforeReductionTotalTimeMs { get; }

        /// <summary>
        /// 经过道具、盟友帮助减少后的总时间（毫秒）
        /// </summary>
        long afterReductionTotalTimeMs { get; }

        // 联盟帮助实例id
        public long guildHelpId { get; } 
    }
    
    /// <summary>
    /// 火星时间加速窗口
    /// </summary>
    public class GGUIWndMarsTimeSpeedUp : _ANPGGUIBasicWnd<GGUIMonoMarsTimeSpeedUp>
    {
        private static GGUIWndMarsTimeSpeedUp _g_instance;
        public static GGUIWndMarsTimeSpeedUp instance { get { return _g_instance ??= new GGUIWndMarsTimeSpeedUp(); } }

        private GGUIWndMarsTimeSpeedUpBagItemContainer _m_wItemContainer; // 物品容器
        private GGUIWndBagPopCounter _m_wPopCounter; // 数量计数器
        private NPGGUIWndCommonItem _m_wCompleteNowCostItem; // 立即完成消耗物品展示
        
        private _IMarsTimeSpeedUpObject _m_speedUpTargetObject; // 加速目标对象
        private _IMarsCompleteNowObject _m_completeNowTargetObject;// 立即完成目标对象
        private MarsBagItemTimeTypeRefObj _m_timeTypeRefObj; // 时间类型配置对象
        private ALCommonEnableTaskController _m_tRemainTimeTickTask; // 剩余时间倒计时任务
        [NotNull] private List<MarsTimeSpeedUpBagItemShowData> _m_lCurTypeSpeedUpBagItemDataList = new List<MarsTimeSpeedUpBagItemShowData>(); // 当前类型加速背包道具数据列表
        [NotNull] private List<MarsTimeSpeedUpBagItemShowData> _m_lGeneralTypeSpeedUpBagItemDataList = new List<MarsTimeSpeedUpBagItemShowData>(); // 通用类型加速背包道具数据列表
        [NotNull] private List<MarsTimeSpeedUpBagItemShowData> _m_lTotalSpeedUpBagItemDataSortByTimeList = new List<MarsTimeSpeedUpBagItemShowData>();// 所有加速背包道具数据列表, 按照减少时间从少到多排序
        [NotNull] private List<MarsTimeSpeedUpBagItemShowData> _m_lShowSpeedUpBagItemDataList = new List<MarsTimeSpeedUpBagItemShowData>(); // 显示的加速背包道具数据列表
        private MarsTimeSpeedUpBagItemShowData _m_selectedItemShowData; // 当前选中的物品数据
        private long _m_selectedItemUseCount = 1; // 选中物品使用数量
        private bool _m_bIsRequestingUseItems = false;
        private long _m_helpShowSerializeOp = 0;
        
        private List<NPCommon.NPCommon_ItemInfo> _m_lTmpUseItemCommonInfoList = new List<NPCommon.NPCommon_ItemInfo>();

        public GGUIWndMarsTimeSpeedUp() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsTimeSpeedUp.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsTimeSpeedUp.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化物品容器
            if (wnd.monoItemContainer != null)
            {
                _m_wItemContainer = new GGUIWndMarsTimeSpeedUpBagItemContainer(wnd.monoItemContainer);
                _m_wItemContainer.onSelectItemChg += _onSelectItemChg;
            }

            // 初始化数量计数器
            if (wnd.monoPopCounter != null)
            {
                _m_wPopCounter = new GGUIWndBagPopCounter(wnd.monoPopCounter);
                _m_wPopCounter.regCounterChangedEvent(_onUseItemCountChg);
            }

            // 初始化立即完成消耗物品
            if (wnd.monoCompleteNowCostItem != null)
                _m_wCompleteNowCostItem = new NPGGUIWndCommonItem(wnd.monoCompleteNowCostItem);

            // 绑定按钮
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnUse, _onClickUse);
            ALUGUICommon.combineBtnClick(wnd.btnAutoUse, _onClickAutoUse);
            ALUGUICommon.combineBtnClick(wnd.btnCompleteNow, _onClickCompleteNow);
            ALUGUICommon.combineBtnClick(wnd.btnAssist, _onClickAssist);
        }

        protected override void _onDiscard()
        {
            if (_m_wItemContainer != null)
            {
                _m_wItemContainer.onSelectItemChg -= _onSelectItemChg;
                _m_wItemContainer.discard();
                _m_wItemContainer = null;                
            }

            if (_m_wPopCounter != null)
            {
                _m_wPopCounter.discard();
                _m_wPopCounter = null;                
            }

            _m_wCompleteNowCostItem?.discard();
            _m_wCompleteNowCostItem = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
                ALUGUICommon.uncombineBtnClick(wnd.btnUse, _onClickUse);
                ALUGUICommon.uncombineBtnClick(wnd.btnAutoUse, _onClickAutoUse);
                ALUGUICommon.uncombineBtnClick(wnd.btnCompleteNow, _onClickCompleteNow);
                ALUGUICommon.uncombineBtnClick(wnd.btnAssist, _onClickAssist);
            }

            _m_speedUpTargetObject = null;
            _m_timeTypeRefObj = null;
            _m_lCurTypeSpeedUpBagItemDataList.Clear();
            _m_lGeneralTypeSpeedUpBagItemDataList.Clear();
            _m_lTotalSpeedUpBagItemDataSortByTimeList.Clear();
            _m_lShowSpeedUpBagItemDataList.Clear();
            _m_selectedItemShowData = null;
            _m_selectedItemUseCount = 1;
            
            _m_lTmpUseItemCommonInfoList?.Clear();
            _m_lTmpUseItemCommonInfoList = null;
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemAdd);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemRemove);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, _refreshWnd);
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _onBuildingStateChg;
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemAdd);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemRemove);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, _refreshWnd);
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg -= _onBuildingStateChg;

            _m_wItemContainer?.hideWnd();
            _m_wPopCounter?.hideWnd();
            _m_wCompleteNowCostItem?.hideWnd();

            _m_bIsRequestingUseItems = false;
            _m_helpShowSerializeOp = ALSerializeOpMgr.next();

            // 销毁倒计时任务
            _discardRemainTimeTickTask();
        }

        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
            _m_wPopCounter?.resetWnd();
            _m_wCompleteNowCostItem?.resetWnd();
        }

        public void setData(_IMarsTimeSpeedUpObject _speedUpTargetObject, _IMarsCompleteNowObject _completeNowObject)
        {
            // 若数据不存在, 或剩余时间小于等于0, 则直接关闭窗口
            if (_speedUpTargetObject == null || _speedUpTargetObject.remainTimeMs <= 0)
            {
                // 延迟到later关闭窗口, 防止在有ShowWndAnimation时, 先销毁了资源再播放动画导致报错
                ALCommonTaskController.CommonActionAddLaterMonoTask(() =>
                {
                    _doCloseWnd();
                });
                return;
            }
            
            _m_speedUpTargetObject = _speedUpTargetObject;
            _m_completeNowTargetObject = _completeNowObject;
            _m_timeTypeRefObj = GRefdataCoreMgr.instance.marsBagItemTimeTypeRefCore.getRef((long) _speedUpTargetObject.timeType);
            _updateTotalSpeedUpBagItemDataList();
            
            _refreshWnd();
        }

        private void _updateTotalSpeedUpBagItemDataList()
        {
            if(_m_speedUpTargetObject == null)
                return;
            
            _m_lCurTypeSpeedUpBagItemDataList.Clear();
            _m_lGeneralTypeSpeedUpBagItemDataList.Clear();
            _m_lTotalSpeedUpBagItemDataSortByTimeList.Clear();
            GRefdataCoreMgr.instance.dealCanUseMarsTimeReduceBagItem(_m_speedUpTargetObject.timeType, (timeReduceRefObj) =>
            {
                if (timeReduceRefObj == null || 
                    (timeReduceRefObj.time_type != _m_speedUpTargetObject.timeType && timeReduceRefObj.time_type != EMarsBagItemUseTimeType.ALL))
                    return true;

                BagItemRefObj bagItemRef = GRefdataCoreMgr.instance.bagItemCore.getRef(timeReduceRefObj.id);
                if (bagItemRef == null)
                    return true;

                MarsTimeSpeedUpBagItemShowData itemShowData = new MarsTimeSpeedUpBagItemShowData(timeReduceRefObj, bagItemRef);
                if (timeReduceRefObj.time_type == _m_speedUpTargetObject.timeType)
                {
                    _m_lCurTypeSpeedUpBagItemDataList.Add(itemShowData);
                    _m_lTotalSpeedUpBagItemDataSortByTimeList.Add(itemShowData);
                }
                else if (timeReduceRefObj.time_type == EMarsBagItemUseTimeType.ALL)
                {
                    _m_lGeneralTypeSpeedUpBagItemDataList.Add(itemShowData);
                    _m_lTotalSpeedUpBagItemDataSortByTimeList.Add(itemShowData);
                }

                return true;
            });
            _m_lCurTypeSpeedUpBagItemDataList.Sort(_sortSpeedUpBagItemData);
            _m_lGeneralTypeSpeedUpBagItemDataList.Sort(_sortSpeedUpBagItemData);
            
            // 按照减少时间从少到多排序
            _m_lTotalSpeedUpBagItemDataSortByTimeList.Sort((_a, _b) =>
            {
                if(_b == null) return -1;
                if(_a == null) return 1;
                if(Object.ReferenceEquals(_a, _b)) return 0;
                
                //通用道具最后使用, 且通用道具枚举值为除NONE外最小, 所以相当于按照枚举倒序(从大到小)排序
                int sortType = _b.timeReduceRef.time_type.CompareTo(_a.timeReduceRef.time_type);
                if(sortType != 0)
                    return sortType;
                
                int sortReduceTime = _a.timeReduceRef.reduce_sec.CompareTo(_b.timeReduceRef.reduce_sec);
                if(sortReduceTime != 0)
                    return sortReduceTime;
                
                // 最后按照物品ID排序
                return _a.bagItemId.CompareTo(_b.bagItemId);
            });
        }

        private int _sortSpeedUpBagItemData(MarsTimeSpeedUpBagItemShowData _a, MarsTimeSpeedUpBagItemShowData _b)
        {
            if(_b == null) return -1;
            if(_a == null) return 1;
            if(Object.ReferenceEquals(_a, _b)) return 0;

            // //通用道具最后展示, 且通用道具枚举值为除NONE外最小, 所以相当于按照枚举倒序(从大到小)排序
            // int sortType = _b.timeReduceRef.time_type.CompareTo(_a.timeReduceRef.time_type);
            // if(sortType != 0)
            //     return sortType;

            // 按照品质低到高排序
            EQuality qualityA = GCommon.getItemQuality(MarsTimeSpeedUpBagItemShowData.SpeedUpItemType, _a.bagItemId);
            EQuality qualityB = GCommon.getItemQuality(MarsTimeSpeedUpBagItemShowData.SpeedUpItemType, _b.bagItemId);
            int sortQuality = qualityA.CompareTo(qualityB);
            if(sortQuality != 0)
                return sortQuality;
                
            // 按照减少时间少到多排序
            int sortReduceTime = _a.timeReduceRef.reduce_sec.CompareTo(_b.timeReduceRef.reduce_sec);
            if(sortReduceTime != 0)
                return sortReduceTime;
                
            // 最后按照物品ID排序
            return _a.bagItemId.CompareTo(_b.bagItemId);
        }
        
        private void _updateShowSpeedUpBagItemDataList()
        {
            _m_lShowSpeedUpBagItemDataList.Clear();
            foreach (var itemShowData in _m_lCurTypeSpeedUpBagItemDataList)
            {
                if(itemShowData == null)
                    continue;
                
                // 更新一下背包数据
                itemShowData.updateBagInfo();
                // 有数量的物品才显示
                if(itemShowData.bagItemInfo != null && itemShowData.itemCount > 0)
                {
                    _m_lShowSpeedUpBagItemDataList.Add(itemShowData);
                }
            }
            foreach (var itemShowData in _m_lGeneralTypeSpeedUpBagItemDataList)
            {
                if(itemShowData == null)
                    continue;
                
                // 更新一下背包数据
                itemShowData.updateBagInfo();
                // 有数量的物品才显示
                if(itemShowData.bagItemInfo != null && itemShowData.itemCount > 0)
                {
                    _m_lShowSpeedUpBagItemDataList.Add(itemShowData);
                }
            }
        }
        
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            _updateShowSpeedUpBagItemDataList();
            
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndTitle_str, _m_timeTypeRefObj?.name));

            int helpCount = 0;
            int maxHelpCount = 0;

            ALUGUICommon.setGameObjEnable(wnd.notAskHelpHideList, false);
            
            
            bool canAssist = _m_speedUpTargetObject != null && _m_speedUpTargetObject.guildHelpId <= 0 && NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding();
            ALUGUICommon.setGameObjEnable(wnd.canAssistShowGos, canAssist);
            ALUGUICommon.setGameObjEnable(wnd.canAssistHideGos, !canAssist);

            if (_m_speedUpTargetObject != null && _m_speedUpTargetObject.guildHelpId > 0)
            {
                _m_helpShowSerializeOp = ALSerializeOpMgr.next();
                long helpShowSerializeOp = _m_helpShowSerializeOp;
                NPPlayer.instance.guildMarsHelpComp.reqGuildMarsHelpInfo(_m_speedUpTargetObject.guildHelpId,
                    (_isSuc, _curCount, _maxCount) =>
                    {
                        if (helpShowSerializeOp != _m_helpShowSerializeOp)
                            return;
                        ALUGUICommon.setLabelTxt(wnd.txtHelpCount, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num,
                                _curCount, _maxCount));
                        ALUGUICommon.setGameObjEnable(wnd.notAskHelpHideList, _isSuc);
                    });
            }

            // 刷新剩余时间
            _refreshRemainTime();

            _refreshShowBagItem();
            _refreshCompleteNow();
            
            _initRemainTimeTickTask();
        }

        /// <summary>
        /// 刷新剩余时间
        /// </summary>
        private void _refreshRemainTime()
        {
            if (wnd == null || _m_speedUpTargetObject == null)
                return;

            long remainMs = _m_speedUpTargetObject.remainTimeMs;
            long totalTimeMs = _m_speedUpTargetObject.beforeReductionTotalTimeMs;
            long passedMs = totalTimeMs - remainMs;
            
            // 剩余时间（秒）
            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.millisecondsToTime_dhms(remainMs));

            // 进度条
            ALUGUICommon.setSliderScale(wnd.sldTimeProgress, totalTimeMs <= 0 ? 0 : 1f * passedMs / totalTimeMs);
        }

        /// <summary>
        /// 刷新显示的背包道具相关显示(当_m_lShowSpeedUpBagItemDataList更新时, 调用)
        /// </summary>
        private void _refreshShowBagItem()
        {
            _refreshShowBagItemContainer();
            _refreshSelectedItem();
        }
                
        /// <summary>
        /// 刷新需要显示的背包物品列表(显示道具)
        /// </summary>
        private void _refreshShowBagItemContainer()
        {
            if (wnd == null || _m_wItemContainer == null)
                return;

            _m_wItemContainer.showWnd();
            _m_wItemContainer.setData(_m_lShowSpeedUpBagItemDataList, _m_selectedItemShowData);
        }
        
        /// <summary>
        /// 刷新选中道具相关显示(与显示道具、选中道具、剩余时间有关)
        /// </summary>
        private void _refreshSelectedItem()
        {
            _refreshSelectedItemCanUseCount();
            _refreshSelectedItemUseCount();
        }

        /// <summary>
        /// 刷新选中物品可使用数量
        /// </summary>
        private void _refreshSelectedItemCanUseCount()
        {
            if(wnd == null || _m_speedUpTargetObject == null)
                return;

            if (_m_wPopCounter != null)
            {
                long maxCanUseCount = 0;
                if (_m_selectedItemShowData != null)
                {
                    long costItemCount = _m_selectedItemShowData.timeReduceRef.reduce_sec <= 0 ? 0 : 
                        (long) Math.Ceiling(1f * _m_speedUpTargetObject.remainTimeMs / _m_selectedItemShowData.timeReduceRef.reduce_sec / 1000f);
                    // 取 剩余时间会销毁掉的道具数量 和 当前拥有的道具数量 的最小值
                    maxCanUseCount = Math.Min(costItemCount, _m_selectedItemShowData.itemCount);    
                }

                _m_wPopCounter.showWnd();
                if (_m_wPopCounter.totalCount != maxCanUseCount)
                {
                    _m_wPopCounter.init(maxCanUseCount, _m_selectedItemUseCount);
                }
            }
        }
        
        /// <summary>
        /// 刷新选中物品使用数量
        /// </summary>
        private void _refreshSelectedItemUseCount()
        {
            // 刷新总减少时长
            long totalReduceMs = (_m_selectedItemShowData?.timeReduceRef.reduce_sec ?? 0) * 1000 * _m_selectedItemUseCount;
            ALUGUICommon.setLabelTxt(wnd.txtTotalTimeReduce, TextTranslate.instance.getLanguage(TimeUtil.millisecondsToTime_dhms(totalReduceMs)));

            _refreshUseBtnState();
        }
        
        /// <summary>
        /// 刷新使用按钮状态
        /// </summary>
        private void _refreshUseBtnState()
        {
            if (wnd == null)
                return;

            bool canUse = _checkCanUseSelectedItem();
            GGameCommonInfo.grayImage(wnd.cannotUseGrayList, !canUse);
        }
        
        /// <summary>
        /// 刷新立即完成显示(与剩余时间有关)
        /// </summary>
        private void _refreshCompleteNow()
        {
            if (wnd == null || _m_completeNowTargetObject == null)
                return;

            if (!_m_completeNowTargetObject.checkCanCompleteNow(true, false))
            {
                ALUGUICommon.setGameObjEnable(wnd.canCompleteNowShowList, false);
                return;
            }
            
            ALUGUICommon.setGameObjEnable(wnd.canCompleteNowShowList, true);
            
            // 设置立即完成消耗
            if (_m_wCompleteNowCostItem != null)
            {
                _m_wCompleteNowCostItem.showWnd();
                _m_wCompleteNowCostItem.setItem(_m_completeNowTargetObject.completeNowCostItem);
            }
        }

        /// <summary>
        /// 检查是否有可用的加速物品
        /// </summary>
        /// <returns></returns>
        private bool _checkHasCanUseItem(bool _showTip = false)
        {
            if (_m_lShowSpeedUpBagItemDataList.Count <= 0)
            {
                if (_showTip)
                {
                    if(_m_timeTypeRefObj != null && _m_timeTypeRefObj.lack_show_gain_way_item != null && _m_timeTypeRefObj.lack_show_gain_way_item.IsValid)
                        GCommon.dealItemNotEnough(_m_timeTypeRefObj.lack_show_gain_way_item);
                    else
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndSelectedItemNotEnough_none));
                }
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// 检查是否可以使用选中物品
        /// </summary>
        private bool _checkCanUseSelectedItem(bool _showTip = false)
        {
            if (!_checkHasCanUseItem(_showTip))
                return false;
            
            if (_m_selectedItemShowData == null)
            {
                if (_showTip)
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndNotSelectedItem_none));
                }
                return false;
            }
            
            if(_m_selectedItemShowData.itemCount <= 0 || _m_selectedItemShowData.itemCount < _m_selectedItemUseCount)
            {
                if (_showTip)
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndSelectedItemNotEnough_none));
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// 当选中加速道具变化
        /// </summary>
        private void _onSelectItemChg()
        {
            if(_m_wItemContainer == null)
                return;

            _m_selectedItemShowData = _m_wItemContainer.selectedBagItemShowData;

            _refreshSelectedItem();
        }

        /// <summary>
        /// 使用道具数量变化
        /// </summary>
        private void _onUseItemCountChg(long _newCount)
        {
            _m_selectedItemUseCount = _newCount;

            _refreshSelectedItemUseCount();
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        private void _onClickClose(GameObject _go)
        {
            _doCloseWnd();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void _doCloseWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_TIME_SPEEDUP);
        }
        
        /// <summary>
        /// 点击使用
        /// </summary>
        private void _onClickUse(GameObject _go)
        {
            if(_m_speedUpTargetObject == null || !_checkCanUseSelectedItem(true) || _m_selectedItemShowData == null)
                return;

            if (_m_lTmpUseItemCommonInfoList == null)
                _m_lTmpUseItemCommonInfoList = new List<NPCommon.NPCommon_ItemInfo>();
            _m_lTmpUseItemCommonInfoList.Clear();
            _m_lTmpUseItemCommonInfoList.Add(new NPCommon_ItemInfo((int)MarsTimeSpeedUpBagItemShowData.SpeedUpItemType, _m_selectedItemShowData.bagItemId, _m_selectedItemUseCount, null));

            long totalReduceTimeMs = _m_selectedItemShowData.timeReduceRef.reduce_sec * 1000 * _m_selectedItemUseCount;
            if(totalReduceTimeMs > _m_speedUpTargetObject.remainTimeMs && AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.MARS_TIME_REDUCE_OVERFLOW))// 当前物品使用总时间大于剩余时间
            {
                NPMesMgr.instance.showWarningTipMes(() =>
                {
                    _requestUseItemsForTimeReduce(_m_lTmpUseItemCommonInfoList, null);
                }, () =>
                {
                    
                }, ENPWarningType.MARS_TIME_REDUCE_OVERFLOW
                    , TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndReduceTimeOverflowPopWndTitle_none)
                    , TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndReduceTimeOverflowPopWndContent_none));
            }
            else
            {
                _requestUseItemsForTimeReduce(_m_lTmpUseItemCommonInfoList, null);
            }
        }

        /// <summary>
        /// 点击一键使用
        /// </summary>
        private void _onClickAutoUse(GameObject _go)
        {
            if (_m_speedUpTargetObject == null || !_checkHasCanUseItem(true))
                return;

            long remainTimeMs = _m_speedUpTargetObject.remainTimeMs;//剩余时间毫秒数
            long totalReducedMs = 0;//已减少时间毫秒数
            bool usedGeneralItem = false;//是否使用了通用道具
            
            if(_m_lTmpUseItemCommonInfoList == null)
                _m_lTmpUseItemCommonInfoList = new List<NPCommon.NPCommon_ItemInfo>();
            _m_lTmpUseItemCommonInfoList.Clear();
            // 先使用当前类型道具, 从减少时间最多的道具开始使用
            for(int count = _m_lCurTypeSpeedUpBagItemDataList.Count, i = count - 1; i >= 0; i--)
            {
                var itemShowData = _m_lCurTypeSpeedUpBagItemDataList[i];
                itemShowData?.updateBagInfo();//更新一下背包数据
                if(itemShowData == null || itemShowData.itemCount <= 0)
                    continue;

                long itemReduceMs = itemShowData.timeReduceRef.reduce_sec * 1000;//该道具减少时间毫秒数
                long canUseCount = itemReduceMs <= 0 ? 0 : remainTimeMs / itemReduceMs;//可使用的道具数
                long useCount = Math.Min(canUseCount, itemShowData.itemCount);//实际使用的道具数
                if(useCount <= 0)
                    continue;
                
                long reducedMs = useCount * itemReduceMs;
                totalReducedMs += reducedMs;
                remainTimeMs -= reducedMs;

                _m_lTmpUseItemCommonInfoList.Add(new NPCommon_ItemInfo((int)MarsTimeSpeedUpBagItemShowData.SpeedUpItemType, itemShowData.bagItemId, useCount, null));
            }
            // 再使用通用类型道具, 从减少时间最多的道具开始使用
            for(int count = _m_lGeneralTypeSpeedUpBagItemDataList.Count, i = count - 1; i >= 0; i--)
            {
                var itemShowData = _m_lGeneralTypeSpeedUpBagItemDataList[i];
                itemShowData?.updateBagInfo();//更新一下背包数据
                if(itemShowData == null || itemShowData.itemCount <= 0)
                    continue;

                long itemReduceMs = itemShowData.timeReduceRef.reduce_sec * 1000;//该道具减少时间毫秒数
                long canUseCount = itemReduceMs <= 0 ? 0 : remainTimeMs / itemReduceMs;//可使用的道具数
                long useCount = Math.Min(canUseCount, itemShowData.itemCount);//实际使用的道具数
                if(useCount <= 0)
                    continue;
                
                long reducedMs = useCount * itemReduceMs;
                totalReducedMs += reducedMs;
                remainTimeMs -= reducedMs;

                _m_lTmpUseItemCommonInfoList.Add(new NPCommon_ItemInfo((int)MarsTimeSpeedUpBagItemShowData.SpeedUpItemType, itemShowData.bagItemId, useCount, null));
                usedGeneralItem = true;
            }

            // 若还有剩余时间, 找到耗时最少的道具使用
            if (remainTimeMs > 0)
            {
                foreach (var itemShowData in _m_lTotalSpeedUpBagItemDataSortByTimeList)
                {
                    if(itemShowData == null || itemShowData.itemCount <= 0)//若没有数量, 则跳过
                        continue;

                    // 查找是否已经在使用列表中
                    NPCommon_ItemInfo useItemInfo = _m_lTmpUseItemCommonInfoList.Find((_item) =>
                    {
                        return _item != null &&
                               _item.getItemType() == (int) MarsTimeSpeedUpBagItemShowData.SpeedUpItemType &&
                               _item.getSubId() == itemShowData.bagItemId;
                    });

                    long remainItemCount = itemShowData.itemCount - (useItemInfo?.getCount() ?? 0);//该道具剩余可用数量
                    if(remainItemCount <= 0)//若已经使用完该道具, 则跳过
                        continue;
                    
                    long itemReduceMs = itemShowData.timeReduceRef.reduce_sec * 1000;//该道具减少时间毫秒数
                    long canUseCount = itemReduceMs <= 0 ? 0 : (long)Math.Ceiling(1f * remainTimeMs / itemReduceMs);//可使用的道具数(向上取整)
                    long useCount = Math.Min(canUseCount, remainItemCount);//实际使用的道具数
                    if(useCount <= 0)
                        continue;

                    if (itemShowData.timeReduceRef.time_type == EMarsBagItemUseTimeType.ALL)
                        usedGeneralItem = true;
                    
                    if (useItemInfo == null)
                    {
                        _m_lTmpUseItemCommonInfoList.Add(new NPCommon_ItemInfo((int)MarsTimeSpeedUpBagItemShowData.SpeedUpItemType, itemShowData.bagItemId, useCount, null));
                    }
                    else
                    {
                        useItemInfo.setCount(useItemInfo.getCount() + useCount);
                    }
                    
                    long reducedMs = useCount * itemReduceMs;
                    totalReducedMs += reducedMs;
                    remainTimeMs -= reducedMs;
                    
                    // 若剩余时间已经减少完, 则跳出
                    if(remainTimeMs <= 0)
                        break;
                }
            }

            // 显示使用了通用道具提示
            Action showUseGeneralItemTipAction = () =>
            {
                if (usedGeneralItem && AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.MARS_TIME_REDUCE_USED_GENERAL_ITEM_TIP))
                {
                    NPMesMgr.instance.showWarningTipMes(() =>
                        {
                            _requestUseItemsForTimeReduce(_m_lTmpUseItemCommonInfoList, null);
                        }, () =>
                        {
                    
                        }, ENPWarningType.MARS_TIME_REDUCE_USED_GENERAL_ITEM_TIP
                        , TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndUsedGeneralItemPopWndTitle_none)
                        , TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndUsedGeneralItemPopWndContent_none));
                }
                else
                {
                    _requestUseItemsForTimeReduce(_m_lTmpUseItemCommonInfoList, null);
                }
            };
            
            // 显示减少时间溢出提示
            Action showReduceTimeOverflowTipAction = () =>
            {
                if (totalReducedMs > _m_speedUpTargetObject.remainTimeMs && AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.MARS_TIME_REDUCE_OVERFLOW))//若总减少时间大于剩余时间
                {
                    NPMesMgr.instance.showWarningTipMes(() =>
                        {
                            showUseGeneralItemTipAction();
                        }, () =>
                        {
                    
                        }, ENPWarningType.MARS_TIME_REDUCE_OVERFLOW
                        , TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndReduceTimeOverflowPopWndTitle_none)
                        , TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndReduceTimeOverflowPopWndContent_none));
                }
                else
                {
                    showUseGeneralItemTipAction();
                }
            };

            showReduceTimeOverflowTipAction();
        }

        private void _requestUseItemsForTimeReduce(List<NPCommon.NPCommon_ItemInfo> _useItemCommonInfoList, Action<bool> _callback = null)
        {
            if (_m_bIsRequestingUseItems)
            {
                _callback?.Invoke(false);
                return;
            }
            
            if (_m_speedUpTargetObject == null || _useItemCommonInfoList == null || _useItemCommonInfoList.Count <= 0)
            {
                _callback?.Invoke(false);
                return;
            }

            _m_bIsRequestingUseItems = true;
            NPPlayer.instance.marsComp.reqBagUseItemForTimeReduce(_useItemCommonInfoList, _m_speedUpTargetObject.timeType, _m_speedUpTargetObject.timeObjId,
                (_isSucc, _msg) =>
                {
                    _m_bIsRequestingUseItems = false;
                    
                    // 若时间已经结束, 直接关闭窗口
                    if (_m_speedUpTargetObject == null || _m_speedUpTargetObject.remainTimeMs <= 0)
                    {
                        _doCloseWnd();
                        return;
                    }
                    
                    // 刷新剩余时间
                    _refreshRemainTime();
                    
                    // 刷新显示的背包物品
                    _refreshSelectedItem();
                    
                    // 刷新立即完成显示
                    _refreshCompleteNow();
                    
                    _callback?.Invoke(_isSucc);
                });
        }
        
        /// <summary>
        /// 点击立即完成
        /// </summary>
        private void _onClickCompleteNow(GameObject _go)
        {
            if (_m_completeNowTargetObject == null || !_m_completeNowTargetObject.checkCanCompleteNow(false, true))
                return;

            // 检查消耗是否足够
            NPCommonCostItem costItem = _m_completeNowTargetObject.completeNowCostItem;
            if(costItem != null && !GCommon.isItemEnough(costItem, true))
                return;
            
            // 检查是否需要显示今日不再提示的确认窗口
            bool needConfirm = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.MARS_TIME_COMPLETE_NOW_CONFIRM);
            if (needConfirm)
            {
                // 显示确认窗口
                GGUIWndMarsTimeCompleteNowConfim.addNode(_m_completeNowTargetObject);
            }
            else
            {
                // 直接执行立即完成操作
                _m_completeNowTargetObject.dealCompleteNowAction?.Invoke(null);
            }
        }
        private void _onClickAssist(GameObject _obj)
        {
            if (_m_speedUpTargetObject == null) return;
            bool canAssist = _m_speedUpTargetObject.guildHelpId <= 0 && NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding();
            if (!canAssist) return;
            _IGuildMarsHelp helpInfo =NPPlayer.instance.guildMarsHelpComp.getMyHelpInfoByHelpId(_m_speedUpTargetObject.guildHelpId);
            if (helpInfo != null)
                NPPlayer.instance.guildMarsHelpComp.reqSendMarsHelp(helpInfo.helpObjType, helpInfo.guildHelpObjId,
                    (_isSuc) => { _refreshWnd(); });
        }
        /// <summary>
        /// 建筑状态变化事件处理
        /// </summary>
        private void _onBuildingStateChg(long _buildingId, MarsBuildingInfo.StateType _oldState, MarsBuildingInfo.StateType _newState)
        {
            var buildingRef = GRefdataCoreMgr.instance.marsBuildingRefCore.getRef(_buildingId);
            if (buildingRef != null && buildingRef.building_type == EMarsBuildingType.HELP)
            {
                if (_newState == MarsBuildingInfo.StateType.Normal)
                    _refreshWnd();
            }
        }
        /// <summary>
        /// 背包物品变更
        /// 参数: _objs[0] 为 BagItem
        /// </summary>
        private void _onBagItemChg(params object[] _objs)
        {
            // 在这个窗口中除了消耗道具没有其他途径变化数量, 所以这里不刷新, 之前在请求使用道具成功的回调中刷新
            
            // if (_objs == null || _objs.Length < 1 || !(_objs[0] is BagItem _tempItem))
            //     return;
            //
            // foreach (var showSpeedUpBagItem in _m_lShowSpeedUpBagItemDataList)
            // {
            //     if (showSpeedUpBagItem != null && showSpeedUpBagItem.bagItemId == _tempItem.itemId)
            //     {
            //         _refreshShowBagItem();
            //         break;
            //     }
            // }
        }

        /// <summary>
        /// 背包物品添加
        /// 参数: _objs[0] 为 BagItem
        /// </summary>
        private void _onBagItemAdd(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is BagItem _tempItem))
                return;

            // 有道具增加时, 重新更新显示列表
            _updateShowSpeedUpBagItemDataList();
            _refreshShowBagItem();
        }

        /// <summary>
        /// 背包物品移除
        /// 参数: _objs[0] 为 BagItem
        /// </summary>
        private void _onBagItemRemove(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is BagItem _tempItem))
                return;

            // 有道具移除时, 重新更新显示列表
            _updateShowSpeedUpBagItemDataList();
            _refreshShowBagItem();
        }

        #region 倒计时任务
        
        /// <summary>
        /// 销毁倒计时任务
        /// </summary>
        private void _discardRemainTimeTickTask()
        {
            _m_tRemainTimeTickTask.setDisable();
        }
        
        /// <summary>
        /// 初始化倒计时任务
        /// </summary>
        private void _initRemainTimeTickTask()
        {
            _discardRemainTimeTickTask();
            
            _m_tRemainTimeTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_remainTimeTickTaskAction, 1f);
        }

        /// <summary>
        /// 倒计时任务回调
        /// </summary>
        private void _remainTimeTickTaskAction()
        {
            if (_m_speedUpTargetObject == null || _m_speedUpTargetObject.remainTimeMs <= 0)
            {
                // 时间结束，关闭窗口
                // 延迟到later关闭窗口, 防止在有ShowWndAnimation时, 先销毁了资源再播放动画导致报错
                ALCommonTaskController.CommonActionAddLaterMonoTask(() =>
                {
                    _doCloseWnd();
                });
                return;
            }
            
            // 刷新剩余时间显示
            _refreshRemainTime();
            // 刷新选中物品相关显示
            _refreshSelectedItem();
            // 刷新立即完成消耗（因为消耗可能随时间变化）
            _refreshCompleteNow();
        }

        #endregion
        
        public static void addNode(_IMarsTimeSpeedUpObject _speedUpTargetObject, _IMarsCompleteNowObject _completeNowObject, bool _donotCheckHasItem = true)
        {
            if (_speedUpTargetObject == null)
            {
                Debug.LogError($"添加加速窗口失败, _speedUpTargetObject为空!");
                return;
            }
            
            bool hasCanUseSpeedUpItem = false;//是否有可使用的加速道具
            GRefdataCoreMgr.instance.dealCanUseMarsTimeReduceBagItem(_speedUpTargetObject.timeType, (timeReduceRefObj) =>
            {
                if (timeReduceRefObj == null || 
                    (timeReduceRefObj.time_type != _speedUpTargetObject.timeType && timeReduceRefObj.time_type != EMarsBagItemUseTimeType.ALL))
                    return true;

                BagItem bagItem = NPPlayer.instance.bagComp.getItem(timeReduceRefObj.id);
                if (bagItem != null && bagItem.count > 0)
                {
                    hasCanUseSpeedUpItem = true;
                }

                // 若果找到了可用的道具就停止遍历
                return !hasCanUseSpeedUpItem;
            });

            MarsBagItemTimeTypeRefObj timeTypeRefObj = GRefdataCoreMgr.instance.marsBagItemTimeTypeRefCore.getRef((long)_speedUpTargetObject.timeType);
            if (hasCanUseSpeedUpItem || _donotCheckHasItem)
            {
                instance.setData(_speedUpTargetObject, _completeNowObject);
                QueueMgr.instance.addNode_InGame_SingleWnd(instance, instance.showWnd, UINodeTagConst.C_MARS_TIME_SPEEDUP);
            }
            else if(timeTypeRefObj != null && timeTypeRefObj.lack_show_gain_way_item != null && timeTypeRefObj.lack_show_gain_way_item.IsValid)
            {
                GCommon.dealItemNotEnough(timeTypeRefObj.lack_show_gain_way_item);
            }
            else
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_speedUpWndSelectedItemNotEnough_none));
            }
        }
    }
}