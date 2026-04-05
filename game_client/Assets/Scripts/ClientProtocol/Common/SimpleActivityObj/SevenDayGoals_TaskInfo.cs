using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.SimpleActivityObj
{

/// <summary>
/// 七日目标任务信息
/// </summary>
public class SevenDayGoals_TaskInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务ID
/// </summary>
private long taskId;
/// <summary>
/// 额外计数
/// </summary>
private long extraCount;


public SevenDayGoals_TaskInfo() {
	taskId = (long)0;
	extraCount = (long)0;
}

public SevenDayGoals_TaskInfo(
	long _taskId
	, long _extraCount
) {	taskId = _taskId;
	extraCount = _extraCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 任务ID
/// </summary>
public long getTaskId() { return taskId; }
/// <summary>
/// 任务ID
/// </summary>
public void setTaskId(long _taskId) { taskId = _taskId; }
/// <summary>
/// 额外计数
/// </summary>
public long getExtraCount() { return extraCount; }
/// <summary>
/// 额外计数
/// </summary>
public void setExtraCount(long _extraCount) { extraCount = _extraCount; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	taskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extraCount = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(taskId);
	_buf.putLong(extraCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("taskId").Append(":").Append(taskId.ToString()).Append(", ");
	builder.Append("extraCount").Append(":").Append(extraCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

