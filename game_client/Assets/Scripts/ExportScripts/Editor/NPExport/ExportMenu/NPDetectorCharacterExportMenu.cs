using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GOE
{
	//非法字符表
	public class NPDetectorCharacterExportMenu : NPBasicExportMenuItem<NPDetectorCharacterRefObj, NPDetectorCharacterRefObj, NPSODetectorCharacterRefSet>
	{
	    public NPDetectorCharacterExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
	        : base("detector_character", ENPExportSettingEnum.DETECTOR_CHARACTER, NPSODetectorCharacterRefSet.assetPath, NPSODetectorCharacterRefSet.objName, _tag, _judgeCanShowFunc)
	    {
	    }

	    protected override string _menuText
	    {
	        get
	        {
	            return "Detector Character 非法字符表(detector_character)";
	        }
	    }

	    public override List<NPDetectorCharacterRefObj> _exchangeTemplate(List<NPDetectorCharacterRefObj> _tempList)
	    {
	        return _tempList;
	    }

	    public override void _readRefInfo()
	    {
	        obj.id = GetLong("id");
	        obj.illegal_character = GetString("illegal_character");
	    }
	}
}