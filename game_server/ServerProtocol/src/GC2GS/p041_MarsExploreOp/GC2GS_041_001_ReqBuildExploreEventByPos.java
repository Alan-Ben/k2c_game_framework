package GC2GS.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探险-创建事件
 **/
public class GC2GS_041_001_ReqBuildExploreEventByPos implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件生成位置ID，0-随机 */
private long pos;


public GC2GS_041_001_ReqBuildExploreEventByPos() {
	pos = (long)0;
}

public GC2GS_041_001_ReqBuildExploreEventByPos(
	 long _pos
) {	pos = _pos;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)1; }

/** 事件生成位置ID，0-随机 */
public long getPos() { return pos; }
/** 事件生成位置ID，0-随机 */
public void setPos(long _pos) { pos = _pos; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pos = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(pos);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)1);
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

