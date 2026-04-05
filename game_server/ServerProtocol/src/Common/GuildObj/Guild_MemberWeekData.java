package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟成员本周信息
 **/
public class Guild_MemberWeekData implements ALBasicProtocolPack._IALProtocolStructure {
/** 周标记 */
private int weekTag;
/** 已领取活跃宝箱次数 */
private int hadDrawActiveBoxNum;
/** 获得活跃印章数量 */
private int gainActiveStampNum;
/** 已领取联盟大礼次数 */
private int hadDrawGreatRewardNum;


public Guild_MemberWeekData() {
	weekTag = 0;
	hadDrawActiveBoxNum = 0;
	gainActiveStampNum = 0;
	hadDrawGreatRewardNum = 0;
}

public Guild_MemberWeekData(
	 int _weekTag
	, int _hadDrawActiveBoxNum
	, int _gainActiveStampNum
	, int _hadDrawGreatRewardNum
) {	weekTag = _weekTag;
	hadDrawActiveBoxNum = _hadDrawActiveBoxNum;
	gainActiveStampNum = _gainActiveStampNum;
	hadDrawGreatRewardNum = _hadDrawGreatRewardNum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 周标记 */
public int getWeekTag() { return weekTag; }
/** 周标记 */
public void setWeekTag(int _weekTag) { weekTag = _weekTag; }
/** 已领取活跃宝箱次数 */
public int getHadDrawActiveBoxNum() { return hadDrawActiveBoxNum; }
/** 已领取活跃宝箱次数 */
public void setHadDrawActiveBoxNum(int _hadDrawActiveBoxNum) { hadDrawActiveBoxNum = _hadDrawActiveBoxNum; }
/** 获得活跃印章数量 */
public int getGainActiveStampNum() { return gainActiveStampNum; }
/** 获得活跃印章数量 */
public void setGainActiveStampNum(int _gainActiveStampNum) { gainActiveStampNum = _gainActiveStampNum; }
/** 已领取联盟大礼次数 */
public int getHadDrawGreatRewardNum() { return hadDrawGreatRewardNum; }
/** 已领取联盟大礼次数 */
public void setHadDrawGreatRewardNum(int _hadDrawGreatRewardNum) { hadDrawGreatRewardNum = _hadDrawGreatRewardNum; }


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
	if(_buf.remaining() > 0) weekTag = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawActiveBoxNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainActiveStampNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawGreatRewardNum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(weekTag);
	_buf.putInt(hadDrawActiveBoxNum);
	_buf.putInt(gainActiveStampNum);
	_buf.putInt(hadDrawGreatRewardNum);
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

