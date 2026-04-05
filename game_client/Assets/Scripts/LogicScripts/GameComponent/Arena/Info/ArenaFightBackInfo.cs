using Common.ArenaObj;

namespace GOE
{
    /// <summary>
    /// 竞技场对手战报信息
    /// </summary>
    public class ArenaFightBackInfo : ArenaBattleReportInfo
    {
        //是否反击
        private bool _m_bHadFightBack;

        /// <summary>
        /// 是否反击
        /// </summary>
        public bool hadFightBack => _m_bHadFightBack;

        public ArenaFightBackInfo(Arena_FightBackInfo _info):base(_info != null ? _info.getDbId() : 0, _info?.getShowInfo() )
        {
            if(_info == null)
                return;

            _m_bHadFightBack = _info.getHadFightBack();
        }
    }
}
