using System;
using System.Collections.Generic;

using ALPackage;
using ALBasicProtocolPack;
using NPEnum;

namespace GOE
{
    /******************
     * 节点存储cache对象，避免创建过多对象
     **/
    public class NPVarObjCache : _ATVarObjCache<ENPPlayerVariableVarType, NPVarObj>
    {
    }


    /******************
     * 节点存储cache对象，避免创建过多对象
     **/
    public class NPVarInfoCache : _ATVarInfoCache<ENPPlayerVariableVarType, NPVarObj, NPVarInfo>
    {
    }
}

