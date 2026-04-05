namespace GOE
{
    /// <summary>
    /// Unit 的表现接口
    /// </summary>
    /// <remarks>
    /// 实现这个接口，Unit 会在被加入到 GameLogic 的时候对当前注册给 GameLogic 的表现接口做出反应
    /// </remarks>
    public interface _INPGameShowableUnit<T> where T : _INPGameShow
    {
        /// <summary>
        /// 当表现接口被设置了
        /// </summary>
        /* internal C# 8.0 特性 */ void setGameShow(T _gameShow);
        /// <summary>
        /// 当表现接口被移除了
        /// </summary>
        /* internal C# 8.0 特性 */ void resetGameShow();

        /// <summary>
        /// 刷新单位表现对象，TODO 表现层op改动导致变动后逻辑层的表现层单位view引用替换没想到好的方案，临时加上，等coda回归一起确认下
        /// </summary>
        /* internal C# 8.0 特性 */ void refreshSetUnitShow();
    }
}