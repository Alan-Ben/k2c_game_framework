using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class TreasureHuntUtil
    {
        #region 矿石相关方法

        /// <summary>
        /// 获取矿石状态
        /// </summary>
        /// <param name="_oreId"></param>
        /// <returns></returns>
        public static ETreasureHuntOreState getOreState(long _oreId)
        {
            TreasureHuntGotOreInfo oreInfo = NPPlayer.instance.treasureHuntComponent.getGotOreInfo(_oreId);
            if(oreInfo == null)// 若还未获取矿石
                return ETreasureHuntOreState.NOT_GET;

            return oreInfo.oreState;
        }
        
        /// <summary>
        /// 获取矿石数据(不区分是否拥有)
        /// </summary>
        /// <param name="_ore"></param>
        /// <returns></returns>
        public static _ITreasureHuntOreInfo getOreInfo(long _oreId)
        {
            TreasureHuntGotOreInfo oreInfo = NPPlayer.instance.treasureHuntComponent.getGotOreInfo(_oreId);
            if(oreInfo != null)
                return oreInfo;

            return new TreasureHuntNotGetOreInfo(_oreId);
        }

        /// <summary>
        /// 获取所有矿石数据(不区分是否拥有)
        /// </summary>
        /// <param name="_oreInfoList"></param>
        public static void getAllOreInfo(List<_ITreasureHuntOreInfo> _oreInfoList)
        {
            if(_oreInfoList == null)
                return;
            _oreInfoList.Clear();

            _ITreasureHuntOreInfo oreInfo = null;
            foreach (var oreRefObj in GRefdataCoreMgr.instance.treasureHuntOreRefCore.refList)
            {
                if(oreRefObj == null)
                    continue;

                oreInfo = getOreInfo(oreRefObj.id);
                if(oreInfo != null)
                    _oreInfoList.Add(oreInfo);
                else
                    _oreInfoList.Add(new TreasureHuntNotGetOreInfo(oreRefObj));
            }
        }

        /// <summary>
        /// 获取矿石质量显示字符串
        /// </summary>
        /// <param name="_mass"></param>
        public static string getOreMassShowStr(int _mass)
        {
            return (_mass / 1000f).ToString("0.###");
        }
        
        #endregion

        #region 奇物相关方法

        /// <summary>
        /// 获取奇物状态
        /// </summary>
        /// <param name="_treasureId"></param>
        /// <returns></returns>
        public static ETreasureHuntTreasureState getTreasureState(long _treasureId)
        {
            TreasureHuntGotTreasureInfo treasureInfo = NPPlayer.instance.treasureHuntComponent.getGotTreasureInfo(_treasureId);
            if(treasureInfo == null)// 若还未获取奇物
                return ETreasureHuntTreasureState.NOT_GET;
            
            return treasureInfo.treasureState;
        }

        /// <summary>
        /// 获取可放入实验室的奇物数量
        /// </summary>
        /// <param name="_firstCanPutInTreasureInfo">第一个可放入实验室的奇物</param>
        /// <returns></returns>
        public static int getCanPutInLabTreasureCount(out TreasureHuntGotTreasureInfo _firstCanPutInTreasureInfo)
        {
            _firstCanPutInTreasureInfo = null;
            int canPutInLabCount = 0;
            foreach (var treasureInfo in NPPlayer.instance.treasureHuntComponent.gotTreasureInfoList)
            {
                // 处于未激活状态的奇物可以放入实验室
                if (treasureInfo != null && treasureInfo.treasureState == ETreasureHuntTreasureState.GOT_NOT_ACTIVATE)
                {
                    canPutInLabCount++;
                    
                    if (_firstCanPutInTreasureInfo == null || _firstCanPutInTreasureInfo.gainTimeMs > treasureInfo.gainTimeMs)
                        _firstCanPutInTreasureInfo = treasureInfo;
                }
            }
            return canPutInLabCount;
        }

        /// <summary>
        /// 获取奇物数据(不区分是否拥有)
        /// </summary>
        /// <param name="_treasureId"></param>
        /// <returns></returns>
        public static _ITreasureHuntTreasureInfo getTreasureInfo(long _treasureId)
        {
            TreasureHuntGotTreasureInfo treasureInfo = NPPlayer.instance.treasureHuntComponent.getGotTreasureInfo(_treasureId);
            if(treasureInfo != null)
                return treasureInfo;

            return new TreasureHuntNotGetTreasureInfo(_treasureId);
        }
        
        /// <summary>
        /// 获取所有奇物数据(不区分是否拥有)
        /// </summary>
        /// <param name="_treasureInfoList"></param>
        public static void getAllTreasureInfo(List<_ITreasureHuntTreasureInfo> _treasureInfoList)
        {
            if(_treasureInfoList == null)
                return;
            _treasureInfoList.Clear();

            _ITreasureHuntTreasureInfo treasureInfo = null;
            foreach (var treasureRefObj in GRefdataCoreMgr.instance.treasureHuntTreasureRefCore.refList)
            {
                if(treasureRefObj == null)
                    continue;

                treasureInfo = getTreasureInfo(treasureRefObj.id);
                if(treasureInfo != null)
                    _treasureInfoList.Add(treasureInfo);
                else
                    _treasureInfoList.Add(new TreasureHuntNotGetTreasureInfo(treasureRefObj));
            }
        }
        
        #endregion

        #region 区域相关方法

        /// <summary>
        /// 刷新当前所处的太空区域
        /// </summary>
        public static void refreshInArea()
        {
            TreasureHuntAreaRefObj inAreaRefObj = GRefdataCoreMgr.instance.treasureHuntAreaRefCore.getRef(NPPlayer.instance.treasureHuntComponent.saver?.getAreaId() ?? -1);
            if(inAreaRefObj != null && inAreaRefObj.isUnlock())
                return;
            
            // 如果当前所在的太空区域没有解锁，则需要切换到已经解锁的太空区域
            foreach (var areaObj in GRefdataCoreMgr.instance.treasureHuntAreaRefCore.refList)
            {
                if (areaObj != null && areaObj.isUnlock())
                {
                    NPPlayer.instance.treasureHuntComponent.saver?.setAreaId(areaObj.area_id);
                    return;
                }
            }
            
            // 若到这里说明没有任何太空区域解锁，则将当前所在的太空区域id设置为配表中第一个
            NPPlayer.instance.treasureHuntComponent.saver?.setAreaId(GRefdataCoreMgr.instance.treasureHuntAreaRefCore.refList?.SafeGet(0)?.area_id ?? 0);
        }

        #endregion

        /// <summary>
        /// 检查技能是否可升级
        /// </summary>
        public static bool checkCanUpgradeSkill(_ITreasureHuntSkillInfo _skillInfo)
        {
            if (_skillInfo == null || _skillInfo.skillRefObj == null || _skillInfo.skillLevelRefObj == null)
                return false;
                
            // 只有已激活未满级状态的技能才可能升级
            if(_skillInfo.skillState != ETreasureHuntSkillState.UNLOCK_ACTIVATE)
                return false;

            // 判断技能点是否足够
            return _skillInfo.skillPointNum >= _skillInfo.skillLevelRefObj.upgrade_cost_num;
        }
        
        /// <summary>
        /// 展示捕获结果
        /// </summary>
        public static void showCaptureResult(List<Common.TreasureHuntObj.TreasureHunt_CaptureResult> _resultInfoList, Action _showDone = null)
        {
            if (_resultInfoList == null || _resultInfoList.Count <= 0)
            {
                _showDone?.Invoke();
                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(_showDone);

            List<TreasureHuntCommonOreInfo> oreList = new List<TreasureHuntCommonOreInfo>();
            List<TreasureHuntGotTreasureInfo> treasureList = new List<TreasureHuntGotTreasureInfo>();
            List<NPCommonCostItem> rewardItemList = new List<NPCommonCostItem>();
            
            foreach (var resultInfo in _resultInfoList)
            {
                List<Common.TreasureHuntObj.TreasureHunt_CaptureReward> rewardList = resultInfo?.getCaptureRewardList();
                if (rewardList == null)
                    continue;

                foreach (Common.TreasureHuntObj.TreasureHunt_CaptureReward reward in rewardList)
                {
                    if(reward == null)
                        continue;
                    
                    TreasureHuntCaptureResultBase resultBaseInfo = TreasureHuntCaptureResultBase.CreateCaptureResult(reward);
                    if (resultBaseInfo == null)
                        continue;
                    
                    // 获取的物品是矿石
                    if(resultBaseInfo is TreasureHuntCaptureResultOre oreResult)
                    {
                        // 若是首次获得矿石或者首次获得高级矿石，则独立展示捕获结果
                        if (oreResult.isFirstCapture || oreResult.isFirstDrawAdvanced)
                        {
                            stepCounter.chgTotalStepCount(1);
                            resultBaseInfo.showCaptureResult(stepCounter.addDoneStepCount);
                        }

                        // 统计获得的矿石信息, 需要区分普通和高级矿石
                        if (oreResult.oreInfo != null && oreResult.oreInfo.oreRefObj != null)
                        {
                            TreasureHuntCommonOreInfo oreInfo = oreList.Find((_item) =>
                            {
                                return _item != null && _item.oreRefObj != null &&
                                       _item.oreId == oreResult.oreInfo.oreId
                                       && _item.oreRefObj.isReachAdvanceOreMass(_item.mass) ==
                                       oreResult.oreInfo.oreRefObj.isReachAdvanceOreMass(oreResult.oreInfo.mass);
                            });

                            if (oreInfo != null)
                            {
                                oreInfo.updateNum(oreInfo.num + 1);   
                            }
                            else
                            {
                                oreInfo = new TreasureHuntCommonOreInfo(oreResult.oreInfo.oreRefObj,
                                    oreResult.oreInfo.oreState, 1, oreResult.oreInfo.mass, oreResult.oreInfo.getTimeMs,
                                    oreResult.oreInfo.normalSkillInfo, oreResult.oreInfo.advanceSkillInfo);
                                oreList.Add(oreInfo);
                            }
                        }
                    }
                    // 获取的物品是奇物
                    else if (resultBaseInfo is TreasureHuntCaptureResultTreasure treasureResult)
                    {
                        // 获取的物品是奇物，独立展示捕获结果
                        stepCounter.chgTotalStepCount(1);
                        resultBaseInfo.showCaptureResult(stepCounter.addDoneStepCount);

                        TreasureHuntGotTreasureInfo treasureInfo = NPPlayer.instance.treasureHuntComponent.getGotTreasureInfo(treasureResult.treasureId);
                        treasureList.Add(treasureInfo);
                    }
                    // 获取的物品是奖励
                    else if (resultBaseInfo is TreasureHuntCaptureResultReward rewardResult)
                    {
                        if (rewardResult.rewardItemList != null)
                        {
                            foreach (NPCommon.NPCommon_ItemInfo serverItemInfo in rewardResult.rewardItemList)
                            {
                                if(serverItemInfo == null)
                                    continue;
                                
                                NPCommonCostItem rewardItem = rewardItemList.Find((_item) =>
                                {
                                    return _item != null && (int)_item.getItemType() == serverItemInfo.getItemType() && _item.subId == serverItemInfo.getSubId();
                                });

                                if (rewardItem != null)
                                {
                                    rewardItem.setCount(rewardItem.count + serverItemInfo.getCount());
                                }
                                else
                                {
                                    rewardItem = new NPCommonCostItem(serverItemInfo);
                                    rewardItemList.Add(rewardItem);
                                }
                            }
                        }
                    }
                }
            }
            
            List<_ATreasureHuntCaptureHarvestItemInfo> harvestItemInfoList = new List<_ATreasureHuntCaptureHarvestItemInfo>();
            foreach (var oreInfo in oreList)
            {
                if(oreInfo == null)
                    continue;
                
                harvestItemInfoList.Add(new TreasureHuntCaptureHarvestItemInfo_Ore(oreInfo));
            }
            foreach (var treasureInfo in treasureList)
            {
                if(treasureInfo == null)
                    continue;
                
                harvestItemInfoList.Add(new TreasureHuntCaptureHarvestItemInfo_Treasure(treasureInfo));
            }
            foreach (var rewardItem in rewardItemList)
            {
                if(rewardItem == null)
                    continue;
                
                harvestItemInfoList.Add(new TreasureHuntCaptureHarvestItemInfo_Reward(rewardItem));
            }

            if (harvestItemInfoList.Count > 0)
            {
                stepCounter.chgTotalStepCount(1);
                
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_CustomAction((_setDealDone) =>
                {
                    GGUIWndTreasureHuntCaptureResult.instance.load();
                    GGUIWndTreasureHuntCaptureResult.instance.regLoadDoneDelegate(() =>
                    {
                        GGUIWndTreasureHuntCaptureResult.instance.showWnd();
                        GGUIWndTreasureHuntCaptureResult.instance.setData(harvestItemInfoList);
                    });
                }, NPNoticeType.g_AllTypeArr, ()=>
                {
                    GGUIWndTreasureHuntCaptureResult.instance.discard();
                    stepCounter.addDoneStepCount();
                }, default, UINodeTagConst.C_TREASURE_HUNT_CAPTURE_HARVEST_RESULT, true, true, true, true));
            }
            
            stepCounter.chgTotalStepCount(1);
            NPPlayer.instance.treasureHuntComponent.tryShowStationUpgradePopWnd(stepCounter.addDoneStepCount);
            
            stepCounter.addDoneStepCount();
        }
        
        #region 红点

        /// <summary>
        /// 矿石图鉴是否需要显示红点 - 当有矿石可激活或技能可升级时会显示红点
        /// </summary>
        /// <returns></returns>
        public static bool oreCatalogNeedShowRed(_ITreasureHuntOreInfo _oreInfo)
        {
            if(_oreInfo == null)
                return false;
            
            // 检查矿石状态是否为未激活普通/高级矿石（可激活）
            if (_oreInfo.oreState is ETreasureHuntOreState.NOT_ACTIVATE_NORMAL or ETreasureHuntOreState.NOT_ACTIVATE_ADVANCED)
                return true;
                    
            // 检查普通/高级技能是否可升级
            if (checkCanUpgradeSkill(_oreInfo.normalSkillInfo) || checkCanUpgradeSkill(_oreInfo.advanceSkillInfo))
                return true;

            if(_oreInfo is TreasureHuntGotOreInfo gotOreInfo)
            {
                // 检查是否有可领取的RecordReward
                if (gotOreInfo.hasCanDrawRecordReward())
                    return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// 奇物图鉴是否需要显示红点 - 当有奇物可激活或技能可升级时会显示红点
        /// </summary>
        /// <param name="_treasureInfo"></param>
        /// <returns></returns>
        public static bool treasureCatalogNeedShowRed(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            if(_treasureInfo == null)
                return false;
            
            // 检查奇物状态是否为未激活（可激活）
            if (_treasureInfo.treasureState == ETreasureHuntTreasureState.GOT_NOT_ACTIVATE)
                return true;
                    
            // 检查技能是否可升级
            if (checkCanUpgradeSkill(_treasureInfo.skillInfo))
                return true;

            return false;
        }
        
        /// <summary>
        /// 组合图鉴是否需要显示红点 - 当组合图鉴技能可激活或可升级时会显示红点
        /// </summary>
        /// <param name="_compositeCatalogInfo"></param>
        /// <returns></returns>
        public static bool compositeCatalogNeedShowRed(TreasureHuntCompositeCatalogInfo _compositeCatalogInfo)
        {
            if(_compositeCatalogInfo == null)
                return false;

            // 组合图鉴技能
            if(_compositeCatalogInfo.normalSkillInfo != null && 
               (_compositeCatalogInfo.normalSkillInfo.skillState == ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE || 
                checkCanUpgradeSkill(_compositeCatalogInfo.normalSkillInfo)))
                return true;

            return false;
        }
        
        #endregion
    }
}