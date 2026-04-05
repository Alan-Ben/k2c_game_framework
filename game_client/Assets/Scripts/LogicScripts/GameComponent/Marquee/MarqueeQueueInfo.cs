using System.Collections.Generic;
using Common;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 跑马灯队列信息
    /// </summary>
    public class MarqueeQueueInfo
    {
        //位置id
        private int _m_lPosId;
        //保存已读数据
        private MarqueeRemarkInfo _m_recordInfo;
        //跑马灯列表
        [NotNull] private List<MarqueeInfo> _m_lMarqueeList;

        public MarqueeQueueInfo(Common_MarqueeShowPosInfo _info, MarqueeRemarkInfo _recordInfo)
        {
            if (_info == null)
                return;

            _m_recordInfo = _recordInfo;
            _m_lPosId = _info.getShowPosId();
            _m_lMarqueeList = new List<MarqueeInfo>();
            if (_info.getMarqueeList() != null)
            {
                for (int i = 0; i < _info.getMarqueeList().Count; i++)
                {
                    MarqueeInfo marqueeInfo = new MarqueeInfo(_m_lPosId, _info.getMarqueeList()[i]);
                    _m_lMarqueeList.Add(marqueeInfo);
                }
            }
            _m_lMarqueeList.Sort(_sortList);
        }

        public MarqueeQueueInfo(int _showPosId, Common_MarqueeInfo _info, MarqueeRemarkInfo _recordInfo)
        {
            if (_info == null)
                return;

            _m_recordInfo = _recordInfo;
            _m_lPosId = _showPosId;
            _m_lMarqueeList = new List<MarqueeInfo>();
            MarqueeInfo marqueeInfo = new MarqueeInfo(_showPosId, _info);
            _m_lMarqueeList.Add(marqueeInfo);
            _m_lMarqueeList.Sort(_sortList);
        }


        /// <summary>
        /// 获取下一个需要展示的跑马灯信息
        /// </summary>
        /// <returns></returns>
        public MarqueeInfo getNextMarquee()
        {
            while (_m_lMarqueeList.Count > 0)
            {
                MarqueeInfo info = _m_lMarqueeList.GetFirst();
                if (info != null && info.checkIsValid())
                {
                    //记录开始展示时间
                    info.setStartShowTime();
                    return info;
                }
                else
                {
                    //记录已读
                    _recordReadData(info);
                    _m_lMarqueeList.Remove(info);
                }
            }

            return null;
        }

        /// <summary>
        /// 移除跑马灯
        /// </summary>
        /// <param name="_instanceId"></param>
        public bool removeMarquee(long _instanceId)
        {
            for (int i = 0; i < _m_lMarqueeList.Count; i++)
            {
                if (_m_lMarqueeList[i] != null && _m_lMarqueeList[i].instanceId == _instanceId)
                {
                    //记录已读
                    _recordReadData(_m_lMarqueeList[i]);
                    _m_lMarqueeList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 移除跑马灯，不记录已读数据，主要用于后台命令手动操作移除指定跑马灯
        /// </summary>
        /// <returns></returns>
        public bool removeMarqueeWithNoRecord(long _instanceId)
        {
            for (int i = 0; i < _m_lMarqueeList.Count; i++)
            {
                if (_m_lMarqueeList[i] != null && _m_lMarqueeList[i].instanceId == _instanceId)
                {
                    _m_lMarqueeList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 移除所有跑马灯
        /// </summary>
        public void removeAllMarquee()
        {
            if (_m_lMarqueeList.Count == 0)
                return;

            //查找最大实例id
            long maxInstanceId = 0;
            for (int i = 0; i < _m_lMarqueeList.Count; i++)
            {
                if (_m_lMarqueeList[i] != null && maxInstanceId < _m_lMarqueeList[i].instanceId)
                    maxInstanceId = _m_lMarqueeList[i].instanceId;
            }

            for (int i = 0; i < _m_lMarqueeList.Count; i++)
            {
                //记录已读
                if (_m_lMarqueeList[i] != null)
                    _m_recordInfo?.setHadReadMarquee(_m_lPosId, maxInstanceId, _m_lMarqueeList[i].priorityId, _m_lMarqueeList[i].instanceId);
            }

            _m_lMarqueeList.Clear();
        }

        /// <summary>
        /// 获取跑马灯数量
        /// </summary>
        /// <returns></returns>
        public long getMarqueeCount()
        {
            return _m_lMarqueeList.Count;
        }

        /// <summary>
        /// 新增跑马灯
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public void addMarquee(int _showPosId, Common_MarqueeInfo _info)
        {
            if (_info == null)
                return;

            MarqueeInfo marqueeInfo = new MarqueeInfo(_showPosId, _info);
            _m_lMarqueeList.Add(marqueeInfo);
            _m_lMarqueeList.Sort(_sortList);
        }

        /// <summary>
        /// 检查跑马灯是否有效
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <returns></returns>
        public bool checkIsValid(long _instanceId)
        {
            for (int i = 0; i < _m_lMarqueeList.Count; i++)
            {
                if (_m_lMarqueeList[i] != null && _m_lMarqueeList[i].instanceId == _instanceId)
                {
                    return _m_lMarqueeList[i].checkIsValid();
                }
            }

            return false;
        }

        /// <summary>
        /// 处理移除数据，记录已读
        /// </summary>
        /// <param name="_info"></param>
        private void _recordReadData(MarqueeInfo _info)
        {
            if (_info == null)
                return;

            long miniInstanceId = -1;
            for (int i = 0; i < _m_lMarqueeList.Count; i++)
            {
                if(_m_lMarqueeList[i] == null)
                    continue;

                if (miniInstanceId == -1)
                    miniInstanceId = _m_lMarqueeList[i].instanceId;
                else if (miniInstanceId > _m_lMarqueeList[i].instanceId)
                    miniInstanceId = _m_lMarqueeList[i].instanceId;
            }

            //如果当前不是最小实例id的跑马灯，不记录当前实例id
            if (_info.instanceId != miniInstanceId)
                miniInstanceId = -1;

            _m_recordInfo?.setHadReadMarquee(_m_lPosId, miniInstanceId, _info.priorityId, _info.instanceId);
        }

        //按优先级从大到小排序，优先级相同的按顺序
        private int _sortList(MarqueeInfo _a, MarqueeInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            if (_a.priorityId.CompareTo(_b.priorityId) != 0)
                return -(_a.priorityId.CompareTo(_b.priorityId));
            else
                return _a.instanceId.CompareTo(_b.instanceId);
        }
    }
}