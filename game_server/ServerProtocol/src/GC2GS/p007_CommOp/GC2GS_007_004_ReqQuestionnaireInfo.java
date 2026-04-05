package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 查询问卷信息
 **/
public class GC2GS_007_004_ReqQuestionnaireInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 问卷id */
private long id;


public GC2GS_007_004_ReqQuestionnaireInfo() {
	id = (long)0;
}

public GC2GS_007_004_ReqQuestionnaireInfo(
	 long _id
) {	id = _id;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)4; }

/** 问卷id */
public long getId() { return id; }
/** 问卷id */
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
	_buf.put((byte)7);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)4);
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

