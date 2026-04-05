package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildAddMarsMine_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long guildId;
private long mineId;
private long posId;
private long endShowTimeMS;


public GuildAddMarsMine_Req() {
	cid = (long)0;
	guildId = (long)0;
	mineId = (long)0;
	posId = (long)0;
	endShowTimeMS = (long)0;
}

public GuildAddMarsMine_Req(
	 long _cid
	, long _guildId
	, long _mineId
	, long _posId
	, long _endShowTimeMS
) {	cid = _cid;
	guildId = _guildId;
	mineId = _mineId;
	posId = _posId;
	endShowTimeMS = _endShowTimeMS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }
public long getMineId() { return mineId; }
public void setMineId(long _mineId) { mineId = _mineId; }
public long getPosId() { return posId; }
public void setPosId(long _posId) { posId = _posId; }
public long getEndShowTimeMS() { return endShowTimeMS; }
public void setEndShowTimeMS(long _endShowTimeMS) { endShowTimeMS = _endShowTimeMS; }


public final int GetBufSize() {
	int _size = 40;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) posId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endShowTimeMS = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(guildId);
	_buf.putLong(mineId);
	_buf.putLong(posId);
	_buf.putLong(endShowTimeMS);
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

