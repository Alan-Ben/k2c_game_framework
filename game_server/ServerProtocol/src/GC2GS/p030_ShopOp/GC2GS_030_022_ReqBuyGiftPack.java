package GC2GS.p030_ShopOp;

import java.nio.ByteBuffer;
/*********
 * 购买礼包
 **/
public class GC2GS_030_022_ReqBuyGiftPack implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包id */
private long packId;


public GC2GS_030_022_ReqBuyGiftPack() {
	packId = (long)0;
}

public GC2GS_030_022_ReqBuyGiftPack(
	 long _packId
) {	packId = _packId;
}

public final byte getMainOrder() { return (byte)30; }

public final byte getSubOrder() { return (byte)22; }

/** 礼包id */
public long getPackId() { return packId; }
/** 礼包id */
public void setPackId(long _packId) { packId = _packId; }


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
	if(_buf.remaining() > 0) packId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(packId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)22);
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

