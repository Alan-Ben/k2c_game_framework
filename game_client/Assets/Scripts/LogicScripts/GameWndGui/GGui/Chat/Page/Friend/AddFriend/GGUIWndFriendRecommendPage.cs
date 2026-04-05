using System.Collections.Generic;
using ALPackage;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 好友推荐页面
    /// </summary>
    public class GGUIWndFriendRecommendPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoFriendRecommendPage>
    {
        private GGUIWndFriendAddFriendListGrid _m_friendListGrid;

        private List<NPCommonSimplePlayerInfo> _m_recommendList;//推荐列表
        private NPCommonSimplePlayerInfo _m_searchPlayer;//搜索得到的玩家信息
        private bool _m_isInSearch = false;//搜索中
        private bool _m_isSearchResultEnd = false;//搜索有解果了
        private long _m_lShowSerialize;//显示序列号

        public GGUIWndFriendRecommendPage(Transform _parent) : base(_parent)
        {
        }


        protected override string _monoAssetPath { get => GGUIMonoFriendRecommendPage.assetPath; }
        protected override string _monoObjName { get => GGUIMonoFriendRecommendPage.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _clickRefreshRecommend(null);
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            _m_friendListGrid?.discard();
            _m_friendListGrid = null;

            _m_isInSearch = false;
            _m_isSearchResultEnd = false;
            
            _m_recommendList?.Clear();
            _m_recommendList = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            _m_recommendList = new List<NPCommonSimplePlayerInfo>();
            ALUGUICommon.combineBtnClick(wnd.btnSearch, _clickSearch);
            ALUGUICommon.combineBtnClick(wnd.btnClearSearch, _clickClearSearch);
            ALUGUICommon.combineBtnClick(wnd.btnRefreshRecommend, _clickRefreshRecommend);
            if (null != wnd.inputSearch)
            {
                wnd.inputSearch.onValueChanged.AddListener(_onValueChanged);
                wnd.inputSearch.onEndEdit.AddListener(_onEndEdit);
            }

            if (null != wnd.friendListGrid)
            {
                _m_friendListGrid = new GGUIWndFriendAddFriendListGrid(wnd.friendListGrid);
            }
        }

        private void _onValueChanged(string _text)
        {
            string curText = _text;
            CharacterDetermineMgr.instance.replaceIllegalCharacter(false, ref curText);
            ALUGUICommon.setInputTxt(wnd.inputSearch, curText);
        }

        private void _onEndEdit(string arg0)
        {
            if(null == wnd || null == wnd.inputSearch)
                return;
            if (string.IsNullOrEmpty(wnd.inputSearch.text))
            {
                _clickClearSearch(null);
            }
        }

        /// <summary>
        /// 点击搜索
        /// </summary>
        /// <param name="obj"></param>
        private void _clickSearch(GameObject obj)
        {
            if(null == wnd || null == wnd.inputSearch)
                return;
            string inputText = wnd.inputSearch.text;
            //判断合法性，
            if (long.TryParse(inputText, out long _cid))
            {
                //发送消息搜索
                _m_isInSearch = true;
                GCommon.reqPlayerInfo(_cid, (_playerInfo) =>
                {
                    _m_isSearchResultEnd = true;
                    _m_searchPlayer = _playerInfo;
                    _refreshWnd();
                });
            }
            else
            {
                //输入不合法   
            }
        }

        /// <summary>
        /// 点击清除搜索
        /// </summary>
        /// <param name="obj"></param>
        private void _clickClearSearch(GameObject obj)
        {
            _m_searchPlayer = null;
            _m_isInSearch = false;
            _m_isSearchResultEnd = false;
            ALUGUICommon.setInputTxt(wnd.inputSearch, null);
            _refreshWnd();
        }

        /// <summary>
        /// 刷新推荐列表
        /// </summary>
        /// <param name="obj"></param>
        private void _clickRefreshRecommend(GameObject obj)
        {
            _m_isInSearch = false;
            _m_isSearchResultEnd = false;
            long serialize = _m_lShowSerialize;
            //从请求推荐列表
            NPPlayer.instance.friendsComp.reqFriendRecommend((_info) =>
            {
                if (_m_recommendList == null || !isShow || serialize != _m_lShowSerialize || _info == null)
                    return;

                _m_recommendList?.Clear();
                foreach (PlayerInfo_IconShow infoIconShow in _info.getPlayerList())
                {
                    if(infoIconShow == null || infoIconShow.getCid() == NPPlayer.instance.playerInfo.CID)
                        continue;
                    _m_recommendList.Add(new NPCommonSimplePlayerInfo(infoIconShow));
                }
                
                _refreshWnd();
            });
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            if (null != _m_friendListGrid)
            {
                //显示好友列表
                _m_friendListGrid.showWnd();
                
                if (_m_isInSearch && _m_isSearchResultEnd)//显示搜索结果
                {
                    _m_friendListGrid.showItem(_m_searchPlayer);
                }
                else
                {
                    _m_friendListGrid.showItemList(_m_recommendList);
                }
            }

            bool showSearch = _m_isInSearch && _m_isSearchResultEnd;
            //根据是否再搜索中，展示不同的内容
            ALUGUICommon.setGameObjEnable(wnd.recommendShowList,!showSearch);
            ALUGUICommon.setGameObjEnable(wnd.searchShowList,showSearch);
        }
    }
}