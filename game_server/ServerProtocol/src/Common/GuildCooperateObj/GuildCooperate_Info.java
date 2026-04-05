package Common.GuildCooperateObj;

import java.nio.ByteBuffer;
/*********
 * 联盟协作信息
 **/
public class GuildCooperate_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 下次刷新时间 */
private long nextRefreshTimeMs;
/** 已重置次数 */
private int resetCount;
/** 奖励据点信息列表 */
private java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointInfo> pointList;
/** 推荐奖励据点 */
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos recommendPos;


public GuildCooperate_Info() {
	nextRefreshTimeMs = (long)0;
	resetCount = 0;
	pointList = new java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointInfo>();
	recommendPos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
}

public GuildCooperate_Info(
	 long _nextRefreshTimeMs
	, int _resetCount
	, java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointInfo> _pointList
	, Common.GuildCooperateObj.GuildCooperate_RewardPointPos _recommendPos
) {	nextRefreshTimeMs = _nextRefreshTimeMs;
	resetCount = _resetCount;
	pointList = _pointList;
	recommendPos = _recommendPos;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 下次刷新时间 */
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/** 下次刷新时间 */
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }
/** 已重置次数 */
public int getResetCount() { return resetCount; }
/** 已重置次数 */
public void setResetCount(int _resetCount) { resetCount = _resetCount; }
/** 奖励据点信息列表 */
public java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointInfo> getPointList() { return pointList; }
/** 奖励据点信息列表 */
public void addPointList(Common.GuildCooperateObj.GuildCooperate_RewardPointInfo _pointList) { pointList.add(_pointList); }
/** 推荐奖励据点 */
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getRecommendPos() { return recommendPos; }
/** 推荐奖励据点 */
public void setRecommendPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _recommendPos) { recommendPos = _recommendPos; }


public final int GetBufSize() {
	int _size = 28;
	_size += 2;
	for(int _i = 0; _i < pointList.size(); _i++) {
	_size += 4 + pointList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += 2;
	for(int _i = 0; _i < pointList.size(); _i++) {
	_size += 4 + pointList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resetCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _pointListCount = _buf.getShort();
	for(int _i = 0; _i < _pointListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_RewardPointInfo _pointList = new Common.GuildCooperateObj.GuildCooperate_RewardPointInfo();
		if(_buf.remaining() <= 0) return;
	int __pointListCustLen = _buf.getInt();
	int __pointListCurPos = _buf.position();
	_pointList.ReadUnzipBuf(_buf, __pointListCurPos + __pointListCustLen);
	_buf.position(__pointListCurPos + __pointListCustLen);

		pointList.add(_pointList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _recommendPosCustLen = _buf.getInt();
	int _recommendPosCurPos = _buf.position();
	recommendPos.ReadUnzipBuf(_buf, _recommendPosCurPos + _recommendPosCustLen);
	_buf.position(_recommendPosCurPos + _recommendPosCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(nextRefreshTimeMs);
	_buf.putInt(resetCount);
	_buf.putShort((short)pointList.size());
	for(int _i = 0; _i < pointList.size(); _i++) { 
		_buf.putInt(pointList.get(_i).GetBufSize());
	pointList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(recommendPos.GetBufSize());
	recommendPos.PutUnzipBuf(_buf);
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

