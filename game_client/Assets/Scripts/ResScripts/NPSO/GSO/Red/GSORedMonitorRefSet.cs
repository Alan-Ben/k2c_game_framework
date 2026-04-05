using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 红点监听表
	/// </summary>
	[Serializable]
	public class RedMonitorRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long simple_unlock_id;
		public List<string> msg_type_list;


        [NonSerialized]
        private List<WinMsgType> _m_lAddMsgTypeList = null;
        /// <summary>
        /// 客户端需要监听的枚举
        /// </summary>
        public List<WinMsgType> msgTypeList
        {
            get
            {
                if (_m_lAddMsgTypeList == null)
                    _m_lAddMsgTypeList = GCommon.tryEnumParseToWinMsgTypeList(msg_type_list);

                return _m_lAddMsgTypeList;
            }
        }
    }

	public class GSORedMonitorRefSet : _TALSOBasicRefSet<RedMonitorRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
		public static string objName { get { return "red_monitor"; } }
	}
}