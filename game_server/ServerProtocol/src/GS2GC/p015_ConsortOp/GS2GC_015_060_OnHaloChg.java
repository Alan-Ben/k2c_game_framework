package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人星辉数据变更
 **/
public class GS2GC_015_060_OnHaloChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private long consortId;
/** 家人星辉数据 */
private Common.ConsortObj.Consort_Halo halo;


public GS2GC_015_060_OnHaloChg() {
	consortId = (long)0;
	halo = new Common.ConsortObj.Consort_Halo();
}

public GS2GC_015_060_OnHaloChg(
	 long _consortId
	, Common.ConsortObj.Consort_Halo _halo
) {	consortId = _consortId;
	halo = _halo;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)60; }

/** 空 */
public long getConsortId() { return consortId; }
/** 空 */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 家人星辉数据 */
public Common.ConsortObj.Consort_Halo getHalo() { return halo; }
/** 家人星辉数据 */
public void setHalo(Common.ConsortObj.Consort_Halo _halo) { halo = _halo; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _haloCustLen = _buf.getInt();
	int _haloCurPos = _buf.position();
	halo.ReadUnzipBuf(_buf, _haloCurPos + _haloCustLen);
	_buf.position(_haloCurPos + _haloCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putInt(halo.GetBufSize());
	halo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
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

