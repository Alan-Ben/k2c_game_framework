using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndEveningDungeonFinalAttackRecord : _ANPGGUIBasicWnd<GGUIMonoEveningDungeonFinalAttackRecord>
    {
        private static GGUIWndEveningDungeonFinalAttackRecord _g_instance;
        public static GGUIWndEveningDungeonFinalAttackRecord instance { get { return _g_instance ??= new GGUIWndEveningDungeonFinalAttackRecord(); } }
        
        private GGUIWndEveningDungeonFinalAttackRecordContainer _m_wRecordContainer;//记录列表
        private long _m_lShowSerialize;
        
        public GGUIWndEveningDungeonFinalAttackRecord() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEveningDungeonFinalAttackRecord.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEveningDungeonFinalAttackRecord.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoRecordContainer != null)
                _m_wRecordContainer = new GGUIWndEveningDungeonFinalAttackRecordContainer(wnd.monoRecordContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_wRecordContainer?.discard();
            _m_wRecordContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            _m_wRecordContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRecordContainer?.resetWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            long serizlize = _m_lShowSerialize;
            NPPlayer.instance.eveningDungeonComp.reqEveningDungeonDefeatLog((_msg) =>
            {
                if(wnd == null || serizlize != _m_lShowSerialize)
                    return;

                if (_m_wRecordContainer != null)
                {
                    _m_wRecordContainer.showWnd();
                    _m_wRecordContainer.setData(_msg?.getLogList());
                }
            });
        }
        
        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_FINAL_ATTACK_RECORD);
        }
    }
}