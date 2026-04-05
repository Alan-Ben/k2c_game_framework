package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 联姻请求-对指定群体发起请求
 **/
public class GS2GC_014_015_RetAgreeApplyGroup implements ALBasicProtocolPack._IALProtocolStructure {


public GS2GC_014_015_RetAgreeApplyGroup() {
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)15; }



public final int GetBufSize() {
	int _size = 0;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
}

public final void PutUnzipBuf(ByteBuffer _buf) {
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)15);
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

