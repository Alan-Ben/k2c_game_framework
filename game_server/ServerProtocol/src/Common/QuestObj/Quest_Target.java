package Common.QuestObj;

import java.nio.ByteBuffer;
/*********
 * 任务步骤目标数据
 **/
public class Quest_Target implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标配置ID */
private long targetId;
/** 当前计数 */
private long curCount;


public Quest_Target() {
	targetId = (long)0;
	curCount = (long)0;
}

public Quest_Target(
	 long _targetId
	, long _curCount
) {	targetId = _targetId;
	curCount = _curCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 目标配置ID */
public long getTargetId() { return targetId; }
/** 目标配置ID */
public void setTargetId(long _targetId) { targetId = _targetId; }
/** 当前计数 */
public long getCurCount() { return curCount; }
/** 当前计数 */
public void setCurCount(long _curCount) { curCount = _curCount; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(targetId);
	_buf.putLong(curCount);
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

