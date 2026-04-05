using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using ALBasicProtocolPack;

namespace GOE
{
    /** 非安全的消息处理对象，一般用于处理消息序号验证的操作 */
    public class NPGSUnsafeProtocolDispather : ALBasicProtocolDispather
    {
        public NPGSUnsafeProtocolDispather()
        {
            RegProtocol(new NPGSMainDealer_001_BasicOp_Unsafe());
        }
    }
}
