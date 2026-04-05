using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 屏蔽列表
    /// </summary>
    public class GGUIWndFriendShieldList : _ANPGGUIBasicWnd<GGUIMonoFriendShieldList>
    {
        private static GGUIWndFriendShieldList _g_instance;
        public static GGUIWndFriendShieldList instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndFriendShieldList();
                return _g_instance;
            }
        }
        
        private GGUIWndFriendShieldGrid _m_shieldGrid;
        
        public GGUIWndFriendShieldList() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoFriendShieldList.assetPath; }
        protected override string _monoObjName { get => GGUIMonoFriendShieldList.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_SHIELD_CHG, _refreshWnd);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_SHIELD_CHG, _refreshWnd);
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            _m_shieldGrid?.discard();
            _m_shieldGrid = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
            if (null != wnd.friendListGrid)
            {
                _m_shieldGrid = new GGUIWndFriendShieldGrid(wnd.friendListGrid);
            }
        }

        private void _clickClose(GameObject obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            List<PlayerShieldItem> list = new List<PlayerShieldItem>();
            NPPlayer.instance.friendsComp.getShieldList(list);
            _m_shieldGrid?.showWnd();
            _m_shieldGrid?.showItemList(list);

            ALUGUICommon.setLabelTxt(wnd.txtShieldCount, TextTranslate.instance.getLanguage(TransKeyConst.friend_shield_count, list.Count,
                GRefdataCoreMgr.instance.npGeneral.shield_cid_limit));
        }
    }
}