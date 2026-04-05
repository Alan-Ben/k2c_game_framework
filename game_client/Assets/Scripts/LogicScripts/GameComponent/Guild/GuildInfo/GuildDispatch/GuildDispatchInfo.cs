using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 联盟派遣信息
    /// </summary>
    public class GuildDispatchInfo
    {
        /// <summary>
        /// 相性类型
        /// </summary>
        private CommonEnum.ESpecAttrType _m_specAttrType;

        /// <summary>
        /// 派遣的大臣列表
        /// </summary>
        private List<Common.GuildObj.Guild_DispatchHeroInfo> _m_lDispatchHeroInfoList;
        
        /// <summary>
        /// 总加成
        /// </summary>
        private int _m_iTotalAddPer;

        public CommonEnum.ESpecAttrType specAttrType { get { return _m_specAttrType; } }
        public List<Common.GuildObj.Guild_DispatchHeroInfo> dispatchHeroInfoList { get { return _m_lDispatchHeroInfoList; } }
        public int totalAddPer { get { return _m_iTotalAddPer; } }
        
        public GuildDispatchInfo(CommonEnum.ESpecAttrType _specAttrType)
        {
            _m_specAttrType = _specAttrType;
        }
        
        /// <summary>
        /// 更新派遣大臣信息
        /// </summary>
        /// <param name="_dispatchHeroList"></param>
        public void updateInfo(List<Common.GuildObj.Guild_DispatchHeroInfo> _dispatchHeroList)
        {
            _m_lDispatchHeroInfoList = _dispatchHeroList;
            
            _m_iTotalAddPer = 0;
            if (_m_lDispatchHeroInfoList != null)
            {
                for (int i = 0; i < _m_lDispatchHeroInfoList.Count; i++)
                {
                    _m_iTotalAddPer += _m_lDispatchHeroInfoList[i]?.getAddPer() ?? 0;
                }
            }
            
            WinMsg.SendMsg(WinMsgType.ON_GUILD_DISPATCH_INFO_CHG, _m_specAttrType);
        }
    }
}