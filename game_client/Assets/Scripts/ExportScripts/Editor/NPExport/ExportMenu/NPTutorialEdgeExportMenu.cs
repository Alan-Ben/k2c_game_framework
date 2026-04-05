using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;


namespace GOE
{
	//第一个类型是模板类,  第二个类型是实际游戏中运用的类,  第三个类型是表的集合类
	public class NPTutorialEdgeExportMenu : NPBasicExportMenuItem<NPTutorialEdgeRef, NPTutorialEdgeRef, NPGSOTutorialEdgeRefSet> {

	    public NPTutorialEdgeExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
	        : base("Tutorial Edge(引导跳转路径)", ENPExportSettingEnum.TUTORIAL_EDGE, NPGSOTutorialEdgeRefSet.assetPath, NPGSOTutorialEdgeRefSet.objName, _tag, _judgeCanShowFunc)
	    {
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return "引导跳转路径(tutorial_edge)"; } }


	    public override List<NPTutorialEdgeRef> _exchangeTemplate (List<NPTutorialEdgeRef> _tempList) {
	        return _tempList;
	    }

	    /*****************
	     * excel文件的读取函数
	     **/
	    public override void _readRefInfo () {
	        obj.id = GetLong("id");
	        obj.start_node = GetString("start_node");
	        obj.end_node = GetString("end_node");
	        obj.res_path_id = GetLong("res_path_id");
	    }
	}
}