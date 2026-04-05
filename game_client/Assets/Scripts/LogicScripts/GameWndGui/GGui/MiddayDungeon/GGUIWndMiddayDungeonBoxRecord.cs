using System.Collections.Generic;
using ALPackage;
using Common.DungeonObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 午间副本宝箱领取详情
    /// </summary>
    public class GGUIWndMiddayDungeonBoxRecord : _ATALBasicUIWnd<GGUIMonoMiddayDungeonBoxRecord>
    {
        private static GGUIWndMiddayDungeonBoxRecord _g_instance = new GGUIWndMiddayDungeonBoxRecord();
    
        public static GGUIWndMiddayDungeonBoxRecord instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndMiddayDungeonBoxRecord();
                return _g_instance;
            }
        }
        
        private GGUIWndMiddayDungeonBoxRecordGrid _m_itemGridWnd;
        private List<MiddayDungeonBoxRecord> _m_itemDataList = new List<MiddayDungeonBoxRecord>();
        private long _m_boxDbId;
        private MiddayDungeonBoxRefObj _m_rBoxRefObj;

        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        public GGUIWndMiddayDungeonBoxRecord() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoMiddayDungeonBoxRecord.assetPath; }
        protected override string _monoObjName { get => GGUIMonoMiddayDungeonBoxRecord.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
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
            _m_rBoxRefObj = null;
            
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemGrid != null)
                _m_itemGridWnd = new GGUIWndMiddayDungeonBoxRecordGrid(wnd.itemGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            _m_itemGridWnd?.showWnd();
            _m_itemGridWnd?.showItemList(_m_itemDataList);
            if (_m_rBoxRefObj != null)
            {
                NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(_m_rBoxRefObj.draw_box_fixed_cd_id);
                if (fixedCdInfo != null)
                    ALUGUICommon.setLabelTxt(wnd.txtCount, TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_player_can_draw_count, fixedCdInfo.getCount(), fixedCdInfo.getMaxCount()));
            }
        }

        public void setInfo(long _boxId, long _boxDbId)
        {
            _m_boxDbId = _boxDbId;
            NPPlayer.instance.middayDungeonComp.reqMiddayDungeonBoxDrawRecord(_m_boxDbId, (_suc, _recordList) =>
            {
                if (!_suc)
                    return;
                _m_itemDataList.Clear();
                _m_rBoxRefObj = GRefdataCoreMgr.instance.middayDungeonBoxRefCore.getRef(_boxId);
                NPCommonCostItem rewardItem = null;
                if (_m_rBoxRefObj != null && _m_rBoxRefObj.item_list != null && _m_rBoxRefObj.item_list.Count > 0) rewardItem = _m_rBoxRefObj.item_list[0];
                if (_recordList != null)
                    foreach (var record in _recordList)
                    {
                        if (record != null)
                            _m_itemDataList.Add(new MiddayDungeonBoxRecord(record.getDrawTimeMs(), record.getCid(), rewardItem));
                    }
                _refreshWnd();
            });
        }
        // <AutoGen:Method>
        
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MIDDAY_DUNGEON_BOX_RECORD);
        }
        // </AutoGen:Method>
    }
}