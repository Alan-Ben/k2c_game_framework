using Common.GuildEnum;
using Common.GuildObj;

namespace GOE
{
    public class GuildMarsHelpShow_TechUp : _IGuildMarsHelpShow
    {
        private bool _m_isMyHelp;
        private long _m_senderCid;// 发起玩家CID
        private long _m_techId;// 对象实例ID
        private int _m_objLvl;// 对象等级
        private int _m_dealLimit;// 求助允许处理的次数上限
        private int _m_dealedCount;// 被帮助的次数
        private int _m_dealSecs;// 求助扣除的时长（秒）
        
        public bool isMyHelp => _m_isMyHelp;

        public long senderCid => _m_senderCid;

        public int dealLimit => _m_dealLimit;

        public int dealedCount => _m_dealedCount;
        public GuildMarsHelpShow_TechUp(bool _isMyHelp, long _senderCid, int _dealLimit, int _dealedCount, int _dealSecs, byte[] _extData)
        {
            _m_isMyHelp = _isMyHelp;
            _m_senderCid = _senderCid;
            _m_dealLimit = _dealLimit;
            _m_dealedCount = _dealedCount;
            _m_dealSecs  = _dealSecs;
            Guild_MarsHelp_TechUp techUp = new Guild_MarsHelp_TechUp();
            techUp.readPackage(_extData);
            _m_objLvl = techUp.getLvl();
            _m_techId = techUp.getTechId();
        }
        
        public string getDetailStr()
        {
                MarsTechnologyRefObj technologyRef = GRefdataCoreMgr.instance.marsTechnologyRefCore.getRef(_m_techId);
                string techName = technologyRef?.transName;
                return TextTranslate.instance.getLanguage(TransKeyConst.guild_mars_help_tech_desc, _m_objLvl, techName);
        }

        public string getReduceTimeStr()
        {
            if (isMyHelp)
                return TextTranslate.instance.getLanguage(TransKeyConst.guild_mars_help_reduce_time,TimeUtil.millisecondsToTime_hms(dealedCount * _m_dealSecs * 1000));
            return "";
        }
    }
}