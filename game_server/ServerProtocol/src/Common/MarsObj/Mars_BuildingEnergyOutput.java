package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星建筑-建筑能源产出
 **/
public class Mars_BuildingEnergyOutput implements ALBasicProtocolPack._IALProtocolStructure {
/** 建筑ID */
private long buildingId;
/** 已产出数量 */
private long output;
/** 上次结算时间（毫秒） */
private long lastSettleMs;


public Mars_BuildingEnergyOutput() {
	buildingId = (long)0;
	output = (long)0;
	lastSettleMs = (long)0;
}

public Mars_BuildingEnergyOutput(
	 long _buildingId
	, long _output
	, long _lastSettleMs
) {	buildingId = _buildingId;
	output = _output;
	lastSettleMs = _lastSettleMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 已产出数量 */
public long getOutput() { return output; }
/** 已产出数量 */
public void setOutput(long _output) { output = _output; }
/** 上次结算时间（毫秒） */
public long getLastSettleMs() { return lastSettleMs; }
/** 上次结算时间（毫秒） */
public void setLastSettleMs(long _lastSettleMs) { lastSettleMs = _lastSettleMs; }


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
	if(_buf.remaining() > 0) output = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastSettleMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.putLong(output);
	_buf.putLong(lastSettleMs);
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

