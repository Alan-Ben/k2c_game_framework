package NP2CS_R.np_p002_serverInfoOp;

import java.nio.ByteBuffer;
public class NP2CS_R_002_004_ReqUpdateUSHoldInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** US负载信息 */
private Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo holdInfo;


public NP2CS_R_002_004_ReqUpdateUSHoldInfo() {
	holdInfo = new Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo();
}

public NP2CS_R_002_004_ReqUpdateUSHoldInfo(
	 Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo _holdInfo
) {	holdInfo = _holdInfo;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)4; }

/** US负载信息 */
public Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo getHoldInfo() { return holdInfo; }
/** US负载信息 */
public void setHoldInfo(Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo _holdInfo) { holdInfo = _holdInfo; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _holdInfoCustLen = _buf.getInt();
	int _holdInfoCurPos = _buf.position();
	holdInfo.ReadUnzipBuf(_buf, _holdInfoCurPos + _holdInfoCustLen);
	_buf.position(_holdInfoCurPos + _holdInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(holdInfo.GetBufSize());
	holdInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

