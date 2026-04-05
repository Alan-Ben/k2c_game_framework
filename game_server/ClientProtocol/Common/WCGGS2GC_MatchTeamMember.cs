using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_MatchTeamMember : ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private string cname;
private long grades;
private int starhoner;
private long legendscore;
private bool isReady;
private Common.Common_Lineup lineup;
private long icon;
private int level;
private Common.Common_VoiceSetting voiceSetting;
private bool isOnline;
private long iconBgk;


public WCGGS2GC_MatchTeamMember() {
	uid = (long)0;
	cname = "";
	grades = (long)0;
	starhoner = 0;
	legendscore = (long)0;
	isReady = false;
	lineup = new Common.Common_Lineup();
	icon = (long)0;
	level = 0;
	voiceSetting = new Common.Common_VoiceSetting();
	isOnline = false;
	iconBgk = (long)0;
}

public WCGGS2GC_MatchTeamMember(
	long _uid
	, string _cname
	, long _grades
	, int _starhoner
	, long _legendscore
	, bool _isReady
	, Common.Common_Lineup _lineup
	, long _icon
	, int _level
	, Common.Common_VoiceSetting _voiceSetting
	, bool _isOnline
	, long _iconBgk
) {	uid = _uid;
	cname = _cname;
	grades = _grades;
	starhoner = _starhoner;
	legendscore = _legendscore;
	isReady = _isReady;
	lineup = _lineup;
	icon = _icon;
	level = _level;
	voiceSetting = _voiceSetting;
	isOnline = _isOnline;
	iconBgk = _iconBgk;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public string getCname() { return cname; }
public void setCname(string _cname) { cname = _cname; }
public long getGrades() { return grades; }
public void setGrades(long _grades) { grades = _grades; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }
public bool getIsReady() { return isReady; }
public void setIsReady(bool _isReady) { isReady = _isReady; }
public Common.Common_Lineup getLineup() { return lineup; }
public void setLineup(Common.Common_Lineup _lineup) { lineup = _lineup; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public Common.Common_VoiceSetting getVoiceSetting() { return voiceSetting; }
public void setVoiceSetting(Common.Common_VoiceSetting _voiceSetting) { voiceSetting = _voiceSetting; }
public bool getIsOnline() { return isOnline; }
public void setIsOnline(bool _isOnline) { isOnline = _isOnline; }
public long getIconBgk() { return iconBgk; }
public void setIconBgk(long _iconBgk) { iconBgk = _iconBgk; }


public int GetBufSize() {
	int _size = 56;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);
	_size += 4 + lineup.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 58;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);
	_size += 4 + lineup.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cname = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isReady = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _lineupCustLen = _buf.getInt();
	int _lineupCurPos = _buf.getCurPos();
	lineup.ReadUnzipBuf(_buf, _lineupCurPos + _lineupCustLen);
	_buf.setPosition(_lineupCurPos + _lineupCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _voiceSettingCustLen = _buf.getInt();
	int _voiceSettingCurPos = _buf.getCurPos();
	voiceSetting.ReadUnzipBuf(_buf, _voiceSettingCurPos + _voiceSettingCustLen);
	_buf.setPosition(_voiceSettingCurPos + _voiceSettingCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isOnline = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconBgk = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(uid);
	_buf.putString(cname);
	_buf.putLong(grades);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
	_buf.put(isReady?(byte)1:(byte)0);
	_buf.putInt(lineup.GetBufSize());
	lineup.PutUnzipBuf(_buf);
	_buf.putLong(icon);
	_buf.putInt(level);
	_buf.putInt(voiceSetting.GetBufSize());
	voiceSetting.PutUnzipBuf(_buf);
	_buf.put(isOnline?(byte)1:(byte)0);
	_buf.putLong(iconBgk);
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
	builder.Append("cname").Append(":").Append(cname.ToString()).Append(", ");
	builder.Append("grades").Append(":").Append(grades.ToString()).Append(", ");
	builder.Append("starhoner").Append(":").Append(starhoner.ToString()).Append(", ");
	builder.Append("legendscore").Append(":").Append(legendscore.ToString()).Append(", ");
	builder.Append("isReady").Append(":").Append(isReady.ToString()).Append(", ");
	builder.Append("lineup").Append(":").Append(lineup == null ? "null" : lineup.ToString()).Append(", ");
	builder.Append("icon").Append(":").Append(icon.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("voiceSetting").Append(":").Append(voiceSetting == null ? "null" : voiceSetting.ToString()).Append(", ");
	builder.Append("isOnline").Append(":").Append(isOnline.ToString()).Append(", ");
	builder.Append("iconBgk").Append(":").Append(iconBgk.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

