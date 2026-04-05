package GS2GC.p006_BagItemOp;

import java.nio.ByteBuffer;
public class GS2GC_006_051_PushRemoveBagItem implements ALBasicProtocolPack._IALProtocolStructure {
private long itemId;


public GS2GC_006_051_PushRemoveBagItem() {
	itemId = (long)0;
}

public GS2GC_006_051_PushRemoveBagItem(
	 long _itemId
) {	itemId = _itemId;
}

public final byte getMainOrder() { return (byte)6; }

public final byte getSubOrder() { return (byte)51; }

public long getItemId() { return itemId; }
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
	_buf.put((byte)6);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)51);
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

