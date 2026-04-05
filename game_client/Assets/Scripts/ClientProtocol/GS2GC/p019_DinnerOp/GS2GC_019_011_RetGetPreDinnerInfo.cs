using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p019_DinnerOp
{

/// <summary>
/// 取指定宴会的上一条宴会数据
/// </summary>
public class GS2GC_019_011_RetGetPreDinnerInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宴会详情
/// </summary>
private Common.DinnerObj.Dinner_Info info;
/// <summary>
/// 当前排序
/// </summary>
private int idx;
/// <summary>
/// 是否有上一条宴会数据 true-有
/// </summary>
private bool hasPre;
/// <summary>
/// 是否有下一条宴会数据 true-有
/// </summary>
private bool hasNext;


public GS2GC_019_011_RetGetPreDinnerInfo() {
	info = new Common.DinnerObj.Dinner_Info();
	idx = 0;
	hasPre = false;
	hasNext = false;
}

public GS2GC_019_011_RetGetPreDinnerInfo(
	Common.DinnerObj.Dinner_Info _info
	, int _idx
	, bool _hasPre
	, bool _hasNext
) {	info = _info;
	idx = _idx;
	hasPre = _hasPre;
	hasNext = _hasNext;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)11; }

/// <summary>
/// 宴会详情
/// </summary>
public Common.DinnerObj.Dinner_Info getInfo() { return info; }
/// <summary>
/// 宴会详情
/// </summary>
public void setInfo(Common.DinnerObj.Dinner_Info _info) { info = _info; }
/// <summary>
/// 当前排序
/// </summary>
public int getIdx() { return idx; }
/// <summary>
/// 当前排序
/// </summary>
public void setIdx(int _idx) { idx = _idx; }
/// <summary>
/// 是否有上一条宴会数据 true-有
/// </summary>
public bool getHasPre() { return hasPre; }
/// <summary>
/// 是否有上一条宴会数据 true-有
/// </summary>
public void setHasPre(bool _hasPre) { hasPre = _hasPre; }
/// <summary>
/// 是否有下一条宴会数据 true-有
/// </summary>
public bool getHasNext() { return hasNext; }
/// <summary>
/// 是否有下一条宴会数据 true-有
/// </summary>
public void setHasNext(bool _hasNext) { hasNext = _hasNext; }


public int GetBufSize() {
	int _size = 6;
	_size += 4 + info.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 8;
	_size += 4 + info.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.getCurPos();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.setPosition(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	idx = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasPre = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasNext = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putInt(idx);
	_buf.put(hasPre?(byte)1:(byte)0);
	_buf.put(hasNext?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)11);
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
	builder.Append("info").Append(":").Append(info == null ? "null" : info.ToString()).Append(", ");
	builder.Append("idx").Append(":").Append(idx.ToString()).Append(", ");
	builder.Append("hasPre").Append(":").Append(hasPre.ToString()).Append(", ");
	builder.Append("hasNext").Append(":").Append(hasNext.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

