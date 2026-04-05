package NPUSServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import ALBasicServer.ALProcess.ALProcess;
import Common.ServerObj.ServerObj_PHPPlayer;
import NP2US_R.p001_BasicOp.NP2US_R_001_010_PHPGetPlayerInfo;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CallBack._ICallBackBool;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_001_BasicOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerBO;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.HashMap;

public class RequestDealer_NP2US_R_001_010_PHPGetPlayerInfo extends _ABasicGeneralRequestDealer<NP2US_R_001_010_PHPGetPlayerInfo>
{
    public RequestDealer_NP2US_R_001_010_PHPGetPlayerInfo(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_001_010_PHPGetPlayerInfo _msg)
    {
    	final ServerObj_PHPPlayer playerInfo = new ServerObj_PHPPlayer();
    	
    	ALProcess process = ALProcess.CreateProcess("php_get_player");
    	//步骤1:获取玩家基础信息
        process.addResDelegateProcess(action -> _initBasePlayer(_msg, playerInfo, action::dealAction), "player_base_init", null, false);

        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "[PHP] get player cid:{} name:{} fail", _msg.getCid(), _msg.getName());
                _committer.commitFailRes(CommErr.PLAYER_NOT_FOUND.getCode());
            }

            @Override
            public void onRootProecssSuc()
            {
            	_committer.commitSucRes(NP2US_RB_Writer_001_BasicOp.make_010_PHPGetPlayerInfo(playerInfo));
            }
        });
    }
    
    /**
     * 步骤1:获取玩家基础信息
     * @param _msg
     * @param _playerInfo
     * @param _handler
     */
    private void _initBasePlayer(NP2US_R_001_010_PHPGetPlayerInfo _msg, final ServerObj_PHPPlayer _playerInfo, _ICallBackBool _handler)
    {
    	BM bmObj = getUSServer().getBM();
    	
    	HashMap<String, Object> cond = new HashMap<>();
    	if(!_msg.getName().isEmpty())
    	{
    		cond.put("cname", _msg.getName());
    	}
    	else
    	{
    		cond.put("cid", _msg.getCid());
    	}
    	
    	bmObj.getBM(PlayerBO.class).findOne(cond, new _ASelectCallback<PlayerBO>()
        {
			@Override
			public void dealSuc(PlayerBO _bo) 
			{
				_playerInfo.setUid(_bo.getUid());
				_playerInfo.setCid(_bo.getCid());
				_playerInfo.setName(_bo.getCname());
				_playerInfo.setLvl(_bo.getLvl());
				_playerInfo.setVip_lvl(_bo.getVipLvl());
				_playerInfo.setAr_time(_bo.getCreateTime());
				_playerInfo.setCreate_role_time(_bo.getCreateRoleTime());
				_playerInfo.setLast_login_time((int) (_bo.getLatestLoginTimeMs() / 1000));
				
				_handler.onRunOver(true);
			}

			@Override
			public void dealFail() 
			{
				_handler.onRunOver(false);
			}
        });
    }
}
