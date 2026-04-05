package NP2US_RB.p010_MarsMineOp;

import java.nio.ByteBuffer;
public class NP2US_RB_010_001_RetGetMarsMine implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿数据 */
private Common.ServerObj.ServerObj_MarsMine marsMine;


public NP2US_RB_010_001_RetGetMarsMine() {
	marsMine = new Common.ServerObj.ServerObj_MarsMine();
}

public NP2US_RB_010_001_RetGetMarsMine(
	 Common.ServerObj.ServerObj_MarsMine _marsMine
) {	marsMine = _marsMine;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)1; }

/** 矿数据 */
public Common.ServerObj.ServerObj_MarsMine getMarsMine() { return marsMine; }
/** 矿数据 */
public void setMarsMine(Common.ServerObj.ServerObj_MarsMine _marsMine) { marsMine = _marsMine; }


public final int GetBufSize() {
	int _size = 100;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 102;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _marsMineCustLen = _buf.getInt();
	int _marsMineCurPos = _buf.position();
	marsMine.ReadUnzipBuf(_buf, _marsMineCurPos + _marsMineCustLen);
	_buf.position(_marsMineCurPos + _marsMineCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(marsMine.GetBufSize());
	marsMine.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
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

