using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_AchieveInfo : ALBasicProtocolPack._IALProtocolStructure {
private long achieveId;
private int curStep;
private bool curServerDoneState;
private long curCounter;


public Common_AchieveInfo() {
	achieveId = (long)0;
	curStep = 0;
	curServerDoneState = false;
	curCounter = (long)0;
}

public Common_AchieveInfo(
	long _achieveId
	, int _curStep
	, bool _curServerDoneState
	, long _curCounter
) {	achieveId = _achieveId;
	curStep = _curStep;
	curServerDoneState = _curServerDoneState;
	curCounter = _curCounter;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getAchieveId() { return achieveId; }
public void setAchieveId(long _achieveId) { achieveId = _achieveId; }
public int getCurStep() { return curStep; }
public void setCurStep(int _curStep) { curStep = _curStep; }
public bool getCurServerDoneState() { return curServerDoneState; }
public void setCurServerDoneState(bool _curServerDoneState) { curServerDoneState = _curServerDoneState; }
public long getCurCounter() { return curCounter; }
public void setCurCounter(long _curCounter) { curCounter = _curCounter; }


public int GetBufSize() {
	int _size = 21;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	achieveId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curServerDoneState = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curCounter = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(achieveId);
	_buf.putInt(curStep);
	_buf.put(curServerDoneState?(byte)1:(byte)0);
	_buf.putLong(curCounter);
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
	builder.Append("achieveId").Append(":").Append(achieveId.ToString()).Append(", ");
	builder.Append("curStep").Append(":").Append(curStep.ToString()).Append(", ");
	builder.Append("curServerDoneState").Append(":").Append(curServerDoneState.ToString()).Append(", ");
	builder.Append("curCounter").Append(":").Append(curCounter.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

