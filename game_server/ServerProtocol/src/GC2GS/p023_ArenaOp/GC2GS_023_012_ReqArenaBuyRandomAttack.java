package GC2GS.p023_ArenaOp;

import java.nio.ByteBuffer;
/*********
 * 竞技场购买攻击次数
 **/
public class GC2GS_023_012_ReqArenaBuyRandomAttack implements ALBasicProtocolPack._IALProtocolStructure {
private int num;


public GC2GS_023_012_ReqArenaBuyRandomAttack() {
	num = 0;
}

public GC2GS_023_012_ReqArenaBuyRandomAttack(
	 int _num
) {	num = _num;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)12; }

public int getNum() { return num; }
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
	_buf.put((byte)23);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)12);
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

