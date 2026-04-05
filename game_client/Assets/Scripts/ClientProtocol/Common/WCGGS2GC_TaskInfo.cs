using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_TaskInfo : ALBasicProtocolPack._IALProtocolStructure {
private long instanceId;
private long taskRefId;
private long curCounter;
private long toReplaceRefId;


public WCGGS2GC_TaskInfo() {
	instanceId = (long)0;
	taskRefId = (long)0;
	curCounter = (long)0;
	toReplaceRefId = (long)0;
}

public WCGGS2GC_TaskInfo(
	long _instanceId
	, long _taskRefId
	, long _curCounter
	, long _toReplaceRefId
) {	instanceId = _instanceId;
	taskRefId = _taskRefId;
	curCounter = _curCounter;
	toReplaceRefId = _toReplaceRefId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getInstanceId() { return instanceId; }
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
public long getTaskRefId() { return taskRefId; }
public void setTaskRefId(long _taskRefId) { taskRefId = _taskRefId; }
public long getCurCounter() { return curCounter; }
public void setCurCounter(long _curCounter) { curCounter = _curCounter; }
public long getToReplaceRefId() { return toReplaceRefId; }
public void setToReplaceRefId(long _toReplaceRefId) { toReplaceRefId = _toReplaceRefId; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	taskRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curCounter = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	toReplaceRefId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(taskRefId);
	_buf.putLong(curCounter);
	_buf.putLong(toReplaceRefId);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("taskRefId").Append(":").Append(taskRefId.ToString()).Append(", ");
	builder.Append("curCounter").Append(":").Append(curCounter.ToString()).Append(", ");
	builder.Append("toReplaceRefId").Append(":").Append(toReplaceRefId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

