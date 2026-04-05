using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p019_DinnerOp
{

/// <summary>
/// 宴会索引列表
/// </summary>
public class GS2GC_019_006_RetGetDinnerIdxList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宴会索引数据列表
/// </summary>
private List<Common.DinnerObj.Dinner_Idx> idxLIst;
/// <summary>
/// 是否有后续宴会数据
/// </summary>
private bool hasNext;


public GS2GC_019_006_RetGetDinnerIdxList() {
	idxLIst = new List<Common.DinnerObj.Dinner_Idx>();
	hasNext = false;
}

public GS2GC_019_006_RetGetDinnerIdxList(
	List<Common.DinnerObj.Dinner_Idx> _idxLIst
	, bool _hasNext
) {	idxLIst = _idxLIst;
	hasNext = _hasNext;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 宴会索引数据列表
/// </summary>
public List<Common.DinnerObj.Dinner_Idx> getIdxLIst() { return idxLIst; }
/// <summary>
/// 宴会索引数据列表
/// </summary>
public void addIdxLIst(Common.DinnerObj.Dinner_Idx _idxLIst) { idxLIst.Add(_idxLIst); }
/// <summary>
/// 是否有后续宴会数据
/// </summary>
public bool getHasNext() { return hasNext; }
/// <summary>
/// 是否有后续宴会数据
/// </summary>
public void setHasNext(bool _hasNext) { hasNext = _hasNext; }


public int GetBufSize() {
	int _size = 1;
	_size += 2 + (idxLIst.Count * 57);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;
	_size += 2 + (idxLIst.Count * 57);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _idxLIstCount = _buf.getShort();
	for(int _i = 0; _i < _idxLIstCount; _i++) { 
		Common.DinnerObj.Dinner_Idx _idxLIst = new Common.DinnerObj.Dinner_Idx();
		int __idxLIstCustLen = _buf.getInt();
	int __idxLIstCurPos = _buf.getCurPos();
	_idxLIst.ReadUnzipBuf(_buf, __idxLIstCurPos + __idxLIstCustLen);
	_buf.setPosition(__idxLIstCurPos + __idxLIstCustLen);

		idxLIst.Add(_idxLIst);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasNext = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)idxLIst.Count);
	for(int _i = 0; _i < idxLIst.Count; _i++) { 
		_buf.putInt(idxLIst[_i].GetBufSize());
	idxLIst[_i].PutUnzipBuf(_buf);
	}
	_buf.put(hasNext?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)6);
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
	builder.Append("idxLIst").Append(":").Append(idxLIst.ToString()).Append(", ");
	builder.Append("hasNext").Append(":").Append(hasNext.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

