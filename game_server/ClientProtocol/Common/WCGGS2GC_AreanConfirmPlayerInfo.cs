using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_AreanConfirmPlayerInfo : ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private string playerName;
private bool isReady;
private long playerIcon;
private Common.Common_Lineup lineup;
private long playerIconBgk;


public WCGGS2GC_AreanConfirmPlayerInfo() {
	uid = (long)0;
	playerName = "";
	isReady = false;
	playerIcon = (long)0;
	lineup = new Common.Common_Lineup();
	playerIconBgk = (long)0;
}

public WCGGS2GC_AreanConfirmPlayerInfo(
	long _uid
	, string _playerName
	, bool _isReady
	, long _playerIcon
	, Common.Common_Lineup _lineup
	, long _playerIconBgk
) {	uid = _uid;
	playerName = _playerName;
	isReady = _isReady;
	playerIcon = _playerIcon;
	lineup = _lineup;
	playerIconBgk = _playerIconBgk;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public string getPlayerName() { return playerName; }
public void setPlayerName(string _playerName) { playerName = _playerName; }
public bool getIsReady() { return isReady; }
public void setIsReady(bool _isReady) { isReady = _isReady; }
public long getPlayerIcon() { return playerIcon; }
public void setPlayerIcon(long _playerIcon) { playerIcon = _playerIcon; }
public Common.Common_Lineup getLineup() { return lineup; }
public void setLineup(Common.Common_Lineup _lineup) { lineup = _lineup; }
public long getPlayerIconBgk() { return playerIconBgk; }
public void setPlayerIconBgk(long _playerIconBgk) { playerIconBgk = _playerIconBgk; }


public int GetBufSize() {
	int _size = 25;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + lineup.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 27;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + lineup.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isReady = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerIcon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _lineupCustLen = _buf.getInt();
	int _lineupCurPos = _buf.getCurPos();
	lineup.ReadUnzipBuf(_buf, _lineupCurPos + _lineupCustLen);
	_buf.setPosition(_lineupCurPos + _lineupCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerIconBgk = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(uid);
	_buf.putString(playerName);
	_buf.put(isReady?(byte)1:(byte)0);
	_buf.putLong(playerIcon);
	_buf.putInt(lineup.GetBufSize());
	lineup.PutUnzipBuf(_buf);
	_buf.putLong(playerIconBgk);
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
	builder.Append("uid").Append(":").Append(uid.ToString()).Append(", ");
	builder.Append("playerName").Append(":").Append(playerName.ToString()).Append(", ");
	builder.Append("isReady").Append(":").Append(isReady.ToString()).Append(", ");
	builder.Append("playerIcon").Append(":").Append(playerIcon.ToString()).Append(", ");
	builder.Append("lineup").Append(":").Append(lineup == null ? "null" : lineup.ToString()).Append(", ");
	builder.Append("playerIconBgk").Append(":").Append(playerIconBgk.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

