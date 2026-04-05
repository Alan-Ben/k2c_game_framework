package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 子嗣（成年未婚）状态变更
 **/
public class GS2GC_014_059_OnUnmarriedAdultStatusChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣实例ID */
private long id;
/** 子嗣状态 */
private Common.ChildEnum.EAdultStatus status;
/** 请求过期截至时间（秒） */
private int expiredTs;
/** 允许联姻的最小收益数值 */
private long minAllowBonus;


public GS2GC_014_059_OnUnmarriedAdultStatusChg() {
	id = (long)0;
	status = Common.ChildEnum.EAdultStatus.values()[0];
	expiredTs = 0;
	minAllowBonus = (long)0;
}

public GS2GC_014_059_OnUnmarriedAdultStatusChg(
	 long _id
	, Common.ChildEnum.EAdultStatus _status
	, int _expiredTs
	, long _minAllowBonus
) {	id = _id;
	status = _status;
	expiredTs = _expiredTs;
	minAllowBonus = _minAllowBonus;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)59; }

/** 子嗣实例ID */
public long getId() { return id; }
/** 子嗣实例ID */
public void setId(long _id) { id = _id; }
/** 子嗣状态 */
public Common.ChildEnum.EAdultStatus getStatus() { return status; }
/** 子嗣状态 */
public void setStatus(Common.ChildEnum.EAdultStatus _status) { status = _status; }
/** 请求过期截至时间（秒） */
public int getExpiredTs() { return expiredTs; }
/** 请求过期截至时间（秒） */
public void setExpiredTs(int _expiredTs) { expiredTs = _expiredTs; }
/** 允许联姻的最小收益数值 */
public long getMinAllowBonus() { return minAllowBonus; }
/** 允许联姻的最小收益数值 */
public void setMinAllowBonus(long _minAllowBonus) { minAllowBonus = _minAllowBonus; }


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
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) status = Common.ChildEnum.EAdultStatus.EAdultStatus_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expiredTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) minAllowBonus = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(status.ordinal());

	_buf.putInt(expiredTs);
	_buf.putLong(minAllowBonus);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)59);
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

