using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_RechargedLimitGood : ALBasicProtocolPack._IALProtocolStructure {
private long goodId;
private int rechargedNum;
private int limitNum;
private int restRemainSec;


public WCGGS2GC_RechargedLimitGood() {
	goodId = (long)0;
	rechargedNum = 0;
	limitNum = 0;
	restRemainSec = 0;
}

public WCGGS2GC_RechargedLimitGood(
	long _goodId
	, int _rechargedNum
	, int _limitNum
	, int _restRemainSec
) {	goodId = _goodId;
	rechargedNum = _rechargedNum;
	limitNum = _limitNum;
	restRemainSec = _restRemainSec;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getGoodId() { return goodId; }
public void setGoodId(long _goodId) { goodId = _goodId; }
public int getRechargedNum() { return rechargedNum; }
public void setRechargedNum(int _rechargedNum) { rechargedNum = _rechargedNum; }
public int getLimitNum() { return limitNum; }
public void setLimitNum(int _limitNum) { limitNum = _limitNum; }
public int getRestRemainSec() { return restRemainSec; }
public void setRestRemainSec(int _restRemainSec) { restRemainSec = _restRemainSec; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	goodId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rechargedNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	limitNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	restRemainSec = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(goodId);
	_buf.putInt(rechargedNum);
	_buf.putInt(limitNum);
	_buf.putInt(restRemainSec);
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
	builder.Append("goodId").Append(":").Append(goodId.ToString()).Append(", ");
	builder.Append("rechargedNum").Append(":").Append(rechargedNum.ToString()).Append(", ");
	builder.Append("limitNum").Append(":").Append(limitNum.ToString()).Append(", ");
	builder.Append("restRemainSec").Append(":").Append(restRemainSec.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

