using System;
using ALPackage;
using GC2GS.p023_ArenaOp;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p023_ArenaOp;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 贸易站组件
    /// </summary>
    public class StationComponent : _ANPBasicPlayerComponent
    {
        //贸易站信息
        private StationInfo _m_stationInfo;
        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;

        //构造函数
        public StationComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.PLAYER_PERMISSIONS, ENPPlayerCompType.BASIC_INFO };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.STATION; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 贸易站信息
        /// </summary>
        public StationInfo stationInfo { get { return _m_stationInfo; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqStationInit();
        }

        protected override void _dealInit()
        {
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();
            _refreshStationUpgradeRedTip();
            _refreshStationRedTip();
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            _m_tcTickTaskController.setDisable();
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshStationRedTip, 1f);
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("StationComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            _clear();
        }

        //析构函数
        private void _clear()
        {
            _m_stationInfo = null;
            _m_tcTickTaskController.setDisable();
        }

        //刷新贸易站红点
        private void _refreshStationRedTip()
        {
            if (_m_stationInfo == null || _m_stationInfo.level <= 0)
            {
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ARENA_STATION, 0);
                return;
            }

            //红点计数
            long redTipCount = 0;

            //是否有无上限权限
            bool havePermissions = NPPlayer.instance.playerPermissionsComp.checkHavePermissions(GRefdataCoreMgr.instance.npGeneral.arena_station_privilege_permissions_id);
            if (havePermissions)
            {
                //如果有权限，则每日只展示一次红点
                if(AccountSettingMgr.instance.dailyTagSaver != null)
                    redTipCount = AccountSettingMgr.instance.dailyTagSaver.isNewDay(DailyTagConst.ARENA_STATION_COLLECTION) ? 1 : 0;
            }
            else
            {
                //没权限按照存储上限判断，达到上限则显示红点
                long limitTimeSec = _m_stationInfo.stationLevelRef != null ? _m_stationInfo.stationLevelRef.storage_limit_sec : 0;
                long curOutputSec = _m_stationInfo.getCurOutputTimeSec();
                redTipCount = curOutputSec >= limitTimeSec ? 1 : 0;
            }

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ARENA_STATION, redTipCount);
        }

        //刷新贸易站升级红点
        private void _refreshStationUpgradeRedTip()
        {
            long canUpgradeCount = 0;
            if (_m_stationInfo != null && 
                _m_stationInfo.stationLevelRef != null && 
                _m_stationInfo.stationLevelRef.upgrade_cost != null &&
                _m_stationInfo.stationLevelRef.upgrade_cost.getItemType() != ENPItemType.NONE &&
                GCommon.isItemEnough(_m_stationInfo.stationLevelRef.upgrade_cost, false))
                canUpgradeCount = 1;

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ARENA_STATION_UPGRADE, canUpgradeCount);
        }

        //背包物品变更
        private void _onBagItemChg(params object[] _objects)
        {
            _refreshStationUpgradeRedTip();
        }

        #region S2C

        /// <summary>
        /// 贸易站初始化
        /// </summary>
        /// <param name="_msg"></param>
        public void retStationInit(GS2GC_002_031_RetPlayerStationInit _msg)
        {
            if (_msg == null)
                return;

            _m_stationInfo = new StationInfo(_msg.getStationInfo());
            setInitDone();
        }

        /// <summary>
        /// 贸易站数据变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onStationInfoChg(GS2GC_004_057_OnStationInfoChg _msg)
        {
            if(_msg == null)
                return;
            
            if (_m_stationInfo == null)
                _m_stationInfo = new StationInfo(_msg.getStationInfo());
            else
                _m_stationInfo.updateInfo(_msg.getStationInfo());

            //刷新贸易站红点
            _refreshStationRedTip();
            _refreshStationUpgradeRedTip();
            //贸易站信息变更
            WinMsg.SendMsg(WinMsgType.ON_STATION_CHG);
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求贸易站初始化
        /// </summary>
        public void reqStationInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_031_ReqPlayerStationInit());
        }

        /// <summary>
        /// 请求贸易站升级
        /// </summary>
        /// <param name="_callback"></param>
        public void reqArenaStationUpgrade(Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_008_ReqArenaStationUpgrade(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_023_008_RetArenaStationUpgrade>((_msg) => _callback?.Invoke()));
        }

        /// <summary>
        /// 请求贸易站收集
        /// </summary>
        /// <param name="_callback"></param>
        public void reqArenaStationCollect(Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_009_ReqArenaStationCollect(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_023_009_RetArenaStationCollect>((_msg) => _callback?.Invoke()));
        }

        #endregion

    }
}
