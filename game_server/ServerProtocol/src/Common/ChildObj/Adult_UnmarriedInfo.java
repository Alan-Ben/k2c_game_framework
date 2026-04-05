package Common.ChildObj;

import java.nio.ByteBuffer;
/*********
 * 子嗣（成年未婚）数据
 **/
public class Adult_UnmarriedInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣数据 */
private Common.ChildObj.Adult_Info adult;
/** 子嗣状态 */
private Common.ChildEnum.EAdultStatus status;
/** 请求过期截至时间（秒） */
private int expiredTs;
/** 允许联姻的最小收益数值 */
private long minAllowBonus;


public Adult_UnmarriedInfo() {
	adult = new Common.ChildObj.Adult_Info();
	status = Common.ChildEnum.EAdultStatus.values()[0];
	expiredTs = 0;
	minAllowBonus = (long)0;
}

public Adult_UnmarriedInfo(
	 Common.ChildObj.Adult_Info _adult
	, Common.ChildEnum.EAdultStatus _status
	, int _expiredTs
	, long _minAllowBonus
) {	adult = _adult;
	status = _status;
	expiredTs = _expiredTs;
	minAllowBonus = _minAllowBonus;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 子嗣数据 */
public Common.ChildObj.Adult_Info getAdult() { return adult; }
/** 子嗣数据 */
public void setAdult(Common.ChildObj.Adult_Info _adult) { adult = _adult; }
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
	int _size = 16;
	_size += 4 + adult.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 4 + adult.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _adultCustLen = _buf.getInt();
	int _adultCurPos = _buf.position();
	adult.ReadUnzipBuf(_buf, _adultCurPos + _adultCustLen);
	_buf.position(_adultCurPos + _adultCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) status = Common.ChildEnum.EAdultStatus.EAdultStatus_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expiredTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) minAllowBonus = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(adult.GetBufSize());
	adult.PutUnzipBuf(_buf);
	_buf.putInt(status.ordinal());

	_buf.putInt(expiredTs);
	_buf.putLong(minAllowBonus);
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

