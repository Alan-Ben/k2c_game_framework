package GS2GC.p024_DungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_024_005_RetMiddayDungeonBoxCanDraw implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否可领取 */
private boolean canDraw;
/** 剩余可领取次数 */
private int remainDrawCount;


public GS2GC_024_005_RetMiddayDungeonBoxCanDraw() {
	canDraw = false;
	remainDrawCount = 0;
}

public GS2GC_024_005_RetMiddayDungeonBoxCanDraw(
	 boolean _canDraw
	, int _remainDrawCount
) {	canDraw = _canDraw;
	remainDrawCount = _remainDrawCount;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)5; }

/** 是否可领取 */
public boolean getCanDraw() { return canDraw; }
/** 是否可领取 */
public void setCanDraw(boolean _canDraw) { canDraw = _canDraw; }
/** 剩余可领取次数 */
public int getRemainDrawCount() { return remainDrawCount; }
/** 剩余可领取次数 */
public void setRemainDrawCount(int _remainDrawCount) { remainDrawCount = _remainDrawCount; }


public final int GetBufSize() {
	int _size = 5;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 7;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) canDraw = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) remainDrawCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(canDraw?(byte)1:(byte)0);
	_buf.putInt(remainDrawCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)5);
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

