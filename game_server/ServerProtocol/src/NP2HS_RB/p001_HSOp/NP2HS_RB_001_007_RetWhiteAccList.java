package NP2HS_RB.p001_HSOp;

import java.nio.ByteBuffer;
/*********
 * 返回白名单列表
 **/
public class NP2HS_RB_001_007_RetWhiteAccList implements ALBasicProtocolPack._IALProtocolStructure {
/** 白名单列表 */
private Common.ServerObj.ServerObj_WhiteAccList whiteAccList;


public NP2HS_RB_001_007_RetWhiteAccList() {
	whiteAccList = new Common.ServerObj.ServerObj_WhiteAccList();
}

public NP2HS_RB_001_007_RetWhiteAccList(
	 Common.ServerObj.ServerObj_WhiteAccList _whiteAccList
) {	whiteAccList = _whiteAccList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)7; }

/** 白名单列表 */
public Common.ServerObj.ServerObj_WhiteAccList getWhiteAccList() { return whiteAccList; }
/** 白名单列表 */
public void setWhiteAccList(Common.ServerObj.ServerObj_WhiteAccList _whiteAccList) { whiteAccList = _whiteAccList; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + whiteAccList.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + whiteAccList.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _whiteAccListCustLen = _buf.getInt();
	int _whiteAccListCurPos = _buf.position();
	whiteAccList.ReadUnzipBuf(_buf, _whiteAccListCurPos + _whiteAccListCustLen);
	_buf.position(_whiteAccListCurPos + _whiteAccListCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(whiteAccList.GetBufSize());
	whiteAccList.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)7);
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

