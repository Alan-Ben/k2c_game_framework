package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 请求急速兑换
 **/
public class GC2GS_004_101_ReqRushExchange implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否使用钻石补充不足道具 */
private boolean useGemSupplement;


public GC2GS_004_101_ReqRushExchange() {
	useGemSupplement = false;
}

public GC2GS_004_101_ReqRushExchange(
	 boolean _useGemSupplement
) {	useGemSupplement = _useGemSupplement;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)101; }

/** 是否使用钻石补充不足道具 */
public boolean getUseGemSupplement() { return useGemSupplement; }
/** 是否使用钻石补充不足道具 */
public void setUseGemSupplement(boolean _useGemSupplement) { useGemSupplement = _useGemSupplement; }


public final int GetBufSize() {
	int _size = 1;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) useGemSupplement = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(useGemSupplement?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)101);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)101);
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

