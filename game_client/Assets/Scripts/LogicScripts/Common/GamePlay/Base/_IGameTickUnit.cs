using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// Unit 的 Tick 接口
    /// </summary>
    /// <remarks>
    /// 实现这个接口，Unit 将在加入 GameLogic 后被每帧调用
    /// </remarks>
    public interface _IGameTickUnit
    {
        /// <summary>
        /// 每帧的运算
        /// </summary>
        /* internal C# 8.0 特性 */ void tick(float _deltaTime);
    }
    
    public abstract class _AGameTickUnit : _AGameUnit, _IGameTickUnit
    {
        /* 原来访问权限为protected, 但是想在热更工程使用的话, 无法访问带参的protected构造方法, 所以先改成public
         * 
         */public _AGameTickUnit([NotNull] _AGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public abstract void tick(float _deltaTime);
    }
}