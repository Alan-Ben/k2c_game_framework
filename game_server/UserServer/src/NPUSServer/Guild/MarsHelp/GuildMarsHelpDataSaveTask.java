package NPUSServer.Guild.MarsHelp;

import ALBasicServer.ALTask._IALSynTask;

public class GuildMarsHelpDataSaveTask implements _IALSynTask
{
	//数据序列号
	private long _m_lDataSerial;
	//火星求助数据
	private GuildMarsHelpInfo _m_hiHelpInfo;
	
	public GuildMarsHelpDataSaveTask(long _dataSerial, GuildMarsHelpInfo _helpInfo)
	{
		_m_lDataSerial = _dataSerial;
		
		_m_hiHelpInfo = _helpInfo;
	}

	@Override
	public void run() 
	{
		//数据已经删除
		if(_m_hiHelpInfo.isDeled())
			return;
		
		_m_hiHelpInfo._saveHelpData(_m_lDataSerial);
	}
}
