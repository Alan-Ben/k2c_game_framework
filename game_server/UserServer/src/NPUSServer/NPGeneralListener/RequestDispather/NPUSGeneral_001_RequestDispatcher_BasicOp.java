package NPUSServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.NPGeneralListener.RequestDispather.p001_BasicOp.RequestDealer_NP2US_R_001_002_ChgGeneral;
import NPUSServer.NPGeneralListener.RequestDispather.p001_BasicOp.RequestDealer_NP2US_R_001_003_ChgRef;
import NPUSServer.NPGeneralListener.RequestDispather.p001_BasicOp.RequestDealer_NP2US_R_001_010_PHPGetPlayerInfo;
import NPUSServer.NPGeneralListener.RequestDispather.p001_BasicOp.RequestDealer_NP2US_R_001_020_GuildMsg;

public class NPUSGeneral_001_RequestDispatcher_BasicOp extends NPRequestDispatcher
{
    public static void init(NPUSGeneralRequestDispather _dispather)
    {
    	_dispather.regHandler(new RequestDealer_NP2US_R_001_002_ChgGeneral(_dispather.getUSServer()));
    	_dispather.regHandler(new RequestDealer_NP2US_R_001_003_ChgRef(_dispather.getUSServer()));
    	
    	//后台获取玩家信息
    	_dispather.regHandler(new RequestDealer_NP2US_R_001_010_PHPGetPlayerInfo(_dispather.getUSServer()));

        //公会消息处理
        _dispather.regHandler(new RequestDealer_NP2US_R_001_020_GuildMsg(_dispather.getUSServer()));
    }
}
