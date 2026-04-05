using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场选择伙伴弹窗
    /// </summary>
    public class GGUIWndArenaBattleSelectHero : _ANPGGUIBasicWnd<GGUIMonoArenaBattleSelectHero>, _IScrollerSmoothMovable
    {
        private static GGUIWndArenaBattleSelectHero _g_instance;
        public static GGUIWndArenaBattleSelectHero instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndArenaBattleSelectHero();
                return _g_instance;
            }
        }

        //对手cid
        private long _m_lOpponentCid;
        //数据库id
        private long _m_lDbId;
        //谈判类型
        private EArenaSelectBattleType _m_eBattleType;
        //筛选类型
        private ESpecAttrType _m_eFilterType;
        //指定谈判选择道具类型
        private EArenaSelectAttackConsumeItemType _m_eSelectAttackItemType;
        //伙伴列表
        private List<HeroInfo> _m_lHeroInfo;
        //选中的伙伴形象
        private NPGGuiWndTexture _m_wSelectHeroTex;
        //伙伴形象展示
        private NPGGUIWndCommonShowCase _m_wHeroShowCase;
        //伙伴列表
        private GGUIWndArenaBattleSelectHeroGrid _m_wHeroGrid;
        //觉醒技能实力加成列表
        private GGUIWndArenaStarSkillAddPowerContainer _m_wStarSkillAddPowerContainer;
        //筛选子窗口
        private GGUIWndHeroMainFilter _m_wHeroListFilter;
        //指定谈判消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;
        //指定普通谈判开关
        private NPGGUIWndCommonToggleEx _m_wNormalCostToggle;
        //指定高级谈判开关
        private NPGGUIWndCommonToggleEx _m_wAdvancedCostToggle;
        //指定特级谈判开关
        private NPGGUIWndCommonToggleEx _m_wSuperCostToggle;
        //不选中初始增益item
        private GGUIWndArenaBattleInitBuffItem _m_wNoSelectItem;
        //初始增益列表
        private List<GGUIWndArenaBattleInitBuffItem> _m_lBuffItemList;
        //当前选中的增益id
        private long _m_lCurSelectBuffId;
        //是否正在移动
        private bool _m_bIsMoving;
        //显示觉醒技能实力加成列表序列
        private long _m_lShowStarSkillAddPowerListSerialize;

        public GGUIWndArenaBattleSelectHero() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattleSelectHero.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattleSelectHero.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }
        public ScrollRect scrollRect { get => wnd?.scrollRect; }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_SELECT_ARENA_HERO_BY_INDEX, _onSimulateSelectHeroByIndex);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_ARENA_OPEN_SELECT_INITIAL_BUFF, _onSimulateOpenSelectInitialBuff);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_ARENA_SELECT_NULL_INITIAL_BUFF, _onSimulateSelectNullInitialBuff);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_ARENA_START_FIGHT, _onSimulateClickStartFight);
            //默认指定谈判选择普通道具
            _m_eSelectAttackItemType = EArenaSelectAttackConsumeItemType.SIMPLE;
            //默认筛选全部
            _m_eFilterType = ESpecAttrType.NONE;

            _m_bIsMoving = false;
            _m_lCurSelectBuffId = -1;

            //设置默认显示状态
            if (_m_wHeroListFilter != null)
            {
                _m_wHeroListFilter.showWnd();
                _m_wHeroListFilter.initState(_m_eFilterType);
            }
            if (_m_wNormalCostToggle != null)
            {
                _m_wNormalCostToggle.showWnd();
                _m_wNormalCostToggle.setSelected(true, true);
            }
            if (_m_wAdvancedCostToggle != null)
            {
                _m_wAdvancedCostToggle.showWnd();
                _m_wAdvancedCostToggle.setSelected(false, true);
            }
            if (_m_wSuperCostToggle != null)
            {
                _m_wSuperCostToggle.showWnd();
                _m_wSuperCostToggle.setSelected(false, true);
            }
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_SELECT_ARENA_HERO_BY_INDEX, _onSimulateSelectHeroByIndex);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_ARENA_OPEN_SELECT_INITIAL_BUFF, _onSimulateOpenSelectInitialBuff);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_ARENA_SELECT_NULL_INITIAL_BUFF, _onSimulateSelectNullInitialBuff);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_ARENA_START_FIGHT, _onSimulateClickStartFight);
            _m_bIsMoving = false;
            _m_lCurSelectBuffId = -1;
            _m_wHeroGrid?.hideWnd();
            _m_wCostItem?.hideWnd();
            _m_wNormalCostToggle?.hideWnd();
            _m_wAdvancedCostToggle?.hideWnd();
            _m_wSuperCostToggle?.hideWnd();
            _m_wSelectHeroTex?.hideWnd();
            _m_wHeroShowCase?.hideWnd();
            _m_wHeroListFilter?.hideWnd();
            _m_wStarSkillAddPowerContainer?.hideWnd();
            _m_wNoSelectItem?.hideWnd();
            if (_m_lBuffItemList != null)
            {
                for (int i = 0; i < _m_lBuffItemList.Count; i++)
                {
                    _m_lBuffItemList[i]?.hideWnd();
                }
            }

            _m_lShowStarSkillAddPowerListSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wHeroGrid?.resetWnd();
            _m_wCostItem?.resetWnd();
            _m_wNormalCostToggle?.resetWnd();
            _m_wAdvancedCostToggle?.resetWnd();
            _m_wSuperCostToggle?.resetWnd();
            _m_wHeroListFilter?.resetWnd();
            _m_wStarSkillAddPowerContainer?.resetWnd();
            _m_wNoSelectItem?.resetWnd();
            _m_wSelectHeroTex?.discardTexture();
            _m_wHeroShowCase?.resetWnd();
            if (_m_lBuffItemList != null)
            {
                for (int i = 0; i < _m_lBuffItemList.Count; i++)
                {
                    _m_lBuffItemList[i]?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            _m_wHeroGrid?.discard();
            _m_wHeroGrid = null;
            _m_wCostItem?.discard();
            _m_wCostItem = null;
            _m_wNormalCostToggle?.discard();
            _m_wNormalCostToggle = null;
            _m_wAdvancedCostToggle?.discard();
            _m_wAdvancedCostToggle = null;
            _m_wSuperCostToggle?.discard();
            _m_wSuperCostToggle = null;
            _m_wHeroListFilter?.discard();
            _m_wHeroListFilter = null;
            _m_wStarSkillAddPowerContainer?.discard();
            _m_wStarSkillAddPowerContainer = null;
            _m_wNoSelectItem?.discard();
            _m_wNoSelectItem = null;
            _m_wSelectHeroTex?.discard();
            _m_wSelectHeroTex = null;
            _m_wHeroShowCase?.discard();
            _m_wHeroShowCase = null;
            if (_m_lBuffItemList != null)
            {
                for (int i = 0; i < _m_lBuffItemList.Count; i++)
                {
                    _m_lBuffItemList[i]?.discard();
                }
                _m_lBuffItemList.Clear();
                _m_lBuffItemList = null;
            }

            if (wnd == null)
                return; 

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);//点击关闭
            ALUGUICommon.uncombineBtnClick(wnd.btnCloseTwo, _onClickClose);//点击关闭
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);//点击下一步
            ALUGUICommon.uncombineBtnClick(wnd.btnCostNext, _onClickCostNext);//点击指定谈判下一步
            ALUGUICommon.uncombineBtnClick(wnd.btnHeroPowerDetail, _onClickHeroPowerDetail);//点击查看伙伴实力详情
            ALUGUICommon.uncombineBtnClick(wnd.btnLastStep, _onClickBackToHeroList);//点击返回伙伴列表按钮
            ALUGUICommon.uncombineBtnClick(wnd.btnStart, _onClickStart);//点击开始按钮
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_eFilterType = ESpecAttrType.NONE;

            if (wnd.monoHeroGrid != null)
            {
                _m_wHeroGrid = new GGUIWndArenaBattleSelectHeroGrid(wnd.monoHeroGrid);
                _m_wHeroGrid.clickDelegate += _onClickHeroItem;
            }

            if (wnd.monoFilter != null)
            {
                _m_wHeroListFilter = new GGUIWndHeroMainFilter(wnd.monoFilter);
                _m_wHeroListFilter.initState(_m_eFilterType);
                _m_wHeroListFilter.onFilterChanged += _onFilterChg;
            }

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            if (wnd.monoSelectNormalCost != null)
            {
                _m_wNormalCostToggle = new NPGGUIWndCommonToggleEx(wnd.monoSelectNormalCost);
                _m_wNormalCostToggle.clickDelegate += _onClickNormalCostToggle;
            }

            if (wnd.monoSelectAdvancedCost != null)
            {
                _m_wAdvancedCostToggle = new NPGGUIWndCommonToggleEx(wnd.monoSelectAdvancedCost);
                _m_wAdvancedCostToggle.clickDelegate += _onClickAdvancedCostToggle;
            }

            if (wnd.monoSelectSuperCost != null)
            {
                _m_wSuperCostToggle = new NPGGUIWndCommonToggleEx(wnd.monoSelectSuperCost);
                _m_wSuperCostToggle.clickDelegate += _onClickSuperCostToggle;
            }

            if (wnd.imgSelectHero != null)
                _m_wSelectHeroTex = new NPGGuiWndTexture(wnd.imgSelectHero);

            if (wnd.monoSelectHeroShowCase != null)
                _m_wHeroShowCase = new NPGGUIWndCommonShowCase(wnd.monoSelectHeroShowCase);

            if (wnd.monoStarSkillContainer != null)
                _m_wStarSkillAddPowerContainer = new GGUIWndArenaStarSkillAddPowerContainer(wnd.monoStarSkillContainer);

            if (wnd.monoNoSelectItem != null)
            {
                _m_wNoSelectItem = new GGUIWndArenaBattleInitBuffItem(wnd.monoNoSelectItem);
                _m_wNoSelectItem.onClickItem += _onClickSelectBuff;
            }

            if (wnd.monoBuffItemList != null)
            {
                _m_lBuffItemList = new List<GGUIWndArenaBattleInitBuffItem>();
                for (int i = 0; i < wnd.monoBuffItemList.Count; i++)
                {
                    GGUIWndArenaBattleInitBuffItem item = new GGUIWndArenaBattleInitBuffItem(wnd.monoBuffItemList[i]);
                    item.onClickItem += _onClickSelectBuff;
                    _m_lBuffItemList.Add(item);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);//点击关闭
            ALUGUICommon.combineBtnClick(wnd.btnCloseTwo, _onClickClose);//点击关闭
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);//点击免费谈判
            ALUGUICommon.combineBtnClick(wnd.btnCostNext, _onClickCostNext);//点击指定谈判下一步
            ALUGUICommon.combineBtnClick(wnd.btnHeroPowerDetail, _onClickHeroPowerDetail);//点击查看伙伴实力详情
            ALUGUICommon.combineBtnClick(wnd.btnLastStep, _onClickBackToHeroList);//点击返回伙伴列表按钮
            ALUGUICommon.combineBtnClick(wnd.btnStart, _onClickStart);//点击开始按钮
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_type">谈判类型</param>
        /// <param name="_opponentCid">对手cid</param>
        public void setInfo(EArenaSelectBattleType _type, long _opponentCid, long _fightBackDbId)
        {
            _m_eBattleType = _type;
            _m_lOpponentCid = _opponentCid;
            _m_lDbId = _fightBackDbId;
            _m_lCurSelectBuffId = -1;
            wnd?.aniClickNext?.resetAni();
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshHeroList();
            _refreshButtonState();
            _refreshSelectHeroTex();
            _refreshStarSkillAddPowerContainer();
            _refreshBuffContainer();
        }

        //刷新伙伴列表
        private void _refreshHeroList()
        {
            if (_m_lHeroInfo == null)
                _m_lHeroInfo = new List<HeroInfo>();
            _m_lHeroInfo.Clear();

            //根据筛选获取伙伴列表
            NPPlayer.instance.heroComponent.dealAllHero((_heroInfo) =>
            {
                if(_heroInfo != null && (_m_eFilterType == ESpecAttrType.NONE || _heroInfo.specAttrType == _m_eFilterType))
                    _m_lHeroInfo.Add(_heroInfo);
            });
            //排序
            _m_lHeroInfo.Sort(_sortHero);
            //显示伙伴列表
            if (_m_wHeroGrid != null)
            {
                _m_wHeroGrid?.showWnd();
                _m_wHeroGrid?.showHeroList(_m_lHeroInfo, _m_eBattleType == EArenaSelectBattleType.SELECT);
            }
        }

        //刷新谈判按钮状态
        private void _refreshButtonState()
        {
            if (wnd == null)
                return;

            if (_m_eBattleType == EArenaSelectBattleType.SELECT)
            {
                //指定谈判类型
                ALUGUICommon.setGameObjEnable(wnd.goRandomShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goRandomHideList, true);

                //获取消耗道具和倍率
                NPCommonCostItem selectAttackCostItem = null;
                long selectAttackRatio = 0;
                GRefdataCoreMgr.instance.arenaSelectAttackConsumeRefCore.dealAllRef(_ref =>
                {
                    if (_ref != null && _ref.type == _m_eSelectAttackItemType)
                    {
                        selectAttackCostItem = _ref.cost;
                        selectAttackRatio = _ref.ratio;
                    }
                });

                //设置消耗道具
                _m_wCostItem?.showWnd();
                _m_wCostItem?.setItem(selectAttackCostItem);

                //显示倍率
                ALUGUICommon.setLabelTxt(wnd.txtInfluenceCount, TextTranslate.instance.getLanguage(TransKeyConst.common_multiple_num, selectAttackRatio));
                ALUGUICommon.setLabelTxt(wnd.txtItemCount, TextTranslate.instance.getLanguage(TransKeyConst.common_multiple_num, selectAttackRatio));
                ALUGUICommon.setLabelTxt(wnd.txtPowerCount, TextTranslate.instance.getLanguage(TransKeyConst.common_multiple_num, selectAttackRatio));
            }
            else
            {
                //随机谈判类型
                ALUGUICommon.setGameObjEnable(wnd.goRandomHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goRandomShowList, true);
            }
        }

        //刷新选中的伙伴形象
        private void _refreshSelectHeroTex()
        {
            if (wnd == null)
                return;

            bool isSelect = false;

            if (_m_wHeroGrid != null && _m_wHeroGrid.curSelectHeroInfo != null)
            {
                _m_wSelectHeroTex?.showWnd();
                _m_wSelectHeroTex?.setTexture(_m_wHeroGrid.curSelectHeroInfo.getCardImage());
                _m_wHeroShowCase?.showWnd(new ShowCaseCommonResUnitInfoObj(_m_wHeroGrid.curSelectHeroInfo.getTdShow()));
                isSelect = true;
            }
            else
            {
                _m_wSelectHeroTex?.hideWnd();
                _m_wHeroShowCase?.hideWnd();
                isSelect = false;
            }

            ALUGUICommon.setGameObjEnable(wnd.goNoSelectHeroShowList, !isSelect);
            ALUGUICommon.setGameObjEnable(wnd.goNoSelectHeroHideList, isSelect);
        }

        //刷新觉醒技能实力加成列表
        private void _refreshStarSkillAddPowerContainer()
        {
            if (_m_wHeroGrid == null)
                return;

            if(_m_wHeroGrid.curSelectHeroInfo == null)
                _m_wStarSkillAddPowerContainer?.hideWnd();
            else
            {
                HeroInfo curSelectHeroInfo = _m_wHeroGrid.curSelectHeroInfo;
                if(curSelectHeroInfo== null)
                    return;

                //筛选对该伙伴有加成的觉醒技能
                List<ArenaStarSkillAddInfo> starSkillAddInfoList = new List<ArenaStarSkillAddInfo>();
                List<HeroStarSkillInfo> targetInfoList = new List<HeroStarSkillInfo>();
                CommonUnionBonusMgr _m_commonUnionBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.HERO);
                NPPlayer.instance.heroComponent.dealAllHero(_heroInfo =>
                {
                    if (_heroInfo != null)
                    {
                        //觉醒技能列表
                        List<HeroStarSkillInfo> starSkillInfoList = _heroInfo.starSkillInfoMgr.starSkillList;
                        for (int i = 0; i < starSkillInfoList.Count; i++)
                        {
                            HeroStarSkillInfo temp = starSkillInfoList[i];
                            if (temp == null || temp.curStarSkillLevelRefObj == null || temp.curStarSkillLevelRefObj.union_bonus == null)
                                continue;

                            //设置加成容器
                            _m_commonUnionBonusMgr.clear();
                            _m_commonUnionBonusMgr.addBonus(temp.curStarSkillLevelRefObj.union_bonus.unionBonus);

                            //是否对选中的伙伴有加成
                            long addValue = _m_commonUnionBonusMgr.getTotalPropertyBonus(EBonusPropertyType.ARENA_POWER_ADD_PER, curSelectHeroInfo.toJudgeUnionBonus());
                            if (addValue > 0)
                                starSkillAddInfoList.Add(new ArenaStarSkillAddInfo(addValue, temp));
                        }
                    }
                });

                //按照加成值从大到小排序
                starSkillAddInfoList.Sort((_a, _b) =>-(_a.value.CompareTo(_b.value)));

                //获取目标列表
                for (int i = 0; i < starSkillAddInfoList.Count; i++)
                {
                    targetInfoList.Add(starSkillAddInfoList[i].starSkillInfo);
                }

                _m_lShowStarSkillAddPowerListSerialize = ALSerializeOpMgr.next();
                _dealShowStarSkillAddPowerList(targetInfoList, _m_lShowStarSkillAddPowerListSerialize);
            }
        }

        //分批显示觉醒技能实力加成列表
        private void _dealShowStarSkillAddPowerList(List<HeroStarSkillInfo> _infoList, long _serialize)
        {
            if (wnd == null || _infoList == null || _infoList.Count <= 0 || _m_lShowStarSkillAddPowerListSerialize != _serialize)
                return;

            List<HeroStarSkillInfo> showList = new List<HeroStarSkillInfo>();
            if (_infoList.Count > wnd.eachBatchSkillAddPowerShowCount)
            {
                showList.AddRange(_infoList.GetRange(0, wnd.eachBatchSkillAddPowerShowCount));
                _infoList.RemoveRange(0, wnd.eachBatchSkillAddPowerShowCount);
            }
            else
            {
                showList.AddRange(_infoList);
                _infoList.Clear();
            } 

            //展示列表
            _m_wStarSkillAddPowerContainer?.hideWnd();
            _m_wStarSkillAddPowerContainer?.showWnd();
            _m_wStarSkillAddPowerContainer?.showItemList(showList);

            //如果没有列表了不再展示
            if(_infoList.Count <= 0)
                return;

            //延时展示下一批
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (_m_lShowStarSkillAddPowerListSerialize != _serialize)
                    return;

                _dealShowStarSkillAddPowerList(_infoList, _serialize);
            }, wnd.eachBatchSkillAddPowerShowSec);
        }

        //刷新初始增益道具列表
        private void _refreshBuffContainer()
        {
            List<long> buffIdList = GRefdataCoreMgr.instance.npGeneral.arena_initial_choose_buff_list;

            _m_wNoSelectItem?.showWnd();
            _m_wNoSelectItem?.setInfo(0);
            _m_wNoSelectItem?.setSelect(_m_lCurSelectBuffId == 0);
            for (int i = 0; i < buffIdList.Count; i++)
            {
                long buffId = buffIdList[i];
                if (_m_lBuffItemList != null && _m_lBuffItemList.Count > i && _m_lBuffItemList[i] != null)
                {
                    _m_lBuffItemList[i].showWnd();
                    _m_lBuffItemList[i].setInfo(buffId);
                    _m_lBuffItemList[i].setSelect(buffId == _m_lCurSelectBuffId);
                }
            }
        }

        //刷新当前选择的伙伴血量展示
        private void _refreshCurBlood()
        {
            if (wnd == null || _m_wHeroGrid == null || _m_wHeroGrid.curSelectHeroInfo == null)
                return;

            long arenaPower = _m_wHeroGrid.curSelectHeroInfo.getArenaBasePower();
            ALUGUICommon.setLabelTxt(wnd.txtCurBlood, TextTranslate.instance.getLanguage(TransKeyConst.arena_curBlood_num_num, arenaPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), arenaPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
        }

        //排序：未使用 > 已使用，实力从大到小，等级从大到小，品质从高到低，id从小到大
        private int _sortHero(HeroInfo _a, HeroInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            ArenaInfo info = NPPlayer.instance.arenaComp.arenaInfo;
            bool isAlreadySelectA = false;
            bool isAlreadySelectB = false;

            if (_m_eBattleType == EArenaSelectBattleType.SELECT)
            {
                isAlreadySelectA = info != null && info.hadSelectAttackHeroList != null && info.hadSelectAttackHeroList.Contains(_a.id);
                isAlreadySelectB = info != null && info.hadSelectAttackHeroList != null && info.hadSelectAttackHeroList.Contains(_b.id);
            }
            else
            {
                isAlreadySelectA = info != null && info.hadRandomAttackHeroList != null && info.hadRandomAttackHeroList.Contains(_a.id);
                isAlreadySelectB = info != null && info.hadRandomAttackHeroList != null && info.hadRandomAttackHeroList.Contains(_b.id);
            }

            if (isAlreadySelectA != isAlreadySelectB)
                return isAlreadySelectA.CompareTo(isAlreadySelectB);

            int powerCompare = _a.getArenaBasePower().CompareTo(_b.getArenaBasePower());
            if (powerCompare != 0)
                return -powerCompare;

            int levelComp = _a.level.CompareTo(_b.level);
            if (levelComp != 0)
                return -levelComp;

            EQuality qualityA = GCommon.getItemQuality(ENPItemType.HERO, _a.id);
            EQuality qualityB = GCommon.getItemQuality(ENPItemType.HERO, _b.id);
            if (qualityA != qualityB)
                return -(qualityA.CompareTo(qualityB));

            return _a.id.CompareTo(_b.id);
        }

        //处理请求开始进行比赛
        private void _dealReqFight(long _selectBuffId)
        {
            if (_m_wHeroGrid == null || _m_wHeroGrid.curSelectHeroInfo == null)
                return;

            if (_m_eBattleType == EArenaSelectBattleType.RANDOM)
            {
                //请求随机谈判
                int inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                NPPlayer.instance.arenaComp.reqRandomAttackSelectHero(_m_wHeroGrid.curSelectHeroInfo.id, AccountSettingMgr.instance.accountSetting.arenaFightBotName, _selectBuffId, (_isSuc) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                    if (!_isSuc)
                        return;

                    //退出选择伙伴界面
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_SELECT_HERO);
                    //退出准备界面
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_PREPARE);
                    //打开谈判界面
                    QueueMgr.instance.AddNode(new GMainQueueArenaBattleNode());
                });
            }
            else
            {
                //请求指定谈判
                //获取消耗道具和倍率
                ArenaSelectAttackConsumeRefObj selectAttackItem = null;
                GRefdataCoreMgr.instance.arenaSelectAttackConsumeRefCore.dealAllRef(_ref =>
                {
                    if (_ref != null && _ref.type == _m_eSelectAttackItemType)
                        selectAttackItem = _ref;
                });

                int inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                Action<bool> onFightReqReturn = (_isSuc) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                    if (!_isSuc)
                        return;

                    //退出选择伙伴界面
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_SELECT_HERO);
                    //退出准备界面
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_PREPARE);
                    //打开谈判界面
                    QueueMgr.instance.AddNode(new GMainQueueArenaBattleNode());
                };

                //指定谈判，并且是机器人，用另外的协议处理
                if (_m_lOpponentCid <= 0 && NPPlayer.instance.arenaComp.celebrityList != null &&
                    NPPlayer.instance.arenaComp.celebrityList.Count > 0 &&
                    NPPlayer.instance.arenaComp.celebrityList[0].isBot)
                {
                    NPPlayer.instance.arenaComp.reqSysSelectAttackSelectHero(selectAttackItem.id, _m_wHeroGrid.curSelectHeroInfo.id, _selectBuffId, onFightReqReturn);
                    return;
                }

                //有dbId则是复仇
                if (_m_lDbId > 0)
                {
                    NPPlayer.instance.arenaComp.reqFightBackSelectHero(_m_lDbId, selectAttackItem.id, _m_wHeroGrid.curSelectHeroInfo.id, _selectBuffId, onFightReqReturn);
                }
                else
                {
                    NPPlayer.instance.arenaComp.reqSelectAttackSelectHero(_m_lOpponentCid, selectAttackItem.id, _m_wHeroGrid.curSelectHeroInfo.id, _selectBuffId, onFightReqReturn);
                }
            }
        }

        #region 点击事件

        //筛选变更
        private void _onFilterChg(ESpecAttrType _type)
        {
            //保存选择
            _m_eFilterType = _type;
            _refreshHeroList();
            _refreshSelectHeroTex();
            _refreshStarSkillAddPowerContainer();
        }

        //点击选中伙伴item
        private void _onClickHeroItem(HeroInfo _item)
        {
            //正在移动过程中不处理点击
            if (_m_bIsMoving)
                return;

            _refreshSelectHeroTex();
            _refreshStarSkillAddPowerContainer();
            _refreshCurBlood();
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_SELECT_HERO);
        }

        //点击免费谈判
        private void _onClickNext(GameObject _go)
        {
            //正在移动过程中不处理点击
            if (_m_bIsMoving)
                return;

            if (_m_wHeroGrid == null || _m_wHeroGrid.curSelectHeroInfo == null)
            {
                //请选择伙伴
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_startBattleNeedSelectHero_none);
                return;
            }

            float moveTime = 0;
            if (wnd != null && wnd.moveTimeSec > 0)
                moveTime = wnd.moveTimeSec;

            //移动到选择buff界面
            _m_bIsMoving = true;
            new ScrollerSmoothMoveTaskHorizontal(this, 1, moveTime, () =>
            {
                _m_bIsMoving = false;
            }).deal();

            //播放动画
            if(wnd != null && wnd.aniClickNext != null)
                wnd.aniClickNext.forcePlay();
        }

        //点击指定谈判
        private void _onClickCostNext(GameObject _go)
        {
            //正在移动过程中不处理点击
            if (_m_bIsMoving)
                return;

            //获取消耗道具和倍率
            ArenaSelectAttackConsumeRefObj selectAttackItem = null;
            GRefdataCoreMgr.instance.arenaSelectAttackConsumeRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.type == _m_eSelectAttackItemType)
                    selectAttackItem = _ref;
            });

            if (selectAttackItem == null || !GCommon.isItemEnough(selectAttackItem.cost, true))
                return;

            if (_m_wHeroGrid == null || _m_wHeroGrid.curSelectHeroInfo == null)
            {
                //请选择伙伴
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_startBattleNeedSelectHero_none);
                return;
            }

            float moveTime = 0;
            if(wnd != null && wnd.moveTimeSec > 0)
                moveTime = wnd.moveTimeSec;

            //移动到选择buff界面
            _m_bIsMoving = true;
            new ScrollerSmoothMoveTaskHorizontal(this, 1, moveTime, () =>
            {
                _m_bIsMoving = false;
            }).deal();

            //播放动画
            if (wnd != null && wnd.aniClickNext != null)
                wnd.aniClickNext.forcePlay();
        }

        //点击查看伙伴实力详情
        private void _onClickHeroPowerDetail(GameObject _go)
        {
            if (wnd == null)
                return;

            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(5219,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_battleHeroTemporaryAddPowerPer_num, 0),
                (RectTransform)_go.transform, wnd.powerDetailIntervalX, wnd.powerDetailIntervalY));
        }

        //点击指定高级谈判开关
        private void _onClickNormalCostToggle(NPGGUIWndCommonToggleEx _toggle)
        {
            //正在移动过程中不处理点击
            if (_m_bIsMoving)
                return;

            if (_m_wNormalCostToggle == null)
                return;

            bool isOn = !_m_wNormalCostToggle.isOn;
            if (!isOn)
                return;

            _m_wNormalCostToggle.setSelected(true, true);
            _m_wAdvancedCostToggle?.setSelected(false, true);
            _m_wSuperCostToggle?.setSelected(false, true);
            _m_eSelectAttackItemType = EArenaSelectAttackConsumeItemType.SIMPLE;

            _refreshButtonState();
        }

        //点击指定高级谈判开关
        private void _onClickAdvancedCostToggle(NPGGUIWndCommonToggleEx _toggle)
        {
            //正在移动过程中不处理点击
            if (_m_bIsMoving)
                return;

            if (_m_wAdvancedCostToggle == null)
                return;

            bool isOn = !_m_wAdvancedCostToggle.isOn;
            if (!isOn)
                return;

            _m_wAdvancedCostToggle.setSelected(true, true);
            _m_wNormalCostToggle?.setSelected(false, true);
            _m_wSuperCostToggle?.setSelected(false, true);
            _m_eSelectAttackItemType = EArenaSelectAttackConsumeItemType.ADVANCED;

            _refreshButtonState();
        }

        //点击指定特级谈判开关
        private void _onClickSuperCostToggle(NPGGUIWndCommonToggleEx _toggle)
        {
            //正在移动过程中不处理点击
            if (_m_bIsMoving)
                return;

            if (_m_wSuperCostToggle == null)
                return;

            bool isOn = !_m_wSuperCostToggle.isOn;
            if (!isOn)
                return;

            _m_wSuperCostToggle.setSelected(true, true);
            _m_wNormalCostToggle?.setSelected(false, true);
            _m_wAdvancedCostToggle?.setSelected(false, true);
            _m_eSelectAttackItemType = EArenaSelectAttackConsumeItemType.SUPER;

            _refreshButtonState();
        }

        //点击返回伙伴列表
        private void _onClickBackToHeroList(GameObject _go)
        {
            //正在移动过程中不处理点击
            if (_m_bIsMoving)
                return;

            float moveTime = 0;
            if (wnd != null && wnd.moveTimeSec > 0)
                moveTime = wnd.moveTimeSec;

            _m_bIsMoving = true;
            new ScrollerSmoothMoveTaskHorizontal(this, 0, moveTime, () =>
            {
                _m_bIsMoving = false;
            }).deal();

            //播放动画
            if(wnd != null && wnd.aniClickLast != null)
                wnd.aniClickLast.forcePlay();
        }

        //点击开始
        private void _onClickStart(GameObject _go)
        {
            //正在移动过程中不处理点击
            if (_m_bIsMoving)
                return;

            if (_m_wHeroGrid == null || _m_wHeroGrid.curSelectHeroInfo == null)
            {
                //请选择伙伴
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_startBattleNeedSelectHero_none);
                return;
            }

            if (_m_lCurSelectBuffId == -1)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_needChooseInitBuffTip_none);//请先选择初始增益
                return;
            }

            if (_m_lCurSelectBuffId > 0)
            {
                ArenaBuffRefObj arenaBuffRef = GRefdataCoreMgr.instance.arenaBuffRefCore.getRef(_m_lCurSelectBuffId);
                if (arenaBuffRef == null || !GCommon.isItemEnough(arenaBuffRef.cost, true))
                    return;
            }

            _dealReqFight(_m_lCurSelectBuffId);
        }

        //点击选择buff
        private void _onClickSelectBuff(GGUIWndArenaBattleInitBuffItem _item)
        {
            //正在移动过程中不处理点击
            if (_m_bIsMoving)
                return;

            if (_item == null)
                return;

            _m_lCurSelectBuffId = _item.buffRef != null ? _item.buffRef.id : 0;
            _refreshBuffContainer();
        }


        private class ArenaStarSkillAddInfo
        {
            public long value;
            public HeroStarSkillInfo starSkillInfo;

            public ArenaStarSkillAddInfo(long _value, HeroStarSkillInfo _starSkillInfo)
            {
                value = _value;
                starSkillInfo = _starSkillInfo;
            }
        }


        #endregion

        #region 消息事件

        //模拟选择竞技场伙伴
        private void _onSimulateSelectHeroByIndex(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long index = (long)_objects[0];
            _m_wHeroGrid?.setSelectByIndex((int)index);
        }

        //模拟打开初始增益选择
        private void _onSimulateOpenSelectInitialBuff()
        {
            if (_m_eBattleType == EArenaSelectBattleType.SELECT)
            {
                _onClickCostNext(null);
            }
            else
            {
                _onClickNext(null);
            }
        }

        //模拟不选择初始增益
        private void _onSimulateSelectNullInitialBuff()
        {
            _m_lCurSelectBuffId = 0;
            _refreshBuffContainer();
        }

        //模拟点击开始
        private void _onSimulateClickStartFight()
        {
            _onClickStart(null);
        }

        #endregion
    }
}