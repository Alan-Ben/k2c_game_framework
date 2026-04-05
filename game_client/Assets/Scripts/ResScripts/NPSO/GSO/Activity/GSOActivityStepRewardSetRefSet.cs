using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 阶段奖励设置表
	/// </summary>
	[Serializable]
	public class ActivityStepRewardSetRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id; // 阶段奖励id
		public string step_reward_set_name;// 阶段奖励名称
		// public ERankingEventServerType event_trigger_server; // 事件触发服务器类型
		// public LogicEventType logic_event; // 监控事件枚举（假设枚举类型为LogicEventType）
		// public string trigger_condition; // 触发器-触发条件（条件表达式或枚举需根据实际逻辑调整）
		// public float trigger_count_rate; // 触发器count累计到进度值的倍率
		public _NPPlayerVariableSerializeInfo process_cur_count;//计数器的高级公式(ENPPlayerVariableType)
		// public bool is_set; // 是否设置值（TRUE=覆盖模式/FALSE=增量模式）
		// public bool set_greater; // 是否记录更高值（仅在设置值模式生效）
		public EValueFormatType process_num_format;
		public _NPPlayerEffectSerializeInfo go_to; // 转跳效果（系统场景类型）
		public bool is_show_in_step_reward_wnd;//是否显示在阶段奖励界面

	}

	public class GSOActivityStepRewardSetRefSet : _TALSOBasicRefSet<ActivityStepRewardSetRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
		public static string objName { get { return "step_reward_set"; } }
	}
}