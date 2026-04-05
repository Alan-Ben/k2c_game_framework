using System;
using System.Collections.Generic;
using ALBasicProtocolPack;
using ALPackage;
using Common.GuildEnum;
using Common.MarsEnum;
using Common.MarsObj;
using GC2GS.p041_MarsExploreOp;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 队伍额外数据的接口类
    /// 当额外数据出现变化的时候，会调用进入、退出等相关接口
    /// </summary>
    public interface _IMarsTeamExDataInterface
    {
        /// <summary>
        /// 更新扩展数据，如果状态没变化，则调用此接口
        /// </summary>
        /// <param name="_exData"></param>
        void onUpdateExData(_IMarsTeamExDataInterface _exData);
        /// <summary>
        /// 进入本状态类的处理
        /// </summary>
        void onEnterExData();

        /// <summary>
        /// 退出本状态类的处理
        /// </summary>
        void onExitExData();

        /// <summary>
        /// 额外可以减少的存在时间，根据不同状态额外数据做处理
        /// 一般为0
        /// </summary>
        long exReduceTimeMS { get; }
        
        /// <summary>
        /// 状态的时间类型
        /// </summary>
        EMarsBagItemUseTimeType timeType { get; }
        /// <summary>
        /// 公会求助ID
        /// </summary>
        long guildHelpId { get; }
        /// <summary>
        /// 请求立即完成当前状态的处理
        /// </summary>
        void reqCompleteNow(Action<bool> _obj);
    }
}