using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p031_RankOp
{

public class GS2GC_031_004_RetRankFixedAKeyLike : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 点赞结果列表
/// </summary>
private List<Common.RankObj.RankFixed_LikeResult> likeResultList;


public GS2GC_031_004_RetRankFixedAKeyLike() {
	likeResultList = new List<Common.RankObj.RankFixed_LikeResult>();
}

public GS2GC_031_004_RetRankFixedAKeyLike(
	List<Common.RankObj.RankFixed_LikeResult> _likeResultList
) {	likeResultList = _likeResultList;
}

public byte getMainOrder() { return (byte)31; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 点赞结果列表
/// </summary>
public List<Common.RankObj.RankFixed_LikeResult> getLikeResultList() { return likeResultList; }
/// <summary>
/// 点赞结果列表
/// </summary>
public void addLikeResultList(Common.RankObj.RankFixed_LikeResult _likeResultList) { likeResultList.Add(_likeResultList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < likeResultList.Count; _i++) {
	_size += 4 + likeResultList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < likeResultList.Count; _i++) {
	_size += 4 + likeResultList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _likeResultListCount = _buf.getShort();
	for(int _i = 0; _i < _likeResultListCount; _i++) { 
		Common.RankObj.RankFixed_LikeResult _likeResultList = new Common.RankObj.RankFixed_LikeResult();
		int __likeResultListCustLen = _buf.getInt();
	int __likeResultListCurPos = _buf.getCurPos();
	_likeResultList.ReadUnzipBuf(_buf, __likeResultListCurPos + __likeResultListCustLen);
	_buf.setPosition(__likeResultListCurPos + __likeResultListCustLen);

		likeResultList.Add(_likeResultList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)likeResultList.Count);
	for(int _i = 0; _i < likeResultList.Count; _i++) { 
		_buf.putInt(likeResultList[_i].GetBufSize());
	likeResultList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)31);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)31);
	_recBuf.put((byte)4);
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
	builder.Append("likeResultList").Append(":").Append(likeResultList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

