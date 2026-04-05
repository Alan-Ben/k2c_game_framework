package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 子嗣（成年未婚）新增推送
 **/
public class GS2GC_014_054_OnUnmarriedAdultAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private Common.ChildObj.Adult_UnmarriedInfo adult;


public GS2GC_014_054_OnUnmarriedAdultAdd() {
	adult = new Common.ChildObj.Adult_UnmarriedInfo();
}

public GS2GC_014_054_OnUnmarriedAdultAdd(
	 Common.ChildObj.Adult_UnmarriedInfo _adult
) {	adult = _adult;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)54; }

/** 空 */
public Common.ChildObj.Adult_UnmarriedInfo getAdult() { return adult; }
/** 空 */
public void setAdult(Common.ChildObj.Adult_UnmarriedInfo _adult) { adult = _adult; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + adult.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + adult.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _adultCustLen = _buf.getInt();
	int _adultCurPos = _buf.position();
	adult.ReadUnzipBuf(_buf, _adultCurPos + _adultCustLen);
	_buf.position(_adultCurPos + _adultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(adult.GetBufSize());
	adult.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)54);
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

