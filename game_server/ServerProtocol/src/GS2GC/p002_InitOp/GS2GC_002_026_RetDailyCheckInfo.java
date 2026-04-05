package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_026_RetDailyCheckInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 每日签到信息 */
private Common.DailyCheckObj.DailyCheck_Info info;
/** 每日签到奖励信息 */
private Common.DailyCheckObj.DailyCheck_RewardInfo rewardInfo;


public GS2GC_002_026_RetDailyCheckInfo() {
	info = new Common.DailyCheckObj.DailyCheck_Info();
	rewardInfo = new Common.DailyCheckObj.DailyCheck_RewardInfo();
}

public GS2GC_002_026_RetDailyCheckInfo(
	 Common.DailyCheckObj.DailyCheck_Info _info
	, Common.DailyCheckObj.DailyCheck_RewardInfo _rewardInfo
) {	info = _info;
	rewardInfo = _rewardInfo;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)26; }

/** 每日签到信息 */
public Common.DailyCheckObj.DailyCheck_Info getInfo() { return info; }
/** 每日签到信息 */
public void setInfo(Common.DailyCheckObj.DailyCheck_Info _info) { info = _info; }
/** 每日签到奖励信息 */
public Common.DailyCheckObj.DailyCheck_RewardInfo getRewardInfo() { return rewardInfo; }
/** 每日签到奖励信息 */
public void setRewardInfo(Common.DailyCheckObj.DailyCheck_RewardInfo _rewardInfo) { rewardInfo = _rewardInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + info.GetBufSize();
	_size += 4 + rewardInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + info.GetBufSize();
	_size += 4 + rewardInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _rewardInfoCustLen = _buf.getInt();
	int _rewardInfoCurPos = _buf.position();
	rewardInfo.ReadUnzipBuf(_buf, _rewardInfoCurPos + _rewardInfoCustLen);
	_buf.position(_rewardInfoCurPos + _rewardInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putInt(rewardInfo.GetBufSize());
	rewardInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)26);
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

