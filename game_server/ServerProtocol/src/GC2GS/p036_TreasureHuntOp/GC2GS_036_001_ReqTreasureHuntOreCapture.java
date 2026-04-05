package GC2GS.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-捕捉矿石
 **/
public class GC2GS_036_001_ReqTreasureHuntOreCapture implements ALBasicProtocolPack._IALProtocolStructure {
/** 捕捉类型 */
private Common.TreasureHuntEnum.ETreasureHuntCaptureType type;
/** 是否高级 */
private boolean isAdvance;
/** 区域ID */
private long areaId;
/** 距离 */
private long distance;


public GC2GS_036_001_ReqTreasureHuntOreCapture() {
	type = Common.TreasureHuntEnum.ETreasureHuntCaptureType.values()[0];
	isAdvance = false;
	areaId = (long)0;
	distance = (long)0;
}

public GC2GS_036_001_ReqTreasureHuntOreCapture(
	 Common.TreasureHuntEnum.ETreasureHuntCaptureType _type
	, boolean _isAdvance
	, long _areaId
	, long _distance
) {	type = _type;
	isAdvance = _isAdvance;
	areaId = _areaId;
	distance = _distance;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)1; }

/** 捕捉类型 */
public Common.TreasureHuntEnum.ETreasureHuntCaptureType getType() { return type; }
/** 捕捉类型 */
public void setType(Common.TreasureHuntEnum.ETreasureHuntCaptureType _type) { type = _type; }
/** 是否高级 */
public boolean getIsAdvance() { return isAdvance; }
/** 是否高级 */
public void setIsAdvance(boolean _isAdvance) { isAdvance = _isAdvance; }
/** 区域ID */
public long getAreaId() { return areaId; }
/** 区域ID */
public void setAreaId(long _areaId) { areaId = _areaId; }
/** 距离 */
public long getDistance() { return distance; }
/** 距离 */
public void setDistance(long _distance) { distance = _distance; }


public final int GetBufSize() {
	int _size = 21;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.TreasureHuntEnum.ETreasureHuntCaptureType.ETreasureHuntCaptureType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAdvance = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) areaId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) distance = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.put(isAdvance?(byte)1:(byte)0);
	_buf.putLong(areaId);
	_buf.putLong(distance);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)1);
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

