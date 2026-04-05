using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p037_GuildDungeonOp
{

public class GS2GC_037_010_RetGetDungeonLogList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 副本日志数据
/// </summary>
private List<Common.GuildDungeonObj.GuildDungeon_Log> logList;


public GS2GC_037_010_RetGetDungeonLogList() {
	logList = new List<Common.GuildDungeonObj.GuildDungeon_Log>();
}

public GS2GC_037_010_RetGetDungeonLogList(
	List<Common.GuildDungeonObj.GuildDungeon_Log> _logList
) {	logList = _logList;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)10; }

/// <summary>
/// 副本日志数据
/// </summary>
public List<Common.GuildDungeonObj.GuildDungeon_Log> getLogList() { return logList; }
/// <summary>
/// 副本日志数据
/// </summary>
public void addLogList(Common.GuildDungeonObj.GuildDungeon_Log _logList) { logList.Add(_logList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < logList.Count; _i++) {
	_size += 4 + logList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < logList.Count; _i++) {
	_size += 4 + logList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _logListCount = _buf.getShort();
	for(int _i = 0; _i < _logListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_Log _logList = new Common.GuildDungeonObj.GuildDungeon_Log();
		int __logListCustLen = _buf.getInt();
	int __logListCurPos = _buf.getCurPos();
	_logList.ReadUnzipBuf(_buf, __logListCurPos + __logListCustLen);
	_buf.setPosition(__logListCurPos + __logListCustLen);

		logList.Add(_logList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)logList.Count);
	for(int _i = 0; _i < logList.Count; _i++) { 
		_buf.putInt(logList[_i].GetBufSize());
	logList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)10);
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
	builder.Append("logList").Append(":").Append(logList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

