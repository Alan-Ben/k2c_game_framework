using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

namespace GOE.Condition
{
    /// <summary>
    /// 通用条件框架中条件数据的接口类
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public interface _ITNPBasicCondition<E
#if NP_GAME
        , D
#endif
        , T_VarInfo
        >
        where E : Enum
#if NP_GAME
        where D : _ITNPConditionDealerData<E>
#endif
        where T_VarInfo : _IVarInfo
    {
        /// <summary>
        /// 获取条件类型
        /// </summary>
        E conditionType { get; }

#if NP_GAME
        /// <summary>
        /// 判断条件是否匹配
        /// </summary>
        /// <param name="_cond"></param>
        /// <param name="_data"></param>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        bool isEnable(D _data, T_VarInfo _varVariableInfo);
#endif
    }
}

