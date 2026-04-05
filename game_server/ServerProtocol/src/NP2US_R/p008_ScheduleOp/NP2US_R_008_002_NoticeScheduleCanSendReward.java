package NP2US_R.p008_ScheduleOp;

import java.nio.ByteBuffer;
/*********
 * 通知排期可以开始领奖
 **/
public class NP2US_R_008_002_NoticeScheduleCanSendReward implements ALBasicProtocolPack._IALProtocolStructure {
private long scheduleId;


public NP2US_R_008_002_NoticeScheduleCanSendReward() {
	scheduleId = (long)0;
}

public NP2US_R_008_002_NoticeScheduleCanSendReward(
	 long _scheduleId
) {	scheduleId = _scheduleId;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)2; }

public long getScheduleId() { return scheduleId; }
public void setScheduleId(long _scheduleId) { scheduleId = _scheduleId; }


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
	if(_buf.remaining() > 0) scheduleId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(scheduleId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)2);
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

