package NP2SS_R.p001_ScheduleOp;

import java.nio.ByteBuffer;
/*********
 * 命令增加活动排期
 **/
public class NP2SS_R_001_013_CmdAddActivitySchedule implements ALBasicProtocolPack._IALProtocolStructure {
/** 后台排期数据 */
private Common.ScheduleObj.Schedule_PhpInfo scheduleInfo;
/** 是否为GM添加 */
private boolean isGmAdd;


public NP2SS_R_001_013_CmdAddActivitySchedule() {
	scheduleInfo = new Common.ScheduleObj.Schedule_PhpInfo();
	isGmAdd = false;
}

public NP2SS_R_001_013_CmdAddActivitySchedule(
	 Common.ScheduleObj.Schedule_PhpInfo _scheduleInfo
	, boolean _isGmAdd
) {	scheduleInfo = _scheduleInfo;
	isGmAdd = _isGmAdd;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)13; }

/** 后台排期数据 */
public Common.ScheduleObj.Schedule_PhpInfo getScheduleInfo() { return scheduleInfo; }
/** 后台排期数据 */
public void setScheduleInfo(Common.ScheduleObj.Schedule_PhpInfo _scheduleInfo) { scheduleInfo = _scheduleInfo; }
/** 是否为GM添加 */
public boolean getIsGmAdd() { return isGmAdd; }
/** 是否为GM添加 */
public void setIsGmAdd(boolean _isGmAdd) { isGmAdd = _isGmAdd; }


public final int GetBufSize() {
	int _size = 1;
	_size += 4 + scheduleInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += 4 + scheduleInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _scheduleInfoCustLen = _buf.getInt();
	int _scheduleInfoCurPos = _buf.position();
	scheduleInfo.ReadUnzipBuf(_buf, _scheduleInfoCurPos + _scheduleInfoCustLen);
	_buf.position(_scheduleInfoCurPos + _scheduleInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isGmAdd = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(scheduleInfo.GetBufSize());
	scheduleInfo.PutUnzipBuf(_buf);
	_buf.put(isGmAdd?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)13);
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

