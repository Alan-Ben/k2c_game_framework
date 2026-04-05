package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildAddGuildPoint_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long guildId;
private int addPoint;
private int contextType;
private long contextGuid;


public GuildAddGuildPoint_Req() {
	cid = (long)0;
	guildId = (long)0;
	addPoint = 0;
	contextType = 0;
	contextGuid = (long)0;
}

public GuildAddGuildPoint_Req(
	 long _cid
	, long _guildId
	, int _addPoint
	, int _contextType
	, long _contextGuid
) {	cid = _cid;
	guildId = _guildId;
	addPoint = _addPoint;
	contextType = _contextType;
	contextGuid = _contextGuid;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }
public int getAddPoint() { return addPoint; }
public void setAddPoint(int _addPoint) { addPoint = _addPoint; }
public int getContextType() { return contextType; }
public void setContextType(int _contextType) { contextType = _contextType; }
public long getContextGuid() { return contextGuid; }
public void setContextGuid(long _contextGuid) { contextGuid = _contextGuid; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addPoint = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) contextType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) contextGuid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(guildId);
	_buf.putInt(addPoint);
	_buf.putInt(contextType);
	_buf.putLong(contextGuid);
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

