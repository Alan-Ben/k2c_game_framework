package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人羁绊数据变更
 **/
public class GS2GC_015_057_OnFettersChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private long consortId;
/** 家人羁绊数据 */
private Common.ConsortObj.Consort_Fetters fetters;


public GS2GC_015_057_OnFettersChg() {
	consortId = (long)0;
	fetters = new Common.ConsortObj.Consort_Fetters();
}

public GS2GC_015_057_OnFettersChg(
	 long _consortId
	, Common.ConsortObj.Consort_Fetters _fetters
) {	consortId = _consortId;
	fetters = _fetters;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)57; }

/** 空 */
public long getConsortId() { return consortId; }
/** 空 */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 家人羁绊数据 */
public Common.ConsortObj.Consort_Fetters getFetters() { return fetters; }
/** 家人羁绊数据 */
public void setFetters(Common.ConsortObj.Consort_Fetters _fetters) { fetters = _fetters; }


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
	int _fettersCustLen = _buf.getInt();
	int _fettersCurPos = _buf.position();
	fetters.ReadUnzipBuf(_buf, _fettersCurPos + _fettersCustLen);
	_buf.position(_fettersCurPos + _fettersCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putInt(fetters.GetBufSize());
	fetters.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)57);
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

