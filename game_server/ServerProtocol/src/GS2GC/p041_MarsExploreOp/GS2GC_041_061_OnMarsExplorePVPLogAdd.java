package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探索-战报数据增加
 **/
public class GS2GC_041_061_OnMarsExplorePVPLogAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 战报创建时间（毫秒），服务端以此排序 */
private long createdAt;


public GS2GC_041_061_OnMarsExplorePVPLogAdd() {
	createdAt = (long)0;
}

public GS2GC_041_061_OnMarsExplorePVPLogAdd(
	 long _createdAt
) {	createdAt = _createdAt;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)61; }

/** 战报创建时间（毫秒），服务端以此排序 */
public long getCreatedAt() { return createdAt; }
/** 战报创建时间（毫秒），服务端以此排序 */
public void setCreatedAt(long _createdAt) { createdAt = _createdAt; }


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
	if(_buf.remaining() > 0) createdAt = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(createdAt);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)61);
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

