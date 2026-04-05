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
	public class NPItemAlterRefExportMenu : NPBasicExportMenuItem<ItemAlterRefObj, ItemAlterRefObj, GSOItemAlterRefSet>
	{

	    public NPItemAlterRefExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
	        : base("item_alter", ENPExportSettingEnum.ITEM_ALTER, GSOItemAlterRefSet.assetPath, GSOItemAlterRefSet.objName, _tag, _judgeCanShowFunc)
	    {
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return "Item Alter物品替换使用表"; } }

	    public override List<ItemAlterRefObj> _exchangeTemplate(List<ItemAlterRefObj> _tempList)
	    {
	        return _tempList;
	    }

	    public override void _readRefInfo()
	    {
	        obj.id = GetLong("id");
	        obj.type = (ENPInsteadItemType)GetEnum("type", typeof(ENPInsteadItemType));

	        obj.src_item_type = (NPEnum.ENPItemType)GetEnum("src_item_type", typeof(NPEnum.ENPItemType));
	        obj.src_item_id = GetLong("src_item_id");

	        obj.alter_item_type = (NPEnum.ENPItemType)GetEnum("alter_item_type", typeof(NPEnum.ENPItemType));
	        obj.alter_item_id = GetLong("alter_item_id");
	    }
	}

}