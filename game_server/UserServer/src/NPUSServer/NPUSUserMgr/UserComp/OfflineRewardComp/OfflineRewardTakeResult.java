package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;

/**
 * 领取离线奖励的结果数据
 */
public class OfflineRewardTakeResult 
{
	private Result _m_rResult = Result.failed(CommErr.UNKNOW_ERR.getCode(), "UNKNOW_ERR");
	private _IALProtocolStructure _m_extData;
	
	public void setResult(Result _result) {_m_rResult = _result;}
	public Result getResult() {return _m_rResult;}
	
	public void setExtData(_IALProtocolStructure _extData) {_m_extData = _extData;}
	public _IALProtocolStructure getExtData() {return _m_extData;}
}
