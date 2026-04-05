package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星居民-决策数据
 **/
public class Mars_Intelligent implements ALBasicProtocolPack._IALProtocolStructure {
/** 配表ID */
private long id;
/** 冷却结束时间（毫秒） */
private long endMs;


public Mars_Intelligent() {
	id = (long)0;
	endMs = (long)0;
}

public Mars_Intelligent(
	 long _id
	, long _endMs
) {	id = _id;
	endMs = _endMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 配表ID */
public long getId() { return id; }
/** 配表ID */
public void setId(long _id) { id = _id; }
/** 冷却结束时间（毫秒） */
public long getEndMs() { return endMs; }
/** 冷却结束时间（毫秒） */
public void setEndMs(long _endMs) { endMs = _endMs; }


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
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(endMs);
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

