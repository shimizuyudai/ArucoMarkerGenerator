using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity.OpenCV
{
    public class GenerateMarkerSetting
    {
        public string Directory;
        public bool UseStreamingAssetsPath;
        public int Resolution;
        public int DictionaryId;
    }
    public class GenerateCanonicalMarkerSetting : GenerateMarkerSetting
    {
        public int[] MarkerIds;
    }

    public class GenerateGridBoardSetting : GenerateMarkerSetting
    {
        public int MarkersX;
        public int MarkersY;
        public int FirstMarker;
        public float Separation;
        public float MarkerLength;
    }

    public class GenerateChArucoBoardSetting : GenerateMarkerSetting
    {
        public int MarkersX;
        public int MarkersY;
        public float SquareLength;
        public float MarkerLength;
    }
}
