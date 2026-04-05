using JetBrains.Annotations;
using RemarkData;

namespace GOE
{
    /// <summary>
    /// 玩家升级记录数据
    /// </summary>
    public class PlayerUpgradeRemarkInfo:_ANPRemarkInfo
    {
        /// <summary>
        /// 玩家升级记录数据
        /// </summary>
        [NotNull] private PlayerUpgradeData _m_clientInfo;

        public PlayerUpgradeRemarkInfo() : base(ENPClientDataType.PLAYER_UPGRADE)
        {
            _m_clientInfo = new PlayerUpgradeData();
        }
        
        /// <summary>
        /// 设置展示过的升级弹窗等级
        /// </summary>
        /// <param name="_level"></param>
        public void setIsShowUpgradeWndLevel(long _level)
        {
            _m_clientInfo.setAlreadyShowUpgradeWndLevel(_level);
            //保存数据
            saveData();
        }

        protected override byte[] _makeData()
        {
            return _m_clientInfo.makePackage();
        }

        protected override void _readData(byte[] _data)
        {
            if (_m_clientInfo != null && null != _data) 
                _m_clientInfo.readPackage(_data);
        }

        protected override void _resetRemarkInfo()
        {
            _m_clientInfo = new PlayerUpgradeData();
            saveData();
        }
    }
}