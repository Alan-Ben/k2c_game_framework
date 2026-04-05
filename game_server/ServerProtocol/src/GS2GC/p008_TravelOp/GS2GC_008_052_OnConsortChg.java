package GS2GC.p008_TravelOp;

import java.nio.ByteBuffer;
/*********
 * 妃子数据变化推送
 **/
public class GS2GC_008_052_OnConsortChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 妃子数据 */
private Common.TravelObj.Travel_Consort consort;


public GS2GC_008_052_OnConsortChg() {
	consort = new Common.TravelObj.Travel_Consort();
}

public GS2GC_008_052_OnConsortChg(
	 Common.TravelObj.Travel_Consort _consort
) {	consort = _consort;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)52; }

/** 妃子数据 */
public Common.TravelObj.Travel_Consort getConsort() { return consort; }
/** 妃子数据 */
public void setConsort(Common.TravelObj.Travel_Consort _consort) { consort = _consort; }


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
	if(_buf.remaining() <= 0) return;
	int _consortCustLen = _buf.getInt();
	int _consortCurPos = _buf.position();
	consort.ReadUnzipBuf(_buf, _consortCurPos + _consortCustLen);
	_buf.position(_consortCurPos + _consortCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(consort.GetBufSize());
	consort.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)52);
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

