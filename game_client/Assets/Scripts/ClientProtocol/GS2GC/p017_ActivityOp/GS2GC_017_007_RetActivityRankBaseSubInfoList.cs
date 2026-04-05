using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

public class GS2GC_017_007_RetActivityRankBaseSubInfoList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基础子数据列表
/// </summary>
private List<Common.RankObj.Rank_BaseSubItem> subList;


public GS2GC_017_007_RetActivityRankBaseSubInfoList() {
	subList = new List<Common.RankObj.Rank_BaseSubItem>();
}

public GS2GC_017_007_RetActivityRankBaseSubInfoList(
	List<Common.RankObj.Rank_BaseSubItem> _subList
) {	subList = _subList;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)7; }

/// <summary>
/// 基础子数据列表
/// </summary>
public List<Common.RankObj.Rank_BaseSubItem> getSubList() { return subList; }
/// <summary>
/// 基础子数据列表
/// </summary>
public void addSubList(Common.RankObj.Rank_BaseSubItem _subList) { subList.Add(_subList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (subList.Count * 28);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (subList.Count * 28);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _subListCount = _buf.getShort();
	for(int _i = 0; _i < _subListCount; _i++) { 
		Common.RankObj.Rank_BaseSubItem _subList = new Common.RankObj.Rank_BaseSubItem();
		int __subListCustLen = _buf.getInt();
	int __subListCurPos = _buf.getCurPos();
	_subList.ReadUnzipBuf(_buf, __subListCurPos + __subListCustLen);
	_buf.setPosition(__subListCurPos + __subListCustLen);

		subList.Add(_subList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)subList.Count);
	for(int _i = 0; _i < subList.Count; _i++) { 
		_buf.putInt(subList[_i].GetBufSize());
	subList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)7);
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
	builder.Append("subList").Append(":").Append(subList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

