package NPGS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGS2GC_001_012_RetMostRecommendedUSInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 最推荐服务器信息 */
private NPCommon.NP_SYS_ServerItem serverItem;
/** 错误码 */
private int errCode;


public NPGS2GC_001_012_RetMostRecommendedUSInfo() {
	serverItem = new NPCommon.NP_SYS_ServerItem();
	errCode = 0;
}

public NPGS2GC_001_012_RetMostRecommendedUSInfo(
	 NPCommon.NP_SYS_ServerItem _serverItem
	, int _errCode
) {	serverItem = _serverItem;
	errCode = _errCode;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)12; }

/** 最推荐服务器信息 */
public NPCommon.NP_SYS_ServerItem getServerItem() { return serverItem; }
/** 最推荐服务器信息 */
public void setServerItem(NPCommon.NP_SYS_ServerItem _serverItem) { serverItem = _serverItem; }
/** 错误码 */
public int getErrCode() { return errCode; }
/** 错误码 */
public void setErrCode(int _errCode) { errCode = _errCode; }


public final int GetBufSize() {
	int _size = 4;
	_size += 4 + serverItem.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + serverItem.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _serverItemCustLen = _buf.getInt();
	int _serverItemCurPos = _buf.position();
	serverItem.ReadUnzipBuf(_buf, _serverItemCurPos + _serverItemCustLen);
	_buf.position(_serverItemCurPos + _serverItemCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverItem.GetBufSize());
	serverItem.PutUnzipBuf(_buf);
	_buf.putInt(errCode);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)12);
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

