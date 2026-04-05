package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_063_RetGuildInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 自己请求加入联盟列表 */
private java.util.ArrayList<Long> selfRequestJoinGuildList;
/** 加入联盟CD信息 */
private Common.GuildObj.Guild_JoinCdInfo joinCdInfo;
/** 每日数据 */
private Common.GuildObj.Guild_MemberDailyData dailyData;
private Common.GuildObj.Guild_DetailInfo guildInfo;


public GS2GC_002_063_RetGuildInit() {
	selfRequestJoinGuildList = new java.util.ArrayList<Long>();
	joinCdInfo = new Common.GuildObj.Guild_JoinCdInfo();
	dailyData = new Common.GuildObj.Guild_MemberDailyData();
	guildInfo = new Common.GuildObj.Guild_DetailInfo();
}

public GS2GC_002_063_RetGuildInit(
	 java.util.ArrayList<Long> _selfRequestJoinGuildList
	, Common.GuildObj.Guild_JoinCdInfo _joinCdInfo
	, Common.GuildObj.Guild_MemberDailyData _dailyData
	, Common.GuildObj.Guild_DetailInfo _guildInfo
) {	selfRequestJoinGuildList = _selfRequestJoinGuildList;
	joinCdInfo = _joinCdInfo;
	dailyData = _dailyData;
	guildInfo = _guildInfo;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)63; }

/** 自己请求加入联盟列表 */
public java.util.ArrayList<Long> getSelfRequestJoinGuildList() { return selfRequestJoinGuildList; }
/** 自己请求加入联盟列表 */
public void addSelfRequestJoinGuildList(long _selfRequestJoinGuildList) { selfRequestJoinGuildList.add(_selfRequestJoinGuildList); }
/** 加入联盟CD信息 */
public Common.GuildObj.Guild_JoinCdInfo getJoinCdInfo() { return joinCdInfo; }
/** 加入联盟CD信息 */
public void setJoinCdInfo(Common.GuildObj.Guild_JoinCdInfo _joinCdInfo) { joinCdInfo = _joinCdInfo; }
/** 每日数据 */
public Common.GuildObj.Guild_MemberDailyData getDailyData() { return dailyData; }
/** 每日数据 */
public void setDailyData(Common.GuildObj.Guild_MemberDailyData _dailyData) { dailyData = _dailyData; }
public Common.GuildObj.Guild_DetailInfo getGuildInfo() { return guildInfo; }
public void setGuildInfo(Common.GuildObj.Guild_DetailInfo _guildInfo) { guildInfo = _guildInfo; }


public final int GetBufSize() {
	int _size = 16;
	_size += 2 + (selfRequestJoinGuildList.size() * 8);
	_size += 4 + dailyData.GetBufSize();
	_size += 4 + guildInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (selfRequestJoinGuildList.size() * 8);
	_size += 4 + dailyData.GetBufSize();
	_size += 4 + guildInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _selfRequestJoinGuildListCount = _buf.getShort();
	for(int _i = 0; _i < _selfRequestJoinGuildListCount; _i++) { 
		long _selfRequestJoinGuildList = (long)0;
		if(_buf.remaining() > 0) _selfRequestJoinGuildList = _buf.getLong();
		selfRequestJoinGuildList.add(_selfRequestJoinGuildList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _joinCdInfoCustLen = _buf.getInt();
	int _joinCdInfoCurPos = _buf.position();
	joinCdInfo.ReadUnzipBuf(_buf, _joinCdInfoCurPos + _joinCdInfoCustLen);
	_buf.position(_joinCdInfoCurPos + _joinCdInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _dailyDataCustLen = _buf.getInt();
	int _dailyDataCurPos = _buf.position();
	dailyData.ReadUnzipBuf(_buf, _dailyDataCurPos + _dailyDataCustLen);
	_buf.position(_dailyDataCurPos + _dailyDataCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _guildInfoCustLen = _buf.getInt();
	int _guildInfoCurPos = _buf.position();
	guildInfo.ReadUnzipBuf(_buf, _guildInfoCurPos + _guildInfoCustLen);
	_buf.position(_guildInfoCurPos + _guildInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)selfRequestJoinGuildList.size());
	for(int _i = 0; _i < selfRequestJoinGuildList.size(); _i++) { 
		_buf.putLong(selfRequestJoinGuildList.get(_i));
	}
	_buf.putInt(joinCdInfo.GetBufSize());
	joinCdInfo.PutUnzipBuf(_buf);
	_buf.putInt(dailyData.GetBufSize());
	dailyData.PutUnzipBuf(_buf);
	_buf.putInt(guildInfo.GetBufSize());
	guildInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)63);
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

