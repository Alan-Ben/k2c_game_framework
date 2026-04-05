package ALLRPC.US.Child;

import java.nio.ByteBuffer;
public class AgreePersonMarryApply_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 结婚子嗣数据 */
private Common.ServerObj.ServerObj_AdultMarriedInfo marryInfo;


public AgreePersonMarryApply_Req() {
	marryInfo = new Common.ServerObj.ServerObj_AdultMarriedInfo();
}

public AgreePersonMarryApply_Req(
	 Common.ServerObj.ServerObj_AdultMarriedInfo _marryInfo
) {	marryInfo = _marryInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 结婚子嗣数据 */
public Common.ServerObj.ServerObj_AdultMarriedInfo getMarryInfo() { return marryInfo; }
/** 结婚子嗣数据 */
public void setMarryInfo(Common.ServerObj.ServerObj_AdultMarriedInfo _marryInfo) { marryInfo = _marryInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + marryInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + marryInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _marryInfoCustLen = _buf.getInt();
	int _marryInfoCurPos = _buf.position();
	marryInfo.ReadUnzipBuf(_buf, _marryInfoCurPos + _marryInfoCustLen);
	_buf.position(_marryInfoCurPos + _marryInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(marryInfo.GetBufSize());
	marryInfo.PutUnzipBuf(_buf);
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

