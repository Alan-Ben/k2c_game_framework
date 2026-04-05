using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public enum ETravelEventRoleType
    {
        NONE,
        NPC,//npc
        CONSORT,//妃子
    }
    
    /// <summary>
    /// 游历事件角色
    /// </summary>
    [Serializable]
    public class TravelEventRoleConfig
    {
        [SerializeField]
        private ETravelEventRoleType _m_eRoleType;
        [SerializeField]
        private long _m_lId;

#if NP_GAME
        private _ITravelEventRole _m_travelEventRole;
#endif
            
        public TravelEventRoleConfig()
        {
        }
        
        public TravelEventRoleConfig(ETravelEventRoleType roleType, long _id)
        {
            _m_eRoleType = roleType;
            _m_lId = _id;
        }

        public ETravelEventRoleType roleType { get => _m_eRoleType; }
        public long id { get => _m_lId; }

#if NP_GAME
        public _ITravelEventRole roleInfo
        {
            get
            {
                if (_m_travelEventRole == null || _m_travelEventRole.roleType != _m_eRoleType ||
                    _m_travelEventRole.id != _m_lId)
                {
                    switch (_m_eRoleType)
                    {
                        case ETravelEventRoleType.NPC:
                            _m_travelEventRole = new TravelEventNpcRole(_m_lId);
                            break;
                        
                        case ETravelEventRoleType.CONSORT:
                            _m_travelEventRole = new TravelEventConsortRole(_m_lId);
                            break;
                        
                        default:
                            _m_travelEventRole = null;
                            break;
                    }
                }

                return _m_travelEventRole;
            }
        }
#endif
        
#if NP_GAME   
        /// <summary>
        /// 获取角色名
        /// </summary>
        /// <returns></returns>
        public string getRoleName()
        {
            switch (_m_eRoleType)
            {
                case ETravelEventRoleType.NPC:
                    NPNPCRefObj npcRefObj = GRefdataCoreMgr.instance.npcRefCore.getRef(_m_lId);
                    return npcRefObj?.npcName;
                
                case ETravelEventRoleType.CONSORT:
                    return GCommon.getItemName(ENPItemType.CONSORT, _m_lId);
                
                default:
                    return string.Empty;
            }
        }
#endif
        
        /************
         * 读取字符串
         **/
        public static TravelEventRoleConfig readFromStr(string _str)
        {
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);

            if (strs.Length < 2)
            {
                UnityEngine.Debug.LogWarning($"配置错误! 配置方式: ETravelEventRoleType:id, 当前配置:{_str}");
                return null;
            }

            TravelEventRoleConfig ret = new TravelEventRoleConfig();

            ret._m_eRoleType = (ETravelEventRoleType)ALCommon.EnumParse(typeof(ETravelEventRoleType), strs[0], true);
            ret._m_lId = long.Parse(strs[1]);

            return ret;
        }

        /************
         * 读取队列
         **/
        public static List<TravelEventRoleConfig> readList(string _str)
        {
            List<TravelEventRoleConfig> list = new List<TravelEventRoleConfig>();
            if (null == _str || _str.Length <= 0)
                return list;

            string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                TravelEventRoleConfig newItem = TravelEventRoleConfig.readFromStr(strs[i]);
                if(null == newItem)
                    continue;

                list.Add(newItem);
            }
            return list;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", _m_eRoleType, _m_lId);
        }
        
        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public void ParseFromString(string _str)
        {
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

            if (strs.Length < 1)
            {
                UnityEngine.Debug.LogWarning("没有配置 对象名!");
                return;
            }

            this._m_eRoleType = (ETravelEventRoleType)ALCommon.EnumParse(typeof(ETravelEventRoleType), strs[0], true);
            this._m_lId = long.Parse(strs[1]);
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static List<TravelEventRoleConfig> MakeListFromString(string _str)
        {
            return readList(_str);
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static TravelEventRoleConfig[] MakeArrayFromString(string _str)
        {
            return readList(_str).ToArray();
        }
        
        //重载运算符 == 的任何类型还应重载运算符 !=,否则会产生编译错误
        public static bool operator ==(TravelEventRoleConfig _a, TravelEventRoleConfig _b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(_a, _b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)_a == null) || ((object)_b == null))
            {
                return false;
            }

            // Return true if the fields match:
            if (_a._m_eRoleType != _b._m_eRoleType)
                return false;
            if (_a._m_lId != _b._m_lId)
                return false;

            return true;
        }

        public static bool operator !=(TravelEventRoleConfig _a, TravelEventRoleConfig _b)
        {
            return !(_a == _b);
        }

        public override bool Equals(object obj)
        {
            if (obj is TravelEventRoleConfig)
            {
                TravelEventRoleConfig item = obj as TravelEventRoleConfig;
                // Return true if the fields match:
                if (item._m_eRoleType != _m_eRoleType)
                    return false;
                if (item._m_lId != _m_lId)
                    return false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return _m_eRoleType.GetHashCode() ^ _m_lId.GetHashCode();
        }
        
        /// <summary>
        /// 获取一个唯一识别的id
        /// </summary>
        /// <returns></returns>
        public long getId()
        {
            return (int)_m_eRoleType * 100000 + _m_lId;
        }
    }
}