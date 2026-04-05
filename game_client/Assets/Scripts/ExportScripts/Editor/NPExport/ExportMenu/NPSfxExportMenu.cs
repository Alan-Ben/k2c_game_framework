using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace GOE
{
	public class NPSfxExportMenu : NPBasicExportMenuItem<NPSfxRefObj, NPSfxRefObj, NPSOSfxRefSet>
	{

	    public NPSfxExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
	        : base("Sfx", ENPExportSettingEnum.SFX, NPSOSfxRefSet.assetPath, NPSOSfxRefSet.objName, _tag, _judgeCanShowFunc)
	    {
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return "特效信息 (Sfx)"; } }

	    public override List<NPSfxRefObj> _exchangeTemplate(List<NPSfxRefObj> _tempList)
	    {
	        return _tempList;
	    }

	    public override void _readRefInfo()
	    {
	        obj.id = GetLong("id");
	        obj.duration = GetLong("duration");
	        obj.trigger_event_time = GetLong("trigger_event_time");
	        obj.sfx_index = new NPGSfxIndex();
	        obj.sfx_index.readIndex(GetString("sfx_index"));
	        obj.audio_id = GetList<long>("audio_id");
	        obj.min_cache_count = GetInt("min_cache_count");
	        obj.max_cache_count = GetInt("max_cache_count");
	    }
	}
}