using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;
using NPCommon;

namespace GOE
{
    // 常驻通用排行榜列表界面
    public class NPGGUIWndRankFixedList : _ANPGGUIBasicResBarWnd<NPGGUIMonoRankFixedList>
    {
        private static NPGGUIWndRankFixedList _g_instance;
        public static NPGGUIWndRankFixedList instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndRankFixedList();
                return _g_instance;
            }
        }

        //列表容器
        private List<NPGGUIWndRankFixedItem> _m_fixedItemList;

        protected NPGGUIWndRankFixedList()
           : base(EALUIWndLayer.NORMAL)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return NPGGUIMonoRankFixedList.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoRankFixedList.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        #region override
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_OPEN_FIXED_RANK, simulateClickOpenFixedRank);//模拟点击打开常驻排行榜
            if (null == wnd)
                return;

            if (null != _m_fixedItemList)
            {
                NPGGUIWndRankFixedItem temp = null;
                for (int i = 0; i < _m_fixedItemList.Count; i++)
                {
                    temp = _m_fixedItemList[i];
                    if (null == temp)
                        continue;

                    temp.showWnd();
                }
            }

            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_OPEN_FIXED_RANK, simulateClickOpenFixedRank);//模拟点击打开常驻排行榜
            if (null != _m_fixedItemList)
            {
                NPGGUIWndRankFixedItem temp = null;
                for (int i = 0; i < _m_fixedItemList.Count; i++)
                {
                    temp = _m_fixedItemList[i];
                    if (null == temp)
                        continue;

                    temp.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {

        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            if (null != _m_fixedItemList)
            {
                NPGGUIWndRankFixedItem temp = null;
                for (int i = 0; i < _m_fixedItemList.Count; i++)
                {
                    temp = _m_fixedItemList[i];
                    if (null == temp)
                        continue;

                    temp.discard();
                }
                _m_fixedItemList.Clear();
            }
            _m_fixedItemList = null;

            ALUGUICommon.uncombineBtnClick(wnd.aKeyLikeBtn, _aKeyLikeBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.localFixedItemList && wnd.localFixedItemList.Count > 0)
            {
                _m_fixedItemList = new List<NPGGUIWndRankFixedItem>();
                NPGGUIMonoRankFixedItem temp = null;
                for (int i = 0; i < wnd.localFixedItemList.Count; i++)
                {
                    temp = wnd.localFixedItemList[i];
                    if (null == temp)
                        continue;

                    NPGGUIWndRankFixedItem item = new NPGGUIWndRankFixedItem(temp);
                    _m_fixedItemList.Add(item);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.aKeyLikeBtn, _aKeyLikeBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }
        #endregion

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || null == wnd.localFixedItemList || wnd.localFixedItemList.Count == 0)
                return;

            NPGGUIMonoRankFixedItem temp = null;
            List<long> rankFixedIdList = new List<long>();
            for (int i = 0; i < wnd.localFixedItemList.Count; i++)
            {
                temp = wnd.localFixedItemList[i];
                if (null == temp)
                    continue;

                rankFixedIdList.Add(temp.fixedRankId);
            }

            if(null != _m_fixedItemList)
            {
                NPGGUIWndRankFixedItem tempWnd = null;
                for (int i = 0; i < _m_fixedItemList.Count; i++)
                {
                    tempWnd = _m_fixedItemList[i];
                    if (null == temp)
                        continue;

                    tempWnd.setFixedRankIdList(rankFixedIdList, _refreshLikeBtnState);
                }
            }

            _refreshLikeBtnState();
        }

        /// <summary>
        /// 刷新点赞按钮状态
        /// </summary>
        private void _refreshLikeBtnState()
        {
            if (wnd == null)
                return;

            long likeCount = getLeftLikeCount();
            ALUGUICommon.setGameObjEnable(wnd.canAKeyLikeShowGoList, likeCount > 0);
            ALUGUICommon.setGameObjEnable(wnd.noAKeyLikeShowGoList, likeCount == 0);
        }

        //获取剩余点赞次数
        private long getLeftLikeCount()
        {
            long likeCount = 0;
            NPGGUIMonoRankFixedItem temp = null;
            for (int i = 0; i < wnd.localFixedItemList.Count; i++)
            {
                temp = wnd.localFixedItemList[i];
                if (null == temp)
                    continue;

                NPRankFixedRefObj refObj = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(temp.fixedRankId);
                if (null != refObj)
                {
                    NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(refObj.rank_id);
                    if (rankRef != null && rankRef.rank_type == ERankType.GUILD)
                    {
                        if(NPPlayer.instance.rankCommonComp.getHaveGuildInRank(refObj.id))
                            likeCount += GCommon.getItemCount(ENPItemType.FIXED_CD, refObj.like_fixed_cd_id);
                    }
                    else
                        likeCount += GCommon.getItemCount(ENPItemType.FIXED_CD, refObj.like_fixed_cd_id);
                }
            }
            return likeCount;
        }

        /// <summary>
        /// 一键点赞
        /// </summary>
        /// <param name="_go"></param>
        private void _aKeyLikeBtnDidClick(GameObject _go)
        {
            if (null == wnd || null == wnd.localFixedItemList)
                return;

            //如果没有点赞次数，直接返回
            long likeCount = getLeftLikeCount();
            if(likeCount <= 0)
                return;

            List<long> rankFixedIdList = new List<long>();
            NPGGUIMonoRankFixedItem temp = null;
            for (int i = 0; i < wnd.localFixedItemList.Count; i++)
            {
                temp = wnd.localFixedItemList[i];
                if (null == temp)
                    continue;

                rankFixedIdList.Add(temp.fixedRankId);
            }

            NPPlayer.instance.rankCommonComp.reqRankFixedAKeyLike(rankFixedIdList, (_resultList) =>
            {
                if (null == _resultList || _resultList.Count == 0)
                    return;

                foreach (Common.RankObj.RankFixed_LikeResult result in _resultList)
                {
                    NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_RankFixedLikeSuc(result));
                }
                _refreshWnd();
            });

        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Rank.C_MAIN_RANK_FIXED_LIST_NODE);
        }

        //模拟点击打开常驻排行榜
        private void simulateClickOpenFixedRank(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_fixedItemList == null)
                return;

            long fixedRankId = (long) _objects[0];
            if (fixedRankId <= 0)
                return;

            for (int i = 0; i < _m_fixedItemList.Count; i++)
            {
                if (_m_fixedItemList[i].rankId == fixedRankId)
                {
                    _m_fixedItemList[i].dealClickItem();
                    break;
                }
            }
        }
    }
}
