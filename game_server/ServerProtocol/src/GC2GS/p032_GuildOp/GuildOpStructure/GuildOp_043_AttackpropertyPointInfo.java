package GC2GS.p032_GuildOp.GuildOpStructure;

import java.nio.ByteBuffer;
/*********
 * 攻击据点对应属性节点
 **/
public class GuildOp_043_AttackpropertyPointInfo implements ALBasicProtocolPack._IALProtocolStructure {
private String playerName;
private long heroId;
private long power;
/** 是否可获得公会贡献 */
private boolean canGetDevote;


public GuildOp_043_AttackpropertyPointInfo() {
	playerName = "";
	heroId = (long)0;
	power = (long)0;
	canGetDevote = false;
}

public GuildOp_043_AttackpropertyPointInfo(
	 String _playerName
	, long _heroId
	, long _power
	, boolean _canGetDevote
) {	playerName = _playerName;
	heroId = _heroId;
	power = _power;
	canGetDevote = _canGetDevote;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public String getPlayerName() { return playerName; }
public void setPlayerName(String _playerName) { playerName = _playerName; }
public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public long getPower() { return power; }
public void setPower(long _power) { power = _power; }
/** 是否可获得公会贡献 */
public boolean getCanGetDevote() { return canGetDevote; }
/** 是否可获得公会贡献 */
public void setCanGetDevote(boolean _canGetDevote) { canGetDevote = _canGetDevote; }


public final int GetBufSize() {
	int _size = 17;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) power = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) canGetDevote = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playerName);
	_buf.putLong(heroId);
	_buf.putLong(power);
	_buf.put(canGetDevote?(byte)1:(byte)0);
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

