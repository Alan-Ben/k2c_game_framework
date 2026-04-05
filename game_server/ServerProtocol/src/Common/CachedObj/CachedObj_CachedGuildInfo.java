package Common.CachedObj;

import java.nio.ByteBuffer;
public class CachedObj_CachedGuildInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 联盟id */
private long guildId;
/** 联盟名 */
private String guildName;
/** 联盟简称 */
private String guildSimpleName;


public CachedObj_CachedGuildInfo() {
	guildId = (long)0;
	guildName = "";
	guildSimpleName = "";
}

public CachedObj_CachedGuildInfo(
	 long _guildId
	, String _guildName
	, String _guildSimpleName
) {	guildId = _guildId;
	guildName = _guildName;
	guildSimpleName = _guildSimpleName;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 联盟id */
public long getGuildId() { return guildId; }
/** 联盟id */
public void setGuildId(long _guildId) { guildId = _guildId; }
/** 联盟名 */
public String getGuildName() { return guildName; }
/** 联盟名 */
public void setGuildName(String _guildName) { guildName = _guildName; }
/** 联盟简称 */
public String getGuildSimpleName() { return guildSimpleName; }
/** 联盟简称 */
public void setGuildSimpleName(String _guildSimpleName) { guildSimpleName = _guildSimpleName; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildSimpleName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildSimpleName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildSimpleName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guildId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildSimpleName);
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

