using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_PlayerCardInfo : ALBasicProtocolPack._IALProtocolStructure {
private long cardId;
private bool isUnLocked;
private int chipNum;
private int canUseNum;
private int dailyBroughtNum;
private int level;
private int upRemainSec;


public WCGGS2GC_PlayerCardInfo() {
	cardId = (long)0;
	isUnLocked = false;
	chipNum = 0;
	canUseNum = 0;
	dailyBroughtNum = 0;
	level = 0;
	upRemainSec = 0;
}

public WCGGS2GC_PlayerCardInfo(
	long _cardId
	, bool _isUnLocked
	, int _chipNum
	, int _canUseNum
	, int _dailyBroughtNum
	, int _level
	, int _upRemainSec
) {	cardId = _cardId;
	isUnLocked = _isUnLocked;
	chipNum = _chipNum;
	canUseNum = _canUseNum;
	dailyBroughtNum = _dailyBroughtNum;
	level = _level;
	upRemainSec = _upRemainSec;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getCardId() { return cardId; }
public void setCardId(long _cardId) { cardId = _cardId; }
public bool getIsUnLocked() { return isUnLocked; }
public void setIsUnLocked(bool _isUnLocked) { isUnLocked = _isUnLocked; }
public int getChipNum() { return chipNum; }
public void setChipNum(int _chipNum) { chipNum = _chipNum; }
public int getCanUseNum() { return canUseNum; }
public void setCanUseNum(int _canUseNum) { canUseNum = _canUseNum; }
public int getDailyBroughtNum() { return dailyBroughtNum; }
public void setDailyBroughtNum(int _dailyBroughtNum) { dailyBroughtNum = _dailyBroughtNum; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public int getUpRemainSec() { return upRemainSec; }
public void setUpRemainSec(int _upRemainSec) { upRemainSec = _upRemainSec; }


public int GetBufSize() {
	int _size = 29;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 31;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isUnLocked = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chipNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	canUseNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dailyBroughtNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	upRemainSec = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cardId);
	_buf.put(isUnLocked?(byte)1:(byte)0);
	_buf.putInt(chipNum);
	_buf.putInt(canUseNum);
	_buf.putInt(dailyBroughtNum);
	_buf.putInt(level);
	_buf.putInt(upRemainSec);
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
	builder.Append("cardId").Append(":").Append(cardId.ToString()).Append(", ");
	builder.Append("isUnLocked").Append(":").Append(isUnLocked.ToString()).Append(", ");
	builder.Append("chipNum").Append(":").Append(chipNum.ToString()).Append(", ");
	builder.Append("canUseNum").Append(":").Append(canUseNum.ToString()).Append(", ");
	builder.Append("dailyBroughtNum").Append(":").Append(dailyBroughtNum.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("upRemainSec").Append(":").Append(upRemainSec.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

