package Common.DailyCheckObj;

import java.nio.ByteBuffer;
/*********
 * 每日签到奖励展示信息
 **/
public class DailyCheck_RewardShowInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** daily_check_loop_reward配置ID */
private long refId;
/** 当前天数 */
private int curDay;


public DailyCheck_RewardShowInfo() {
	refId = (long)0;
	curDay = 0;
}

public DailyCheck_RewardShowInfo(
	 long _refId
	, int _curDay
) {	refId = _refId;
	curDay = _curDay;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** daily_check_loop_reward配置ID */
public long getRefId() { return refId; }
/** daily_check_loop_reward配置ID */
public void setRefId(long _refId) { refId = _refId; }
/** 当前天数 */
public int getCurDay() { return curDay; }
/** 当前天数 */
public void setCurDay(int _curDay) { curDay = _curDay; }


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
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curDay = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refId);
	_buf.putInt(curDay);
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

