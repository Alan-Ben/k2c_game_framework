using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p024_DungeonOp
{

public class GS2GC_024_005_RetMiddayDungeonBoxCanDraw : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否可领取
/// </summary>
private bool canDraw;
/// <summary>
/// 剩余可领取次数
/// </summary>
private int remainDrawCount;


public GS2GC_024_005_RetMiddayDungeonBoxCanDraw() {
	canDraw = false;
	remainDrawCount = 0;
}

public GS2GC_024_005_RetMiddayDungeonBoxCanDraw(
	bool _canDraw
	, int _remainDrawCount
) {	canDraw = _canDraw;
	remainDrawCount = _remainDrawCount;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 是否可领取
/// </summary>
public bool getCanDraw() { return canDraw; }
/// <summary>
/// 是否可领取
/// </summary>
public void setCanDraw(bool _canDraw) { canDraw = _canDraw; }
/// <summary>
/// 剩余可领取次数
/// </summary>
public int getRemainDrawCount() { return remainDrawCount; }
/// <summary>
/// 剩余可领取次数
/// </summary>
public void setRemainDrawCount(int _remainDrawCount) { remainDrawCount = _remainDrawCount; }


public int GetBufSize() {
	int _size = 5;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 7;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	canDraw = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	remainDrawCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(canDraw?(byte)1:(byte)0);
	_buf.putInt(remainDrawCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)5);
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
	builder.Append("canDraw").Append(":").Append(canDraw.ToString()).Append(", ");
	builder.Append("remainDrawCount").Append(":").Append(remainDrawCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

