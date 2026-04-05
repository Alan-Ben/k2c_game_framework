package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探索-最新联盟分享火星矿数据ID
 **/
public class GS2GC_041_063_OnGuildMarsMineShareAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 最新联盟分享火星矿数据ID */
private long id;


public GS2GC_041_063_OnGuildMarsMineShareAdd() {
	id = (long)0;
}

public GS2GC_041_063_OnGuildMarsMineShareAdd(
	 long _id
) {	id = _id;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)63; }

/** 最新联盟分享火星矿数据ID */
public long getId() { return id; }
/** 最新联盟分享火星矿数据ID */
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
	_buf.put((byte)41);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)63);
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

