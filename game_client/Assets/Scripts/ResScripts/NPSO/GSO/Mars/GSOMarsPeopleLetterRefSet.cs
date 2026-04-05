using ALPackage;
using System;
using SQLite4Unity3d;

namespace GOE
{
	/// <summary>
	/// 火星人民信件表
	/// </summary>
	[Serializable]
	public class MarsPeopleLetterRefObj
	{
		public long id;//唯一id
		public string content;//信件内容
		public bool is_complain;//是否为抱怨
		public bool need_deal;//是否需要处理
		public string go_to_effect;//前往效果
		private _NPPlayerEffectSerializeInfo _m_go_to_effect;
		public string resolve_cond;//处理完成条件
		private _NPPlayerConditionSerializeInfo _m_resolve_cond;
		public int deal_satisfaction_change;//处理完成后的满意度变化

		public long Id { get { return id; } set { id = value; } }
		public string Content { get { return content; } set { content = value; } }
		public bool IsComplain { get { return is_complain; } set { is_complain = value; } }
		public bool NeedDeal { get { return need_deal; } set { need_deal = value; } }
		public string GoToEffect { get { return go_to_effect; } set { go_to_effect = value; } }
		public string ResolveCond { get { return resolve_cond; } set { resolve_cond = value; } }
		public int DealSatisfactionChange { get { return deal_satisfaction_change; } set { deal_satisfaction_change = value; } }
		
		[Ignore]
		public _NPPlayerEffectSerializeInfo goToEffect
		{
			get
			{
				if (_m_go_to_effect == null)
					_m_go_to_effect = _NPPlayerEffectSerializeInfo.ReadFromString(go_to_effect);
				return _m_go_to_effect;
			}
		}
		
		[Ignore]
		public _NPPlayerConditionSerializeInfo resolveCond
		{
			get
			{
				if (_m_resolve_cond == null)
					_m_resolve_cond = _NPPlayerConditionSerializeInfo.ReadFromString(resolve_cond);
				return _m_resolve_cond;
			}
		}
        
		public static string assetPath { get { return "refdata_db/mars.unity3d"; } }
		public static string objName { get { return "refdata_db/mars_people_letter.txt"; } }
		public static string tableName { get { return "mars_people_letter"; } }
	}
}