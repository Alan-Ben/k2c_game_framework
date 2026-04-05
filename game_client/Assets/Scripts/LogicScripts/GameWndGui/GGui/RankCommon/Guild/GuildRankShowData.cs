using System.Collections.Generic;
using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 排行榜数据管理类，本类会自动请求相关数据并处理
    /// </summary>
    public class GuildRankShowData
    {
        //排行榜实例id - 这里先用常驻排行榜id
        private long _m_lRankFixedId;
        //刷新数据序列号
        private long _m_lRefreshSerialize;
        //列表数据
        private List<GuildRankInfo> _m_rankListGridDataList;
        //玩家自己的数据
        private GuildRankInfo _m_rpSelfData;
        //加载完成表示
        private bool _m_isDone;

        /// <summary>
        /// 刷新数据序列号
        /// </summary>
        public long curRefreshSerialize { get { return _m_lRefreshSerialize; } }
        /// <summary>
        /// 联盟排行榜数据列表
        /// </summary>
        public List<GuildRankInfo> rankListGridDataList { get { return _m_rankListGridDataList; } }
        /// <summary>
        /// 自己联盟排行的数据
        /// </summary>
        public GuildRankInfo selfData { get { return _m_rpSelfData; } }
        /// <summary>
        /// 常驻排行榜id
        /// </summary>
        public long rankFixedId { get { return _m_lRankFixedId; } }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isDone { get { return _m_isDone; } }

        public GuildRankShowData()
        {
            _m_lRankFixedId = 0;
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            _m_rankListGridDataList = new List<GuildRankInfo>();
            _m_rpSelfData = null;
        }

        /// <summary>
        /// 设置排行榜实例Id - 先用常驻排行榜id，返回对应最新的操作序列号用于判断合法性
        /// </summary>
        /// <returns></returns>
        public void setRanFixedkId(long _rankFixedId, Action<long> _doneDelegate,out long _refreshSerialize)
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            _refreshSerialize = _m_lRefreshSerialize;

            if (_m_lRankFixedId == _rankFixedId && _m_isDone)
            {
                _doneDelegate?.Invoke(_m_lRefreshSerialize);
                return;
            }


            _m_isDone = false;
            _m_lRankFixedId = _rankFixedId;
            if (_m_rankListGridDataList == null)
                _m_rankListGridDataList = new List<GuildRankInfo>();
            _m_rankListGridDataList.Clear();

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
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        public void reset()
        {
            _m_lRankFixedId = 0;
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            _m_rankListGridDataList?.Clear();
            _m_rpSelfData = null;
            _m_isDone = false;
        }

        /// <summary>
        /// 请求列表数据
        /// </summary>
        private void _reqListData(long _opSerialize, Action _doneDelegate)
        {
            //请求排行榜简要数据
            NPPlayer.instance.rankCommonComp.reqRankFixedBaseList(_m_lRankFixedId, (_list) =>
            {
                try
                {
                    if (null == _list || _opSerialize != _m_lRefreshSerialize)
                        return;

                    NPRankFixedRefObj refObj = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_m_lRankFixedId);
                    if (null == refObj)
                        return;

                    List<Common.RankObj.Rank_BaseItem> baseList = _list.getBaseItemlist();
                    if (baseList != null && baseList.Count > GRefdataCoreMgr.instance.npGeneral.rank_show_max_num)
                        baseList = baseList.GetRange(0, GRefdataCoreMgr.instance.npGeneral.rank_show_max_num);

                    _m_rankListGridDataList = GuildRankInfo.getRankInfoList(baseList, _m_lRankFixedId, false);
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
            //如果未加入联盟，则不请求
            if (!NPPlayer.instance.guildComp.isJoinGuild())
            {
                _m_rpSelfData = null;
                if (null != _doneDelegate)
                    _doneDelegate();
                return;
            }

            //请求自己联盟排行信息
            NPPlayer.instance.rankCommonComp.reqInRankPlayerInfoByCid(_m_lRankFixedId, NPPlayer.instance.guildComp.guildInfo.guildId, _info =>
            {
                try
                {
                    if (_opSerialize != _m_lRefreshSerialize)
                        return;

                    _m_rpSelfData = new GuildRankInfo(_info, _m_lRankFixedId, false);
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
