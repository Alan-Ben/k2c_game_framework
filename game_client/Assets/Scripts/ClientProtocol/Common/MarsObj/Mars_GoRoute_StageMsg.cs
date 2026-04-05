using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 前往火星-阶段留言
/// </summary>
public class Mars_GoRoute_StageMsg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家名称
/// </summary>
private string playerName;
/// <summary>
/// 留言内容
/// </summary>
private string content;
/// <summary>
/// 留言时间（毫秒）
/// </summary>
private long timeMs;


public Mars_GoRoute_StageMsg() {
	playerName = "";
	content = "";
	timeMs = (long)0;
}

public Mars_GoRoute_StageMsg(
	string _playerName
	, string _content
	, long _timeMs
) {	playerName = _playerName;
	content = _content;
	timeMs = _timeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 玩家名称
/// </summary>
public string getPlayerName() { return playerName; }
/// <summary>
/// 玩家名称
/// </summary>
public void setPlayerName(string _playerName) { playerName = _playerName; }
/// <summary>
/// 留言内容
/// </summary>
public string getContent() { return content; }
/// <summary>
/// 留言内容
/// </summary>
public void setContent(string _content) { content = _content; }
/// <summary>
/// 留言时间（毫秒）
/// </summary>
public long getTimeMs() { return timeMs; }
/// <summary>
/// 留言时间（毫秒）
/// </summary>
public void setTimeMs(long _timeMs) { timeMs = _timeMs; }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(playerName);
	_buf.putString(content);
	_buf.putLong(timeMs);
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
	builder.Append("playerName").Append(":").Append(playerName.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("timeMs").Append(":").Append(timeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

