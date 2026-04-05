package GC2GS.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
/*********
 * 火星建筑-部件升级
 **/
public class GC2GS_039_005_ReqUpgradeEquipment implements ALBasicProtocolPack._IALProtocolStructure {
/** 建筑ID */
private long buildingId;
/** 建筑部件ID */
private long equipmentId;


public GC2GS_039_005_ReqUpgradeEquipment() {
	buildingId = (long)0;
	equipmentId = (long)0;
}

public GC2GS_039_005_ReqUpgradeEquipment(
	 long _buildingId
	, long _equipmentId
) {	buildingId = _buildingId;
	equipmentId = _equipmentId;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)5; }

/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 建筑部件ID */
public long getEquipmentId() { return equipmentId; }
/** 建筑部件ID */
public void setEquipmentId(long _equipmentId) { equipmentId = _equipmentId; }


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
	if(_buf.remaining() > 0) equipmentId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.putLong(equipmentId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)5);
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

