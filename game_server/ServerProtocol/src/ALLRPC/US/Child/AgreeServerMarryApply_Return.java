package ALLRPC.US.Child;

import java.nio.ByteBuffer;
public class AgreeServerMarryApply_Return implements ALBasicProtocolPack._IALProtocolStructure {
/** 被结婚子嗣数据 */
private Common.ServerObj.ServerObj_AdultMarriedInfo beMarriedInfo;


public AgreeServerMarryApply_Return() {
	beMarriedInfo = new Common.ServerObj.ServerObj_AdultMarriedInfo();
}

public AgreeServerMarryApply_Return(
	 Common.ServerObj.ServerObj_AdultMarriedInfo _beMarriedInfo
) {	beMarriedInfo = _beMarriedInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 被结婚子嗣数据 */
public Common.ServerObj.ServerObj_AdultMarriedInfo getBeMarriedInfo() { return beMarriedInfo; }
/** 被结婚子嗣数据 */
public void setBeMarriedInfo(Common.ServerObj.ServerObj_AdultMarriedInfo _beMarriedInfo) { beMarriedInfo = _beMarriedInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + beMarriedInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + beMarriedInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _beMarriedInfoCustLen = _buf.getInt();
	int _beMarriedInfoCurPos = _buf.position();
	beMarriedInfo.ReadUnzipBuf(_buf, _beMarriedInfoCurPos + _beMarriedInfoCustLen);
	_buf.position(_beMarriedInfoCurPos + _beMarriedInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(beMarriedInfo.GetBufSize());
	beMarriedInfo.PutUnzipBuf(_buf);
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

