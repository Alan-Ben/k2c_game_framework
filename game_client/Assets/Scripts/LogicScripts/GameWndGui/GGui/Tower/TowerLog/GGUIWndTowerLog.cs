using System.Collections.Generic;
using ALPackage;
using Common.TowerObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 爬塔战报
    /// </summary>
    public class GGUIWndTowerLog : _ATALBasicUIWnd<GGUIMonoTowerLog>
    {
        private static GGUIWndTowerLog _g_instance = new GGUIWndTowerLog();

        public static GGUIWndTowerLog instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerLog();
                return _g_instance;
            }
        }

        public GGUIWndTowerLog() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTowerLog.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerLog.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;

        private GGUIWndTowerLogItemGrid _m_logGrid;
        private List<TowerBattleLog> _m_itemDataList = new List<TowerBattleLog>();

        private long _m_serialzeOp;
    
        protected override void _onShowWnd()
        {
            _m_serialzeOp = ALSerializeOpMgr.next();
            long serialzeOp = _m_serialzeOp;
            NPPlayer.instance.towerComp.reqTowerReportList(_reportInfos =>
            {
                if(serialzeOp != _m_serialzeOp)
                    return; //如果序列化操作不一致，说明已经被重置了，不需要处理
                // 反向排序
                List<TowerBattleLog> logList = new List<TowerBattleLog>();
                for (var i = _reportInfos.Count - 1; i >= 0; i--)
                {
                    Tower_ReportInfo reportInfo = _reportInfos[i];
                    logList.Add(new TowerBattleLog(reportInfo));
                }

                _m_itemDataList = logList;
                _refreshWnd();
            });
        }

        protected override void _onHideWnd()
        {
            _m_serialzeOp = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        
        }

        protected override void _onDiscard()
        {
            _m_logGrid?.discard();
            _m_logGrid = null;
            if (wnd != null) 
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.itemGrid != null)
                _m_logGrid = new GGUIWndTowerLogItemGrid(wnd.itemGrid);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);

        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            _m_logGrid?.showWnd();
            _m_logGrid?.showItemList(_m_itemDataList);
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.emptyShowGos, _m_itemDataList?.Count == 0);
        }
        
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_LOG);
        }
    }
}