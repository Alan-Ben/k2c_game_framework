package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探索-矿产数据变化
 **/
public class GS2GC_041_013_RetNoticeMarsMine implements ALBasicProtocolPack._IALProtocolStructure {
/** 索引数据 */
private Common.MarsObj.Mars_MineDynamic info;


public GS2GC_041_013_RetNoticeMarsMine() {
	info = new Common.MarsObj.Mars_MineDynamic();
}

public GS2GC_041_013_RetNoticeMarsMine(
	 Common.MarsObj.Mars_MineDynamic _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)13; }

/** 索引数据 */
public Common.MarsObj.Mars_MineDynamic getInfo() { return info; }
/** 索引数据 */
public void setInfo(Common.MarsObj.Mars_MineDynamic _info) { info = _info; }


public final int GetBufSize() {
	int _size = 84;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 86;

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
	_buf.put((byte)41);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)13);
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

