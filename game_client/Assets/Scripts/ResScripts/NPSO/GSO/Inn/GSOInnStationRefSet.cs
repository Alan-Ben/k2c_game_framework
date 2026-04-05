using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 旅店设施表
	/// </summary>
	[Serializable]
	public class InnStationRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long need_receive_guest_num; // 所需接待客人数量
		public NPCommonCostItem build_cost; // 建造消耗
		public string name; // 名字
		public string desc; // 描述
		public NPGTextureIndex station_tex; // 设施大图
        public NPGTextureIndex icon; // 图标
		public NPGGoIndex td_res_index; // td 资源索引
		public NPGGoIndex unbuilt_td_res_index; // 未建造的 td 资源索引
		public long build_sfx_id; // 建造特效
		public float build_sfx_delay; // 建造特效延迟
		public string unlock_tip;
		public List<string> unlock_tip_params;

		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		public List<long> dish_id_list;


#if NP_GAME
		
		public string unlockTipTranslated { get { return TextTranslate.instance.getLanguage(unlock_tip, unlock_tip_params); } }
		
#endif
	}

	public class GSOInnStationRefSet : _TALSOBasicRefSet<InnStationRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_station"; } }
	}
}