package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 每日签到奖励信息变更
 **/
public class GS2GC_021_069_OnDailyCheckRewardChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 每日签到奖励信息 */
private Common.DailyCheckObj.DailyCheck_RewardInfo rewardInfo;


public GS2GC_021_069_OnDailyCheckRewardChg() {
	rewardInfo = new Common.DailyCheckObj.DailyCheck_RewardInfo();
}

public GS2GC_021_069_OnDailyCheckRewardChg(
	 Common.DailyCheckObj.DailyCheck_RewardInfo _rewardInfo
) {	rewardInfo = _rewardInfo;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)69; }

/** 每日签到奖励信息 */
public Common.DailyCheckObj.DailyCheck_RewardInfo getRewardInfo() { return rewardInfo; }
/** 每日签到奖励信息 */
public void setRewardInfo(Common.DailyCheckObj.DailyCheck_RewardInfo _rewardInfo) { rewardInfo = _rewardInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + rewardInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + rewardInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _rewardInfoCustLen = _buf.getInt();
	int _rewardInfoCurPos = _buf.position();
	rewardInfo.ReadUnzipBuf(_buf, _rewardInfoCurPos + _rewardInfoCustLen);
	_buf.position(_rewardInfoCurPos + _rewardInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(rewardInfo.GetBufSize());
	rewardInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)69);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)69);
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

