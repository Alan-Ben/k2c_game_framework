package GS2GC.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 参加宴会玩家推送
 **/
public class GS2GC_019_053_OnJoinerAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 赴宴玩家CID */
private long cid;
/** 赴宴配置ID */
private long costId;
/** 宴会配置ID */
private long dinnerId;
/** 赴宴玩家名称 */
private String cname;


public GS2GC_019_053_OnJoinerAdd() {
	cid = (long)0;
	costId = (long)0;
	dinnerId = (long)0;
	cname = "";
}

public GS2GC_019_053_OnJoinerAdd(
	 long _cid
	, long _costId
	, long _dinnerId
	, String _cname
) {	cid = _cid;
	costId = _costId;
	dinnerId = _dinnerId;
	cname = _cname;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)53; }

/** 赴宴玩家CID */
public long getCid() { return cid; }
/** 赴宴玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 赴宴配置ID */
public long getCostId() { return costId; }
/** 赴宴配置ID */
public void setCostId(long _costId) { costId = _costId; }
/** 宴会配置ID */
public long getDinnerId() { return dinnerId; }
/** 宴会配置ID */
public void setDinnerId(long _dinnerId) { dinnerId = _dinnerId; }
/** 赴宴玩家名称 */
public String getCname() { return cname; }
/** 赴宴玩家名称 */
public void setCname(String _cname) { cname = _cname; }


public final int GetBufSize() {
	int _size = 24;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) costId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dinnerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cname = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(costId);
	_buf.putLong(dinnerId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, cname);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)53);
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

