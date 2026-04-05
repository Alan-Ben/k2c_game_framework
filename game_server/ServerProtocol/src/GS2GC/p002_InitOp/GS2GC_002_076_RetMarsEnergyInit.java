package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 火星能量数据初始化
 **/
public class GS2GC_002_076_RetMarsEnergyInit implements ALBasicProtocolPack._IALProtocolStructure {
private Common.PlayerObj.Player_MarsEnergyInfo info;


public GS2GC_002_076_RetMarsEnergyInit() {
	info = new Common.PlayerObj.Player_MarsEnergyInfo();
}

public GS2GC_002_076_RetMarsEnergyInit(
	 Common.PlayerObj.Player_MarsEnergyInfo _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)76; }

public Common.PlayerObj.Player_MarsEnergyInfo getInfo() { return info; }
public void setInfo(Common.PlayerObj.Player_MarsEnergyInfo _info) { info = _info; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

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
	_buf.put((byte)2);
	_buf.put((byte)76);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)76);
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

