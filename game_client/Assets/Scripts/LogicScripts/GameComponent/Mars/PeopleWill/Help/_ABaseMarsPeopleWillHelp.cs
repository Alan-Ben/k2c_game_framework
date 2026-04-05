using Common.MarsEnum;
using Common.MarsObj;

namespace GOE
{
    public abstract class _ABaseMarsPeopleWillHelp : _IMarsPeopleWillHelp
    {
        protected long _m_instanceId;
        protected long _m_refId;
        protected MarsPeopleHelpRefObj _m_refObj;
        protected long _m_npcRefId;
        protected NPNPCRefObj _m_npcRefObj;
        protected bool _m_bIsFinished;//是否已完成
        
        public _ABaseMarsPeopleWillHelp(Mars_Help _serverHelpInfo)
        {
            update(_serverHelpInfo);
        }
        
        public long instanceId { get { return _m_instanceId; } }
        public long refId { get { return _m_refId; } }

        public MarsPeopleHelpRefObj refObj
        {
            get
            {
                if(_m_refObj == null || _m_refObj.id != _m_refId)
                    _m_refObj = GRefdataCoreMgr.instance.marsPeopleHelpRefCore.getRef(_m_refId);
                return _m_refObj;
            }
        }

        public NPNPCRefObj npcRefObj
        {
            get
            {
                if(_m_npcRefObj == null || _m_npcRefObj.id != _m_npcRefId)
                    _m_npcRefObj = GRefdataCoreMgr.instance.npcRefCore.getRef(_m_npcRefId);
                return _m_npcRefObj;
            }
        }

        public EMarsPopularWillHelpState state
        {
            get
            {
                if (_m_bIsFinished)
                    return EMarsPopularWillHelpState.HANDLED;
                else
                    return EMarsPopularWillHelpState.WAIT_HANDLE;
            }
        }

        public void update(Mars_Help _serverHelpInfo)
        {
            if(_serverHelpInfo == null)
                return;
            
            _m_instanceId = _serverHelpInfo.getId();
            _m_refId = _serverHelpInfo.getHelpId();
            _m_npcRefId = _serverHelpInfo.getNpcId();

            _m_bIsFinished = _serverHelpInfo.getIsFinish();

            _onUpdate(_serverHelpInfo);
        }

        public abstract void _onUpdate(Mars_Help _serverHelpInfo);
        
        public abstract void deal();
        
        public static _IMarsPeopleWillHelp getNewInstance(Mars_Help _serverHelpInfo)
        {
            if(_serverHelpInfo == null)
                return null;

            MarsPeopleHelpRefObj refObj = GRefdataCoreMgr.instance.marsPeopleHelpRefCore.getRef(_serverHelpInfo.getHelpId());
            if(refObj == null)
            {
                Debug.LogError_EditorOnly($"[MarsPeopleWillHelp getNewInstance]: 找不到id为{_serverHelpInfo.getHelpId()}的MarsPeopleHelpRefObj配表数据");
                return null;
            }

            switch (refObj.type)
            {
                case EMarsPeopleHelpType.REWARD:
                    return new MarsPeopleWillHelp_Reward(_serverHelpInfo);
                case EMarsPeopleHelpType.CHOICE:
                    return new MarsPeopleWillHelp_Choice(_serverHelpInfo);
                default:
                    Debug.LogError_EditorOnly($"[MarsPeopleWillHelp getNewInstance]: 未实现类型:{refObj.type}的数据创建方式");
                    return null;
            }
        }
    }
}