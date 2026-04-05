package Common.NpChatObj;

import java.nio.ByteBuffer;
/*********
 * 妃子分享
 **/
public class NPCommon_ChatContent_ConsortShare implements ALBasicProtocolPack._IALProtocolStructure {
/** 妃子id */
private long consortId;
/** 皮肤id */
private long skinId;
/** 魅力 */
private long charm;
/** 亲密度 */
private long intimacy;
/** 羁绊 */
private Common.ConsortObj.Consort_Fetters consortFetters;


public NPCommon_ChatContent_ConsortShare() {
	consortId = (long)0;
	skinId = (long)0;
	charm = (long)0;
	intimacy = (long)0;
	consortFetters = new Common.ConsortObj.Consort_Fetters();
}

public NPCommon_ChatContent_ConsortShare(
	 long _consortId
	, long _skinId
	, long _charm
	, long _intimacy
	, Common.ConsortObj.Consort_Fetters _consortFetters
) {	consortId = _consortId;
	skinId = _skinId;
	charm = _charm;
	intimacy = _intimacy;
	consortFetters = _consortFetters;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 妃子id */
public long getConsortId() { return consortId; }
/** 妃子id */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 皮肤id */
public long getSkinId() { return skinId; }
/** 皮肤id */
public void setSkinId(long _skinId) { skinId = _skinId; }
/** 魅力 */
public long getCharm() { return charm; }
/** 魅力 */
public void setCharm(long _charm) { charm = _charm; }
/** 亲密度 */
public long getIntimacy() { return intimacy; }
/** 亲密度 */
public void setIntimacy(long _intimacy) { intimacy = _intimacy; }
/** 羁绊 */
public Common.ConsortObj.Consort_Fetters getConsortFetters() { return consortFetters; }
/** 羁绊 */
public void setConsortFetters(Common.ConsortObj.Consort_Fetters _consortFetters) { consortFetters = _consortFetters; }


public final int GetBufSize() {
	int _size = 40;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) charm = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) intimacy = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _consortFettersCustLen = _buf.getInt();
	int _consortFettersCurPos = _buf.position();
	consortFetters.ReadUnzipBuf(_buf, _consortFettersCurPos + _consortFettersCustLen);
	_buf.position(_consortFettersCurPos + _consortFettersCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(skinId);
	_buf.putLong(charm);
	_buf.putLong(intimacy);
	_buf.putInt(consortFetters.GetBufSize());
	consortFetters.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

