package GC2GS.p008_TravelOp;

import java.nio.ByteBuffer;
/*********
 * 处理博彩游历事件
 **/
public class GC2GS_008_011_ReqDealGamblingTravel implements ALBasicProtocolPack._IALProtocolStructure {
/** 待处理事件实例ID */
private long instanceId;
/** 押注钻石数量 */
private int betAmount;
/** 是否放弃事件 */
private boolean isAbandon;


public GC2GS_008_011_ReqDealGamblingTravel() {
	instanceId = (long)0;
	betAmount = 0;
	isAbandon = false;
}

public GC2GS_008_011_ReqDealGamblingTravel(
	 long _instanceId
	, int _betAmount
	, boolean _isAbandon
) {	instanceId = _instanceId;
	betAmount = _betAmount;
	isAbandon = _isAbandon;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)11; }

/** 待处理事件实例ID */
public long getInstanceId() { return instanceId; }
/** 待处理事件实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 押注钻石数量 */
public int getBetAmount() { return betAmount; }
/** 押注钻石数量 */
public void setBetAmount(int _betAmount) { betAmount = _betAmount; }
/** 是否放弃事件 */
public boolean getIsAbandon() { return isAbandon; }
/** 是否放弃事件 */
public void setIsAbandon(boolean _isAbandon) { isAbandon = _isAbandon; }


public final int GetBufSize() {
	int _size = 13;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) betAmount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAbandon = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(betAmount);
	_buf.put(isAbandon?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)11);
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

