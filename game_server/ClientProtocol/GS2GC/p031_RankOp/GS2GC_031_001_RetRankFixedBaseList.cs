using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p031_RankOp
{

public class GS2GC_031_001_RetRankFixedBaseList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基础数据列表
/// </summary>
private List<Common.RankObj.Rank_BaseItem> baseItemlist;


public GS2GC_031_001_RetRankFixedBaseList() {
	baseItemlist = new List<Common.RankObj.Rank_BaseItem>();
}

public GS2GC_031_001_RetRankFixedBaseList(
	List<Common.RankObj.Rank_BaseItem> _baseItemlist
) {	baseItemlist = _baseItemlist;
}

public byte getMainOrder() { return (byte)31; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 基础数据列表
/// </summary>
public List<Common.RankObj.Rank_BaseItem> getBaseItemlist() { return baseItemlist; }
/// <summary>
/// 基础数据列表
/// </summary>
public void addBaseItemlist(Common.RankObj.Rank_BaseItem _baseItemlist) { baseItemlist.Add(_baseItemlist); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (baseItemlist.Count * 32);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (baseItemlist.Count * 32);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _baseItemlistCount = _buf.getShort();
	for(int _i = 0; _i < _baseItemlistCount; _i++) { 
		Common.RankObj.Rank_BaseItem _baseItemlist = new Common.RankObj.Rank_BaseItem();
		int __baseItemlistCustLen = _buf.getInt();
	int __baseItemlistCurPos = _buf.getCurPos();
	_baseItemlist.ReadUnzipBuf(_buf, __baseItemlistCurPos + __baseItemlistCustLen);
	_buf.setPosition(__baseItemlistCurPos + __baseItemlistCustLen);

		baseItemlist.Add(_baseItemlist);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)baseItemlist.Count);
	for(int _i = 0; _i < baseItemlist.Count; _i++) { 
		_buf.putInt(baseItemlist[_i].GetBufSize());
	baseItemlist[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)31);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)31);
	_recBuf.put((byte)1);
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
	builder.Append("baseItemlist").Append(":").Append(baseItemlist.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

