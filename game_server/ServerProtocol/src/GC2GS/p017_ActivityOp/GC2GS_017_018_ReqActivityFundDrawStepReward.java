package GC2GS.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动基金-请求领取阶段奖励
 **/
public class GC2GS_017_018_ReqActivityFundDrawStepReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 基金ID */
private long fundId;


public GC2GS_017_018_ReqActivityFundDrawStepReward() {
	fundId = (long)0;
}

public GC2GS_017_018_ReqActivityFundDrawStepReward(
	 long _fundId
) {	fundId = _fundId;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)18; }

/** 基金ID */
public long getFundId() { return fundId; }
/** 基金ID */
public void setFundId(long _fundId) { fundId = _fundId; }


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
	if(_buf.remaining() > 0) fundId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(fundId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)18);
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

