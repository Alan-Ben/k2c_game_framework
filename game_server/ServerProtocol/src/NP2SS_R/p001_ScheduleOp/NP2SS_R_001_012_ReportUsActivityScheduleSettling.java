package NP2SS_R.p001_ScheduleOp;

import java.nio.ByteBuffer;
/*********
 * US上报活动进入结算期
 **/
public class NP2SS_R_001_012_ReportUsActivityScheduleSettling implements ALBasicProtocolPack._IALProtocolStructure {
private int usId;
/** 排期ID */
private long scheduleId;
/** US分组ID */
private long usGroupId;


public NP2SS_R_001_012_ReportUsActivityScheduleSettling() {
	usId = 0;
	scheduleId = (long)0;
	usGroupId = (long)0;
}

public NP2SS_R_001_012_ReportUsActivityScheduleSettling(
	 int _usId
	, long _scheduleId
	, long _usGroupId
) {	usId = _usId;
	scheduleId = _scheduleId;
	usGroupId = _usGroupId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)12; }

public int getUsId() { return usId; }
public void setUsId(int _usId) { usId = _usId; }
/** 排期ID */
public long getScheduleId() { return scheduleId; }
/** 排期ID */
public void setScheduleId(long _scheduleId) { scheduleId = _scheduleId; }
/** US分组ID */
public long getUsGroupId() { return usGroupId; }
/** US分组ID */
public void setUsGroupId(long _usGroupId) { usGroupId = _usGroupId; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) scheduleId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usGroupId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(usId);
	_buf.putLong(scheduleId);
	_buf.putLong(usGroupId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)12);
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

