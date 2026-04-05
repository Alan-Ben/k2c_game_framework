using ALPackage;

namespace GOE
{
    public abstract class _ATravelSpecificEventResultWnd<T_Mono, T_EventInfo, T_EventResultInfo> : _ATravelResultWnd<T_Mono, T_EventResultInfo>
        where T_Mono : _ATravelResultMono
        where T_EventInfo : _ATravelEventInfo
        where T_EventResultInfo : _ATravelSpecificEventResultInfo<T_EventInfo>
    {
        public _ATravelSpecificEventResultWnd(T_EventResultInfo _eventResultInfo, long _uiResPathId, EALUIWndLayer _layer) : base(_eventResultInfo, _uiResPathId, _layer)
        {
        }
    }
}