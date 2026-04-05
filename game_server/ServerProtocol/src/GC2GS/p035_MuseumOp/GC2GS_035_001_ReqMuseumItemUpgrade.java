package GC2GS.p035_MuseumOp;

import java.nio.ByteBuffer;
/*********
 * 博物馆物品升级
 **/
public class GC2GS_035_001_ReqMuseumItemUpgrade implements ALBasicProtocolPack._IALProtocolStructure {
/** 物品ID */
private long itemId;


public GC2GS_035_001_ReqMuseumItemUpgrade() {
	itemId = (long)0;
}

public GC2GS_035_001_ReqMuseumItemUpgrade(
	 long _itemId
) {	itemId = _itemId;
}

public final byte getMainOrder() { return (byte)35; }

public final byte getSubOrder() { return (byte)1; }

/** 物品ID */
public long getItemId() { return itemId; }
/** 物品ID */
public void setItemId(long _itemId) { itemId = _itemId; }


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
	if(_buf.remaining() > 0) itemId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(itemId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)35);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)35);
	_recBuf.put((byte)1);
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

