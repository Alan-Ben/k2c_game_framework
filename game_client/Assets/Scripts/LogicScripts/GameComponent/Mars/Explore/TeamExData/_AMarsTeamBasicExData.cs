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
    public abstract class _ATMarsTeamBasicExData<T> : _IMarsTeamExDataInterface where T : _IALProtocolStructure
    {
        //队伍数据对象
        private MarsExploreTeamInfo _m_tiTeamInfo;
        //额外的数据存储对象
        private T _m_dExData;

        public _ATMarsTeamBasicExData(MarsExploreTeamInfo _tiTeamInfo, byte[] _exData)
        {
            _m_tiTeamInfo = _tiTeamInfo;
            _m_dExData = _readExData(_exData);
        }

        public MarsExploreTeamInfo teamInfo { get { return _m_tiTeamInfo; } }
        public T data { get { return _m_dExData; } }

        /// <summary>
        /// 额外可以减少的存在时间，根据不同状态额外数据做处理
        /// 一般为0
        /// </summary>
        public virtual long exReduceTimeMS { get { return 0; } }
        /// <inheritdoc/>
        public abstract EMarsBagItemUseTimeType timeType { get; }
        /// <inheritdoc/>
        public abstract long guildHelpId { get; }
        /// <inheritdoc/>
        public abstract void reqCompleteNow(Action<bool> _obj);

        /// <summary>
        /// 读取数据，并返回结构体
        /// </summary>
        /// <param name="_exData"></param>
        /// <returns></returns>
        protected abstract T _readExData(byte[] _exData);

        /// <summary>
        /// 更新扩展数据，如果状态没变化，则调用此接口
        /// </summary>
        /// <param name="_exData"></param>
        public abstract void onUpdateExData(_IMarsTeamExDataInterface _exData);
        /// <summary>
        /// 进入本状态类的处理
        /// </summary>
        public abstract void onEnterExData();

        /// <summary>
        /// 退出本状态类的处理
        /// </summary>
        public abstract void onExitExData();
    }
}