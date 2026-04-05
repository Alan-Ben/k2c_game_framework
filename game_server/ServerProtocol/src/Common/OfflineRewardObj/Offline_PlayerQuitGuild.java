package Common.OfflineRewardObj;

import java.nio.ByteBuffer;
/*********
 * 玩家退出联盟
 **/
public class Offline_PlayerQuitGuild implements ALBasicProtocolPack._IALProtocolStructure {
/** 原联盟ID */
private long oriGuildId;
private String oriGuildName;
private long timestamp;
/** 是否被踢出 */
private boolean isKick;


public Offline_PlayerQuitGuild() {
	oriGuildId = (long)0;
	oriGuildName = "";
	timestamp = (long)0;
	isKick = false;
}

public Offline_PlayerQuitGuild(
	 long _oriGuildId
	, String _oriGuildName
	, long _timestamp
	, boolean _isKick
) {	oriGuildId = _oriGuildId;
	oriGuildName = _oriGuildName;
	timestamp = _timestamp;
	isKick = _isKick;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 原联盟ID */
public long getOriGuildId() { return oriGuildId; }
/** 原联盟ID */
public void setOriGuildId(long _oriGuildId) { oriGuildId = _oriGuildId; }
public String getOriGuildName() { return oriGuildName; }
public void setOriGuildName(String _oriGuildName) { oriGuildName = _oriGuildName; }
public long getTimestamp() { return timestamp; }
public void setTimestamp(long _timestamp) { timestamp = _timestamp; }
/** 是否被踢出 */
public boolean getIsKick() { return isKick; }
/** 是否被踢出 */
public void setIsKick(boolean _isKick) { isKick = _isKick; }


public final int GetBufSize() {
	int _size = 17;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(oriGuildName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(oriGuildName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) oriGuildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) oriGuildName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timestamp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isKick = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(oriGuildId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, oriGuildName);
	_buf.putLong(timestamp);
	_buf.put(isKick?(byte)1:(byte)0);
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

