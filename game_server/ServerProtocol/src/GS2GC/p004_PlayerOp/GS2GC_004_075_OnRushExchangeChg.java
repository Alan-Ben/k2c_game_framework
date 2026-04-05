package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 急速兑换信息变更
 **/
public class GS2GC_004_075_OnRushExchangeChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 急速兑换信息 */
private Common.RushExchangeObj.RushExchange_Info info;


public GS2GC_004_075_OnRushExchangeChg() {
	info = new Common.RushExchangeObj.RushExchange_Info();
}

public GS2GC_004_075_OnRushExchangeChg(
	 Common.RushExchangeObj.RushExchange_Info _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)75; }

/** 急速兑换信息 */
public Common.RushExchangeObj.RushExchange_Info getInfo() { return info; }
/** 急速兑换信息 */
public void setInfo(Common.RushExchangeObj.RushExchange_Info _info) { info = _info; }


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
	_buf.put((byte)4);
	_buf.put((byte)75);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)75);
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

