package NPGS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGS2GC_001_020_RetReconnectInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int receivedMesCount;
private int curServerResVersion;


public NPGS2GC_001_020_RetReconnectInfo() {
	receivedMesCount = 0;
	curServerResVersion = 0;
}

public NPGS2GC_001_020_RetReconnectInfo(
	 int _receivedMesCount
	, int _curServerResVersion
) {	receivedMesCount = _receivedMesCount;
	curServerResVersion = _curServerResVersion;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)20; }

public int getReceivedMesCount() { return receivedMesCount; }
public void setReceivedMesCount(int _receivedMesCount) { receivedMesCount = _receivedMesCount; }
public int getCurServerResVersion() { return curServerResVersion; }
public void setCurServerResVersion(int _curServerResVersion) { curServerResVersion = _curServerResVersion; }


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
	if(_buf.remaining() > 0) receivedMesCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curServerResVersion = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(receivedMesCount);
	_buf.putInt(curServerResVersion);
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

