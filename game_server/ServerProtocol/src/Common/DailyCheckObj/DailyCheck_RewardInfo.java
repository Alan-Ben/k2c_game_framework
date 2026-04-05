package Common.DailyCheckObj;

import java.nio.ByteBuffer;
/*********
 * 每日签到奖励信息
 **/
public class DailyCheck_RewardInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 累计签到天数 */
private int totalCheckDays;
/** 已经领取奖励的天数 */
private int rewardedDays;
/** 展示数据列表 */
private java.util.ArrayList<Common.DailyCheckObj.DailyCheck_RewardShowInfo> showInfoList;
/** 数据列表的前一条数据，即上一组最后一条展示数据 */
private Common.DailyCheckObj.DailyCheck_RewardShowInfo preShowInfo;
/** 数据列表的后一条数据，即下一组第一条展示数据 */
private Common.DailyCheckObj.DailyCheck_RewardShowInfo nextShowInfo;


public DailyCheck_RewardInfo() {
	totalCheckDays = 0;
	rewardedDays = 0;
	showInfoList = new java.util.ArrayList<Common.DailyCheckObj.DailyCheck_RewardShowInfo>();
	preShowInfo = new Common.DailyCheckObj.DailyCheck_RewardShowInfo();
	nextShowInfo = new Common.DailyCheckObj.DailyCheck_RewardShowInfo();
}

public DailyCheck_RewardInfo(
	 int _totalCheckDays
	, int _rewardedDays
	, java.util.ArrayList<Common.DailyCheckObj.DailyCheck_RewardShowInfo> _showInfoList
	, Common.DailyCheckObj.DailyCheck_RewardShowInfo _preShowInfo
	, Common.DailyCheckObj.DailyCheck_RewardShowInfo _nextShowInfo
) {	totalCheckDays = _totalCheckDays;
	rewardedDays = _rewardedDays;
	showInfoList = _showInfoList;
	preShowInfo = _preShowInfo;
	nextShowInfo = _nextShowInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 累计签到天数 */
public int getTotalCheckDays() { return totalCheckDays; }
/** 累计签到天数 */
public void setTotalCheckDays(int _totalCheckDays) { totalCheckDays = _totalCheckDays; }
/** 已经领取奖励的天数 */
public int getRewardedDays() { return rewardedDays; }
/** 已经领取奖励的天数 */
public void setRewardedDays(int _rewardedDays) { rewardedDays = _rewardedDays; }
/** 展示数据列表 */
public java.util.ArrayList<Common.DailyCheckObj.DailyCheck_RewardShowInfo> getShowInfoList() { return showInfoList; }
/** 展示数据列表 */
public void addShowInfoList(Common.DailyCheckObj.DailyCheck_RewardShowInfo _showInfoList) { showInfoList.add(_showInfoList); }
/** 数据列表的前一条数据，即上一组最后一条展示数据 */
public Common.DailyCheckObj.DailyCheck_RewardShowInfo getPreShowInfo() { return preShowInfo; }
/** 数据列表的前一条数据，即上一组最后一条展示数据 */
public void setPreShowInfo(Common.DailyCheckObj.DailyCheck_RewardShowInfo _preShowInfo) { preShowInfo = _preShowInfo; }
/** 数据列表的后一条数据，即下一组第一条展示数据 */
public Common.DailyCheckObj.DailyCheck_RewardShowInfo getNextShowInfo() { return nextShowInfo; }
/** 数据列表的后一条数据，即下一组第一条展示数据 */
public void setNextShowInfo(Common.DailyCheckObj.DailyCheck_RewardShowInfo _nextShowInfo) { nextShowInfo = _nextShowInfo; }


public final int GetBufSize() {
	int _size = 40;
	_size += 2 + (showInfoList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;
	_size += 2 + (showInfoList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalCheckDays = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rewardedDays = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _showInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _showInfoListCount; _i++) { 
		Common.DailyCheckObj.DailyCheck_RewardShowInfo _showInfoList = new Common.DailyCheckObj.DailyCheck_RewardShowInfo();
		if(_buf.remaining() <= 0) return;
	int __showInfoListCustLen = _buf.getInt();
	int __showInfoListCurPos = _buf.position();
	_showInfoList.ReadUnzipBuf(_buf, __showInfoListCurPos + __showInfoListCustLen);
	_buf.position(__showInfoListCurPos + __showInfoListCustLen);

		showInfoList.add(_showInfoList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _preShowInfoCustLen = _buf.getInt();
	int _preShowInfoCurPos = _buf.position();
	preShowInfo.ReadUnzipBuf(_buf, _preShowInfoCurPos + _preShowInfoCustLen);
	_buf.position(_preShowInfoCurPos + _preShowInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _nextShowInfoCustLen = _buf.getInt();
	int _nextShowInfoCurPos = _buf.position();
	nextShowInfo.ReadUnzipBuf(_buf, _nextShowInfoCurPos + _nextShowInfoCustLen);
	_buf.position(_nextShowInfoCurPos + _nextShowInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(totalCheckDays);
	_buf.putInt(rewardedDays);
	_buf.putShort((short)showInfoList.size());
	for(int _i = 0; _i < showInfoList.size(); _i++) { 
		_buf.putInt(showInfoList.get(_i).GetBufSize());
	showInfoList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(preShowInfo.GetBufSize());
	preShowInfo.PutUnzipBuf(_buf);
	_buf.putInt(nextShowInfo.GetBufSize());
	nextShowInfo.PutUnzipBuf(_buf);
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

