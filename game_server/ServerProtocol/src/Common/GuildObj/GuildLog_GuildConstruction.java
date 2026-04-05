package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟建设
 **/
public class GuildLog_GuildConstruction implements ALBasicProtocolPack._IALProtocolStructure {
private String playerName;
private long refId;
/** 联盟经验 */
private int guildExp;
/** 联盟财富 */
private int guildWealth;
/** 个人联盟币 */
private int guildCoin;


public GuildLog_GuildConstruction() {
	playerName = "";
	refId = (long)0;
	guildExp = 0;
	guildWealth = 0;
	guildCoin = 0;
}

public GuildLog_GuildConstruction(
	 String _playerName
	, long _refId
	, int _guildExp
	, int _guildWealth
	, int _guildCoin
) {	playerName = _playerName;
	refId = _refId;
	guildExp = _guildExp;
	guildWealth = _guildWealth;
	guildCoin = _guildCoin;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public String getPlayerName() { return playerName; }
public void setPlayerName(String _playerName) { playerName = _playerName; }
public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
/** 联盟经验 */
public int getGuildExp() { return guildExp; }
/** 联盟经验 */
public void setGuildExp(int _guildExp) { guildExp = _guildExp; }
/** 联盟财富 */
public int getGuildWealth() { return guildWealth; }
/** 联盟财富 */
public void setGuildWealth(int _guildWealth) { guildWealth = _guildWealth; }
/** 个人联盟币 */
public int getGuildCoin() { return guildCoin; }
/** 个人联盟币 */
public void setGuildCoin(int _guildCoin) { guildCoin = _guildCoin; }


public final int GetBufSize() {
	int _size = 20;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildExp = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildWealth = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildCoin = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playerName);
	_buf.putLong(refId);
	_buf.putInt(guildExp);
	_buf.putInt(guildWealth);
	_buf.putInt(guildCoin);
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

