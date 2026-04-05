package Common.NpChatObj;

import java.nio.ByteBuffer;
/*********
 * 联盟成员火星矿被攻击分享
 **/
public class ChatContent_GuildMarsMineAttackShare implements ALBasicProtocolPack._IALProtocolStructure {
/** 攻击玩家昵称 */
private String attackCname;
/** 攻击玩家的联盟简称 */
private String attackGuildSimpleName;
/** 联盟被攻击玩家昵称 */
private String cname;
/** 火星矿实例ID */
private long mineInstanceId;


public ChatContent_GuildMarsMineAttackShare() {
	attackCname = "";
	attackGuildSimpleName = "";
	cname = "";
	mineInstanceId = (long)0;
}

public ChatContent_GuildMarsMineAttackShare(
	 String _attackCname
	, String _attackGuildSimpleName
	, String _cname
	, long _mineInstanceId
) {	attackCname = _attackCname;
	attackGuildSimpleName = _attackGuildSimpleName;
	cname = _cname;
	mineInstanceId = _mineInstanceId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 攻击玩家昵称 */
public String getAttackCname() { return attackCname; }
/** 攻击玩家昵称 */
public void setAttackCname(String _attackCname) { attackCname = _attackCname; }
/** 攻击玩家的联盟简称 */
public String getAttackGuildSimpleName() { return attackGuildSimpleName; }
/** 攻击玩家的联盟简称 */
public void setAttackGuildSimpleName(String _attackGuildSimpleName) { attackGuildSimpleName = _attackGuildSimpleName; }
/** 联盟被攻击玩家昵称 */
public String getCname() { return cname; }
/** 联盟被攻击玩家昵称 */
public void setCname(String _cname) { cname = _cname; }
/** 火星矿实例ID */
public long getMineInstanceId() { return mineInstanceId; }
/** 火星矿实例ID */
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackCname);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackGuildSimpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackCname);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackGuildSimpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attackCname = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attackGuildSimpleName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cname = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineInstanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, attackCname);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, attackGuildSimpleName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, cname);
	_buf.putLong(mineInstanceId);
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

