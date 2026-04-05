using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class FarmingBuildingView : _AALBasicLoadObj
    {
        [NotNull] private readonly FarmingBuildingInfo _m_buildingInfo;
        private readonly Vector3 _m_position;
        private readonly NPGGoIndex _m_resIndex;
        private int _m_level;

        private int _m_initSerialize;

        private GTDMonoFarmingBuilding _m_mono;
        private GGUICommonFollowTarget _m_nameFollowTarget;
        private GGUICommonFollowTarget _m_earningsFollowTarget;
        private GGUICommonFollowTarget _m_multipleFollowTarget;
        private GGUICommonFollowTarget _m_clickEarningsFollowTarget;
        private GGUIWndFarmingBuildingNameFollowItemController _m_nameFollower;


        public FarmingBuildingView([NotNull] FarmingBuildingInfo _buildingInfo, Vector3 _position)
        {
            _m_buildingInfo = _buildingInfo;
            _m_position = _position;
            _m_resIndex = _buildingInfo.getCurrentResIndex();
            _m_level = _buildingInfo.level;
            WinMsg.RegisterMsg(WinMsgType.IS_BUILDING_UNDER_CONSTRUCTION, _onBuildingUnderConstruction);
        }
        

        public long id { get { return _m_buildingInfo.id; } }
        public int level { get { return _m_level; } }
        public NPGGoIndex resIndex { get { return _m_resIndex; } }
        public Vector3 position { get { return _m_position; } }


        public void refresh()
        {
            _m_level = _m_buildingInfo.level;
            _m_nameFollower?.refreshWnd();
            _refreshUnderConstructionShow(NPPlayer.instance.buildingComp.haveBuildingUnderConstruction);
        }
        public void showEarningTip(float _deltaTime)
        {
            if (_m_earningsFollowTarget == null || _m_buildingInfo.earningsPerS <= 0)
                return;
            
            GGUIWndBuildingCoinEarningTipFollowItemController earningTip = new GGUIWndBuildingCoinEarningTipFollowItemController();
            earningTip.setCoinNum((long)(_m_buildingInfo.earningsPerS * _deltaTime));
            _m_earningsFollowTarget.addController(earningTip);
        }
        public void moveFocusToFarmingBuildingUpgradeBtnAndShowGuideHand()
        {
            if (_m_mono == null || _m_mono.levelUpClickMono == null)
                return;
            
            MainAdditionBuildingTDScene.instance.focusToTarget(_m_mono.levelUpClickMono.transform.position, 0.5f);
            if (!Game.instance.isInTutorial)
            {
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (_m_mono == null || _m_buildingInfo == null)
                        return;
                    
                    //如果不在引导并且条件通过，展示入口手指引导
                    if (!Game.instance.isInTutorial && GRefdataCoreMgr.instance.npGeneral.entrance_show_hand_guide_cond.IsEnable(null))
                        WinMsg.SendMsg(WinMsgType.SHOW_GUIDE_HAND, _m_mono.levelUpClickMono.transform, _m_buildingInfo.baseRef.upgrade_btn_guide_hand_res_id);
                }, 0.5f);
            }
        }
        
        
        protected override void _loadOp()
        {
            MainAdditionBuildingTDScene.instance.createBuilding<GTDMonoFarmingBuilding>(_m_resIndex, _m_position, _mono =>
            {
                if (_mono == null)
                {
                    _setLoadDone();
                    return;
                }
                
                _m_mono = _mono;

                if (_m_mono.nameHudTarget != null)
                {
                    _m_nameFollowTarget = new GGUICommonFollowTarget(_m_mono.nameHudTarget, Vector3.zero);
                    GGUIWndBuildingFollow.instance.regInstance(_m_nameFollowTarget);
                }
                if (_m_mono.earningsHudTarget != null)
                {
                    _m_earningsFollowTarget = new GGUICommonFollowTarget(_m_mono.earningsHudTarget, Vector3.zero);
                    GGUIWndBuildingFollow.instance.regInstance(_m_earningsFollowTarget);   
                }
                if (_m_mono.multipleHudTarget != null)
                {
                    _m_multipleFollowTarget = new GGUICommonFollowTarget(_m_mono.multipleHudTarget, Vector3.zero);
                    GGUIWndBuildingFollow.instance.regInstance(_m_multipleFollowTarget);   
                }
                if (_m_mono.clickEarningsHudTarget != null)
                {
                    _m_clickEarningsFollowTarget = new GGUICommonFollowTarget(_m_mono.clickEarningsHudTarget, Vector3.zero);
                    GGUIWndBuildingFollow.instance.regInstance(_m_clickEarningsFollowTarget);
                }
                if (_m_mono.earningClickMono != null)
                    _m_mono.earningClickMono.onClick += _onEarningsClick;
                if (_m_mono.levelUpClickMono != null)
                    _m_mono.levelUpClickMono.onClick += _onLevelUpClick;

                if (_m_nameFollowTarget != null)
                {
                    _m_nameFollower = new GGUIWndFarmingBuildingNameFollowItemController();
                    _m_nameFollower.setBuildingInfo(_m_buildingInfo);
                    _m_nameFollowTarget.addController(_m_nameFollower);
                }

                refreshUpgradeState();
                
                _setLoadDone();
            });
        }
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.IS_BUILDING_UNDER_CONSTRUCTION, _onBuildingUnderConstruction);
            if (_m_mono == null)
                return;
            
            _m_nameFollowTarget?.discard();
            _m_earningsFollowTarget?.discard();
            _m_multipleFollowTarget?.discard();
            _m_nameFollowTarget = null;
            _m_earningsFollowTarget = null;
            _m_multipleFollowTarget = null;
            _m_nameFollower = null;
            
            if (_m_mono.earningClickMono != null)
                _m_mono.earningClickMono.onClick -= _onEarningsClick;
            if (_m_mono.levelUpClickMono != null)
                _m_mono.levelUpClickMono.onClick -= _onLevelUpClick;
            
            MainAdditionBuildingTDScene.instance.discardBuilding<GTDMonoFarmingBuilding>(_m_resIndex, _m_mono);
            _m_mono = null;
            
            _m_initSerialize = ALSerializeOpMgr.next();
        }
        
        
        internal void _onEarningsClick()
        {
            if (_m_clickEarningsFollowTarget == null)
                return;

            int serialize = _m_initSerialize;
            NPPlayer.instance.buildingComp.reqFarmClickOutput(_m_buildingInfo.id, _isSuc =>
            {
                if (!_isSuc)
                    return;
                
                if (serialize != _m_initSerialize || _m_clickEarningsFollowTarget == null)
                    return;

                //上浮暴击tip
                FarmingMultipleInfo multipleInfo = NPPlayer.instance.buildingComp.farmingMultipleInfo;
                long multipleNum = 1;
                if (multipleInfo != null && _m_multipleFollowTarget != null)
                {
                    multipleNum = multipleInfo.addOpCountAndGetMutiple(_m_buildingInfo.id, _m_buildingInfo.level);
                    if (multipleNum > 1)
                    {
                        GGUIWndBuildingEarningMultipleTipFollowItemController multipleTipController = new GGUIWndBuildingEarningMultipleTipFollowItemController(new GResPathIndex(1121));
                        multipleTipController.setMultipleNum(multipleNum);
                        _m_multipleFollowTarget.addController(multipleTipController);
                    }
                }
                else
                {
                    //增加点击次数
                    multipleInfo?.addOpCount();
                }

                //上浮收益tip
                GGUIWndBuildingCoinEarningTipFollowItemController coinEarning = new GGUIWndBuildingCoinEarningTipFollowItemController(new GResPathIndex(1114));
                coinEarning.setCoinNum(_m_buildingInfo.clickEarnings * multipleNum);
                _m_clickEarningsFollowTarget.addController(coinEarning);

                //播放粒子
                if (_m_mono.particleTarget != null)
                {
                    Vector2 screenPos = GCommon.worldPos2UIPos(_m_mono.particleTarget);
                    GCommon.showItemParticle(new NPCommon_ItemInfo((int)ENPItemType.CURRENCY, (long)ECurrency.SILVER, _m_buildingInfo.clickEarnings, null), screenPos, _m_mono.specialCollectParticleId);
                }
                
                _playEarningsEffect(multipleNum);
            });
        }
        //刷新有建筑正在建造的显示状态
        private void _refreshUnderConstructionShow(bool _isUnderConstruction)
        {
            if (_m_mono == null)
                return;

            ALUGUICommon.setGameObjEnable(_m_mono.goBuildBuildingHideList, !_isUnderConstruction);
            ALUGUICommon.setGameObjEnable(_m_mono.goBuildBuildingShowList, _isUnderConstruction);
        }
        private void _onLevelUpClick()
        {
            GGUIWndFarmingBuildingUpgrade.instance.refreshWnd(_m_buildingInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFarmingBuildingUpgrade.instance, GGUIWndFarmingBuildingUpgrade.instance.showWnd);
        }
        private void _playEarningsEffect(long _multipleNum)
        {
            if (_m_mono == null)
                return;

            //播放收集特效
            if (_m_mono.collectEffectTarget != null)
                PlaySfxMgr.instance.playSfxByPos(_m_mono.collectSfxId, _m_mono.collectEffectTarget.position);

            //播放暴击特效
            if (_multipleNum > 1 && _m_mono.multipleEffectTarget != null)
                PlaySfxMgr.instance.playSfxByPos(_m_mono.multipleSfxId, _m_mono.multipleEffectTarget.position);

            //播放收集动画
            if (_m_mono.collectAnimation != null)
                _m_mono.collectAnimation.Play(_m_mono.collectAnimationName);
        }
        public void refreshUpgradeState()
        {
            if (_m_mono == null || _m_buildingInfo.levelData == null)
                return;

            ALUGUICommon.setGameObjEnable(_m_mono.listUpgradableShow, GCommon.isItemEnough(_m_buildingInfo.levelData.upgrade_cost_item, false));
        }

        /// <summary>
        /// 有建筑正在建造事件
        /// </summary>
        /// <param name="_objects"></param>
        private void _onBuildingUnderConstruction(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_mono == null)
                return;

            bool isUnderConstruction = (bool)_objects[0];
            _refreshUnderConstructionShow(isUnderConstruction);
        }
    }
}