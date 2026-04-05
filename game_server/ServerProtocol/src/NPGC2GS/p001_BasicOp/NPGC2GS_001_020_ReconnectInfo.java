package NPGC2GS.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGC2GS_001_020_ReconnectInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int curRecMesCount;


public NPGC2GS_001_020_ReconnectInfo() {
	curRecMesCount = 0;
}

public NPGC2GS_001_020_ReconnectInfo(
	 int _curRecMesCount
) {	curRecMesCount = _curRecMesCount;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)20; }

public int getCurRecMesCount() { return curRecMesCount; }
public void setCurRecMesCount(int _curRecMesCount) { curRecMesCount = _curRecMesCount; }


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
	if(_buf.remaining() > 0) curRecMesCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(curRecMesCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)20);
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

