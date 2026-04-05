package GC2GS.p011_ClientDataOp;

import java.nio.ByteBuffer;
/*********
 * 客户端数据
 **/
public class GC2GS_011_001_ReqQueryClientData implements ALBasicProtocolPack._IALProtocolStructure {
private int index;


public GC2GS_011_001_ReqQueryClientData() {
	index = 0;
}

public GC2GS_011_001_ReqQueryClientData(
	 int _index
) {	index = _index;
}

public final byte getMainOrder() { return (byte)11; }

public final byte getSubOrder() { return (byte)1; }

public int getIndex() { return index; }
public void setIndex(int _index) { index = _index; }


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
	if(_buf.remaining() > 0) index = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(index);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)11);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)11);
	_recBuf.put((byte)1);
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

