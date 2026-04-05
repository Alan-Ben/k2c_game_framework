using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能数据管理器
    /// </summary>
    public class HeroStarSkillInfoMgr
    {
        //伙伴觉醒数据列表
        [NotNull] private List<HeroStarSkillInfo> _m_lStarSkillList = new List<HeroStarSkillInfo>();

        /// <summary>
        /// 伙伴觉醒数据列表
        /// </summary>
        [NotNull] public List<HeroStarSkillInfo> starSkillList { get { return _m_lStarSkillList; } }

        public HeroStarSkillInfoMgr()
        {
        }

        /// <summary>
        /// 初始化觉醒技能列表
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_star"></param>
        public void initStarSkillList(long _heroId, long _star)
        {
            _m_lStarSkillList.Clear();
            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_heroId);
            if (heroRef == null)
                return;

            List<long> starSkillIdList = heroRef.star_skill_id_list;
            if (starSkillIdList != null)
            {
                for (int i = 0; i < starSkillIdList.Count; i++)
                {
                    HeroStarSkillInfo starSkillInfo = new HeroStarSkillInfo(_heroId, starSkillIdList[i], _star);
                    _m_lStarSkillList.Add(starSkillInfo);
                    //累加属性
                    HeroStarSkillLevelRefObj skillLevelRef = starSkillInfo.curStarSkillLevelRefObj;
                    if (skillLevelRef != null)
                    {
                        if(skillLevelRef.union_bonus != null)
                            NPPlayer.instance.heroComponent.commonUnionBonusMgr.addBonus(skillLevelRef.union_bonus.unionBonus);
                        if(skillLevelRef.add_player != null)
                            NPPlayer.instance.heroComponent.playerPropertyContainer.addModifier(skillLevelRef.add_player);
                    }
                }
            }
        }

        /// <summary>
        /// 更新觉醒技能数据
        /// </summary>
        /// <param name="_star"></param>
        public void updateStar(int _star)
        {
            if (_star <= 0)
                return;

            for (int i = 0; i < _m_lStarSkillList.Count; i++)
            {
                if (_m_lStarSkillList[i] != null)
                {
                    HeroStarSkillLevelRefObj oriLevelRef = _m_lStarSkillList[i].curStarSkillLevelRefObj;
                    _m_lStarSkillList[i].updateStar(_star);
                    HeroStarSkillLevelRefObj curLevelRef = _m_lStarSkillList[i].curStarSkillLevelRefObj;
                    NPPlayer.instance.heroComponent.commonUnionBonusMgr.replaceBonus(oriLevelRef?.union_bonus?.unionBonus, curLevelRef?.union_bonus?.unionBonus);

                    //更新玩家属性
                    if(oriLevelRef != null && oriLevelRef.add_player != null)
                        NPPlayer.instance.heroComponent.playerPropertyContainer.removeModifier(oriLevelRef.add_player);
                    if(curLevelRef != null && curLevelRef.add_player != null)
                        NPPlayer.instance.heroComponent.playerPropertyContainer.addModifier(curLevelRef.add_player);
                }
            }
        }

        /// <summary>
        /// 获取伙伴觉醒技能数据
        /// </summary>
        /// <param name="_starSkillId"></param>
        /// <returns></returns>
        public HeroStarSkillInfo getStarSkillInfo(long _starSkillId)
        {
            for (int i = 0; i < _m_lStarSkillList.Count; i++)
            {
                if (_m_lStarSkillList[i] != null && _m_lStarSkillList[i].starSkillId == _starSkillId)
                {
                    return _m_lStarSkillList[i];
                }
            }

            return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _m_lStarSkillList.Count; i++)
            {
                if(_m_lStarSkillList[i] != null)
                    sb.Append(_m_lStarSkillList[i].starSkillId).Append(":").AppendLine(_m_lStarSkillList[i].level.ToString());
            }
            return sb.ToString();
        }
    }
}