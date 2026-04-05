using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using ALPackage;


namespace GOE
{
	public class NPComfirmItem : ALComfirmItem
	{
	    public NPComfirmItem (string _text, Action _delegate)
	        : base(_text, _delegate) {
	    }

	    /**************
	     * 具体的处理函数
	     */
	    protected override void _dealComfirm()
	    {
	        ALExportDataCore.instance.save();
	        NPInputTabData.instance.save();

	        base._dealComfirm();

	        AssetDatabase.Refresh();//导出后刷新一次
	    }
	}
}