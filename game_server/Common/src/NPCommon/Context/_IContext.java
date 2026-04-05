package NPCommon.Context;

import NPCommon.Enum.NPCommonEnum;

public interface _IContext
{
    long getGuid(); //返回上下文实例id

    int getContextId();//返回上下文ID

    //获取层级，一般用于判断是否可能出现死循环
    int getDeep();

    _IContext duplicate();

    NPCommonEnum.ENPContextType getType();
}
