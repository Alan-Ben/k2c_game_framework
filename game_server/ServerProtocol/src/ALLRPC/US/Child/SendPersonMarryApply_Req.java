package ALLRPC.US.Child;

import java.nio.ByteBuffer;
public class SendPersonMarryApply_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣联姻请求数据 */
private Common.ServerObj.ServerObj_AdultMarryApplyInfo apply;


public SendPersonMarryApply_Req() {
	apply = new Common.ServerObj.ServerObj_AdultMarryApplyInfo();
}

public SendPersonMarryApply_Req(
	 Common.ServerObj.ServerObj_AdultMarryApplyInfo _apply
) {	apply = _apply;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 子嗣联姻请求数据 */
public Common.ServerObj.ServerObj_AdultMarryApplyInfo getApply() { return apply; }
/** 子嗣联姻请求数据 */
public void setApply(Common.ServerObj.ServerObj_AdultMarryApplyInfo _apply) { apply = _apply; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + apply.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + apply.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _applyCustLen = _buf.getInt();
	int _applyCurPos = _buf.position();
	apply.ReadUnzipBuf(_buf, _applyCurPos + _applyCustLen);
	_buf.position(_applyCurPos + _applyCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(apply.GetBufSize());
	apply.PutUnzipBuf(_buf);
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

