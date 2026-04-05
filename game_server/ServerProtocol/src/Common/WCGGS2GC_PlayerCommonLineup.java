package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_PlayerCommonLineup implements ALBasicProtocolPack._IALProtocolStructure {
private Common.Common_Lineup lineUp;
private long teamConfigID;
private byte[] raceExInfo;
private byte[] playerExInfo;


public WCGGS2GC_PlayerCommonLineup() {
	lineUp = new Common.Common_Lineup();
	teamConfigID = (long)0;
	raceExInfo = null;
	playerExInfo = null;
}

public WCGGS2GC_PlayerCommonLineup(
	 Common.Common_Lineup _lineUp
	, long _teamConfigID
	, byte[] _raceExInfo
	, byte[] _playerExInfo
) {	lineUp = _lineUp;
	teamConfigID = _teamConfigID;
	raceExInfo = _raceExInfo;
	playerExInfo = _playerExInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.Common_Lineup getLineUp() { return lineUp; }
public void setLineUp(Common.Common_Lineup _lineUp) { lineUp = _lineUp; }
public long getTeamConfigID() { return teamConfigID; }
public void setTeamConfigID(long _teamConfigID) { teamConfigID = _teamConfigID; }
public byte[] getRaceExInfo() { return raceExInfo; }
public java.nio.ByteBuffer get_buffer_RaceExInfo() { if(null == raceExInfo)return null; else return ByteBuffer.wrap(raceExInfo); }

public void setRaceExInfo(byte[] _raceExInfo) { raceExInfo = _raceExInfo; }
public void setRaceExInfo(java.nio.ByteBuffer _raceExInfo) 
{
	if(null == _raceExInfo){return;}
	int _oldPos = _raceExInfo.position();
	int _bufLength = _raceExInfo.remaining();
	raceExInfo = new byte[_bufLength];
	_raceExInfo.get(raceExInfo);
	_raceExInfo.position(_oldPos);
}

public byte[] getPlayerExInfo() { return playerExInfo; }
public java.nio.ByteBuffer get_buffer_PlayerExInfo() { if(null == playerExInfo)return null; else return ByteBuffer.wrap(playerExInfo); }

public void setPlayerExInfo(byte[] _playerExInfo) { playerExInfo = _playerExInfo; }
public void setPlayerExInfo(java.nio.ByteBuffer _playerExInfo) 
{
	if(null == _playerExInfo){return;}
	int _oldPos = _playerExInfo.position();
	int _bufLength = _playerExInfo.remaining();
	playerExInfo = new byte[_bufLength];
	_playerExInfo.get(playerExInfo);
	_playerExInfo.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 8;
	_size += 4 + lineUp.GetBufSize();
	_size += 4 + (raceExInfo == null ? 0 : raceExInfo.length);
	_size += 4 + (playerExInfo == null ? 0 : playerExInfo.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + lineUp.GetBufSize();
	_size += 4 + (raceExInfo == null ? 0 : raceExInfo.length);
	_size += 4 + (playerExInfo == null ? 0 : playerExInfo.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _lineUpCustLen = _buf.getInt();
	int _lineUpCurPos = _buf.position();
	lineUp.ReadUnzipBuf(_buf, _lineUpCurPos + _lineUpCustLen);
	_buf.position(_lineUpCurPos + _lineUpCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamConfigID = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _raceExInfoCount = _buf.getInt();
	if(0 < _raceExInfoCount){
		raceExInfo = new byte[_raceExInfoCount];
		_buf.get(raceExInfo);
	}

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _playerExInfoCount = _buf.getInt();
	if(0 < _playerExInfoCount){
		playerExInfo = new byte[_playerExInfoCount];
		_buf.get(playerExInfo);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(lineUp.GetBufSize());
	lineUp.PutUnzipBuf(_buf);
	_buf.putLong(teamConfigID);
	_buf.putInt((raceExInfo == null ? 0 : raceExInfo.length));
	if(null != raceExInfo){_buf.put(raceExInfo);}

	_buf.putInt((playerExInfo == null ? 0 : playerExInfo.length));
	if(null != playerExInfo){_buf.put(playerExInfo);}

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

