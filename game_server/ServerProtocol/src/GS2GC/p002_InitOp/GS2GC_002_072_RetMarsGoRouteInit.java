package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 火星-前往火星数据初始化
 **/
public class GS2GC_002_072_RetMarsGoRouteInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否全部完成 */
private boolean isAllDone;
/** 当前阶段 */
private int stage;
/** 开始时间 */
private long startMs;
/** 当前阶段开始时间（毫秒） */
private long stageStartMs;


public GS2GC_002_072_RetMarsGoRouteInit() {
	isAllDone = false;
	stage = 0;
	startMs = (long)0;
	stageStartMs = (long)0;
}

public GS2GC_002_072_RetMarsGoRouteInit(
	 boolean _isAllDone
	, int _stage
	, long _startMs
	, long _stageStartMs
) {	isAllDone = _isAllDone;
	stage = _stage;
	startMs = _startMs;
	stageStartMs = _stageStartMs;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)72; }

/** 是否全部完成 */
public boolean getIsAllDone() { return isAllDone; }
/** 是否全部完成 */
public void setIsAllDone(boolean _isAllDone) { isAllDone = _isAllDone; }
/** 当前阶段 */
public int getStage() { return stage; }
/** 当前阶段 */
public void setStage(int _stage) { stage = _stage; }
/** 开始时间 */
public long getStartMs() { return startMs; }
/** 开始时间 */
public void setStartMs(long _startMs) { startMs = _startMs; }
/** 当前阶段开始时间（毫秒） */
public long getStageStartMs() { return stageStartMs; }
/** 当前阶段开始时间（毫秒） */
public void setStageStartMs(long _stageStartMs) { stageStartMs = _stageStartMs; }


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
	if(_buf.remaining() > 0) isAllDone = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stage = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stageStartMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isAllDone?(byte)1:(byte)0);
	_buf.putInt(stage);
	_buf.putLong(startMs);
	_buf.putLong(stageStartMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)72);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)72);
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

