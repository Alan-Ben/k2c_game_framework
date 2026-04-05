package GC2GS.p010_BuildingOp;

import java.nio.ByteBuffer;
/*********
 * 请求点击农田产出
 **/
public class GC2GS_010_003_ReqFarmClickOutput implements ALBasicProtocolPack._IALProtocolStructure {
/** 建筑ID */
private long buildingId;
/** 当前点击次数 */
private long clickNum;
/** 客户端操作时间戳 */
private long clientOpTimeMs;


public GC2GS_010_003_ReqFarmClickOutput() {
	buildingId = (long)0;
	clickNum = (long)0;
	clientOpTimeMs = (long)0;
}

public GC2GS_010_003_ReqFarmClickOutput(
	 long _buildingId
	, long _clickNum
	, long _clientOpTimeMs
) {	buildingId = _buildingId;
	clickNum = _clickNum;
	clientOpTimeMs = _clientOpTimeMs;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)3; }

/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 当前点击次数 */
public long getClickNum() { return clickNum; }
/** 当前点击次数 */
public void setClickNum(long _clickNum) { clickNum = _clickNum; }
/** 客户端操作时间戳 */
public long getClientOpTimeMs() { return clientOpTimeMs; }
/** 客户端操作时间戳 */
public void setClientOpTimeMs(long _clientOpTimeMs) { clientOpTimeMs = _clientOpTimeMs; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clickNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientOpTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.putLong(clickNum);
	_buf.putLong(clientOpTimeMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)3);
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

