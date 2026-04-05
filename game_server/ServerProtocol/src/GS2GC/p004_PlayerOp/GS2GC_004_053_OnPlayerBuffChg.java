package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_053_OnPlayerBuffChg implements ALBasicProtocolPack._IALProtocolStructure {
private NPCommon.NPCommon_PlayerBuffInfo buff;


public GS2GC_004_053_OnPlayerBuffChg() {
	buff = new NPCommon.NPCommon_PlayerBuffInfo();
}

public GS2GC_004_053_OnPlayerBuffChg(
	 NPCommon.NPCommon_PlayerBuffInfo _buff
) {	buff = _buff;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)53; }

public NPCommon.NPCommon_PlayerBuffInfo getBuff() { return buff; }
public void setBuff(NPCommon.NPCommon_PlayerBuffInfo _buff) { buff = _buff; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _buffCustLen = _buf.getInt();
	int _buffCurPos = _buf.position();
	buff.ReadUnzipBuf(_buf, _buffCurPos + _buffCustLen);
	_buf.position(_buffCurPos + _buffCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(buff.GetBufSize());
	buff.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
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

