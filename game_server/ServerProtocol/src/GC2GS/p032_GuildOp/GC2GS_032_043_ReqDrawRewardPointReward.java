package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 领取奖励据点个人奖励
 **/
public class GC2GS_032_043_ReqDrawRewardPointReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 奖励据点位置 */
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos pos;


public GC2GS_032_043_ReqDrawRewardPointReward() {
	pos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
}

public GC2GS_032_043_ReqDrawRewardPointReward(
	 Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos
) {	pos = _pos;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)43; }

/** 奖励据点位置 */
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getPos() { return pos; }
/** 奖励据点位置 */
public void setPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos) { pos = _pos; }


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
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.position();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.position(_posCurPos + _posCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)43);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)43);
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

