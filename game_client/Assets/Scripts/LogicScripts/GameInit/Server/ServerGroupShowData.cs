using System.Collections.Generic;

namespace GOE
{
    public class ServerGroupShowData
    {
        //是否是自己的服务器列表
        private bool _m_bIsSelf;
        //服务器组名称
        private string _m_sGroupName;
        //服务器列表
        private List<ServerShowData> _m_serverList;
        //最大的服务器id，用于排序
        private int _m_iMaxServerId;

        /// <summary>
        /// 是否是自己的服务器分组
        /// </summary>
        public bool isSelf
        {
            get { return _m_bIsSelf; }
            set { _m_bIsSelf = value; }
        }

        /// <summary>
        /// 组名称
        /// </summary>
        public string groupName
        {
            get { return TextTranslate.instance.getLanguage(_m_sGroupName); }
            set { _m_sGroupName = value; }
        }

        /// <summary>
        /// 当前服务器组最大服务器id
        /// </summary>
        public int maxServerId
        {
            get { return _m_iMaxServerId; }
        }

        /// <summary>
        /// 添加服务器信息
        /// </summary>
        /// <param name="_data"></param>
        public void addSeverData(ServerShowData _data)
        {
            if (_data == null || _data.serverDataInfo == null)
                return;

            if (_m_serverList == null)
                _m_serverList = new List<ServerShowData>();

            if (_m_iMaxServerId < _data.serverDataInfo.serverId)
                _m_iMaxServerId = _data.serverDataInfo.serverId;

            _m_serverList.Add(_data);
        }

        /// <summary>
        /// 添加服务器列表信息
        /// </summary>
        /// <param name="_dataList"></param>
        public void addSeverDataList(List<ServerShowData> _dataList)
        {
            if (_dataList == null)
                return;

            if (_m_serverList == null)
                _m_serverList = new List<ServerShowData>();

            for (int i = 0; i < _dataList.Count; i++)
            {
                if (_dataList[i] == null || _dataList[i].serverDataInfo == null)
                    continue;

                if (_m_iMaxServerId < _dataList[i].serverDataInfo.serverId)
                    _m_iMaxServerId = _dataList[i].serverDataInfo.serverId;
            }

            _m_serverList.AddRange(_dataList);
        }

        /// <summary>
        /// 获取服务器列表信息
        /// </summary>
        /// <returns></returns>
        public List<ServerShowData> getServerList()
        {
            if (_m_serverList == null)
                return new List<ServerShowData>();

            _m_serverList.Sort(sortSeverList);
            return _m_serverList;
        }

        //服务器列表排序，id从大到小
        private int sortSeverList(ServerShowData _a, ServerShowData _b)
        {
            if (_a == null || _a.serverDataInfo == null || _b == null || _b.serverDataInfo == null)
                return 0;

            int compServerId = _a.serverDataInfo.serverId.CompareTo(_b.serverDataInfo.serverId);
            if (compServerId != 0)
                return -compServerId;
            else
                return 0;
        }
    }
}