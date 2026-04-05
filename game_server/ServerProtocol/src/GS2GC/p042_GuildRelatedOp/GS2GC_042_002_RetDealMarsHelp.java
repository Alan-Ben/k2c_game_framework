package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
public class GS2GC_042_002_RetDealMarsHelp implements ALBasicProtocolPack._IALProtocolStructure {
private int dealCount;
private long rewardCount;


public GS2GC_042_002_RetDealMarsHelp() {
	dealCount = 0;
	rewardCount = (long)0;
}

public GS2GC_042_002_RetDealMarsHelp(
	 int _dealCount
	, long _rewardCount
) {	dealCount = _dealCount;
	rewardCount = _rewardCount;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)2; }

public int getDealCount() { return dealCount; }
public void setDealCount(int _dealCount) { dealCount = _dealCount; }
public long getRewardCount() { return rewardCount; }
public void setRewardCount(long _rewardCount) { rewardCount = _rewardCount; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rewardCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(dealCount);
	_buf.putLong(rewardCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)2);
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

