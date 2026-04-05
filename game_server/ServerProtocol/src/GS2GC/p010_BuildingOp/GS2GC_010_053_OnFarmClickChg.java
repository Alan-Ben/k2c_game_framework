package GS2GC.p010_BuildingOp;

import java.nio.ByteBuffer;
/*********
 * 推送农田建筑点击数据变更
 **/
public class GS2GC_010_053_OnFarmClickChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 建筑ID */
private long buildingId;
/** 上次点击时间点（毫秒） */
private long lastClickMS;


public GS2GC_010_053_OnFarmClickChg() {
	buildingId = (long)0;
	lastClickMS = (long)0;
}

public GS2GC_010_053_OnFarmClickChg(
	 long _buildingId
	, long _lastClickMS
) {	buildingId = _buildingId;
	lastClickMS = _lastClickMS;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)53; }

/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 上次点击时间点（毫秒） */
public long getLastClickMS() { return lastClickMS; }
/** 上次点击时间点（毫秒） */
public void setLastClickMS(long _lastClickMS) { lastClickMS = _lastClickMS; }


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
	if(_buf.remaining() > 0) buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastClickMS = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.putLong(lastClickMS);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)53);
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

