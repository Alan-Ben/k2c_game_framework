using System.Collections.Generic;
using System;
using ALPackage;
using Common.RankObj;

namespace GOE
{
    //排行榜数据管理类，本类会自动请求相关数据并处理
    public class NPRankData
    {
        //排行榜实例id - 这里先用常驻排行榜id
        private long _m_rankFixedId;

        //刷新数据序列号
        private long _m_lRefreshSerialize;

        //列表数据
        private List<NPRankCommonShowInfo> _m_rankListGridDataList;

        //玩家自己的数据
        private NPRankCommonShowInfo _m_rpSelfData;

        //第一名玩家数据
        // private NPRankCommonShowInfo _m_firstPlayerInfo;

        //加载完成表示
        private bool _m_isDone;

        public NPRankData()
        {
            _m_rankFixedId = 0;
            _m_lRefreshSerialize = ALSerializeOpMgr.next();


            _m_rankListGridDataList = new List<NPRankCommonShowInfo>();
            // _m_firstPlayerInfo = null;
            _m_rpSelfData = null;
        }

        public long curRefreshSerialize { get { return _m_lRefreshSerialize; } }
        public List<NPRankCommonShowInfo> rankListGridDataList { get { return _m_rankListGridDataList; } }
        public NPRankCommonShowInfo selfData { get { return _m_rpSelfData; } }
        public long rankFixedId { get { return _m_rankFixedId; } }
        public bool isDone { get { return _m_isDone; } }

        // public NPRankCommonShowInfo firstPlayerInfo { get { return _m_firstPlayerInfo; } }
        //重置数据
        public void reset()
        {
            _m_rankFixedId = 0;
            _m_lRefreshSerialize = ALSerializeOpMgr.next();

            _m_rankListGridDataList = new List<NPRankCommonShowInfo>();
            // _m_firstPlayerInfo = null;
            _m_rpSelfData = null;
            _m_isDone = false;
        }

        /// <summary>
        /// 设置排行榜实例Id - 先用常驻排行榜id，返回对应最新的操作序列号用于判断合法性
        /// </summary>
        /// <param name="_rankId"></param>
        /// <returns></returns>
        public long setRanFixedkId(long _rankFixedId, Action<long> _doneDelegate)
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();

            if (_m_rankFixedId == _rankFixedId && _m_isDone)
            {
                return _m_lRefreshSerialize;
            }
            _m_isDone = false;
            _m_rankFixedId = _rankFixedId;
            _m_rankListGridDataList.Clear();
            // _m_firstPlayerInfo = null;

            //注册回调
            long curSerialize = _m_lRefreshSerialize;
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                _m_isDone = true;
                if (null != _doneDelegate)
                    _doneDelegate(curSerialize);
            });

            //逐个请求2个部分数据
            _reqListData(_m_lRefreshSerialize, stepCounter.addDoneStepCount);
            _reqSelfData(_m_lRefreshSerialize, stepCounter.addDoneStepCount);

            return _m_lRefreshSerialize;
        }

        /// <summary>
        /// 请求列表数据
        /// </summary>
        private void _reqListData(long _opSerialize, Action _doneDelegate)
        {
            //请求排行榜简要数据
            NPPlayer.instance.rankCommonComp.reqRankFixedBaseList(_m_rankFixedId, (_list) =>
            {
                try
                {
                    if (null == _list || _opSerialize != _m_lRefreshSerialize)
                        return;

                    NPRankFixedRefObj refObj = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_m_rankFixedId);
                    if (null == refObj)
                        return;

                    List<Common.RankObj.Rank_BaseItem> baseList = _list.getBaseItemlist();
                    if (baseList.Count > GRefdataCoreMgr.instance.npGeneral.rank_show_max_num)
                    {
                        baseList = baseList.GetRange(0, GRefdataCoreMgr.instance.npGeneral.rank_show_max_num);
                    }

                    List<NPRankCommonShowInfo> gridListShowInfoList = new List<NPRankCommonShowInfo>();
                    Common.RankObj.Rank_BaseItem temp = null;
                    for (int i = 0; i < baseList.Count; i++)
                    {
                        temp = baseList[i];
                        if (null == temp)
                            continue;

                        NPRankCommonShowInfo info = new NPRankCommonShowInfo(temp, false);
                        if (null != refObj)
                            info.setRankId(refObj.rank_id, refObj.id);
                        //第一名请求详细信息
                        // if (temp.getRank() == 1)
                        // {
                        //     _m_firstPlayerInfo = new NPRankCommonShowInfo(temp);
                        //     _m_firstPlayerInfo.setRankId(refObj.rank_id, refObj.id);
                        //     continue;
                        // }
                        //排行榜列表
                        gridListShowInfoList.Add(info);
                    }

                    //数据管理
                    _m_rankListGridDataList = gridListShowInfoList;
                }
                finally
                {
                    if (null != _doneDelegate)
                        _doneDelegate();
                }
            });
        }

        /// <summary>
        /// 请求自己当前的排名信息
        /// </summary>
        /// <param name="_opSerialize"></param>
        private void _reqSelfData(long _opSerialize, Action _doneDelegate)
        {
            NPPlayer.instance.rankCommonComp.reqInRankPlayerInfoByCid(_m_rankFixedId, NPPlayer.instance.playerInfo.CID, (_player) =>
            {
                try
                {
                    if (_opSerialize != _m_lRefreshSerialize)
                        return;

                    NPRankFixedRefObj refObj = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_m_rankFixedId);
                    if (null == refObj)
                        return;

                    _m_rpSelfData = new NPRankCommonShowInfo(_player, false);
                    _m_rpSelfData.setRankId(refObj.rank_id, refObj.id);
                }
                finally
                {
                    if (null != _doneDelegate)
                        _doneDelegate();
                }
            });
        }

    }
}
