using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace GOE
{
	public class NPVersionUpRewardExportMenu : NPBasicExportMenuItem<NPGVersionUpRewardRefObj, NPGVersionUpRewardRefObj, NPSOVersionUpRewardRefSet>
	{

	    public NPVersionUpRewardExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
	        : base("version_up_reward", ENPExportSettingEnum.VERSION_UP_REWARD, NPSOVersionUpRewardRefSet.assetPath, NPSOVersionUpRewardRefSet.objName, _tag, _judgeCanShowFunc)
	    {
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return "版本更新奖励信息(VersionUpReward)"; } }

	    public override List<NPGVersionUpRewardRefObj> _exchangeTemplate(List<NPGVersionUpRewardRefObj> _tempList)
	    {
	        return _tempList;
	    }

	    public override void _readRefInfo()
	    {
	        obj.div_num = GetLong("div_num");
	        obj.reward_list = NPCommonCostItem.readList(GetString("reward_list"));
	    }
	}
}