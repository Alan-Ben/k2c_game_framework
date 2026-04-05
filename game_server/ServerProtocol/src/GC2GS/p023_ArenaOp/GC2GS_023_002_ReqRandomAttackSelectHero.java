package GC2GS.p023_ArenaOp;

import java.nio.ByteBuffer;
/*********
 * 随机攻击选择出战大臣
 **/
public class GC2GS_023_002_ReqRandomAttackSelectHero implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
/** 对手机器人名字 */
private String opponentBotName;
private long buffId;


public GC2GS_023_002_ReqRandomAttackSelectHero() {
	heroId = (long)0;
	opponentBotName = "";
	buffId = (long)0;
}

public GC2GS_023_002_ReqRandomAttackSelectHero(
	 long _heroId
	, String _opponentBotName
	, long _buffId
) {	heroId = _heroId;
	opponentBotName = _opponentBotName;
	buffId = _buffId;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)2; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 对手机器人名字 */
public String getOpponentBotName() { return opponentBotName; }
/** 对手机器人名字 */
public void setOpponentBotName(String _opponentBotName) { opponentBotName = _opponentBotName; }
public long getBuffId() { return buffId; }
public void setBuffId(long _buffId) { buffId = _buffId; }


public final int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(opponentBotName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(opponentBotName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) opponentBotName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buffId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, opponentBotName);
	_buf.putLong(buffId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)2);
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

