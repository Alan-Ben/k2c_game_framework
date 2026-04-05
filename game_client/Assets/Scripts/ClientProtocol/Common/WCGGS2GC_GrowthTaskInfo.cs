using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_GrowthTaskInfo : ALBasicProtocolPack._IALProtocolStructure {
private long taskRefId;
private long curCounter;
private bool hasTookReward;


public WCGGS2GC_GrowthTaskInfo() {
	taskRefId = (long)0;
	curCounter = (long)0;
	hasTookReward = false;
}

public WCGGS2GC_GrowthTaskInfo(
	long _taskRefId
	, long _curCounter
	, bool _hasTookReward
) {	taskRefId = _taskRefId;
	curCounter = _curCounter;
	hasTookReward = _hasTookReward;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getTaskRefId() { return taskRefId; }
public void setTaskRefId(long _taskRefId) { taskRefId = _taskRefId; }
public long getCurCounter() { return curCounter; }
public void setCurCounter(long _curCounter) { curCounter = _curCounter; }
public bool getHasTookReward() { return hasTookReward; }
public void setHasTookReward(bool _hasTookReward) { hasTookReward = _hasTookReward; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	taskRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curCounter = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasTookReward = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(taskRefId);
	_buf.putLong(curCounter);
	_buf.put(hasTookReward?(byte)1:(byte)0);
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
	builder.Append("taskRefId").Append(":").Append(taskRefId.ToString()).Append(", ");
	builder.Append("curCounter").Append(":").Append(curCounter.ToString()).Append(", ");
	builder.Append("hasTookReward").Append(":").Append(hasTookReward.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

