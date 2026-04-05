package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-随机邀约
 **/
public class GS2GC_015_004_RetCallRand implements ALBasicProtocolPack._IALProtocolStructure {
/** 邀约结果 */
private Common.ConsortObj.Consort_CallRes res;


public GS2GC_015_004_RetCallRand() {
	res = new Common.ConsortObj.Consort_CallRes();
}

public GS2GC_015_004_RetCallRand(
	 Common.ConsortObj.Consort_CallRes _res
) {	res = _res;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)4; }

/** 邀约结果 */
public Common.ConsortObj.Consort_CallRes getRes() { return res; }
/** 邀约结果 */
public void setRes(Common.ConsortObj.Consort_CallRes _res) { res = _res; }


public final int GetBufSize() {
	int _size = 37;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 39;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _resCustLen = _buf.getInt();
	int _resCurPos = _buf.position();
	res.ReadUnzipBuf(_buf, _resCurPos + _resCustLen);
	_buf.position(_resCurPos + _resCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(res.GetBufSize());
	res.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)4);
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

