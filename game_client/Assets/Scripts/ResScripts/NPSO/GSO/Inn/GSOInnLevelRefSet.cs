using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 旅店等级表
	/// </summary>
	[Serializable]
	public class InnLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return level; } }
		public long level; // 等级
		public long need_popularity; // 所需人气值
		public long receive_guest_limit; // 接待客人上限
		public int unlock_guest_num;
		public string name;
		public List<string> name_params;
		public string level_tip;
		public List<string> level_tip_params;
		public string medal_name;
		public List<string> medal_name_params;
		public string medal_desc;
		public List<string> medal_desc_params;
		public NPGTextureIndex medal_icon;
		public long upgrade_dialogue_id; // 升级时播放的对话
		public int star_num;
		public NPGTextureIndex star_icon;
        public NPGGoIndex inn_cash_register_index; // 客栈收银台资源
        public NPGTextureIndex inn_cash_register_icon; // 客栈收银台图标

#if NP_GAME
		
		public string nameTranslated { get { return TextTranslate.instance.getLanguage(name, name_params); } }
		public string medalNameTranslated { get { return TextTranslate.instance.getLanguage(medal_name, medal_name_params); } }
		public string medalDescTranslated { get { return TextTranslate.instance.getLanguage(medal_desc, medal_desc_params); } }
		public string levelTipTranslated { get { return TextTranslate.instance.getLanguage(level_tip, level_tip_params); } }
		
#endif
	}

	public class GSOInnLevelRefSet : _TALSOBasicRefSet<InnLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_level"; } }
	}
}