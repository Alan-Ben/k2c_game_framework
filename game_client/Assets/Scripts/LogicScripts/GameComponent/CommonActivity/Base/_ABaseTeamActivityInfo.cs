using System;
using GC2GS.p012_ActivityTeamOp;
using GS2GC.p012_ActivityTeamOp;

namespace GOE
{
    /// <summary>
    /// 带组队功能的活动基类
    /// </summary>
    public abstract class _ABaseTeamActivityInfo : _ABaseActivityInfo
    {
        private ActivityTeamInfo _m_activityTeamInfo;

        public ActivityTeamInfo activityTeamInfo
        {
            get { return _m_activityTeamInfo; }
        }

        protected sealed override void _onSubDataInit(Action _doneDelegate)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_003_ReqSelfActivityTeam(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_012_003_RetSelfActivityTeam>((_isSuc, _msg) =>
                {
                    if (_msg != null) 
                        _initTeamInfo(_msg.getTeam());

                    _onSubDataInitEx(_doneDelegate);
                }));
        }

        /// <summary>
        /// 加入队伍（不管是自己创建的还是别人邀请的，都是加入队伍了，外部不需要区分）
        /// </summary>
        /// <param name="_teamInfo"></param>
        public void JoinTeam(Common.CrossTeamObj.CrossTeam_Info _teamInfo)
        {
            _initTeamInfo(_teamInfo);
        }
        
        public void quitTeam()
        {
            _m_activityTeamInfo = null;
        }
        
        public void addTeamMember(Common.CrossTeamObj.CrossTeamMember_Info _teamMemberInfo)
        {
            if (_m_activityTeamInfo == null)
                return;
            
            _m_activityTeamInfo.addMember(_teamMemberInfo);
        }
        
        public void removeTeamMember(long _cid)
        {
            if (_m_activityTeamInfo == null)
                return;
            
            _m_activityTeamInfo.removeMember(_cid);
        } 
        
        //初始化队伍信息
        private void _initTeamInfo(Common.CrossTeamObj.CrossTeam_Info _teamInfo)
        {
            if(null == _teamInfo || _teamInfo.getTeamBase()?.getTeamId() == 0)
                return;
            
            _m_activityTeamInfo = new ActivityTeamInfo(_teamInfo);
        }
        
        protected abstract void _onSubDataInitEx(Action _doneDelegate);
    }
}