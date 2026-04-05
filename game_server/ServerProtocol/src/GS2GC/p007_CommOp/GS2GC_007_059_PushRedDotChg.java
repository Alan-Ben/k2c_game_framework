package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_059_PushRedDotChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 红点信息 */
private Common.Common_RedDotInfo redDotInfo;


public GS2GC_007_059_PushRedDotChg() {
	redDotInfo = new Common.Common_RedDotInfo();
}

public GS2GC_007_059_PushRedDotChg(
	 Common.Common_RedDotInfo _redDotInfo
) {	redDotInfo = _redDotInfo;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)59; }

/** 红点信息 */
public Common.Common_RedDotInfo getRedDotInfo() { return redDotInfo; }
/** 红点信息 */
public void setRedDotInfo(Common.Common_RedDotInfo _redDotInfo) { redDotInfo = _redDotInfo; }


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
	int _redDotInfoCustLen = _buf.getInt();
	int _redDotInfoCurPos = _buf.position();
	redDotInfo.ReadUnzipBuf(_buf, _redDotInfoCurPos + _redDotInfoCustLen);
	_buf.position(_redDotInfoCurPos + _redDotInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(redDotInfo.GetBufSize());
	redDotInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)59);
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

