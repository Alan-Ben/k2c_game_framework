package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 联盟协作奖励据点击败推送
 **/
public class GS2GC_032_077_OnRewardPointDefeat implements ALBasicProtocolPack._IALProtocolStructure {
/** 奖励据点位置 */
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos pos;
/** 解锁据点时的盟主CID */
private long leaderCid;


public GS2GC_032_077_OnRewardPointDefeat() {
	pos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
	leaderCid = (long)0;
}

public GS2GC_032_077_OnRewardPointDefeat(
	 Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos
	, long _leaderCid
) {	pos = _pos;
	leaderCid = _leaderCid;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)77; }

/** 奖励据点位置 */
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getPos() { return pos; }
/** 奖励据点位置 */
public void setPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos) { pos = _pos; }
/** 解锁据点时的盟主CID */
public long getLeaderCid() { return leaderCid; }
/** 解锁据点时的盟主CID */
public void setLeaderCid(long _leaderCid) { leaderCid = _leaderCid; }


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
	if(_buf.remaining() <= 0) return;
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.position();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.position(_posCurPos + _posCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) leaderCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
	_buf.putLong(leaderCid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)77);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)77);
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

