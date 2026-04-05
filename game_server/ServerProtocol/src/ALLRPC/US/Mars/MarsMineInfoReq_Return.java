package ALLRPC.US.Mars;

import java.nio.ByteBuffer;
public class MarsMineInfoReq_Return implements ALBasicProtocolPack._IALProtocolStructure {
/** 具体矿的信息 */
private Common.ServerObj.ServerObj_MarsMine info;


public MarsMineInfoReq_Return() {
	info = new Common.ServerObj.ServerObj_MarsMine();
}

public MarsMineInfoReq_Return(
	 Common.ServerObj.ServerObj_MarsMine _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 具体矿的信息 */
public Common.ServerObj.ServerObj_MarsMine getInfo() { return info; }
/** 具体矿的信息 */
public void setInfo(Common.ServerObj.ServerObj_MarsMine _info) { info = _info; }


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
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

