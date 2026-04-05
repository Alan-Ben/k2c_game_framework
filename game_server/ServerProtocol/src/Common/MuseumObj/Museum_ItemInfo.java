package Common.MuseumObj;

import java.nio.ByteBuffer;
/*********
 * 博物馆_物品信息
 **/
public class Museum_ItemInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 物品ID */
private long itemId;
/** 是否激活 */
private boolean isActive;
/** 等级 */
private int level;
/** 获得时间(毫秒) */
private long gainTimeMs;


public Museum_ItemInfo() {
	itemId = (long)0;
	isActive = false;
	level = 0;
	gainTimeMs = (long)0;
}

public Museum_ItemInfo(
	 long _itemId
	, boolean _isActive
	, int _level
	, long _gainTimeMs
) {	itemId = _itemId;
	isActive = _isActive;
	level = _level;
	gainTimeMs = _gainTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 物品ID */
public long getItemId() { return itemId; }
/** 物品ID */
public void setItemId(long _itemId) { itemId = _itemId; }
/** 是否激活 */
public boolean getIsActive() { return isActive; }
/** 是否激活 */
public void setIsActive(boolean _isActive) { isActive = _isActive; }
/** 等级 */
public int getLevel() { return level; }
/** 等级 */
public void setLevel(int _level) { level = _level; }
/** 获得时间(毫秒) */
public long getGainTimeMs() { return gainTimeMs; }
/** 获得时间(毫秒) */
public void setGainTimeMs(long _gainTimeMs) { gainTimeMs = _gainTimeMs; }


public final int GetBufSize() {
	int _size = 21;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isActive = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(itemId);
	_buf.put(isActive?(byte)1:(byte)0);
	_buf.putInt(level);
	_buf.putLong(gainTimeMs);
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

