using System;
using System.Collections.Generic;

using System.IO;
using UnityEngine;
using UnityEditor;
using ALPackage;
using Excel;



namespace GOE
{
	//第一个类型是模板类,  第二个类型是实际游戏中运用的类,  第三个类型是表的集合类
	public class _TNPAutoExportRefMenu<Tobj, TMap> : NPBasicAutoExportMenuItem<Tobj, TMap> where Tobj : _IALBasicRefObj, new() where TMap : _TALSOBasicRefSet<Tobj>, new()
	{
	    private string _m_sMenuStr;

	    public _TNPAutoExportRefMenu(ENPExportSettingEnum _exprotEnum, string _assetPath, string _objName, string _tag, string _menuText, Func<string, string, bool> _judgeCanShowFunc)
	        : base(_exprotEnum, _assetPath, _objName, _tag, _judgeCanShowFunc)
	    {
	        _m_sMenuStr = _menuText;
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return _m_sMenuStr; } }
	}

}