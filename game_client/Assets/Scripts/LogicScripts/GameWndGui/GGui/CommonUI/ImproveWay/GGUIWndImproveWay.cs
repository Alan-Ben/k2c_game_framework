using ALPackage;
using Common.ConsortObj;
using CommonEnum;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace GOE
{
    /// <summary>
    /// 提升途径弹窗
    /// </summary>
    public class GGUIWndImproveWay : _ANPGGUIBasicWnd<GGUIMonoImproveWay>
    {
        private static GGUIWndImproveWay _g_instance;

        public static GGUIWndImproveWay instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndImproveWay();
                return _g_instance;
            }
        }

        //获取途径item缓存池
        private NPGAccessWayItemCache<NPGGUIWndAccessWayItem, NPGGUIMonoAccessWayItem> _m_cacheAccessWayItem;
        //使用道具途径item
        private GGUIWndImproveWayUseItem _m_wUseItem;
        //提升目标类型
        private EImproveTargetType _m_eTargetType;

        private GGUIWndImproveWay() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoImproveWay.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoImproveWay.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _resetAllItem();
        }

        protected override void _onReset()
        {
            _m_wUseItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_cacheAccessWayItem?.discard();
            _m_cacheAccessWayItem = null;

            _m_wUseItem?.discard();
            _m_wUseItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //item 缓存池
            if (wnd.goItemParent != null)
            {
                if (wnd.monoAccessWay != null)
                {
                    _m_cacheAccessWayItem = new NPGAccessWayItemCache<NPGGUIWndAccessWayItem, NPGGUIMonoAccessWayItem>(wnd.goItemParent, 0, 5);
                    _m_cacheAccessWayItem.init(wnd.monoAccessWay);
                }

                if (wnd.monoUseItem != null)
                {
                    GGUIMonoImproveWayUseItem monoImproveWayUseItem = GameObject.Instantiate(wnd.monoUseItem);
                    monoImproveWayUseItem.transform.SetParent(wnd.goItemParent);
                    monoImproveWayUseItem.transform.localPosition = Vector3.zero;
                    monoImproveWayUseItem.transform.localScale = Vector3.one;
                    _m_wUseItem = new GGUIWndImproveWayUseItem(monoImproveWayUseItem);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(EImproveTargetType _type)
        {
            _m_eTargetType = _type;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_cacheAccessWayItem == null)
                return;

            _resetAllItem();
            GGUIMonoImproveWayTargetTypeParam targetTypeParam = wnd.getImproveTargetTypeParam(_m_eTargetType);
            if (targetTypeParam == null)
                return;

            List<EImproveWayType> showImproveWayList = targetTypeParam.showImproveWayList;
            if (showImproveWayList == null)
                return;

            //设置描述
            ALUGUICommon.setLabelTxt(wnd.txtImproveTipDesc, TextTranslate.instance.getLanguage(targetTypeParam.improveTipDescKey));

            //排序
            showImproveWayList.Sort(_sortImproveWayType);
            //创建item
            for (int i = 0; i < showImproveWayList.Count; i++)
            {
                EImproveWayType curImproveWayType = showImproveWayList[i];
                if (curImproveWayType != EImproveWayType.USE_ITEM)
                {
                    NPGGUIWndAccessWayItem wayItem = _m_cacheAccessWayItem.popAccessItem();
                    if (wayItem != null)
                    {
                        NPAccessInfo accessInfo = new NPAccessInfo(wnd.getAccessId(curImproveWayType), true);
                        wayItem.showWnd();
                        wayItem.setInfo(accessInfo);
                    }
                }
                else
                {
                    //设置一下层级
                    _m_wUseItem?.rectTransform?.SetSiblingIndex(i);
                    _m_wUseItem?.showWnd();
                    _m_wUseItem?.setInfo(_m_eTargetType, targetTypeParam.useItemWayDescKey, targetTypeParam.useItemGoToEffect);
                }
            }
        }

        /// <summary>
        /// 重置item
        /// </summary>
        private void _resetAllItem()
        {
            if (_m_cacheAccessWayItem != null)
                _m_cacheAccessWayItem.pushBackAllAccessItem();

            _m_wUseItem?.hideWnd();
        }

        /// <summary>
        /// 排序提升途径类型
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        private int _sortImproveWayType(EImproveWayType _a, EImproveWayType _b)
        {
            int sortIdA = _getSortId(_a);
            int sortIdB = _getSortId(_b);
            if (sortIdA != sortIdB)
                return sortIdA.CompareTo(sortIdB);

            return _a.CompareTo(_b);
        }

        /// <summary>
        /// 获取排序id，1:可提升1  2:可提升2  3:不可提升
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        private int _getSortId(EImproveWayType _type)
        {
            if (wnd == null)
                return 3;

            switch (_type)
            {
                //招募员工
                case EImproveWayType.RECRUIT_EMPLOYEES:
                    //获取招聘员工最低消耗建筑
                    BusinessBuildingInfo targetBuilding = NPPlayer.instance.buildingComp.getBusinessBuildingInfo(EBusinessBuildingType.EMPLOYEE_LOWEST_COST);
                    if (targetBuilding == null)
                        return 3;
                    if (GCommon.isItemEnough(targetBuilding.getHirCost(wnd.recruitEmployeesMinNum_First), false))
                        return 1;
                    else if (GCommon.isItemEnough(targetBuilding.getHirCost(wnd.recruitEmployeesMinNum_Second), false))
                        return 2;
                    else
                        return 3;
                //培养顾问
                case EImproveWayType.TRAIN_HERO:
                    //获取等级最低的顾问
                    HeroInfo heroInfo = null;
                    NPPlayer.instance.heroComponent.dealAllHero(_info =>
                    {
                        if (heroInfo == null || _info.level < heroInfo.level)
                            heroInfo = _info;
                    });
                    if (heroInfo == null)
                        return 3;

                    long firstExpCount = 0;
                    long secondExpCount = 0;
                    GRefdataCoreMgr.instance.heroLevelRefCore.dealAllRef(_refObj =>
                    {
                        if (_refObj != null && _refObj.level >= heroInfo.level)
                        {
                            if(_refObj.level < (heroInfo.level + wnd.trainHeroUpgradeLevel_First))
                                firstExpCount += _refObj.need_exp;

                            if (_refObj.level < (heroInfo.level + wnd.trainHeroUpgradeLevel_Second))
                                secondExpCount += _refObj.need_exp;
                        }
                    });
                    if (GCommon.isItemEnough(ENPItemType.CURRENCY, (long) ECurrency.HERO_EXP, firstExpCount, false))
                        return 1;

                    if (GCommon.isItemEnough(ENPItemType.CURRENCY, (long)ECurrency.HERO_EXP, secondExpCount, false))
                        return 2;

                    return 3;
                //使用道具
                case EImproveWayType.USE_ITEM:
                    List<BagItemUseRefObj> bagItemUseRefList = ListPool<BagItemUseRefObj>.Get();
                    GRefdataCoreMgr.instance.getBagItemUseRefByImproveTargetType(_m_eTargetType, bagItemUseRefList);
                    for (int i = 0; i < bagItemUseRefList.Count; i++)
                    {
                        if (bagItemUseRefList[i] != null && GCommon.isItemEnough(ENPItemType.BAG_ITEM, bagItemUseRefList[i].id, 1, false))
                        {
                            ListPool<BagItemUseRefObj>.Release(bagItemUseRefList);
                            return 1;
                        }
                    }
                    ListPool<BagItemUseRefObj>.Release(bagItemUseRefList);
                    return 2;
                //培养学员
                case EImproveWayType.TRAIN_CHILD:
                    List<SeatInfo> seatInfoList = ListPool<SeatInfo>.Get();
                    NPPlayer.instance.childComp.getSeatListNonAlloc(seatInfoList);
                    bool haveFirst = false;
                    bool haveSecond = false;
                    foreach (SeatInfo seatInfo in seatInfoList)
                    {
                        if(seatInfo == null)
                            continue;

                        if (seatInfo.energy >= wnd.trainChildLeftCount_First)
                            haveFirst = true;

                        if (seatInfo.energy >= wnd.trainChildLeftCount_Second)
                            haveSecond = true;
                    }
                    if (haveFirst)
                        return 1;
                    if (haveSecond)
                        return 2;
                    return 3;
                //培养情人技能
                case EImproveWayType.TRAIN_CONSORT_SKILL:
                {
                    List<GGottenConsortInfo> consortInfoList = ListPool<GGottenConsortInfo>.Get();
                    NPPlayer.instance.consortComp.getConsortList(consortInfoList);
                    int curSortId = 3;
                    if (consortInfoList != null)
                    {
                        foreach (GGottenConsortInfo consortInfo in consortInfoList)
                        {
                            if (consortInfo == null)
                                continue;

                            foreach (ConsortBusinessSkillRefObj businessSkillRefObj in GRefdataCoreMgr.instance.consortBusinessSkillRefCore.refList)
                            {
                                if (businessSkillRefObj == null)
                                    continue;

                                consortInfo.getBusinessSkillInfo(businessSkillRefObj.id, _businessSkillInfo =>
                                {

                                    if (_businessSkillInfo == null)
                                    {
                                        //刚解锁
                                        if (businessSkillRefObj.unlock_need_intimacy <= consortInfo.intimacy)
                                        {
                                            OpCostRefObj normalOpCostRefObj = businessSkillRefObj.normalOpCostGroupRefObj?.getOpCostRefObj(0);
                                            OpCostRefObj advanceOpCostRefObj = businessSkillRefObj.advanceOpCostGroupRefObj?.getOpCostRefObj(0);
                                            if (GCommon.isItemEnough(normalOpCostRefObj?.cost_item, false) || GCommon.isItemEnough(advanceOpCostRefObj?.cost_item, false))
                                                curSortId = 2;
                                        }
                                    }
                                    else
                                    {
                                        //已解锁未满级
                                        if (!_businessSkillInfo.isReachAddMax)
                                        {
                                            OpCostRefObj normalOpCostRefObj = businessSkillRefObj.normalOpCostGroupRefObj?.getOpCostRefObj(_businessSkillInfo.normalOpCount);
                                            OpCostRefObj advanceOpCostRefObj = businessSkillRefObj.advanceOpCostGroupRefObj?.getOpCostRefObj(_businessSkillInfo.advanceOpCount);
                                            if (GCommon.isItemEnough(normalOpCostRefObj?.cost_item, false) || GCommon.isItemEnough(advanceOpCostRefObj?.cost_item, false))
                                                curSortId = 2;
                                        }
                                    }
                                });
                            }
                        }
                    }
                    ListPool<GGottenConsortInfo>.Release(consortInfoList);
                    return curSortId;
                }
                //培养情人加护
                case EImproveWayType.TRAIN_CONSORT_BLESS:
                {
                    List<GGottenConsortInfo> consortInfoList = ListPool<GGottenConsortInfo>.Get();
                    NPPlayer.instance.consortComp.getConsortList(consortInfoList);
                    int curSortId = 3;
                    foreach (GGottenConsortInfo consortInfo in consortInfoList)
                    {
                        if(consortInfo == null)
                            continue;

                        foreach (ConsortBlessSkillInfo blessSkillInfo in consortInfo.blessSkillInfoList)
                        {
                            if (blessSkillInfo == null)
                                continue;

                            // 若技能等级未满级且技能点足够升级
                            if (blessSkillInfo.checkHasCanLevelUp(consortInfo))
                            {
                                curSortId = 2;
                                break;
                            }
                        }
                    }
                    ListPool<GGottenConsortInfo>.Release(consortInfoList);
                    return curSortId;
                }
                default:
                    return 3;
            }
            return 3;
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_IMPROVE_WAY);
        }
    }
}
