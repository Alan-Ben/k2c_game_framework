using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

namespace GOE.Variable
{
    /// <summary>
    /// 通用条件框架中条件数据的接口类
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public interface _ITNPBasicVariable<E
#if NP_GAME
        , D
#endif
        >
        where E : Enum
#if NP_GAME
        where D : _ITNPVariableDealerData<E>
#endif
    {
        /// <summary>
        /// 获取条件类型
        /// </summary>
        E variableType { get; }

#if NP_GAME
        /// <summary>
        /// 判断条件是否匹配
        /// </summary>
        /// <param name="_cond"></param>
        /// <param name="_data"></param>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        long calValue(D _data, NPVarInfo _varVariableInfo);
#endif
    }
}

