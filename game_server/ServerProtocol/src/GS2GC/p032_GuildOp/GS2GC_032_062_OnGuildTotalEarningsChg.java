package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_062_OnGuildTotalEarningsChg implements ALBasicProtocolPack._IALProtocolStructure {
private long totalEarnings;


public GS2GC_032_062_OnGuildTotalEarningsChg() {
	totalEarnings = (long)0;
}

public GS2GC_032_062_OnGuildTotalEarningsChg(
	 long _totalEarnings
) {	totalEarnings = _totalEarnings;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)62; }

public long getTotalEarnings() { return totalEarnings; }
public void setTotalEarnings(long _totalEarnings) { totalEarnings = _totalEarnings; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalEarnings = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(totalEarnings);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)62);
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

