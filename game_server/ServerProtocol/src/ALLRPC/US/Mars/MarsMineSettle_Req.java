package ALLRPC.US.Mars;

import java.nio.ByteBuffer;
public class MarsMineSettle_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 结算数据 */
private Common.ServerObj.ServerObj_MarsMineSettle info;


public MarsMineSettle_Req() {
	info = new Common.ServerObj.ServerObj_MarsMineSettle();
}

public MarsMineSettle_Req(
	 Common.ServerObj.ServerObj_MarsMineSettle _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 结算数据 */
public Common.ServerObj.ServerObj_MarsMineSettle getInfo() { return info; }
/** 结算数据 */
public void setInfo(Common.ServerObj.ServerObj_MarsMineSettle _info) { info = _info; }


public final int GetBufSize() {
	int _size = 60;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 62;

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

