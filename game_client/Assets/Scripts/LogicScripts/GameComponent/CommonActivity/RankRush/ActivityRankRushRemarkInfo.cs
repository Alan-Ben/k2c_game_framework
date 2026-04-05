using Common.ClientData;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 冲榜相关记录的信息
    /// </summary>
    public class ActivityRankRushRemarkInfo : _ANPRemarkInfo
    {
        //记录信息
        [NotNull] private List<ActivityRankRush_ClientData> _m_lRecordList;

        public ActivityRankRushRemarkInfo() : base(ENPClientDataType.RANK_RUSH)
        {
            _m_lRecordList = new List<ActivityRankRush_ClientData>();
        }

        /// <summary>
        /// 记录冲榜初始分数
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_rankId"></param>
        /// <param name="_recordInitialValue"></param>
        public void setRecordInitialValue(long _instanceId, long _rankId, long _recordInitialValue)
        {
            for (int i = 0; i < _m_lRecordList.Count; i++)
            {
                if (_m_lRecordList[i].getInstanceId() == _instanceId && _m_lRecordList[i].getRankId() == _rankId)
                {
                    _m_lRecordList[i].setRecordInitialValue(_recordInitialValue);
                    saveData();
                    return;
                }
            }

            _m_lRecordList.Add(new ActivityRankRush_ClientData(_instanceId, _rankId, _recordInitialValue, 0));
            saveData();
        }

        /// <summary>
        /// 获取记录的初始值
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_rankId"></param>
        /// <returns></returns>
        public long getRecordInitialValue(long _instanceId, long _rankId)
        {
            for (int i = 0; i < _m_lRecordList.Count; i++)
            {
                if (_m_lRecordList[i].getInstanceId() == _instanceId && _m_lRecordList[i].getRankId() == _rankId)
                    return _m_lRecordList[i].getRecordInitialValue();
            }

            return -1;
        }

        /// <summary>
        /// 记录冲榜排名
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_rankId"></param>
        /// <param name="_ranking"></param>
        public void setRecordRanking(long _instanceId, long _rankId, long _ranking)
        {
            for (int i = 0; i < _m_lRecordList.Count; i++)
            {
                if (_m_lRecordList[i].getInstanceId() == _instanceId && _m_lRecordList[i].getRankId() == _rankId)
                {
                    _m_lRecordList[i].setRanking(_ranking);
                    saveData();
                    return;
                }
            }
            
            _m_lRecordList.Add(new ActivityRankRush_ClientData(_instanceId, _rankId, -1, _ranking));
            saveData();
        }

        /// <summary>
        /// 获取冲榜上次记录的排名
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_rankId"></param>
        /// <returns></returns>
        public long getRecordRanking(long _instanceId, long _rankId)
        {
            for (int i = 0; i < _m_lRecordList.Count; i++)
            {
                if (_m_lRecordList[i].getInstanceId() == _instanceId && _m_lRecordList[i].getRankId() == _rankId)
                    return _m_lRecordList[i].getRanking();
            }

            return 0;
        }

        /// <summary>
        /// 检查活动是否还在进行中，如果不在进行中则清除数据
        /// </summary>
        public void checkActivityIsRunnung()
        {
            bool isRemove = false;
            for (int i = _m_lRecordList.Count - 1; i >= 0; i--)
            {
                if (_m_lRecordList[i] == null)
                {
                    _m_lRecordList.RemoveAt(i);
                    isRemove = true;
                    continue;
                }

                _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(_m_lRecordList[i].getInstanceId());
                if (activityInfo == null)
                {
                    _m_lRecordList.RemoveAt(i);
                    isRemove = true;
                }
            }

            if (isRemove)
                saveData();
        }

        /// <summary>
        /// 检查排名和上次记录的是否有变化
        /// </summary>
        public void checkRankingIsChange()
        {
            //获取冲榜活动数据列表
            List<ActivityRankRushInfo> rankRushInfoList = new List<ActivityRankRushInfo>();
            NPPlayer.instance.commonActivityComp.getActivityRankRushInfoList(rankRushInfoList);
            for (int i = 0; i < rankRushInfoList.Count; i++)
            {
                ActivityRankRushInfo rankRushInfo = rankRushInfoList[i];
                if (rankRushInfo == null || rankRushInfo.activityInfo == null || rankRushInfo.rankRefObj == null || rankRushInfo.activityRankRushRefObj == null)
                    continue;

                long activityInstanceId = rankRushInfo.activityInfo.instanceId;
                long rankId = rankRushInfo.rankRefObj.rank_id;
                long rankRushId = rankRushInfo.activityRankRushRefObj.id;
                //上次记录的排名
                long lastRanking = getRecordRanking(rankRushInfo.activityInfo.instanceId, rankRushInfo.rankRefObj.rank_id);
                //请求自己的排名信息
                NPPlayer.instance.commonActivityComp.reqActivityRankBaseInfoByKey(activityInstanceId, rankId, NPPlayer.instance.playerInfo.CID, rankRushInfo.activityInfo.isCross,
                    _msg =>
                    {
                        if (_msg == null || _msg.getBaseItem() == null)
                            return;

                        //当前排名
                        long curRanking = _msg.getBaseItem().getRank();

                        //记录当前排名
                        if (lastRanking != curRanking)
                            setRecordRanking(activityInstanceId, rankId, curRanking);

                        //排名有变化且变化了名次或者在前十名展示tip
                        if (lastRanking > 0 && curRanking > 0 && lastRanking != curRanking && (lastRanking > curRanking || curRanking <= 10))
                        {
                            //添加tip展示
                            ActivityRankRushChangeTipMgr.instance.addRankingChangeTipInfo(activityInstanceId, rankRushId, lastRanking, curRanking);
                        }
                    });
            }
        }

        protected override byte[] _makeData()
        {
            ActivityRankRushList_ClientData data = new ActivityRankRushList_ClientData(_m_lRecordList);
            return data.makePackage();
        }

        protected override void _readData(byte[] _data)
        {
            ActivityRankRushList_ClientData data = new ActivityRankRushList_ClientData();
            if(_data != null)
                data.readPackage(_data);

            _m_lRecordList = data.getDataList();
        }
        
        protected override void _resetRemarkInfo()
        {
            ActivityRankRushList_ClientData data = new ActivityRankRushList_ClientData();
            saveData();
            _readData(data.makePackage());
        }
    }
}