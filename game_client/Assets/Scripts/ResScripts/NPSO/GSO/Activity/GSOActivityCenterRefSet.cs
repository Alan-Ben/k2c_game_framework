using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 活动中心表
	/// </summary>
	[Serializable]
	public class ActivityCenterRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public string name;//名称
        public NPGTextureIndex icon;//图标
        public EActivityCenterTabType type;//页签类型
        public long activity_id;//活动id（仅用于ACTIVITY类型）
        public long sort_id;//排序id
        public _NPPlayerConditionSerializeInfo show_condition;//页签显示条件
        public List<string> add_msg_type_list;//客户端刷新显隐需要监听的枚举数组{WinMsgType}
        public long red_tip_id;//红点id

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

	public class GSOActivityCenterRefSet : _TALSOBasicRefSet<ActivityCenterRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
		public static string objName { get { return "activity_center"; } }
	}
}