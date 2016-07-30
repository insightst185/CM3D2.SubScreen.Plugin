﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;
using System.Linq;
using System.Xml.XPath;

namespace CM3D2.SubScreen.Plugin
{
    [PluginFilter("CM3D2x64"),
    PluginFilter("CM3D2x86"),
    PluginFilter("CM3D2VRx64"),
    PluginFilter("CM3D2OHx64"),
    PluginFilter("CM3D2OHx86"),
    PluginFilter("CM3D2OHVRx64"),
    PluginName("CM3D2 SubScreen"),
    PluginVersion("0.3.9.12")]
    public class SubScreen : PluginBase
    {
        public const string Version = "0.3.9.13";

        private bool isChubLip = false;
		private bool isVR = false;
        private bool isCameraToggle = false;


        public readonly string WinFileName = Directory.GetCurrentDirectory() + @"\UnityInjector\Config\SubScreen.png";

        private string presetXmlFileName = Application.dataPath + "/../UnityInjector/Config/SubScreenPreset.xml";

        const int SubScreenLayer = 1;

        const string PKeyEnable = "ENABLE";
        const string PKeyLookAtMaid = "LOOK_AT_MAID";

        const string PKeyAlwaysLookAtMaid = "ALWAYS_LOOK_AT_MAID";

        const string PKeyAlwaysLookAtFace = "ALWAYS_LOOK_AT_FACE";
        const string PPropLookAtFaceUp = "LOOK_AT_FACE.up";
        const string PPropLookAtFaceLeft = "LOOK_AT_FACE.left";
        const string PPropLookAtFaceFront = "LOOK_AT_FACE.front";

        const string PKeyAlwaysLookAtXxx = "ALWAYS_LOOK_AT_XXX";
        const string PPropLookAtXxxUp = "LOOK_AT_XXX.up";
        const string PPropLookAtXxxLeft = "LOOK_AT_XXX.left";
        const string PPropLookAtXxxFront = "LOOK_AT_XXX.front";

        const string PKeyAlwaysScreenOnMainCamera = "ALWAYS_SCREEN";
        const string PPropAlwaysScreenOnMainCameraUp = "ALWAYS_SCREEN.up";
        const string PPropAlwaysScreenOnMainCameraLeft = "ALWAYS_SCREEN.left";
        const string PPropAlwaysScreenOnMainCameraFront = "ALWAYS_SCREEN.front";

        const string PKeyResetCameraPos = "RESET_CAMERA_POS";
        const string PKeyMoveToBack = "MOVE_TO_BACK";

        const string PKeySubCamera = "SUB_CAMERA";
        const string PPropSubCameraPosX = "SUB_CAMERA.xMin";
        const string PPropSubCameraPosY = "SUB_CAMERA.yMin";
        const string PPropSubCameraWidth = "SUB_CAMERA.width";
        const string PPropSubCameraLocX = "SUB_CAMERA.LocX";
        const string PPropSubCameraLocY = "SUB_CAMERA.LocY";
        const string PPropSubCameraLocZ = "SUB_CAMERA.LocZ";
        const string PPropSubCameraAngX = "SUB_CAMERA.AngX";
        const string PPropSubCameraAngY = "SUB_CAMERA.AngY";
        const string PPropSubCameraAngZ = "SUB_CAMERA.AngZ";

        const string PKeyMainLight = "MAIN_LIGHT";
        const string PPropMainLightLuminance = "MAIN_LIGHT.l";
        const string PPropMainLightColorRed = "MAIN_LIGHT.r";
        const string PPropMainLightColorGreen = "MAIN_LIGHT.g";
        const string PPropMainLightColorBlue = "MAIN_LIGHT.b";

        const string PKeySubLight = "SUB_LIGHT";
        const string PPropSubLightRange = "SUB_LIGHT.range";
        const string PPropSubLightLuminance = "SUB_LIGHT.l";
        const string PPropSubLightColorRed = "SUB_LIGHT.r";
        const string PPropSubLightColorGreen = "SUB_LIGHT.g";
        const string PPropSubLightColorBlue = "SUB_LIGHT.b";

        const string PKeyCameraColor = "CAMERA_COLOR";
        const string PPropCameraColorRed = "CAMERA_COLOR.r";
        const string PPropCameraColorGreen = "CAMERA_COLOR.g";
        const string PPropCameraColorBlue = "CAMERA_COLOR.b";
        const string PPropCameraColorAlpha = "CAMERA_COLOR.a";
        const string PPropSubCameraFieldOfView = "CAMERA_COLOR.fieldOfView";

        const string PKeyScreenFilter = "SCREEN_FILTER";
        const string PPropScreenFilterLuminance = "SCREEN_FILTER.l";
        const string PPropScreenFilterRed = "SCREEN_FILTER.r";
        const string PPropScreenFilterGreen = "SCREEN_FILTER.g";
        const string PPropScreenFilterBlue = "SCREEN_FILTER.b";
        const string PPropScreenFilterAlpha = "SCREEN_FILTER.a";

        const string PEyeMove = "EYE_MOVE";
        const string PPropEyeMoveL = "EYE_MOVE.l";
        const string PPropEyeMoveM = "EYE_MOVE.m";
        const string PPropEyeMoveN = "EYE_MOVE.n";
        const string PPropEyeMoveR = "EYE_MOVE.r";
        const string PPropEyeMoveS = "EYE_MOVE.s";
        const string PPropEyeMoveT = "EYE_MOVE.t";

        const string PKeyBSPos = "BS_POS";
        const string PPropBSPosX = "BS_POS.x";
        const string PPropBSPosY = "BS_POS.y";
        const string PPropBSPosZ = "BS_POS.z";

        const string PKeyBSSize = "BS_SIZE";
        const string PPropBSSize = "BS_SIZE";

        const string PKeyBSAngle = "BS_ANGLE";
        const string PPropBSAngleX = "BS_ANGLE.x";
        const string PPropBSAngleY = "BS_ANGLE.y";
        const string PPropBSAngleZ = "BS_ANGLE.z";

        const string PKeyBSColor = "BS_COLOR";
        const string PPropScreenLightLuminance = "SCREEN_LIGHT.l";
        const string PPropBSColorLuminance = "BS_COLOR.l";
        const string PPropBSColorRed = "BS_COLOR.r";
        const string PPropBSColorGreen = "BS_COLOR.g";
        const string PPropBSColorBlue = "BS_COLOR.b";
        const string PPropBSColorAlpha = "BS_COLOR.a";

        private enum TargetLevel
        {
            //ダンス1:ドキドキ☆Fallin' Love
            SceneDance_DDFL = 4,

            // エディット
            SceneEdit = 5,

            // 夜伽
            SceneYotogi = 14,

            // ADVパート
            SceneADV = 15,

            // ダンス2:entrance to you
            SceneDance_ETYL = 20,

            // ダンス3:scarlet leap
            SceneDance_SCLP = 22,

            // ダンス4:stellar my tears
            SceneDance_STMT = 26,

            // ダンス5:rhythmix to you
            SceneDance_RYFU = 28,

            // ダンス6:happy!happy!スキャンダル
            SceneDance_HAPY = 30
        }

        private enum TargetLevelCbl
        {
            //ダンス1:ドキドキ☆Fallin' Love
            SceneDance_DDFL_Release = 3,

            // エディット
            SceneEdit = 4,

            // 夜伽
            SceneYotogiWithChubLip = 10,

            // ADVパート
            SceneADV = 11,

            // ダンス2:entrance to you
            SceneDance_ETYL_Release = 16,

            // ダンス3:scarlet leap
            SceneDance_SCL_Release = 18,

            // ダンス4:stellar my tears
            SceneDance_SMT_Release = 20,

            // ダンス5:rhythmix to you
            SceneDance_RTY_Release = 22
        }
        private enum MenuType
        {
            None,
            Main,
            LoadPreset,
            SavePreset,
            RemovePreset,
            SaveScenePreset
        }

        MenuType menuType = MenuType.None;

        const float LowSpeed = 1f;

        const float HighSpeed = 6f;

        const int RenderTextureScale = 1;

        private GameObject goSubCam;

        private GameObject goSubLight;

        private GameObject goSubScreen;

        private GameObject goSsLight;

        private GameObject goSsFrontFilter;

        private GameObject goMoveTarget;

        private RenderTexture rTex;

        private Maid maid;

        private SubScreenParam ssParam;

        private PixelValues pv;

        private Rect winRect;

        private Vector2 lastScreenSize;

        private Vector2 scrollViewVector = Vector2.zero;

        private float guiWidth = 0.25f;

        private bool xmlLoaded;

        private bool bsEnable = false;

        private bool screenCreated = false;

        private string currentBg = null;

        private string presetName = "";

        private YotogiManager yotogiManager;

        private Dictionary<string, SSPreset> presets;

        private Dictionary<string, string> scenePresets;

        Dictionary<string, UIButton[]> uiOnOffButton = new Dictionary<string, UIButton[]>();
        Dictionary<string, UIButton> uiCommandButton = new Dictionary<string, UIButton>();
        Dictionary<string, Dictionary<string, UILabel>> uiValueLable = new Dictionary<string, Dictionary<string, UILabel>>();

        Dictionary<string, float> currentValues = new Dictionary<string, float>();
        private string currentYotogiName;

        private class SubScreenParam
        {
            public readonly string DefMatchPattern = @"([-+]?[0-9]*\.?[0-9]+)";
            public readonly string XmlFileName = Directory.GetCurrentDirectory() + @"\UnityInjector\Config\SubScreenParam.xml";

            public string XmlFormat;
            public KeyCode toggleKey = KeyCode.Pause;
            public bool autoPreset = false;
            public bool sub_position = false;
            public List<string> sKey = new List<string>();

            public Dictionary<string, bool> bEnabled = new Dictionary<string, bool>();
            public Dictionary<string, string> sDescription = new Dictionary<string, string>();
            public Dictionary<string, string> sType = new Dictionary<string, string>();
            public Dictionary<string, bool> bVisible = new Dictionary<string, bool>();

            public Dictionary<string, string[]> sPropName = new Dictionary<string, string[]>();
            public Dictionary<string, Dictionary<string, float>> fValue = new Dictionary<string, Dictionary<string, float>>();
            public Dictionary<string, Dictionary<string, float>> fVmin = new Dictionary<string, Dictionary<string, float>>();
            public Dictionary<string, Dictionary<string, float>> fVmax = new Dictionary<string, Dictionary<string, float>>();
            public Dictionary<string, Dictionary<string, float>> fVdef = new Dictionary<string, Dictionary<string, float>>();
            public Dictionary<string, Dictionary<string, string>> sVType = new Dictionary<string, Dictionary<string, string>>();
            public Dictionary<string, Dictionary<string, string>> sLabel = new Dictionary<string, Dictionary<string, string>>();
            public Dictionary<string, Dictionary<string, string>> sMatchPattern = new Dictionary<string, Dictionary<string, string>>();
            public Dictionary<string, Dictionary<string, bool>> bVVisible = new Dictionary<string, Dictionary<string, bool>>();

            public int KeyCount
            { get { return sKey.Count; } }
            public int ValCount(string key)
            { return sPropName[key].Length; }

            //--------

            public SubScreenParam()
            {
                Init();
            }

            public bool Init()
            {
                if (!loadXML())
                {
                    SubScreen.ErrorLog("loadXML() failed.");
                    return false;
                }

                return true;
            }

            public bool IsToggle(string key)
            {
                return (sType[key] == "toggle") ? true : false;
            }

            public bool IsButton(string key)
            {
                return (sType[key] == "button") ? true : false;
            }

            public bool IsCheck(string key)
            {
                return (sType[key] == "check") ? true : false;
            }

            //--------

            private bool loadXML()
            {
                if (!File.Exists(XmlFileName))
                {
                    SubScreen.ErrorLog(XmlFileName, "not exist.");
                    return false;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(XmlFileName);

                XmlNode mods = doc.DocumentElement;
                XmlFormat = ((XmlElement)mods).GetAttribute("format");
                if (XmlFormat != "1.0")
                {
                    SubScreen.ErrorLog(SubScreen.Version, " requires fomart=\"1.0\" of SubScreenParam.xml.");
                    return false;
                }
                string keyAttr = ((XmlElement)mods).GetAttribute("toggleKey");
                if (keyAttr != null && !keyAttr.Equals(""))
                {
                    foreach (string keyName in Enum.GetNames(typeof(KeyCode)))
                    {
                        if (keyAttr.Equals(keyName))
                        {
                            toggleKey = (KeyCode)Enum.Parse(typeof(KeyCode), keyAttr);
                        }
                    }
                }

                string ap = ((XmlElement)mods).GetAttribute("autoPreset");
                if (ap != null && ap.ToLower().Equals("true"))
                {
                    autoPreset = true;
                    DebugLog("autoPreset", "enabled");
                }
                ap = ((XmlElement)mods).GetAttribute("sub_position");
                if (ap != null && ap.ToLower().Equals("new"))
                {
                    sub_position = true;
                }
                DebugLog("toggleKey", Enum.GetName(typeof(KeyCode), toggleKey));
                XmlNodeList modNodeS = mods.SelectNodes("/mods/mod");
                if (!(modNodeS.Count > 0))
                {
                    SubScreen.ErrorLog(XmlFileName, " has no <mod>elements.");
                    return false;
                }

                sKey.Clear();

                foreach (XmlNode modNode in modNodeS)
                {
                    // mod属性
                    string key = ((XmlElement)modNode).GetAttribute("id");
                    if (key != "" && !sKey.Contains(key))
                        sKey.Add(key);
                    else
                        continue;

                    bool b = false;
                    bEnabled[key] = false;
                    sDescription[key] = ((XmlElement)modNode).GetAttribute("description");
                    bVisible[key] = (Boolean.TryParse(((XmlElement)modNode).GetAttribute("visible"), out b)) ? b : true;

                    sType[key] = ((XmlElement)modNode).GetAttribute("type");

                    if (sType[key] == "")
                        sType[key] = "slider";
                    if (IsCheck(key))
                        continue;
                    if (IsButton(key))
                        continue;

                    XmlNodeList valueNodeS = ((XmlElement)modNode).GetElementsByTagName("value");
                    if (!(valueNodeS.Count > 0))
                        continue;

                    sPropName[key] = new string[valueNodeS.Count];
                    fValue[key] = new Dictionary<string, float>();
                    fVmin[key] = new Dictionary<string, float>();
                    fVmax[key] = new Dictionary<string, float>();
                    fVdef[key] = new Dictionary<string, float>();
                    sVType[key] = new Dictionary<string, string>();
                    sLabel[key] = new Dictionary<string, string>();
                    sMatchPattern[key] = new Dictionary<string, string>();
                    bVVisible[key] = new Dictionary<string, bool>();

                    // value属性
                    int j = 0;
                    foreach (XmlNode valueNode in valueNodeS)
                    {
                        float x = 0f;

                        string prop = ((XmlElement)valueNode).GetAttribute("prop_name");
                        if (prop != "" && Array.IndexOf(sPropName[key], prop) < 0)
                        {
                            sPropName[key][j] = prop;
                        }
                        else
                        {
                            sKey.Remove(key);
                            break;
                        }

                        sVType[key][prop] = ((XmlElement)valueNode).GetAttribute("type");
                        switch (sVType[key][prop])
                        {
                            case "num":
                                break;
                            case "scale":
                                break;
                            case "int":
                                break;
                            case "button":
                                break;
                            default:
                                sVType[key][prop] = "num";
                                break;
                        }

                        fVmin[key][prop] = Single.TryParse(((XmlElement)valueNode).GetAttribute("min"), out x) ? x : 0f;
                        fVmax[key][prop] = Single.TryParse(((XmlElement)valueNode).GetAttribute("max"), out x) ? x : 0f;
                        fVdef[key][prop] = Single.TryParse(((XmlElement)valueNode).GetAttribute("default"), out x) ? x : (sVType[key][prop] == "scale") ? 1f : 0f;
                        fValue[key][prop] = fVdef[key][prop];

                        sLabel[key][prop] = ((XmlElement)valueNode).GetAttribute("label");
                        sMatchPattern[key][prop] = ((XmlElement)valueNode).GetAttribute("match_pattern");
                        bVVisible[key][prop] = (Boolean.TryParse(((XmlElement)valueNode).GetAttribute("visible"), out b)) ? b : true;

                        j++;
                    }
                    if (j == 0)
                        sKey.Remove(key);
                }

                return true;
            }
        }

        private class PixelValues
        {
            public float BaseWidth = 1280f;
            public float PropRatio = 0.6f;
            public int Margin;

            private Dictionary<string, int> font = new Dictionary<string, int>();
            private Dictionary<string, int> line = new Dictionary<string, int>();
            private Dictionary<string, int> sys = new Dictionary<string, int>();

            public PixelValues()
            {
                Margin = PropPx(10);

                font["C1"] = 11;
                font["C2"] = 12;
                font["H1"] = 14;
                font["H2"] = 16;
                font["H3"] = 20;

                line["C1"] = 14;
                line["C2"] = 18;
                line["H1"] = 22;
                line["H2"] = 24;
                line["H3"] = 30;

                sys["Menu.Height"] = 45;
                sys["OkButton.Height"] = 95;

                sys["HScrollBar.Width"] = 15;
            }

            public int Font(string key)
            { return PropPx(font[key]); }
            public int Line(string key)
            { return PropPx(line[key]); }
            public int Sys(string key)
            { return PropPx(sys[key]); }

            public int Font_(string key)
            { return font[key]; }
            public int Line_(string key)
            { return line[key]; }
            public int Sys_(string key)
            { return sys[key]; }

            public Rect PropScreen(float left, float top, float width, float height)
            {
                return new Rect((int)((Screen.width - Margin * 2) * left + Margin)
                               , (int)((Screen.height - Margin * 2) * top + Margin)
                               , (int)((Screen.width - Margin * 2) * width)
                               , (int)((Screen.height - Margin * 2) * height));
            }

            public Rect PropScreenMH(float left, float top, float width, float height)
            {
                Rect r = PropScreen(left, top, width, height);
                r.y += Sys("Menu.Height");
                r.height -= (Sys("Menu.Height") + Sys("OkButton.Height"));

                return r;
            }

            public Rect PropScreenMH(float left, float top, float width, float height, Vector2 last)
            {
                Rect r = PropScreen((float)(left / (last.x - Margin * 2)), (float)(top / (last.y - Margin * 2)), width, height);
                r.height -= (Sys("Menu.Height") + Sys("OkButton.Height"));

                return r;
            }

            public Rect InsideRect(Rect rect)
            {
                return new Rect(Margin, Margin, rect.width - Margin * 2, rect.height - Margin * 2);
            }

            public Rect InsideRect(Rect rect, int height)
            {
                return new Rect(Margin, Margin, rect.width - Margin * 2, height);
            }

            public int PropPx(int px)
            {
                return (int)(px * (1f + (Screen.width / BaseWidth - 1f) * PropRatio));
            }
        }

        private void Awake()
        {
            string dataPath = Application.dataPath;
            isChubLip = dataPath.Contains("CM3D2OH");

			isVR = dataPath.Contains("VRx64");

            ssParam = new SubScreenParam();
            pv = new PixelValues();
            lastScreenSize = new Vector2(Screen.width, Screen.height);
            currentBg = GameMain.Instance.BgMgr.GetBGName();
        }

        private void OnLevelWasLoaded(int level)
        {
            if ((!isChubLip && !Enum.IsDefined(typeof(TargetLevel), level)) || (isChubLip && !Enum.IsDefined(typeof(TargetLevelCbl), level)) )
            {
                return;
            }
            menuType = MenuType.None;
            bsEnable = false;
            screenCreated = false;
            currentYotogiName = "";
            xmlLoaded = ssParam.Init();
            winRect = pv.PropScreenMH(1f - guiWidth, 0f, guiWidth, 1f);
            createScreen();
            if (!isChubLip && level == (int)TargetLevel.SceneYotogi)
            {
                yotogiManager = GameObject.Find("YotogiManager").GetComponent<YotogiManager>();
            }

            if (ssParam.autoPreset)
            {
                loadPresetXml();
                string key = generateSceneKey(level.ToString(), currentBg, "");
                DebugLog("autoPresetTarget", key);
                if (scenePresets != null && scenePresets.ContainsKey(key) && presets.ContainsKey(scenePresets[key]))
                {
                    SetPreset(presets[scenePresets[key]]);
                }
            }
        }

        private void Update()
        {
            if ((!isChubLip && !Enum.IsDefined(typeof(TargetLevel), Application.loadedLevel))
              || (isChubLip && !Enum.IsDefined(typeof(TargetLevelCbl), Application.loadedLevel)))
            {
                if (menuType == MenuType.Main)
                {
                    menuType = MenuType.None;
                }
                return;
            }

            if (!currentBg.Equals(GameMain.Instance.BgMgr.GetBGName()))
            {
                DebugLog("BG changed", currentBg + " >> " + GameMain.Instance.BgMgr.GetBGName());
                currentBg = GameMain.Instance.BgMgr.GetBGName();
                if (ssParam.autoPreset)
                {
                    this.setScenePreset();
                }
            }
            if (ssParam.autoPreset && !isChubLip && Application.loadedLevel == (int)TargetLevel.SceneYotogi)
            {
                this.detectSkill();

            } else if (ssParam.autoPreset && isChubLip && Application.loadedLevel == (int)TargetLevelCbl.SceneYotogiWithChubLip){
                // Chu-B Lipの場合
                this.detectSkillCbl();
            }
            else
            {
                currentYotogiName = "";
            }

            if (ssParam != null)
            {
                if (menuType == MenuType.Main)
                {
                    if (winRect.Contains(Input.mousePosition))
                    {
                        GameMain.Instance.MainCamera.SetControl(false);
                    }
                    else
                    {
                        GameMain.Instance.MainCamera.SetControl(true);
                    }
                }
                if (Input.GetKeyDown(ssParam.toggleKey))
                {
                    if (menuType == MenuType.None)
                    {
                        menuType = MenuType.Main;
                    }
                    else
                    {
                        menuType = MenuType.None;
                    }
                }

                if (bsEnable && screenCreated)
                {
                    maid = GameMain.Instance.CharacterMgr.GetMaid(0);
                    if (maid == null)
                        return;

                    InputCheck();
                    showSubCamera();
                    applyScreen();
                   
                }
                if(maid != null){
	                eyeMove();
	            }
            }

        }

        private void detectSkill() {

            foreach (YotogiManager.PlayingSkillData skillData in yotogiManager.play_skill_array.Reverse())
            {
                if (skillData.is_play)
                {
                    var yotogiName = skillData.skill_pair.base_data.name;
                    if (currentYotogiName == null || !yotogiName.Equals(currentYotogiName))
                    {
                        DebugLog("Yotogi changed", currentYotogiName + " >> " + yotogiName);
                        currentYotogiName = yotogiName;
                        this.setScenePreset();
                    }
                    break;
                }
            }
        }

        private void detectSkillCbl() {

            var skillData = YotogiPlayManagerWithChubLip.GetSelectSkillData();
            if (skillData != null) {
                var yotogiName = skillData.skill_name;
                if (currentYotogiName == null || !yotogiName.Equals(currentYotogiName))
                {
                    DebugLog("Yotogi changed", currentYotogiName + " >> " + yotogiName);
                    currentYotogiName = yotogiName;
                    this.setScenePreset();
                }
            }
        }

        private void setScenePreset() {

            string key = generateSceneKey(Application.loadedLevel.ToString(), currentBg, currentYotogiName);
            if (scenePresets.ContainsKey(key) && presets.ContainsKey(scenePresets[key]))
            {
                SetPreset(presets[scenePresets[key]]);
            }
        }

        private void showSubCamera()
        {
            if (goSubCam != null)
            {

                if (ssParam.bEnabled[PKeyEnable])
                {
                    SetRendererEnabled(goSubCam, true);
                    goSubCam.GetComponent<Camera>().enabled = true;
                    if (ssParam.bEnabled[PKeySubCamera])
                    {
                        goSubCam.GetComponent<Camera>().targetTexture = null;
                        SetRendererEnabled(goSubScreen, false);
                        goSsLight.GetComponent<Light>().enabled = false;
                        SetRendererEnabled(goSsFrontFilter, false);
                    }
                    else
                    {
                        goSubCam.GetComponent<Camera>().targetTexture = rTex;
                        SetRendererEnabled(goSubScreen, true);
                        goSsLight.GetComponent<Light>().enabled = true;
                    }
                    if (ssParam.bEnabled[PKeyAlwaysLookAtFace])
                    {
                        Transform tr = maid.body0.trsHead;
                        goSubCam.transform.position = tr.position;
                        goSubCam.transform.position += tr.TransformDirection(Vector3.up) * ssParam.fValue[PKeyAlwaysLookAtFace][PPropLookAtFaceFront];//前後
                        goSubCam.transform.position += tr.TransformDirection(Vector3.left) * ssParam.fValue[PKeyAlwaysLookAtFace][PPropLookAtFaceUp];//上下
                        goSubCam.transform.position += tr.TransformDirection(Vector3.back) * ssParam.fValue[PKeyAlwaysLookAtFace][PPropLookAtFaceLeft];//左右
                        goSubCam.transform.LookAt(tr);
                    }
                    else if (ssParam.bEnabled[PKeyAlwaysLookAtXxx])
                    {
                        Transform tr = maid.body0.Pelvis.transform;
                        goSubCam.transform.position = tr.position;
                        goSubCam.transform.position += tr.TransformDirection(Vector3.up) * ssParam.fValue[PKeyAlwaysLookAtXxx][PPropLookAtXxxFront];//前後
                        goSubCam.transform.position += tr.TransformDirection(Vector3.left) * ssParam.fValue[PKeyAlwaysLookAtXxx][PPropLookAtXxxUp];//上下
                        goSubCam.transform.position += tr.TransformDirection(Vector3.back) * ssParam.fValue[PKeyAlwaysLookAtXxx][PPropLookAtXxxLeft];//左右
                        goSubCam.transform.LookAt(tr.position);
                    }
                    else if (ssParam.bEnabled[PKeyAlwaysLookAtMaid])
                    {
                        goSubCam.transform.LookAt(maid.body0.trsHead.transform);
                    }
                    else if (ssParam.bEnabled[PKeySubCamera])
                    {
                        goSubCam.transform.position = new Vector3(ssParam.fValue[PKeySubCamera][PPropSubCameraLocX],
                                                                  ssParam.fValue[PKeySubCamera][PPropSubCameraLocY],
                                                                  ssParam.fValue[PKeySubCamera][PPropSubCameraLocZ]);
                        goSubCam.transform.eulerAngles = new Vector3(ssParam.fValue[PKeySubCamera][PPropSubCameraAngX],
                                                                     ssParam.fValue[PKeySubCamera][PPropSubCameraAngY],
                                                                     ssParam.fValue[PKeySubCamera][PPropSubCameraAngZ]);
                    }
                    if (ssParam.bEnabled[PKeyAlwaysScreenOnMainCamera])
                    {
                        Transform tr = GameMain.Instance.MainCamera.transform;
                        goSubScreen.transform.position = tr.position;
						float offsetSubScreen = 1f;
						if (isVR) {
							offsetSubScreen = 3f;
						}
						goSubScreen.transform.position += tr.TransformDirection(Vector3.forward) * ssParam.fValue[PKeyAlwaysScreenOnMainCamera][PPropAlwaysScreenOnMainCameraFront] / offsetSubScreen;
                        goSubScreen.transform.LookAt(tr);
						goSubScreen.transform.position += goSubScreen.transform.TransformDirection(Vector3.up) * ssParam.fValue[PKeyAlwaysScreenOnMainCamera][PPropAlwaysScreenOnMainCameraUp] / offsetSubScreen;
						goSubScreen.transform.position += goSubScreen.transform.TransformDirection(Vector3.left) * ssParam.fValue[PKeyAlwaysScreenOnMainCamera][PPropAlwaysScreenOnMainCameraLeft] / offsetSubScreen;
                    }
                }
                else
                {
                    SetRendererEnabled(goSubCam, false);
                    goSubCam.GetComponent<Camera>().enabled = false;
                    SetRendererEnabled(goSubScreen, false);
                    SetRendererEnabled(goSsFrontFilter, false);
                }
            }
        }

        private void InputCheck()
        {
            var speed = LowSpeed;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = HighSpeed;
            }

            if (Input.GetKeyUp (KeyCode.V)) {
                isCameraToggle = !isCameraToggle;
            }
            goMoveTarget = (isCameraToggle) ? goSubScreen : goSubCam;

            if(Input.GetKeyUp (KeyCode.M)) {
                presetName = currentBg + "_" + currentYotogiName;
                DebugLog("Memory quick scene preset. saved", presetName);
                SavePreset(presetName);
                SaveScenePreset(presetName);
            }

            if ((!isVR && Input.GetKey(KeyCode.W)) || (isVR && Input.GetKey(KeyCode.I)))
            {
                if (goMoveTarget.GetComponent<Renderer>().enabled)
                {
                    if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                    {
                        goMoveTarget.transform.Rotate(new Vector3(speed * Time.deltaTime * -20f, 0f, 0f));
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleX] = goSubScreen.transform.eulerAngles.x;
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleY] = goSubScreen.transform.eulerAngles.y;
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleZ] = goSubScreen.transform.eulerAngles.z;
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        goMoveTarget.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                    else
                    {
                        goMoveTarget.transform.position += goMoveTarget.transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                }
            }
            else if ((!isVR && Input.GetKey(KeyCode.S)) || (isVR && Input.GetKey(KeyCode.K)))
            {
                if (goMoveTarget.GetComponent<Renderer>().enabled)
                {
                    if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                    {
                        goMoveTarget.transform.Rotate(new Vector3(speed * Time.deltaTime * 20f, 0f, 0f));
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleX] = goSubScreen.transform.eulerAngles.x;
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleY] = goSubScreen.transform.eulerAngles.y;
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleZ] = goSubScreen.transform.eulerAngles.z;
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        goMoveTarget.transform.position += new Vector3(0, 0, speed * Time.deltaTime);
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                    else
                    {
                        goMoveTarget.transform.position += goMoveTarget.transform.TransformDirection(Vector3.back) * speed * Time.deltaTime;
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                }
            }
            else if ((!isVR && Input.GetKey(KeyCode.A)) || (isVR && Input.GetKey(KeyCode.J)))
            {
                if (goMoveTarget.GetComponent<Renderer>().enabled)
                {
                    if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                    {
                        goMoveTarget.transform.Rotate(new Vector3(0f, speed * Time.deltaTime * -20f, 0f));
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleX] = goSubScreen.transform.eulerAngles.x;
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleY] = goSubScreen.transform.eulerAngles.y;
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleZ] = goSubScreen.transform.eulerAngles.z;
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        goMoveTarget.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                    else
                    {
                        goMoveTarget.transform.position += goMoveTarget.transform.TransformDirection(Vector3.left) * speed * Time.deltaTime;
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                }
            }
            else if ((!isVR && Input.GetKey(KeyCode.D)) || (isVR && Input.GetKey(KeyCode.L)))
            {
                if (goMoveTarget.GetComponent<Renderer>().enabled)
                {
                    if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                    {
                        goMoveTarget.transform.Rotate(new Vector3(0f, speed * Time.deltaTime * 20f, 0f));
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleX] = goSubScreen.transform.eulerAngles.x;
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleY] = goSubScreen.transform.eulerAngles.y;
                            ssParam.fValue[PKeyBSAngle][PPropBSAngleZ] = goSubScreen.transform.eulerAngles.z;
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        goMoveTarget.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                    else
                    {
                        goMoveTarget.transform.position += goMoveTarget.transform.TransformDirection(Vector3.right) * speed * Time.deltaTime;
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                }
            }
            else if ((!isVR && Input.GetKey(KeyCode.Q)) || (isVR && Input.GetKey(KeyCode.U)))
            {
                if (goMoveTarget.GetComponent<Renderer>().enabled)
                {
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        goMoveTarget.transform.position -= new Vector3(0f, speed * Time.deltaTime, 0f);
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                    else
                    {
                        goMoveTarget.transform.position += goMoveTarget.transform.TransformDirection(Vector3.down) * speed * Time.deltaTime;
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                }
            }
            else if ((!isVR && Input.GetKey(KeyCode.E)) || (isVR && Input.GetKey(KeyCode.O)))
            {
                if (goMoveTarget.GetComponent<Renderer>().enabled)
                {
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        goMoveTarget.transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                    else
                    {
                        goMoveTarget.transform.position += goMoveTarget.transform.TransformDirection(Vector3.up) * speed * Time.deltaTime;
                        if(isCameraToggle) {
                            ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                            ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
                        }
                    }
                }
            }
        }

        private void createScreen()
        {
            DebugLog("createScreen", "start");
            goSubScreen = GameObject.CreatePrimitive(PrimitiveType.Cube);
            goSubScreen.layer = SubScreenLayer;
            goSsFrontFilter = GameObject.CreatePrimitive(PrimitiveType.Cube);
            SetRendererEnabled(goSsFrontFilter, false);
            goSsFrontFilter.layer = SubScreenLayer;
            goSsFrontFilter.transform.SetParent(goSubScreen.transform);
            goSsFrontFilter.transform.localScale = new Vector3(1f, 1f, 0.001f);
            goSsFrontFilter.transform.localPosition += new Vector3(0, 0, 0.001f);
            goSsFrontFilter.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            goSubScreen.transform.localScale = new Vector3(1f, 1f, 0.01f);
            goSsLight = new GameObject();
            goSsLight.transform.localPosition += new Vector3(0, 0.5f, 0.5f);
            Light ssLight = goSsLight.AddComponent<Light>();
            ssLight.enabled = false;
            ssLight.GetComponent<Light>().type = LightType.Directional;
            ssLight.GetComponent<Light>().transform.LookAt(goSubScreen.transform);
            ssLight.GetComponent<Light>().cullingMask &= ~(1 << LayerMask.NameToLayer("Default"));
            ssLight.GetComponent<Light>().cullingMask &= ~(1 << LayerMask.NameToLayer("BackGround"));
            ssLight.GetComponent<Light>().cullingMask &= ~(1 << LayerMask.NameToLayer("Charactor"));
            ssLight.intensity = 0.5f;
            goSsLight.transform.SetParent(goSubScreen.transform);
            SetRendererEnabled(goSubScreen, false);

            goSubCam = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            SetRendererEnabled(goSubCam, false);
            goSubCam.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            Camera cam = goSubCam.AddComponent<Camera>();
            cam.cullingMask &= ~(1 << SubScreenLayer);
            cam.enabled = false;
            cam.depth = 9;
            cam.rect = new Rect(0f, 0f, 1f, 1f);
            goSubLight = new GameObject("sub light");
            goSubLight.AddComponent<Light>();
            goSubLight.transform.SetParent(goSubCam.transform);
            goSubLight.GetComponent<Light>().type = LightType.Spot;
            goSubLight.GetComponent<Light>().range = 10;
            goSubLight.GetComponent<Light>().enabled = false;
            goSubLight.GetComponent<Light>().cullingMask &= ~(1 << SubScreenLayer);

            rTex = new RenderTexture(
                Screen.width / RenderTextureScale,
                Screen.height / RenderTextureScale,
                24);
            cam.targetTexture = rTex;

            goSubScreen.GetComponent<Renderer>().material.mainTexture = rTex;
            goSubCam.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            goSubScreen.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

            Transform mainTransform = GameMain.Instance.MainCamera.transform;
            goSubCam.transform.position = mainTransform.position;
            goSubCam.transform.eulerAngles = mainTransform.eulerAngles;

            StartCoroutine(SetLocalTexture
                 (goSubCam, Application.dataPath + "/../UnityInjector/Config/SubScreenCamera.png"));
            StartCoroutine(SetLocalTexture
                 (goSsFrontFilter, Application.dataPath + "/../UnityInjector/Config/SubScreenFilter.png"));

            screenCreated = true;
            DebugLog("createScreen", "end");
        }

        private System.Collections.IEnumerator SetLocalTexture(GameObject gameObject, string filePath)
        {
            DebugLog("SetLocalTexture:", filePath);
            WWW file = new WWW("file://" + filePath);
            yield return file;
            gameObject.GetComponent<Renderer>().material.mainTexture = file.texture;
        }

        public void onClickButton(String key)
        {
            DebugLog("onClickButton", key);
            if (key.Equals(PKeyLookAtMaid))
            {
                goSubCam.GetComponent<Camera>().transform.LookAt(maid.body0.trsHead.transform);
            }
            else if (key.Equals(PKeyResetCameraPos))
            {
                Transform mainTransform = GameMain.Instance.MainCamera.transform;
                Vector3 vec = maid.transform.position;
                vec.y = goSubScreen.transform.position.y;
                goSubScreen.transform.LookAt(vec);
                goSubCam.transform.position = new Vector3(mainTransform.position.x, mainTransform.position.y, mainTransform.position.z);
                goSubCam.transform.LookAt(maid.body0.trsHead.transform);
            }
            else if (key.Equals(PKeyMoveToBack))
            {
                Vector3 vec = GameMain.Instance.MainCamera.GetTargetPos();
                vec.y = 1.3f;
                vec.z -= 1.3f;
                goSubScreen.transform.position = vec;
                ssParam.fValue[PKeyBSPos][PPropBSPosX] = goSubScreen.transform.position.x;
                ssParam.fValue[PKeyBSPos][PPropBSPosY] = goSubScreen.transform.position.y;
                ssParam.fValue[PKeyBSPos][PPropBSPosZ] = goSubScreen.transform.position.z;
            }
        }

        public void applyScreen()
        {
            if (ssParam.bEnabled[PKeySubCamera])
            {
                goSubCam.GetComponent<Camera>().rect = new Rect(ssParam.fValue[PKeySubCamera][PPropSubCameraPosX], ssParam.fValue[PKeySubCamera][PPropSubCameraPosY],
                    ssParam.fValue[PKeySubCamera][PPropSubCameraWidth], ssParam.fValue[PKeySubCamera][PPropSubCameraWidth]);
            }
            else
            {
                goSubCam.GetComponent<Camera>().rect = new Rect(0, 0, 1f, 1f);
            }

            if (!ssParam.bEnabled[PKeyAlwaysScreenOnMainCamera])
            {
                goSubScreen.transform.position = new Vector3(
                ssParam.fValue[PKeyBSPos][PPropBSPosX],
                ssParam.fValue[PKeyBSPos][PPropBSPosY],
                ssParam.fValue[PKeyBSPos][PPropBSPosZ]);

                goSubScreen.transform.eulerAngles = new Vector3(ssParam.fValue[PKeyBSAngle][PPropBSAngleX],
                    ssParam.fValue[PKeyBSAngle][PPropBSAngleY], ssParam.fValue[PKeyBSAngle][PPropBSAngleZ]);
            }
            var x = ssParam.fValue[PKeyBSSize][PPropBSSize];
            var y = x * Screen.height / Screen.width;
            goSubScreen.transform.localScale = new Vector3(x, y, goSubScreen.transform.localScale.z);
            Color color = goSubScreen.GetComponent<Renderer>().material.color;
            color.r = ssParam.fValue[PKeyBSColor][PPropBSColorRed] * ssParam.fValue[PKeyBSColor][PPropBSColorLuminance];
            color.g = ssParam.fValue[PKeyBSColor][PPropBSColorGreen] * ssParam.fValue[PKeyBSColor][PPropBSColorLuminance];
            color.b = ssParam.fValue[PKeyBSColor][PPropBSColorBlue] * ssParam.fValue[PKeyBSColor][PPropBSColorLuminance];
            color.a = ssParam.fValue[PKeyBSColor][PPropBSColorAlpha];
            goSubScreen.GetComponent<Renderer>().material.color = color;
			float offsetLight = 1f;
			if(isVR) {
				offsetLight = 2f;
			}
			goSsLight.GetComponent<Light>().intensity = ssParam.fValue[PKeyBSColor][PPropScreenLightLuminance] * offsetLight;
            if (ssParam.fValue[PKeyBSColor][PPropScreenLightLuminance] > 0)
            {
                GameMain.Instance.MainLight.GetComponent<Light>().cullingMask &= ~(1 << SubScreenLayer);
            }
            else
            {
                GameMain.Instance.MainLight.GetComponent<Light>().cullingMask |= (1 << SubScreenLayer);
            }

            color = goSubCam.GetComponent<Renderer>().material.color;
            color.r = ssParam.fValue[PKeyCameraColor][PPropCameraColorRed] * ssParam.fValue[PKeyBSColor][PPropBSColorLuminance];
            color.g = ssParam.fValue[PKeyCameraColor][PPropCameraColorGreen] * ssParam.fValue[PKeyBSColor][PPropBSColorLuminance];
            color.b = ssParam.fValue[PKeyCameraColor][PPropCameraColorBlue] * ssParam.fValue[PKeyBSColor][PPropBSColorLuminance];
            color.a = ssParam.fValue[PKeyCameraColor][PPropCameraColorAlpha];
            goSubCam.GetComponent<Renderer>().material.color = color;
            goSubCam.GetComponent<Camera>().fieldOfView = ssParam.fValue[PKeyCameraColor][PPropSubCameraFieldOfView];

            if (ssParam.bEnabled[PKeyMainLight])
            {
                Light mainLight = GameMain.Instance.MainLight.GetComponent<Light>();
                color = mainLight.color;
                color.r = ssParam.fValue[PKeyMainLight][PPropMainLightColorRed] * ssParam.fValue[PKeyMainLight][PPropMainLightLuminance];
                color.g = ssParam.fValue[PKeyMainLight][PPropMainLightColorGreen] * ssParam.fValue[PKeyMainLight][PPropMainLightLuminance];
                color.b = ssParam.fValue[PKeyMainLight][PPropMainLightColorBlue] * ssParam.fValue[PKeyMainLight][PPropMainLightLuminance];
                mainLight.color = color;
            }

            if (ssParam.bEnabled[PKeyEnable] && ssParam.bEnabled[PKeySubLight])
            {
                goSubLight.GetComponent<Light>().enabled = true;
                goSubLight.GetComponent<Light>().spotAngle = ssParam.fValue[PKeySubLight][PPropSubLightRange];
                color = goSubLight.GetComponent<Light>().color;
                color.r = ssParam.fValue[PKeySubLight][PPropSubLightColorRed] * ssParam.fValue[PKeySubLight][PPropSubLightLuminance];
                color.g = ssParam.fValue[PKeySubLight][PPropSubLightColorGreen] * ssParam.fValue[PKeySubLight][PPropSubLightLuminance];
                color.b = ssParam.fValue[PKeySubLight][PPropSubLightColorBlue] * ssParam.fValue[PKeySubLight][PPropSubLightLuminance];
                goSubLight.GetComponent<Light>().color = color;
            }
            else
            {
                goSubLight.GetComponent<Light>().enabled = false;
            }
            if (ssParam.bEnabled[PKeyEnable] && ssParam.bEnabled[PKeyScreenFilter])
            {
                SetRendererEnabled(goSsFrontFilter, true);
                color = goSsFrontFilter.GetComponent<Renderer>().material.color;
                color.r = ssParam.fValue[PKeyScreenFilter][PPropScreenFilterRed] * ssParam.fValue[PKeyScreenFilter][PPropScreenFilterLuminance];
                color.g = ssParam.fValue[PKeyScreenFilter][PPropScreenFilterGreen] * ssParam.fValue[PKeyScreenFilter][PPropScreenFilterLuminance];
                color.b = ssParam.fValue[PKeyScreenFilter][PPropScreenFilterBlue] * ssParam.fValue[PKeyScreenFilter][PPropScreenFilterLuminance];
                color.a = ssParam.fValue[PKeyScreenFilter][PPropScreenFilterAlpha];
                goSsFrontFilter.GetComponent<Renderer>().material.color = color;
            }
            else
            {
                SetRendererEnabled(goSsFrontFilter, false);
            }
        }

        public void OnGUI()
        {
            if ((!isChubLip && !Enum.IsDefined(typeof(TargetLevel), Application.loadedLevel)) || (isChubLip && !Enum.IsDefined(typeof(TargetLevelCbl), Application.loadedLevel)))
            {
                return;
            }
            if (menuType == MenuType.None)
                return;

            maid = GameMain.Instance.CharacterMgr.GetMaid(0);
            if (maid == null)
                return;

            GUIStyle winStyle = "box";
            winStyle.fontSize = pv.Font("C1");
            winStyle.alignment = TextAnchor.UpperRight;

            if (lastScreenSize != new Vector2(Screen.width, Screen.height))
            {
                winRect = pv.PropScreenMH(winRect.x, winRect.y, guiWidth, 1f, lastScreenSize);
                lastScreenSize = new Vector2(Screen.width, Screen.height);
            }

            switch (menuType)
            {
                case MenuType.LoadPreset:
                    winRect = GUI.Window(0, winRect, DoLoadPreset, SubScreen.Version, winStyle);
                    break;
                case MenuType.SavePreset:
                    winRect = GUI.Window(0, winRect, DoSavePreset, SubScreen.Version, winStyle);
                    break;
                case MenuType.RemovePreset:
                    winRect = GUI.Window(0, winRect, DoRemovePreset, SubScreen.Version, winStyle);
                    break;
                case MenuType.SaveScenePreset:
                    winRect = GUI.Window(0, winRect, DoSaveScenePreset, SubScreen.Version, winStyle);
                    break;
                case MenuType.Main:
                    winRect = GUI.Window(0, winRect, DoMainMenu, SubScreen.Version, winStyle);
                    if (!bsEnable && ssParam.bEnabled[PKeyEnable])
                    {
                        onClickButton(PKeyMoveToBack);
                    }
                    bsEnable = ssParam.bEnabled[PKeyEnable];
                    break;
            }

        }

        private void DoMainMenu(int winID)
        {
            int mod_num = ssParam.KeyCount;
            Rect baseRect = pv.InsideRect(this.winRect);
            Rect headerRect = new Rect(baseRect.x, baseRect.y, baseRect.width, pv.Line("H3"));
            Rect scrollRect = new Rect(baseRect.x, baseRect.y + headerRect.height + pv.Margin
                                      , baseRect.width + pv.PropPx(5), baseRect.height - headerRect.height - pv.Margin);
            Rect conRect = new Rect(0, 0, scrollRect.width - pv.Sys_("HScrollBar.Width") - pv.Margin, 0);
            Rect outRect = new Rect();
            GUIStyle lStyle = "label";
            GUIStyle tStyle = "toggle";
            GUIStyle bStyle = "button";
            Color color = new Color(1f, 1f, 1f, 0.98f);

            for (int i = 0; i < mod_num; i++)
            {
                string key = ssParam.sKey[i];

                conRect.height += pv.Line("H1");
                if (ssParam.sType[key] == "check")
                {
                    conRect.height += pv.Margin;
                }
                else if (ssParam.sType[key] == "button")
                {
                    conRect.height += pv.Line("H1") + pv.Margin;
                }
                else if (ssParam.sType[key] == "toggle" && !ssParam.bEnabled[key])
                {
                    conRect.height += pv.Margin;
                }
                else
                {
                    for (int j = 0; j < ssParam.ValCount(key); j++)
                        conRect.height += pv.Line("H1");
                    conRect.height += pv.Margin * 2;
                }
            }
            conRect.height += pv.Margin * 4;
            if (ssParam.autoPreset)
            {
                conRect.height += pv.Line("H1") * 2 + pv.Margin;
            }

            lStyle.normal.textColor = color;
            tStyle.normal.textColor = color;
            bStyle.normal.textColor = color;
            lStyle.fontSize = pv.Font("H3");
            bStyle.fontSize = pv.Font("H1");

            drawWinHeader(headerRect, "サブスクリーン", lStyle);

            // スクロールビュー
            scrollViewVector = GUI.BeginScrollView(scrollRect, scrollViewVector, conRect);

            // 各modスライダー
            outRect.Set(0, 0, conRect.width, 0);
            for (int i = 0; i < mod_num; i++)
            {
                string key = ssParam.sKey[i];

                //----
                outRect.width = conRect.width;
                outRect.height = pv.Line("H1");
                color = new Vector4(1f, 1f, 1f, 0.98f);
                lStyle.normal.textColor = color;
                tStyle.normal.textColor = color;

                tStyle.fontSize = pv.Font("H1");

                if (ssParam.sType[key] == "button")
                {
                    if (GUI.Button(outRect, ssParam.sDescription[key], bStyle))
                    {
                        onClickButton(key);
                    }
                    outRect.y += outRect.height;
                    outRect.y += pv.Margin;
                    continue;
                }

                if (ssParam.sType[key] == "check")
                {
                    ssParam.bEnabled[key] = GUI.Toggle(outRect, ssParam.bEnabled[key], ssParam.sDescription[key] + " (" + key + ")", tStyle);
                    outRect.y += outRect.height;
                    outRect.y += pv.Margin;
                    continue;
                }
                if (ssParam.sType[key] == "toggle")
                {
                    ssParam.bEnabled[key] = GUI.Toggle(outRect, ssParam.bEnabled[key], ssParam.sDescription[key] + " (" + key + ")", tStyle);
                    outRect.y += outRect.height;
                    outRect.y += pv.Margin;
                    if (!ssParam.bEnabled[key])
                        continue;
                }
                else
                {
                    // slider
                    GUI.Label(outRect, ssParam.sDescription[key], tStyle);
                    outRect.y += outRect.height;
                }
                int val_num = ssParam.ValCount(key);
                for (int j = 0; j < val_num; j++)
                {
                    string prop = ssParam.sPropName[key][j];

                    float value = ssParam.fValue[key][prop];
                    float vmin = ssParam.fVmin[key][prop];
                    float vmax = ssParam.fVmax[key][prop];
                    string label = ssParam.sLabel[key][prop] + " : ";
                    string vType = ssParam.sVType[key][prop];

                    outRect.width = conRect.width;
                    outRect.height = pv.Line("H1");
                    lStyle.fontSize = pv.Font("H1");
                    if (value < vmin)
                        value = vmin;
                    if (value > vmax)
                        value = vmax;
                    if (vType == "scale" && vmin < 1f)
                    {
                        if (vmin < 0f)
                            vmin = 0f;
                        if (value < 0f)
                            value = 0f;

                        float tmpmin = -Mathf.Abs(vmax - 1f);
                        float tmpmax = Mathf.Abs(vmax - 1f);
                        float tmp = (value < 1f) ? tmp = Mathf.Abs((1f - value) / (1f - vmin)) * tmpmin : value - 1f;

                        if (tmp < tmpmin)
                            tmp = tmpmin;
                        if (tmp > tmpmax)
                            tmp = tmpmax;

                        tmp = drawModValueSlider(outRect, tmp, tmpmin, tmpmax, label, lStyle);

                        ssParam.fValue[key][prop] = (tmp < 0f) ? 1f - tmp / tmpmin * Mathf.Abs(1f - vmin) : 1f + tmp;
                    }
                    else if (vType == "int")
                    {
                        value = (int)Mathf.Round(value);
                        ssParam.fValue[key][prop] = (int)Mathf.Round(drawModValueSlider(outRect, value, vmin, vmax, label, lStyle));
                    }
                    else
                        ssParam.fValue[key][prop] = drawModValueSlider(outRect, value, vmin, vmax, label, lStyle);

                    outRect.y += outRect.height;
                }

                outRect.y += pv.Margin * 2;
            }

            GUIStyle winStyle = "box";
            winStyle.fontSize = pv.Font("C1");
            winStyle.alignment = TextAnchor.UpperRight;

            if (GUI.Button(outRect, "プリセットの呼び出し", bStyle))
            {
                loadPresetXml();
                if (presets != null && presets.Count() > 0)
                {
                    menuType = MenuType.LoadPreset;
                }
            }
            outRect.y += outRect.height + pv.Margin;
            if (GUI.Button(outRect, "プリセットの削除", bStyle))
            {
                loadPresetXml();
                if (presets != null && presets.Count() > 0)
                {
                    menuType = MenuType.RemovePreset;
                }
            }
            outRect.y += outRect.height + pv.Margin;
            if (GUI.Button(outRect, "現在値をプリセットとして保存", bStyle))
            {
                loadPresetXml();
                menuType = MenuType.SavePreset;
            }
            outRect.y += outRect.height + pv.Margin;
            if (ssParam.autoPreset)
            {
                if (GUI.Button(outRect, "現在のシーンにプリセットを割り当て", bStyle))
                {
                    loadPresetXml();
                    menuType = MenuType.SaveScenePreset;
                }
                outRect.y += outRect.height + pv.Margin;
            }
            GUI.EndScrollView();
            GUI.DragWindow();
        }

        private void DoLoadPreset(int winId)
        {
            Rect baseRect = pv.InsideRect(this.winRect);
            Rect headerRect = new Rect(baseRect.x, baseRect.y, baseRect.width, pv.Line("H3"));
            Rect scrollRect = new Rect(baseRect.x, baseRect.y + headerRect.height + pv.Margin
                                      , baseRect.width + pv.PropPx(5), baseRect.height - headerRect.height - pv.Margin);
            Rect conRect = new Rect(0, 0, scrollRect.width - pv.Sys_("HScrollBar.Width") - pv.Margin, 0);
            Rect outRect = new Rect();
            outRect.width = conRect.width;
            outRect.height = pv.Line("H1");
            GUIStyle lStyle = "label";
            GUIStyle bStyle = "button";
            Color color = new Color(1f, 1f, 1f, 0.98f);
            lStyle.normal.textColor = color;
            lStyle.fontSize = pv.Font("H3");
            bStyle.normal.textColor = color;
            bStyle.fontSize = pv.Font("H1");

            drawWinHeader(headerRect, "プリセットから読み込み", lStyle);

            conRect.height += (outRect.height + pv.Margin) * (presets.Count() + 1);
            // スクロールビュー
            scrollViewVector = GUI.BeginScrollView(scrollRect, scrollViewVector, conRect);
            foreach (KeyValuePair<string, SSPreset> pair in presets)
            {
                if (GUI.Button(outRect, pair.Key, bStyle))
                {
                    SetPreset(pair.Value);
                }
                outRect.y += outRect.height + pv.Margin;
            }
            GUI.EndScrollView();
            outRect.x = pv.Margin;
            outRect.y = winRect.height - outRect.height - pv.Margin;
            if (GUI.Button(outRect, "閉じる", bStyle))
            {
                menuType = MenuType.Main;
            }
            GUI.DragWindow();
        }

        private void DoRemovePreset(int winId)
        {
            Rect baseRect = pv.InsideRect(this.winRect);
            Rect headerRect = new Rect(baseRect.x, baseRect.y, baseRect.width, pv.Line("H3"));
            Rect scrollRect = new Rect(baseRect.x, baseRect.y + headerRect.height + pv.Margin
                                      , baseRect.width + pv.PropPx(5), baseRect.height - headerRect.height - pv.Margin);
            Rect conRect = new Rect(0, 0, scrollRect.width - pv.Sys_("HScrollBar.Width") - pv.Margin, 0);
            Rect outRect = new Rect();
            outRect.width = conRect.width;
            outRect.height = pv.Line("H1");
            GUIStyle lStyle = "label";
            GUIStyle bStyle = "button";
            Color color = new Color(1f, 1f, 1f, 0.98f);
            lStyle.normal.textColor = color;
            lStyle.fontSize = pv.Font("H3");
            bStyle.normal.textColor = color;
            bStyle.fontSize = pv.Font("H1");

            drawWinHeader(headerRect, "プリセットを削除", lStyle);

            conRect.height += (outRect.height + pv.Margin) * (presets.Count() + 1);
            // スクロールビュー
            scrollViewVector = GUI.BeginScrollView(scrollRect, scrollViewVector, conRect);
            foreach (KeyValuePair<string, SSPreset> pair in presets)
            {
                if (GUI.Button(outRect, pair.Key, bStyle))
                {
                    RemovePreset(pair.Key);
                }
                outRect.y += outRect.height + pv.Margin;
            }
            GUI.EndScrollView();
            outRect.x = pv.Margin;
            outRect.y = winRect.height - outRect.height - pv.Margin;
            if (GUI.Button(outRect, "閉じる", bStyle))
            {
                menuType = MenuType.Main;
            }
            GUI.DragWindow();
        }

        private void SetPreset(SSPreset preset)
        {
            ssParam.bEnabled[PKeyEnable] = preset.dParams[PKeyEnable].enabled;
            bsEnable = ssParam.bEnabled[PKeyEnable];
            ssParam.bEnabled[PKeyAlwaysLookAtMaid] = preset.dParams[PKeyAlwaysLookAtMaid].enabled;

            ssParam.bEnabled[PKeyAlwaysLookAtFace] = preset.dParams[PKeyAlwaysLookAtFace].enabled;
            ssParam.fValue[PKeyAlwaysLookAtFace][PPropLookAtFaceUp] = preset.dParams[PKeyAlwaysLookAtFace].dValues[PPropLookAtFaceUp];
            ssParam.fValue[PKeyAlwaysLookAtFace][PPropLookAtFaceLeft] = preset.dParams[PKeyAlwaysLookAtFace].dValues[PPropLookAtFaceLeft];
            ssParam.fValue[PKeyAlwaysLookAtFace][PPropLookAtFaceFront] = preset.dParams[PKeyAlwaysLookAtFace].dValues[PPropLookAtFaceFront];

			ssParam.bEnabled[PKeyAlwaysLookAtXxx] = preset.dParams[PKeyAlwaysLookAtXxx].enabled;
            ssParam.fValue[PKeyAlwaysLookAtXxx][PPropLookAtXxxUp] = preset.dParams[PKeyAlwaysLookAtXxx].dValues[PPropLookAtXxxUp];
            ssParam.fValue[PKeyAlwaysLookAtXxx][PPropLookAtXxxLeft] = preset.dParams[PKeyAlwaysLookAtXxx].dValues[PPropLookAtXxxLeft];
            ssParam.fValue[PKeyAlwaysLookAtXxx][PPropLookAtXxxFront] = preset.dParams[PKeyAlwaysLookAtXxx].dValues[PPropLookAtXxxFront];

            ssParam.bEnabled[PKeyAlwaysScreenOnMainCamera] = preset.dParams[PKeyAlwaysScreenOnMainCamera].enabled;
            ssParam.fValue[PKeyAlwaysScreenOnMainCamera][PPropAlwaysScreenOnMainCameraUp] = preset.dParams[PKeyAlwaysScreenOnMainCamera].dValues[PPropAlwaysScreenOnMainCameraUp];
            ssParam.fValue[PKeyAlwaysScreenOnMainCamera][PPropAlwaysScreenOnMainCameraLeft] = preset.dParams[PKeyAlwaysScreenOnMainCamera].dValues[PPropAlwaysScreenOnMainCameraLeft];
            ssParam.fValue[PKeyAlwaysScreenOnMainCamera][PPropAlwaysScreenOnMainCameraFront] = preset.dParams[PKeyAlwaysScreenOnMainCamera].dValues[PPropAlwaysScreenOnMainCameraFront];

            ssParam.bEnabled[PKeySubCamera] = preset.dParams[PKeySubCamera].enabled;
            ssParam.fValue[PKeySubCamera][PPropSubCameraPosX] = preset.dParams[PKeySubCamera].dValues[PPropSubCameraPosX];
            ssParam.fValue[PKeySubCamera][PPropSubCameraPosY] = preset.dParams[PKeySubCamera].dValues[PPropSubCameraPosY];
            ssParam.fValue[PKeySubCamera][PPropSubCameraWidth] = preset.dParams[PKeySubCamera].dValues[PPropSubCameraWidth];
            try{
                ssParam.fValue[PKeySubCamera][PPropSubCameraLocX] = preset.dParams[PKeySubCamera].dValues[PPropSubCameraLocX];
                ssParam.fValue[PKeySubCamera][PPropSubCameraLocY] = preset.dParams[PKeySubCamera].dValues[PPropSubCameraLocY];
                ssParam.fValue[PKeySubCamera][PPropSubCameraLocZ] = preset.dParams[PKeySubCamera].dValues[PPropSubCameraLocZ];
                ssParam.fValue[PKeySubCamera][PPropSubCameraAngX] = preset.dParams[PKeySubCamera].dValues[PPropSubCameraAngX];
                ssParam.fValue[PKeySubCamera][PPropSubCameraAngY] = preset.dParams[PKeySubCamera].dValues[PPropSubCameraAngY];
                ssParam.fValue[PKeySubCamera][PPropSubCameraAngZ] = preset.dParams[PKeySubCamera].dValues[PPropSubCameraAngZ];
            }
            catch(KeyNotFoundException e){
                DebugLog("nyaaa");
            }


            ssParam.bEnabled[PKeyMainLight] = preset.dParams[PKeyMainLight].enabled;
            ssParam.fValue[PKeyMainLight][PPropMainLightLuminance] = preset.dParams[PKeyMainLight].dValues[PPropMainLightLuminance];
            ssParam.fValue[PKeyMainLight][PPropMainLightColorRed] = preset.dParams[PKeyMainLight].dValues[PPropMainLightColorRed];
            ssParam.fValue[PKeyMainLight][PPropMainLightColorGreen] = preset.dParams[PKeyMainLight].dValues[PPropMainLightColorGreen];
            ssParam.fValue[PKeyMainLight][PPropMainLightColorBlue] = preset.dParams[PKeyMainLight].dValues[PPropMainLightColorBlue];

            ssParam.bEnabled[PKeySubLight] = preset.dParams[PKeySubLight].enabled;
            ssParam.fValue[PKeySubLight][PPropSubLightRange] = preset.dParams[PKeySubLight].dValues[PPropSubLightRange];
            ssParam.fValue[PKeySubLight][PPropSubLightLuminance] = preset.dParams[PKeySubLight].dValues[PPropSubLightLuminance];
            ssParam.fValue[PKeySubLight][PPropSubLightColorRed] = preset.dParams[PKeySubLight].dValues[PPropSubLightColorRed];
            ssParam.fValue[PKeySubLight][PPropSubLightColorGreen] = preset.dParams[PKeySubLight].dValues[PPropSubLightColorGreen];
            ssParam.fValue[PKeySubLight][PPropSubLightColorBlue] = preset.dParams[PKeySubLight].dValues[PPropSubLightColorBlue];

            ssParam.fValue[PKeyCameraColor][PPropCameraColorRed] = preset.dParams[PKeyCameraColor].dValues[PPropCameraColorRed];
            ssParam.fValue[PKeyCameraColor][PPropCameraColorGreen] = preset.dParams[PKeyCameraColor].dValues[PPropCameraColorGreen];
            ssParam.fValue[PKeyCameraColor][PPropCameraColorBlue] = preset.dParams[PKeyCameraColor].dValues[PPropCameraColorBlue];
            ssParam.fValue[PKeyCameraColor][PPropCameraColorAlpha] = preset.dParams[PKeyCameraColor].dValues[PPropCameraColorAlpha];
            ssParam.fValue[PKeyCameraColor][PPropSubCameraFieldOfView] = preset.dParams[PKeyCameraColor].dValues[PPropSubCameraFieldOfView];

            ssParam.fValue[PKeyBSPos][PPropBSPosX] = preset.dParams[PKeyBSPos].dValues[PPropBSPosX];
            ssParam.fValue[PKeyBSPos][PPropBSPosY] = preset.dParams[PKeyBSPos].dValues[PPropBSPosY];
            ssParam.fValue[PKeyBSPos][PPropBSPosZ] = preset.dParams[PKeyBSPos].dValues[PPropBSPosZ];

            ssParam.fValue[PKeyBSSize][PPropBSSize] = preset.dParams[PKeyBSSize].dValues[PPropBSSize];

            ssParam.fValue[PKeyBSAngle][PPropBSAngleX] = preset.dParams[PKeyBSAngle].dValues[PPropBSAngleX];
            ssParam.fValue[PKeyBSAngle][PPropBSAngleY] = preset.dParams[PKeyBSAngle].dValues[PPropBSAngleY];
            ssParam.fValue[PKeyBSAngle][PPropBSAngleZ] = preset.dParams[PKeyBSAngle].dValues[PPropBSAngleZ];

            ssParam.fValue[PKeyBSColor][PPropScreenLightLuminance] = preset.dParams[PKeyBSColor].dValues[PPropScreenLightLuminance];
            ssParam.fValue[PKeyBSColor][PPropBSColorLuminance] = preset.dParams[PKeyBSColor].dValues[PPropBSColorLuminance];
            ssParam.fValue[PKeyBSColor][PPropBSColorRed] = preset.dParams[PKeyBSColor].dValues[PPropBSColorRed];
            ssParam.fValue[PKeyBSColor][PPropBSColorGreen] = preset.dParams[PKeyBSColor].dValues[PPropBSColorGreen];
            ssParam.fValue[PKeyBSColor][PPropBSColorBlue] = preset.dParams[PKeyBSColor].dValues[PPropBSColorBlue];
            ssParam.fValue[PKeyBSColor][PPropBSColorAlpha] = preset.dParams[PKeyBSColor].dValues[PPropBSColorAlpha];

            ssParam.bEnabled[PKeyScreenFilter] = preset.dParams[PKeyScreenFilter].enabled;
            ssParam.fValue[PKeyScreenFilter][PPropScreenFilterLuminance] = preset.dParams[PKeyScreenFilter].dValues[PPropScreenFilterLuminance];
            ssParam.fValue[PKeyScreenFilter][PPropScreenFilterRed] = preset.dParams[PKeyScreenFilter].dValues[PPropScreenFilterRed];
            ssParam.fValue[PKeyScreenFilter][PPropScreenFilterGreen] = preset.dParams[PKeyScreenFilter].dValues[PPropScreenFilterGreen];
            ssParam.fValue[PKeyScreenFilter][PPropScreenFilterBlue] = preset.dParams[PKeyScreenFilter].dValues[PPropScreenFilterBlue];
            ssParam.fValue[PKeyScreenFilter][PPropScreenFilterAlpha] = preset.dParams[PKeyScreenFilter].dValues[PPropScreenFilterAlpha];

            // 美しくないが、いい案思いつかなかった。ちかたないね
            // sub_positionにnewを指定した場合は、サブカメラのスライダー位置を反映
            if(goSubCam != null && ssParam.sub_position){
                goSubCam.transform.position = new Vector3(ssParam.fValue[PKeySubCamera][PPropSubCameraLocX],
                                                          ssParam.fValue[PKeySubCamera][PPropSubCameraLocY],
                                                          ssParam.fValue[PKeySubCamera][PPropSubCameraLocZ]);
                goSubCam.transform.eulerAngles = new Vector3(ssParam.fValue[PKeySubCamera][PPropSubCameraAngX],
                                                             ssParam.fValue[PKeySubCamera][PPropSubCameraAngY],
                                                             ssParam.fValue[PKeySubCamera][PPropSubCameraAngZ]);
             }

            DebugLog("SetPreset", preset.name);
        }

        private void DoSavePreset(int winId)
        {
            Rect baseRect = pv.InsideRect(this.winRect);
            Rect headerRect = new Rect(baseRect.x, baseRect.y, baseRect.width, pv.Line("H3"));
            Rect outRect = new Rect();
            outRect.width = baseRect.width;
            outRect.height = pv.Line("H1");
            outRect.x = pv.Margin;
            GUIStyle lStyle = "label";
            GUIStyle bStyle = "button";
            Color color = new Color(1f, 1f, 1f, 0.98f);
            lStyle.normal.textColor = color;
            lStyle.fontSize = pv.Font("H3");
            bStyle.normal.textColor = color;
            bStyle.fontSize = pv.Font("H1");

            drawWinHeader(headerRect, "プリセットとして保存", lStyle);
            outRect.y += headerRect.height + pv.Margin;
            outRect.width = baseRect.width * 0.3f;
            lStyle.fontSize = pv.Font("H1");
            GUI.Label(outRect, "プリセット名", lStyle);
            outRect.x += outRect.width;
            outRect.width = baseRect.width * 0.7f;

            lStyle.fontSize = pv.Font("H2");
            presetName = GUI.TextField(outRect, presetName);
            outRect.x = pv.Margin;
            outRect.y += outRect.height + pv.Margin;
            outRect.width = baseRect.width / 2 - pv.Margin;
            if (GUI.Button(outRect, "保存", bStyle))
            {
                SavePreset(presetName);
            }
            outRect.x += outRect.width + pv.Margin;
            if (GUI.Button(outRect, "閉じる", bStyle))
            {
                menuType = MenuType.Main;
            }

            GUI.DragWindow();
        }

        private void DoSaveScenePreset(int winId)
        {
            Rect baseRect = pv.InsideRect(this.winRect);
            Rect headerRect = new Rect(baseRect.x, baseRect.y, baseRect.width, pv.Line("H3"));
            Rect scrollRect = new Rect(baseRect.x, baseRect.y + headerRect.height + pv.Margin
                                      , baseRect.width + pv.PropPx(5), baseRect.height - headerRect.height - pv.Margin - pv.Line("H1") * 2);
            Rect conRect = new Rect(0, 0, scrollRect.width - pv.Sys_("HScrollBar.Width") - pv.Margin, 0);
            Rect outRect = new Rect();
            outRect.width = conRect.width;
            outRect.height = pv.Line("H1");
            GUIStyle lStyle = "label";
            GUIStyle bStyle = "button";
            Color color = new Color(1f, 1f, 1f, 0.98f);
            lStyle.normal.textColor = color;
            lStyle.fontSize = pv.Font("H3");
            bStyle.normal.textColor = color;
            bStyle.fontSize = pv.Font("H1");

            drawWinHeader(headerRect, "現在のシーンにプリセットを割り当て", lStyle);

            conRect.height += (outRect.height + pv.Margin) * (presets.Count() + 2);
            // スクロールビュー
            scrollViewVector = GUI.BeginScrollView(scrollRect, scrollViewVector, conRect);
            foreach (KeyValuePair<string, SSPreset> pair in presets)
            {
                if (GUI.Button(outRect, pair.Key, bStyle))
                {
                    SaveScenePreset(pair.Key);
                    menuType = MenuType.Main;
                }
                outRect.y += outRect.height + pv.Margin;
            }
            GUI.EndScrollView();
            outRect.x = pv.Margin;
            outRect.y = winRect.height - outRect.height * 2 - pv.Margin;
            if (GUI.Button(outRect, "割り当てを解除", bStyle))
            {
                RemoveScenePreset();
                menuType = MenuType.Main;
            }
            outRect.y = winRect.height - outRect.height - pv.Margin;
            if (GUI.Button(outRect, "閉じる", bStyle))
            {
                menuType = MenuType.Main;
            }
            GUI.DragWindow();
        }

        private void loadPresetXml()
        {
            if (!File.Exists(presetXmlFileName))
            {
                return;
            }
            var xdoc = XDocument.Load(presetXmlFileName);
            var presetNodes = xdoc.Descendants("preset");
            if (presetNodes.Count() == 0)
            {
                return;
            }
            presets = new Dictionary<string, SSPreset>();
            foreach (var presetNode in presetNodes)
            {
                SSPreset preset = new SSPreset();
                preset.dParams = new Dictionary<string, SSParam>();
                preset.name = presetNode.Attribute("name").Value;
                presets.Add(preset.name, preset);
                var paramNodes = presetNode.Descendants("param");
                foreach (var paramNode in paramNodes)
                {
                    SSParam param = new SSParam();
                    param.id = paramNode.Attribute("id").Value;
                    preset.dParams.Add(param.id, param);
                    bool? enabled = (bool?)paramNode.Attribute("value");
                    if (enabled.HasValue && (bool)enabled)
                    {
                        param.enabled = true;
                    }
                    var valueNodes = paramNode.Descendants("value");
                    if (valueNodes.Count() > 0)
                    {
                        param.dValues = new Dictionary<string, float>();
                        foreach (var valueNode in valueNodes)
                        {
                            param.dValues.Add(valueNode.Attribute("prop_Name").Value, (float)valueNode.Attribute("value"));
                        }
                    }
                }
            }
            var scenePresetNodes = xdoc.Descendants("scenePreset");
            if (scenePresetNodes.Count() == 0)
            {
                return;
            }
            scenePresets = new Dictionary<string, string>();
            foreach (var sceneNode in scenePresetNodes)
            {
                scenePresets.Add(
                    generateSceneKey(sceneNode.Attribute("level").Value, sceneNode.Attribute("bgName").Value, sceneNode.Attribute("yotogiName").Value)
                    , sceneNode.Value);
            }

        }

        private string generateSceneKey(params string[] keys)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var key in keys)
            {
                sb.Append(key);
                sb.Append(",");
            }
            return sb.ToString();
        }

        private void savePresetXml()
        {
            if (!File.Exists(presetXmlFileName))
            {
                var xml = new XDocument(
                     new XDeclaration("1.0", "utf-8", "true"),
                     new XElement("presets"));
                xml.Save(presetXmlFileName);
            }

            var xdoc = XDocument.Load(presetXmlFileName);

            var preset = new XElement("preset",
                new XAttribute("name", presetName),
                    new XElement("param",
                        new XAttribute("id", PKeyEnable),
                        new XAttribute("value", ssParam.bEnabled[PKeyEnable])
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyAlwaysLookAtMaid),
                        new XAttribute("value", ssParam.bEnabled[PKeyAlwaysLookAtMaid])
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyAlwaysLookAtFace),
                        new XAttribute("value", ssParam.bEnabled[PKeyAlwaysLookAtFace]),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropLookAtFaceUp),
                            new XAttribute("value", ssParam.fValue[PKeyAlwaysLookAtFace][PPropLookAtFaceUp])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropLookAtFaceLeft),
                            new XAttribute("value", ssParam.fValue[PKeyAlwaysLookAtFace][PPropLookAtFaceLeft])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropLookAtFaceFront),
                            new XAttribute("value", ssParam.fValue[PKeyAlwaysLookAtFace][PPropLookAtFaceFront])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyAlwaysLookAtXxx),
                        new XAttribute("value", ssParam.bEnabled[PKeyAlwaysLookAtXxx]),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropLookAtXxxUp),
                            new XAttribute("value", ssParam.fValue[PKeyAlwaysLookAtXxx][PPropLookAtXxxUp])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropLookAtXxxLeft),
                            new XAttribute("value", ssParam.fValue[PKeyAlwaysLookAtXxx][PPropLookAtXxxLeft])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropLookAtXxxFront),
                            new XAttribute("value", ssParam.fValue[PKeyAlwaysLookAtXxx][PPropLookAtXxxFront])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyAlwaysScreenOnMainCamera),
                        new XAttribute("value", ssParam.bEnabled[PKeyAlwaysScreenOnMainCamera]),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropAlwaysScreenOnMainCameraUp),
                            new XAttribute("value", ssParam.fValue[PKeyAlwaysScreenOnMainCamera][PPropAlwaysScreenOnMainCameraUp])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropAlwaysScreenOnMainCameraLeft),
                            new XAttribute("value", ssParam.fValue[PKeyAlwaysScreenOnMainCamera][PPropAlwaysScreenOnMainCameraLeft])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropAlwaysScreenOnMainCameraFront),
                            new XAttribute("value", ssParam.fValue[PKeyAlwaysScreenOnMainCamera][PPropAlwaysScreenOnMainCameraFront])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeySubCamera),
                        new XAttribute("value", ssParam.bEnabled[PKeySubCamera]),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubCameraPosX),
                            new XAttribute("value", ssParam.fValue[PKeySubCamera][PPropSubCameraPosX])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubCameraPosY),
                            new XAttribute("value", ssParam.fValue[PKeySubCamera][PPropSubCameraPosY])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubCameraWidth),
                            new XAttribute("value", ssParam.fValue[PKeySubCamera][PPropSubCameraWidth])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubCameraLocX),
                            new XAttribute("value", ssParam.fValue[PKeySubCamera][PPropSubCameraLocX])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubCameraLocY),
                            new XAttribute("value", ssParam.fValue[PKeySubCamera][PPropSubCameraLocY])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubCameraLocZ),
                            new XAttribute("value", ssParam.fValue[PKeySubCamera][PPropSubCameraLocZ])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubCameraAngX),
                            new XAttribute("value", ssParam.fValue[PKeySubCamera][PPropSubCameraAngX])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubCameraAngY),
                            new XAttribute("value", ssParam.fValue[PKeySubCamera][PPropSubCameraAngY])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubCameraAngZ),
                            new XAttribute("value", ssParam.fValue[PKeySubCamera][PPropSubCameraAngZ])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyMainLight),
                        new XAttribute("value", ssParam.bEnabled[PKeyMainLight]),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropMainLightLuminance),
                            new XAttribute("value", ssParam.fValue[PKeyMainLight][PPropMainLightLuminance])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropMainLightColorRed),
                            new XAttribute("value", ssParam.fValue[PKeyMainLight][PPropMainLightColorRed])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropMainLightColorGreen),
                            new XAttribute("value", ssParam.fValue[PKeyMainLight][PPropMainLightColorGreen])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropMainLightColorBlue),
                            new XAttribute("value", ssParam.fValue[PKeyMainLight][PPropMainLightColorBlue])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeySubLight),
                        new XAttribute("value", ssParam.bEnabled[PKeySubLight]),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubLightRange),
                            new XAttribute("value", ssParam.fValue[PKeySubLight][PPropSubLightRange])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubLightLuminance),
                            new XAttribute("value", ssParam.fValue[PKeySubLight][PPropSubLightLuminance])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubLightColorRed),
                            new XAttribute("value", ssParam.fValue[PKeySubLight][PPropSubLightColorRed])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubLightColorGreen),
                            new XAttribute("value", ssParam.fValue[PKeySubLight][PPropSubLightColorGreen])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubLightColorBlue),
                            new XAttribute("value", ssParam.fValue[PKeySubLight][PPropSubLightColorBlue])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyCameraColor),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropCameraColorRed),
                            new XAttribute("value", ssParam.fValue[PKeyCameraColor][PPropCameraColorRed])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropCameraColorGreen),
                            new XAttribute("value", ssParam.fValue[PKeyCameraColor][PPropCameraColorGreen])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropCameraColorBlue),
                            new XAttribute("value", ssParam.fValue[PKeyCameraColor][PPropCameraColorBlue])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropCameraColorAlpha),
                            new XAttribute("value", ssParam.fValue[PKeyCameraColor][PPropCameraColorAlpha])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropSubCameraFieldOfView),
                            new XAttribute("value", ssParam.fValue[PKeyCameraColor][PPropSubCameraFieldOfView])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyBSPos),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSPosX),
                            new XAttribute("value", ssParam.fValue[PKeyBSPos][PPropBSPosX])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSPosY),
                            new XAttribute("value", ssParam.fValue[PKeyBSPos][PPropBSPosY])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSPosZ),
                            new XAttribute("value", ssParam.fValue[PKeyBSPos][PPropBSPosZ])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyBSSize),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSSize),
                            new XAttribute("value", ssParam.fValue[PKeyBSSize][PPropBSSize])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyBSAngle),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSAngleX),
                            new XAttribute("value", ssParam.fValue[PKeyBSAngle][PPropBSAngleX])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSAngleY),
                            new XAttribute("value", ssParam.fValue[PKeyBSAngle][PPropBSAngleY])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSAngleZ),
                            new XAttribute("value", ssParam.fValue[PKeyBSAngle][PPropBSAngleZ])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyBSColor),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropScreenLightLuminance),
                            new XAttribute("value", ssParam.fValue[PKeyBSColor][PPropScreenLightLuminance])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSColorLuminance),
                            new XAttribute("value", ssParam.fValue[PKeyBSColor][PPropBSColorLuminance])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSColorRed),
                            new XAttribute("value", ssParam.fValue[PKeyBSColor][PPropBSColorRed])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSColorGreen),
                            new XAttribute("value", ssParam.fValue[PKeyBSColor][PPropBSColorGreen])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSColorBlue),
                            new XAttribute("value", ssParam.fValue[PKeyBSColor][PPropBSColorBlue])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropBSColorAlpha),
                            new XAttribute("value", ssParam.fValue[PKeyBSColor][PPropBSColorAlpha])
                            )
                        ),
                    new XElement("param",
                        new XAttribute("id", PKeyScreenFilter),
                        new XAttribute("value", ssParam.bEnabled[PKeyScreenFilter]),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropScreenFilterLuminance),
                            new XAttribute("value", ssParam.fValue[PKeyScreenFilter][PPropScreenFilterLuminance])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropScreenFilterRed),
                            new XAttribute("value", ssParam.fValue[PKeyScreenFilter][PPropScreenFilterRed])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropScreenFilterGreen),
                            new XAttribute("value", ssParam.fValue[PKeyScreenFilter][PPropScreenFilterGreen])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropScreenFilterBlue),
                            new XAttribute("value", ssParam.fValue[PKeyScreenFilter][PPropScreenFilterBlue])
                            ),
                        new XElement("value",
                            new XAttribute("prop_Name", PPropScreenFilterAlpha),
                            new XAttribute("value", ssParam.fValue[PKeyScreenFilter][PPropScreenFilterAlpha])
                            )
                        )
                    );
            var presetNodes = xdoc.Descendants("preset");
            if (presetNodes.Count() == 0)
            {
                xdoc.Root.AddFirst(preset);
            }
            else
            {
                presetNodes.Last().AddAfterSelf(preset);
            }
            xdoc.Save(presetXmlFileName);
        }

        private void SavePreset(string presetName) {
            if (presetName.Equals(""))
            {
                // 名無しはNG
                return;
            }
            if (presets != null && presets.ContainsKey(presetName))
            {
                // 同名は削除してから保存
                RemovePreset(presetName);
                savePresetXml();
                menuType = MenuType.Main;
            }
            else
            {
                savePresetXml();
                menuType = MenuType.Main;
            }
        }

        private void SaveScenePreset(string presetName)
        {
            RemoveScenePreset();

            var xdoc = XDocument.Load(presetXmlFileName);

            var scenePreset = new XElement("scenePreset",
                new XAttribute("level", Application.loadedLevel),
                new XAttribute("bgName", currentBg),
                new XAttribute("yotogiName", currentYotogiName),
                presetName);

            DebugLog("save scene preset", scenePreset.ToString());
            var scenePresetNodes = xdoc.Descendants("scenePreset");
            if (scenePresetNodes.Count() == 0)
            {
                xdoc.Root.Add(scenePreset);
            }
            else
            {
                scenePresetNodes.Last().AddAfterSelf(scenePreset);
            }
            xdoc.Save(presetXmlFileName);
        }

        private void RemoveScenePreset()
        {
            var xdoc = XDocument.Load(presetXmlFileName);
            IEnumerable<XElement> removeTarget =
                from el in xdoc.Descendants("scenePreset")
                where (string)el.Attribute("level") == Application.loadedLevel.ToString()
                  && (string)el.Attribute("bgName") == currentBg
                  && (string)el.Attribute("yotogiName") == currentYotogiName
                select el;

            if (removeTarget.Count() > 0)
            {
                foreach (var elem in removeTarget.ToList())
                {
                    DebugLog("remove scene preset", elem.ToString());
                    elem.Remove();
                }
                xdoc.Save(presetXmlFileName);
            }
        }

        private void RemovePreset(string presetName)
        {
            var xdoc = XDocument.Load(presetXmlFileName);
            IEnumerable<XElement> removeTarget =
                from el in xdoc.Descendants("preset")
                where (string)el.Attribute("name") == presetName
                select el;

            if (removeTarget.Count() > 0)
            {
                foreach (var elem in removeTarget.ToList())
                {
                    DebugLog("remove preset", elem.ToString());
                    elem.Remove();
                }
                xdoc.Save(presetXmlFileName);
                loadPresetXml();
            }
        }

        private int fixPx(int px)
        {
            float mag = 1f + (Screen.width / 1280f - 1f) * 0.6f;

            return (int)(mag * px);
        }

        private void drawWinHeader(Rect rect, string s, GUIStyle style)
        {
            GUI.Label(rect, s, style);
            {
                ;
            }
        }

        private float drawModValueSlider(Rect outRect, float value, float min, float max, string label, GUIStyle lstyle)
        {
            float conWidth = outRect.width;

            outRect.width = conWidth * 0.18f;
            GUI.Label(outRect, label, lstyle);
            outRect.x += outRect.width;

            outRect.width = conWidth * 0.12f;
            value = float.Parse(GUI.TextField(outRect,  value.ToString("F")));
            outRect.x += outRect.width;

            outRect.width = conWidth * 0.7f;
            outRect.y += pv.PropPx(5);
            return GUI.HorizontalSlider(outRect, value, min, max);
        }

        private void SetRendererEnabled(GameObject obj, bool enabled)
        {
            Assert.IsNotNull(obj);
            var renderer = obj.GetComponent<Renderer>();
            if (renderer)
            {
                renderer.enabled = enabled;
            }
        }

        const string DebugLogHeader = "SubScreen:";

        public static void DebugLog(string message)
        {
            Debug.Log(DebugLogHeader + message);

        }

        public static void DebugLog(string key, string message)
        {
            Debug.Log(DebugLogHeader + key + ":" + message);
        }

        public static void ErrorLog(string message)
        {
            Debug.Log(DebugLogHeader + message);

        }

        public static void ErrorLog(string key, string message)
        {
            Debug.Log(DebugLogHeader + key + ":" + message);
        }
        
        class SSPreset
        {
            public string name;

            public Dictionary<string, SSParam> dParams;
        }
        class SSParam
        {
            public string id;

            public bool enabled = false;

            public Dictionary<string, float> dValues;
        }
        
        private void eyeMove(){
	        	if(ssParam.bEnabled[PEyeMove]){
                Vector3 vl = maid.body0.trsEyeL.localPosition;
                Vector3 vr = maid.body0.trsEyeR.localPosition;
                maid.body0.trsEyeL.localPosition = new Vector3((ssParam.fValue[PEyeMove][PPropEyeMoveN] -2.0f)/100f, ssParam.fValue[PEyeMove][PPropEyeMoveL]/100f, -1.0f*ssParam.fValue[PEyeMove][PPropEyeMoveM]/100f);
                maid.body0.trsEyeR.localPosition = new Vector3((ssParam.fValue[PEyeMove][PPropEyeMoveT] -2.0f)/100f, -1.0f*ssParam.fValue[PEyeMove][PPropEyeMoveR]/100f, ssParam.fValue[PEyeMove][PPropEyeMoveS]/100f);
//                Vector3 vl = maid.body0.trsEyeL.localEulerAngles;
//                Vector3 vr = maid.body0.trsEyeR.localEulerAngles;
//                maid.body0.trsEyeL.localEulerAngles = new Vector3(ssParam.fValue[PEyeMove][PPropEyeMoveM], ssParam.fValue[PEyeMove][PPropEyeMoveL], vl.z);
//                maid.body0.trsEyeR.localEulerAngles = new Vector3(ssParam.fValue[PEyeMove][PPropEyeMoveS], ssParam.fValue[PEyeMove][PPropEyeMoveR], vl.z);
        	}
        }
        
         public static class Assert
    {
        [System.Diagnostics.Conditional("DEBUG")]
        public static void IsNotNull(object obj)
        {
            if (obj == null)
            {
                string msg = "Assertion failed. Value is null.";
                UnityEngine.Debug.LogError(msg);
            }
        }
    }
    }
}