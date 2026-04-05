using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 妃子形象动作类型
    /// </summary>
    public enum EConsortTdShowAniType
    {
        NONE,
        GIVE_GIFT,//送礼物
        ENTER,//进入
        IDLE,//停留
    }
    
    /// <summary>
    /// 妃子形象展示动画配置
    /// </summary>
    [Serializable]
    public class ConsortTdShowAniConfig
    {
        public EConsortTdShowAniType aniType;//动画类型
        public string aniTag = "";//动画tag

        public ConsortTdShowAniConfig()
        {
        }
        
        public ConsortTdShowAniConfig(EConsortTdShowAniType _aniType, string _aniTag)
        {
            aniType = _aniType;
            aniTag = _aniTag;
        }
        
        /************
        * 读取字符串
        **/
        public static ConsortTdShowAniConfig readFromStr(string _str)
        {
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (strs.Length < 2)
            {
                UnityEngine.Debug.LogError($"[ConsortTdShowAniConfig readFromStr] _str:{_str} 配置错误, 正确格式 EConsortTdShowAniType:aniTag");
                return null;
            }

            ConsortTdShowAniConfig ret = new ConsortTdShowAniConfig();

            ret.aniType = (EConsortTdShowAniType)ALCommon.EnumParse(typeof(EConsortTdShowAniType), strs[0], true);
            ret.aniTag = strs[1];

            return ret;
        }

        /************
         * 读取队列
         **/
        public static List<ConsortTdShowAniConfig> readList(string _str)
        {
            List<ConsortTdShowAniConfig> list = new List<ConsortTdShowAniConfig>();
            if (null == _str || _str.Length <= 0)
                return list;

            string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                ConsortTdShowAniConfig newConfig = ConsortTdShowAniConfig.readFromStr(strs[i]);
                if(null == newConfig)
                    continue;

                list.Add(newConfig);
            }
            return list;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", aniType, aniTag);
        }
        
        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public void ParseFromString(string _str)
        {
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (strs.Length < 2)
            {
                UnityEngine.Debug.LogError($"[ConsortTdShowAniConfig ParseFromString] _str:{_str} 配置错误, 示例: EConsortTdShowAniType:aniTag");
                return;
            }

            this.aniType = (EConsortTdShowAniType)ALCommon.EnumParse(typeof(EConsortTdShowAniType), strs[0], true);
            this.aniTag = strs[1];
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static List<ConsortTdShowAniConfig> MakeListFromString(string _str)
        {
            return readList(_str);
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static ConsortTdShowAniConfig[] MakeArrayFromString(string _str)
        {
            return readList(_str).ToArray();
        }
        
        //重载运算符 == 的任何类型还应重载运算符 !=,否则会产生编译错误
        public static bool operator ==(ConsortTdShowAniConfig _a, ConsortTdShowAniConfig _b)
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
            if (_a.aniType != _b.aniType)
                return false;
            if (_a.aniTag != _b.aniTag)
                return false;

            return true;
        }

        public static bool operator !=(ConsortTdShowAniConfig _a, ConsortTdShowAniConfig _b)
        {
            return !(_a == _b);
        }

        public override bool Equals(object obj)
        {
            if (obj is ConsortTdShowAniConfig)
            {
                ConsortTdShowAniConfig config = obj as ConsortTdShowAniConfig;
                // Return true if the fields match:
                if (config.aniType != aniType)
                    return false;
                if (config.aniTag != aniTag)
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
            return aniType.GetHashCode() ^ aniTag.GetHashCode();
        }
    }
}