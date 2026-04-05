package GS2GC.p040_MarsPeopleOp;

import java.nio.ByteBuffer;
/*********
 * 火星居民信件变更
 **/
public class GS2GC_040_053_OnLetterChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.MarsObj.Mars_Letter letter;


public GS2GC_040_053_OnLetterChg() {
	letter = new Common.MarsObj.Mars_Letter();
}

public GS2GC_040_053_OnLetterChg(
	 Common.MarsObj.Mars_Letter _letter
) {	letter = _letter;
}

public final byte getMainOrder() { return (byte)40; }

public final byte getSubOrder() { return (byte)53; }

public Common.MarsObj.Mars_Letter getLetter() { return letter; }
public void setLetter(Common.MarsObj.Mars_Letter _letter) { letter = _letter; }


public final int GetBufSize() {
	int _size = 29;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 31;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _letterCustLen = _buf.getInt();
	int _letterCurPos = _buf.position();
	letter.ReadUnzipBuf(_buf, _letterCurPos + _letterCustLen);
	_buf.position(_letterCurPos + _letterCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(letter.GetBufSize());
	letter.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)53);
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

