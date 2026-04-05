using System.Collections.Generic;
using Common;

namespace GOE
{
    /// <summary>
    /// 跑马灯记录的信息
    /// </summary>
    public class MarqueeRemarkInfo:_ANPRemarkInfo
    {
        //已读记录列表
        private List<Common_MarqueeRecordReadInfo> _m_readInfoList;

        /// <summary>
        /// 已读记录列表
        /// </summary>
        public List<Common_MarqueeRecordReadInfo> readInfoList { get { return _m_readInfoList; } }

        public MarqueeRemarkInfo() : base(ENPClientDataType.MARQUEE)
        {
            _m_readInfoList = new List<Common_MarqueeRecordReadInfo>();
        }

        /// <summary>
        /// 获取初始化需要的记录数据
        /// </summary>
        /// <returns></returns>
        public List<Common_MarqueeShowPosReadInfo> getInitRecordDataList()
        {
            List<Common_MarqueeShowPosReadInfo> infoList = new List<Common_MarqueeShowPosReadInfo>();
            if (_m_readInfoList != null)
            {
                for (int i = 0; i < _m_readInfoList.Count; i++)
                {
                    if (_m_readInfoList[i] != null)
                    {
                        Common_MarqueeShowPosReadInfo temp = new Common_MarqueeShowPosReadInfo(_m_readInfoList[i].getShowPosId(), _m_readInfoList[i].getRecordDbId());
                        infoList.Add(temp);
                    }
                }
            }

            return infoList;
        }

        /// <summary>
        /// 设置跑马灯已读
        /// </summary>
        /// <param name="_showPosId"></param>
        /// <param name="_recordDbId"></param>
        /// <param name="_priorityId"></param>
        /// <param name="_curPriorityHadReadDbId"></param>
        public void setHadReadMarquee(int _showPosId, long _recordDbId, long _priorityId, long _curPriorityHadReadDbId)
        {
            if (_m_readInfoList == null)
                _m_readInfoList = new List<Common_MarqueeRecordReadInfo>();

            //数据是否变更
            bool isChangeData = false;

            //当前位置记录的数据
            Common_MarqueeRecordReadInfo recordReadInfo = null;
            for (int i = 0; i < _m_readInfoList.Count; i++)
            {
                if (_m_readInfoList[i] != null && _m_readInfoList[i].getShowPosId() == _showPosId)
                {
                    recordReadInfo = _m_readInfoList[i];
                    break;
                }
            }

            //数据为空，说明还没记录数据，构建数据
            if (recordReadInfo == null)
            {
                recordReadInfo = new Common_MarqueeRecordReadInfo();
                recordReadInfo.setShowPosId(_showPosId);
                _m_readInfoList.Add(recordReadInfo);
                isChangeData = true;
            }

            //如果记录的请求数据实例id小于新实例id，更新数据
            if (recordReadInfo.getRecordDbId() < _recordDbId)
            {
                recordReadInfo.setRecordDbId(_recordDbId);
                isChangeData = true;
            }

            //记录不同优先级已读数据
            List<Common.Common_MarqueeRecordPriorityReadInfo> recordPriorityReadInfoList = recordReadInfo.getPriorityReadInfoList();
            //当前传入优先级的已读数据
            Common_MarqueeRecordPriorityReadInfo priorityReadInfo = null;
            for (int i = 0; i < recordPriorityReadInfoList.Count; i++)
            {
                if (recordPriorityReadInfoList[i] != null && recordPriorityReadInfoList[i].getPriorityId() == _priorityId)
                {
                    priorityReadInfo = recordPriorityReadInfoList[i];
                    break;
                }
            }

            //数据为空，说明还没记录数据，构建数据
            if (priorityReadInfo == null)
            {
                priorityReadInfo = new Common_MarqueeRecordPriorityReadInfo(_priorityId, _curPriorityHadReadDbId);
                recordReadInfo.addPriorityReadInfoList(priorityReadInfo);
                isChangeData = true;
            }
            else if(priorityReadInfo.getHadReadDbId() < _curPriorityHadReadDbId)
            {
                //如果记录的实例id小于新记录实例id，更新数据
                priorityReadInfo.setHadReadDbId(_curPriorityHadReadDbId);
                isChangeData = true;
            }

            if (isChangeData)
                saveData();
        }

        /// <summary>
        /// 检查数据是否有效可展示
        /// </summary>
        /// <param name="_showPosId"></param>
        /// <param name="_priorityId"></param>
        /// <param name="_dbId"></param>
        public bool checkIsValid(int _showPosId, long _priorityId, long _dbId)
        {
            if (_m_readInfoList == null)
                return true;

            Common_MarqueeRecordReadInfo recordReadInfo = null;
            for (int i = 0; i < _m_readInfoList.Count; i++)
            {
                if (_m_readInfoList[i] != null && _m_readInfoList[i].getShowPosId() == _showPosId)
                {
                    recordReadInfo = _m_readInfoList[i];
                    break;
                }
            }

            if (recordReadInfo == null)
                return true;

            //记录不同优先级已读数据
            List<Common.Common_MarqueeRecordPriorityReadInfo> recordPriorityReadInfoList = recordReadInfo.getPriorityReadInfoList();
            if (recordPriorityReadInfoList == null || recordPriorityReadInfoList.Count == 0)
                return true;

            for (int i = 0; i < recordPriorityReadInfoList.Count; i++)
            {
                if (recordPriorityReadInfoList[i] != null && recordPriorityReadInfoList[i].getPriorityId() == _priorityId)
                    return recordPriorityReadInfoList[i].getHadReadDbId() < _dbId;
            }

            return true;
        }

        protected override byte[] _makeData()
        {
            Common_MarqueeRecordReadInfoList data = new Common_MarqueeRecordReadInfoList(_m_readInfoList);
            return data.makePackage();
        }

        protected override void _readData(byte[] _data)
        {
            Common_MarqueeRecordReadInfoList data = new Common_MarqueeRecordReadInfoList();
            if (_data != null) 
                data.readPackage(_data);

            _m_readInfoList = data.getInfoList();
        }
        
        protected override void _resetRemarkInfo()
        {
            Common_MarqueeRecordReadInfoList data = new Common_MarqueeRecordReadInfoList();
            saveData();
            _readData(data.makePackage());
        }
    }
}