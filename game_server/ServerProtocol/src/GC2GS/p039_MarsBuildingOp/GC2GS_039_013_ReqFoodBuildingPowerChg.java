package GC2GS.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
/*********
 * 火星建筑-基地开关
 **/
public class GC2GS_039_013_ReqFoodBuildingPowerChg implements ALBasicProtocolPack._IALProtocolStructure {
private long buildingId;
private boolean powerOn;


public GC2GS_039_013_ReqFoodBuildingPowerChg() {
	buildingId = (long)0;
	powerOn = false;
}

public GC2GS_039_013_ReqFoodBuildingPowerChg(
	 long _buildingId
	, boolean _powerOn
) {	buildingId = _buildingId;
	powerOn = _powerOn;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)13; }

public long getBuildingId() { return buildingId; }
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
public boolean getPowerOn() { return powerOn; }
public void setPowerOn(boolean _powerOn) { powerOn = _powerOn; }


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
	if(_buf.remaining() > 0) powerOn = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.put(powerOn?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)13);
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

