package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_MatchTeamMember implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private String cname;
private long grades;
private int starhoner;
private long legendscore;
private boolean isReady;
private Common.Common_Lineup lineup;
private long icon;
private int level;
private Common.Common_VoiceSetting voiceSetting;
private boolean isOnline;
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
	, String _cname
	, long _grades
	, int _starhoner
	, long _legendscore
	, boolean _isReady
	, Common.Common_Lineup _lineup
	, long _icon
	, int _level
	, Common.Common_VoiceSetting _voiceSetting
	, boolean _isOnline
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public String getCname() { return cname; }
public void setCname(String _cname) { cname = _cname; }
public long getGrades() { return grades; }
public void setGrades(long _grades) { grades = _grades; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }
public boolean getIsReady() { return isReady; }
public void setIsReady(boolean _isReady) { isReady = _isReady; }
public Common.Common_Lineup getLineup() { return lineup; }
public void setLineup(Common.Common_Lineup _lineup) { lineup = _lineup; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public Common.Common_VoiceSetting getVoiceSetting() { return voiceSetting; }
public void setVoiceSetting(Common.Common_VoiceSetting _voiceSetting) { voiceSetting = _voiceSetting; }
public boolean getIsOnline() { return isOnline; }
public void setIsOnline(boolean _isOnline) { isOnline = _isOnline; }
public long getIconBgk() { return iconBgk; }
public void setIconBgk(long _iconBgk) { iconBgk = _iconBgk; }


public final int GetBufSize() {
	int _size = 56;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);
	_size += 4 + lineup.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 58;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);
	_size += 4 + lineup.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cname = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isReady = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _lineupCustLen = _buf.getInt();
	int _lineupCurPos = _buf.position();
	lineup.ReadUnzipBuf(_buf, _lineupCurPos + _lineupCustLen);
	_buf.position(_lineupCurPos + _lineupCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _voiceSettingCustLen = _buf.getInt();
	int _voiceSettingCurPos = _buf.position();
	voiceSetting.ReadUnzipBuf(_buf, _voiceSettingCurPos + _voiceSettingCustLen);
	_buf.position(_voiceSettingCurPos + _voiceSettingCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOnline = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconBgk = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, cname);
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

