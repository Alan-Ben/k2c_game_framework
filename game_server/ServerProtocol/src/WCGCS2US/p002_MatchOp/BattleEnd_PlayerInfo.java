package WCGCS2US.p002_MatchOp;

import java.nio.ByteBuffer;
public class BattleEnd_PlayerInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int summonScore;
private long matchingRewardRefId;
private long matchingOvertimeRewadId;
private long casualMatchingId;
private java.util.ArrayList<Integer> honorList;


public BattleEnd_PlayerInfo() {
	summonScore = 0;
	matchingRewardRefId = (long)0;
	matchingOvertimeRewadId = (long)0;
	casualMatchingId = (long)0;
	honorList = new java.util.ArrayList<Integer>();
}

public BattleEnd_PlayerInfo(
	 int _summonScore
	, long _matchingRewardRefId
	, long _matchingOvertimeRewadId
	, long _casualMatchingId
	, java.util.ArrayList<Integer> _honorList
) {	summonScore = _summonScore;
	matchingRewardRefId = _matchingRewardRefId;
	matchingOvertimeRewadId = _matchingOvertimeRewadId;
	casualMatchingId = _casualMatchingId;
	honorList = _honorList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getSummonScore() { return summonScore; }
public void setSummonScore(int _summonScore) { summonScore = _summonScore; }
public long getMatchingRewardRefId() { return matchingRewardRefId; }
public void setMatchingRewardRefId(long _matchingRewardRefId) { matchingRewardRefId = _matchingRewardRefId; }
public long getMatchingOvertimeRewadId() { return matchingOvertimeRewadId; }
public void setMatchingOvertimeRewadId(long _matchingOvertimeRewadId) { matchingOvertimeRewadId = _matchingOvertimeRewadId; }
public long getCasualMatchingId() { return casualMatchingId; }
public void setCasualMatchingId(long _casualMatchingId) { casualMatchingId = _casualMatchingId; }
public java.util.ArrayList<Integer> getHonorList() { return honorList; }
public void addHonorList(int _honorList) { honorList.add(_honorList); }


public final int GetBufSize() {
	int _size = 28;
	_size += 2 + (honorList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += 2 + (honorList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) summonScore = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) matchingRewardRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) matchingOvertimeRewadId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) casualMatchingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _honorListCount = _buf.getShort();
	for(int _i = 0; _i < _honorListCount; _i++) { 
		int _honorList = 0;
		if(_buf.remaining() > 0) _honorList = _buf.getInt();
		honorList.add(_honorList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(summonScore);
	_buf.putLong(matchingRewardRefId);
	_buf.putLong(matchingOvertimeRewadId);
	_buf.putLong(casualMatchingId);
	_buf.putShort((short)honorList.size());
	for(int _i = 0; _i < honorList.size(); _i++) { 
		_buf.putInt(honorList.get(_i));
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

