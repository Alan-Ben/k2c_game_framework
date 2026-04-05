using System.Collections.Generic;
using Common.ClientData;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 联盟建设记录的信息
    /// </summary>
    public class GuildConstructRemarkInfo : _ANPRemarkInfo
    {
        //捐献记录信息
        [NotNull] private GuildConstruct_ClientData _m_constructClientInfo;

        /// <summary>
        /// 捐献记录信息
        /// </summary>
        public GuildConstruct_ClientData constructClientInfo { get { return _m_constructClientInfo; } }

        public GuildConstructRemarkInfo() : base(ENPClientDataType.GUILD_CONSTRUCT)
        {
            _m_constructClientInfo = new GuildConstruct_ClientData();
        }

        protected override byte[] _makeData()
        {
            if (_m_constructClientInfo == null)
                return null;

            return _m_constructClientInfo.makePackage();
        }

        protected override void _readData(byte[] _data)
        {
            if (_m_constructClientInfo == null || _data == null)
                return;

            _m_constructClientInfo.readPackage(_data);
        }

        protected override void _resetRemarkInfo()
        {
            _m_constructClientInfo = new GuildConstruct_ClientData();
            saveData();
        }
        
        /// <summary>
        /// 设置记录
        /// </summary>
        /// <param name="_constructRefId"></param>
        public void setRecord(long _constructRefId)
        {
            //服务器当前日期
            int curDate = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);

            if (_m_constructClientInfo.getDate() != curDate)
            {
                _m_constructClientInfo.getConstructRefIdList()?.Clear();
                _m_constructClientInfo.setDate(curDate);
                _m_constructClientInfo.getConstructRefIdList()?.Add(_constructRefId);
            }
            else
                _m_constructClientInfo.getConstructRefIdList()?.Add(_constructRefId);

            saveData();
        }
    }
}