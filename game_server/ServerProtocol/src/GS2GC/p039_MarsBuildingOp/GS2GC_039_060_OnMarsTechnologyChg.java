package GS2GC.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
/*********
 * 火星科技数据变化推送
 **/
public class GS2GC_039_060_OnMarsTechnologyChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.MarsObj.Mars_Technology info;


public GS2GC_039_060_OnMarsTechnologyChg() {
	info = new Common.MarsObj.Mars_Technology();
}

public GS2GC_039_060_OnMarsTechnologyChg(
	 Common.MarsObj.Mars_Technology _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)60; }

public Common.MarsObj.Mars_Technology getInfo() { return info; }
public void setInfo(Common.MarsObj.Mars_Technology _info) { info = _info; }


public final int GetBufSize() {
	int _size = 49;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 51;

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
	_buf.put((byte)39);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)60);
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

