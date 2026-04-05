using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_GuildChatInfo : ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private string playerName;
private long playerIcon;
private int guildPost;
private long grades;
private int starhoner;
private long legendscore;
private string content;
private int chatTime;
private int playerLevel;
private long playerIconBgk;
private int msgType;
private long id;


public Common_GuildChatInfo() {
	uid = (long)0;
	playerName = "";
	playerIcon = (long)0;
	guildPost = 0;
	grades = (long)0;
	starhoner = 0;
	legendscore = (long)0;
	content = "";
	chatTime = 0;
	playerLevel = 0;
	playerIconBgk = (long)0;
	msgType = 0;
	id = (long)0;
}

public Common_GuildChatInfo(
	long _uid
	, string _playerName
	, long _playerIcon
	, int _guildPost
	, long _grades
	, int _starhoner
	, long _legendscore
	, string _content
	, int _chatTime
	, int _playerLevel
	, long _playerIconBgk
	, int _msgType
	, long _id
) {	uid = _uid;
	playerName = _playerName;
	playerIcon = _playerIcon;
	guildPost = _guildPost;
	grades = _grades;
	starhoner = _starhoner;
	legendscore = _legendscore;
	content = _content;
	chatTime = _chatTime;
	playerLevel = _playerLevel;
	playerIconBgk = _playerIconBgk;
	msgType = _msgType;
	id = _id;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public string getPlayerName() { return playerName; }
public void setPlayerName(string _playerName) { playerName = _playerName; }
public long getPlayerIcon() { return playerIcon; }
public void setPlayerIcon(long _playerIcon) { playerIcon = _playerIcon; }
public int getGuildPost() { return guildPost; }
public void setGuildPost(int _guildPost) { guildPost = _guildPost; }
public long getGrades() { return grades; }
public void setGrades(long _grades) { grades = _grades; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }
public string getContent() { return content; }
public void setContent(string _content) { content = _content; }
public int getChatTime() { return chatTime; }
public void setChatTime(int _chatTime) { chatTime = _chatTime; }
public int getPlayerLevel() { return playerLevel; }
public void setPlayerLevel(int _playerLevel) { playerLevel = _playerLevel; }
public long getPlayerIconBgk() { return playerIconBgk; }
public void setPlayerIconBgk(long _playerIconBgk) { playerIconBgk = _playerIconBgk; }
public int getMsgType() { return msgType; }
public void setMsgType(int _msgType) { msgType = _msgType; }
public long getId() { return id; }
public void setId(long _id) { id = _id; }


public int GetBufSize() {
	int _size = 68;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 70;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerIcon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildPost = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chatTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerIconBgk = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msgType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(uid);
	_buf.putString(playerName);
	_buf.putLong(playerIcon);
	_buf.putInt(guildPost);
	_buf.putLong(grades);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
	_buf.putString(content);
	_buf.putInt(chatTime);
	_buf.putInt(playerLevel);
	_buf.putLong(playerIconBgk);
	_buf.putInt(msgType);
	_buf.putLong(id);
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
	builder.Append("playerIcon").Append(":").Append(playerIcon.ToString()).Append(", ");
	builder.Append("guildPost").Append(":").Append(guildPost.ToString()).Append(", ");
	builder.Append("grades").Append(":").Append(grades.ToString()).Append(", ");
	builder.Append("starhoner").Append(":").Append(starhoner.ToString()).Append(", ");
	builder.Append("legendscore").Append(":").Append(legendscore.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("chatTime").Append(":").Append(chatTime.ToString()).Append(", ");
	builder.Append("playerLevel").Append(":").Append(playerLevel.ToString()).Append(", ");
	builder.Append("playerIconBgk").Append(":").Append(playerIconBgk.ToString()).Append(", ");
	builder.Append("msgType").Append(":").Append(msgType.ToString()).Append(", ");
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

