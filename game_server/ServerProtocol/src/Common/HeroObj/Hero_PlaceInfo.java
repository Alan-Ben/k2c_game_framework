package Common.HeroObj;

import java.nio.ByteBuffer;
/*********
 * 大臣放置信息
 **/
public class Hero_PlaceInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 所在建筑id */
private long buildingId;
/** 放置序列号 */
private long serial;


public Hero_PlaceInfo() {
	buildingId = (long)0;
	serial = (long)0;
}

public Hero_PlaceInfo(
	 long _buildingId
	, long _serial
) {	buildingId = _buildingId;
	serial = _serial;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 所在建筑id */
public long getBuildingId() { return buildingId; }
/** 所在建筑id */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 放置序列号 */
public long getSerial() { return serial; }
/** 放置序列号 */
public void setSerial(long _serial) { serial = _serial; }


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
	if(_buf.remaining() > 0) serial = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.putLong(serial);
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

