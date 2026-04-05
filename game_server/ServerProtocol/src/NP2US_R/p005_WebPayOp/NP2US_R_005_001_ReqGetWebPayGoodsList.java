package NP2US_R.p005_WebPayOp;

import java.nio.ByteBuffer;
/*********
 * 获取网页支付商品列表
 **/
public class NP2US_R_005_001_ReqGetWebPayGoodsList implements ALBasicProtocolPack._IALProtocolStructure {
/** 角色ID */
private long cid;
/** 渠道标识 */
private String channelCode;


public NP2US_R_005_001_ReqGetWebPayGoodsList() {
	cid = (long)0;
	channelCode = "";
}

public NP2US_R_005_001_ReqGetWebPayGoodsList(
	 long _cid
	, String _channelCode
) {	cid = _cid;
	channelCode = _channelCode;
}

public final byte getMainOrder() { return (byte)5; }

public final byte getSubOrder() { return (byte)1; }

/** 角色ID */
public long getCid() { return cid; }
/** 角色ID */
public void setCid(long _cid) { cid = _cid; }
/** 渠道标识 */
public String getChannelCode() { return channelCode; }
/** 渠道标识 */
public void setChannelCode(String _channelCode) { channelCode = _channelCode; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelCode);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelCode);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) channelCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, channelCode);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)5);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)5);
	_recBuf.put((byte)1);
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

