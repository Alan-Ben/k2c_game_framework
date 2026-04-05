using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 常驻排行榜界面基类
    /// </summary>
    public abstract class _AGGUIWndRankFixedDetailBase<T> : _ANPGGUIBasicResBarWnd<T> where T : GGUIMonoRankFixedDetail
    {
        //排行榜实例id列表
        private List<long> _m_rankFixedIdList;
        //当前排行榜下标
        private int _m_curIdx = -1;
        //排行榜数据对象
        private NPRankData _m_rdRankData;
        //当前处理的排行数据序列号
        private long _m_lCurDealRankSerialize;
        //排名列表
        private GGUIWndRankFixedDetailRankGrid _m_wRankGrid;
        //自己的排名item
        private GGUIWndRankFixedDetailSelfRankItem _m_wSelfRankItem;
        //排行榜详情前几名信息附加窗口
        private List<GGUIWndRankFixedTopPlayerInfo> _m_lTopPlayerInfoList;

        protected _AGGUIWndRankFixedDetailBase(EALUIWndLayer _layer) : base(_layer)
        {
            _m_rdRankData = new NPRankData();
            _m_lCurDealRankSerialize = 0;
        }

        private long _m_lUIResId = UIResPathConst.C_COMMON_RANK_NORMAL_RES_ID;//资源id

        /// <summary>
        /// 窗口资源id
        /// </summary>
        public long uiResId
        {
            get { return _m_lUIResId; }
            set { _m_lUIResId = value; }
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(uiResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(uiResId); } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        /// <summary>
        /// 当前排行榜显示下标
        /// </summary>
        public int cutRankIndex { get { return _m_curIdx; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_FIXED_RANK_LIKE, _simulateClickLike);//模拟点击常驻排行榜点赞
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_FIXED_RANK_LIKE, _simulateClickLike);//模拟点击常驻排行榜点赞
            _m_wRankGrid?.hideWnd();
            _m_wSelfRankItem?.hideWnd();
            if (_m_lTopPlayerInfoList != null)
            {
                foreach (GGUIWndRankFixedTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            _m_wRankGrid?.resetWnd();
            _m_wSelfRankItem?.resetWnd();
            if (_m_lTopPlayerInfoList != null)
            {
                foreach (GGUIWndRankFixedTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            _m_wRankGrid?.discard();
            _m_wRankGrid = null;
            _m_wSelfRankItem?.discard();
            _m_wSelfRankItem = null;

            if (_m_lTopPlayerInfoList != null)
            {
                foreach (GGUIWndRankFixedTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.discard();
                }
                _m_lTopPlayerInfoList.Clear();
                _m_lTopPlayerInfoList = null;
            }

            _m_rankFixedIdList = null;
            _m_curIdx = -1;
            //重置数据
            _m_rdRankData?.reset();

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnLike, _onClickLike);
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
            ALUGUICommon.uncombineBtnClick(wnd.btnLast, _onClickLast);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.monoRankListGrid != null)
                _m_wRankGrid = new GGUIWndRankFixedDetailRankGrid(wnd.monoRankListGrid);

            if (wnd.monoSelfRankItem != null)
                _m_wSelfRankItem = new GGUIWndRankFixedDetailSelfRankItem(wnd.monoSelfRankItem);

            _m_lTopPlayerInfoList = new List<GGUIWndRankFixedTopPlayerInfo>();
            if (wnd.monoTopPlayerList != null)
            {
                for (int i = 0; i < wnd.monoTopPlayerList.Count; i++)
                {
                    _m_lTopPlayerInfoList.Add(new GGUIWndRankFixedTopPlayerInfo(wnd.monoTopPlayerList[i]));
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnLike, _onClickLike);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
            ALUGUICommon.combineBtnClick(wnd.btnLast, _onClickLast);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_rankFixedIdList"></param>
        /// <param name="_curIdx"></param>
        public void setInfo(List<long> _rankFixedIdList, int _curIdx)
        {
            if (null == _rankFixedIdList || _rankFixedIdList.Count == 0 || _curIdx >= _rankFixedIdList.Count || _curIdx < 0)
                return;

            _m_rankFixedIdList = _rankFixedIdList;
            _m_curIdx = _curIdx;
            _setRankFixedId(_m_rankFixedIdList[_m_curIdx]);
        }

        /// <summary>
        /// 重置列表的位置
        /// </summary>
        public void resetGridPos()
        {
            _m_wRankGrid?.moveToTop();
        }

        //设置单个排行榜显示
        private void _setRankFixedId(long _rankFixedId)
        {
            //先重置显示
            if (!_m_rdRankData.isDone && _m_rdRankData.rankFixedId != _rankFixedId && _m_lTopPlayerInfoList != null)
            {
                for (int i = 0; i < _m_lTopPlayerInfoList.Count; i++)
                {
                    _m_lTopPlayerInfoList[i]?.showWnd();
                    _m_lTopPlayerInfoList[i]?.setInfo(null);
                }
            }
            _m_wSelfRankItem?.showWnd();
            _m_wSelfRankItem?.setInfo(_m_rdRankData.selfData);
            wnd?.switchRankAnim?.resetAni();

            //刷新排行榜描述
            _refreshRankDesc(_rankFixedId);

            //请求数据刷新界面
            _m_lCurDealRankSerialize = _m_rdRankData.setRanFixedkId(_rankFixedId
                , (_serialize) =>
                {
                    //判断序列号是否一致，保证不会进行错误刷新
                    if (_m_lCurDealRankSerialize != _serialize)
                        return;

                    //设置加载的prefab数据
                    _refreshWnd();
                });
        }

        //刷新排行榜描述
        private void _refreshRankDesc(long _rankFixedId)
        {
            if (null == wnd)
                return;

            NPRankFixedRefObj rankFixedRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_rankFixedId);
            if (null == rankFixedRef)
                return;

            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(rankFixedRef.rank_id);
            if (null == rankRef)
                return;

            //设置排行榜名称
            ALUGUICommon.setLabelTxt(wnd.txtRankName, rankRef.nameStr);
            //设置排行榜分数标题
            ALUGUICommon.setLabelTxt(wnd.txtScoreTitle, TextTranslate.instance.getLanguage(rankRef.score_name));
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (null == wnd)
                return;

            if (null == _m_rankFixedIdList || _m_rankFixedIdList.Count == 0 || _m_curIdx >= _m_rankFixedIdList.Count || _m_curIdx < 0 )
                return;

            long rankFixedId = _m_rankFixedIdList[_m_curIdx];
            NPRankFixedRefObj rankFixedRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(rankFixedId);
            if (null == rankFixedRef)
                return;

            //刷新点赞
            _refreshLike();
            //判断序列号
            if (null == _m_rdRankData || _m_lCurDealRankSerialize != _m_rdRankData.curRefreshSerialize)
                return;

            NPRankFixedRefObj fixedRefObj = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(rankFixedId);
            if (null == fixedRefObj)
                return;

            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(fixedRefObj.rank_id);
            if (null == rankRef)
                return;

            //设置切换按钮显隐
            bool hasNext = null != _m_rankFixedIdList && _m_rankFixedIdList.Count > 0 && _m_curIdx < _m_rankFixedIdList.Count - 1;
            bool hasLast = null != _m_rankFixedIdList && _m_rankFixedIdList.Count > 0 && _m_curIdx > 0;
            ALUGUICommon.setGameObjEnable(wnd.goNoLastHideList, hasLast);
            ALUGUICommon.setGameObjEnable(wnd.goNoNextHideList, hasNext);

            //获取排名列表
            List<NPRankCommonShowInfo> rankShowInfoList = new List<NPRankCommonShowInfo>();
            if(_m_rdRankData.rankListGridDataList != null)
                rankShowInfoList.AddRange(_m_rdRankData.rankListGridDataList);
            rankShowInfoList.Sort((_a,_b)=>_a.rankSortId.CompareTo(_b.rankSortId));
            //设置前几名的特殊展示
            for (int i = 0; i < _m_lTopPlayerInfoList.Count; i++)
            {
                _m_lTopPlayerInfoList[i]?.showWnd();
                _m_lTopPlayerInfoList[i]?.setInfo(rankShowInfoList.Count > i ? rankShowInfoList[i] : null);
            }
            //设置不包含前几名的排名列表
            int topCount = Math.Min(_m_lTopPlayerInfoList.Count, rankShowInfoList.Count);
            for (int i = topCount - 1; i >= 0; i--)
            {
                rankShowInfoList.RemoveAt(i);
            }
            _m_wRankGrid?.showWnd();
            _m_wRankGrid?.setInfo(rankShowInfoList);
            //设置自己的排名
            _m_wSelfRankItem?.showWnd();
            _m_wSelfRankItem?.setInfo(_m_rdRankData.selfData);
        }

        //刷新点赞状态
        private void _refreshLike()
        {
            if (wnd == null || _m_rankFixedIdList == null)
                return;

            //剩余点赞次数
            int count = 0;
            long rankFixedId = _m_rankFixedIdList[_m_curIdx];
            NPRankFixedRefObj fixedRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(rankFixedId);
            if (fixedRef != null)
                count = NPPlayer.instance.fixedCdComp.getCount(fixedRef.like_fixed_cd_id);

            bool needLike = fixedRef != null && fixedRef.like_fixed_cd_id > 0;
            ALUGUICommon.setGameObjEnable(wnd.goCanLikeShowList, needLike && count > 0);
            ALUGUICommon.setGameObjEnable(wnd.goCanNotLikeShowList, needLike && count <= 0);
        }

        //查找并切换下一个可以点赞的排行榜
        private void _findAndSwitchNextCanLike()
        {
            if (null == _m_rankFixedIdList || _m_rankFixedIdList.Count == 0)
                return;

            for (int i = 0; i < _m_rankFixedIdList.Count; i++)
            {
                int count = 0;
                NPRankFixedRefObj fixedRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_m_rankFixedIdList[i]);
                if (fixedRef != null)
                    count = NPPlayer.instance.fixedCdComp.getCount(fixedRef.like_fixed_cd_id);

                if (count > 0)
                {
                    _switchToTargetRank(i);
                    return;
                }
            }
        }

        //切换到对应排行榜
        private void _switchToTargetRank(int _index)
        {
            if (null == _m_rankFixedIdList || _m_rankFixedIdList.Count == 0 || _index >= _m_rankFixedIdList.Count || _index < 0)
                return;

            _m_curIdx = _index;
            NPRankFixedRefObj rankFixRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_m_rankFixedIdList[_index]);
            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(rankFixRef != null ? rankFixRef.rank_id : 0);
            bool isGuild = rankRef != null && rankRef.rank_type == ERankType.GUILD;
            if (isGuild)
            {
                //记录当前数据
                List<long> rankFixedIdList = new List<long>();
                rankFixedIdList.AddRange(_m_rankFixedIdList);

                //关闭当前窗口切换到联盟排行榜窗口
                _onClickClose(null);
                QueueMgr.instance.AddNode(new GNodeGuildRank(rankFixedIdList, _index));
            }
            else
                _setRankFixedId(_m_rankFixedIdList[_index]);
        }

        #region 点击事件

        //点赞按钮
        private void _onClickLike(GameObject _go)
        {
            if (null == _m_rankFixedIdList || _m_rankFixedIdList.Count == 0 || _m_curIdx >= _m_rankFixedIdList.Count || _m_curIdx < 0)
                return;

            long rankFixedId = _m_rankFixedIdList[_m_curIdx];

            //剩余点赞次数
            int count = 0;
            NPRankFixedRefObj fixedRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(rankFixedId);
            if (fixedRef != null)
                count = NPPlayer.instance.fixedCdComp.getCount(fixedRef.like_fixed_cd_id);

            //已点赞不处理
            if (count <= 0)
                return;

            NPPlayer.instance.rankCommonComp.reqRankFixedLike(rankFixedId, (_result) =>
            {
                if (null == _result)
                    return;

                _refreshLike();
                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_RankFixedLikeSuc(_result, () =>
                {
                    //点赞成功后，查找下一个可以点赞的排行榜
                    _findAndSwitchNextCanLike();
                }));
            });
        }

        //下一个排行榜
        private void _onClickNext(GameObject _go)
        {
            if (null == _m_rankFixedIdList || _m_rankFixedIdList.Count == 0 || _m_curIdx >= _m_rankFixedIdList.Count - 1)
                return;

            wnd?.switchRankAnim?.forcePlay();
            _m_curIdx++;
            resetGridPos();

            _switchToTargetRank(_m_curIdx);
        }

        //上一个排行榜
        private void _onClickLast(GameObject _go)
        {
            if (null == _m_rankFixedIdList || _m_rankFixedIdList.Count == 0 || _m_curIdx <= 0)
                return;

            wnd?.switchRankAnim?.forcePlay();
            _m_curIdx--;
            resetGridPos();

            _switchToTargetRank(_m_curIdx);
        }

        //点击关闭按钮
        protected abstract void _onClickClose(GameObject _go);

        #endregion


        //模拟点击常驻排行榜点赞
        private void _simulateClickLike(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long rankId = (long) _objects[0];
            if (rankId <= 0 || _m_rankFixedIdList == null || rankId != _m_rankFixedIdList[_m_curIdx])
                return;
            
            _onClickLike(null);
        }
    }
}
