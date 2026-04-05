package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildJoin_2C_Return implements ALBasicProtocolPack._IALProtocolStructure {
private long maxEarning;
/** 火星自动互助有效时间，如无则为0 */
private long marsAutoHelpEndTimeMS;


public GuildJoin_2C_Return() {
	maxEarning = (long)0;
	marsAutoHelpEndTimeMS = (long)0;
}

public GuildJoin_2C_Return(
	 long _maxEarning
	, long _marsAutoHelpEndTimeMS
) {	maxEarning = _maxEarning;
	marsAutoHelpEndTimeMS = _marsAutoHelpEndTimeMS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getMaxEarning() { return maxEarning; }
public void setMaxEarning(long _maxEarning) { maxEarning = _maxEarning; }
/** 火星自动互助有效时间，如无则为0 */
public long getMarsAutoHelpEndTimeMS() { return marsAutoHelpEndTimeMS; }
/** 火星自动互助有效时间，如无则为0 */
public void setMarsAutoHelpEndTimeMS(long _marsAutoHelpEndTimeMS) { marsAutoHelpEndTimeMS = _marsAutoHelpEndTimeMS; }


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
	if(_buf.remaining() > 0) maxEarning = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) marsAutoHelpEndTimeMS = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(maxEarning);
	_buf.putLong(marsAutoHelpEndTimeMS);
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

