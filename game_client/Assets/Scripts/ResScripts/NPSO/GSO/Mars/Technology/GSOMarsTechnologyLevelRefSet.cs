using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 火星科技等级表
	/// </summary>
	[Serializable]
	public class MarsTechnologyLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;//唯一id
		
		public long technology_id;//科技id
		public int level;//科技等级
		public List<long> condition_id_list;//升级的条件列表
		public List<NPCommonCostItem> upgrade_consume_list;//升级消耗
		public long upgrade_time_sec;//升级时间(秒)
		public MarsPropertyModifier mars_property;//火星属性加成
		public NPPlayerPropertyModifier player_property;//玩家属性加成
		public long mars_power;//火星实力加成

		/// <summary>
		/// 升级需要实际时间毫秒秒), 经过科技等其他加成加速后的时间
		/// </summary>
		public long upgradeRealTimeMs
		{
			get
			{
#if NP_GAME
				long property = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_TECH_SPEED_PER);
				long upgradeMs = upgrade_time_sec * 1000 * 10000 / (10000 + property);
				upgradeMs = Math.Max(upgradeMs, 0);//保底为0
				
				return upgradeMs;
#endif
				return upgrade_time_sec;
			}
		}
		
		/// <summary>
		/// 升级条件是否满足
		/// </summary>
		/// <returns></returns>
		public bool upgradeConditionEnable(bool _showTip = false)
		{
			if (condition_id_list == null)
				return true;
#if NP_GAME
			foreach (var conditionId in condition_id_list)
			{
				MarsBuildingConditionRefObj conditionRefObj = GRefdataCoreMgr.instance.marsBuildingConditionRefCore.getRef(conditionId);
				if(conditionRefObj != null && conditionRefObj.condition != null && !conditionRefObj.condition.isNoConditionOrEnable(null))
				{
					if(_showTip)
						NPGUIAddSceneCenterTip.instance.showTextInfo(conditionRefObj.conditionDesc);
					
					return false;
				}
			}
#endif			

			return true;
		}

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
				_dealAction(MarsPowerPropertyShow.instance, mars_power);		
			}
		}
		
		/// <summary>
		/// 获取升级消耗列表
		/// </summary>
		public void getRealConsumeList(List<NPCommonCostItem> _costItemList)
		{
			if(_costItemList == null)
				return;

			if (upgrade_consume_list == null)
			{
				_costItemList.Clear();
				return;
			}

#if NP_GAME
			// 计算真实消耗
			long costProperty = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_TECH_COST_PER);
			int i = 0;
			int count = upgrade_consume_list.Count;
			int realItemCount = 0;
			for (;i < count; ++i)
			{
				NPCommonCostItem commonCostItem = upgrade_consume_list[i];
				if(commonCostItem == null)
					continue;

				long realCostCount = commonCostItem.count * (10000 - costProperty) / 10000;
				if (realCostCount < 1)
					realCostCount = 1;

				if (realItemCount >= _costItemList.Count)
				{
					_costItemList.Add(new NPCommonCostItem(commonCostItem.item, realCostCount));
				}
				else
				{
					NPCommonCostItem tmpCostItem = _costItemList[realItemCount];
					if (tmpCostItem == null)
					{
						tmpCostItem = new NPCommonCostItem(commonCostItem.item, realCostCount);
						_costItemList[realItemCount] = tmpCostItem;
					}
					else
					{
						tmpCostItem.item = commonCostItem.item;
						tmpCostItem.count = realCostCount;
					}
				}

				realItemCount++;
			}
#endif
		}

		public List<NPCommonCostItem> getRealConsumeList()
		{
			List<NPCommonCostItem> realConsumeList = new List<NPCommonCostItem>();
			getRealConsumeList(realConsumeList);
			return realConsumeList;
		}
	}

	public class GSOMarsTechnologyLevelRefSet : _TALSOBasicRefSet<MarsTechnologyLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_technology_level"; } }
	}
}
