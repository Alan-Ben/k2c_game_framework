package GC2GS.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
/*********
 * 火星建筑-派遣居民
 **/
public class GC2GS_039_004_ReqDispatchPeople implements ALBasicProtocolPack._IALProtocolStructure {
private long buildingId;
/** 居民数量 */
private int peopleNum;


public GC2GS_039_004_ReqDispatchPeople() {
	buildingId = (long)0;
	peopleNum = 0;
}

public GC2GS_039_004_ReqDispatchPeople(
	 long _buildingId
	, int _peopleNum
) {	buildingId = _buildingId;
	peopleNum = _peopleNum;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)4; }

public long getBuildingId() { return buildingId; }
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 居民数量 */
public int getPeopleNum() { return peopleNum; }
/** 居民数量 */
public void setPeopleNum(int _peopleNum) { peopleNum = _peopleNum; }


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
	if(_buf.remaining() > 0) buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) peopleNum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.putInt(peopleNum);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)4);
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

