using UnityEngine;
using System.Collections;
using ALBasicProtocolPack;

namespace GOE
{
    public class NPGSMainDealer_001_BasicOp_Unsafe : ALBasicProtocolMainOrderDealer
    {
        public NPGSMainDealer_001_BasicOp_Unsafe()
            : base((byte)1, 30)
        {
            regDealer(new NPGSSubDealer_001_001_RetBasicInfo());
            regDealer(new NPGSSubDealer_001_002_HeartPack());
            regDealer(new NPGSSubDealer_001_003_RetReloginKey());
            regDealer(new NPGSSubDealer_001_004_OnUSEnterDone());
            regDealer(new NPGSSubDealer_001_005_EnterUSRes());
            regDealer(new NPGSSubDealer_001_006_QuitUSRes());
            regDealer(new NPGSSubDealer_001_007_RetQueueInfo());
            regDealer(new NPGSSubDealer_001_008_QuitQueue());
            regDealer(new NPGSSubDealer_001_009_ResumeUSConnectionRes());
            regDealer(new NPGSSubDealer_001_010_OnMsgInvalid());
            regDealer(new NPGSSubDealer_001_011_RetPlayerJoinedUSList());
            regDealer(new NPGSSubDealer_001_012_RetMostRecommendedUSInfo());
            regDealer(new NPGSSubDealer_001_013_EnterUSButFreeze());
            regDealer(new NPGSSubDealer_001_020_RetReconnectInfo());
            regDealer(new NPGSSubDealer_001_021_ReceivedMsg());
            regDealer(new NPGSSubDealer_001_022_SendSerializeMsg());
            regDealer(new NPGSSubDealer_001_023_RetClientRequest());
            regDealer(new NPGSSubDealer_001_030_BeDeviceKicked());
        }
    }
}
