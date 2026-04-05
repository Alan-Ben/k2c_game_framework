using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_CustomRoomListItem : ALBasicProtocolPack._IALProtocolStructure {
private long roomId;
private bool isNeedPassord;
private int minGrade;
private int matchType;
private int roomState;
private long hostUid;
private string hostName;
private int maxFightNum;
private int curFightNum;
private int maxObNum;
private int curObNum;
private long dungeonId;
private bool isCanOb;
private bool isEquit;


public Common_CustomRoomListItem() {
	roomId = (long)0;
	isNeedPassord = false;
	minGrade = 0;
	matchType = 0;
	roomState = 0;
	hostUid = (long)0;
	hostName = "";
	maxFightNum = 0;
	curFightNum = 0;
	maxObNum = 0;
	curObNum = 0;
	dungeonId = (long)0;
	isCanOb = false;
	isEquit = false;
}

public Common_CustomRoomListItem(
	long _roomId
	, bool _isNeedPassord
	, int _minGrade
	, int _matchType
	, int _roomState
	, long _hostUid
	, string _hostName
	, int _maxFightNum
	, int _curFightNum
	, int _maxObNum
	, int _curObNum
	, long _dungeonId
	, bool _isCanOb
	, bool _isEquit
) {	roomId = _roomId;
	isNeedPassord = _isNeedPassord;
	minGrade = _minGrade;
	matchType = _matchType;
	roomState = _roomState;
	hostUid = _hostUid;
	hostName = _hostName;
	maxFightNum = _maxFightNum;
	curFightNum = _curFightNum;
	maxObNum = _maxObNum;
	curObNum = _curObNum;
	dungeonId = _dungeonId;
	isCanOb = _isCanOb;
	isEquit = _isEquit;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getRoomId() { return roomId; }
public void setRoomId(long _roomId) { roomId = _roomId; }
public bool getIsNeedPassord() { return isNeedPassord; }
public void setIsNeedPassord(bool _isNeedPassord) { isNeedPassord = _isNeedPassord; }
public int getMinGrade() { return minGrade; }
public void setMinGrade(int _minGrade) { minGrade = _minGrade; }
public int getMatchType() { return matchType; }
public void setMatchType(int _matchType) { matchType = _matchType; }
public int getRoomState() { return roomState; }
public void setRoomState(int _roomState) { roomState = _roomState; }
public long getHostUid() { return hostUid; }
public void setHostUid(long _hostUid) { hostUid = _hostUid; }
public string getHostName() { return hostName; }
public void setHostName(string _hostName) { hostName = _hostName; }
public int getMaxFightNum() { return maxFightNum; }
public void setMaxFightNum(int _maxFightNum) { maxFightNum = _maxFightNum; }
public int getCurFightNum() { return curFightNum; }
public void setCurFightNum(int _curFightNum) { curFightNum = _curFightNum; }
public int getMaxObNum() { return maxObNum; }
public void setMaxObNum(int _maxObNum) { maxObNum = _maxObNum; }
public int getCurObNum() { return curObNum; }
public void setCurObNum(int _curObNum) { curObNum = _curObNum; }
public long getDungeonId() { return dungeonId; }
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
public bool getIsCanOb() { return isCanOb; }
public void setIsCanOb(bool _isCanOb) { isCanOb = _isCanOb; }
public bool getIsEquit() { return isEquit; }
public void setIsEquit(bool _isEquit) { isEquit = _isEquit; }


public int GetBufSize() {
	int _size = 55;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(hostName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 57;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(hostName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isNeedPassord = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	minGrade = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	matchType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roomState = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hostUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hostName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxFightNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curFightNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxObNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curObNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isCanOb = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isEquit = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(roomId);
	_buf.put(isNeedPassord?(byte)1:(byte)0);
	_buf.putInt(minGrade);
	_buf.putInt(matchType);
	_buf.putInt(roomState);
	_buf.putLong(hostUid);
	_buf.putString(hostName);
	_buf.putInt(maxFightNum);
	_buf.putInt(curFightNum);
	_buf.putInt(maxObNum);
	_buf.putInt(curObNum);
	_buf.putLong(dungeonId);
	_buf.put(isCanOb?(byte)1:(byte)0);
	_buf.put(isEquit?(byte)1:(byte)0);
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
	builder.Append("roomId").Append(":").Append(roomId.ToString()).Append(", ");
	builder.Append("isNeedPassord").Append(":").Append(isNeedPassord.ToString()).Append(", ");
	builder.Append("minGrade").Append(":").Append(minGrade.ToString()).Append(", ");
	builder.Append("matchType").Append(":").Append(matchType.ToString()).Append(", ");
	builder.Append("roomState").Append(":").Append(roomState.ToString()).Append(", ");
	builder.Append("hostUid").Append(":").Append(hostUid.ToString()).Append(", ");
	builder.Append("hostName").Append(":").Append(hostName.ToString()).Append(", ");
	builder.Append("maxFightNum").Append(":").Append(maxFightNum.ToString()).Append(", ");
	builder.Append("curFightNum").Append(":").Append(curFightNum.ToString()).Append(", ");
	builder.Append("maxObNum").Append(":").Append(maxObNum.ToString()).Append(", ");
	builder.Append("curObNum").Append(":").Append(curObNum.ToString()).Append(", ");
	builder.Append("dungeonId").Append(":").Append(dungeonId.ToString()).Append(", ");
	builder.Append("isCanOb").Append(":").Append(isCanOb.ToString()).Append(", ");
	builder.Append("isEquit").Append(":").Append(isEquit.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

