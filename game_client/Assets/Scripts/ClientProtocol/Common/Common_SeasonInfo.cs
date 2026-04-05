using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_SeasonInfo : ALBasicProtocolPack._IALProtocolStructure {
private int lastSeasonDesc;
private int curSeason;
private int seasonStartTime;
private int seasonEndTime;


public Common_SeasonInfo() {
	lastSeasonDesc = 0;
	curSeason = 0;
	seasonStartTime = 0;
	seasonEndTime = 0;
}

public Common_SeasonInfo(
	int _lastSeasonDesc
	, int _curSeason
	, int _seasonStartTime
	, int _seasonEndTime
) {	lastSeasonDesc = _lastSeasonDesc;
	curSeason = _curSeason;
	seasonStartTime = _seasonStartTime;
	seasonEndTime = _seasonEndTime;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getLastSeasonDesc() { return lastSeasonDesc; }
public void setLastSeasonDesc(int _lastSeasonDesc) { lastSeasonDesc = _lastSeasonDesc; }
public int getCurSeason() { return curSeason; }
public void setCurSeason(int _curSeason) { curSeason = _curSeason; }
public int getSeasonStartTime() { return seasonStartTime; }
public void setSeasonStartTime(int _seasonStartTime) { seasonStartTime = _seasonStartTime; }
public int getSeasonEndTime() { return seasonEndTime; }
public void setSeasonEndTime(int _seasonEndTime) { seasonEndTime = _seasonEndTime; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastSeasonDesc = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curSeason = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	seasonStartTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	seasonEndTime = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(lastSeasonDesc);
	_buf.putInt(curSeason);
	_buf.putInt(seasonStartTime);
	_buf.putInt(seasonEndTime);
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
	builder.Append("lastSeasonDesc").Append(":").Append(lastSeasonDesc.ToString()).Append(", ");
	builder.Append("curSeason").Append(":").Append(curSeason.ToString()).Append(", ");
	builder.Append("seasonStartTime").Append(":").Append(seasonStartTime.ToString()).Append(", ");
	builder.Append("seasonEndTime").Append(":").Append(seasonEndTime.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

