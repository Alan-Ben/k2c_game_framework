using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_PlayerCommonLineup : ALBasicProtocolPack._IALProtocolStructure {
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public Common.Common_Lineup getLineUp() { return lineUp; }
public void setLineUp(Common.Common_Lineup _lineUp) { lineUp = _lineUp; }
public long getTeamConfigID() { return teamConfigID; }
public void setTeamConfigID(long _teamConfigID) { teamConfigID = _teamConfigID; }
public byte[] getRaceExInfo() { return raceExInfo; }

public void setRaceExInfo(byte[] _raceExInfo) { raceExInfo = _raceExInfo; }

public byte[] getPlayerExInfo() { return playerExInfo; }

public void setPlayerExInfo(byte[] _playerExInfo) { playerExInfo = _playerExInfo; }



public int GetBufSize() {
	int _size = 8;
	_size += 4 + lineUp.GetBufSize();
	_size += 4 + (raceExInfo == null ? 0 : raceExInfo.Length);
	_size += 4 + (playerExInfo == null ? 0 : playerExInfo.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + lineUp.GetBufSize();
	_size += 4 + (raceExInfo == null ? 0 : raceExInfo.Length);
	_size += 4 + (playerExInfo == null ? 0 : playerExInfo.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _lineUpCustLen = _buf.getInt();
	int _lineUpCurPos = _buf.getCurPos();
	lineUp.ReadUnzipBuf(_buf, _lineUpCurPos + _lineUpCustLen);
	_buf.setPosition(_lineUpCurPos + _lineUpCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamConfigID = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	raceExInfo = _buf.getByteBuffer();

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerExInfo = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(lineUp.GetBufSize());
	lineUp.PutUnzipBuf(_buf);
	_buf.putLong(teamConfigID);
	_buf.putByteBuffer(raceExInfo);

	_buf.putByteBuffer(playerExInfo);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("lineUp").Append(":").Append(lineUp == null ? "null" : lineUp.ToString()).Append(", ");
	builder.Append("teamConfigID").Append(":").Append(teamConfigID.ToString()).Append(", ");
	builder.Append("raceExInfo").Append(":").Append(raceExInfo == null ? "null" : raceExInfo.ToString()).Append(", ");
	builder.Append("playerExInfo").Append(":").Append(playerExInfo == null ? "null" : playerExInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

