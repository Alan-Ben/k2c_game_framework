using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using ALBasicProtocolPack;
using NPGC2LS.p001_BasicOp;

namespace GOE
{
    /***************
     * 协议构造对象
     **/
    public class NPLSWriter_001_LoginOp
    {
        public static NPGC2LS_001_001_ReqBasicInfo make_001_001_ReqBasicInfo()
        {
            NPGC2LS_001_001_ReqBasicInfo protocol = new NPGC2LS_001_001_ReqBasicInfo();
            return protocol;
        }

        public static NPGC2LS_001_002_EnterGame make_001_002_EnterGame()
        {
            NPGC2LS_001_002_EnterGame protocol = new NPGC2LS_001_002_EnterGame();
            return protocol;
        }

        public static NPGC2LS_001_003_CancelQueue make_001_003_CancelQueue()
        {
            NPGC2LS_001_003_CancelQueue protocol = new NPGC2LS_001_003_CancelQueue();
            return protocol;
        }

        public static NPGC2LS_001_005_ReqQueueHeadIdx make_001_005_ReqQueueHeadIdx()
        {
            NPGC2LS_001_005_ReqQueueHeadIdx protocol = new NPGC2LS_001_005_ReqQueueHeadIdx();
            return protocol;
        }

        public static NPGC2LS_001_006_ReqEnterSNCode make_001_006_ReqEnterSNCode(string _SNCode)
        {
            NPGC2LS_001_006_ReqEnterSNCode protocl = new NPGC2LS_001_006_ReqEnterSNCode();
            protocl.setSnCode(_SNCode);
            return protocl;
        }
    }
}
