using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_EquitableArenaInfo : ALBasicProtocolPack._IALProtocolStructure {
private int arenatype;
private int freeTimes;
private bool isChallenge;
private int victory;
private int failed;
private bool hasGotBalanceBox;
private int maxVictory;
private int remainSec;


public Common_EquitableArenaInfo() {
	arenatype = 0;
	freeTimes = 0;
	isChallenge = false;
	victory = 0;
	failed = 0;
	hasGotBalanceBox = false;
	maxVictory = 0;
	remainSec = 0;
}

public Common_EquitableArenaInfo(
	int _arenatype
	, int _freeTimes
	, bool _isChallenge
	, int _victory
	, int _failed
	, bool _hasGotBalanceBox
	, int _maxVictory
	, int _remainSec
) {	arenatype = _arenatype;
	freeTimes = _freeTimes;
	isChallenge = _isChallenge;
	victory = _victory;
	failed = _failed;
	hasGotBalanceBox = _hasGotBalanceBox;
	maxVictory = _maxVictory;
	remainSec = _remainSec;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getArenatype() { return arenatype; }
public void setArenatype(int _arenatype) { arenatype = _arenatype; }
public int getFreeTimes() { return freeTimes; }
public void setFreeTimes(int _freeTimes) { freeTimes = _freeTimes; }
public bool getIsChallenge() { return isChallenge; }
public void setIsChallenge(bool _isChallenge) { isChallenge = _isChallenge; }
public int getVictory() { return victory; }
public void setVictory(int _victory) { victory = _victory; }
public int getFailed() { return failed; }
public void setFailed(int _failed) { failed = _failed; }
public bool getHasGotBalanceBox() { return hasGotBalanceBox; }
public void setHasGotBalanceBox(bool _hasGotBalanceBox) { hasGotBalanceBox = _hasGotBalanceBox; }
public int getMaxVictory() { return maxVictory; }
public void setMaxVictory(int _maxVictory) { maxVictory = _maxVictory; }
public int getRemainSec() { return remainSec; }
public void setRemainSec(int _remainSec) { remainSec = _remainSec; }


public int GetBufSize() {
	int _size = 26;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 28;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	arenatype = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	freeTimes = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isChallenge = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	victory = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	failed = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasGotBalanceBox = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxVictory = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	remainSec = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(arenatype);
	_buf.putInt(freeTimes);
	_buf.put(isChallenge?(byte)1:(byte)0);
	_buf.putInt(victory);
	_buf.putInt(failed);
	_buf.put(hasGotBalanceBox?(byte)1:(byte)0);
	_buf.putInt(maxVictory);
	_buf.putInt(remainSec);
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
	builder.Append("arenatype").Append(":").Append(arenatype.ToString()).Append(", ");
	builder.Append("freeTimes").Append(":").Append(freeTimes.ToString()).Append(", ");
	builder.Append("isChallenge").Append(":").Append(isChallenge.ToString()).Append(", ");
	builder.Append("victory").Append(":").Append(victory.ToString()).Append(", ");
	builder.Append("failed").Append(":").Append(failed.ToString()).Append(", ");
	builder.Append("hasGotBalanceBox").Append(":").Append(hasGotBalanceBox.ToString()).Append(", ");
	builder.Append("maxVictory").Append(":").Append(maxVictory.ToString()).Append(", ");
	builder.Append("remainSec").Append(":").Append(remainSec.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

