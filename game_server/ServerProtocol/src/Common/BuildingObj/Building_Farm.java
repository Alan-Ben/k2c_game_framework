package Common.BuildingObj;

import java.nio.ByteBuffer;
/*********
 * 农田建筑数据
 **/
public class Building_Farm implements ALBasicProtocolPack._IALProtocolStructure {
/** 建筑ID */
private long buildingId;
/** 等级 */
private int lvl;
/** 上次点击时间点（毫秒） */
private long lastClickMS;


public Building_Farm() {
	buildingId = (long)0;
	lvl = 0;
	lastClickMS = (long)0;
}

public Building_Farm(
	 long _buildingId
	, int _lvl
	, long _lastClickMS
) {	buildingId = _buildingId;
	lvl = _lvl;
	lastClickMS = _lastClickMS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 等级 */
public int getLvl() { return lvl; }
/** 等级 */
public void setLvl(int _lvl) { lvl = _lvl; }
/** 上次点击时间点（毫秒） */
public long getLastClickMS() { return lastClickMS; }
/** 上次点击时间点（毫秒） */
public void setLastClickMS(long _lastClickMS) { lastClickMS = _lastClickMS; }


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
	if(_buf.remaining() > 0) lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastClickMS = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.putInt(lvl);
	_buf.putLong(lastClickMS);
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

