using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p033_SimpleActivityOp
{

public class GS2GC_033_103_OnSevenDayGoalTaskChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务信息
/// </summary>
private Common.SimpleActivityObj.SevenDayGoals_TaskInfo taskInfo;


public GS2GC_033_103_OnSevenDayGoalTaskChg() {
	taskInfo = new Common.SimpleActivityObj.SevenDayGoals_TaskInfo();
}

public GS2GC_033_103_OnSevenDayGoalTaskChg(
	Common.SimpleActivityObj.SevenDayGoals_TaskInfo _taskInfo
) {	taskInfo = _taskInfo;
}

public byte getMainOrder() { return (byte)33; }

public byte getSubOrder() { return (byte)103; }

/// <summary>
/// 任务信息
/// </summary>
public Common.SimpleActivityObj.SevenDayGoals_TaskInfo getTaskInfo() { return taskInfo; }
/// <summary>
/// 任务信息
/// </summary>
public void setTaskInfo(Common.SimpleActivityObj.SevenDayGoals_TaskInfo _taskInfo) { taskInfo = _taskInfo; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _taskInfoCustLen = _buf.getInt();
	int _taskInfoCurPos = _buf.getCurPos();
	taskInfo.ReadUnzipBuf(_buf, _taskInfoCurPos + _taskInfoCustLen);
	_buf.setPosition(_taskInfoCurPos + _taskInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(taskInfo.GetBufSize());
	taskInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)103);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)103);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("taskInfo").Append(":").Append(taskInfo == null ? "null" : taskInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

