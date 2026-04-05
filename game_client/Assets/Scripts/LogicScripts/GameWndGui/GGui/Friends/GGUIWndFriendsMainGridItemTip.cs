using System;
using ALPackage;
using Common.NpChatObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 好友列表额外按钮tip
    /// </summary>
    public class GGUIWndFriendsMainGridItemTip : _ATNPGGUIWndCommonItemToolTip<GGUIMonoFriendsMainGridItemTip>
    {
        // 单例
        private static GGUIWndFriendsMainGridItemTip _m_gInstance;
        public static GGUIWndFriendsMainGridItemTip instance
        {
            get
            {
                if (_m_gInstance == null)
                    _m_gInstance = new GGUIWndFriendsMainGridItemTip();
                return _m_gInstance;
            }
        }

        //好友数据
        private PlayerFriendItemData _m_itemData;

        public GGUIWndFriendsMainGridItemTip() : base(GGUIMonoFriendsMainGridItemTip.assetPath, GGUIMonoFriendsMainGridItemTip.objName)
        {
        }

        protected override string _monoAssetPath => GGUIMonoFriendsMainGridItemTip.assetPath;
        protected override string _monoObjName => GGUIMonoFriendsMainGridItemTip.objName;
        protected override _AALResourceCore _resourceCore => GameResCore.instance;


        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            ALUGUICommon.uncombineBtnClick(wnd.deleteBtn, _deleteBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.blockBtn, _blockBtnDidClick);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            base._onWndInitDone();

            ALUGUICommon.combineBtnClick(wnd.deleteBtn, _deleteBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.blockBtn, _blockBtnDidClick);
        }
        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Friends.C_ADD_FRIEND_BTNS_TIP);
        }

        /// <summary>
        /// 显示好友按钮tip
        /// </summary>
        public void showWnd(PlayerFriendItemData _itemData, RectTransform _targetTransRoot, float _interval)
        {
            _m_itemData = _itemData;
            base.showWnd();
            setPos(_targetTransRoot, _interval);
        }

        //删除好友
        private void _removeFriend()
        {
            if (null == _m_itemData || null == _m_itemData.playerInfo)
                return;

            //提示删除
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.friends_sure_delete_str, _m_itemData.playerInfo.name))
                  , TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                  , null
                  , TextTranslate.instance.getLanguage(TransKeyConst.confirm)
                  , () =>
                  {
                      NPPlayer.instance.friendsComp.reqRemoveFriend(_m_itemData.playerInfo.cid, () =>
                      {
                          NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_delete_suc_none);
                      });
                  });
        }

        private void _deleteBtnDidClick(GameObject _go)
        {
            _removeFriend();
        }

        private void _blockBtnDidClick(GameObject _go)
        {
            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_sysUnOpen_none);
        }

    }
}