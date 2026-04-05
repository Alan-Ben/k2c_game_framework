using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 资源收获表现接口对象
    /// </summary>
    public interface _IHarvestTarget
    {
        /// <summary>
        /// 对应的收获资源类型
        /// </summary>
        EHarvestType harvestType { get; }

        /// <summary>
        /// 获取粒子收集结束对象
        /// </summary>
        RectTransform targetUIObj { get; }

        /// <summary>
        /// 粒子开始表现时的处理
        /// 此时需要设置好相关的状态以及可能触发消息的处理，避免错误的刷新操作
        /// </summary>
        /// <param name="_serialize">本次操作的序列号，每一次操作都会带入不同序列号用于区分，可以使用NPGGUIHarvestNumController做管理</param>
        void particleStart(long _serialize);
        /// <summary>
        /// 粒子表现结束的处理
        /// </summary>
        /// <param name="_serialize">每一次操作都会带入不同序列号用于区分，可以使用NPGGUIHarvestNumController做管理</param>
        void particleComplete(long _serialize);
        /// <summary>
        /// 单个粒子飞行到位的处理
        /// </summary>
        /// <param name="_serialize">每一次操作都会带入不同序列号用于区分，可以使用NPGGUIHarvestNumController做管理</param>
        /// <param name="_itemCount">单个粒子代表数量</param>
        void particleItemDone(long _serialize, long _itemCount);
    }
}