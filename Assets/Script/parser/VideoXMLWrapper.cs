﻿
public class VideoXMLWrapper {
	private string _file;
    private int _height;
    private int _width;
    private int _fps;

	public string VideoFile {
		get {
			return _file;
		}
	}

    public int FPS
    {
        get
        {
            return _fps;
        }
    }

    public int Height
    {
        get
        {
            return _height;
        }
    }

    public int Width
    {
        get
        {
            return _width;
        }
    }

	public VideoXMLWrapper(string _f, int _fs, int _h, int _w) {
		_file = _f;
        _fps = _fs;
        _height = _h;
        _width = _w;
	}
}
