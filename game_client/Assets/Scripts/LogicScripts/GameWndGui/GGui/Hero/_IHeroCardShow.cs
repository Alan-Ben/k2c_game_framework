
namespace GOE
{
    /// <summary>
    /// 伙伴卡牌展示接口
    /// </summary>
    public interface _IHeroCardShow
    {
        /// <summary>
        /// 伙伴id
        /// </summary>
        long id { get; }
        /// <summary>
        /// 伙伴数据
        /// </summary>
        HeroRefObj heroRefObj { get ; }
        /// <summary>
        /// 伙伴阶段数据
        /// </summary>
        HeroStepRefObj curHeroStepRef { get; }
        /// <summary>
        /// 获取等级
        /// </summary>
        long level { get ; }
        /// <summary>
        /// 获取实力
        /// </summary>
        long power { get; }
        /// <summary>
        /// 获取星级
        /// </summary>
        long star { get ; }
        /// <summary>
        /// 是否解锁
        /// </summary>
        bool isUnlock { get; }
        /// <summary>
        /// 获取总资质
        /// </summary>
        /// <returns></returns>
        long getTotalTalent();
        /// <summary>
        /// 显示头像
        /// </summary>
        /// <returns></returns>
        NPGTextureIndex getIcon();
        /// <summary>
        /// 显示卡牌半身像
        /// </summary>
        /// <returns></returns>
        NPGTextureIndex getCardImage();
        /// <summary>
        /// 显示卡牌背景图
        /// </summary>
        /// <returns></returns>
        NPGTextureIndex getCardBg();
        /// <summary>
        /// 显示全身形象
        /// </summary>
        /// <returns></returns>
        NPGGoIndex getTdShow();
        /// <summary>
        /// 获取形象背景
        /// </summary>
        /// <returns></returns>
        NPGGoIndex getTdBg();
    }
}