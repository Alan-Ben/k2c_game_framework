package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 火星探险数据初始化
 **/
public class GS2GC_002_077_RetMarsExploreInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 探索数据 */
private Common.MarsObj.Mars_Explore explore;
/** 事件数据 */
private java.util.ArrayList<Common.MarsObj.Mars_ExploreEvent> eventList;
/** 队伍列表 */
private java.util.ArrayList<Common.MarsObj.Mars_Team> teamList;
/** 火星矿产数据索引列表 */
private java.util.ArrayList<Common.MarsObj.Mars_MineIdx> mineIdx;


public GS2GC_002_077_RetMarsExploreInit() {
	explore = new Common.MarsObj.Mars_Explore();
	eventList = new java.util.ArrayList<Common.MarsObj.Mars_ExploreEvent>();
	teamList = new java.util.ArrayList<Common.MarsObj.Mars_Team>();
	mineIdx = new java.util.ArrayList<Common.MarsObj.Mars_MineIdx>();
}

public GS2GC_002_077_RetMarsExploreInit(
	 Common.MarsObj.Mars_Explore _explore
	, java.util.ArrayList<Common.MarsObj.Mars_ExploreEvent> _eventList
	, java.util.ArrayList<Common.MarsObj.Mars_Team> _teamList
	, java.util.ArrayList<Common.MarsObj.Mars_MineIdx> _mineIdx
) {	explore = _explore;
	eventList = _eventList;
	teamList = _teamList;
	mineIdx = _mineIdx;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)77; }

/** 探索数据 */
public Common.MarsObj.Mars_Explore getExplore() { return explore; }
/** 探索数据 */
public void setExplore(Common.MarsObj.Mars_Explore _explore) { explore = _explore; }
/** 事件数据 */
public java.util.ArrayList<Common.MarsObj.Mars_ExploreEvent> getEventList() { return eventList; }
/** 事件数据 */
public void addEventList(Common.MarsObj.Mars_ExploreEvent _eventList) { eventList.add(_eventList); }
/** 队伍列表 */
public java.util.ArrayList<Common.MarsObj.Mars_Team> getTeamList() { return teamList; }
/** 队伍列表 */
public void addTeamList(Common.MarsObj.Mars_Team _teamList) { teamList.add(_teamList); }
/** 火星矿产数据索引列表 */
public java.util.ArrayList<Common.MarsObj.Mars_MineIdx> getMineIdx() { return mineIdx; }
/** 火星矿产数据索引列表 */
public void addMineIdx(Common.MarsObj.Mars_MineIdx _mineIdx) { mineIdx.add(_mineIdx); }


public final int GetBufSize() {
	int _size = 12;
	_size += 2 + (eventList.size() * 45);
	_size += 2;
	for(int _i = 0; _i < teamList.size(); _i++) {
	_size += 4 + teamList.get(_i).GetBufSize();
	}

	_size += 2 + (mineIdx.size() * 44);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (eventList.size() * 45);
	_size += 2;
	for(int _i = 0; _i < teamList.size(); _i++) {
	_size += 4 + teamList.get(_i).GetBufSize();
	}

	_size += 2 + (mineIdx.size() * 44);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _exploreCustLen = _buf.getInt();
	int _exploreCurPos = _buf.position();
	explore.ReadUnzipBuf(_buf, _exploreCurPos + _exploreCustLen);
	_buf.position(_exploreCurPos + _exploreCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _eventListCount = _buf.getShort();
	for(int _i = 0; _i < _eventListCount; _i++) { 
		Common.MarsObj.Mars_ExploreEvent _eventList = new Common.MarsObj.Mars_ExploreEvent();
		if(_buf.remaining() <= 0) return;
	int __eventListCustLen = _buf.getInt();
	int __eventListCurPos = _buf.position();
	_eventList.ReadUnzipBuf(_buf, __eventListCurPos + __eventListCustLen);
	_buf.position(__eventListCurPos + __eventListCustLen);

		eventList.add(_eventList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _teamListCount = _buf.getShort();
	for(int _i = 0; _i < _teamListCount; _i++) { 
		Common.MarsObj.Mars_Team _teamList = new Common.MarsObj.Mars_Team();
		if(_buf.remaining() <= 0) return;
	int __teamListCustLen = _buf.getInt();
	int __teamListCurPos = _buf.position();
	_teamList.ReadUnzipBuf(_buf, __teamListCurPos + __teamListCustLen);
	_buf.position(__teamListCurPos + __teamListCustLen);

		teamList.add(_teamList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _mineIdxCount = _buf.getShort();
	for(int _i = 0; _i < _mineIdxCount; _i++) { 
		Common.MarsObj.Mars_MineIdx _mineIdx = new Common.MarsObj.Mars_MineIdx();
		if(_buf.remaining() <= 0) return;
	int __mineIdxCustLen = _buf.getInt();
	int __mineIdxCurPos = _buf.position();
	_mineIdx.ReadUnzipBuf(_buf, __mineIdxCurPos + __mineIdxCustLen);
	_buf.position(__mineIdxCurPos + __mineIdxCustLen);

		mineIdx.add(_mineIdx);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(explore.GetBufSize());
	explore.PutUnzipBuf(_buf);
	_buf.putShort((short)eventList.size());
	for(int _i = 0; _i < eventList.size(); _i++) { 
		_buf.putInt(eventList.get(_i).GetBufSize());
	eventList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)teamList.size());
	for(int _i = 0; _i < teamList.size(); _i++) { 
		_buf.putInt(teamList.get(_i).GetBufSize());
	teamList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)mineIdx.size());
	for(int _i = 0; _i < mineIdx.size(); _i++) { 
		_buf.putInt(mineIdx.get(_i).GetBufSize());
	mineIdx.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)77);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)77);
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

