package Hotfix.V02.GC2GS.p202_NumMergeOp;

import java.nio.ByteBuffer;
/*********
 * 数字合并-游戏结束/重新开始
 **/
public class GC2GS_202_004_ReqNumMergeGameOver implements ALBasicProtocolPack._IALProtocolStructure {


public GC2GS_202_004_ReqNumMergeGameOver() {
}

public final byte getMainOrder() { return (byte)202; }

public final byte getSubOrder() { return (byte)4; }



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
	_buf.put((byte)202);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)202);
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

