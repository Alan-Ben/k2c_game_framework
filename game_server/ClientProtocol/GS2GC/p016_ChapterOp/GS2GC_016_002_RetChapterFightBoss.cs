using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p016_ChapterOp
{

public class GS2GC_016_002_RetChapterFightBoss : ALBasicProtocolPack._IALProtocolStructure {
private long chapterId;
/// <summary>
/// 物品列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> rewardList;


public GS2GC_016_002_RetChapterFightBoss() {
	chapterId = (long)0;
	rewardList = new List<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_016_002_RetChapterFightBoss(
	long _chapterId
	, List<NPCommon.NPCommon_ItemInfo> _rewardList
) {	chapterId = _chapterId;
	rewardList = _rewardList;
}

public byte getMainOrder() { return (byte)16; }

public byte getSubOrder() { return (byte)2; }

public long getChapterId() { return chapterId; }
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
/// <summary>
/// 物品列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getRewardList() { return rewardList; }
/// <summary>
/// 物品列表
/// </summary>
public void addRewardList(NPCommon.NPCommon_ItemInfo _rewardList) { rewardList.Add(_rewardList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _rewardList = new NPCommon.NPCommon_ItemInfo();
		int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.getCurPos();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.setPosition(__rewardListCurPos + __rewardListCustLen);

		rewardList.Add(_rewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(chapterId);
	_buf.putShort((short)rewardList.Count);
	for(int _i = 0; _i < rewardList.Count; _i++) { 
		_buf.putInt(rewardList[_i].GetBufSize());
	rewardList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
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
	builder.Append("chapterId").Append(":").Append(chapterId.ToString()).Append(", ");
	builder.Append("rewardList").Append(":").Append(rewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

