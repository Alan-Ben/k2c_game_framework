using ALPackage;

namespace GOE
{
    public abstract class _ATNPGGUIMonoRefreshGrid<T> : _ATNPGGUIMonoShowAnimGrid<T> where T : _TALUGUIMonoGridItem
    {
        [ALHeader("滚动条监控")]
        public NPGGUIMonoScrollRefreshMonitor monoRefreshMonitor;
    }
}
