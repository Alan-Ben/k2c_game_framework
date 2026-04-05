using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_ActivityInfo : ALBasicProtocolPack._IALProtocolStructure {
private long activity;
private int curStep;
private int curStepDoneTime;
private bool curServerDoneState;
private long curCounter;
private bool isReaded;


public Common_ActivityInfo() {
	activity = (long)0;
	curStep = 0;
	curStepDoneTime = 0;
	curServerDoneState = false;
	curCounter = (long)0;
	isReaded = false;
}

public Common_ActivityInfo(
	long _activity
	, int _curStep
	, int _curStepDoneTime
	, bool _curServerDoneState
	, long _curCounter
	, bool _isReaded
) {	activity = _activity;
	curStep = _curStep;
	curStepDoneTime = _curStepDoneTime;
	curServerDoneState = _curServerDoneState;
	curCounter = _curCounter;
	isReaded = _isReaded;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getActivity() { return activity; }
public void setActivity(long _activity) { activity = _activity; }
public int getCurStep() { return curStep; }
public void setCurStep(int _curStep) { curStep = _curStep; }
public int getCurStepDoneTime() { return curStepDoneTime; }
public void setCurStepDoneTime(int _curStepDoneTime) { curStepDoneTime = _curStepDoneTime; }
public bool getCurServerDoneState() { return curServerDoneState; }
public void setCurServerDoneState(bool _curServerDoneState) { curServerDoneState = _curServerDoneState; }
public long getCurCounter() { return curCounter; }
public void setCurCounter(long _curCounter) { curCounter = _curCounter; }
public bool getIsReaded() { return isReaded; }
public void setIsReaded(bool _isReaded) { isReaded = _isReaded; }


public int GetBufSize() {
	int _size = 26;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 28;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activity = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curStepDoneTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curServerDoneState = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curCounter = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isReaded = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activity);
	_buf.putInt(curStep);
	_buf.putInt(curStepDoneTime);
	_buf.put(curServerDoneState?(byte)1:(byte)0);
	_buf.putLong(curCounter);
	_buf.put(isReaded?(byte)1:(byte)0);
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
	builder.Append("activity").Append(":").Append(activity.ToString()).Append(", ");
	builder.Append("curStep").Append(":").Append(curStep.ToString()).Append(", ");
	builder.Append("curStepDoneTime").Append(":").Append(curStepDoneTime.ToString()).Append(", ");
	builder.Append("curServerDoneState").Append(":").Append(curServerDoneState.ToString()).Append(", ");
	builder.Append("curCounter").Append(":").Append(curCounter.ToString()).Append(", ");
	builder.Append("isReaded").Append(":").Append(isReaded.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

