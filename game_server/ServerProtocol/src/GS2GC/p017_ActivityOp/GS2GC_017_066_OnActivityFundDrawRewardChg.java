package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动基金-领取奖励后推送
 **/
public class GS2GC_017_066_OnActivityFundDrawRewardChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 基金ID */
private long fundId;
/** 已领取免费档阶段 */
private int hadDrawFreeStep;
/** 已领取付费档阶段 */
private int hadDrawPayStep;


public GS2GC_017_066_OnActivityFundDrawRewardChg() {
	fundId = (long)0;
	hadDrawFreeStep = 0;
	hadDrawPayStep = 0;
}

public GS2GC_017_066_OnActivityFundDrawRewardChg(
	 long _fundId
	, int _hadDrawFreeStep
	, int _hadDrawPayStep
) {	fundId = _fundId;
	hadDrawFreeStep = _hadDrawFreeStep;
	hadDrawPayStep = _hadDrawPayStep;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)66; }

/** 基金ID */
public long getFundId() { return fundId; }
/** 基金ID */
public void setFundId(long _fundId) { fundId = _fundId; }
/** 已领取免费档阶段 */
public int getHadDrawFreeStep() { return hadDrawFreeStep; }
/** 已领取免费档阶段 */
public void setHadDrawFreeStep(int _hadDrawFreeStep) { hadDrawFreeStep = _hadDrawFreeStep; }
/** 已领取付费档阶段 */
public int getHadDrawPayStep() { return hadDrawPayStep; }
/** 已领取付费档阶段 */
public void setHadDrawPayStep(int _hadDrawPayStep) { hadDrawPayStep = _hadDrawPayStep; }


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
	if(_buf.remaining() > 0) fundId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawFreeStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawPayStep = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(fundId);
	_buf.putInt(hadDrawFreeStep);
	_buf.putInt(hadDrawPayStep);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)66);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)66);
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

