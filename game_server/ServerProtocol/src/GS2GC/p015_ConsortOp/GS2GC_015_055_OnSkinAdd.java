package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人新增皮肤推送
 **/
public class GS2GC_015_055_OnSkinAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private long consortId;
/** 皮肤数据 */
private Common.ConsortObj.Consort_SkinInfo skin;


public GS2GC_015_055_OnSkinAdd() {
	consortId = (long)0;
	skin = new Common.ConsortObj.Consort_SkinInfo();
}

public GS2GC_015_055_OnSkinAdd(
	 long _consortId
	, Common.ConsortObj.Consort_SkinInfo _skin
) {	consortId = _consortId;
	skin = _skin;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)55; }

/** 空 */
public long getConsortId() { return consortId; }
/** 空 */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 皮肤数据 */
public Common.ConsortObj.Consort_SkinInfo getSkin() { return skin; }
/** 皮肤数据 */
public void setSkin(Common.ConsortObj.Consort_SkinInfo _skin) { skin = _skin; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _skinCustLen = _buf.getInt();
	int _skinCurPos = _buf.position();
	skin.ReadUnzipBuf(_buf, _skinCurPos + _skinCustLen);
	_buf.position(_skinCurPos + _skinCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putInt(skin.GetBufSize());
	skin.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)55);
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

