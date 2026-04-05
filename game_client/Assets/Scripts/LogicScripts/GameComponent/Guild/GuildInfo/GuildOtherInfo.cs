using System;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 其他联盟信息
    /// </summary>
    public class GuildOtherInfo : GuildBaseInfo
    {
        public GuildOtherInfo(GS2GC_032_004_RetOtherGuildInfo _msg) : base(_msg?.getShowInfo(), _msg?.getMemberList())
        {
        }

        /// <summary>
        /// 是否正在申请加入
        /// </summary>
        /// <returns></returns>
        public bool isApplying { get { return NPPlayer.instance.guildComp.isApplyJoinGuild(guildId); } }

        private bool _m_bIsReqGuildInfo;
        private Action _m_aOnReReqGuildInfoDone;
        /// <summary>
        /// 重新请求联盟数据
        /// </summary>
        /// <param name="_done"></param>
        public void reReqGuildInfo(Action _done)
        {
            _m_aOnReReqGuildInfoDone += _done;
            if (_m_bIsReqGuildInfo)
                return;
            
            _m_bIsReqGuildInfo = true;
            NPPlayer.instance.guildComp.reqOtherGuildInfo(guildId, (_msg) =>
            {
                _m_bIsReqGuildInfo = false;

                Action action = _m_aOnReReqGuildInfoDone;
                _m_aOnReReqGuildInfoDone = null;
                
                if (_msg == null)
                {
                    
                    action?.Invoke();
                    return;
                }
                
                updateShowInfo(_msg.getShowInfo());
                updateMemberBaseInfoList(_msg.getMemberList());
                
                action?.Invoke();
            }, () =>
            {
                _m_bIsReqGuildInfo = false;

                Action action = _m_aOnReReqGuildInfoDone;
                _m_aOnReReqGuildInfoDone = null;
                action?.Invoke();
            });
        }
    }
}