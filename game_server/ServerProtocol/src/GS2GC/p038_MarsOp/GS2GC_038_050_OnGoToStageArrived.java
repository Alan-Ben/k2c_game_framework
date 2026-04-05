package GS2GC.p038_MarsOp;

import java.nio.ByteBuffer;
/*********
 * 前往火星-到达新阶段
 **/
public class GS2GC_038_050_OnGoToStageArrived implements ALBasicProtocolPack._IALProtocolStructure {
/** 当前阶段 */
private int stage;
/** 开启前往火星时间（毫秒） */
private long startMs;
/** 当前阶段开始时间（毫秒） */
private long stageStartMs;


public GS2GC_038_050_OnGoToStageArrived() {
	stage = 0;
	startMs = (long)0;
	stageStartMs = (long)0;
}

public GS2GC_038_050_OnGoToStageArrived(
	 int _stage
	, long _startMs
	, long _stageStartMs
) {	stage = _stage;
	startMs = _startMs;
	stageStartMs = _stageStartMs;
}

public final byte getMainOrder() { return (byte)38; }

public final byte getSubOrder() { return (byte)50; }

/** 当前阶段 */
public int getStage() { return stage; }
/** 当前阶段 */
public void setStage(int _stage) { stage = _stage; }
/** 开启前往火星时间（毫秒） */
public long getStartMs() { return startMs; }
/** 开启前往火星时间（毫秒） */
public void setStartMs(long _startMs) { startMs = _startMs; }
/** 当前阶段开始时间（毫秒） */
public long getStageStartMs() { return stageStartMs; }
/** 当前阶段开始时间（毫秒） */
public void setStageStartMs(long _stageStartMs) { stageStartMs = _stageStartMs; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stage = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stageStartMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(stage);
	_buf.putLong(startMs);
	_buf.putLong(stageStartMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)38);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)38);
	_recBuf.put((byte)50);
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

