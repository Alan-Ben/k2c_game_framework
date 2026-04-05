using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_UserLogInfo : ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private string name;
private long gem;
private long cardCoin;
private long silver;
private long recharge;
private int level;
private int lastLoginTime;
private int lastLoginDate;
private bool isBanded;


public Common_UserLogInfo() {
	cid = (long)0;
	name = "";
	gem = (long)0;
	cardCoin = (long)0;
	silver = (long)0;
	recharge = (long)0;
	level = 0;
	lastLoginTime = 0;
	lastLoginDate = 0;
	isBanded = false;
}

public Common_UserLogInfo(
	long _cid
	, string _name
	, long _gem
	, long _cardCoin
	, long _silver
	, long _recharge
	, int _level
	, int _lastLoginTime
	, int _lastLoginDate
	, bool _isBanded
) {	cid = _cid;
	name = _name;
	gem = _gem;
	cardCoin = _cardCoin;
	silver = _silver;
	recharge = _recharge;
	level = _level;
	lastLoginTime = _lastLoginTime;
	lastLoginDate = _lastLoginDate;
	isBanded = _isBanded;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public string getName() { return name; }
public void setName(string _name) { name = _name; }
public long getGem() { return gem; }
public void setGem(long _gem) { gem = _gem; }
public long getCardCoin() { return cardCoin; }
public void setCardCoin(long _cardCoin) { cardCoin = _cardCoin; }
public long getSilver() { return silver; }
public void setSilver(long _silver) { silver = _silver; }
public long getRecharge() { return recharge; }
public void setRecharge(long _recharge) { recharge = _recharge; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public int getLastLoginTime() { return lastLoginTime; }
public void setLastLoginTime(int _lastLoginTime) { lastLoginTime = _lastLoginTime; }
public int getLastLoginDate() { return lastLoginDate; }
public void setLastLoginDate(int _lastLoginDate) { lastLoginDate = _lastLoginDate; }
public bool getIsBanded() { return isBanded; }
public void setIsBanded(bool _isBanded) { isBanded = _isBanded; }


public int GetBufSize() {
	int _size = 53;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 55;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gem = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cardCoin = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	silver = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	recharge = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastLoginTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastLoginDate = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isBanded = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putString(name);
	_buf.putLong(gem);
	_buf.putLong(cardCoin);
	_buf.putLong(silver);
	_buf.putLong(recharge);
	_buf.putInt(level);
	_buf.putInt(lastLoginTime);
	_buf.putInt(lastLoginDate);
	_buf.put(isBanded?(byte)1:(byte)0);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("gem").Append(":").Append(gem.ToString()).Append(", ");
	builder.Append("cardCoin").Append(":").Append(cardCoin.ToString()).Append(", ");
	builder.Append("silver").Append(":").Append(silver.ToString()).Append(", ");
	builder.Append("recharge").Append(":").Append(recharge.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("lastLoginTime").Append(":").Append(lastLoginTime.ToString()).Append(", ");
	builder.Append("lastLoginDate").Append(":").Append(lastLoginDate.ToString()).Append(", ");
	builder.Append("isBanded").Append(":").Append(isBanded.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

