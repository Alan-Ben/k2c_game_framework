package GC2GS.p006_BagItemOp;

import java.nio.ByteBuffer;
public class GC2GS_006_007_ReqConvert implements ALBasicProtocolPack._IALProtocolStructure {
/** 原道具id */
private long bagItemId;
/** 想要转换获得的目标道具数量 */
private long bagItemCount;


public GC2GS_006_007_ReqConvert() {
	bagItemId = (long)0;
	bagItemCount = (long)0;
}

public GC2GS_006_007_ReqConvert(
	 long _bagItemId
	, long _bagItemCount
) {	bagItemId = _bagItemId;
	bagItemCount = _bagItemCount;
}

public final byte getMainOrder() { return (byte)6; }

public final byte getSubOrder() { return (byte)7; }

/** 原道具id */
public long getBagItemId() { return bagItemId; }
/** 原道具id */
public void setBagItemId(long _bagItemId) { bagItemId = _bagItemId; }
/** 想要转换获得的目标道具数量 */
public long getBagItemCount() { return bagItemCount; }
/** 想要转换获得的目标道具数量 */
public void setBagItemCount(long _bagItemCount) { bagItemCount = _bagItemCount; }


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
	if(_buf.remaining() > 0) bagItemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bagItemCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(bagItemId);
	_buf.putLong(bagItemCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)7);
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

