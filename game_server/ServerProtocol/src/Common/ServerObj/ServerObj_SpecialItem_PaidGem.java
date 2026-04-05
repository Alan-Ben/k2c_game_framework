package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 特殊物品付费钻石数据
 **/
public class ServerObj_SpecialItem_PaidGem implements ALBasicProtocolPack._IALProtocolStructure {
/** 数量 */
private long count;
/** 总获得数量 */
private long totalGainCount;


public ServerObj_SpecialItem_PaidGem() {
	count = (long)0;
	totalGainCount = (long)0;
}

public ServerObj_SpecialItem_PaidGem(
	 long _count
	, long _totalGainCount
) {	count = _count;
	totalGainCount = _totalGainCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数量 */
public long getCount() { return count; }
/** 数量 */
public void setCount(long _count) { count = _count; }
/** 总获得数量 */
public long getTotalGainCount() { return totalGainCount; }
/** 总获得数量 */
public void setTotalGainCount(long _totalGainCount) { totalGainCount = _totalGainCount; }


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
	if(_buf.remaining() > 0) count = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalGainCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(count);
	_buf.putLong(totalGainCount);
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

