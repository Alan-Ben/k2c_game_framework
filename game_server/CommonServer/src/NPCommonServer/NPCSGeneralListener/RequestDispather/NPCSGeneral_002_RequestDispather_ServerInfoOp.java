package NPCommonServer.NPCSGeneralListener.RequestDispather;

import Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo;
import Common.NpServerObj.NpServerObj_SYS_ServerItem;
import NP2CS_R.np_p002_serverInfoOp.*;
import NP2CS_R.p002_ServerInfoOP.NP2CS_R_002_015_ReqUpdateWhiteAccList;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_005_RetUSHoldInfo;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_006_RetUSFreezeInfo;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.ErrMain.CSErr;
import NPCommon.ErrMain.LoginErr;
import NPCommon.Log.CommLog;
import NPCommon.NP_SYS_ServerItem;
import NPCommonServer.CommonServerConf;
import NPCommonServer.GeneralV.CsID;
import NPCommonServer.NPCrossGameServerHandleMgr.NPCrossGameServerInfo;
import NPCommonServer.NPCrossGameServerHandleMgr.NPCrossGameServerMgr;
import NPCommonServer.PHPParmMgr.CSPHPParamMgr;
import NPCommonServer.PlayerFreezeMgr.PlayerFreezeInfo;
import NPCommonServer.PlayerFreezeMgr.PlayerFreezeMgr;
import NPCommonServer.USServerHandleUserMgr.USServerHandleUserMgr;
import NPCommonServer.USServerListMgr.USServerListMgrCSInstance;
import NPCommonServer.WhiteAccMgr.CSWhiteAccMgr;
import NPEnum.EServerOnlineState;
import NPServerProtocolWriter.NP2CS.RequestBack.NP2CS_RB_Writer_002_ServerInfoOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.List;

/**
 * @description:
 * @author: ricci
 * @date: 2022-06-24 17:54:34
 */
public class NPCSGeneral_002_RequestDispather_ServerInfoOp extends NPRequestDispatcher
{
    public static void init(NPCSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_001_ReqUSInfoListInit>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_001_ReqUSInfoListInit _msg)
            {
                if (!USServerListMgrCSInstance.getInstance().isInited())
                {
                    _receiver.commitFailRes(CSErr.US_INFO_LIST_NOT_READY.getCode());
                    return;
                }
                _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp
                        .make_001_ReqUSInfoListInit(USServerListMgrCSInstance.getInstance().getServerIndexItemList()));
            }
        });
        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_002_ReqUSInfoList>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_002_ReqUSInfoList _msg)
            {
                if (!USServerListMgrCSInstance.getInstance().isInited())
                {
                    _receiver.commitFailRes(CSErr.US_INFO_LIST_NOT_READY.getCode());
                    return;
                }
                //获取服务器列表
                List<NP_SYS_ServerItem> serverItemList = USServerListMgrCSInstance.getInstance()
                        .lookupServerListByTestFunc(serverItem -> _msg.getUsLogicIdList().contains(serverItem.getServerLogicId()));
                //根据负载信息对服务器排序
                serverItemList.sort((o1, o2) ->
                {
                    NpServerObj_SYS_ServerHoldInfo holdInfo_1 = USServerHandleUserMgr.getInstance().lookup(o1.getServerTypeId());
                    NpServerObj_SYS_ServerHoldInfo holdInfo_2 = USServerHandleUserMgr.getInstance().lookup(o2.getServerTypeId());
                    if (holdInfo_1 == null)
                    {
                        return 1;
                    }
                    if (holdInfo_2 == null)
                    {
                        return -1;
                    }
                    //剩余负载数量
                    int canHoldCount_1 = holdInfo_1.getTotalCount() - holdInfo_1.getHoldCount();
                    int canHoldCount_2 = holdInfo_2.getTotalCount() - holdInfo_2.getHoldCount();
                    //根据负载数量判断
                    return Integer.compare(canHoldCount_2, canHoldCount_1);
                });
                //回包
                _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp
                        .make_002_ReqUSInfoList(serverItemList));
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_003_ReqUSInfoCSInit>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_003_ReqUSInfoCSInit _msg)
            {
                //同步US服务器信息列表
                USServerListMgrCSInstance.getInstance().initLoadServerList(_msg.getServerItemList(), _msg.getServerIndexList());
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_004_ReqUpdateUSHoldInfo>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_004_ReqUpdateUSHoldInfo _msg)
            {

                USServerHandleUserMgr.getInstance().update(_msg.getHoldInfo());
                _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_004_RetUpdateUSHoldInfo());
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_005_ReqUSHoldInfo>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_005_ReqUSHoldInfo _msg)
            {

                //获取负载信息列表
                NP2CS_RB_002_005_RetUSHoldInfo proto =
                        NP2CS_RB_Writer_002_ServerInfoOp.make_005_RetUSHoldInfo(
                                USServerHandleUserMgr.getInstance().lookup(_msg.getServerTypeIdList()));
                _receiver.commitSucRes(proto);
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_006_ReqUSFreezeInfo>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_006_ReqUSFreezeInfo _msg)
            {
                boolean isFreeze = PlayerFreezeMgr.getInstance().checkPlayerIsFreeze(_msg.getUid());
                PlayerFreezeInfo freezeInfo = PlayerFreezeMgr.getInstance().lookup(_msg.getUid());

                boolean inWhiteList = CSWhiteAccMgr.getInstance().checkAccInWhiteList(_msg.getUid());

                NP2CS_RB_002_006_RetUSFreezeInfo proto =
                        NP2CS_RB_Writer_002_ServerInfoOp.make_006_RetUSFreezeInfo(
                                isFreeze,
                                freezeInfo == null ? 0 : freezeInfo.getFreezeTime(),
                                inWhiteList);
                _receiver.commitSucRes(proto);
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_007_ReqUSInfoListByTypeId>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_007_ReqUSInfoListByTypeId _msg)
            {
                //获取服务器列表
                NpServerObj_SYS_ServerItem serverItem = USServerListMgrCSInstance.getInstance().lookupServerInfoByTypeId(_msg.getUsTypeId());
                if (serverItem == null)
                {
                    _receiver.commitFailRes(CSErr.US_INFO_NOT_FOUND.getCode());
                    return;
                }

                _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_007_RetUSInfoListByTypeId(serverItem));
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_008_ReqMostRecommendedUSInfo>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_008_ReqMostRecommendedUSInfo _msg)
            {
                // 查询最推荐的服务器logicId
                NP_SYS_ServerItem targetServer = null;
                int targetServerPriority = -1; // 初始优先级设置为-1，表示最低
                int targetServerLoad = Integer.MIN_VALUE; // 初始负载设置为最小值
                // 兜底服务器：当所有服务器都不是OPEN状态时，使用最后一个维护中的服务器
                NP_SYS_ServerItem fallbackServer = null;

                // 遍历服务器列表
                for (NP_SYS_ServerItem checkServer : USServerListMgrCSInstance.getInstance().makeServerItemList())
                {
                    if (checkServer == null)
                        continue;

                    // 记录最后一个非测试状态的服务器作为兜底（排除TEMP_CLOSED测试服务器）
                    if (checkServer.getOnlineStateTypeId() != EServerOnlineState.TEMP_CLOSED.ordinal())
                        fallbackServer = checkServer;

                    // 判断服务器是否开放
                    if (checkServer.getOnlineStateTypeId() != EServerOnlineState.OPEN.ordinal())
                        continue;

                    // 查询目标服务器的状态
                    NpServerObj_SYS_ServerHoldInfo checkServerHoldInfo = USServerHandleUserMgr.getInstance().lookup(checkServer.getServerTypeId());
                    if (checkServerHoldInfo == null)
                    {
                        CommLog.error("NP2CS_R_002_008_ReqMostRecommendedUSInfo checkServerHoldInfo is null, serverTypeId:{}", checkServer.getServerTypeId());
                        continue;
                    }

                    // 计算当前服务器的优先级和剩余负载
                    int currentServerStatus = checkServer.getShowStateTypeId(); // getShowStateTypeId()返回0, 1, 2分别对应新服、爆满、推荐
                    int currentServerPriority = (currentServerStatus == 1) ? 0 : (currentServerStatus == 0 ? 1 : 2); // 推荐 > 新服 > 爆满
                    int currentServerLoad = checkServerHoldInfo.getTotalCount() - checkServerHoldInfo.getHoldCount();

                    // 判断是否需要更新targetServer
                    if (targetServer == null || currentServerPriority > targetServerPriority || (currentServerPriority == targetServerPriority && currentServerLoad > targetServerLoad))
                    {
                        targetServer = checkServer;
                        targetServerPriority = currentServerPriority;
                        targetServerLoad = currentServerLoad;
                    }
                }

                // 判断是否找到目标服务器
                if (targetServer == null)
                {
                    // 如果没有找到OPEN状态的服务器，使用兜底服务器（最后一个服务器）
                    if (fallbackServer == null)
                    {
                        // 服务器列表为空或全部为测试状态，返回失败
                        _receiver.commitFailRes(LoginErr.RECOMMEND_SERVER_NOT_FOUND.getCode());
                    }
                    else
                    {
                        // 使用兜底服务器
                        _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_008_RetMostRecommendedUSInfo(fallbackServer));
                    }
                }
                else
                {
                    // 找到OPEN状态的推荐服务器，优先返回
                    _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_008_RetMostRecommendedUSInfo(targetServer));
                }
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_009_ReqTryCrossGameServer>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_009_ReqTryCrossGameServer _msg)
            {
                NPCrossGameServerInfo serverInfo = NPCrossGameServerMgr.getInstance().tryHandleCrossGameServer(_msg.getTmpHandleWeight());
                if (null == serverInfo)
                {
                    _receiver.commitFailRes(CSErr.HANDLE_SERVER_NOT_FOUND.getCode());
                } else
                {
                    //生成对应的实例ID
                    long instanceId = CsID.makeCsId(_msg.getGeneralV(), EServerType.CROSS_GAME.ordinal(), serverInfo.getServerTypeId());
                    //返回协议
                    _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_009_RetTryCrossGameServer(serverInfo.getServerTypeId(), instanceId));
                }
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_010_ReqSetCrossGameWeight>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_010_ReqSetCrossGameWeight _msg)
            {
                NPCrossGameServerInfo serverInfo = NPCrossGameServerMgr.getInstance().lookupServerById(_msg.getServerTypeId());
                if (null == serverInfo)
                {
                    _receiver.commitFailRes(CSErr.HANDLE_OBJ_NOT_FOUND.getCode());
                    return;
                }

                //设置权重
                serverInfo.setHandleWeight(_msg.getServerHandleWeight());

                //返回协议
                _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_010_RetSetCrossGameWeight());
            }
        });
        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_011_ReqUpdateUSInfoList>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_011_ReqUpdateUSInfoList _msg)
            {
            	if(CommonServerConf.getInstance().getNeedLoadFooServerList())
            	{
                    _receiver.commitFailRes(CSErr.US_INFO_SET_FFOM_FILE.getCode());
                    return;
                }
            	
                if (!USServerListMgrCSInstance.getInstance().updateServerItem(_msg.getServerItemList()))
                {
                    _receiver.commitFailRes(CSErr.US_INFO_LIST_NOT_READY.getCode());
                    return;
                }
                _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_011_RetUpdateUSInfoList());
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_012_ReqBanUid>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_012_ReqBanUid _msg)
            {
                PlayerFreezeMgr.getInstance().freeze(_msg.getUid(), _msg.getFreezeTimeMs());
                _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_012_RetBanUid());
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_013_ReqUnBanUid>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_013_ReqUnBanUid _msg)
            {
                PlayerFreezeMgr.getInstance().freeze(_msg.getUid(), 0);
                _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_013_RetUnBanUid());
            }
        });
        
        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_014_ReqUpdatePHPParamList>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_014_ReqUpdatePHPParamList _msg)
            {
            	CSPHPParamMgr.getInstance().updatePHPParamList(_msg.getDataSerial(), _msg.getPhpSerial(), _msg.getPListObj());

                _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_014_ReqPHPParamList());
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CS_R_002_015_ReqUpdateWhiteAccList>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2CS_R_002_015_ReqUpdateWhiteAccList _msg)
            {
                // 根据操作类型进行增量更新
                switch (_msg.getOpType())
                {
                    case ADD:
                        CSWhiteAccMgr.getInstance().addAcc(_msg.getAcc());
                        break;
                    case REMOVE:
                        CSWhiteAccMgr.getInstance().removeAcc(_msg.getAcc());
                        break;
                    default:
                        CommLog.error("Unknown white acc op type: {}", _msg.getOpType());
                        break;
                }
                _receiver.commitSucRes(NP2CS_RB_Writer_002_ServerInfoOp.make_015_RetUpdateWhiteAccList());
            }
        });
    }
}
