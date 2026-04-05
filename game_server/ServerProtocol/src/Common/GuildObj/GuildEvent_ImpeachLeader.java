package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟弹劾盟主事件
 **/
public class GuildEvent_ImpeachLeader implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件状态 */
private Common.GuildEnum.EGuildImpeachLeaderEventState state;
/** 盟主cid */
private long leaderCid;
/** 发起时间戳 */
private long requestTimeMs;
/** 同意列表 */
private java.util.ArrayList<Long> agreeMemberCidList;


public GuildEvent_ImpeachLeader() {
	state = Common.GuildEnum.EGuildImpeachLeaderEventState.values()[0];
	leaderCid = (long)0;
	requestTimeMs = (long)0;
	agreeMemberCidList = new java.util.ArrayList<Long>();
}

public GuildEvent_ImpeachLeader(
	 Common.GuildEnum.EGuildImpeachLeaderEventState _state
	, long _leaderCid
	, long _requestTimeMs
	, java.util.ArrayList<Long> _agreeMemberCidList
) {	state = _state;
	leaderCid = _leaderCid;
	requestTimeMs = _requestTimeMs;
	agreeMemberCidList = _agreeMemberCidList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 事件状态 */
public Common.GuildEnum.EGuildImpeachLeaderEventState getState() { return state; }
/** 事件状态 */
public void setState(Common.GuildEnum.EGuildImpeachLeaderEventState _state) { state = _state; }
/** 盟主cid */
public long getLeaderCid() { return leaderCid; }
/** 盟主cid */
public void setLeaderCid(long _leaderCid) { leaderCid = _leaderCid; }
/** 发起时间戳 */
public long getRequestTimeMs() { return requestTimeMs; }
/** 发起时间戳 */
public void setRequestTimeMs(long _requestTimeMs) { requestTimeMs = _requestTimeMs; }
/** 同意列表 */
public java.util.ArrayList<Long> getAgreeMemberCidList() { return agreeMemberCidList; }
/** 同意列表 */
public void addAgreeMemberCidList(long _agreeMemberCidList) { agreeMemberCidList.add(_agreeMemberCidList); }


public final int GetBufSize() {
	int _size = 20;
	_size += 2 + (agreeMemberCidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (agreeMemberCidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) state = Common.GuildEnum.EGuildImpeachLeaderEventState.EGuildImpeachLeaderEventState_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) leaderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) requestTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _agreeMemberCidListCount = _buf.getShort();
	for(int _i = 0; _i < _agreeMemberCidListCount; _i++) { 
		long _agreeMemberCidList = (long)0;
		if(_buf.remaining() > 0) _agreeMemberCidList = _buf.getLong();
		agreeMemberCidList.add(_agreeMemberCidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(state.ordinal());

	_buf.putLong(leaderCid);
	_buf.putLong(requestTimeMs);
	_buf.putShort((short)agreeMemberCidList.size());
	for(int _i = 0; _i < agreeMemberCidList.size(); _i++) { 
		_buf.putLong(agreeMemberCidList.get(_i));
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

