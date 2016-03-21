using System.Collections.Generic;
using System.Xml;

public class VideoXMLParser {
    private static readonly VideoXMLParser _instance = new VideoXMLParser();
	private XmlDocument _XMLDocument;

    public static VideoXMLParser Instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            else
                return null;
        }
    }

	private VideoXMLParser() {
        _XMLDocument = new XmlDocument();
	}

    public void LoadXML(string fileName)
    {
        _XMLDocument.Load(fileName);
    }

    public VideoXMLWrapper ParseVideoXML()
    {
        XmlNodeList video_node = _XMLDocument.SelectNodes("/data/videos/video");

        return new VideoXMLWrapper(
            video_node.Item(0).InnerText,
            int.Parse(video_node.Item(0).Attributes.GetNamedItem("frame").Value),
            int.Parse(video_node.Item(0).Attributes.GetNamedItem("height").Value),
            int.Parse(video_node.Item(0).Attributes.GetNamedItem("width").Value)
        );
    }

    public List<MarkerXMLWrapper> ParseMarkerXML()
    {
        List<MarkerXMLWrapper> result = new List<MarkerXMLWrapper>();
        XmlNodeList marker_node = _XMLDocument.SelectNodes("/data/markers/marker");

        foreach (XmlNode marker in marker_node)
        {
            int markerId = int.Parse(marker.Attributes.GetNamedItem("id").Value);
            XmlNodeList track_list = marker.ChildNodes;
            MarkerXMLWrapper tmp = new MarkerXMLWrapper();
            tmp.MarkerID = markerId;
            foreach (XmlNode track in track_list)
            {
                int pos_x = (int)float.Parse(track.Attributes.GetNamedItem("position_x").Value.ToString());
                int pos_y = (int)float.Parse(track.Attributes.GetNamedItem("position_y").Value.ToString());
                int frame_id = int.Parse(track.Attributes.GetNamedItem("frame").Value.ToString());
                tmp.TrackList.Add(new MarkerWrapper(
                    frame_id, pos_x, pos_y
                    ));
            }
            result.Add(tmp);
        }

        return result;
    }
}
