package Common;

import java.nio.ByteBuffer;
public class Common_ActivityInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long activity;
private int curStep;
private int curStepDoneTime;
private boolean curServerDoneState;
private long curCounter;
private boolean isReaded;


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
	, boolean _curServerDoneState
	, long _curCounter
	, boolean _isReaded
) {	activity = _activity;
	curStep = _curStep;
	curStepDoneTime = _curStepDoneTime;
	curServerDoneState = _curServerDoneState;
	curCounter = _curCounter;
	isReaded = _isReaded;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getActivity() { return activity; }
public void setActivity(long _activity) { activity = _activity; }
public int getCurStep() { return curStep; }
public void setCurStep(int _curStep) { curStep = _curStep; }
public int getCurStepDoneTime() { return curStepDoneTime; }
public void setCurStepDoneTime(int _curStepDoneTime) { curStepDoneTime = _curStepDoneTime; }
public boolean getCurServerDoneState() { return curServerDoneState; }
public void setCurServerDoneState(boolean _curServerDoneState) { curServerDoneState = _curServerDoneState; }
public long getCurCounter() { return curCounter; }
public void setCurCounter(long _curCounter) { curCounter = _curCounter; }
public boolean getIsReaded() { return isReaded; }
public void setIsReaded(boolean _isReaded) { isReaded = _isReaded; }


public final int GetBufSize() {
	int _size = 26;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 28;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activity = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curStepDoneTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curServerDoneState = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curCounter = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isReaded = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activity);
	_buf.putInt(curStep);
	_buf.putInt(curStepDoneTime);
	_buf.put(curServerDoneState?(byte)1:(byte)0);
	_buf.putLong(curCounter);
	_buf.put(isReaded?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

