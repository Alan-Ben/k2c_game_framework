using ALPackage;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
	/// <summary>
	/// 火星科技表
	/// </summary>
	[Serializable]
	public class MarsTechnologyRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;//科技id
		
		public EMarsTechnologyType type;//科技类型
		public List<long> parent_list;//父节点列表
		public NPGTextureIndex icon;//科技图标
		public string name;//科技名称
		public List<string> name_args;//名称参数
		public string desc;//科技描述
		public List<string> desc_args;//描述参数
		public int max_level;//最高等级
		public int skill_type_id;//技能类型
		
		////父节点对象列表
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		[NotNull] public List<MarsTechnologyRefObj> parentTechnologyList = new List<MarsTechnologyRefObj>();
		
		//子节点对象列表
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		[NotNull] public List<MarsTechnologyRefObj> childTechnologyList = new List<MarsTechnologyRefObj>();

		//科技层级
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		public int layer;

		private string _m_sTransName;
		public string transName
		{
			get
			{
				if (!string.IsNullOrEmpty(_m_sTransName))
					return _m_sTransName;
				
				return TextTranslate.instance.getLanguage(name, name_args);
			}
		}
		
		private string _m_sTransDesc;
		public string transDesc
		{
			get
			{
				if (!string.IsNullOrEmpty(_m_sTransDesc))
					return _m_sTransDesc;
				
				return TextTranslate.instance.getLanguage(desc, desc_args); 
			}
		}
#if NP_GAME
		public EMarsTechnologyState technologyState
		{
			get
			{
				// 获取0级配表数据
				MarsTechnologyLevelRefObj lvl0RefObj = GRefdataCoreMgr.instance.getMarsTechnologyLevelRefObj(id, 0);
				if (lvl0RefObj == null)
					return EMarsTechnologyState.NONE;
				
				if (!lvl0RefObj.upgradeConditionEnable() // 0级升级条件未达成
				    || !MarsUtil.checkTechnologyReachLevel(parent_list, 1) // 前置科技等级未达成
				    )
				{
					return EMarsTechnologyState.LOCK;// 有升级条件未达成, 则为LOCK状态(自身为0级, 升级条件又未达成)
				}

				// 到这里说明0级升级条件已达成, 则为LEVEL_0_CONDITION_REACH状态
				return EMarsTechnologyState.LEVEL_0_CONDITION_REACH;
			}
		}
#endif
	}

	public class GSOMarsTechnologyRefSet : _TALSOBasicRefSet<MarsTechnologyRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_technology"; } }
	}
}
