package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_047_RetGuildIconShow implements ALBasicProtocolPack._IALProtocolStructure {
/** 联盟最简展示数据 */
private Common.GuildObj.Guild_IconShow info;


public GS2GC_032_047_RetGuildIconShow() {
	info = new Common.GuildObj.Guild_IconShow();
}

public GS2GC_032_047_RetGuildIconShow(
	 Common.GuildObj.Guild_IconShow _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)47; }

/** 联盟最简展示数据 */
public Common.GuildObj.Guild_IconShow getInfo() { return info; }
/** 联盟最简展示数据 */
public void setInfo(Common.GuildObj.Guild_IconShow _info) { info = _info; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + info.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + info.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)47);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)47);
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

