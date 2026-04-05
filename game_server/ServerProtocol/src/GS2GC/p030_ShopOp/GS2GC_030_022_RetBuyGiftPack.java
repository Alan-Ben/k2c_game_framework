package GS2GC.p030_ShopOp;

import java.nio.ByteBuffer;
public class GS2GC_030_022_RetBuyGiftPack implements ALBasicProtocolPack._IALProtocolStructure {


public GS2GC_030_022_RetBuyGiftPack() {
}

public final byte getMainOrder() { return (byte)30; }

public final byte getSubOrder() { return (byte)22; }



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
	_buf.put((byte)30);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)22);
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

