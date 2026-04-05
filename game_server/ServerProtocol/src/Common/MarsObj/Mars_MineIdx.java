package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-矿产数据索引
 **/
public class Mars_MineIdx implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;
/** 配置ID */
private long refId;
/** 位置ID */
private long pos;
/** 结束展示时间（毫秒） */
private long endShowMs;
/** 开始展示时间（毫秒） */
private long startShowMs;


public Mars_MineIdx() {
	id = (long)0;
	refId = (long)0;
	pos = (long)0;
	endShowMs = (long)0;
	startShowMs = (long)0;
}

public Mars_MineIdx(
	 long _id
	, long _refId
	, long _pos
	, long _endShowMs
	, long _startShowMs
) {	id = _id;
	refId = _refId;
	pos = _pos;
	endShowMs = _endShowMs;
	startShowMs = _startShowMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 配置ID */
public long getRefId() { return refId; }
/** 配置ID */
public void setRefId(long _refId) { refId = _refId; }
/** 位置ID */
public long getPos() { return pos; }
/** 位置ID */
public void setPos(long _pos) { pos = _pos; }
/** 结束展示时间（毫秒） */
public long getEndShowMs() { return endShowMs; }
/** 结束展示时间（毫秒） */
public void setEndShowMs(long _endShowMs) { endShowMs = _endShowMs; }
/** 开始展示时间（毫秒） */
public long getStartShowMs() { return startShowMs; }
/** 开始展示时间（毫秒） */
public void setStartShowMs(long _startShowMs) { startShowMs = _startShowMs; }


public final int GetBufSize() {
	int _size = 40;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pos = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endShowMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startShowMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(refId);
	_buf.putLong(pos);
	_buf.putLong(endShowMs);
	_buf.putLong(startShowMs);
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

