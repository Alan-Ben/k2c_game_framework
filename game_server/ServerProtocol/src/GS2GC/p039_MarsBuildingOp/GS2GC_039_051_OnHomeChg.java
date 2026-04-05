package GS2GC.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
/*********
 * 建筑主基地数据变化
 **/
public class GS2GC_039_051_OnHomeChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.MarsObj.Mars_HomeBuilding info;


public GS2GC_039_051_OnHomeChg() {
	info = new Common.MarsObj.Mars_HomeBuilding();
}

public GS2GC_039_051_OnHomeChg(
	 Common.MarsObj.Mars_HomeBuilding _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)51; }

public Common.MarsObj.Mars_HomeBuilding getInfo() { return info; }
public void setInfo(Common.MarsObj.Mars_HomeBuilding _info) { info = _info; }


public final int GetBufSize() {
	int _size = 18;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 20;

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
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)51);
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

