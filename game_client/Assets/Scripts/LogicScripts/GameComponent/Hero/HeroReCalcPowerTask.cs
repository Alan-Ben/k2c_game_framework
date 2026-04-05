using ALPackage;

namespace GOE
{
    /// <summary>
    /// 英雄重新计算实力的计算处理任务
    /// </summary>
    public class HeroReCalcPowerTask : _IALBaseMonoTask
    {
        //大臣信息对象
        private HeroInfo _m_hiHeroInfo;

        public HeroReCalcPowerTask(HeroInfo _heroInfo)
        {
            _m_hiHeroInfo = _heroInfo;
        }

        public void deal()
        {
            if (_m_hiHeroInfo == null)
                return;

            HeroCommon.dealRecalculatePower(_m_hiHeroInfo, true);
        }
    }
}