using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p031_RankOp
{

public class GS2GC_031_002_RetRankFixedLike : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 点赞结果
/// </summary>
private Common.RankObj.RankFixed_LikeResult likeResult;


public GS2GC_031_002_RetRankFixedLike() {
	likeResult = new Common.RankObj.RankFixed_LikeResult();
}

public GS2GC_031_002_RetRankFixedLike(
	Common.RankObj.RankFixed_LikeResult _likeResult
) {	likeResult = _likeResult;
}

public byte getMainOrder() { return (byte)31; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 点赞结果
/// </summary>
public Common.RankObj.RankFixed_LikeResult getLikeResult() { return likeResult; }
/// <summary>
/// 点赞结果
/// </summary>
public void setLikeResult(Common.RankObj.RankFixed_LikeResult _likeResult) { likeResult = _likeResult; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + likeResult.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + likeResult.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _likeResultCustLen = _buf.getInt();
	int _likeResultCurPos = _buf.getCurPos();
	likeResult.ReadUnzipBuf(_buf, _likeResultCurPos + _likeResultCustLen);
	_buf.setPosition(_likeResultCurPos + _likeResultCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(likeResult.GetBufSize());
	likeResult.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)31);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)31);
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
	builder.Append("likeResult").Append(":").Append(likeResult == null ? "null" : likeResult.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

