package ALLRPC.US.Child;

import java.nio.ByteBuffer;
public class RefusePersonMarryApply_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 被拒绝的请求数据 */
private Common.ServerObj.ServerObj_AdultMarryRefuseApply refusedApply;


public RefusePersonMarryApply_Req() {
	refusedApply = new Common.ServerObj.ServerObj_AdultMarryRefuseApply();
}

public RefusePersonMarryApply_Req(
	 Common.ServerObj.ServerObj_AdultMarryRefuseApply _refusedApply
) {	refusedApply = _refusedApply;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 被拒绝的请求数据 */
public Common.ServerObj.ServerObj_AdultMarryRefuseApply getRefusedApply() { return refusedApply; }
/** 被拒绝的请求数据 */
public void setRefusedApply(Common.ServerObj.ServerObj_AdultMarryRefuseApply _refusedApply) { refusedApply = _refusedApply; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _refusedApplyCustLen = _buf.getInt();
	int _refusedApplyCurPos = _buf.position();
	refusedApply.ReadUnzipBuf(_buf, _refusedApplyCurPos + _refusedApplyCustLen);
	_buf.position(_refusedApplyCurPos + _refusedApplyCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(refusedApply.GetBufSize());
	refusedApply.PutUnzipBuf(_buf);
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

