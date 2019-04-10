using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;
using System.IO;

namespace UtilPack4Unity.OpenCV
{
    public class GenerateMarkerFromFile : MonoBehaviour
    {
        [SerializeField]
        MarkerGenerator markerGenerator;
        [SerializeField]
        string canonicalSettingFileName, gridBoardSettingFileName, chArucoBoardSettingFileName;
        [SerializeField]
        KeyCode generateCanonicalMarkerKey, generateGridBoardKey, generateChArucoBoardKey;

        // Start is called before the first frame update
        void Start()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(generateCanonicalMarkerKey))
            {
                GenerateCanonicalMarker();
            }
            if (Input.GetKeyDown(generateGridBoardKey))
            {
                GenerateGridBoard();
            }
            if (Input.GetKeyDown(generateChArucoBoardKey))
            {
                GenerateChArucoBoard();
            }
        }

        private void GenerateGridBoard()
        {
            var setting = IOHandler.LoadJson<GenerateGridBoardSetting>(IOHandler.IntoStreamingAssets(gridBoardSettingFileName));
            if (setting == null) return;
            if (setting == null) return;
            var directory = setting.UseStreamingAssetsPath ? IOHandler.IntoStreamingAssets(setting.Directory) : setting.Directory;
            if (!Directory.Exists(directory)) return;
            var dictionary = (MarkerGenerator.ArUcoDictionary)setting.DictionaryId;
            var name = "GridBoard_" + dictionary.ToString() + ".jpg";
            var path = Path.Combine(directory, name);
            markerGenerator.GenerateGridBoard(path, setting.DictionaryId, setting.Resolution, setting.FirstMarker, setting.MarkersX, setting.MarkersY, setting.MarkerLength, setting.Separation);
        }

        private void GenerateChArucoBoard()
        {
            var setting = IOHandler.LoadJson<GenerateChArucoBoardSetting>(IOHandler.IntoStreamingAssets(chArucoBoardSettingFileName));
            if (setting == null) return;
            var directory = setting.UseStreamingAssetsPath ? IOHandler.IntoStreamingAssets(setting.Directory) : setting.Directory;
            if (!Directory.Exists(directory)) return;
            var dictionary = (MarkerGenerator.ArUcoDictionary)setting.DictionaryId;
            var name = "ChArucoBoard_" + dictionary.ToString() + ".jpg";
            var path = Path.Combine(directory, name);
            markerGenerator.GenerateChArucoBoard(path, setting.DictionaryId, setting.Resolution, setting.MarkersX, setting.MarkersY, setting.SquareLength, setting.MarkerLength);
        }

        private void GenerateCanonicalMarker()
        {
            var setting = IOHandler.LoadJson<GenerateCanonicalMarkerSetting>(IOHandler.IntoStreamingAssets(canonicalSettingFileName));
            if (setting == null) return;
            var directory = setting.UseStreamingAssetsPath ? IOHandler.IntoStreamingAssets(setting.Directory) : setting.Directory;
            if (!Directory.Exists(directory)) return;
            var dictionary = (MarkerGenerator.ArUcoDictionary)setting.DictionaryId;
            foreach (var markerId in setting.MarkerIds)
            {
                var name = dictionary.ToString() + "_" + markerId + ".jpg";
                var path = Path.Combine(directory, name);
                markerGenerator.GenerateCanonicalMarker(path, setting.DictionaryId, markerId, setting.Resolution);
            }
        }

        [ContextMenu("ExportCanonicalMarkerSettingExample")]
        void ExportCanonicalMarkerSettingExample()
        {
            var path = Path.Combine(Application.streamingAssetsPath, canonicalSettingFileName);
            var setting = new GenerateCanonicalMarkerSetting();
            setting.Resolution = MarkerGenerator.Resolution;
            setting.UseStreamingAssetsPath = true;
            setting.Directory = "CanonicalMarker";
            setting.MarkerIds = new int[] { 0, 1, 2, 3 };
            IOHandler.SaveJson(path, setting);
        }

        [ContextMenu("ExportGridBoardSettingExample")]
        void ExportGridBoardSettingExample()
        {
            var path = Path.Combine(Application.streamingAssetsPath, gridBoardSettingFileName);
            var setting = new GenerateGridBoardSetting();
            setting.Resolution = MarkerGenerator.Resolution;
            setting.UseStreamingAssetsPath = true;
            setting.Directory = "GridBoard";
            setting.MarkersX = MarkerGenerator.GridBoardMarkersX;
            setting.MarkersY = MarkerGenerator.GridBoardMarkersY;
            setting.MarkerLength = MarkerGenerator.GridBoardMarkerLength;
            setting.Separation = MarkerGenerator.GridBoardMarkerSeparation;
            setting.FirstMarker = 0;
            IOHandler.SaveJson(path, setting);
        }

        [ContextMenu("ExportChArucoBoardSettingExample")]
        void ExportChArucoBoardSettingExample()
        {
            var path = Path.Combine(Application.streamingAssetsPath, chArucoBoardSettingFileName);
            var setting = new GenerateChArucoBoardSetting();
            setting.Resolution = MarkerGenerator.Resolution;
            setting.UseStreamingAssetsPath = true;
            setting.Directory = "ChArucoBoard";
            setting.MarkersX = MarkerGenerator.ChArUcoBoardMarkersX;
            setting.MarkersY = MarkerGenerator.ChArUcoBoardMarkersY;
            setting.SquareLength = MarkerGenerator.ChArUcoBoardSquareLength;
            setting.MarkerLength = MarkerGenerator.ChArUcoBoardMarkerLength;
            IOHandler.SaveJson(path, setting);
        }
    }
}
