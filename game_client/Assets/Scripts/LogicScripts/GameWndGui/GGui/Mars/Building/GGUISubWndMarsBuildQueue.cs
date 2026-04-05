using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsBuildQueue : _ATALBasicUISubWnd<GGUIMonoMarsBuildQueue>
    {
        private GGUISubWndMarsBuildQueueContainer _m_subWndBuildQueueContainer;
        private bool _m_isExpanded;


        public GGUISubWndMarsBuildQueue(GGUIMonoMarsBuildQueue _wnd)
            : base(_wnd)
        {
            _m_isExpanded = false;

            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_subWndBuildQueueContainer?.showWnd();
            
            refreshWnd();

            NPPlayer.instance.playerPropertyMgr.propertyChgDelegate += _onPropertyChgDelegate;
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff += _onBuffChgDelegate;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff -= _onBuffChgDelegate;
            NPPlayer.instance.playerPropertyMgr.propertyChgDelegate += _onPropertyChgDelegate;
            
            _m_subWndBuildQueueContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_subWndBuildQueueContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnShow, _onClickShow);
            ALUGUICommon.uncombineBtnClick(wnd.btnHide, _onClickHide);

            _m_subWndBuildQueueContainer?.discard();
            _m_subWndBuildQueueContainer = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnShow, _onClickShow);
            ALUGUICommon.combineBtnClick(wnd.btnHide, _onClickHide);

            if (wnd.monoBuildQueueContainer != null)
                _m_subWndBuildQueueContainer = new GGUISubWndMarsBuildQueueContainer(wnd.monoBuildQueueContainer);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_subWndBuildQueueContainer?.refreshWnd();
            wnd.anim.Sample(_m_isExpanded ? wnd.showAnimName : wnd.hideAnimName, 1);
        }


        private void _onClickShow(GameObject _go)
        {
            _m_isExpanded = true;
            if (wnd == null)
                return;

            if (wnd.anim == null)
                return;
            
            wnd.anim.ForcePlay(wnd.showAnimName);
        }
        private void _onClickHide(GameObject _go)
        {
            _m_isExpanded = false;
            if (wnd == null)
                return;

            if (wnd.anim == null)
                return;
            
            wnd.anim.ForcePlay(wnd.hideAnimName);
        }
        private void _onPropertyChgDelegate(ENPPlayerPropertyType _type, long _oldValue, long _newValue)
        {
            if (_type == ENPPlayerPropertyType.MARS_BUILDING_QUEUE_NUM)
                refreshWnd();
        }
        private void _onBuffChgDelegate(NPPlayerBuffInfo _buffInfo, int _layer, long _timeMs)
        {
            if (_buffInfo == null)
                return;
            
            if (_buffInfo.buffId == GRefdataCoreMgr.instance.npGeneral.mars_temp_building_queue_gain_buff_item.subId)
                refreshWnd();
        }
    }
}
