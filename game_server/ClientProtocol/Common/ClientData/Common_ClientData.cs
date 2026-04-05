using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

/// <summary>
/// 通用ClientData
/// </summary>
public class Common_ClientData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否看过开篇剧情
/// </summary>
private bool isReadStoryVideo;
/// <summary>
/// 是否选择了登录火星的地点
/// </summary>
private bool isSelectLandingMarsArea;
/// <summary>
/// 是否完成了商店评价
/// </summary>
private bool isFinishStoreReviews;


public Common_ClientData() {
	isReadStoryVideo = false;
	isSelectLandingMarsArea = false;
	isFinishStoreReviews = false;
}

public Common_ClientData(
	bool _isReadStoryVideo
	, bool _isSelectLandingMarsArea
	, bool _isFinishStoreReviews
) {	isReadStoryVideo = _isReadStoryVideo;
	isSelectLandingMarsArea = _isSelectLandingMarsArea;
	isFinishStoreReviews = _isFinishStoreReviews;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 是否看过开篇剧情
/// </summary>
public bool getIsReadStoryVideo() { return isReadStoryVideo; }
/// <summary>
/// 是否看过开篇剧情
/// </summary>
public void setIsReadStoryVideo(bool _isReadStoryVideo) { isReadStoryVideo = _isReadStoryVideo; }
/// <summary>
/// 是否选择了登录火星的地点
/// </summary>
public bool getIsSelectLandingMarsArea() { return isSelectLandingMarsArea; }
/// <summary>
/// 是否选择了登录火星的地点
/// </summary>
public void setIsSelectLandingMarsArea(bool _isSelectLandingMarsArea) { isSelectLandingMarsArea = _isSelectLandingMarsArea; }
/// <summary>
/// 是否完成了商店评价
/// </summary>
public bool getIsFinishStoreReviews() { return isFinishStoreReviews; }
/// <summary>
/// 是否完成了商店评价
/// </summary>
public void setIsFinishStoreReviews(bool _isFinishStoreReviews) { isFinishStoreReviews = _isFinishStoreReviews; }


public int GetBufSize() {
	int _size = 3;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 5;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isReadStoryVideo = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isSelectLandingMarsArea = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isFinishStoreReviews = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isReadStoryVideo?(byte)1:(byte)0);
	_buf.put(isSelectLandingMarsArea?(byte)1:(byte)0);
	_buf.put(isFinishStoreReviews?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("isReadStoryVideo").Append(":").Append(isReadStoryVideo.ToString()).Append(", ");
	builder.Append("isSelectLandingMarsArea").Append(":").Append(isSelectLandingMarsArea.ToString()).Append(", ");
	builder.Append("isFinishStoreReviews").Append(":").Append(isFinishStoreReviews.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

