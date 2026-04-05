package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星建筑-主基地
 **/
public class Mars_HomeBuilding implements ALBasicProtocolPack._IALProtocolStructure {
/** 建筑ID */
private long buildingId;
/** 普通功率开启 */
private boolean isNormalOn;
/** 最高功率开启 */
private boolean isOverdriveOn;
/** 最后一次收集资源的时间（秒） */
private int lastCollectTimeS;


public Mars_HomeBuilding() {
	buildingId = (long)0;
	isNormalOn = false;
	isOverdriveOn = false;
	lastCollectTimeS = 0;
}

public Mars_HomeBuilding(
	 long _buildingId
	, boolean _isNormalOn
	, boolean _isOverdriveOn
	, int _lastCollectTimeS
) {	buildingId = _buildingId;
	isNormalOn = _isNormalOn;
	isOverdriveOn = _isOverdriveOn;
	lastCollectTimeS = _lastCollectTimeS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 普通功率开启 */
public boolean getIsNormalOn() { return isNormalOn; }
/** 普通功率开启 */
public void setIsNormalOn(boolean _isNormalOn) { isNormalOn = _isNormalOn; }
/** 最高功率开启 */
public boolean getIsOverdriveOn() { return isOverdriveOn; }
/** 最高功率开启 */
public void setIsOverdriveOn(boolean _isOverdriveOn) { isOverdriveOn = _isOverdriveOn; }
/** 最后一次收集资源的时间（秒） */
public int getLastCollectTimeS() { return lastCollectTimeS; }
/** 最后一次收集资源的时间（秒） */
public void setLastCollectTimeS(int _lastCollectTimeS) { lastCollectTimeS = _lastCollectTimeS; }


public final int GetBufSize() {
	int _size = 14;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 16;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isNormalOn = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOverdriveOn = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastCollectTimeS = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.put(isNormalOn?(byte)1:(byte)0);
	_buf.put(isOverdriveOn?(byte)1:(byte)0);
	_buf.putInt(lastCollectTimeS);
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

