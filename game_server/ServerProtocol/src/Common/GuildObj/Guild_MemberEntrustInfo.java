package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟成员委托信息
 **/
public class Guild_MemberEntrustInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
/** 当日处理次数 */
private int dayDealTimes;
/** 处理总次数 */
private int totalDealTimes;


public Guild_MemberEntrustInfo() {
	cid = (long)0;
	dayDealTimes = 0;
	totalDealTimes = 0;
}

public Guild_MemberEntrustInfo(
	 long _cid
	, int _dayDealTimes
	, int _totalDealTimes
) {	cid = _cid;
	dayDealTimes = _dayDealTimes;
	totalDealTimes = _totalDealTimes;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/** 当日处理次数 */
public int getDayDealTimes() { return dayDealTimes; }
/** 当日处理次数 */
public void setDayDealTimes(int _dayDealTimes) { dayDealTimes = _dayDealTimes; }
/** 处理总次数 */
public int getTotalDealTimes() { return totalDealTimes; }
/** 处理总次数 */
public void setTotalDealTimes(int _totalDealTimes) { totalDealTimes = _totalDealTimes; }


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
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dayDealTimes = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalDealTimes = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(dayDealTimes);
	_buf.putInt(totalDealTimes);
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

