package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人CG数据变更
 **/
public class GS2GC_015_061_OnCgChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 家人CG数据 */
private Common.ConsortObj.Consort_CGInfo cg;


public GS2GC_015_061_OnCgChg() {
	cg = new Common.ConsortObj.Consort_CGInfo();
}

public GS2GC_015_061_OnCgChg(
	 Common.ConsortObj.Consort_CGInfo _cg
) {	cg = _cg;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)61; }

/** 家人CG数据 */
public Common.ConsortObj.Consort_CGInfo getCg() { return cg; }
/** 家人CG数据 */
public void setCg(Common.ConsortObj.Consort_CGInfo _cg) { cg = _cg; }


public final int GetBufSize() {
	int _size = 13;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _cgCustLen = _buf.getInt();
	int _cgCurPos = _buf.position();
	cg.ReadUnzipBuf(_buf, _cgCurPos + _cgCustLen);
	_buf.position(_cgCurPos + _cgCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(cg.GetBufSize());
	cg.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)61);
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

