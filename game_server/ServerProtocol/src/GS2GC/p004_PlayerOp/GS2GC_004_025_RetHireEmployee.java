package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_025_RetHireEmployee implements ALBasicProtocolPack._IALProtocolStructure {
/** 雇佣数量 */
private int num;


public GS2GC_004_025_RetHireEmployee() {
	num = 0;
}

public GS2GC_004_025_RetHireEmployee(
	 int _num
) {	num = _num;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)24; }

/** 雇佣数量 */
public int getNum() { return num; }
/** 雇佣数量 */
public void setNum(int _num) { num = _num; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(num);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)24);
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

