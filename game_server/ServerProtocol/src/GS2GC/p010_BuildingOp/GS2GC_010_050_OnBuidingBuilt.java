package GS2GC.p010_BuildingOp;

import java.nio.ByteBuffer;
/*********
 * 推送建筑完成
 **/
public class GS2GC_010_050_OnBuidingBuilt implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private long buildingId;


public GS2GC_010_050_OnBuidingBuilt() {
	buildingId = (long)0;
}

public GS2GC_010_050_OnBuidingBuilt(
	 long _buildingId
) {	buildingId = _buildingId;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)50; }

/** 空 */
public long getBuildingId() { return buildingId; }
/** 空 */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buildingId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)50);
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

