using UnityEngine;
using System.Collections;
using ALPackage;

namespace GOE
{
    public static class NPGSWriter_001_BasicOp
    {
        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_001_ReqBasicInfo make_001_ReqBasicInfo()
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_001_ReqBasicInfo protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_001_ReqBasicInfo();
            return protocol;
        }
        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_002_HeartPack make_002_HeartPack(long _clientTimeTag, long _calServerTimeTag, long _clientHeartSerialize)
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_002_HeartPack protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_002_HeartPack();

            protocol.setCalServerTimeTag(_calServerTimeTag);
            protocol.setClientTimeTag(_clientTimeTag);

            protocol.setClientHeartSerialize(_clientHeartSerialize);

            return protocol;
        }
        
        
        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_005_RequestEnterUS make_005_RequestEnterUS(long _clientSerialize, int _usLogicId)
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_005_RequestEnterUS protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_005_RequestEnterUS();
            protocol.setClientSerialize(_clientSerialize);
            protocol.setServerLogicId(_usLogicId);
            return protocol;
        }
        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_006_QuitUS make_006_QuitUS(long _clientSerialize, int _usId)
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_006_QuitUS protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_006_QuitUS();
            protocol.setClientSerialize(_clientSerialize);
            protocol.setUsId(_usId);
            return protocol;
        }
        
        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_007_ReqQueueInfo make_007_ReqQueueInfo(long _clientSerialize)
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_007_ReqQueueInfo protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_007_ReqQueueInfo();
            protocol.setClientSerialize(_clientSerialize);
            return protocol;
        }
        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_008_ReqQuitQueue make_008_ReqQuitQueue(long _clientSerialize)
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_008_ReqQuitQueue protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_008_ReqQuitQueue();
            protocol.setClientSerialize(_clientSerialize);
            return protocol;
        }

        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_009_ResumeUSConnection make_009_ResumeUSConnection(long _clientSerialize, int _usLogicId)
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_009_ResumeUSConnection protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_009_ResumeUSConnection();
            protocol.setClientSerialize(_clientSerialize);
            protocol.setServerLogicId(_usLogicId);
            return protocol;
        }

        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_011_ReqPlayerJoinedUSList make_011_ReqPlayerJoinedUSList()
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_011_ReqPlayerJoinedUSList protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_011_ReqPlayerJoinedUSList();
            return protocol;
        }
        
        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_012_ReqMostRecommendedUSInfo make_012_ReqMostRecommendedUSInfo()
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_012_ReqMostRecommendedUSInfo protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_012_ReqMostRecommendedUSInfo();
            return protocol;
        }

        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_020_ReconnectInfo make_020_ReconnectInfo(int _curRecMesCount)
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_020_ReconnectInfo protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_020_ReconnectInfo();

            protocol.setCurRecMesCount(_curRecMesCount);

            return protocol;
        }

        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_021_ReceivedMsg make_021_ReceivedMsg(int _curRecMesCount)
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_021_ReceivedMsg protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_021_ReceivedMsg();

            protocol.setCurRecMesCount(_curRecMesCount);

            return protocol;
        }

        //连接聊天服务器
        public static NPGC2GS.p001_BasicOp.NPGC2GS_001_022_SendSerializeMsg make_022_SendSerializeMsg(int _serialize, long _clientRequestSerialize, byte[] _msg)
        {
            NPGC2GS.p001_BasicOp.NPGC2GS_001_022_SendSerializeMsg protocol = new NPGC2GS.p001_BasicOp.NPGC2GS_001_022_SendSerializeMsg();

            protocol.setMsgSerialize(_serialize);
            protocol.setClientRequestSerialize(_clientRequestSerialize);
            protocol.setMsg(_msg);

            return protocol;
        }
    }
}
