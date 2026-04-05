package Common.GachaObj;

import java.nio.ByteBuffer;
/*********
 * 抽卡保底信息
 **/
public class Gacha_GuaranteeInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 保底规则id */
private long guaranteeId;
/** 剩余次数 */
private int remianTimes;


public Gacha_GuaranteeInfo() {
	guaranteeId = (long)0;
	remianTimes = 0;
}

public Gacha_GuaranteeInfo(
	 long _guaranteeId
	, int _remianTimes
) {	guaranteeId = _guaranteeId;
	remianTimes = _remianTimes;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 保底规则id */
public long getGuaranteeId() { return guaranteeId; }
/** 保底规则id */
public void setGuaranteeId(long _guaranteeId) { guaranteeId = _guaranteeId; }
/** 剩余次数 */
public int getRemianTimes() { return remianTimes; }
/** 剩余次数 */
public void setRemianTimes(int _remianTimes) { remianTimes = _remianTimes; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guaranteeId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) remianTimes = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guaranteeId);
	_buf.putInt(remianTimes);
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

