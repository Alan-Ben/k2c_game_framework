using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p014_ChildOp
{

/// <summary>
/// 增加训练位脑力值
/// </summary>
public class GC2GS_014_003_ReqAddSeatEnergy : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 训练位ID
/// </summary>
private long seatId;
/// <summary>
/// 增加数量
/// </summary>
private int addCount;


public GC2GS_014_003_ReqAddSeatEnergy() {
	seatId = (long)0;
	addCount = 0;
}

public GC2GS_014_003_ReqAddSeatEnergy(
	long _seatId
	, int _addCount
) {	seatId = _seatId;
	addCount = _addCount;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 训练位ID
/// </summary>
public long getSeatId() { return seatId; }
/// <summary>
/// 训练位ID
/// </summary>
public void setSeatId(long _seatId) { seatId = _seatId; }
/// <summary>
/// 增加数量
/// </summary>
public int getAddCount() { return addCount; }
/// <summary>
/// 增加数量
/// </summary>
public void setAddCount(int _addCount) { addCount = _addCount; }


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
	seatId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(seatId);
	_buf.putInt(addCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)3);
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
	builder.Append("seatId").Append(":").Append(seatId.ToString()).Append(", ");
	builder.Append("addCount").Append(":").Append(addCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

