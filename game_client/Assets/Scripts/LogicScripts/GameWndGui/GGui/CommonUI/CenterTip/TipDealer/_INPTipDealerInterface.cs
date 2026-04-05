using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 提示处理接口
    /// </summary>
    public interface _INPTipDealerInterface
    {
        /// <summary>
        /// 标记，用于特殊删除管理需求
        /// </summary>
        ETipDealerTagType tag { get; }
        
        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="_serialize">序列号</param>
        /// <param name="_parent"></param>
        /// <param name="_speedRate"></param>
        /// <param name="_onCdDone">间隔时间结束回调<调用时传入的序列号></param>
        /// <param name="_doneAction">销毁的时候回调</param>
        void showTip(long _serialize, Transform _parent, float _speedRate, Action<long> _onCdDone, Action<_INPTipDealerInterface> _doneAction);

        /// <summary>
        /// 销毁
        /// </summary>
        void discard();
    }
}
