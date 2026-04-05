package Common;

import java.nio.ByteBuffer;
public class Common_FighterInfo implements ALBasicProtocolPack._IALProtocolStructure {
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
private String clientVersion;
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
	, String _clientVersion
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

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
public String getClientVersion() { return clientVersion; }
public void setClientVersion(String _clientVersion) { clientVersion = _clientVersion; }
public long getIconBgk() { return iconBgk; }
public void setIconBgk(long _iconBgk) { iconBgk = _iconBgk; }


public final int GetBufSize() {
	int _size = 64;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(clientVersion);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 66;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(clientVersion);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serialVictory = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serialFail = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) grades = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) ladderFightNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalVictory = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalDraw = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalFail = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) victory1v1 = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) victory2v2 = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientVersion = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconBgk = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
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
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, clientVersion);
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

