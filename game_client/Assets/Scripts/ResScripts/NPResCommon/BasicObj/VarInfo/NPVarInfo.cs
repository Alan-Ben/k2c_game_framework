using System;
using System.Collections.Generic;

using ALPackage;
using ALBasicProtocolPack;
using NPEnum;

namespace GOE
{
    /**************
     * 战斗内变量系数的缓存
     **/
    public class NPVarInfo : _ATVarInfo<ENPPlayerVariableVarType, NPVarObj>
    {
        protected override NPVarObj _getNewVarObj()
        {
            return WCGSingleton<NPVarObjCache>.instance.popItem();
        }

        protected override void _resetVarObj(NPVarObj _varObj)
        {
            WCGSingleton<NPVarObjCache>.instance.pushBackCacheItem(_varObj);
        }
    }
}

