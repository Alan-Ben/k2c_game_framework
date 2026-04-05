package GS2GC.p033_SimpleActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_033_103_OnSevenDayGoalTaskChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务信息 */
private Common.SimpleActivityObj.SevenDayGoals_TaskInfo taskInfo;


public GS2GC_033_103_OnSevenDayGoalTaskChg() {
	taskInfo = new Common.SimpleActivityObj.SevenDayGoals_TaskInfo();
}

public GS2GC_033_103_OnSevenDayGoalTaskChg(
	 Common.SimpleActivityObj.SevenDayGoals_TaskInfo _taskInfo
) {	taskInfo = _taskInfo;
}

public final byte getMainOrder() { return (byte)33; }

public final byte getSubOrder() { return (byte)103; }

/** 任务信息 */
public Common.SimpleActivityObj.SevenDayGoals_TaskInfo getTaskInfo() { return taskInfo; }
/** 任务信息 */
public void setTaskInfo(Common.SimpleActivityObj.SevenDayGoals_TaskInfo _taskInfo) { taskInfo = _taskInfo; }


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
	if(_buf.remaining() <= 0) return;
	int _taskInfoCustLen = _buf.getInt();
	int _taskInfoCurPos = _buf.position();
	taskInfo.ReadUnzipBuf(_buf, _taskInfoCurPos + _taskInfoCustLen);
	_buf.position(_taskInfoCurPos + _taskInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(taskInfo.GetBufSize());
	taskInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)103);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)103);
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

