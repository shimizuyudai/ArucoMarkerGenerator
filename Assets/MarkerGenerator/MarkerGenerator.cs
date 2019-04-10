using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ArucoModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.UnityUtils;
using UtilPack4Unity;
using System.IO;

namespace UtilPack4Unity.OpenCV
{
    public class MarkerGenerator : TextureHolderBase
    {
        public const int BorderBits = 1;
        public const int GridBoardMarkersX = 5;
        public const int GridBoardMarkersY = 7;
        public const float GridBoardMarkerLength = 0.04f;
        public const float GridBoardMarkerSeparation = 0.01f;
        public const int GridBoardMarkerFirstMarker = 0;
        public const int GridBoardMarginSize = 0;

        public const int ChArUcoBoardMarkersX = 5;
        public const int ChArUcoBoardMarkersY = 7;
        public const float ChArUcoBoardSquareLength = 0.04f;
        public const float ChArUcoBoardMarkerLength = 0.02f;
        public const int ChArUcoBoardMarginSize = 0;

        public const int Resolution = 2048;
        public Texture2D ResultTexture
        {
            get;
            private set;
        }
        [SerializeField]
        readonly string defaultDirectory = Application.streamingAssetsPath;

        public override Texture GetTexture()
        {
            return ResultTexture;
        }
        [SerializeField]
        TextureUtilBehaviour textureUtilBehaviour;

        // Start is called before the first frame update
        void Start()
        {
            ResultTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        [ContextMenu("ExportMarker")]
        public void TestExportMarker()
        {
            var markerId = 0;
            var dictionary = ArUcoDictionary.DICT_4X4_1000;
            var path = Path.Combine(defaultDirectory, "marker_" + markerId + ".png");
            GenerateCanonicalMarker(path, (int)ArUcoDictionary.DICT_4X4_1000, markerId);
        }

        [ContextMenu("ExportGridBoard")]
        void TestExportGridBoard()
        {
            var path = Path.Combine(defaultDirectory, "gridboard_" + 0 + ".png");
            GenerateGridBoard(path, (int)ArUcoDictionary.DICT_4X4_1000);
        }

        [ContextMenu("ExportCharucoBoard")]
        void TestExportChArucoBoard()
        {
            var path = Path.Combine(defaultDirectory, "chArucoboard_" + 0 + ".png");
            GenerateChArucoBoard(path, (int)ArUcoDictionary.DICT_4X4_1000);
        }

        public void GenerateCanonicalMarker(string path, int dictionaryId, int markerId, int resolution = Resolution)
        {
            ResultTexture = textureUtilBehaviour.SecureTexture(ResultTexture, resolution, resolution);
            Dictionary dictionary = Aruco.getPredefinedDictionary((int)dictionaryId);
            using (var mat = new Mat(resolution, resolution, CvType.CV_8UC3))
            {
                Aruco.drawMarker(dictionary, (int)markerId, resolution, mat, BorderBits);
                Utils.matToTexture2D(mat, ResultTexture);
                ExportTexture(path);
            }
        }

        public void GenerateGridBoard(string path, int dictionaryId, int resolution = Resolution, int firstMarker = GridBoardMarkerFirstMarker, int markersX = GridBoardMarkersX, int markersY = GridBoardMarkersY, float markerLength = GridBoardMarkerLength, float separation = GridBoardMarkerSeparation)
        {
            var w = markersX * (markerLength + separation) - separation;
            var h = markersY * (markerLength + separation) - separation;
            var r = EMath.GetShrinkFitSize(new Vector2(w, h), Vector2.one * resolution);
            ResultTexture = textureUtilBehaviour.SecureTexture(ResultTexture, (int)r.x, (int)r.y);
            this.SetTexture(ResultTexture);
            Dictionary dictionary = Aruco.getPredefinedDictionary((int)dictionaryId);

            using (var mat = new Mat((int)r.x, (int)r.y, CvType.CV_8UC3))
            using (var gridBoard = GridBoard.create(markersX, markersY, markerLength, separation, dictionary, firstMarker))
            {
                gridBoard.draw(new Size(r.x, r.y), mat, GridBoardMarginSize, BorderBits);
                Utils.matToTexture2D(mat, ResultTexture);
                ExportTexture(path);
            }
        }

        public void GenerateChArucoBoard(string path, int dictionaryId, int resolution = Resolution, int markersX = ChArUcoBoardMarkersX, int markersY = ChArUcoBoardMarkersY, float squareLength = ChArUcoBoardSquareLength, float markerLength = ChArUcoBoardMarkerLength)
        {
            var w = markersX * (squareLength);
            var h = markersY * (squareLength);
            var r = EMath.GetShrinkFitSize(new Vector2(w, h), Vector2.one * resolution);
            ResultTexture = textureUtilBehaviour.SecureTexture(ResultTexture, (int)r.x, (int)r.y);
            this.SetTexture(ResultTexture);
            Dictionary dictionary = Aruco.getPredefinedDictionary((int)dictionaryId);
            using (var mat = new Mat((int)r.x, (int)r.y, CvType.CV_8UC3))
            using (var chArucoBoard = CharucoBoard.create(markersX, markersY, squareLength, markerLength, dictionary))
            {
                chArucoBoard.draw(new Size(r.x, r.y), mat, ChArUcoBoardMarginSize, BorderBits);
                Utils.matToTexture2D(mat, ResultTexture);
                ExportTexture(path);
            }
        }

        private void ExportTexture(string path)
        {
            print(path);
            var extension = Path.GetExtension(path);
            extension = extension.ToLower();
            print(extension);
            byte[] bytes = null;
            if (extension == ".jpg")
            {
                bytes = ResultTexture.EncodeToJPG();
            }
            else if (extension == ".png")
            {
                bytes = ResultTexture.EncodeToPNG();
            }
            if (bytes != null)
            {
                File.WriteAllBytes(path, bytes);
            }
        }

        public enum MarkerType
        {
            CanonicalMarker = 0,
            GridBoard,
            ChArUcoBoard
        }

        public enum ArUcoDictionary
        {
            DICT_4X4_50 = Aruco.DICT_4X4_50,
            DICT_4X4_100 = Aruco.DICT_4X4_100,
            DICT_4X4_250 = Aruco.DICT_4X4_250,
            DICT_4X4_1000 = Aruco.DICT_4X4_1000,
            DICT_5X5_50 = Aruco.DICT_5X5_50,
            DICT_5X5_100 = Aruco.DICT_5X5_100,
            DICT_5X5_250 = Aruco.DICT_5X5_250,
            DICT_5X5_1000 = Aruco.DICT_5X5_1000,
            DICT_6X6_50 = Aruco.DICT_6X6_50,
            DICT_6X6_100 = Aruco.DICT_6X6_100,
            DICT_6X6_250 = Aruco.DICT_6X6_250,
            DICT_6X6_1000 = Aruco.DICT_6X6_1000,
            DICT_7X7_50 = Aruco.DICT_7X7_50,
            DICT_7X7_100 = Aruco.DICT_7X7_100,
            DICT_7X7_250 = Aruco.DICT_7X7_250,
            DICT_7X7_1000 = Aruco.DICT_7X7_1000,
            DICT_ARUCO_ORIGINAL = Aruco.DICT_ARUCO_ORIGINAL,
        }

        //public enum ArUcoDictionary
        //{
        //    DICT_4X4_50 = 0,
        //    DICT_4X4_100,
        //    DICT_4X4_250,
        //    DICT_4X4_1000,
        //    DICT_5X5_50,
        //    DICT_5X5_100,
        //    DICT_5X5_250,
        //    DICT_5X5_1000,
        //    DICT_6X6_50,
        //    DICT_6X6_100,
        //    DICT_6X6_250,
        //    DICT_6X6_1000,
        //    DICT_7X7_50,
        //    DICT_7X7_100,
        //    DICT_7X7_250,
        //    DICT_7X7_1000,
        //    DICT_ARUCO_ORIGINAL
        //}
    }
}