package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人新增推送
 **/
public class GS2GC_015_050_OnConsortAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private Common.ConsortObj.Consort_Info consort;
/** 来源类型 */
private Common.ConsortEnum.EConsortSourceType sourceType;


public GS2GC_015_050_OnConsortAdd() {
	consort = new Common.ConsortObj.Consort_Info();
	sourceType = Common.ConsortEnum.EConsortSourceType.values()[0];
}

public GS2GC_015_050_OnConsortAdd(
	 Common.ConsortObj.Consort_Info _consort
	, Common.ConsortEnum.EConsortSourceType _sourceType
) {	consort = _consort;
	sourceType = _sourceType;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)50; }

/** 空 */
public Common.ConsortObj.Consort_Info getConsort() { return consort; }
/** 空 */
public void setConsort(Common.ConsortObj.Consort_Info _consort) { consort = _consort; }
/** 来源类型 */
public Common.ConsortEnum.EConsortSourceType getSourceType() { return sourceType; }
/** 来源类型 */
public void setSourceType(Common.ConsortEnum.EConsortSourceType _sourceType) { sourceType = _sourceType; }


public final int GetBufSize() {
	int _size = 4;
	_size += 4 + consort.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + consort.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _consortCustLen = _buf.getInt();
	int _consortCurPos = _buf.position();
	consort.ReadUnzipBuf(_buf, _consortCurPos + _consortCustLen);
	_buf.position(_consortCurPos + _consortCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sourceType = Common.ConsortEnum.EConsortSourceType.EConsortSourceType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(consort.GetBufSize());
	consort.PutUnzipBuf(_buf);
	_buf.putInt(sourceType.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)50);
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

