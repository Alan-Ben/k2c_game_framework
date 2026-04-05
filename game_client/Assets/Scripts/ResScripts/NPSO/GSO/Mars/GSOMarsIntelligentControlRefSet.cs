using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星智能控制表
	/// </summary>
	[Serializable]
	public class MarsIntelligentControlRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		
		public string name;//决策名
		public string desc;//决策描述
		public List<string> desc_args_list;//决策描述参数列表
		public NPGTextureIndex icon;//决策图标
		public long cooling_time;//冷却时间(秒)
		public long cost_satisfaction_value;//消耗满意值
		public _NPPlayerConditionSerializeInfo unlock_cond;//解锁条件
		public string unlock_cond_unable_tip;//解锁条件未达到提示
		public _NPPlayerConditionSerializeInfo use_cond;//使用条件
		public string use_cond_unable_tip;//使用条件未达到时的提示
		public long buff_id;//关联buff_id
		public long client_show_effect_building_id;//客户端表现效果关联的建筑id

		public long coolingTimeMs { get { return cooling_time * 1000; } }
		public string transName { get { return TextTranslate.instance.getLanguage(name); } }
		public string transDesc { get { return TextTranslate.instance.getLanguage(desc, desc_args_list); } }

#if NP_GAME
		public EMarsIntelligentControlState getState(bool _showTip = false)
		{
			MarsIntelligentControlInfo controlInfo =
				NPPlayer.instance.marsComp.peopleSubComponent.getIntelligentControlInfo(id);
			if(controlInfo != null)
				return controlInfo.getState(_showTip);
			else
			{
				// 有解锁条件, 并且解锁条件不满足
				if (unlock_cond != null && !unlock_cond.isNoConditionOrEnable(null))
				{
					if(_showTip)
						NPGUIAddSceneCenterTip.instance.showTransTextInfo(unlock_cond_unable_tip);
                
					return EMarsIntelligentControlState.LOCK;
				}
				
				if (cannotUseStateCheck(_showTip))
					return EMarsIntelligentControlState.CANNOT_USE;

				return EMarsIntelligentControlState.CAN_USE;
			}
		}
		
		public bool cannotUseStateCheck(bool _showTip = false)
		{
			// 判断是否满足使用条件
			if(use_cond != null && !use_cond.isNoConditionOrEnable(null))
			{
				if (_showTip)
					NPGUIAddSceneCenterTip.instance.showTransTextInfo(use_cond_unable_tip);
                
				return true;
			}
            
			// 判断满意值道具数量是否足够
			NPCommonItem satisfactionValueItem = GRefdataCoreMgr.instance.npGeneral.mars_satisfaction_value_common_item;
			if (satisfactionValueItem != null && !GCommon.isItemEnough(satisfactionValueItem.itemType,
				    satisfactionValueItem.itemId, cost_satisfaction_value, _showTip))
			{
				return true;
			}

			return false;
		}
#endif
	}

	public class GSOMarsIntelligentControlRefSet : _TALSOBasicRefSet<MarsIntelligentControlRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_intelligent_control"; } }
	}
}