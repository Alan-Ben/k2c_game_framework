package WCGCS2US.p002_MatchOp;

import java.nio.ByteBuffer;
public class WCGCS2US_002_020_OnArenaEnd implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private long usedRace;
private int arenaType;
private java.util.ArrayList<Long> winUidList;
private boolean isDraw;
private WCGCS2US.p002_MatchOp.BattleEnd_PlayerInfo battleEndPlayerInfo;
private WCGCS2US.p002_MatchOp.BattleEnd_BattileInfo battleEndBattleInfo;
private int matchingType;


public WCGCS2US_002_020_OnArenaEnd() {
	uid = (long)0;
	usedRace = (long)0;
	arenaType = 0;
	winUidList = new java.util.ArrayList<Long>();
	isDraw = false;
	battleEndPlayerInfo = new WCGCS2US.p002_MatchOp.BattleEnd_PlayerInfo();
	battleEndBattleInfo = new WCGCS2US.p002_MatchOp.BattleEnd_BattileInfo();
	matchingType = 0;
}

public WCGCS2US_002_020_OnArenaEnd(
	 long _uid
	, long _usedRace
	, int _arenaType
	, java.util.ArrayList<Long> _winUidList
	, boolean _isDraw
	, WCGCS2US.p002_MatchOp.BattleEnd_PlayerInfo _battleEndPlayerInfo
	, WCGCS2US.p002_MatchOp.BattleEnd_BattileInfo _battleEndBattleInfo
	, int _matchingType
) {	uid = _uid;
	usedRace = _usedRace;
	arenaType = _arenaType;
	winUidList = _winUidList;
	isDraw = _isDraw;
	battleEndPlayerInfo = _battleEndPlayerInfo;
	battleEndBattleInfo = _battleEndBattleInfo;
	matchingType = _matchingType;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)20; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public long getUsedRace() { return usedRace; }
public void setUsedRace(long _usedRace) { usedRace = _usedRace; }
public int getArenaType() { return arenaType; }
public void setArenaType(int _arenaType) { arenaType = _arenaType; }
public java.util.ArrayList<Long> getWinUidList() { return winUidList; }
public void addWinUidList(long _winUidList) { winUidList.add(_winUidList); }
public boolean getIsDraw() { return isDraw; }
public void setIsDraw(boolean _isDraw) { isDraw = _isDraw; }
public WCGCS2US.p002_MatchOp.BattleEnd_PlayerInfo getBattleEndPlayerInfo() { return battleEndPlayerInfo; }
public void setBattleEndPlayerInfo(WCGCS2US.p002_MatchOp.BattleEnd_PlayerInfo _battleEndPlayerInfo) { battleEndPlayerInfo = _battleEndPlayerInfo; }
public WCGCS2US.p002_MatchOp.BattleEnd_BattileInfo getBattleEndBattleInfo() { return battleEndBattleInfo; }
public void setBattleEndBattleInfo(WCGCS2US.p002_MatchOp.BattleEnd_BattileInfo _battleEndBattleInfo) { battleEndBattleInfo = _battleEndBattleInfo; }
public int getMatchingType() { return matchingType; }
public void setMatchingType(int _matchingType) { matchingType = _matchingType; }


public final int GetBufSize() {
	int _size = 45;
	_size += 2 + (winUidList.size() * 8);
	_size += 4 + battleEndPlayerInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 47;
	_size += 2 + (winUidList.size() * 8);
	_size += 4 + battleEndPlayerInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usedRace = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) arenaType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _winUidListCount = _buf.getShort();
	for(int _i = 0; _i < _winUidListCount; _i++) { 
		long _winUidList = (long)0;
		if(_buf.remaining() > 0) _winUidList = _buf.getLong();
		winUidList.add(_winUidList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isDraw = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _battleEndPlayerInfoCustLen = _buf.getInt();
	int _battleEndPlayerInfoCurPos = _buf.position();
	battleEndPlayerInfo.ReadUnzipBuf(_buf, _battleEndPlayerInfoCurPos + _battleEndPlayerInfoCustLen);
	_buf.position(_battleEndPlayerInfoCurPos + _battleEndPlayerInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _battleEndBattleInfoCustLen = _buf.getInt();
	int _battleEndBattleInfoCurPos = _buf.position();
	battleEndBattleInfo.ReadUnzipBuf(_buf, _battleEndBattleInfoCurPos + _battleEndBattleInfoCustLen);
	_buf.position(_battleEndBattleInfoCurPos + _battleEndBattleInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) matchingType = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putLong(usedRace);
	_buf.putInt(arenaType);
	_buf.putShort((short)winUidList.size());
	for(int _i = 0; _i < winUidList.size(); _i++) { 
		_buf.putLong(winUidList.get(_i));
	}
	_buf.put(isDraw?(byte)1:(byte)0);
	_buf.putInt(battleEndPlayerInfo.GetBufSize());
	battleEndPlayerInfo.PutUnzipBuf(_buf);
	_buf.putInt(battleEndBattleInfo.GetBufSize());
	battleEndBattleInfo.PutUnzipBuf(_buf);
	_buf.putInt(matchingType);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)20);
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

