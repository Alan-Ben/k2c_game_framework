using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 收集成就
    /// </summary>
    public class GGUIWndTreasureHuntCollectAchieve : _ANPGGUIBasicResBarWnd<GGUIMonoTreasureHuntCollectAchieve>
    {
        private static GGUIWndTreasureHuntCollectAchieve _g_instance;
        public static GGUIWndTreasureHuntCollectAchieve instance { get { return _g_instance ??= new GGUIWndTreasureHuntCollectAchieve(); } }
        
        private GGUISubWndCommonRewardShow _m_wAchievePointRewardShow;//成就点奖励展示
        private NPGGUIWndProgress _m_wAchievePointProgress;//成就点进度条
        private GGUIWndAchieveStepGrid _m_wAchieveStepGrid;//成就步骤列表

        private List<AchieveInfo> _m_lAchieveInfoList;//成就列表
        private List<AchieveStepInfo> _m_lAchieveStepInfoList;//成就步骤列表
        
        private AchievePointRefObj _m_rAchievePointRefObj;//成就点配表数据
        private AchievePointStepRefObj _m_rCurAchievePointStepRef;//当前成就点阶段数据

        public GGUIWndTreasureHuntCollectAchieve() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntCollectAchieve.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntCollectAchieve.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public override bool needDiscardOnSwitch { get { return true; } }
        public override bool showResBarBySelf { get { return true; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoAchievePointRewardShow != null)
                _m_wAchievePointRewardShow = new GGUISubWndCommonRewardShow(wnd.monoAchievePointRewardShow);

            if(wnd.monoAchievePointProgress != null)
                _m_wAchievePointProgress = new NPGGUIWndProgress(wnd.monoAchievePointProgress);
            
            if (wnd.monoAchieveStepGrid != null)
                _m_wAchieveStepGrid = new GGUIWndAchieveStepGrid(wnd.monoAchieveStepGrid);
            
            // 获取成就步骤数据列表
            if (_m_lAchieveStepInfoList == null)
                _m_lAchieveStepInfoList = new List<AchieveStepInfo>();
            _m_lAchieveStepInfoList.Clear();
            _m_lAchieveInfoList = NPPlayer.instance.achieveComp.getInfosByType(EAchieveType.TREASURE_HUNT);
            if (_m_lAchieveInfoList != null)
            {
                foreach (var achieveInfo in _m_lAchieveInfoList)
                {
                    if(achieveInfo != null && achieveInfo.stepInfoList != null)
                        _m_lAchieveStepInfoList.AddRange(achieveInfo.stepInfoList);
                }
            }

            // 获取成就点配表数据
            _m_rAchievePointRefObj = GRefdataCoreMgr.instance.achievePointRefCore.getRef((long) EAchieveType.TREASURE_HUNT);
            
            ALUGUICommon.combineBtnClick(wnd.btnDrawAchievePointReward, _onDrawAchievePointBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDrawAchievePointReward, _onDrawAchievePointBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
            
            _m_wAchievePointRewardShow?.discard();
            _m_wAchievePointRewardShow = null;
            
            _m_wAchievePointProgress?.discard();
            _m_wAchievePointProgress = null;
            
            _m_wAchieveStepGrid?.discard();
            _m_wAchieveStepGrid = null;
            
            _m_lAchieveInfoList?.Clear();
            _m_lAchieveInfoList = null;
            _m_lAchieveStepInfoList?.Clear();
            _m_lAchieveStepInfoList = null;

            _m_rAchievePointRefObj = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, _onAchieveInfoChg);
            WinMsg.RegisterMsg(WinMsgType.ON_ACHIEVE_POINT_CHG, _onAchievePointChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, _onAchieveInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_ACHIEVE_POINT_CHG, _onAchievePointChg);
            
            _m_wAchievePointRewardShow?.hideWnd();
            _m_wAchievePointProgress?.hideWnd();
            _m_wAchieveStepGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wAchievePointRewardShow?.resetWnd();
            _m_wAchievePointProgress?.resetWnd();
            _m_wAchieveStepGrid?.resetWnd();
        }
        
        private void _refreshWnd()
        {
            _refreshAchievePoint();//刷新成就点
            _refreshAchieveStepGrid();//刷新成就步骤列表
        }
        
        /// <summary>
        /// 刷新成就点
        /// </summary>
        private void _refreshAchievePoint()
        {
            if(wnd == null)
                return;
            
            // 获取当前所处成就点阶段
            _m_rCurAchievePointStepRef = NPPlayer.instance.achieveComp.getCurAchievePointOrLastStepRef(EAchieveType.TREASURE_HUNT);

            // 设置当前成就点阶段的奖励
            if (_m_wAchievePointRewardShow != null)
            {
                NPCommonCostItem needShowRewardItem = null;
                if (_m_rCurAchievePointStepRef != null && _m_rCurAchievePointStepRef.reward_item_list != null)
                {
                    foreach (var rewardItem in _m_rCurAchievePointStepRef.reward_item_list)
                    {
                        if (rewardItem != null && rewardItem.getItemType() == ENPItemType.PLAYER_SKIN)
                        {
                            needShowRewardItem = rewardItem;
                            break;
                        }
                    }
                }

                if (needShowRewardItem == null)
                {
                    _m_wAchievePointRewardShow.hideWnd();
                }
                else
                {
                    _m_wAchievePointRewardShow.showWnd();
                    _m_wAchievePointRewardShow.setShowReward(needShowRewardItem);
                }
            }

            ALUGUICommon.setLabelTxt(wnd.txtAchievePointName, GCommon.getItemName(ENPItemType.ACHIEVE_POINT, (long) EAchieveType.TREASURE_HUNT));
            ALUGUICommon.setLabelTxt(wnd.txtAchievePointDesc, GCommon.getItemDesc(ENPItemType.ACHIEVE_POINT, (long) EAchieveType.TREASURE_HUNT));

            // 设置成就点阶段进度条
            if (_m_wAchievePointProgress != null)
            {
                if (_m_rCurAchievePointStepRef == null)
                {
                    _m_wAchievePointProgress.hideWnd();
                }
                else
                {
                    _m_wAchievePointProgress.showWnd();
                    _m_wAchievePointProgress.setProgress(GCommon.getItemCount(_m_rCurAchievePointStepRef?.need_point?.item)
                        , _m_rCurAchievePointStepRef?.need_point?.count ?? 0, EValueFormatType.NORMAL);
                }
            }
            
            ENPCommonGetStat getStat = NPPlayer.instance.achieveComp.achievePointStepGetStat(_m_rCurAchievePointStepRef);
            NPCommonEnumStatInfo<ENPCommonGetStat>.setStat(wnd.achievePointRewardGetStateShow, getStat);
        }
        
        /// <summary>
        /// 刷新成就步骤列表
        /// </summary>
        private void _refreshAchieveStepGrid()
        {
            if(_m_wAchieveStepGrid == null)
                return;
            
            _m_wAchieveStepGrid.showWnd();
            _m_wAchieveStepGrid.setShowData(_m_lAchieveStepInfoList);
        }

        /// <summary>
        /// 移动成就步骤列表到第一个可领奖 或 第一个未达到的步骤
        /// </summary>
        public void moveAchieveStepGrid()
        {
            _m_wAchieveStepGrid?.moveToFirstCanDrawOrFirstUnReachedStep();
        }

        #region 按钮事件

        /// <summary>
        /// 点击领取成就点奖励按钮
        /// </summary>
        private void _onDrawAchievePointBtnClick(GameObject _go)
        {
            if (_m_rCurAchievePointStepRef == null)
                return;

            ENPCommonGetStat getStat = NPPlayer.instance.achieveComp.achievePointStepGetStat(_m_rCurAchievePointStepRef);
            if(getStat != ENPCommonGetStat.CAN_GET)
                return;
            
            // 领取成就点奖励
            NPPlayer.instance.achieveComp.reqGainAchievePointReward(_m_rCurAchievePointStepRef.id, () =>
            {
                _refreshAchievePoint();
            });
        }

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_COLLECT_ACHIEVE);
        }
        
        #endregion
        
        #region WinMsg监听

        //成就信息变更
        private void _onAchieveInfoChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_lAchieveInfoList == null)
                return;

            AchieveInfo info = (AchieveInfo) _objects[0];
            if(info == null)
                return;
            
            foreach (var item in _m_lAchieveInfoList)
            {
                if (item != null && item.achieveId == info.achieveId)
                {
                    _refreshAchieveStepGrid();
                    moveAchieveStepGrid();
                    return;
                }
            }
        }

        /// <summary>
        /// 成就点数变化
        /// </summary>
        /// <param name="_objs"></param>
        private void _onAchievePointChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || 
                !(_objs[0] is int _achieveType) || _achieveType != (int) EAchieveType.TREASURE_HUNT)
                return;

            // 刷新成就点
            _refreshAchievePoint();
        }

        #endregion
    }
}