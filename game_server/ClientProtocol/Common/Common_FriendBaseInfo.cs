using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_FriendBaseInfo : ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private string name;
private long icon;
private int grade;
private int starHonor;
private int legendScore;
private int addTime;
private int level;
private long iconBgk;


public Common_FriendBaseInfo() {
	uid = (long)0;
	name = "";
	icon = (long)0;
	grade = 0;
	starHonor = 0;
	legendScore = 0;
	addTime = 0;
	level = 0;
	iconBgk = (long)0;
}

public Common_FriendBaseInfo(
	long _uid
	, string _name
	, long _icon
	, int _grade
	, int _starHonor
	, int _legendScore
	, int _addTime
	, int _level
	, long _iconBgk
) {	uid = _uid;
	name = _name;
	icon = _icon;
	grade = _grade;
	starHonor = _starHonor;
	legendScore = _legendScore;
	addTime = _addTime;
	level = _level;
	iconBgk = _iconBgk;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public string getName() { return name; }
public void setName(string _name) { name = _name; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public int getGrade() { return grade; }
public void setGrade(int _grade) { grade = _grade; }
public int getStarHonor() { return starHonor; }
public void setStarHonor(int _starHonor) { starHonor = _starHonor; }
public int getLegendScore() { return legendScore; }
public void setLegendScore(int _legendScore) { legendScore = _legendScore; }
public int getAddTime() { return addTime; }
public void setAddTime(int _addTime) { addTime = _addTime; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public long getIconBgk() { return iconBgk; }
public void setIconBgk(long _iconBgk) { iconBgk = _iconBgk; }


public int GetBufSize() {
	int _size = 44;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 46;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	grade = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starHonor = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	legendScore = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconBgk = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(uid);
	_buf.putString(name);
	_buf.putLong(icon);
	_buf.putInt(grade);
	_buf.putInt(starHonor);
	_buf.putInt(legendScore);
	_buf.putInt(addTime);
	_buf.putInt(level);
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
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("icon").Append(":").Append(icon.ToString()).Append(", ");
	builder.Append("grade").Append(":").Append(grade.ToString()).Append(", ");
	builder.Append("starHonor").Append(":").Append(starHonor.ToString()).Append(", ");
	builder.Append("legendScore").Append(":").Append(legendScore.ToString()).Append(", ");
	builder.Append("addTime").Append(":").Append(addTime.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("iconBgk").Append(":").Append(iconBgk.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

