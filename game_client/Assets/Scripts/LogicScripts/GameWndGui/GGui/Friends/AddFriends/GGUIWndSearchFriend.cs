using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;
using GS2GC.p004_PlayerOp;
using Common.NpPlayerInfoObj;

namespace GOE
{
    // 好友搜索界面
    public class GGUIWndSearchFriend : _ANPGGUIBasicWnd<GGUIMonoSearchFriend>
    {

        private static GGUIWndSearchFriend _g_instance = new GGUIWndSearchFriend();
        public static GGUIWndSearchFriend instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndSearchFriend();
                return _g_instance;
            }
        }

        //玩家信息
        private NPGGUIWndPlayerIcon _m_playerIconWnd;

        //玩家数据
        private NPCommonSimplePlayerInfo _m_playerInfo;

        //当前搜索状态
        private ENPFriendsSearchResultTabType _m_resultType;

        //当前搜索到的用户cid 
        private long _m_resultCid;

        //已发过申请的玩家列表
        private List<long> _m_isSendIdList;

        protected GGUIWndSearchFriend()
           : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoSearchFriend.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSearchFriend.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        #region override
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.playerIconMono)
                _m_playerIconWnd = new NPGGUIWndPlayerIcon(wnd.playerIconMono);

            _m_isSendIdList = new List<long>();

            _m_resultType = ENPFriendsSearchResultTabType.NONE;
        }
        protected override void _onShowWnd()
        {
            _refresh();

            if (null != _m_playerIconWnd)
                _m_playerIconWnd.showWnd();

            ALUGUICommon.combineBtnClick(wnd.reportBtn, _reportBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.searchBtn, _searchBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.requestBtn, _requestBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);

        }

        protected override void _onHideWnd()
        {
            ALUGUICommon.uncombineBtnClick(wnd.reportBtn, _reportBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.searchBtn, _searchBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.requestBtn, _requestBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _closeBtnDidClick);

        }

        protected override void _onReset()
        {
            if (null != _m_playerIconWnd)
                _m_playerIconWnd.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_playerIconWnd)
                _m_playerIconWnd.discard();
            _m_playerIconWnd = null;

            _m_resultType = ENPFriendsSearchResultTabType.NONE;

            _m_isSendIdList.Clear();
            _m_isSendIdList = null;
        }

        #endregion

        public void _refresh()
        {
            if (null == wnd)
                return;

            //设置显隐
            GGUIFriendsSearchResultTabMono temp = null;
            for (int i = 0; i < wnd.resultShowGoList.Count; i++)
            {
                temp = wnd.resultShowGoList[i];
                if (null == temp)
                    continue;

                ALUGUICommon.setGameObjEnable(temp.goList, _m_resultType == temp.tabType);
            }

            if (_m_resultType != ENPFriendsSearchResultTabType.HASCID || null == _m_playerInfo)
                return;

            //设置玩家信息
            if (null != _m_playerIconWnd)
            {
                _m_playerIconWnd.setPlayerInfo(_m_playerInfo);
            }
        }

        /// <summary>
        /// 搜索好友
        /// </summary>
        private void _search()
        {
            string input = wnd.cidInputField.text;
            if (input.Length == 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_inputId_none);
                return;
            }

            long cid = 0;
            if (!long.TryParse(input, out cid))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_inputIdError_none);
                return;
            }

            //请求玩家详情
            GCommon.reqPlayerInfoSer(cid, (_info) =>
            {
                if (null == wnd || null == _info.getSomeOneShowInfo() || _info.getSomeOneShowInfo().getCid() != cid)
                    return;

                _m_resultCid = cid;

                if (null == _m_playerInfo)
                    _m_playerInfo = new NPCommonSimplePlayerInfo(_info.getSomeOneShowInfo());
                else
                    _m_playerInfo.update(_info.getSomeOneShowInfo());

                if (null == _m_playerInfo || _m_playerInfo.cid == 0)
                    _m_resultType = ENPFriendsSearchResultTabType.NOCID;
                else
                    _m_resultType = ENPFriendsSearchResultTabType.HASCID;

                _refresh();
            });
        }

        #region 点击事件

        /// <summary>
        /// 举报用户
        /// </summary>
        private void _reportBtnDidClick(GameObject _go)
        {
            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_sysUnOpen_none);
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="_go"></param>
        private void _searchBtnDidClick(GameObject _go)
        {
            _search();
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        private void _requestBtnDidClick(GameObject _go)
        {
            FriendCommon.sendAddFriendRequest(_m_resultCid);
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _closeBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Friends.C_ADD_ADD_FRIENDS_NODE);
        }
        #endregion
    }
}
