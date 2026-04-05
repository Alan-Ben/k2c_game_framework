package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 情人收集-选择目标情人
 **/
public class GC2GS_004_104_ReqSetLoverTarget implements ALBasicProtocolPack._IALProtocolStructure {
/** 选择的情人配置ID */
private long loverId;


public GC2GS_004_104_ReqSetLoverTarget() {
	loverId = (long)0;
}

public GC2GS_004_104_ReqSetLoverTarget(
	 long _loverId
) {	loverId = _loverId;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)104; }

/** 选择的情人配置ID */
public long getLoverId() { return loverId; }
/** 选择的情人配置ID */
public void setLoverId(long _loverId) { loverId = _loverId; }


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
	if(_buf.remaining() > 0) loverId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(loverId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)104);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)104);
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

