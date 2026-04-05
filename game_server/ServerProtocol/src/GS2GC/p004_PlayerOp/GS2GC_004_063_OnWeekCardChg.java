package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 周卡信息变更推送
 **/
public class GS2GC_004_063_OnWeekCardChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.WeekCardObj.WeekCard_Info info;


public GS2GC_004_063_OnWeekCardChg() {
	info = new Common.WeekCardObj.WeekCard_Info();
}

public GS2GC_004_063_OnWeekCardChg(
	 Common.WeekCardObj.WeekCard_Info _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)63; }

public Common.WeekCardObj.WeekCard_Info getInfo() { return info; }
public void setInfo(Common.WeekCardObj.WeekCard_Info _info) { info = _info; }


public final int GetBufSize() {
	int _size = 21;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;

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
	_buf.put((byte)4);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)63);
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

