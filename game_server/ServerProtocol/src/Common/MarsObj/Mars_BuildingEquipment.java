package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星建筑-部件
 **/
public class Mars_BuildingEquipment implements ALBasicProtocolPack._IALProtocolStructure {
/** 建筑ID */
private long buildingId;
/** 部件ID */
private long equipmentId;
/** 建筑部件等级 */
private int lvl;


public Mars_BuildingEquipment() {
	buildingId = (long)0;
	equipmentId = (long)0;
	lvl = 0;
}

public Mars_BuildingEquipment(
	 long _buildingId
	, long _equipmentId
	, int _lvl
) {	buildingId = _buildingId;
	equipmentId = _equipmentId;
	lvl = _lvl;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 部件ID */
public long getEquipmentId() { return equipmentId; }
/** 部件ID */
public void setEquipmentId(long _equipmentId) { equipmentId = _equipmentId; }
/** 建筑部件等级 */
public int getLvl() { return lvl; }
/** 建筑部件等级 */
public void setLvl(int _lvl) { lvl = _lvl; }


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
	if(_buf.remaining() > 0) buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) equipmentId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.putLong(equipmentId);
	_buf.putInt(lvl);
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

