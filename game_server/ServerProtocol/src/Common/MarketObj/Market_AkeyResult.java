package Common.MarketObj;

import java.nio.ByteBuffer;
/*********
 * 集市一键经营结果数据
 **/
public class Market_AkeyResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 集市ID */
private long marketId;
/** 倍数 */
private int multiple;
/** 是否暴击 */
private boolean isCrit;


public Market_AkeyResult() {
	marketId = (long)0;
	multiple = 0;
	isCrit = false;
}

public Market_AkeyResult(
	 long _marketId
	, int _multiple
	, boolean _isCrit
) {	marketId = _marketId;
	multiple = _multiple;
	isCrit = _isCrit;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 集市ID */
public long getMarketId() { return marketId; }
/** 集市ID */
public void setMarketId(long _marketId) { marketId = _marketId; }
/** 倍数 */
public int getMultiple() { return multiple; }
/** 倍数 */
public void setMultiple(int _multiple) { multiple = _multiple; }
/** 是否暴击 */
public boolean getIsCrit() { return isCrit; }
/** 是否暴击 */
public void setIsCrit(boolean _isCrit) { isCrit = _isCrit; }


public final int GetBufSize() {
	int _size = 13;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) marketId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) multiple = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isCrit = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(marketId);
	_buf.putInt(multiple);
	_buf.put(isCrit?(byte)1:(byte)0);
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

