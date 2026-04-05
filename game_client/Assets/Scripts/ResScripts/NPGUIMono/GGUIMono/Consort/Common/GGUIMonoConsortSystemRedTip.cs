using ALPackage;

namespace GOE
{
    /// <summary>
    /// 妃子系统红点
    /// </summary>
    public class GGUIMonoConsortSystemRedTip : _AALBasicUIWndMono
    {
        [ALHeader("红点节点id")] 
        public long redParentId;  
        [ALHeader("红点Mono")] 
        public NPGGUIMonoCommonRedTip monoRed;
    }
}