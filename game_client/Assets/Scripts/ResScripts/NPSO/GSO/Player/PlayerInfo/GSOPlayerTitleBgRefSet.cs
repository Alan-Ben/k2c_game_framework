using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 玩家组合称号底框表
	/// </summary>
	[Serializable]
	public class PlayerTitleBgRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public long asset_path_id;//prefab路径
        public Color text_color;//选中该底框时文本颜色
        public _NPPlayerConditionSerializeInfo unlock_condition;//解锁条件
        public List<string> add_msg_type_list;//客户端需要监听的枚举数组{WinMsgType}
    }

	public class GSOPlayerTitleBgRefSet : _TALSOBasicRefSet<PlayerTitleBgRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
		public static string objName { get { return "player_title_bg"; } }
	}
}