using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class MCC_Common_User_Extra : ALBasicProtocolPack._IALProtocolStructure {
private string name;
private long icon;
private long iconFrame;
private long lvl;
private long grade;
private int starhoner;
private long legendscore;


public MCC_Common_User_Extra() {
	name = "";
	icon = (long)0;
	iconFrame = (long)0;
	lvl = (long)0;
	grade = (long)0;
	starhoner = 0;
	legendscore = (long)0;
}

public MCC_Common_User_Extra(
	string _name
	, long _icon
	, long _iconFrame
	, long _lvl
	, long _grade
	, int _starhoner
	, long _legendscore
) {	name = _name;
	icon = _icon;
	iconFrame = _iconFrame;
	lvl = _lvl;
	grade = _grade;
	starhoner = _starhoner;
	legendscore = _legendscore;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public string getName() { return name; }
public void setName(string _name) { name = _name; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public long getIconFrame() { return iconFrame; }
public void setIconFrame(long _iconFrame) { iconFrame = _iconFrame; }
public long getLvl() { return lvl; }
public void setLvl(long _lvl) { lvl = _lvl; }
public long getGrade() { return grade; }
public void setGrade(long _grade) { grade = _grade; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }


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
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconFrame = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lvl = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	grade = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	legendscore = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(name);
	_buf.putLong(icon);
	_buf.putLong(iconFrame);
	_buf.putLong(lvl);
	_buf.putLong(grade);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
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
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("icon").Append(":").Append(icon.ToString()).Append(", ");
	builder.Append("iconFrame").Append(":").Append(iconFrame.ToString()).Append(", ");
	builder.Append("lvl").Append(":").Append(lvl.ToString()).Append(", ");
	builder.Append("grade").Append(":").Append(grade.ToString()).Append(", ");
	builder.Append("starhoner").Append(":").Append(starhoner.ToString()).Append(", ");
	builder.Append("legendscore").Append(":").Append(legendscore.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

