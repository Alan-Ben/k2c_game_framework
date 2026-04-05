using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

public class GS2GC_042_002_RetDealMarsHelp : ALBasicProtocolPack._IALProtocolStructure {
private int dealCount;
private long rewardCount;


public GS2GC_042_002_RetDealMarsHelp() {
	dealCount = 0;
	rewardCount = (long)0;
}

public GS2GC_042_002_RetDealMarsHelp(
	int _dealCount
	, long _rewardCount
) {	dealCount = _dealCount;
	rewardCount = _rewardCount;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)2; }

public int getDealCount() { return dealCount; }
public void setDealCount(int _dealCount) { dealCount = _dealCount; }
public long getRewardCount() { return rewardCount; }
public void setRewardCount(long _rewardCount) { rewardCount = _rewardCount; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rewardCount = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(dealCount);
	_buf.putLong(rewardCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)2);
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
	builder.Append("dealCount").Append(":").Append(dealCount.ToString()).Append(", ");
	builder.Append("rewardCount").Append(":").Append(rewardCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

