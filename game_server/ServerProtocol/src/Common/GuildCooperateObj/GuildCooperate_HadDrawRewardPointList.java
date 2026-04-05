package Common.GuildCooperateObj;

import java.nio.ByteBuffer;
/*********
 * 联盟协作已领取奖励点
 **/
public class GuildCooperate_HadDrawRewardPointList implements ALBasicProtocolPack._IALProtocolStructure {
/** 上次刷新时间 ms */
private long lastRefreshTimeMs;
/** 已领取奖励点列表 */
private java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> hadDrawList;


public GuildCooperate_HadDrawRewardPointList() {
	lastRefreshTimeMs = (long)0;
	hadDrawList = new java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointPos>();
}

public GuildCooperate_HadDrawRewardPointList(
	 long _lastRefreshTimeMs
	, java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> _hadDrawList
) {	lastRefreshTimeMs = _lastRefreshTimeMs;
	hadDrawList = _hadDrawList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 上次刷新时间 ms */
public long getLastRefreshTimeMs() { return lastRefreshTimeMs; }
/** 上次刷新时间 ms */
public void setLastRefreshTimeMs(long _lastRefreshTimeMs) { lastRefreshTimeMs = _lastRefreshTimeMs; }
/** 已领取奖励点列表 */
public java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> getHadDrawList() { return hadDrawList; }
/** 已领取奖励点列表 */
public void addHadDrawList(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _hadDrawList) { hadDrawList.add(_hadDrawList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (hadDrawList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (hadDrawList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_RewardPointPos _hadDrawList = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
		if(_buf.remaining() <= 0) return;
	int __hadDrawListCustLen = _buf.getInt();
	int __hadDrawListCurPos = _buf.position();
	_hadDrawList.ReadUnzipBuf(_buf, __hadDrawListCurPos + __hadDrawListCustLen);
	_buf.position(__hadDrawListCurPos + __hadDrawListCustLen);

		hadDrawList.add(_hadDrawList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(lastRefreshTimeMs);
	_buf.putShort((short)hadDrawList.size());
	for(int _i = 0; _i < hadDrawList.size(); _i++) { 
		_buf.putInt(hadDrawList.get(_i).GetBufSize());
	hadDrawList.get(_i).PutUnzipBuf(_buf);
	}
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

