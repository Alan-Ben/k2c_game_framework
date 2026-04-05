package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GC2GS_021_046_ReqClientNotifyFuncUnlock implements ALBasicProtocolPack._IALProtocolStructure {
/** 功能类型 */
private NPEnum.ENPFunctionType funcType;


public GC2GS_021_046_ReqClientNotifyFuncUnlock() {
	funcType = NPEnum.ENPFunctionType.values()[0];
}

public GC2GS_021_046_ReqClientNotifyFuncUnlock(
	 NPEnum.ENPFunctionType _funcType
) {	funcType = _funcType;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)46; }

/** 功能类型 */
public NPEnum.ENPFunctionType getFuncType() { return funcType; }
/** 功能类型 */
public void setFuncType(NPEnum.ENPFunctionType _funcType) { funcType = _funcType; }


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
	if(_buf.remaining() > 0) funcType = NPEnum.ENPFunctionType.ENPFunctionType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(funcType.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)46);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)46);
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

