using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-矿石数量信息
/// </summary>
public class TreasureHunt_OreNumInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已获得数量
/// </summary>
private int totalGainNum;
/// <summary>
/// 普通矿石待处理数量
/// </summary>
private int normalPendingNum;
/// <summary>
/// 高级矿石待处理数量
/// </summary>
private int advancedPendingNum;


public TreasureHunt_OreNumInfo() {
	totalGainNum = 0;
	normalPendingNum = 0;
	advancedPendingNum = 0;
}

public TreasureHunt_OreNumInfo(
	int _totalGainNum
	, int _normalPendingNum
	, int _advancedPendingNum
) {	totalGainNum = _totalGainNum;
	normalPendingNum = _normalPendingNum;
	advancedPendingNum = _advancedPendingNum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 已获得数量
/// </summary>
public int getTotalGainNum() { return totalGainNum; }
/// <summary>
/// 已获得数量
/// </summary>
public void setTotalGainNum(int _totalGainNum) { totalGainNum = _totalGainNum; }
/// <summary>
/// 普通矿石待处理数量
/// </summary>
public int getNormalPendingNum() { return normalPendingNum; }
/// <summary>
/// 普通矿石待处理数量
/// </summary>
public void setNormalPendingNum(int _normalPendingNum) { normalPendingNum = _normalPendingNum; }
/// <summary>
/// 高级矿石待处理数量
/// </summary>
public int getAdvancedPendingNum() { return advancedPendingNum; }
/// <summary>
/// 高级矿石待处理数量
/// </summary>
public void setAdvancedPendingNum(int _advancedPendingNum) { advancedPendingNum = _advancedPendingNum; }


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
	totalGainNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	normalPendingNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	advancedPendingNum = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(totalGainNum);
	_buf.putInt(normalPendingNum);
	_buf.putInt(advancedPendingNum);
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
	builder.Append("totalGainNum").Append(":").Append(totalGainNum.ToString()).Append(", ");
	builder.Append("normalPendingNum").Append(":").Append(normalPendingNum.ToString()).Append(", ");
	builder.Append("advancedPendingNum").Append(":").Append(advancedPendingNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

