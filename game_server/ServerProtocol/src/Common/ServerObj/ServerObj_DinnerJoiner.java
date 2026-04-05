package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 赴宴玩家数据
 **/
public class ServerObj_DinnerJoiner implements ALBasicProtocolPack._IALProtocolStructure {
/** 赴宴玩家CID */
private long cid;
/** 赴宴玩家消耗配置ID */
private long costId;
/** 赴宴玩家获得宴会币 */
private long gainCoin;
/** 赴宴玩家获得宴人气 */
private long gainScore;
/** 赴宴玩家名称 */
private String cname;


public ServerObj_DinnerJoiner() {
	cid = (long)0;
	costId = (long)0;
	gainCoin = (long)0;
	gainScore = (long)0;
	cname = "";
}

public ServerObj_DinnerJoiner(
	 long _cid
	, long _costId
	, long _gainCoin
	, long _gainScore
	, String _cname
) {	cid = _cid;
	costId = _costId;
	gainCoin = _gainCoin;
	gainScore = _gainScore;
	cname = _cname;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 赴宴玩家CID */
public long getCid() { return cid; }
/** 赴宴玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 赴宴玩家消耗配置ID */
public long getCostId() { return costId; }
/** 赴宴玩家消耗配置ID */
public void setCostId(long _costId) { costId = _costId; }
/** 赴宴玩家获得宴会币 */
public long getGainCoin() { return gainCoin; }
/** 赴宴玩家获得宴会币 */
public void setGainCoin(long _gainCoin) { gainCoin = _gainCoin; }
/** 赴宴玩家获得宴人气 */
public long getGainScore() { return gainScore; }
/** 赴宴玩家获得宴人气 */
public void setGainScore(long _gainScore) { gainScore = _gainScore; }
/** 赴宴玩家名称 */
public String getCname() { return cname; }
/** 赴宴玩家名称 */
public void setCname(String _cname) { cname = _cname; }


public final int GetBufSize() {
	int _size = 32;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) costId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainCoin = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cname = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(costId);
	_buf.putLong(gainCoin);
	_buf.putLong(gainScore);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, cname);
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

