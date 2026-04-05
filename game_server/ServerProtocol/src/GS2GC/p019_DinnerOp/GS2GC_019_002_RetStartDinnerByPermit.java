package GS2GC.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 开启宴会（许可证模式）
 **/
public class GS2GC_019_002_RetStartDinnerByPermit implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会详情 */
private Common.DinnerObj.Dinner_Info info;


public GS2GC_019_002_RetStartDinnerByPermit() {
	info = new Common.DinnerObj.Dinner_Info();
}

public GS2GC_019_002_RetStartDinnerByPermit(
	 Common.DinnerObj.Dinner_Info _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)2; }

/** 宴会详情 */
public Common.DinnerObj.Dinner_Info getInfo() { return info; }
/** 宴会详情 */
public void setInfo(Common.DinnerObj.Dinner_Info _info) { info = _info; }


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
	_buf.put((byte)19);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)2);
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

