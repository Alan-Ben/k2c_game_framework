
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class PlayerHeroUnlockShowRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id; // 唯一 id 也决定默认的列表顺序
        public long hero_id;
        public _NPPlayerConditionSerializeInfo unlock_condition;
        public _NPPlayerConditionSerializeInfo get_condition;//可领取条件
        public List<string> refresh_get_cond_msg_list;//刷新可领取条件消息列表
        public string unlock_desc;
        public List<string> unlock_desc_params;
        public _NPPlayerEffectSerializeInfo jump_effect;

        [NonSerialized]
        private List<WinMsgType> _m_lRefreshMsgList = null;
        /// <summary>
        /// 客户端需要监听的枚举
        /// </summary>
        public List<WinMsgType> refreshMsgTypeList
        {
            get
            {
                if (_m_lRefreshMsgList == null)
                    _m_lRefreshMsgList = GCommon.tryEnumParseToWinMsgTypeList(refresh_get_cond_msg_list);

                return _m_lRefreshMsgList;
            }
        }
    }

    public class GSOPlayerHeroUnlockShowRefSet : _TALSOBasicRefSet<PlayerHeroUnlockShowRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
        public static string objName { get { return "player_hero_unlock_show"; } }
    }
}
