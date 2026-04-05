package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 请求周卡的结算信息
 **/
public class GC2GS_004_015_ReqWeekCardSettleInfo implements ALBasicProtocolPack._IALProtocolStructure {


public GC2GS_004_015_ReqWeekCardSettleInfo() {
}

public final byte getMainOrder() { return (byte)4; }

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
	_buf.put((byte)4);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
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

