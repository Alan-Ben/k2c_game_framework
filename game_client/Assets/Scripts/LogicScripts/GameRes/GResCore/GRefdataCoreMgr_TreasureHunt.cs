namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        private void _initTreasureHunt()
        {
            treasureHuntOreRefCore?.dealAllRef((_refObj) =>
            {
                if(_refObj == null)
                    return;

                // 对mass_reward_grade_list 字段按照质量从小到大排序
                if (_refObj.mass_reward_grade_list != null)
                {
                    _refObj.mass_reward_grade_list.Sort((_a, _b) =>
                    {
                        if (_b == null) return -1;
                        if (_a == null) return 1;
                        if (object.ReferenceEquals(_a, _b)) return 0;

                        return _a.mass.CompareTo(_b.mass);
                    });
                }
            });
            
            treasureHuntTreasureRefCore?.dealAllRef((_refObj) =>
            {
                if(_refObj == null)
                    return;

                TreasureHuntLabRefObj labRefObj = treasureHuntLabRefCore?.getRef(_refObj.related_lab_id);
                labRefObj?.addTreasure(_refObj);
            });
            
            // 距离从小到大排序
            treasureHuntAreaDistanceRefCore?.refList?.Sort((_a, _b) => _a.distance.CompareTo(_b.distance));
        }
        
        /// <summary>
        /// 太空寻宝 - 获取技能等级配表数据
        /// </summary>
        /// <param name="_skillId"></param>
        /// <param name="_level"></param>
        /// <returns></returns>
        public TreasureHuntSkillLevelRefObj getTreasureHuntSkillLevelRefObj(long _skillId, int _level)
        {
            foreach (TreasureHuntSkillLevelRefObj skillLevelRefObj in treasureHuntSkillLevelRefCore.refList)
            {
                if (skillLevelRefObj != null && skillLevelRefObj.skill_id == _skillId &&
                    skillLevelRefObj.level == _level)
                    return skillLevelRefObj;
            }

            return null;
        }
        
        /// <summary>
        /// 获取第一个解锁的实验室配表数据
        /// </summary>
        /// <returns></returns>
        public TreasureHuntLabRefObj getFirstUnlockTreasureHuntLabRefObj()
        {
            foreach (var labRefObj in treasureHuntLabRefCore.refList)
            {
                if (labRefObj != null && labRefObj.isUnlock())
                    return labRefObj;
            }

            return null;
        }
        /// <summary>
        /// 传入当前距离，获得当前的相关配置
        /// </summary>
        public TreasureHuntAreaDistanceRefObj getTreasureHuntAreaDistanceRefObj(long _distance)
        {
            TreasureHuntAreaDistanceRefObj result = null;
            foreach (TreasureHuntAreaDistanceRefObj refObj in treasureHuntAreaDistanceRefCore.refList)
            {
                if (refObj == null)
                    continue;

                if (refObj.distance > _distance)
                    break;
                
                result = refObj;
            }

            return result;
        }
        /// <summary>
        /// 传入当前距离，获得下一个距离点的相关配置
        /// </summary>
        public TreasureHuntAreaDistanceRefObj getNextTreasureHuntAreaDistanceRefObj(long _distance)
        {
            TreasureHuntAreaDistanceRefObj result = null;
            foreach (TreasureHuntAreaDistanceRefObj refObj in treasureHuntAreaDistanceRefCore.refList)
            {
                if (refObj == null)
                    continue;

                if (refObj.distance > _distance)
                {
                    result = refObj;
                    break;
                }
            }

            return result;
        }
    }
}