package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GC2GS_021_045_ReqViewPlayerLazyCD implements ALBasicProtocolPack._IALProtocolStructure {
/** CD类型ID */
private int cdId;


public GC2GS_021_045_ReqViewPlayerLazyCD() {
	cdId = 0;
}

public GC2GS_021_045_ReqViewPlayerLazyCD(
	 int _cdId
) {	cdId = _cdId;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)45; }

/** CD类型ID */
public int getCdId() { return cdId; }
/** CD类型ID */
public void setCdId(int _cdId) { cdId = _cdId; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cdId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(cdId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)45);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)45);
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

