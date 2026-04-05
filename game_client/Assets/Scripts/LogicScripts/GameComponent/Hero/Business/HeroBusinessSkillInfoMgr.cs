using System.Collections.Generic;
using System.Text;
using Common.HeroObj;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 伙伴经营技能数据管理器
    /// </summary>
    public class HeroBusinessSkillInfoMgr
    {
        //伙伴信息
        private HeroInfo _m_heroInfo;
        //伙伴经营数据列表
        [NotNull] private List<HeroBusinessSkillInfo> _m_lBusinessList = new List<HeroBusinessSkillInfo>();

        public HeroBusinessSkillInfoMgr()
        {
        }

        /// <summary>
        /// 初始化经营技能列表
        /// </summary>
        /// <param name="_infoList"></param>
        public void initBusinessSkillList(HeroInfo _heroInfo, List<Hero_BusinessSkillInfo> _infoList)
        {
            if (_infoList == null || _heroInfo == null)
                return;

            _m_heroInfo = _heroInfo;
            _m_lBusinessList.Clear();

            for (int i = 0; i < _infoList.Count; i++)
            {
                _m_lBusinessList.Add(new HeroBusinessSkillInfo(_m_heroInfo.id, _infoList[i]));
            }
        }

        /// <summary>
        /// 更新经营技能数据
        /// </summary>
        /// <param name="_businessSkillInfo"></param>
        public void updateBusinessSkillInfo(Hero_BusinessSkillInfo _businessSkillInfo)
        {
            if (_businessSkillInfo == null)
                return;

            updateBusinessSkillInfo(_businessSkillInfo.getBusinessSkillId(), _businessSkillInfo.getLevel());
        }

        /// <summary>
        /// 更新经营技能数据
        /// </summary>
        /// <param name="_businessSkillId"></param>
        /// <param name="_level"></param>
        public void updateBusinessSkillInfo(long _businessSkillId, int _level)
        {
            if (_businessSkillId <= 0 || _level <= 0 || _m_heroInfo == null)
                return;

            HeroBusinessSkillInfo businessSkillInfo = getBusinessSkillInfo(_businessSkillId);
            if (businessSkillInfo != null)
                businessSkillInfo.updateInfo(_businessSkillId, _level);
            else
                _m_lBusinessList.Add(new HeroBusinessSkillInfo(_m_heroInfo.id, _businessSkillId, _level));

            //更新建筑加成
            if (_m_heroInfo.placeData.isPlaced)
            {
                BusinessBuildingInfo buildingInfo = NPPlayer.instance.buildingComp.getBusinessBuildingInfo(_m_heroInfo.placeData.buildingId);
                buildingInfo?.recalEarnings();
            }
        }

        /// <summary>
        /// 获取伙伴经营技能数据
        /// </summary>
        /// <param name="_businessSkillId"></param>
        /// <returns></returns>
        public HeroBusinessSkillInfo getBusinessSkillInfo(long _businessSkillId)
        {
            for (int i = 0; i < _m_lBusinessList.Count; i++)
            {
                if (_m_lBusinessList[i] != null && _m_lBusinessList[i].businessSkillId == _businessSkillId)
                {
                    return _m_lBusinessList[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 获取经营技能属性加成值
        /// </summary>
        /// <param name="_businessBuildingRef"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        public long getBusinessSkillAddPropValue(BusinessBuildingRefObj _businessBuildingRef, EBonusPropertyType _type, NPVarInfo _varInfo = null)
        {
            long targetValue = 0;
            for (int i = 0; i < _m_lBusinessList.Count; i++)
            {
                if (_m_lBusinessList[i] != null)
                    targetValue += _m_lBusinessList[i].getBusinessSkillAddPropValue(_businessBuildingRef, _type, _varInfo);
            }

            return targetValue;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _m_lBusinessList.Count; i++)
            {
                if(_m_lBusinessList[i] != null)
                    sb.Append(_m_lBusinessList[i].businessSkillId).Append(":").AppendLine(_m_lBusinessList[i].level.ToString());
            }
            return sb.ToString();
        }
    }
}