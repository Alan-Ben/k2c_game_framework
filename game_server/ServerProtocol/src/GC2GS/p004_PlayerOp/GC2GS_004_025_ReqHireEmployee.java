package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 请求雇佣员工
 **/
public class GC2GS_004_025_ReqHireEmployee implements ALBasicProtocolPack._IALProtocolStructure {
/** 雇佣数量 */
private int num;
/** 建筑ID */
private long buildingId;


public GC2GS_004_025_ReqHireEmployee() {
	num = 0;
	buildingId = (long)0;
}

public GC2GS_004_025_ReqHireEmployee(
	 int _num
	, long _buildingId
) {	num = _num;
	buildingId = _buildingId;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)25; }

/** 雇佣数量 */
public int getNum() { return num; }
/** 雇佣数量 */
public void setNum(int _num) { num = _num; }
/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buildingId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(num);
	_buf.putLong(buildingId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)25);
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

