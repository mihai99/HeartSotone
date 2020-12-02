#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System;

namespace FirebaseImporter
{

    [Serializable]
    public class ManifestJson
    {
        public Dictionary<string, string> dependencies;
        public ScopeData[] scopedRegistries;
    }

    [Serializable]
    public class ScopeData
    {
        public string name;
        public string url;
        public string[] scopes;
    }

    [Serializable]
    public class StructJsonPackage
    {
        public string name; // name package
        public Dictionary<string, VersionData> versions;
        public Dictionary<string, string> time;
    }

    [Serializable]
    public class VersionData
    {
        public string name;// "com.google.firebase.app",
        public string version;//: "6.13.0",
        public string displayName;//": "Firebase App (Core)",
        public string description;//": "Firebase App is the core library every Firebase package depends on.",
        public string unity;//": "2017.1",
        public Dictionary<string, string> dist;//": {
                                               //  "tarball": "https://dl.google.com/games/registry/unity/com.google.firebase.app/com.google.firebase.app-6.13.0.tgz",
                                               //  "shasum": "c2221d7d7692bfaa9b8e30ff949ff82ffef7a998"
                                               //},
        public Dictionary<string, string> author;//": {
                                                 //  "name": "Google LLC"
                                                 //},
        public Dictionary<string, string> dependencies;//": {
                                                       //  "com.google.external-dependency-manager": "1.2.144"
                                                       //},
        public string publishTime;//": "2020-03-25T19:25:06Z",
        public string[] keywords;//": [
                                 //  "Google",
                                 //  "Firebase",
                                 //  "FirebaseApp",
                                 //  "App"
                                 //]
    }

    public class FireBaseImportTool : EditorWindow
    {
        enum TypeGetData
        {
            none, loading, done
        }
        public const string ButtonInstall = "Install";
        public const string ButtonRemove = "Remove";
        const int widthS = 800;
        const int heightS = 500;

        string textButton = "";
        int selected = 2, // package đã được chọn --> đang active
            selectedVersion = 0; //version đã được chọn --> đang active
        int selecting = 2,//package đang chọn trên option dropdown
            selectingVersion = 0; //version đang chọn trên option dropdown

        Texture2D sprite;
        static StructJsonPackage currentData;
        static VersionData lastest;
        GUIStyle styleDefault = new GUIStyle();
        TypeGetData typeCurrent;

        static ManifestJson currentManifest;

        [MenuItem("Importer/Firebase Importer Tools")]
        public static void ShowWindow()
        {
            EditorWindow editor = EditorWindow.GetWindow(typeof(FireBaseImportTool));
            editor.titleContent = new GUIContent("Firebase Importer Tools");
            editor.minSize = new Vector2(widthS, heightS);
            editor.maxSize = new Vector2(widthS, heightS);
        }

        EditorCoroutine m_LoggerCoroutine;
        void RunLoad(string namePackage, bool isReload = false)
        {
            if (!isReload && CheckFileIsExist(namePackage))
            {
                string textData = File.ReadAllText(Configs.PathSaveJson + namePackage + ".json");
                ReadData(textData);
            }
            else
            {
                typeCurrent = TypeGetData.loading;
                m_LoggerCoroutine = EditorCoroutineUtility.StartCoroutineOwnerless(GetRequest("https://unityregistry-pa.googleapis.com/" + namePackage));
            }
        }

        bool CheckFileIsExist(string namePackage)
        {

            if (!Directory.Exists(Configs.PathSaveJson))
            {
                Directory.CreateDirectory(Configs.PathSaveJson);
            }

            return File.Exists(GetFileName(namePackage));
        }

        string GetFileName(string namePackage)
        {
            return Configs.PathSaveJson + namePackage + ".json";
        }

        private void Awake()
        {
            typeCurrent = TypeGetData.none;
            sprite = Resources.Load<Texture2D>("FMPluginEditor/progressindicator");
            //Debug.Log(sprite.name);
            RunLoad(Configs.Options[selected]);
        }

        void OnDisable()
        {
            if (m_LoggerCoroutine != null)
            {
                EditorCoroutineUtility.StopCoroutine(m_LoggerCoroutine);
            }
        }

        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    if (ReadData(webRequest.downloadHandler.text))
                    {
                        string path = GetFileName(Configs.Options[selected]);
                        if (CheckFileIsExist(Configs.Options[selected]))
                        {
                            File.Delete(path);
                        }
                        File.WriteAllText(path, webRequest.downloadHandler.text);
                    }
                }
            }
        }

        bool ReadData(string textData)
        {
            selectingVersion = 0;

            typeCurrent = TypeGetData.done;
            //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            currentData = JsonConvert.DeserializeObject<StructJsonPackage>(textData);
            //Debug.Log(m.name);
            currentData.versions = currentData.versions.Reverse().ToDictionary(x => x.Key, x => x.Value);

            string keyVersionInManifest = GetIndexPackageInManifest(Configs.Options[selected]);
            if (keyVersionInManifest != "")
            {
                selectedVersion = currentData.versions.Keys.ToList().IndexOf(keyVersionInManifest);
                GetDataByVersion(keyVersionInManifest);
            }
            else
            {
                selectedVersion = 0;
                GetDataByVersion(currentData.versions.ElementAt(0).Key);
            }
            return true;
            //string key = "\n\"" + lastest.name + "\": \"" + lastest.version + "\",";
            //Debug.Log(key);
        }

        void GetDataByVersion(string key)
        {
            lastest = currentData.versions[key];
        }

        void LoadInfomation(VersionData data)
        {
            if (data == null) return;
            CheckPackageExistInManifest(data.name, data.version);
            styleDefault.alignment = TextAnchor.MiddleLeft;
            GUILayout.Space(20);
            GUILayout.BeginVertical("box", GUILayout.Width(widthS - 20));
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 18;
            myStyle.fontStyle = FontStyle.Bold;
            GUILayout.Space(10);
            GUILayout.Label(data.displayName, myStyle);

            GUILayout.Space(5);
            myStyle = new GUIStyle();
            myStyle.fontSize = 14;
            GUILayout.Label("Version: " + data.version, myStyle);

            GUILayout.Space(5);
            myStyle = new GUIStyle();
            myStyle.fontStyle = FontStyle.Italic;
            GUILayout.Label(data.name, myStyle);

            if (data.author != null)
            {
                string str = "";
                for (int i = 0; i < data.author.Count; i++)
                {
                    str += data.author.ElementAt(i).Value + (i == data.author.Count - 1 ? "" : ",");
                }
                GUILayout.Space(5);
                styleDefault.wordWrap = true;
                GUILayout.Label("Author: " + str, styleDefault);
            }

            GUILayout.Space(5);
            GUILayout.Label("Description: " + data.description, styleDefault);
            GUILayout.Space(5);
            GUILayout.Label("Plugin support Unity: " + data.unity, styleDefault);

            GUILayout.Space(10);
            GUILayout.EndVertical();
        }

        void Link(string str, string url, float width = 128)
        {
            GUILayout.Space(15);
            GUILayout.BeginHorizontal("box");
            GUIStyle myStyle = new GUIStyle();
            //myStyle.fontStyle = FontStyle.Bold;
            myStyle.fixedWidth = width;
            GUILayout.Label(str, myStyle);
            GUIStyle stl = new GUIStyle();
            stl.normal.textColor = Color.blue;
            stl.fontStyle = FontStyle.Bold;
            stl.alignment = TextAnchor.MiddleLeft;
            if (GUILayout.Button("Click Here", stl))
            {
                Help.BrowseURL(url);
            }
            GUILayout.EndHorizontal();
        }

        private void OnGUI()
        {

            GUILayout.Space(15);
            GUILayout.BeginVertical("box");
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontStyle = FontStyle.Bold;
            myStyle.fontSize = 11;
            myStyle.normal.textColor = Color.red;
            GUILayout.Label("Note - Important: ", myStyle);
            myStyle = new GUIStyle();
            myStyle.wordWrap = true;
            GUILayout.Label(Configs.NotesContent, myStyle);
            GUILayout.EndVertical();

            Link("Firebase Console - Create project and dowload your json project:", Configs.UrlFirebaseConsole, 383);
            Link("Firebase Example git:", Configs.UrlFirebaseExample);

            GUILayout.Space(15);
            myStyle = new GUIStyle("Popup");
            myStyle.fontStyle = FontStyle.Bold;
            selected = EditorGUILayout.Popup("Select Firebase Package", selected, Configs.Options, myStyle);
            if (selecting != selected)
            {
                selecting = selected;
                RunLoad(Configs.Options[selected]);
            }

            switch (typeCurrent)
            {
                case TypeGetData.none:
                    break;
                case TypeGetData.loading:
                    GUILayout.Space(5);
                    styleDefault.alignment = TextAnchor.MiddleCenter;
                    GUILayout.Label("Loading... ", styleDefault);
                    break;
                case TypeGetData.done:

                    if (currentData != null && currentData.versions != null)
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();

                        //GUILayout.BeginVertical();
                        myStyle = new GUIStyle("Popup");
                        myStyle.fontStyle = FontStyle.Bold;
                        selectedVersion = EditorGUILayout.Popup("Select Version Package", selectedVersion, currentData.versions.Keys.ToArray(), myStyle);
                        if (selectingVersion != selectedVersion)
                        {
                            selectingVersion = selectedVersion;
                            GetDataByVersion(currentData.versions.ElementAt(selectedVersion).Key);
                        }
                        // GUILayout.EndVertical();

                        GUILayout.Space(5);
                        myStyle = new GUIStyle("Button");
                        myStyle.fontStyle = FontStyle.Bold;
                        myStyle.normal.textColor = Color.blue;
                        //GUILayout.BeginArea(new Rect(position.width - 170, lastRect.y + 50, 150, 100));
                        if (GUILayout.Button(textButton, myStyle, GUILayout.Width(150), GUILayout.Height(30)))
                        {
                            if (selecting != selected)
                            {
                                selecting = selected;
                                RunLoad(Configs.Options[selected]);
                            }
                            InstallOrRemovePackage(lastest.name, lastest.version);
                        }

                        if (GUILayout.Button("Reload Data", GUILayout.Width(100), GUILayout.Height(30)))
                        {
                            RunLoad(Configs.Options[selected], true);
                        }
                        // GUILayout.EndArea();
                        GUILayout.EndHorizontal();
                    }

                    if (lastest != null)
                    {
                        LoadInfomation(lastest);
                    }
                    break;
            }

        }

        void LoadManifest()
        {
            string jsonString = File.ReadAllText(Configs.PathManifest);
            currentManifest = JsonConvert.DeserializeObject<ManifestJson>(jsonString);
        }

        string GetIndexPackageInManifest(string key)
        {
            if (currentManifest == null) LoadManifest();

            if (currentManifest.dependencies.ContainsKey(key))
            {
                return currentManifest.dependencies[key];
            }
            return "";
        }

        void CheckPackageExistInManifest(string key, string keyvalue)
        {
            if (currentManifest == null) LoadManifest();
            if (currentManifest.dependencies.ContainsKey(key))
            {
                if (currentManifest.dependencies[key] == keyvalue)
                    textButton = ButtonRemove;
                else
                    textButton = ButtonInstall;
            }
            else
                textButton = ButtonInstall;
        }

        public void InstallOrRemovePackage(string packagekey, string version)
        {
            if (currentManifest == null) return;

            if (textButton == ButtonInstall)
            {
                bool a = currentManifest.dependencies.ContainsKey(packagekey);
                if (a)
                {
                    Debug.Log("Remove package First" + packagekey);
                    currentManifest.dependencies.Remove(packagekey);
                }
                Debug.Log("Add package " + packagekey);
                currentManifest.dependencies.Add(packagekey, version);
            }
            else
            {
                Debug.Log("Remove package " + packagekey);
                currentManifest.dependencies.Remove(packagekey);
            }

            if (currentManifest.scopedRegistries == null || currentManifest.scopedRegistries.Length == 0)
            {
                Debug.Log("Create package google");
                ScopeData data = new ScopeData();
                data.name = Configs.NameGoogle;
                data.url = Configs.UrlGoogle;
                data.scopes = new string[] { Configs.ScopeGoogle };
                currentManifest.scopedRegistries = new ScopeData[] { data };
            }

            string newJson = JsonConvert.SerializeObject(currentManifest, Formatting.Indented);
            File.WriteAllText(Configs.PathManifest, newJson);
            AssetDatabase.Refresh();
        }
        Rect GetRectLoadingImage()
        {
            int Swidth = Screen.width;
            int SHeight = Screen.height;
            int w = sprite.width;
            int h = sprite.height;
            return new Rect(Swidth / 2 - w / 2, SHeight / 2 - h / 2, w, h);
        }

        void ExtendToolGetNamePackageFromFolder()
        {
            string path = @"C:/Users/Marks/AppData/Local/Unity/cache/npm/unityregistry-pa.googleapis.com/unityregistry-pa.googleapis.com";
            string[] subdirectoryEntries = Directory.GetDirectories(path);

            string str = "";
            foreach (string subdirectory in subdirectoryEntries)
            {
                int index = subdirectory.IndexOf("\\");
                if (index != -1)
                {
                    str += "\"" + subdirectory.Substring(index + 1) + "\",";
                }
            }
            Debug.Log(str);
        }
    }
}
#endif