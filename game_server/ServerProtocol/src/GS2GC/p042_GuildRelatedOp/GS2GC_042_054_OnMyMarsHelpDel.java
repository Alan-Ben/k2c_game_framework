package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 自身发起的求助数据删除
 **/
public class GS2GC_042_054_OnMyMarsHelpDel implements ALBasicProtocolPack._IALProtocolStructure {
/** 求助实例ID */
private long id;


public GS2GC_042_054_OnMyMarsHelpDel() {
	id = (long)0;
}

public GS2GC_042_054_OnMyMarsHelpDel(
	 long _id
) {	id = _id;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)54; }

/** 求助实例ID */
public long getId() { return id; }
/** 求助实例ID */
public void setId(long _id) { id = _id; }


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
	if(_buf.remaining() > 0) id = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)54);
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

