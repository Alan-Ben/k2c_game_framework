package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_057_RetInnInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 旅店信息 */
private Common.InnObj.Inn_Info innInfo;


public GS2GC_002_057_RetInnInit() {
	innInfo = new Common.InnObj.Inn_Info();
}

public GS2GC_002_057_RetInnInit(
	 Common.InnObj.Inn_Info _innInfo
) {	innInfo = _innInfo;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)57; }

/** 旅店信息 */
public Common.InnObj.Inn_Info getInnInfo() { return innInfo; }
/** 旅店信息 */
public void setInnInfo(Common.InnObj.Inn_Info _innInfo) { innInfo = _innInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + innInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + innInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _innInfoCustLen = _buf.getInt();
	int _innInfoCurPos = _buf.position();
	innInfo.ReadUnzipBuf(_buf, _innInfoCurPos + _innInfoCustLen);
	_buf.position(_innInfoCurPos + _innInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(innInfo.GetBufSize());
	innInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)57);
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

