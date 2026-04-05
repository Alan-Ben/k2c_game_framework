using System;
using System.Collections.Generic;

using ALPackage;



namespace GOE
{
	/// <summary>
	/// 仅服务端配表导出txt文件用
	/// </summary>
	public class NPOnlyServerRefExportMenu : BaseTextExportMenuItem
	{
	    //显示文本
	    private string _m_sMenuText;
	    private string _m_sTag;
	    private ENPExportSettingEnum _m_eExportSetting;

	    public NPOnlyServerRefExportMenu(string _tag, ENPExportSettingEnum _settingEnum, string _menuText, Func<string, string, bool> _judgeCanShowFunc)
	        : base(_tag, _settingEnum, _judgeCanShowFunc)
	    {
	        _m_sTag = _tag;
	        _m_eExportSetting = _settingEnum;
	        _m_sMenuText = _menuText;
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return _m_sMenuText; } }
	    protected override void realExportGeneralRefSet()
	    {
		    ALExportDataCore.instance.save();
		    doExportRefSet(ALExportDataCore.instance.getValue(_m_eExportSetting.ToString()), _m_sTag, "");
	    }

        protected override void _exExport()
        {
			realExportGeneralRefSet();
		}
    }
      
}