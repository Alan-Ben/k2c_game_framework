using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场贸易站信息弹窗
    /// </summary>
    public class GGUIWndArenaStationInfo : _ANPGGUIBasicWnd<GGUIMonoArenaStationInfo>
    {
        private static GGUIWndArenaStationInfo _g_instance;
        public static GGUIWndArenaStationInfo instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndArenaStationInfo();
                return _g_instance;
            }
        }

        //贸易站信息
        private StationInfo _m_stationInfo;
        //升级消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndArenaStationInfo() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaStationInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaStationInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_stationInfo = NPPlayer.instance.stationComp.stationInfo;
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wCostItem?.discard();
            _m_wCostItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
            ALUGUICommon.uncombineBtnClick(wnd.btnSpeedDetail, _onClickSpeedDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnLimitDetail, _onClickLimitDetail);
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
            ALUGUICommon.combineBtnClick(wnd.btnSpeedDetail, _onClickSpeedDetail);
            ALUGUICommon.combineBtnClick(wnd.btnLimitDetail, _onClickLimitDetail);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            if (_m_stationInfo == null || _m_stationInfo.stationLevelRef == null)
                return;

            NPCommonItem outputItem = GRefdataCoreMgr.instance.npGeneral.arena_station_output_item;
            
            //贸易站下一等级数据
            ArenaStationLevelRefObj nextStationLevelRef = GRefdataCoreMgr.instance.arenaStationLevelRefCore.getRef(_m_stationInfo.level + 1);
            //设置等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_stationInfo.level));
            //设置下一等级
            ALUGUICommon.setLabelTxt(wnd.txtNextLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_stationInfo.level + 1));
            //当前速度
            ALUGUICommon.setLabelTxt(wnd.txtCurCollectSpeed, TextTranslate.instance.getLanguage(TransKeyConst.arena_stationOutputSpeed_num, _m_stationInfo.outputSpeed.ToLargeString(outputItem?.getLargeStringType() ?? PrimitiveExtension.ELargeStringType.DEFAULT)));
            //当前上限时间
            ALUGUICommon.setLabelTxt(wnd.txtCurLimit, TextTranslate.instance.getLanguage(TransKeyConst.time_minute_num, _m_stationInfo.stationLevelRef.storage_limit_sec / 60));

            //是否满级
            if (nextStationLevelRef == null)
            {
                //满级
                ALUGUICommon.setGameObjEnable(wnd.goMaxLevelHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goMaxLevelShowList, true);
            }
            else
            {
                //未满级
                ALUGUICommon.setGameObjEnable(wnd.goMaxLevelShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goMaxLevelHideList, true);

                //下一等级速度
                long nextSpeed = (long) Math.Ceiling(NPPlayer.instance.specialItemComp.goldData.earnings * (nextStationLevelRef.harvest_ratio / 10000f));
                ALUGUICommon.setLabelTxt(wnd.txtNextCollectSpeed, TextTranslate.instance.getLanguage(TransKeyConst.arena_stationOutputSpeed_num, nextSpeed.ToLargeString(outputItem?.getLargeStringType() ?? PrimitiveExtension.ELargeStringType.DEFAULT)));
                //下一等级上限时间
                ALUGUICommon.setLabelTxt(wnd.txtNextLimit, TextTranslate.instance.getLanguage(TransKeyConst.time_minute_num, nextStationLevelRef.storage_limit_sec / 60));
                //升级消耗道具
                if (_m_wCostItem != null)
                {
                    _m_wCostItem.showWnd();
                    _m_wCostItem.setItem(_m_stationInfo.stationLevelRef.upgrade_cost);
                }
            }

            //设置显隐
            bool havePermissions = NPPlayer.instance.playerPermissionsComp.checkHavePermissions(GRefdataCoreMgr.instance.npGeneral.arena_station_privilege_permissions_id);
            ALUGUICommon.setGameObjEnable(wnd.goHavePrivilegeHideList, !havePermissions);
            ALUGUICommon.setGameObjEnable(wnd.goHavePrivilegeShowList, havePermissions);
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_STATION_INFO);
        }

        //点击升级
        private void _onClickUpgrade(GameObject _go)
        {
            if(_m_stationInfo == null || _m_stationInfo.stationLevelRef == null)
                return;

            //道具是否充足
            if (!GCommon.isItemEnough(_m_stationInfo.stationLevelRef.upgrade_cost, true))
                return;

            //是否有产出资源还未收集（有结算资源 或 产出时间超过一轮）
            if (_m_stationInfo.hadOutputNum > 0 || 
                (_m_stationInfo.getCurOutputTimeSec() > GRefdataCoreMgr.instance.npGeneral.arena_collection_resource_interval_sec && _m_stationInfo.getCurOutputSilverCount() > 0))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_stationUpgradeNeedGetResFirst_none);
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_STATION_INFO);
                return;
            }

            //请求贸易站升级
            long serialize = _m_lShowSerialize;
            NPPlayer.instance.stationComp.reqArenaStationUpgrade(() =>
            {
                if (_m_lShowSerialize != serialize)
                    return;

                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_STATION_INFO);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaStationUpgradeSuc.instance, GGUIWndArenaStationUpgradeSuc.instance.showWnd, UINodeTagConst.C_ARENA_STATION_UPGRADE_SUC);
            });
        }

        //点击收益详情
        private void _onClickSpeedDetail(GameObject _go)
        {
            if(_go == null || wnd == null || _m_stationInfo == null || _m_stationInfo.stationLevelRef == null)
                return;

            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text_Text(5218,
                TextTranslate.instance.getLanguage(TransKeyConst.common_earningSpeed_num, NPPlayer.instance.specialItemComp.goldData.earnings.ToLargeString(GRefdataCoreMgr.instance.npGeneral.arena_station_output_item?.getLargeStringType() ?? PrimitiveExtension.ELargeStringType.DEFAULT)),
                TextTranslate.instance.getLanguage(TransKeyConst.arena_stationHarvestRatio_num, _m_stationInfo.stationLevelRef.harvest_ratio / 10000f),
                (RectTransform)_go.transform, wnd.speedTipInterval));
        }

        //点击上限详情
        private void _onClickLimitDetail(GameObject _go)
        {
            if (_go == null || wnd == null)
                return;

            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                TextTranslate.instance.getLanguage(TransKeyConst.arena_stationRemoveLimitTimeDesc_none),
                (RectTransform)_go.transform, wnd.limitTipIntervalX, wnd.limitTipIntervalY));
        }

        #endregion
    }
}