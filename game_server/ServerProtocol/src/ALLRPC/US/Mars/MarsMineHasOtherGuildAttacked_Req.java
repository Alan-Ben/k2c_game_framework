package ALLRPC.US.Mars;

import java.nio.ByteBuffer;
public class MarsMineHasOtherGuildAttacked_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long mineInstanceId;
private long guildId;


public MarsMineHasOtherGuildAttacked_Req() {
	mineInstanceId = (long)0;
	guildId = (long)0;
}

public MarsMineHasOtherGuildAttacked_Req(
	 long _mineInstanceId
	, long _guildId
) {	mineInstanceId = _mineInstanceId;
	guildId = _guildId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getMineInstanceId() { return mineInstanceId; }
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }
public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mineInstanceId);
	_buf.putLong(guildId);
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

