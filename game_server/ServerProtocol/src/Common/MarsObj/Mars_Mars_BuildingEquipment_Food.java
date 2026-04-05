package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星建筑-部件-Food
 **/
public class Mars_Mars_BuildingEquipment_Food implements ALBasicProtocolPack._IALProtocolStructure {
/** 建筑ID */
private long buildingId;
/** 功率开关 */
private boolean isPowerOn;


public Mars_Mars_BuildingEquipment_Food() {
	buildingId = (long)0;
	isPowerOn = false;
}

public Mars_Mars_BuildingEquipment_Food(
	 long _buildingId
	, boolean _isPowerOn
) {	buildingId = _buildingId;
	isPowerOn = _isPowerOn;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 功率开关 */
public boolean getIsPowerOn() { return isPowerOn; }
/** 功率开关 */
public void setIsPowerOn(boolean _isPowerOn) { isPowerOn = _isPowerOn; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isPowerOn = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.put(isPowerOn?(byte)1:(byte)0);
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

