package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GS2GC_021_052_OnFixedCDChged implements ALBasicProtocolPack._IALProtocolStructure {
/** 定时恢复cd信息 */
private NPCommon.NPCommon_PlayerFixedCD cdInfo;


public GS2GC_021_052_OnFixedCDChged() {
	cdInfo = new NPCommon.NPCommon_PlayerFixedCD();
}

public GS2GC_021_052_OnFixedCDChged(
	 NPCommon.NPCommon_PlayerFixedCD _cdInfo
) {	cdInfo = _cdInfo;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)52; }

/** 定时恢复cd信息 */
public NPCommon.NPCommon_PlayerFixedCD getCdInfo() { return cdInfo; }
/** 定时恢复cd信息 */
public void setCdInfo(NPCommon.NPCommon_PlayerFixedCD _cdInfo) { cdInfo = _cdInfo; }


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
	int _cdInfoCustLen = _buf.getInt();
	int _cdInfoCurPos = _buf.position();
	cdInfo.ReadUnzipBuf(_buf, _cdInfoCurPos + _cdInfoCustLen);
	_buf.position(_cdInfoCurPos + _cdInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(cdInfo.GetBufSize());
	cdInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)52);
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

