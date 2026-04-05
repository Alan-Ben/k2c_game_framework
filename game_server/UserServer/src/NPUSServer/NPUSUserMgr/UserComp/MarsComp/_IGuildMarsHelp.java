package NPUSServer.NPUSUserMgr.UserComp.MarsComp;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.GuildEnum.EGuildMarsHelpObjType;

public interface _IGuildMarsHelp 
{
	/**
	 * 求助对象类型
	 * @return
	 */
	public EGuildMarsHelpObjType getObjType();
	/**
	 * 求助对象ID
	 * @return
	 */
	public long getObjId();
	/**
	 * 构造额外数据
	 * @return
	 */
	public _IALProtocolStructure makeExt();
	/**
	 * 获取求助数据ID
	 * @return
	 */
	public long getHelpId();
	/**
	 * 获取求助累计秒数
	 * @return
	 */
	public int getHelpSecs();
	/**
	 * 是否可以发送求助
	 * @param _objType
	 * @return
	 */
	public boolean canSendHelp(EGuildMarsHelpObjType _objType);
	/**
	 * 更新求职数据
	 * @param _helpId
	 */
	public void setSendHelp(long _helpId);
	/**
	 * 更新发送求助数据
	 * @param _helpId
	 * @param _addHelpSecs
	 */
	public void updateHelpSecs(long _helpId, int _addHelpSecs);
}
