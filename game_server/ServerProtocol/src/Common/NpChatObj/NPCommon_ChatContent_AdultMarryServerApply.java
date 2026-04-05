package Common.NpChatObj;

import java.nio.ByteBuffer;
/*********
 * 子嗣全服联姻
 **/
public class NPCommon_ChatContent_AdultMarryServerApply implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣数据 */
private Common.ChildObj.Adult_Info adult;


public NPCommon_ChatContent_AdultMarryServerApply() {
	adult = new Common.ChildObj.Adult_Info();
}

public NPCommon_ChatContent_AdultMarryServerApply(
	 Common.ChildObj.Adult_Info _adult
) {	adult = _adult;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 子嗣数据 */
public Common.ChildObj.Adult_Info getAdult() { return adult; }
/** 子嗣数据 */
public void setAdult(Common.ChildObj.Adult_Info _adult) { adult = _adult; }


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

