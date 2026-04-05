using UnityEngine;
using System.Collections;
using ALBasicProtocolPack;

namespace GOE
{
    public class NPLSMainDealer_001_LoginOp : ALBasicProtocolMainOrderDealer
    {
        public NPLSMainDealer_001_LoginOp()
            : base((byte)1, 60)
        {
            regDealer(new NPLSSubDealer_001_001_RetBasicInfo());
            regDealer(new NPLSSubDealer_001_002_EnterGameRes());
            regDealer(new NPLSSubDealer_001_003_CancelQueueRes());
            regDealer(new NPLSSubDealer_001_004_EnterQueue());
            regDealer(new NPLSSubDealer_001_005_RetQueueHeadIdx());
            regDealer(new NPLSSubDealer_001_006_RetEnterSNCode());
            regDealer(new NPLSSubDealer_001_050_OnNotifyNeedCheckSN());
        }
    }
}
