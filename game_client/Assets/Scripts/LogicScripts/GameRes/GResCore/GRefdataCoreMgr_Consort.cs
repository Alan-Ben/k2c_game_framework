using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    //情人相关
    public partial class GRefdataCoreMgr
    {
        private void _initConsort()
        {
            GConsortRefObj consortRefObj = null;
            
            //情人的皮肤
            consortSkinRefCore.dealAllRef((refObj) =>
            {
                if(refObj == null)
                    return;
                consortRefObj = consortRefCore.getRef(refObj.consort_id);
                if (null == consortRefObj)
                    return;
                consortRefObj.addSkinRef(refObj);
            });
            
            consortVoiceGroupRefCore.dealAllRef((refObj) =>
            {
                if(refObj == null)
                    return;
                consortRefObj = consortRefCore.getRef(refObj.consort_id);
                if (null == consortRefObj)
                    return;
                consortRefObj.addConsortVoiceGroupRefObj(refObj);
            });

            // 妃子羁绊等级表按照等级排序
            consortFettersLvlRefCore.refList.Sort((_obj1, _obj2) =>
            {
                if (_obj2 == null)
                    return -1;
                if (_obj1 == null)
                    return 1;
                if (object.ReferenceEquals(_obj1, _obj2))
                    return 0;

                return _obj1.lvl.CompareTo(_obj2.lvl);
            });
            
            
            // 将经营技能按照解锁所需亲密度排序
            consortBusinessSkillRefCore.refList.Sort((_obj1, _obj2) =>
            {
                if (_obj2 == null)
                    return -1;
                if (_obj1 == null)
                    return 1;
                if (object.ReferenceEquals(_obj1, _obj2))
                    return 0;

                if (_obj1.unlock_need_intimacy.CompareTo(_obj2.unlock_need_intimacy) != 0)
                    return _obj1.unlock_need_intimacy.CompareTo(_obj2.unlock_need_intimacy);

                return _obj1.id.CompareTo(_obj2.id);
            });
        }

        #region 皮肤

        /// <summary>
        /// 获得皮肤等级配置
        /// </summary>
        /// <param name="_consortSkinId"></param>
        /// <param name="_lvl"></param>
        /// <returns></returns>
        public GConsortSkinLvlRefObj getSkinLvlRef(long _consortSkinId, int _lvl)
        {
            GConsortSkinLvlRefObj skinLvlRefObj = null;
            for (int i = 0; i < consortSkinLvlRefCore.refList.Count; i++)
            {
                skinLvlRefObj = consortSkinLvlRefCore.refList[i];
                if(null == skinLvlRefObj)
                    continue;
                if (skinLvlRefObj.consort_skin_id == _consortSkinId &&
                    skinLvlRefObj.lvl == _lvl)
                    return skinLvlRefObj;
            }

            return null;
        }

        #endregion

        #region 羁绊技能

        /// <summary>
        /// 获取对应等级的羁绊技能等级配表数据
        /// </summary>
        /// <param name="_level"></param>
        /// <returns></returns>
        public ConsortFettersSkillLvlRefObj getConsortFettersSkillLvlRefObj(long _skillId, int _level)
        {
            ConsortFettersSkillRefObj skillRefObj = GRefdataCoreMgr.instance.consortFettersSkillRefCore.getRef(_skillId);
            if (skillRefObj == null)
            {
                Debug.LogError($"[getConsortFettersLvlRefObj] 找不到 _skillId:{_skillId} 的配表数据");
                return null;
            }

            return getConsortFettersSkillLvlRefObj(skillRefObj, _level);
        }

        public ConsortFettersSkillLvlRefObj getConsortFettersSkillLvlRefObj(ConsortFettersSkillRefObj _skillRefObj, int _level)
        {
            if (_skillRefObj == null)
                return null;
         
            if (_skillRefObj.skillLvlRefList != null)
            {
                for (int i = 0; i < _skillRefObj.skillLvlRefList.Count; i++)
                {
                    ConsortFettersSkillLvlRefObj consortFettersSkillLvlRefObj = _skillRefObj.skillLvlRefList[i];
                    if (consortFettersSkillLvlRefObj != null && consortFettersSkillLvlRefObj.lvl == _level)
                    {
                        return consortFettersSkillLvlRefObj;
                    }
                }
            }
            
            Debug.LogError($"[getConsortFettersLvlRefObj] 无法从 _skillRefObj.fetters_skill_id:{_skillRefObj.fetters_skill_id}的skillLvlRefList列表中找到 _level:{_level} 的配表数据");
            return null;
        }

        #endregion

        #region 加护技能

        /// <summary>
        /// 获取妃子守护技能等级
        /// </summary>
        /// <returns></returns>
        public ConsortBlessSkillLvlRefObj getConsortBlessSkillLvlRefObj(long _skillId, int _level)
        {
            ConsortBlessSkillRefObj consortBlessSkillRefObj = GRefdataCoreMgr.instance.consortBlessSkillRefCore.getRef(_skillId);
            if (consortBlessSkillRefObj == null)
            {
                Debug.LogError($"[getConsortBlessSkillLvlRefObj] 找不到 _skillId:{_skillId} 的配表数据");
                return null;
            }

            return getConsortBlessSkillLvlRefObj(consortBlessSkillRefObj, _level);
        }
        
        /// <summary>
        /// 获取对应等级的妃子守护技能等级配表数据
        /// </summary>
        /// <param name="_skillRefObj"></param>
        /// <param name="_level"></param>
        /// <returns></returns>
        public ConsortBlessSkillLvlRefObj getConsortBlessSkillLvlRefObj(ConsortBlessSkillRefObj _skillRefObj, int _level)
        {
            if (_skillRefObj == null)
                return null;
         
            if (_skillRefObj.skillLvlRefList != null)
            {
                for (int i = 0; i < _skillRefObj.skillLvlRefList.Count; i++)
                {
                    ConsortBlessSkillLvlRefObj consortBlessSkillLvlRefObj = _skillRefObj.skillLvlRefList[i];
                    if (consortBlessSkillLvlRefObj != null && consortBlessSkillLvlRefObj.lvl == _level)
                    {
                        return consortBlessSkillLvlRefObj;
                    }
                }
            }
            
            Debug.LogError($"[getConsortBlessSkillLvlRefObj] 无法从 _skillRefObj.bless_skill_id:{_skillRefObj.bless_skill_id}的skillLvlRefList列表中找到 _level:{_level} 的配表数据");
            return null;
        }

        #endregion

        #region CG

        /// <summary>
        /// 获取妃子cg配表数据
        /// </summary>
        /// <param name="_cgType"></param>
        /// <returns></returns>
        public List<ConsortCGRefObj> getConsortCGRefList(EConsortCGType _cgType)
        {
            List<ConsortCGRefObj> consortCGRefList = new List<ConsortCGRefObj>();
            for (int i = 0; i < consortCGRefCore.refList.Count; i++)
            {
                ConsortCGRefObj consortCGRefObj = consortCGRefCore.refList[i];
                if (consortCGRefObj == null)
                    continue;
                
                if (consortCGRefObj.cg_type == _cgType)
                    consortCGRefList.Add(consortCGRefObj);
            }

            return consortCGRefList;
        }

        public void getConsortCGRefList(EConsortCGType _cgType, List<ConsortCGRefObj> _consortCgRefList)
        {
            if(_consortCgRefList == null)
                return;
            
            _consortCgRefList.Clear();
            for (int i = 0; i < consortCGRefCore.refList.Count; i++)
            {
                ConsortCGRefObj consortCGRefObj = consortCGRefCore.refList[i];
                if (consortCGRefObj == null)
                    continue;
                
                if (consortCGRefObj.cg_type == _cgType)
                    _consortCgRefList.Add(consortCGRefObj);
            }
        }

        #endregion
        
        #region 故事背景

        /// <summary>
        /// 通过故事id获取所有满足生效条件的妃子故事背景配置
        /// </summary>
        /// <param name="_storyId">故事id</param>
        /// <returns></returns>
        public List<ConsortStoryBgRefObj> getConsortStoryBgRefObjList(long _storyId)
        {
            List<ConsortStoryBgRefObj> storyBgRefObjList = new List<ConsortStoryBgRefObj>();
            if (_storyId <= 0 || consortStoryBgRefCore == null || consortStoryBgRefCore.refList == null)
                return storyBgRefObjList;

            for (int i = 0; i < consortStoryBgRefCore.refList.Count; i++)
            {
                ConsortStoryBgRefObj storyBgRefObj = consortStoryBgRefCore.refList[i];
                if (storyBgRefObj == null || storyBgRefObj.story_id != _storyId)
                    continue;

                if (storyBgRefObj.unlock_condition == null || storyBgRefObj.unlock_condition.isNoConditionOrEnable(null))
                    storyBgRefObjList.Add(storyBgRefObj);
            }

            return storyBgRefObjList;
        }

        #endregion

        #region 经营技能

        /// <summary>
        /// 获取亲密度在minIntimacy到maxIntimacy之间的解锁经营技能配表数据
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<ConsortBusinessSkillRefObj> getConsortUnlockBusinessSkillRefListInIntimacyRange(long minIntimacy, long maxIntimacy)
        {
            // 若minIntimacy大于maxIntimacy，则交换两者的值
            if (minIntimacy > maxIntimacy)
            {
                minIntimacy = minIntimacy ^ maxIntimacy;
                maxIntimacy = minIntimacy ^ maxIntimacy;
                minIntimacy = minIntimacy ^ maxIntimacy;
            }
            
            List<ConsortBusinessSkillRefObj> businessSkillRefList = new List<ConsortBusinessSkillRefObj>();
            for (int i = 0; i < consortBusinessSkillRefCore.refList.Count; i++)
            {
                ConsortBusinessSkillRefObj businessSkillRefObj = consortBusinessSkillRefCore.refList[i];
                // 解锁所需亲密度不在指定范围的, 跳过, 因为配表初始化完成后按照解锁所需亲密度排序过, 所以可以这样
                if (businessSkillRefObj == null || businessSkillRefObj.unlock_need_intimacy < minIntimacy || businessSkillRefObj.unlock_need_intimacy > maxIntimacy)
                    continue;
                
                businessSkillRefList.Add(businessSkillRefObj);
            }

            return businessSkillRefList;
        }
        
        #endregion
        
    }
}