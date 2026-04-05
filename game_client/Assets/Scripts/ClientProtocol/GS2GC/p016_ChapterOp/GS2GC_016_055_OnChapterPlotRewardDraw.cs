using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p016_ChapterOp
{

/// <summary>
/// 关卡剧情奖励领取推送
/// </summary>
public class GS2GC_016_055_OnChapterPlotRewardDraw : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 剧情ID列表
/// </summary>
private List<long> plotIdList;


public GS2GC_016_055_OnChapterPlotRewardDraw() {
	plotIdList = new List<long>();
}

public GS2GC_016_055_OnChapterPlotRewardDraw(
	List<long> _plotIdList
) {	plotIdList = _plotIdList;
}

public byte getMainOrder() { return (byte)16; }

public byte getSubOrder() { return (byte)55; }

/// <summary>
/// 剧情ID列表
/// </summary>
public List<long> getPlotIdList() { return plotIdList; }
/// <summary>
/// 剧情ID列表
/// </summary>
public void addPlotIdList(long _plotIdList) { plotIdList.Add(_plotIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (plotIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (plotIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _plotIdListCount = _buf.getShort();
	for(int _i = 0; _i < _plotIdListCount; _i++) { 
		long _plotIdList = (long)0;
		_plotIdList = _buf.getLong();
		plotIdList.Add(_plotIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)plotIdList.Count);
	for(int _i = 0; _i < plotIdList.Count; _i++) { 
		_buf.putLong(plotIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)55);
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
	builder.Append("plotIdList").Append(":").Append(plotIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

