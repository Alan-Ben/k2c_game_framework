package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_BagItemInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long itemId;
private long itemCount;
private int lastGetTimeS;
/** 首次获得本物品的时间戳 */
private int newItemTimeS;
/** 最后一次点击本物品的时间戳 */
private int clickItemTimeS;


public NPCommon_BagItemInfo() {
	itemId = (long)0;
	itemCount = (long)0;
	lastGetTimeS = 0;
	newItemTimeS = 0;
	clickItemTimeS = 0;
}

public NPCommon_BagItemInfo(
	 long _itemId
	, long _itemCount
	, int _lastGetTimeS
	, int _newItemTimeS
	, int _clickItemTimeS
) {	itemId = _itemId;
	itemCount = _itemCount;
	lastGetTimeS = _lastGetTimeS;
	newItemTimeS = _newItemTimeS;
	clickItemTimeS = _clickItemTimeS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getItemId() { return itemId; }
public void setItemId(long _itemId) { itemId = _itemId; }
public long getItemCount() { return itemCount; }
public void setItemCount(long _itemCount) { itemCount = _itemCount; }
public int getLastGetTimeS() { return lastGetTimeS; }
public void setLastGetTimeS(int _lastGetTimeS) { lastGetTimeS = _lastGetTimeS; }
/** 首次获得本物品的时间戳 */
public int getNewItemTimeS() { return newItemTimeS; }
/** 首次获得本物品的时间戳 */
public void setNewItemTimeS(int _newItemTimeS) { newItemTimeS = _newItemTimeS; }
/** 最后一次点击本物品的时间戳 */
public int getClickItemTimeS() { return clickItemTimeS; }
/** 最后一次点击本物品的时间戳 */
public void setClickItemTimeS(int _clickItemTimeS) { clickItemTimeS = _clickItemTimeS; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastGetTimeS = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) newItemTimeS = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clickItemTimeS = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(itemId);
	_buf.putLong(itemCount);
	_buf.putInt(lastGetTimeS);
	_buf.putInt(newItemTimeS);
	_buf.putInt(clickItemTimeS);
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

