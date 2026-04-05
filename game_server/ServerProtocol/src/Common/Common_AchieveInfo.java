package Common;

import java.nio.ByteBuffer;
public class Common_AchieveInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long achieveId;
private int curStep;
private boolean curServerDoneState;
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
	, boolean _curServerDoneState
	, long _curCounter
) {	achieveId = _achieveId;
	curStep = _curStep;
	curServerDoneState = _curServerDoneState;
	curCounter = _curCounter;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getAchieveId() { return achieveId; }
public void setAchieveId(long _achieveId) { achieveId = _achieveId; }
public int getCurStep() { return curStep; }
public void setCurStep(int _curStep) { curStep = _curStep; }
public boolean getCurServerDoneState() { return curServerDoneState; }
public void setCurServerDoneState(boolean _curServerDoneState) { curServerDoneState = _curServerDoneState; }
public long getCurCounter() { return curCounter; }
public void setCurCounter(long _curCounter) { curCounter = _curCounter; }


public final int GetBufSize() {
	int _size = 21;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) achieveId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curServerDoneState = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curCounter = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(achieveId);
	_buf.putInt(curStep);
	_buf.put(curServerDoneState?(byte)1:(byte)0);
	_buf.putLong(curCounter);
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

