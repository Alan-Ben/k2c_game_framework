package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 火星探险-占领火星矿玩家
 **/
public class ServerObj_MarsTeam_OccupyMinePlayer implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家CID */
private long cid;
/** 玩家队伍ID */
private long teamId;
/** 玩家队伍单兵实力 */
private long teamSoldierPower;
/** 玩家队伍带兵量 */
private long teamTroopNum;
/** 玩家队伍损耗量 */
private long teamLossValue;
/** 联盟ID */
private long guildId;
/** 玩家昵称 */
private String cname;
/** 联盟简称 */
private String guildBName;


public ServerObj_MarsTeam_OccupyMinePlayer() {
	cid = (long)0;
	teamId = (long)0;
	teamSoldierPower = (long)0;
	teamTroopNum = (long)0;
	teamLossValue = (long)0;
	guildId = (long)0;
	cname = "";
	guildBName = "";
}

public ServerObj_MarsTeam_OccupyMinePlayer(
	 long _cid
	, long _teamId
	, long _teamSoldierPower
	, long _teamTroopNum
	, long _teamLossValue
	, long _guildId
	, String _cname
	, String _guildBName
) {	cid = _cid;
	teamId = _teamId;
	teamSoldierPower = _teamSoldierPower;
	teamTroopNum = _teamTroopNum;
	teamLossValue = _teamLossValue;
	guildId = _guildId;
	cname = _cname;
	guildBName = _guildBName;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 玩家队伍ID */
public long getTeamId() { return teamId; }
/** 玩家队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 玩家队伍单兵实力 */
public long getTeamSoldierPower() { return teamSoldierPower; }
/** 玩家队伍单兵实力 */
public void setTeamSoldierPower(long _teamSoldierPower) { teamSoldierPower = _teamSoldierPower; }
/** 玩家队伍带兵量 */
public long getTeamTroopNum() { return teamTroopNum; }
/** 玩家队伍带兵量 */
public void setTeamTroopNum(long _teamTroopNum) { teamTroopNum = _teamTroopNum; }
/** 玩家队伍损耗量 */
public long getTeamLossValue() { return teamLossValue; }
/** 玩家队伍损耗量 */
public void setTeamLossValue(long _teamLossValue) { teamLossValue = _teamLossValue; }
/** 联盟ID */
public long getGuildId() { return guildId; }
/** 联盟ID */
public void setGuildId(long _guildId) { guildId = _guildId; }
/** 玩家昵称 */
public String getCname() { return cname; }
/** 玩家昵称 */
public void setCname(String _cname) { cname = _cname; }
/** 联盟简称 */
public String getGuildBName() { return guildBName; }
/** 联盟简称 */
public void setGuildBName(String _guildBName) { guildBName = _guildBName; }


public final int GetBufSize() {
	int _size = 48;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildBName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 50;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildBName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamSoldierPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamTroopNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamLossValue = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cname = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildBName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(teamId);
	_buf.putLong(teamSoldierPower);
	_buf.putLong(teamTroopNum);
	_buf.putLong(teamLossValue);
	_buf.putLong(guildId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, cname);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildBName);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

