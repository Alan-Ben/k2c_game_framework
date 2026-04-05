package NPUSServer.NPUSUserMgr.UserComp.GuildBoxComp;

import ALBasicServer.ALTask._IALSynTask;

/**
 * 宝箱数据保存失败重新尝试任务
 * @author mj
 *
 */
public class GuildBoxSaveTask implements _IALSynTask
{
	private long _m_lDataSerial;
	private _APlayerGuildBoxInfo _m_biBoxInfo;
	
	public GuildBoxSaveTask(long _dataSerial, _APlayerGuildBoxInfo _info)
	{
		_m_lDataSerial = _dataSerial;
		_m_biBoxInfo = _info;
	}

	@Override
	public void run() 
	{
		_m_biBoxInfo.saveBoxData(_m_lDataSerial);
	}
}
