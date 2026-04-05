using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 系统任务表
	/// </summary>
	[Serializable]
	public class SystemQuestRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public long group_id;//任务组ID
        public int step;//步骤ID
        public string quest_name;//任务标题文本
        public List<string> quest_name_args;//任务标题文本参数列表
        public List<NPCommonCostItem> done_gain_item_list;//完成后奖励物品列表
        public EValueFormatType process_num_format;//进度值格式化显示方式
        public int process_count;//进度目标值
        public _NPPlayerVariableSerializeInfo process_cur_count;//进度当前值 (高级公式)
        public bool is_client_target;//是否客户端判断的目标值，true的情况下服务器会通过客户端提交的消息增加进度
        public List<ClientTriggerMsgInfo> client_trigger_msg;//在is_client_target有效的情况下，客户端监听增加进度的消息
        public List<string> add_msg_type_list;//客户端需要监听的枚举
        public _NPPlayerEffectSerializeInfo go_to;//跳转效果

		[NonSerialized]
        private List<WinMsgType> _m_lAddMsgTypeList = null;
        /// <summary>
        /// 客户端需要监听的枚举
        /// </summary>
        public List<WinMsgType> addMsgTypeList
        {
            get
            {
                if (_m_lAddMsgTypeList == null)
                    _m_lAddMsgTypeList = GCommon.tryEnumParseToWinMsgTypeList(add_msg_type_list);

                return _m_lAddMsgTypeList;
            }
        }
    }

	public class GSOSystemQuestRefSet : _TALSOBasicRefSet<SystemQuestRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/system_quest_refdata.unity3d"; } }
		public static string objName { get { return "system_quest"; } }
	}
}