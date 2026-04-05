package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_TaskInfo implements ALBasicProtocolPack._IALProtocolStructure {
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getInstanceId() { return instanceId; }
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
public long getTaskRefId() { return taskRefId; }
public void setTaskRefId(long _taskRefId) { taskRefId = _taskRefId; }
public long getCurCounter() { return curCounter; }
public void setCurCounter(long _curCounter) { curCounter = _curCounter; }
public long getToReplaceRefId() { return toReplaceRefId; }
public void setToReplaceRefId(long _toReplaceRefId) { toReplaceRefId = _toReplaceRefId; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) taskRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curCounter = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) toReplaceRefId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(taskRefId);
	_buf.putLong(curCounter);
	_buf.putLong(toReplaceRefId);
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

