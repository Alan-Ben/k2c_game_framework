using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_FighterInfo : ALBasicProtocolPack._IALProtocolStructure {
private int serialVictory;
private int serialFail;
private int grades;
private int starhoner;
private long legendscore;
private long icon;
private int ladderFightNum;
private int totalVictory;
private int totalDraw;
private int totalFail;
private int victory1v1;
private int victory2v2;
private string clientVersion;
private long iconBgk;


public Common_FighterInfo() {
	serialVictory = 0;
	serialFail = 0;
	grades = 0;
	starhoner = 0;
	legendscore = (long)0;
	icon = (long)0;
	ladderFightNum = 0;
	totalVictory = 0;
	totalDraw = 0;
	totalFail = 0;
	victory1v1 = 0;
	victory2v2 = 0;
	clientVersion = "";
	iconBgk = (long)0;
}

public Common_FighterInfo(
	int _serialVictory
	, int _serialFail
	, int _grades
	, int _starhoner
	, long _legendscore
	, long _icon
	, int _ladderFightNum
	, int _totalVictory
	, int _totalDraw
	, int _totalFail
	, int _victory1v1
	, int _victory2v2
	, string _clientVersion
	, long _iconBgk
) {	serialVictory = _serialVictory;
	serialFail = _serialFail;
	grades = _grades;
	starhoner = _starhoner;
	legendscore = _legendscore;
	icon = _icon;
	ladderFightNum = _ladderFightNum;
	totalVictory = _totalVictory;
	totalDraw = _totalDraw;
	totalFail = _totalFail;
	victory1v1 = _victory1v1;
	victory2v2 = _victory2v2;
	clientVersion = _clientVersion;
	iconBgk = _iconBgk;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getSerialVictory() { return serialVictory; }
public void setSerialVictory(int _serialVictory) { serialVictory = _serialVictory; }
public int getSerialFail() { return serialFail; }
public void setSerialFail(int _serialFail) { serialFail = _serialFail; }
public int getGrades() { return grades; }
public void setGrades(int _grades) { grades = _grades; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public int getLadderFightNum() { return ladderFightNum; }
public void setLadderFightNum(int _ladderFightNum) { ladderFightNum = _ladderFightNum; }
public int getTotalVictory() { return totalVictory; }
public void setTotalVictory(int _totalVictory) { totalVictory = _totalVictory; }
public int getTotalDraw() { return totalDraw; }
public void setTotalDraw(int _totalDraw) { totalDraw = _totalDraw; }
public int getTotalFail() { return totalFail; }
public void setTotalFail(int _totalFail) { totalFail = _totalFail; }
public int getVictory1v1() { return victory1v1; }
public void setVictory1v1(int _victory1v1) { victory1v1 = _victory1v1; }
public int getVictory2v2() { return victory2v2; }
public void setVictory2v2(int _victory2v2) { victory2v2 = _victory2v2; }
public string getClientVersion() { return clientVersion; }
public void setClientVersion(string _clientVersion) { clientVersion = _clientVersion; }
public long getIconBgk() { return iconBgk; }
public void setIconBgk(long _iconBgk) { iconBgk = _iconBgk; }


public int GetBufSize() {
	int _size = 64;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(clientVersion);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 66;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(clientVersion);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serialVictory = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serialFail = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	grades = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	ladderFightNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalVictory = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalDraw = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalFail = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	victory1v1 = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	victory2v2 = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clientVersion = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconBgk = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(serialVictory);
	_buf.putInt(serialFail);
	_buf.putInt(grades);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
	_buf.putLong(icon);
	_buf.putInt(ladderFightNum);
	_buf.putInt(totalVictory);
	_buf.putInt(totalDraw);
	_buf.putInt(totalFail);
	_buf.putInt(victory1v1);
	_buf.putInt(victory2v2);
	_buf.putString(clientVersion);
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
	builder.Append("serialVictory").Append(":").Append(serialVictory.ToString()).Append(", ");
	builder.Append("serialFail").Append(":").Append(serialFail.ToString()).Append(", ");
	builder.Append("grades").Append(":").Append(grades.ToString()).Append(", ");
	builder.Append("starhoner").Append(":").Append(starhoner.ToString()).Append(", ");
	builder.Append("legendscore").Append(":").Append(legendscore.ToString()).Append(", ");
	builder.Append("icon").Append(":").Append(icon.ToString()).Append(", ");
	builder.Append("ladderFightNum").Append(":").Append(ladderFightNum.ToString()).Append(", ");
	builder.Append("totalVictory").Append(":").Append(totalVictory.ToString()).Append(", ");
	builder.Append("totalDraw").Append(":").Append(totalDraw.ToString()).Append(", ");
	builder.Append("totalFail").Append(":").Append(totalFail.ToString()).Append(", ");
	builder.Append("victory1v1").Append(":").Append(victory1v1.ToString()).Append(", ");
	builder.Append("victory2v2").Append(":").Append(victory2v2.ToString()).Append(", ");
	builder.Append("clientVersion").Append(":").Append(clientVersion.ToString()).Append(", ");
	builder.Append("iconBgk").Append(":").Append(iconBgk.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

