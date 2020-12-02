using System.IO;
using UnityEngine;

namespace FirebaseImporter
{
    public class Configs
    {
        public static string GetRealPath(string p)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                return p;
            if (Application.platform == RuntimePlatform.OSXEditor)
                return p.Replace("\\", "/");
            return "\\";
        }

        public const string UrlFirebaseConsole = "https://console.firebase.google.com/u/0/";
        public const string UrlFirebaseExample = "https://github.com/firebase/quickstart-unity";
        public const string KEY_IS_DEBUG_ON_EDIOR = "Is_Debug_On_Editor";
        public const string NameGoogle = "Game Package Registry by Google";
        public const string UrlGoogle = "https://unityregistry-pa.googleapis.com";
        public const string ScopeGoogle = "com.google";
        public const string path = "\\..\\Packages\\manifest.json";
        public const string NotesContent = "Always import com.google.firebase.app(core) to not get\"DllNotFoundException: FirebaseCppApp_6_16_1...\" error when building your app.";

        public const string pathSaveJson = "\\..\\Library\\FirebaseImporter\\";
        public static string PathManifest { get => Application.dataPath + GetRealPath(path); }
        public static string PathSaveJson { get => Application.dataPath + GetRealPath(pathSaveJson); }

        public static string[] Options = new string[]
        {
            "com.google.android.appbundle",
            "com.google.external-dependency-manager",
            "com.google.firebase.analytics",
            "com.google.firebase.app",
            "com.google.firebase.auth",
            "com.google.firebase.crashlytics",
            "com.google.firebase.database",
            "com.google.firebase.dynamic-links",
            "com.google.firebase.firestore",
            "com.google.firebase.functions",
            "com.google.firebase.instance-id",
            "com.google.firebase.messaging",
            "com.google.firebase.remote-config",
            "com.google.firebase.storage",
            "com.google.play.assetdelivery",
            "com.google.play.billing",
            "com.google.play.common",
            "com.google.play.core",
            "com.google.play.instant",
            "com.google.play.review"
        };

        public static string[] OptionsDefine = new string[]
        {
            "FB_APP_BUNDLE",// "com.google.android.appbundle",
            "FB_EX_MANAGER",//"com.google.external-dependency-manager",
            "FB_ANALYTICS",//"com.google.firebase.analytics",
            "FB_APP",//"com.google.firebase.app",
            "FB_AUTHEN",//"com.google.firebase.auth",
            "FB_CRASHLYTICS",//"com.google.firebase.crashlytics",
            "FB_DATABASE",//"com.google.firebase.database",
            "FB_DYNAMIC_LINK",//"com.google.firebase.dynamic-links",
            "FB_FIRESTORE",//"com.google.firebase.firestore",
            "FB_FUNCTIONS",//"com.google.firebase.functions",
            "FB_INSTANCE_ID",//"com.google.firebase.instance-id",
            "FB_MESSAGING",//"com.google.firebase.messaging",
            "FB_REMOTE_CONFIG",//"com.google.firebase.remote-config",
            "FB_STORAGE",//"com.google.firebase.storage",
            "FB_ASSET_DELIVERY",//"com.google.play.assetdelivery",
            "FB_BILLING",//"com.google.play.billing",
            "FB_PLAY_COMMON",//"com.google.play.common",
            "FB_PLAY_CORE",//"com.google.play.core",
            "FB_PLAY_INSTANT",//"com.google.play.instant",
            "FB_REVIEW",//"com.google.play.review"
        };
        public static string[] OptionsPath = new string[]
        {
            "",//"com.google.android.appbundle",
            "",//"com.google.external-dependency-manager",
            "firebase-analytics-unity",//"com.google.firebase.analytics",
            "firebase-app-unity",//"com.google.firebase.app",
            "firebase-auth-unity",//"com.google.firebase.auth",
            "firebase-crashlytics-unity",//"com.google.firebase.crashlytics",
            "firebase-database-unity",//"com.google.firebase.database",
            "firebase-dynamic-links-unity",//"com.google.firebase.dynamic-links",
            "firebase-firestore-unity",//"com.google.firebase.firestore",
            "firebase-functions-unity",//"com.google.firebase.functions",
            "firebase-instance-id-unity",//"com.google.firebase.instance-id",
            "firebase-messaging-unity",//"com.google.firebase.messaging",
            "firebase-config-unity",//"com.google.firebase.remote-config",
            "firebase-storage-unity",//"com.google.firebase.storage",
            "",//"com.google.play.assetdelivery",
            "",//"com.google.play.billing",
            "",//"com.google.play.common",
            "",//"com.google.play.core",
            "",//"com.google.play.instant",
            "",//"com.google.play.review"
        };

    }
}
