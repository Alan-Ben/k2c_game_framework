using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GOE
{
	//玩家取名非法字符表
	public class NPDetectorPlayerNameExportMenu : NPBasicExportMenuItem<NPDetectorPlayerNameRefObj, NPDetectorPlayerNameRefObj, NPSODetectorPlayerNameRefSet>
	{
	    public NPDetectorPlayerNameExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
	        : base("detector_player_name", ENPExportSettingEnum.DETECTOR_PLAYER_NAME, NPSODetectorPlayerNameRefSet.assetPath, NPSODetectorPlayerNameRefSet.objName, _tag, _judgeCanShowFunc)
	    {
	    }

	    protected override string _menuText
	    {
	        get
	        {
	            return "Detector Player Name 玩家取名非法字符表(detector_player_name)";
	        }
	    }

	    public override List<NPDetectorPlayerNameRefObj> _exchangeTemplate(List<NPDetectorPlayerNameRefObj> _tempList)
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