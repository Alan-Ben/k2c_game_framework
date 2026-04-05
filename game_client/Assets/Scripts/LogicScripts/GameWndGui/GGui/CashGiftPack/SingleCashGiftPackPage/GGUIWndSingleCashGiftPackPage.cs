using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 现金礼包主页面
    /// </summary>
    public class GGUIWndSingleCashGiftPackPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoSingleCashGiftPackPage>
    {
        //资源id
        private long _m_lUIResId;
        //礼包id
        private long _m_lGiftPackId;

        private GGUIWndCashGiftPackGroupGoodsContainerItem _m_wGiftPackWnd;

        public GGUIWndSingleCashGiftPackPage(long _uiResId, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            _refreshWnd();
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }
        
        protected override void _onReset()
        {
        }
        
        protected override void _onDiscard()
        {
            _m_wGiftPackWnd?.discard();
            _m_wGiftPackWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.cashGiftItem != null)
            {
                _m_wGiftPackWnd = new GGUIWndCashGiftPackGroupGoodsContainerItem(wnd.cashGiftItem);
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_giftPackGroupRefLsit"></param>
        public void setInfo(long _giftPackId)
        {
            _m_lGiftPackId = _giftPackId;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null )
                return;
            _m_wGiftPackWnd?.setInfo(_m_lGiftPackId);
        }

        
        //活动状态变化
        private void _onActivityStateChg(params object[] _objects)
        {
            _refreshWnd();
        }
    }
}
