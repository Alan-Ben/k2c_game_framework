using System;
using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 分享-好友页签
    /// </summary>
    public class NPGGUIWndCommonShareFriendPage : _ATALBasicLoadPrefabSubUIWnd<NPGGUIMonoCommonShareFriendPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        private NPGGUIWndCommonShareContainer _m_wItemContainer;//分享列表
        private Action<long,Action> _m_shareAction;
        private Func<long, bool> _m_isOnCDFunc;
        private ENPShareType _m_shareType;

        public NPGGUIWndCommonShareFriendPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_shareAction = null;
            _m_isOnCDFunc = null;

            _m_wItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_shareAction = null;
            _m_isOnCDFunc = null;

            _m_wItemContainer?.discard();
            _m_wItemContainer = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnOneKeyShare, _onClickBtnOneKeyShare);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //分享列表
            if (wnd.monoItemContainer != null)
            {
                _m_wItemContainer = new NPGGUIWndCommonShareContainer(wnd.monoItemContainer);
            }

            ALUGUICommon.combineBtnClick(wnd.btnOneKeyShare, _onClickBtnOneKeyShare);
        }


        #region 点击事件

        /// <summary>
        /// 点击一键分享按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnOneKeyShare(GameObject _go)
        {
            //item子窗体内部判断CD
            bool sucFlag = false;
            _m_wItemContainer?.refreshAllItem((_itemWnd) =>
            {
                sucFlag = _itemWnd.tryDealShare() || sucFlag;
            });
            // if (sucFlag)
            //     NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_share_suc_tip_none));
            // else
            //     NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_oneKey_share_cd_tip_none));
        }

        #endregion


        #region 窗体事件

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refreshWnd()
        {
            //好友分享列表
            List<PlayerFriendItemData> friendList = new List<PlayerFriendItemData>();
            NPPlayer.instance.friendsComp.getFriendsDataList((_itemDataList)=> {
                friendList.AddRange(_itemDataList);
                List<long> playerCidList = new List<long>();
                for (int i = 0; i < friendList.Count; i++)
                {
                    playerCidList.Add(friendList[i].cid);
                }
                _m_wItemContainer?.showItemList(_m_shareType, playerCidList, _m_shareAction, _m_isOnCDFunc);
            });

        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_shareAction"></param>
        /// <param name="_isOnCDFunc"></param>
        public void setInfo(ENPShareType _type, Action<long,Action> _shareAction,Func<long,bool> _isOnCDFunc)
        {
            _m_shareType = _type;
            _m_shareAction = _shareAction;
            _m_isOnCDFunc = _isOnCDFunc;
            _refreshWnd();
        }

        #endregion
    }
}
