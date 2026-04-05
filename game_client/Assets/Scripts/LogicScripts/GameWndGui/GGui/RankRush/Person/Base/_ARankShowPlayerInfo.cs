using System;
using CommonEnum;

namespace GOE
{
    public abstract class _ARankCommonShowInfo<T_RealInfoType> where T_RealInfoType : _ARankCommonShowInfo<T_RealInfoType>
    {
        /// <summary>
        /// 排名名次
        /// </summary>
        public abstract int rankSortId { get; }
        
        /// <summary>
        /// 排行分数
        /// </summary>
        public abstract long rankScore { get; }
        
        /// <summary>
        /// 排行分数显示格式
        /// </summary>
        public abstract EValueFormatType rankScoreFormat { get; }

        /// <summary>
        /// 基础信息 - 排行玩家的cid
        /// </summary>
        public abstract long cid { get; }

        /// <summary>
        /// 展示类型(点击排行榜展示的具体数据)
        /// </summary>
        public abstract ERankDetailShowType showType { get; }

        /// <summary>
        /// 是否是跨服排行榜
        /// </summary>
        public abstract bool isCross { get; }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="_isDetail"></param>
        /// <param name="_callBack"></param>
        public abstract void getInfo(bool _isDetail, Action<T_RealInfoType> _callBack);
        
        /// <summary>
        /// 玩家信息
        /// </summary>
        public abstract NPCommonSimplePlayerInfo playerInfo { get; }
    }
}