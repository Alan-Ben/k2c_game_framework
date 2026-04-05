using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
	/// <summary>
	/// 活动配表导出txt文件用
	/// </summary>
	public class ActivityRefExportMenu : BaseTextExportMenuItem
	{
	    //显示文本
	    private string _m_sMenuText;
	    private List<string> _m_lExcelSheetList;
	    private ENPExportSettingEnum _m_eExportSetting;
        private string _m_sAssetPath = NPABString.C_HotfixRefdataPath;

        public ActivityRefExportMenu(string _menuText, ENPExportSettingEnum _settingEnum, Func<string, string, bool> _judgeCanShowFunc, params string[] _params)
	        : base(_settingEnum.ToString().ToLowerInvariant(), _settingEnum, _judgeCanShowFunc)
        {
	        _m_sMenuText = _menuText;
	        _m_eExportSetting = _settingEnum;
            _m_lExcelSheetList = new List<string>();
			if(_params != null)
                _m_lExcelSheetList.AddRange(_params);
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return _m_sMenuText; } }
	    protected override void realExportGeneralRefSet()
	    {
		    ALExportDataCore.instance.save();

            if (_m_lExcelSheetList != null)
            {
                for (int i = 0; i < _m_lExcelSheetList.Count; i++)
                {
                    doExportRefSet(ALExportDataCore.instance.getValue(_m_eExportSetting.ToString()), _m_lExcelSheetList[i], _m_sAssetPath);
                }
            }
	    }

        protected override void _exExport()
        {
			realExportGeneralRefSet();
		}
    }
}