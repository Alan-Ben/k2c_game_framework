package ALLRPC.Common;

import java.nio.ByteBuffer;
public class PushPHPParamList_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long serial;
/** 平台参数列表 */
private Common.ServerObj.ServerObj_PHPParamList pListObj;


public PushPHPParamList_Req() {
	serial = (long)0;
	pListObj = new Common.ServerObj.ServerObj_PHPParamList();
}

public PushPHPParamList_Req(
	 long _serial
	, Common.ServerObj.ServerObj_PHPParamList _pListObj
) {	serial = _serial;
	pListObj = _pListObj;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getSerial() { return serial; }
public void setSerial(long _serial) { serial = _serial; }
/** 平台参数列表 */
public Common.ServerObj.ServerObj_PHPParamList getPListObj() { return pListObj; }
/** 平台参数列表 */
public void setPListObj(Common.ServerObj.ServerObj_PHPParamList _pListObj) { pListObj = _pListObj; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + pListObj.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + pListObj.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _pListObjCustLen = _buf.getInt();
	int _pListObjCurPos = _buf.position();
	pListObj.ReadUnzipBuf(_buf, _pListObjCurPos + _pListObjCustLen);
	_buf.position(_pListObjCurPos + _pListObjCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(serial);
	_buf.putInt(pListObj.GetBufSize());
	pListObj.PutUnzipBuf(_buf);
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

