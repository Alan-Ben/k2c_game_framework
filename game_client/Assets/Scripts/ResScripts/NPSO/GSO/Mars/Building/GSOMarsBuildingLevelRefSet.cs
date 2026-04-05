using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星建筑等级
	/// </summary>
	[Serializable]
	public class MarsBuildingLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long group_id;
		public int level;
		public List<long> upgrade_condition_id_list;
		public List<NPCommonCostItem> upgrade_cost_list;
		public long upgrade_time_cost_sec;
		public NPGGoIndex upgrading_res_index;
		public NPGGoIndex upgraded_res_index;
		public long mars_power_value;
		public NPPlayerPropertyModifier player_property;
		public MarsPropertyModifier mars_property;
		public long building_level_cofficient;
		
		/// <summary>
		/// 添加属性到字典
		/// </summary>
		/// <param name="_propertyDict"></param>
		public void addPropertyToCollections(Dictionary<_IPropertyShow, long> _propertyDict, List<_IPropertyShow> _propertyList, bool _containsMarsPower = true)
		{
			dealAllProperty((propertyShow, propertyValue) =>
			{
				if (propertyShow == null)
					return;

				if (_propertyList != null && !_propertyList.Contains(propertyShow))
				{
					_propertyList.Add(propertyShow);
				}

				if (_propertyDict != null)
				{
					_propertyDict.TryGetValue(propertyShow, out long _value);
					_value += propertyValue;
					_propertyDict[propertyShow] = _value;
				}
			}, _containsMarsPower);
		}
		
		public void dealAllProperty(Action<_IPropertyShow, long> _dealAction, bool _containsMarsPower = true)
		{
			if (_dealAction == null)
				return;
#if NP_GAME
			// 处理火星属性
			if (mars_property != null && mars_property.propertyObjList != null)
			{
				foreach (var propertyInfoObj in mars_property.propertyObjList)
				{
					if(propertyInfoObj == null)
						continue;

					_IPropertyShow propertyShow = propertyInfoObj.type.getPropertyShow();
					if(propertyShow == null)
						continue;

					_dealAction(propertyShow, propertyInfoObj.value);
				}
			}

			// 处理玩家属性
			if (player_property != null && player_property.propertyObjList != null)
			{
				foreach (var propertyInfoObj in player_property.propertyObjList)
				{
					if(propertyInfoObj == null)
						continue;

					_IPropertyShow propertyShow = propertyInfoObj.type.getPropertyShow();
					if(propertyShow == null)
						continue;

					_dealAction(propertyShow, propertyInfoObj.value);
				}
			}
#endif
			if (_containsMarsPower)
			{
				// 处理火星实力
				_dealAction(MarsPowerPropertyShow.instance, mars_power_value);		
			}
		}
	}

	public class GSOMarsBuildingLevelRefSet : _TALSOBasicRefSet<MarsBuildingLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_building_level"; } }
	}
}
