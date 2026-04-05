package NPRecordServer.NPGeneralListener.RequestDispather.NP2RCS_001_RSOp;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo;
import NP2CS_R.np_p002_serverInfoOp.NP2CS_R_002_002_ReqUSInfoList;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_002_RetUSInfoList;
import NP2LCS_R.p001_BasicOp.NP2RCS_R_001_002_ReqLookupLoginRecord;
import NPCommon.CommonCache._AKeyListAdapter;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.NP_SYS_ServerItem;
import NPCommon.Util.Delegate.HandlerOne;
import NPRecordServer.NPRecordMgr.RecordInfoList;
import NPRecordServer.NPRecordMgr.RecordInfoListMgr;
import NPRecordServer.NPRecordServer;
import NPServerProtocolWriter.NP2CS.Request.NP2CS_R_Writer_002_ServerInfoOp;
import NPServerProtocolWriter.NP2RCS.RequestBack.NP2RCS_RB_Writer_001_BasicOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;
import java.util.Map;

/**
 * @description:
 * @author: ricci
 * @date: 2022-06-27 17:43:19
 */
public class NP2RCSRequestDealer_R_001_002_ReqLookupLoginRecord extends NPRequestDealer<NP2RCS_R_001_002_ReqLookupLoginRecord>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2RCS_R_001_002_ReqLookupLoginRecord _msg)
    {

        RecordInfoList recordInfoList = RecordInfoListMgr.getInstance().lookup(_msg.getAccountId());
        if (recordInfoList == null)
        {
            _receiver.commitSucRes(new NP2RCS_R_001_002_ReqLookupLoginRecord());
            return;
        }
        ArrayList<NP_SYS_PlayerJoinedUSInfo> joinedUSInfos = recordInfoList.makeProto();

        ArrayList<Integer> serverLogicIdList = new ArrayList<>();
        for (NP_SYS_PlayerJoinedUSInfo info : joinedUSInfos)
        {
            serverLogicIdList.add(info.getServerItem().getServerLogicId());
        }

        //查询服务器状态信息
        NP2CS_R_002_002_ReqUSInfoList reqUSInfoList = NP2CS_R_Writer_002_ServerInfoOp.make_002_ReqUSInfoList(serverLogicIdList);
        NPRecordServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.COMMON.ordinal(), reqUSInfoList, new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_002_002_RetUSInfoList();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _ret)
                    {
                        NP2CS_RB_002_002_RetUSInfoList ret = (NP2CS_RB_002_002_RetUSInfoList) _ret;
                        //完善回包数据
                        for (NP_SYS_ServerItem retServerItem : ret.getServerItemList())
                        {
                            for (NP_SYS_PlayerJoinedUSInfo joinedUSInfo : joinedUSInfos)
                            {
                                if (retServerItem.getServerLogicId() == joinedUSInfo.getServerItem().getServerLogicId())
                                {
                                    joinedUSInfo.setServerItem(retServerItem);
                                }
                            }
                        }

                        //检查 cache 完善玩家展示数据
                        _AKeyListAdapter<NP_SYS_PlayerJoinedUSInfo> adapter = new _AKeyListAdapter<NP_SYS_PlayerJoinedUSInfo>(joinedUSInfos)
                        {
                            @Override
                            public long getKey(NP_SYS_PlayerJoinedUSInfo _item)
                            {
                                return _item.getCid();
                            }
                        };

                        NPRecordServer.getInstance().getPlayerCacheGetter().getInfoList(NP_SYS_PlayerJoinedUSInfo.class, adapter,
                                new HandlerOne<Map<Long, NP_SYS_PlayerJoinedUSInfo>>()
                        {
                            @Override
                            public void handle(Map<Long, NP_SYS_PlayerJoinedUSInfo> _cacheInfoMap)
                            {
                                for (NP_SYS_PlayerJoinedUSInfo joinedUSInfo : joinedUSInfos)
                                {
                                    NP_SYS_PlayerJoinedUSInfo tempInfo = _cacheInfoMap.get(joinedUSInfo.getCid());
                                    if (tempInfo == null)
                                        continue;

                                    joinedUSInfo.setPlayerName(tempInfo.getPlayerName());
                                    joinedUSInfo.setIconShowInfo(tempInfo.getIconShowInfo());
                                    joinedUSInfo.setIsFreeze(tempInfo.getIsFreeze());
                                    joinedUSInfo.setFreezeTimeMs(tempInfo.getFreezeTimeMs());
                                }

                                _receiver.commitSucRes(NP2RCS_RB_Writer_001_BasicOp.make_001_002_ReqLookupLoginRecord(joinedUSInfos));
                            }
                        });

                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _receiver.commitFailRes(_errCode);
                    }
                });

    }
}
