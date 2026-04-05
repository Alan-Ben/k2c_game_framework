using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 竞技场贸易站升级成功弹窗
    /// </summary>
    public class GGUIWndArenaStationUpgradeSuc : _ANPGGUIBasicWnd<GGUIMonoArenaStationUpgradeSuc>
    {
        private static GGUIWndArenaStationUpgradeSuc _g_instance;
        public static GGUIWndArenaStationUpgradeSuc instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndArenaStationUpgradeSuc();
                return _g_instance;
            }
        }

        public GGUIWndArenaStationUpgradeSuc() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaStationUpgradeSuc.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaStationUpgradeSuc.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            StationInfo arenaStationInfo = NPPlayer.instance.stationComp.stationInfo;
            if (arenaStationInfo == null || arenaStationInfo.stationLevelRef == null)
                return;

            ArenaStationLevelRefObj lastLevelRef = GRefdataCoreMgr.instance.arenaStationLevelRefCore.getRef(arenaStationInfo.level - 1);
            if (lastLevelRef == null)
                return;

            NPCommonItem outputItem = GRefdataCoreMgr.instance.npGeneral.arena_station_output_item;

            //设置等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level2_num, arenaStationInfo.level));
            //上个收益速度
            long lastSpeed = (long) Math.Ceiling(NPPlayer.instance.specialItemComp.goldData.earnings * (lastLevelRef.harvest_ratio / 10000f));
            ALUGUICommon.setLabelTxt(wnd.txtLastSpeed, TextTranslate.instance.getLanguage(TransKeyConst.arena_stationOutputSpeed_num, lastSpeed.ToLargeString(outputItem?.getLargeStringType() ?? PrimitiveExtension.ELargeStringType.DEFAULT)));
            //当前收益速度
            ALUGUICommon.setLabelTxt(wnd.txtCurSpeed, TextTranslate.instance.getLanguage(TransKeyConst.arena_stationOutputSpeed_num, arenaStationInfo.outputSpeed.ToLargeString(outputItem?.getLargeStringType() ?? PrimitiveExtension.ELargeStringType.DEFAULT)));
            //上个上限
            ALUGUICommon.setLabelTxt(wnd.txtLastLimit, TextTranslate.instance.getLanguage(TransKeyConst.time_minute_num, lastLevelRef.storage_limit_sec / 60));
            //当前上限
            ALUGUICommon.setLabelTxt(wnd.txtCurLimit, TextTranslate.instance.getLanguage(TransKeyConst.time_minute_num, arenaStationInfo.stationLevelRef.storage_limit_sec / 60));

            //设置显隐
            bool havePermissions = NPPlayer.instance.playerPermissionsComp.checkHavePermissions(GRefdataCoreMgr.instance.npGeneral.arena_station_privilege_permissions_id);
            ALUGUICommon.setGameObjEnable(wnd.goHavePrivilegeHideList, !havePermissions);
            ALUGUICommon.setGameObjEnable(wnd.goHavePrivilegeShowList, havePermissions);
        }
    }
}