using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用互斥筛选窗口
    /// </summary>
    public abstract class _ATNPGGUIMonoCommonMutexFiiterWnd<T> : _AALBasicUIWndMono
    {
        [ALHeader("可选tabContainer")]
        public NPGGUIMonoCommonFitterContainer tabContainer;

        [ALHeader("筛选类型tab配置列表")]
        public List<NPGGUICommonFitterMono<T>> fitterTabConfigList;
        
        [ALHeader("默认选中类型(当窗口选择了上面未配置的类型时, 会选择默认类型)")]
        public T defaultSelectType;
    }
}