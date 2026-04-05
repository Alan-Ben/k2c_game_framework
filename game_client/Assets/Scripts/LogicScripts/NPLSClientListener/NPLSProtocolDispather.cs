using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using ALBasicProtocolPack;

namespace GOE
{
    public class NPLSProtocolDispather : ALBasicProtocolDispather
    {
        public NPLSProtocolDispather()
        {
            RegProtocol(new NPLSMainDealer_000_BasicOp());
            RegProtocol(new NPLSMainDealer_001_LoginOp());
        }
    }
}
