package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_BagItemInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long itemId;
private long itemCount;
private int lastGetTimeS;


public WCGGS2GC_BagItemInfo() {
	itemId = (long)0;
	itemCount = (long)0;
	lastGetTimeS = 0;
}

public WCGGS2GC_BagItemInfo(
	 long _itemId
	, long _itemCount
	, int _lastGetTimeS
) {	itemId = _itemId;
	itemCount = _itemCount;
	lastGetTimeS = _lastGetTimeS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getItemId() { return itemId; }
public void setItemId(long _itemId) { itemId = _itemId; }
public long getItemCount() { return itemCount; }
public void setItemCount(long _itemCount) { itemCount = _itemCount; }
public int getLastGetTimeS() { return lastGetTimeS; }
public void setLastGetTimeS(int _lastGetTimeS) { lastGetTimeS = _lastGetTimeS; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastGetTimeS = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(itemId);
	_buf.putLong(itemCount);
	_buf.putInt(lastGetTimeS);
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

