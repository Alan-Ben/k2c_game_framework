using System;

namespace GOE
{
    /// <summary>
    /// 热更配表导出基类
    /// </summary>
    public abstract class _AHotfixExportMenuItem : BaseTextExportMenuItem
    {
        private string _m_sTag;
        
        protected _AHotfixExportMenuItem(ENPExportSettingEnum _exportEnum, Func<string, string, bool> _judgeCanShowFunc) :
            base(_exportEnum.ToString().ToLowerInvariant(), _exportEnum, _judgeCanShowFunc)
        {
            _m_sTag = _exportEnum.ToString().ToLowerInvariant();
        }
        
        protected override string _menuText { get { return _m_sTag; } }
    }
}