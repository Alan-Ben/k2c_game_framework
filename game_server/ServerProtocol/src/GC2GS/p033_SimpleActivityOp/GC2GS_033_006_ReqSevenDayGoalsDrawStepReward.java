package GC2GS.p033_SimpleActivityOp;

import java.nio.ByteBuffer;
/*********
 * 领取阶段奖励
 **/
public class GC2GS_033_006_ReqSevenDayGoalsDrawStepReward implements ALBasicProtocolPack._IALProtocolStructure {
private long refId;


public GC2GS_033_006_ReqSevenDayGoalsDrawStepReward() {
	refId = (long)0;
}

public GC2GS_033_006_ReqSevenDayGoalsDrawStepReward(
	 long _refId
) {	refId = _refId;
}

public final byte getMainOrder() { return (byte)33; }

public final byte getSubOrder() { return (byte)6; }

public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }


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
	if(_buf.remaining() > 0) refId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)6);
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

