package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_GrowthTaskInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long taskRefId;
private long curCounter;
private boolean hasTookReward;


public WCGGS2GC_GrowthTaskInfo() {
	taskRefId = (long)0;
	curCounter = (long)0;
	hasTookReward = false;
}

public WCGGS2GC_GrowthTaskInfo(
	 long _taskRefId
	, long _curCounter
	, boolean _hasTookReward
) {	taskRefId = _taskRefId;
	curCounter = _curCounter;
	hasTookReward = _hasTookReward;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getTaskRefId() { return taskRefId; }
public void setTaskRefId(long _taskRefId) { taskRefId = _taskRefId; }
public long getCurCounter() { return curCounter; }
public void setCurCounter(long _curCounter) { curCounter = _curCounter; }
public boolean getHasTookReward() { return hasTookReward; }
public void setHasTookReward(boolean _hasTookReward) { hasTookReward = _hasTookReward; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) taskRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curCounter = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasTookReward = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(taskRefId);
	_buf.putLong(curCounter);
	_buf.put(hasTookReward?(byte)1:(byte)0);
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

