using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 妃子星辉技能等级变化信息
    /// </summary>
    public class ConsortHaloSkillLvlChgInfo
    {
        public long halo_skill_id;//星辉技能id
        public int pre_lvl;//上一等级
        public int next_lvl;//下一等级

        public ConsortHaloSkillLvlChgInfo(long _skillId, int _preLvl, int _nextLvl)
        {
            halo_skill_id = _skillId;
            pre_lvl = _preLvl;
            next_lvl = _nextLvl;
        }
    }
    
    /// <summary>
    /// 妃子星辉技能信息
    /// </summary>
    [Serializable]
    public class ConsortHaloSkillInfo
    {
        public long halo_skill_id;//星辉技能id
        public int lvl;//技能等级
        
        [NonSerialized]
        private ConsortHaloSkillRefObj _m_rHaloSkillRefObj;//星辉技能配表数据

        public ConsortHaloSkillRefObj haloSkillRefObj
        {
            get
            {
                if (_m_rHaloSkillRefObj == null || _m_rHaloSkillRefObj.halo_skill_id != halo_skill_id)
                {
#if NP_GAME
                    _m_rHaloSkillRefObj = GRefdataCoreMgr.instance.consortHaloSkillRefCore.getRef(halo_skill_id);
                    if (_m_rHaloSkillRefObj == null)
                    {
                        Debug.LogError($"星辉技能表中找不到halo_skill_id : {halo_skill_id} 的配表数据, 请检查");
                    }
#endif
                }

                return _m_rHaloSkillRefObj;
            }
        }

        private ConsortHaloSkillLvlRefObj _m_rHaloSkillLvlRefObj;//星辉技能等级配表数据

        public ConsortHaloSkillLvlRefObj haloSkillLvlRefObj
        {
            get
            {
                if (_m_rHaloSkillLvlRefObj == null)
                    _m_rHaloSkillLvlRefObj = haloSkillRefObj?.getHaloSkillLvlRefObj(lvl);
                if (_m_rHaloSkillLvlRefObj == null)
                {
                    Debug.LogError($"星辉技能等级表中找不到halo_skill_id : {halo_skill_id} 的 {lvl}级 配表数据, 请检查");
                }
                
                return _m_rHaloSkillLvlRefObj;
            }
        }
        
        /************
        * 读取字符串
        **/
        public static ConsortHaloSkillInfo readFromStr(string _str)
        {
            if (string.IsNullOrEmpty(_str))
            {
                Debug.LogError("ConsortHaloSkillInfo readFromStr _str is null");
                return null;
            }
            
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length < 2)
            {
                Debug.LogError($"ConsortHaloSkillInfo readFromStr strs.Length < 2");
                return null;
            }

            ConsortHaloSkillInfo ret = new ConsortHaloSkillInfo();

            ret.halo_skill_id = ALCommon.ParseLong(strs[0]);
            ret.lvl = ALCommon.ParseInt(strs[1]);

            return ret;
        }

        /************
         * 读取队列
         **/
        public static List<ConsortHaloSkillInfo> readList(string _str)
        {
            List<ConsortHaloSkillInfo> list = new List<ConsortHaloSkillInfo>();
            if (string.IsNullOrEmpty(_str))
                return list;

            string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                ConsortHaloSkillInfo newSkillInfo = ConsortHaloSkillInfo.readFromStr(strs[i]);
                if(null == newSkillInfo)
                    continue;

                list.Add(newSkillInfo);
            }
            return list;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", halo_skill_id, lvl);
        }
        
        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public void ParseFromString(string _str)
        {
            if(string.IsNullOrEmpty(_str))
                return;
            
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);

            if (strs.Length < 2)
            {
                Debug.LogError($"ConsortHaloSkillInfo ParseFromString strs.Length < 2");
                return;
            }

            halo_skill_id = ALCommon.ParseLong(strs[0]);
            lvl = ALCommon.ParseInt(strs[1]);
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static List<ConsortHaloSkillInfo> MakeListFromString(string _str)
        {
            return readList(_str);
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static ConsortHaloSkillInfo[] MakeArrayFromString(string _str)
        {
            return readList(_str).ToArray();
        }
        
        //重载运算符 == 的任何类型还应重载运算符 !=,否则会产生编译错误
        public static bool operator ==(ConsortHaloSkillInfo _a, ConsortHaloSkillInfo _b)
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
            if (_a.halo_skill_id != _b.halo_skill_id)
                return false;
            if (_a.lvl != _b.lvl)
                return false;

            return true;
        }

        public static bool operator !=(ConsortHaloSkillInfo _a, ConsortHaloSkillInfo _b)
        {
            return !(_a == _b);
        }

        public override bool Equals(object obj)
        {
            if (obj is ConsortHaloSkillInfo)
            {
                ConsortHaloSkillInfo item = obj as ConsortHaloSkillInfo;
                // Return true if the fields match:
                if (item.halo_skill_id != halo_skill_id)
                    return false;
                if (item.lvl != lvl)
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
            return halo_skill_id.GetHashCode() ^ lvl.GetHashCode();
        }

        /// <summary>
        /// 获取星辉技能等级变化信息列表
        /// </summary>
        /// <param name="_preHaloSkillInfoList"></param>
        /// <param name="_nextHaloSkillInfoList"></param>
        /// <returns></returns>
        public static List<ConsortHaloSkillLvlChgInfo> getHaloSkillLvlChgInfoList(List<ConsortHaloSkillInfo> _preHaloSkillInfoList, List<ConsortHaloSkillInfo> _nextHaloSkillInfoList)
        {
            List<ConsortHaloSkillLvlChgInfo> resList = new List<ConsortHaloSkillLvlChgInfo>();
            if (_preHaloSkillInfoList == null)
            {
                if (_nextHaloSkillInfoList != null)//上一星辉技能等级列表为空，下一等级不为空时, 代表所有下一级的星辉技能都是新增的, 遍历_nextHaloSkillInfoList列表全部加上, 之前等级为0
                {
                    foreach (ConsortHaloSkillInfo haloSkillInfo in _nextHaloSkillInfoList)
                    {
                        if(haloSkillInfo != null && haloSkillInfo.lvl > 0)//当前等级大于0, 才需要加入
                            resList.Add(new ConsortHaloSkillLvlChgInfo(haloSkillInfo.halo_skill_id, 0, haloSkillInfo.lvl));
                    }
                }
                
                return resList;
            }

            if (_nextHaloSkillInfoList == null)//若下一星辉技能等级列表为空, 代表所有之前的星辉技能都被删除了, 遍历之前的星辉技能等级列表, 当前星辉等级为0
            {
                foreach (ConsortHaloSkillInfo haloSkillInfo in _preHaloSkillInfoList)
                {
                    if(haloSkillInfo != null && haloSkillInfo.lvl > 0)//之前等级大于0, 才需要加入
                        resList.Add(new ConsortHaloSkillLvlChgInfo(haloSkillInfo.halo_skill_id, haloSkillInfo.lvl, 0));
                }

                return resList;
            }
            
            List<ConsortHaloSkillInfo> tmpNextHaloSkillInfoList = new List<ConsortHaloSkillInfo>(_nextHaloSkillInfoList);
            foreach (ConsortHaloSkillInfo preHaloSkillInfo in _preHaloSkillInfoList)//遍历之前的星辉技能等级列表
            {
                if(preHaloSkillInfo == null)
                    continue;
                // 从下一星辉技能等级列表中找到对应的上一星辉技能等级, 并进行移除
                ConsortHaloSkillInfo nowHaloSkillInfo = tmpNextHaloSkillInfoList.FindAndRemove((_refObj) => _refObj != null && _refObj.halo_skill_id == preHaloSkillInfo.halo_skill_id);
                int nowSkillLvl = nowHaloSkillInfo?.lvl ?? 0;
                if (nowSkillLvl <= 0 && preHaloSkillInfo.lvl > 0)//若没有找到 且 之前技能等级大于0, 代表下一等级中该的星辉技能被删除了, 星辉技能等级变为0
                {
                    resList.Add(new ConsortHaloSkillLvlChgInfo(preHaloSkillInfo.halo_skill_id, preHaloSkillInfo.lvl, 0));
                }
                else if(nowSkillLvl != preHaloSkillInfo.lvl)//若找到了, 且等级发生变化, 记录变化信息
                {
                    resList.Add(new ConsortHaloSkillLvlChgInfo(preHaloSkillInfo.halo_skill_id, preHaloSkillInfo.lvl, nowSkillLvl));
                }
            }

            // 因为在上面遍历之前的星辉技能等级列表时, 已经将当前星辉技能等级列表中对应的星辉技能等级移除了, 所以剩下的星辉技能等级都是新增的(若传入的参数同一个列表含有两个相同技能等级数据, 那是传入数据有问题, 外部应该自己避免这种问题, 这里不做处理)
            foreach (ConsortHaloSkillInfo nowHaloSkillInfo in tmpNextHaloSkillInfoList)
            {
                if(nowHaloSkillInfo != null && nowHaloSkillInfo.lvl > 0)//当前等级大于0, 才需要加入
                    resList.Add(new ConsortHaloSkillLvlChgInfo(nowHaloSkillInfo.halo_skill_id, 0, nowHaloSkillInfo.lvl));
            }

            return resList;
        }
    }
}