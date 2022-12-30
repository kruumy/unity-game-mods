using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class LogitechGSDK
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void logiArxCB(int eventType, int eventValue, [MarshalAs(UnmanagedType.LPWStr)] string eventArg, IntPtr context);

	public struct logiArxCbContext
	{
		public logiArxCB arxCallBack;

		public IntPtr arxContext;
	}

	public enum keyboardNames
	{
		ESC = 1,
		F1 = 59,
		F2 = 60,
		F3 = 61,
		F4 = 62,
		F5 = 63,
		F6 = 64,
		F7 = 65,
		F8 = 66,
		F9 = 67,
		F10 = 68,
		F11 = 87,
		F12 = 88,
		PRINT_SCREEN = 311,
		SCROLL_LOCK = 70,
		PAUSE_BREAK = 69,
		TILDE = 41,
		ONE = 2,
		TWO = 3,
		THREE = 4,
		FOUR = 5,
		FIVE = 6,
		SIX = 7,
		SEVEN = 8,
		EIGHT = 9,
		NINE = 10,
		ZERO = 11,
		MINUS = 12,
		EQUALS = 13,
		BACKSPACE = 14,
		INSERT = 338,
		HOME = 327,
		PAGE_UP = 329,
		NUM_LOCK = 325,
		NUM_SLASH = 309,
		NUM_ASTERISK = 55,
		NUM_MINUS = 74,
		TAB = 15,
		Q = 16,
		W = 17,
		E = 18,
		R = 19,
		T = 20,
		Y = 21,
		U = 22,
		I = 23,
		O = 24,
		P = 25,
		OPEN_BRACKET = 26,
		CLOSE_BRACKET = 27,
		BACKSLASH = 43,
		KEYBOARD_DELETE = 339,
		END = 335,
		PAGE_DOWN = 337,
		NUM_SEVEN = 71,
		NUM_EIGHT = 72,
		NUM_NINE = 73,
		NUM_PLUS = 78,
		CAPS_LOCK = 58,
		A = 30,
		S = 31,
		D = 32,
		F = 33,
		G = 34,
		H = 35,
		J = 36,
		K = 37,
		L = 38,
		SEMICOLON = 39,
		APOSTROPHE = 40,
		ENTER = 28,
		NUM_FOUR = 75,
		NUM_FIVE = 76,
		NUM_SIX = 77,
		LEFT_SHIFT = 42,
		Z = 44,
		X = 45,
		C = 46,
		V = 47,
		B = 48,
		N = 49,
		M = 50,
		COMMA = 51,
		PERIOD = 52,
		FORWARD_SLASH = 53,
		RIGHT_SHIFT = 54,
		ARROW_UP = 328,
		NUM_ONE = 79,
		NUM_TWO = 80,
		NUM_THREE = 81,
		NUM_ENTER = 284,
		LEFT_CONTROL = 29,
		LEFT_WINDOWS = 347,
		LEFT_ALT = 56,
		SPACE = 57,
		RIGHT_ALT = 312,
		RIGHT_WINDOWS = 348,
		APPLICATION_SELECT = 349,
		RIGHT_CONTROL = 285,
		ARROW_LEFT = 331,
		ARROW_DOWN = 336,
		ARROW_RIGHT = 333,
		NUM_ZERO = 82,
		NUM_PERIOD = 83
	}

	public enum DeviceType
	{
		Keyboard = 0,
		Mouse = 3,
		Mousemat = 4,
		Headset = 8,
		Speaker = 14
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct GkeyCode
	{
		public ushort complete;

		public int keyIdx => complete & 0xFF;

		public int keyDown => (complete >> 8) & 1;

		public int mState => (complete >> 9) & 3;

		public int mouse => (complete >> 11) & 0xF;

		public int reserved1 => (complete >> 15) & 1;

		public int reserved2 => (complete >> 16) & 0x1FFFF;
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void logiGkeyCB(GkeyCode gkeyCode, [MarshalAs(UnmanagedType.LPWStr)] string gkeyOrButtonString, IntPtr context);

	public struct logiGKeyCbContext
	{
		public logiGkeyCB gkeyCallBack;

		public IntPtr gkeyContext;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct LogiControllerPropertiesData
	{
		public bool forceEnable;

		public int overallGain;

		public int springGain;

		public int damperGain;

		public bool defaultSpringEnabled;

		public int defaultSpringGain;

		public bool combinePedals;

		public int wheelRange;

		public bool gameSettingsEnabled;

		public bool allowGameSettings;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct DIJOYSTATE2ENGINES
	{
		public int lX;

		public int lY;

		public int lZ;

		public int lRx;

		public int lRy;

		public int lRz;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglSlider;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] rgdwPOV;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] rgbButtons;

		public int lVX;

		public int lVY;

		public int lVZ;

		public int lVRx;

		public int lVRy;

		public int lVRz;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglVSlider;

		public int lAX;

		public int lAY;

		public int lAZ;

		public int lARx;

		public int lARy;

		public int lARz;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglASlider;

		public int lFX;

		public int lFY;

		public int lFZ;

		public int lFRx;

		public int lFRy;

		public int lFRz;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglFSlider;
	}

	public const int LOGI_ARX_ORIENTATION_PORTRAIT = 1;

	public const int LOGI_ARX_ORIENTATION_LANDSCAPE = 16;

	public const int LOGI_ARX_EVENT_FOCUS_ACTIVE = 1;

	public const int LOGI_ARX_EVENT_FOCUS_INACTIVE = 2;

	public const int LOGI_ARX_EVENT_TAP_ON_TAG = 4;

	public const int LOGI_ARX_EVENT_MOBILEDEVICE_ARRIVAL = 8;

	public const int LOGI_ARX_EVENT_MOBILEDEVICE_REMOVAL = 16;

	public const int LOGI_ARX_DEVICETYPE_IPHONE = 1;

	public const int LOGI_ARX_DEVICETYPE_IPAD = 2;

	public const int LOGI_ARX_DEVICETYPE_ANDROID_SMALL = 3;

	public const int LOGI_ARX_DEVICETYPE_ANDROID_NORMAL = 4;

	public const int LOGI_ARX_DEVICETYPE_ANDROID_LARGE = 5;

	public const int LOGI_ARX_DEVICETYPE_ANDROID_XLARGE = 6;

	public const int LOGI_ARX_DEVICETYPE_ANDROID_OTHER = 7;

	public const int LOGI_LED_BITMAP_WIDTH = 21;

	public const int LOGI_LED_BITMAP_HEIGHT = 6;

	public const int LOGI_LED_BITMAP_BYTES_PER_KEY = 4;

	public const int LOGI_LED_BITMAP_SIZE = 504;

	public const int LOGI_LED_DURATION_INFINITE = 0;

	private const int LOGI_DEVICETYPE_MONOCHROME_ORD = 0;

	private const int LOGI_DEVICETYPE_RGB_ORD = 1;

	private const int LOGI_DEVICETYPE_PERKEY_RGB_ORD = 2;

	public const int LOGI_DEVICETYPE_MONOCHROME = 1;

	public const int LOGI_DEVICETYPE_RGB = 2;

	public const int LOGI_DEVICETYPE_PERKEY_RGB = 4;

	public const int LOGI_LCD_COLOR_BUTTON_LEFT = 256;

	public const int LOGI_LCD_COLOR_BUTTON_RIGHT = 512;

	public const int LOGI_LCD_COLOR_BUTTON_OK = 1024;

	public const int LOGI_LCD_COLOR_BUTTON_CANCEL = 2048;

	public const int LOGI_LCD_COLOR_BUTTON_UP = 4096;

	public const int LOGI_LCD_COLOR_BUTTON_DOWN = 8192;

	public const int LOGI_LCD_COLOR_BUTTON_MENU = 16384;

	public const int LOGI_LCD_MONO_BUTTON_0 = 1;

	public const int LOGI_LCD_MONO_BUTTON_1 = 2;

	public const int LOGI_LCD_MONO_BUTTON_2 = 4;

	public const int LOGI_LCD_MONO_BUTTON_3 = 8;

	public const int LOGI_LCD_MONO_WIDTH = 160;

	public const int LOGI_LCD_MONO_HEIGHT = 43;

	public const int LOGI_LCD_COLOR_WIDTH = 320;

	public const int LOGI_LCD_COLOR_HEIGHT = 240;

	public const int LOGI_LCD_TYPE_MONO = 1;

	public const int LOGI_LCD_TYPE_COLOR = 2;

	public const int LOGITECH_MAX_MOUSE_BUTTONS = 20;

	public const int LOGITECH_MAX_GKEYS = 29;

	public const int LOGITECH_MAX_M_STATES = 3;

	public const int LOGI_MAX_CONTROLLERS = 2;

	public const int LOGI_FORCE_NONE = -1;

	public const int LOGI_FORCE_SPRING = 0;

	public const int LOGI_FORCE_CONSTANT = 1;

	public const int LOGI_FORCE_DAMPER = 2;

	public const int LOGI_FORCE_SIDE_COLLISION = 3;

	public const int LOGI_FORCE_FRONTAL_COLLISION = 4;

	public const int LOGI_FORCE_DIRT_ROAD = 5;

	public const int LOGI_FORCE_BUMPY_ROAD = 6;

	public const int LOGI_FORCE_SLIPPERY_ROAD = 7;

	public const int LOGI_FORCE_SURFACE_EFFECT = 8;

	public const int LOGI_NUMBER_FORCE_EFFECTS = 9;

	public const int LOGI_FORCE_SOFTSTOP = 10;

	public const int LOGI_FORCE_CAR_AIRBORNE = 11;

	public const int LOGI_PERIODICTYPE_NONE = -1;

	public const int LOGI_PERIODICTYPE_SINE = 0;

	public const int LOGI_PERIODICTYPE_SQUARE = 1;

	public const int LOGI_PERIODICTYPE_TRIANGLE = 2;

	public const int LOGI_DEVICE_TYPE_NONE = -1;

	public const int LOGI_DEVICE_TYPE_WHEEL = 0;

	public const int LOGI_DEVICE_TYPE_JOYSTICK = 1;

	public const int LOGI_DEVICE_TYPE_GAMEPAD = 2;

	public const int LOGI_DEVICE_TYPE_OTHER = 3;

	public const int LOGI_NUMBER_DEVICE_TYPES = 4;

	public const int LOGI_MANUFACTURER_NONE = -1;

	public const int LOGI_MANUFACTURER_LOGITECH = 0;

	public const int LOGI_MANUFACTURER_MICROSOFT = 1;

	public const int LOGI_MANUFACTURER_OTHER = 2;

	public const int LOGI_MODEL_G27 = 0;

	public const int LOGI_MODEL_DRIVING_FORCE_GT = 1;

	public const int LOGI_MODEL_G25 = 2;

	public const int LOGI_MODEL_MOMO_RACING = 3;

	public const int LOGI_MODEL_MOMO_FORCE = 4;

	public const int LOGI_MODEL_DRIVING_FORCE_PRO = 5;

	public const int LOGI_MODEL_DRIVING_FORCE = 6;

	public const int LOGI_MODEL_NASCAR_RACING_WHEEL = 7;

	public const int LOGI_MODEL_FORMULA_FORCE = 8;

	public const int LOGI_MODEL_FORMULA_FORCE_GP = 9;

	public const int LOGI_MODEL_FORCE_3D_PRO = 10;

	public const int LOGI_MODEL_EXTREME_3D_PRO = 11;

	public const int LOGI_MODEL_FREEDOM_24 = 12;

	public const int LOGI_MODEL_ATTACK_3 = 13;

	public const int LOGI_MODEL_FORCE_3D = 14;

	public const int LOGI_MODEL_STRIKE_FORCE_3D = 15;

	public const int LOGI_MODEL_G940_JOYSTICK = 16;

	public const int LOGI_MODEL_G940_THROTTLE = 17;

	public const int LOGI_MODEL_G940_PEDALS = 18;

	public const int LOGI_MODEL_RUMBLEPAD = 19;

	public const int LOGI_MODEL_RUMBLEPAD_2 = 20;

	public const int LOGI_MODEL_CORDLESS_RUMBLEPAD_2 = 21;

	public const int LOGI_MODEL_CORDLESS_GAMEPAD = 22;

	public const int LOGI_MODEL_DUAL_ACTION_GAMEPAD = 23;

	public const int LOGI_MODEL_PRECISION_GAMEPAD_2 = 24;

	public const int LOGI_MODEL_CHILLSTREAM = 25;

	public const int LOGI_NUMBER_MODELS = 26;

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxInit(string identifier, string friendlyName, ref logiArxCbContext callback);

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxAddFileAs(string filePath, string fileName, string mimeType);

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxAddContentAs(byte[] content, int size, string fileName, string mimeType = "");

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxAddUTF8StringAs(string stringContent, string fileName, string mimeType = "");

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxAddImageFromBitmap(byte[] bitmap, int width, int height, string fileName);

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxSetIndex(string fileName);

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxSetTagPropertyById(string tagId, string prop, string newValue);

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxSetTagsPropertyByClass(string tagsClass, string prop, string newValue);

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxSetTagContentById(string tagId, string newContent);

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxSetTagsContentByClass(string tagsClass, string newContent);

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiArxGetLastError();

	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void LogiArxShutdown();

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedInit();

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetTargetDevice(int targetDevice);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedGetSdkVersion(ref int majorNum, ref int minorNum, ref int buildNum);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSaveCurrentLighting();

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLighting(int redPercentage, int greenPercentage, int bluePercentage);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedRestoreLighting();

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedFlashLighting(int redPercentage, int greenPercentage, int bluePercentage, int milliSecondsDuration, int milliSecondsInterval);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedPulseLighting(int redPercentage, int greenPercentage, int bluePercentage, int milliSecondsDuration, int milliSecondsInterval);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedStopEffects();

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingFromBitmap(byte[] bitmap);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingForKeyWithScanCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingForKeyWithHidCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingForKeyWithQuartzCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingForKeyWithKeyName(keyboardNames keyCode, int redPercentage, int greenPercentage, int bluePercentage);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingForTargetZone(DeviceType deviceType, int zone, int redPercentage, int greenPercentage, int bluePercentage);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSaveLightingForKey(keyboardNames keyName);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedRestoreLightingForKey(keyboardNames keyName);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedFlashSingleKey(keyboardNames keyName, int redPercentage, int greenPercentage, int bluePercentage, int msDuration, int msInterval);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedPulseSingleKey(keyboardNames keyName, int startRedPercentage, int startGreenPercentage, int startBluePercentage, int finishRedPercentage, int finishGreenPercentage, int finishBluePercentage, int msDuration, bool isInfinite);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedStopEffectsOnKey(keyboardNames keyName);

	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern void LogiLedShutdown();

	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdInit(string friendlyName, int lcdType);

	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdIsConnected(int lcdType);

	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdIsButtonPressed(int button);

	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void LogiLcdUpdate();

	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void LogiLcdShutdown();

	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdMonoSetBackground(byte[] monoBitmap);

	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdMonoSetText(int lineNumber, string text);

	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdColorSetBackground(byte[] colorBitmap);

	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdColorSetTitle(string text, int red, int green, int blue);

	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdColorSetText(int lineNumber, string text, int red, int green, int blue);

	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGkeyInitWithoutCallback();

	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGkeyInitWithoutContext(logiGkeyCB gkeyCB);

	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGkeyInit(ref logiGKeyCbContext cbStruct);

	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGkeyIsMouseButtonPressed(int buttonNumber);

	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern IntPtr LogiGkeyGetMouseButtonString(int buttonNumber);

	public static string LogiGkeyGetMouseButtonStr(int buttonNumber)
	{
		return Marshal.PtrToStringUni(LogiGkeyGetMouseButtonString(buttonNumber));
	}

	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGkeyIsKeyboardGkeyPressed(int gkeyNumber, int modeNumber);

	[DllImport("LogitechGKeyEnginesWrapper")]
	private static extern IntPtr LogiGkeyGetKeyboardGkeyString(int gkeyNumber, int modeNumber);

	public static string LogiGkeyGetKeyboardGkeyStr(int gkeyNumber, int modeNumber)
	{
		return Marshal.PtrToStringUni(LogiGkeyGetKeyboardGkeyString(gkeyNumber, modeNumber));
	}

	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void LogiGkeyShutdown();

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiSteeringInitialize(bool ignoreXInputControllers);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiSteeringShutdown();

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiUpdate();

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr LogiGetStateENGINES(int index);

	public static DIJOYSTATE2ENGINES LogiGetStateUnity(int index)
	{
		DIJOYSTATE2ENGINES result = default(DIJOYSTATE2ENGINES);
		result.rglSlider = new int[2];
		result.rgdwPOV = new uint[4];
		result.rgbButtons = new byte[128];
		result.rglVSlider = new int[2];
		result.rglASlider = new int[2];
		result.rglFSlider = new int[2];
		try
		{
			result = (DIJOYSTATE2ENGINES)Marshal.PtrToStructure(LogiGetStateENGINES(index), typeof(DIJOYSTATE2ENGINES));
			return result;
		}
		catch (ArgumentException)
		{
			Debug.Log("Exception catched");
			return result;
		}
	}

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGetFriendlyProductName(int index, StringBuilder buffer, int bufferSize);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsConnected(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsDeviceConnected(int index, int deviceType);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsManufacturerConnected(int index, int manufacturerName);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsModelConnected(int index, int modelName);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiButtonTriggered(int index, int buttonNbr);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiButtonReleased(int index, int buttonNbr);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiButtonIsPressed(int index, int buttonNbr);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGenerateNonLinearValues(int index, int nonLinCoeff);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGetNonLinearValue(int index, int inputValue);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiHasForceFeedback(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsPlaying(int index, int forceType);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySpringForce(int index, int offsetPercentage, int saturationPercentage, int coefficientPercentage);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSpringForce(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayConstantForce(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopConstantForce(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayDamperForce(int index, int coefficientPercentage);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopDamperForce(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySideCollisionForce(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayFrontalCollisionForce(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayDirtRoadEffect(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopDirtRoadEffect(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayBumpyRoadEffect(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopBumpyRoadEffect(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySlipperyRoadEffect(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSlipperyRoadEffect(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySurfaceEffect(int index, int type, int magnitudePercentage, int period);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSurfaceEffect(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayCarAirborne(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopCarAirborne(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySoftstopForce(int index, int usableRangePercentage);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSoftstopForce(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiSetPreferredControllerProperties(LogiControllerPropertiesData properties);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGetCurrentControllerProperties(int index, ref LogiControllerPropertiesData properties);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGetShifterMode(int index);

	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayLeds(int index, float currentRPM, float rpmFirstLedTurnsOn, float rpmRedLine);
}
