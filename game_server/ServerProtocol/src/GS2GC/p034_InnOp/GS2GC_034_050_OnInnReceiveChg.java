package GS2GC.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店接待信息变更
 **/
public class GS2GC_034_050_OnInnReceiveChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 接待信息 */
private Common.InnObj.Inn_ReceiveList receiveList;


public GS2GC_034_050_OnInnReceiveChg() {
	receiveList = new Common.InnObj.Inn_ReceiveList();
}

public GS2GC_034_050_OnInnReceiveChg(
	 Common.InnObj.Inn_ReceiveList _receiveList
) {	receiveList = _receiveList;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)50; }

/** 接待信息 */
public Common.InnObj.Inn_ReceiveList getReceiveList() { return receiveList; }
/** 接待信息 */
public void setReceiveList(Common.InnObj.Inn_ReceiveList _receiveList) { receiveList = _receiveList; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + receiveList.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + receiveList.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _receiveListCustLen = _buf.getInt();
	int _receiveListCurPos = _buf.position();
	receiveList.ReadUnzipBuf(_buf, _receiveListCurPos + _receiveListCustLen);
	_buf.position(_receiveListCurPos + _receiveListCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(receiveList.GetBufSize());
	receiveList.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)50);
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

