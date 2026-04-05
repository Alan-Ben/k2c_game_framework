package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-组合信息
 **/
public class TreasureHunt_CompositeInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 组合ID */
private long refId;
/** 收集时间 ms */
private long collectTimeMs;
/** 是否激活普通组合 */
private boolean isNormalActive;
/** 是否激活高级组合 */
private boolean isAdvancedActive;


public TreasureHunt_CompositeInfo() {
	refId = (long)0;
	collectTimeMs = (long)0;
	isNormalActive = false;
	isAdvancedActive = false;
}

public TreasureHunt_CompositeInfo(
	 long _refId
	, long _collectTimeMs
	, boolean _isNormalActive
	, boolean _isAdvancedActive
) {	refId = _refId;
	collectTimeMs = _collectTimeMs;
	isNormalActive = _isNormalActive;
	isAdvancedActive = _isAdvancedActive;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 组合ID */
public long getRefId() { return refId; }
/** 组合ID */
public void setRefId(long _refId) { refId = _refId; }
/** 收集时间 ms */
public long getCollectTimeMs() { return collectTimeMs; }
/** 收集时间 ms */
public void setCollectTimeMs(long _collectTimeMs) { collectTimeMs = _collectTimeMs; }
/** 是否激活普通组合 */
public boolean getIsNormalActive() { return isNormalActive; }
/** 是否激活普通组合 */
public void setIsNormalActive(boolean _isNormalActive) { isNormalActive = _isNormalActive; }
/** 是否激活高级组合 */
public boolean getIsAdvancedActive() { return isAdvancedActive; }
/** 是否激活高级组合 */
public void setIsAdvancedActive(boolean _isAdvancedActive) { isAdvancedActive = _isAdvancedActive; }


public final int GetBufSize() {
	int _size = 18;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 20;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) collectTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isNormalActive = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAdvancedActive = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refId);
	_buf.putLong(collectTimeMs);
	_buf.put(isNormalActive?(byte)1:(byte)0);
	_buf.put(isAdvancedActive?(byte)1:(byte)0);
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

