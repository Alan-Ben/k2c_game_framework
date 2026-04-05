package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 推荐奖励据点变更推送
 **/
public class GS2GC_032_075_OnRecommendRewardPointChange implements ALBasicProtocolPack._IALProtocolStructure {
/** 推荐奖励据点 */
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos recommendPos;


public GS2GC_032_075_OnRecommendRewardPointChange() {
	recommendPos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
}

public GS2GC_032_075_OnRecommendRewardPointChange(
	 Common.GuildCooperateObj.GuildCooperate_RewardPointPos _recommendPos
) {	recommendPos = _recommendPos;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)75; }

/** 推荐奖励据点 */
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getRecommendPos() { return recommendPos; }
/** 推荐奖励据点 */
public void setRecommendPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _recommendPos) { recommendPos = _recommendPos; }


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
	int _recommendPosCustLen = _buf.getInt();
	int _recommendPosCurPos = _buf.position();
	recommendPos.ReadUnzipBuf(_buf, _recommendPosCurPos + _recommendPosCustLen);
	_buf.position(_recommendPosCurPos + _recommendPosCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(recommendPos.GetBufSize());
	recommendPos.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)75);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)75);
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

