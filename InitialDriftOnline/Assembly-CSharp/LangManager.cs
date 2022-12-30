using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LangManager : MonoBehaviour
{
	[Header("EXTERNAL SCRIPT")]
	[Space]
	public GameObject SRToffuLivraison;

	public GameObject SRToffuLivraison2;

	public GameObject SRToffuManager;

	public GameObject EnterArea;

	public GameObject EnterAreaGarage;

	public GameObject RaceManager;

	public GameObject SRautorespawn;

	[Space]
	public Dropdown selector;

	[Space]
	public Dropdown selectorfromtuto;

	[Space]
	private int SelectedLanguage;

	[Space]
	[Header("MENU")]
	public Text DriverLicence;

	public Text Settings;

	public Text Help;

	public Text Credits;

	public Text Lobby;

	public Text Exit;

	public Text TofuRunCount;

	public Text TofuBestTime;

	public Text TofuRunCount2;

	public Text TofuBestTime2;

	public Text AutoTransmission;

	public Text Master;

	public Text Sound;

	public Text Music;

	public Text Resolution;

	public Text DisplayDistance;

	public Text Quality;

	public Text Fullscreen;

	public Text Language;

	public Text Keyboard;

	public Text PlayerList;

	public Text Leaderboard;

	public Text BackCam;

	public Text Radio;

	public Text Shadows;

	public Text SteeringSensitivity;

	public Text steeringHelper;

	public Text Vibration;

	public Text PlayerList2;

	public Text MinimapHigh;

	public Text ReatCamRot;

	public Text popupchat;

	[Space]
	[Header("CAR DEALER")]
	public Text CarDealerTitle;

	[Space]
	[Header("GARAGE")]
	public Text GarageTitle;

	public Text WheelCamber;

	public Text FrontCamber;

	public Text RearCamber;

	public Text Suspension;

	public Text FrontSuspensions;

	public Text RearSuspensions;

	public Text Power;

	public Text Break;

	public Text Torque;

	public Text EngineSettings;

	public Text Press;

	public Text Origin1;

	public Text Origin2;

	public Text Origin3;

	public Text Turbo;

	public Text RevLimiter;

	public Text ExhaustFlame;

	public Text[] Stage1;

	public Text[] Stage2;

	public Text[] Stage3;

	public Text[] Stage4;

	public Text[] Stage5;

	[Space]
	[Header("TOFU")]
	public Text MakeATofuDelivery;

	public Text Yes;

	public Text No;

	[Space]
	[Header("SCHEMA HELP")]
	public Image KeyboardHelp;

	public Image XboxHelp;

	[Space]
	public Sprite KBFR;

	public Sprite XBOXFR;

	public Sprite KBEN;

	public Sprite XBOXEN;

	public Sprite KBRU;

	public Sprite XBOXRU;

	public Sprite KBCT;

	public Sprite XBOXCT;

	public Sprite KBCS;

	public Sprite XBOXCS;

	[Header("TUTORIEL")]
	public Text VoiciQuelqueInfo;

	public Text OpenMenuTouche;

	public Text EndOfTuto;

	public Text IfYouPlayController;

	public Text ChooseYourLanguage;

	public Text WelcomeIn;

	public GameObject DDTUTO;

	private void Awake()
	{
		StartCoroutine(SetGoodLang());
	}

	private IEnumerator SetGoodLang()
	{
		yield return new WaitForSeconds(1f);
		if (PlayerPrefs.GetInt("LANGUAGE") == 0)
		{
			selector.value = 0;
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 1)
		{
			selector.value = 1;
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 2)
		{
			selector.value = 2;
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 3)
		{
			selector.value = 3;
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 4)
		{
			selector.value = 4;
		}
	}

	private void Start()
	{
		if (PlayerPrefs.GetInt("LANGUAGE") == 0)
		{
			SetEnglish();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 1)
		{
			SetRusse();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 2)
		{
			SetFrancais();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 3)
		{
			SetChinoisSimplifier();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 4)
		{
			SetChinoistradi();
		}
	}

	public void ChangeLang()
	{
		SelectedLanguage = selector.value;
		PlayerPrefs.SetInt("LANGUAGE", SelectedLanguage);
		Start();
	}

	public void ChangeLangFromTuto()
	{
		SelectedLanguage = selectorfromtuto.value;
		PlayerPrefs.SetInt("LANGUAGE", SelectedLanguage);
		Start();
	}

	public void SetEnglish()
	{
		DriverLicence.text = "DRIVER LICENCE";
		Settings.text = "SETTINGS";
		Help.text = "HELP";
		Credits.text = "CREDITS";
		Lobby.text = "LOBBY";
		Exit.text = "EXIT";
		TofuRunCount.text = "DELIVERY RUN COUNT";
		TofuBestTime.text = "DELIVERY BEST TIME";
		TofuRunCount2.text = "DELIVERY RUN COUNT";
		TofuBestTime2.text = "DELIVERY BEST TIME";
		AutoTransmission.text = "AUTO TRANMISSION";
		Master.text = "MASTER";
		Sound.text = "SOUND";
		Music.text = "MUSIC";
		Resolution.text = "RESOLUTION";
		DisplayDistance.text = "DISPLAY DISTANCE";
		Quality.text = "QUALITY";
		Fullscreen.text = "FULLSCREEN";
		Language.text = "LANGUAGE";
		Keyboard.text = "KEYBOARD";
		PlayerList.text = "PLAYERS LIST";
		PlayerList2.text = "PLAYERS LIST";
		Leaderboard.text = "LEADERBOARD";
		BackCam.text = "BACK CAM";
		Radio.text = "RADIO";
		Shadows.text = "SHADOWS";
		SteeringSensitivity.text = "STEERING SENSITIVITY";
		steeringHelper.text = "STEERING HELPER";
		Vibration.text = "VIBRATION";
		MinimapHigh.text = "MINIMAP ZOOM";
		ReatCamRot.text = "REVERSE CAMERA ROTATION";
		popupchat.text = "CHAT POP-UP";
		CarDealerTitle.text = "CAR DEALER";
		GarageTitle.text = "GARAGE";
		WheelCamber.text = "WHEEL CAMBER";
		FrontCamber.text = "FRONT CAMBER";
		RearCamber.text = "REAR CAMBER";
		Suspension.text = "SUSPENSION";
		FrontSuspensions.text = "FRONT SUSPENSION";
		RearSuspensions.text = "REAR SUSPENSION";
		Power.text = "POWER";
		Break.text = "BRAKE";
		Torque.text = "TORQUE";
		EngineSettings.text = "ENGINE SETTINGS";
		Origin1.text = "ORIGIN";
		Origin2.text = "ORIGIN";
		Origin3.text = "ORIGIN";
		Turbo.text = "TURBO";
		RevLimiter.text = "REV LIMITER";
		ExhaustFlame.text = "EXHAUST FLAME";
		Press.text = "PRESS";
		MakeATofuDelivery.text = "MAKE A TOFU DELIVERY ?";
		Yes.text = "YES";
		No.text = "NO";
		SRToffuManager.GetComponent<SRToffuManager>().HaveAgoodDrive = "RIKO: HAVE A GOOD RUN !";
		SRToffuManager.GetComponent<SRToffuManager>().InterruptedDelivery = "INTERRUPTED DELIVERY !";
		SRToffuManager.GetComponent<SRToffuManager>().DeliveryCompleted = "DELIVERY COMPLETED !";
		SRToffuManager.GetComponent<SRToffuManager>().DeliveryTime = "DELIVERY TIME !";
		if ((bool)EnterArea)
		{
			EnterArea.GetComponent<EnterArea>().LangWelcome = ": WELCOME !";
			EnterArea.GetComponent<EnterArea>().ToOpenCarDealer = "TO OPEN CAR DEALER";
			EnterArea.GetComponent<EnterArea>().LangSeeYouSoon = ": SEE YOU SOON";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().LangWelcome = ": WELCOME !";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().ToOpenCarGarage = "TO OPEN GARAGE";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().LangSeeYouSoon = ": SEE YOU SOON";
		}
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangWelcome = "RIKO: WELCOME !";
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangDeliveryInProgress = "RIKO: DELIVERY IN PROGRESS";
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangSeeYouSoon = "RIKO: SEE YOU SOON";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangWelcome = "RIKO: WELCOME !";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangDeliveryInProgress = "RIKO: DELIVERY IN PROGRESS";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangSeeYouSoon = "RIKO: SEE YOU SOON";
		RaceManager.GetComponent<RaceManager>().ImpossibleNow = "IMPOSSIBLE NOW";
		RaceManager.GetComponent<RaceManager>().ForRaceAgainst = "FOR RACE AGAINST ";
		RaceManager.GetComponent<RaceManager>().InvitationSentTo = "INVITATION SENT TO ";
		RaceManager.GetComponent<RaceManager>().ChallengeYou = " CHALLENGE YOU";
		RaceManager.GetComponent<RaceManager>().BattleImpossibleNow = "BATTLE IMPOSSIBLE NOW";
		RaceManager.GetComponent<RaceManager>().BattleInterrupted = "BATTLE INTERRUPTED";
		RaceManager.GetComponent<RaceManager>().YouWinThisBattle = "YOU WIN THIS BATTLE";
		RaceManager.GetComponent<RaceManager>().YouLooseTheBattle = "YOU LOSE THE BATTLE";
		RaceManager.GetComponent<RaceManager>().AsLeftTheBattleFinishFor = "AS LEFT THE BATTLE \n finish for the reward";
		SRautorespawn.GetComponent<SRautorespawn>().WAITTXT = "WAIT";
		SRautorespawn.GetComponent<SRautorespawn>().StayOnTheRoad = "STAY ON THE ROAD";
		Text[] stage = Stage1;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 1";
		}
		stage = Stage2;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 2";
		}
		stage = Stage3;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 3";
		}
		stage = Stage4;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 4";
		}
		stage = Stage5;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 5";
		}
		PlayerPrefs.SetString("JoinTxt", "JOIN");
		PlayerPrefs.SetString("ServerListtxt", "SERVER LIST");
		PlayerPrefs.SetString("Exitranslatetxt", "EXIT");
		KeyboardHelp.sprite = KBEN;
		XboxHelp.sprite = XBOXEN;
		GetComponentInParent<SRUIManager>().BattleWintrad = "BATTLE WIN";
		GetComponentInParent<SRUIManager>().Moneytrad = "MONEY";
		GetComponentInParent<SRUIManager>().PlayingTimetrad = "PLAYING TIME";
		GetComponentInParent<SRUIManager>().UpdateLanguage();
		OpenMenuTouche.text = "press [ESC] or [START] to open menu";
		IfYouPlayController.text = "IF YOU PLAY WITH A CONTROLLER : CHANGE KEYBOARD TO XBOX IN SETTINGS";
		GetComponent<SRTutoManager>().InfoCarsDealer = "HERE YOU CAN FIND THE CARS AND SKINS SHOP";
		GetComponent<SRTutoManager>().InfoGarage = "Here you can custom your car: \n suspension and camber of the wheels (for drift), power, braking, etc.";
		GetComponent<SRTutoManager>().Tips = "TIP : USE YOUR HANDBRAKE [SPACE] or [A] FOR PERFECT DRIFT";
		GetComponent<SRTutoManager>().InfoTofuNeedTranslate = "Tofu ? : Fill your wallet with tofu deliveries";
		if ((bool)ChooseYourLanguage)
		{
			ChooseYourLanguage.text = "Choose your language";
		}
		if ((bool)WelcomeIn)
		{
			WelcomeIn.text = "Welcome to Initial Drift Online";
		}
	}

	public void SetRusse()
	{
		DriverLicence.text = "ВОДИТЕЛЬСКОЕ УДОСТОВЕРЕНИЕ";
		Settings.text = "НАСТРОЙКИ";
		Help.text = "ПОМОЩЬ";
		Credits.text = "ТИТРЫ";
		Lobby.text = "ЛОББИ";
		Exit.text = "ВЫХОД";
		TofuRunCount.text = "КОЛИЧЕСТВО ДОСТАВОК";
		TofuBestTime.text = "ЛУЧШЕЕ ВРЕМЯ ДОСТАВКИ";
		TofuRunCount2.text = "КОЛИЧЕСТВО ДОСТАВОК";
		TofuBestTime2.text = "ЛУЧШЕЕ ВРЕМЯ ДОСТАВКИ";
		AutoTransmission.text = "АВТ. ТРАНСМИССИЯ";
		Master.text = "ОБЩАЯ";
		Sound.text = "ЗВУК";
		Music.text = "МУЗЫКА";
		Resolution.text = "РАЗРЕШЕНИЕ";
		DisplayDistance.text = "ДАЛЬНОСТЬ ПРОРИСОВКИ";
		Quality.text = "КАЧЕСТВО";
		Fullscreen.text = "ПОЛНЫЙ ЭКРАН";
		Language.text = "ЯЗЫК";
		Keyboard.text = "КЛАВИАТУРА";
		PlayerList.text = "СПИСОК ИГРОКОВ";
		PlayerList2.text = "СПИСОК ИГРОКОВ";
		Leaderboard.text = "лидерский состав";
		BackCam.text = "камера заднего вида";
		Radio.text = "радио";
		Shadows.text = "тени";
		SteeringSensitivity.text = "Чувствительность автомобиля";
		steeringHelper.text = "РУЛЕВОЙ ПОМОЩНИК";
		Vibration.text = "вибрация";
		MinimapHigh.text = "масштабирование карты";
		ReatCamRot.text = "обратное вращение камеры";
		popupchat.text = "всплывающее окно чата";
		CarDealerTitle.text = "АВТОДИЛЕР";
		GarageTitle.text = "ГАРАЖ";
		WheelCamber.text = "РАЗВАЛ";
		FrontCamber.text = "ПЕРЕДНИЙ РАЗВАЛ";
		RearCamber.text = "ЗАДНИЙ РАЗВАЛ";
		Suspension.text = "ПОДВЕСКА";
		FrontSuspensions.text = "ПЕРЕДНЯЯ ПОДВЕСКА";
		RearSuspensions.text = "ЗАДНЯЯ ПОДВЕСКА";
		Power.text = "МОЩНОСТЬ";
		Break.text = "ТОРМОЗ";
		Torque.text = "МОМЕНТ";
		EngineSettings.text = "НАСТРОЙКИ ДВИГАТЕЛЯ";
		Origin1.text = "ПРОИСХОЖДЕНИЕ";
		Origin2.text = "ПРОИСХОЖДЕНИЕ";
		Origin3.text = "ПРОИСХОЖДЕНИЕ";
		Turbo.text = "ПРОИСХОЖДЕНИЕ";
		RevLimiter.text = "ОГРАНИЧИТЕЛЬ ОБОРОТОВ";
		ExhaustFlame.text = "ПЛАМЯ ВЫХЛОПА";
		Press.text = "НАЖМИТЕ";
		MakeATofuDelivery.text = "ВЫПОЛНИТЬ ДОСТАВКУ ТОФУ ?";
		Yes.text = "ДА";
		No.text = "НЕТ";
		SRToffuManager.GetComponent<SRToffuManager>().HaveAgoodDrive = "RIKO: ПРИЯТНОЙ ПОЕЗДКИ!";
		SRToffuManager.GetComponent<SRToffuManager>().InterruptedDelivery = "ДОСТАВКА ПРЕРВАНА!";
		SRToffuManager.GetComponent<SRToffuManager>().DeliveryCompleted = "ДОСТАВКА ВЫПОЛНЕНА!";
		SRToffuManager.GetComponent<SRToffuManager>().DeliveryTime = "ВРЕМЯ ДОСТАВКИ!";
		if ((bool)EnterArea)
		{
			EnterArea.GetComponent<EnterArea>().LangWelcome = ": ДОБРО ПОЖАЛОВАТЬ !";
			EnterArea.GetComponent<EnterArea>().ToOpenCarDealer = "ПОСЕТИТЬ АВТОДИЛЕРА";
			EnterArea.GetComponent<EnterArea>().LangSeeYouSoon = ": УВИДИМСЯ";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().LangWelcome = ": ДОБРО ПОЖАЛОВАТЬ !";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().ToOpenCarGarage = "ЧТОБЫ ОТКРЫТЬ ГАРАЖ";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().LangSeeYouSoon = ": УВИДИМСЯ";
		}
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangWelcome = "RIKO: ДОБРО ПОЖАЛОВАТЬ !";
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangDeliveryInProgress = "RIKO: ДОСТАВКА В ПРОЦЕССЕ";
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangSeeYouSoon = "RIKO: УВИДИМСЯ";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangWelcome = "RIKO: ДОБРО ПОЖАЛОВАТЬ !";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangDeliveryInProgress = "RIKO: ДОСТАВКА В ПРОЦЕССЕ";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangSeeYouSoon = "RIKO: УВИДИМСЯ";
		RaceManager.GetComponent<RaceManager>().ImpossibleNow = "НЕДОСТУПНО";
		RaceManager.GetComponent<RaceManager>().ForRaceAgainst = "ГОНКА ПРОТИВ ";
		RaceManager.GetComponent<RaceManager>().InvitationSentTo = "ПРИГЛАШЕНИЕ ОТПРАВЛЕНО ";
		RaceManager.GetComponent<RaceManager>().ChallengeYou = " СДЕЛАЛ ВАМ ВЫЗОВ";
		RaceManager.GetComponent<RaceManager>().BattleImpossibleNow = "НЕВОЗМОЖНО НАЧАТЬ БАТТЛ";
		RaceManager.GetComponent<RaceManager>().BattleInterrupted = "БАТТЛ ПРЕРВАН";
		RaceManager.GetComponent<RaceManager>().YouWinThisBattle = "ВЫ ВЫИГРАЛИ БАТТЛ";
		RaceManager.GetComponent<RaceManager>().YouLooseTheBattle = "ВЫ ПРОИГРАЛИ БАТТЛ";
		RaceManager.GetComponent<RaceManager>().AsLeftTheBattleFinishFor = "ПОКИНУЛ БАТТЛ \n закончил гонку, чтобы получить награду";
		SRautorespawn.GetComponent<SRautorespawn>().WAITTXT = "ЖДИТЕ";
		SRautorespawn.GetComponent<SRautorespawn>().StayOnTheRoad = "НЕ ПОКИДАЙТЕ ДОРОГУ";
		Text[] stage = Stage1;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "уровень 1";
		}
		stage = Stage2;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "уровень 2";
		}
		stage = Stage3;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "уровень 3";
		}
		stage = Stage4;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "уровень 4";
		}
		stage = Stage5;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "уровень 5";
		}
		PlayerPrefs.SetString("JoinTxt", "ПРИСОЕДИНИТЬСЯ");
		PlayerPrefs.SetString("ServerListtxt", "СЕРВЕРНЫЙ СПИСОК");
		PlayerPrefs.SetString("Exitranslatetxt", "оставлять");
		KeyboardHelp.sprite = KBRU;
		XboxHelp.sprite = XBOXRU;
		GetComponentInParent<SRUIManager>().BattleWintrad = "победа в битве";
		GetComponentInParent<SRUIManager>().Moneytrad = "Деньги";
		GetComponentInParent<SRUIManager>().PlayingTimetrad = "игровое время";
		GetComponentInParent<SRUIManager>().UpdateLanguage();
		OpenMenuTouche.text = "нажмите [ESC] или [START], чтобы открыть меню";
		IfYouPlayController.text = "ЕСЛИ ВЫ ИГРАЕТЕ С КОНТРОЛЛЕРОМ: ЗАМЕНИТЕ КЛАВИАТУРУ НА XBOX В НАСТРОЙКАХ";
		GetComponent<SRTutoManager>().InfoCarsDealer = "ЗДЕСЬ ВЫ МОЖЕТЕ НАЙТИ МАГАЗИН АВТОМОБИЛЕЙ И СКИНОВ \n (Открой это / OPEN)";
		GetComponent<SRTutoManager>().InfoGarage = "Здесь вы можете кастомизировать свою машину: \n подвеска и развал колес (для заноса / Drift), мощность, торможение,...";
		GetComponent<SRTutoManager>().Tips = "СОВЕТ: ИСПОЛЬЗУЙТЕ РУЧНОЙ ТОРМОЗ [ПРОБЕЛ] или [A] ДЛЯ ИДЕАЛЬНОГО ДРЕЙФА";
		GetComponent<SRTutoManager>().InfoTofuNeedTranslate = "Tofu ? : Наполните свой кошелек доставкой тофу";
		if ((bool)ChooseYourLanguage)
		{
			ChooseYourLanguage.text = "Выберите свой язык";
		}
		if ((bool)WelcomeIn)
		{
			WelcomeIn.text = "добро пожаловать в Initial Drift Online";
		}
	}

	public void SetFrancais()
	{
		DriverLicence.text = "PERMIS DE CONDUIRE";
		Settings.text = "OPTIONS";
		Help.text = "AIDE";
		Credits.text = "CREDITS";
		Lobby.text = "LOBBY";
		Exit.text = "EXIT";
		TofuRunCount.text = "NOMBRE DE LIVRAISONS";
		TofuBestTime.text = "MEILLEUR TEMPS DE LIVRAISON";
		TofuRunCount2.text = "NOMBRE DE LIVRAISONS";
		TofuBestTime2.text = "MEILLEUR TEMPS DE LIVRAISON";
		AutoTransmission.text = "BOITE AUTO";
		Master.text = "GENERALE";
		Sound.text = "EFFET";
		Music.text = "MUSIQUE";
		Resolution.text = "RESOLUTION";
		DisplayDistance.text = "DISTANCE D'AFFICHAGE";
		Quality.text = "QUALITER";
		Fullscreen.text = "PLEIN ECRAN";
		Language.text = "LANGUAGE";
		Keyboard.text = "CLAVIER";
		PlayerList.text = "PLAYERS LIST";
		PlayerList2.text = "PLAYERS LIST";
		Leaderboard.text = "CLASSEMENT";
		BackCam.text = "CAMÉRA ARRIÈRE";
		Radio.text = "RADIO";
		Shadows.text = "OMBRES";
		SteeringSensitivity.text = "SENSIBILITÉ DIRECTION";
		steeringHelper.text = "AIDE A LA CONDUIRE";
		Vibration.text = "VIBRATION";
		MinimapHigh.text = "ZOOM MINIMAP";
		ReatCamRot.text = "ROTATION CAM (MARCHE AR)";
		popupchat.text = "CHAT POP-UP";
		CarDealerTitle.text = "CONCESSIONNAIRE";
		GarageTitle.text = "GARAGE";
		WheelCamber.text = "COURBURE DES ROUES";
		FrontCamber.text = "COURBURE AVANT";
		RearCamber.text = "COURBURE ARRIERE";
		Suspension.text = "SUSPENSION";
		FrontSuspensions.text = "SUSPENSION AV";
		RearSuspensions.text = "SUSPENSION AR";
		Power.text = "PUISSANCE";
		Break.text = "FREIN";
		Torque.text = "COUPLE";
		EngineSettings.text = "PARAMETRE MOTEUR";
		Origin1.text = "ORIGINE";
		Origin2.text = "ORIGINE";
		Origin3.text = "ORIGINE";
		Turbo.text = "TURBO";
		RevLimiter.text = "RUPTEUR";
		ExhaustFlame.text = "RETOUR DE FLAMES";
		Press.text = "APPUIE SUR";
		MakeATofuDelivery.text = "EFFECTUER UNE LIVRAISON DE TOFU ?";
		Yes.text = "OUI";
		No.text = "NON";
		SRToffuManager.GetComponent<SRToffuManager>().HaveAgoodDrive = "BLANCHE: BONNE ROUTE !";
		SRToffuManager.GetComponent<SRToffuManager>().InterruptedDelivery = "LIVRAISON INTERROMPU !";
		SRToffuManager.GetComponent<SRToffuManager>().DeliveryCompleted = "LIVRAISON TERMINE !";
		SRToffuManager.GetComponent<SRToffuManager>().DeliveryTime = "TEMPS DE LIVRAISON";
		if ((bool)EnterArea)
		{
			EnterArea.GetComponent<EnterArea>().LangWelcome = ": BIENVENUE !";
			EnterArea.GetComponent<EnterArea>().ToOpenCarDealer = "POUR OUVRIR LE CONCESSIONNAIRE";
			EnterArea.GetComponent<EnterArea>().LangSeeYouSoon = ": A BIENTOT";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().LangWelcome = ": BIENVENUE !";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().ToOpenCarGarage = "POUR OUVRIR LE GARAGE";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().LangSeeYouSoon = ": A BIENTOT";
		}
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangWelcome = "BLANCHE: BIENVENUE !";
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangDeliveryInProgress = "BLANCHE: LIVRAISON EN COURS";
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangSeeYouSoon = "BLANCHE: A BIENTOT";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangWelcome = "BLANCHE: BIENVENUE !";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangDeliveryInProgress = "BLANCHE: LIVRAISON EN COURS";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangSeeYouSoon = "BLANCHE: A BIENTOT";
		RaceManager.GetComponent<RaceManager>().ImpossibleNow = "IMPOSSIBLE POUR L'INSTANT";
		RaceManager.GetComponent<RaceManager>().ForRaceAgainst = "POUR DEFIER ";
		RaceManager.GetComponent<RaceManager>().InvitationSentTo = "INVITATION ENVOYE A ";
		RaceManager.GetComponent<RaceManager>().ChallengeYou = " VOUS CHALLENGE";
		RaceManager.GetComponent<RaceManager>().BattleImpossibleNow = "BATTLE IMPOSSIBLE POUR L'INSTANT";
		RaceManager.GetComponent<RaceManager>().BattleInterrupted = "BATTLE INTERROMPU";
		RaceManager.GetComponent<RaceManager>().YouWinThisBattle = "VOUS AVEZ GAGNEZ CETTE BATTLE";
		RaceManager.GetComponent<RaceManager>().YouLooseTheBattle = "VOUS AVEZ PERDU CETTE BATTLE";
		RaceManager.GetComponent<RaceManager>().AsLeftTheBattleFinishFor = "A QUITTE LA COURSE \n fini pour la récompense";
		SRautorespawn.GetComponent<SRautorespawn>().WAITTXT = "PATIENTE";
		SRautorespawn.GetComponent<SRautorespawn>().StayOnTheRoad = "RESTE SUR LA ROUTE";
		Text[] stage = Stage1;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 1";
		}
		stage = Stage2;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 2";
		}
		stage = Stage3;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 3";
		}
		stage = Stage4;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 4";
		}
		stage = Stage5;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 5";
		}
		PlayerPrefs.SetString("JoinTxt", "REJOINDRE");
		PlayerPrefs.SetString("ServerListtxt", "LISTE DES SERVEURS");
		PlayerPrefs.SetString("Exitranslatetxt", "QUITTER");
		KeyboardHelp.sprite = KBFR;
		XboxHelp.sprite = XBOXFR;
		GetComponentInParent<SRUIManager>().BattleWintrad = "BATTLE GAGNÉ";
		GetComponentInParent<SRUIManager>().Moneytrad = "MONNAIE";
		GetComponentInParent<SRUIManager>().PlayingTimetrad = "TEMPS DE JEUX";
		GetComponentInParent<SRUIManager>().UpdateLanguage();
		OpenMenuTouche.text = "Appuis sur [ESC] ou [START] pour ouvrir le menu";
		IfYouPlayController.text = "SI TU JOUES AVEC UNE MANETTE : CHANGE KEYBOARD PAR XBOX DANS LES SETTINGS";
		GetComponent<SRTutoManager>().InfoCarsDealer = "ICI TU PEUX TROUVER LE MAGASIN DES VOITURES ET DES SKINS \n (OUVRE-LE)";
		GetComponent<SRTutoManager>().InfoGarage = "Ici tu peux custom ta voiture: \n suspension et cambrure des roues (pour le drift), Puissance, Freinage, etc.";
		GetComponent<SRTutoManager>().Tips = "CONSEIL : UTILISE LE FREIN A MAIN [SPACE] ou [A] pour un drift parfait";
		GetComponent<SRTutoManager>().InfoTofuNeedTranslate = "Tofu ? : Rempli ton porte monnaie en effectuant des livraisons de tofu";
		if ((bool)ChooseYourLanguage)
		{
			ChooseYourLanguage.text = "Choisissez votre langue";
		}
		if ((bool)WelcomeIn)
		{
			WelcomeIn.text = "Bienvenue dans Initial Drift Online";
		}
	}

	public void SetChinoistradi()
	{
		DriverLicence.text = "駕駛證";
		Settings.text = "設定";
		Help.text = "幫助";
		Credits.text = "致謝";
		Lobby.text = "大堂";
		Exit.text = "退出";
		TofuRunCount.text = "交貨次數";
		TofuBestTime.text = "交貨最佳成績";
		TofuRunCount2.text = "交貨次數";
		TofuBestTime2.text = "交貨最佳成績";
		AutoTransmission.text = "自動波";
		Master.text = "主音量";
		Sound.text = "環境音量";
		Music.text = "音樂";
		Resolution.text = "顯示模式";
		DisplayDistance.text = "觀察距離";
		Quality.text = "畫質";
		Fullscreen.text = "全螢幕";
		Language.text = "語言";
		Keyboard.text = "鍵盤";
		PlayerList.text = "球員名單";
		PlayerList2.text = "球員名單";
		Leaderboard.text = "排行榜";
		BackCam.text = "後視鏡頭";
		Radio.text = "收音機";
		Shadows.text = "光影";
		SteeringSensitivity.text = "方向盤靈感度";
		steeringHelper.text = "方向盤輔助";
		Vibration.text = "手柄震動";
		MinimapHigh.text = "小地圖縮放";
		ReatCamRot.text = "後置攝像頭旋轉";
		popupchat.text = "聊天弹出";
		CarDealerTitle.text = "汽車銷售員";
		GarageTitle.text = "車庫";
		WheelCamber.text = "車輪外傾角";
		FrontCamber.text = "前輪外傾角";
		RearCamber.text = "後輪外傾角";
		Suspension.text = "懸掛";
		FrontSuspensions.text = "前懸掛";
		RearSuspensions.text = "後懸掛";
		Power.text = "能量";
		Break.text = "制動器";
		Torque.text = "扭力";
		EngineSettings.text = "設定發動機";
		Origin1.text = "原狀";
		Origin2.text = "原狀";
		Origin3.text = "原狀";
		Turbo.text = "渦輪增壓器";
		RevLimiter.text = "轉速限制器";
		ExhaustFlame.text = "排氣火焰";
		Press.text = "按";
		MakeATofuDelivery.text = "要運送豆腐嗎？";
		Yes.text = "要";
		No.text = "不要";
		SRToffuManager.GetComponent<SRToffuManager>().HaveAgoodDrive = "里子: 祝你一路順風！";
		SRToffuManager.GetComponent<SRToffuManager>().InterruptedDelivery = "運送中斷！";
		SRToffuManager.GetComponent<SRToffuManager>().DeliveryCompleted = "運送成功！";
		SRToffuManager.GetComponent<SRToffuManager>().DeliveryTime = "運送時間！";
		if ((bool)EnterArea)
		{
			EnterArea.GetComponent<EnterArea>().LangWelcome = ": 歡迎光臨！";
			EnterArea.GetComponent<EnterArea>().ToOpenCarDealer = "與銷售員對話";
			EnterArea.GetComponent<EnterArea>().LangSeeYouSoon = ": 有空再來吧";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().LangWelcome = ": 歡迎光臨！";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().ToOpenCarGarage = "展開車庫";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().LangSeeYouSoon = ": 有空再來吧";
		}
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangWelcome = "里子: 歡迎光臨！";
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangDeliveryInProgress = "里子: 運送進行中";
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangSeeYouSoon = "里子: 有空再來吧";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangWelcome = "里子: 歡迎光臨！";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangDeliveryInProgress = "里子: 運送進行中";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangSeeYouSoon = "里子: 有空再來吧";
		RaceManager.GetComponent<RaceManager>().ImpossibleNow = "無法進行";
		RaceManager.GetComponent<RaceManager>().ForRaceAgainst = "挑戰對方 ";
		RaceManager.GetComponent<RaceManager>().InvitationSentTo = "已邀請 ";
		RaceManager.GetComponent<RaceManager>().ChallengeYou = " 想挑戰你";
		RaceManager.GetComponent<RaceManager>().BattleImpossibleNow = "比賽無法進行";
		RaceManager.GetComponent<RaceManager>().BattleInterrupted = "比賽中斷";
		RaceManager.GetComponent<RaceManager>().YouWinThisBattle = "玩家獲勝";
		RaceManager.GetComponent<RaceManager>().YouLooseTheBattle = "玩家敗北";
		RaceManager.GetComponent<RaceManager>().AsLeftTheBattleFinishFor = "已離開比賽 \n 完成獲得獎勵";
		SRautorespawn.GetComponent<SRautorespawn>().WAITTXT = "請稍候";
		SRautorespawn.GetComponent<SRautorespawn>().StayOnTheRoad = "保持在道路上行駛";
		Text[] stage = Stage1;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 1";
		}
		stage = Stage2;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 2";
		}
		stage = Stage3;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 3";
		}
		stage = Stage4;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 4";
		}
		stage = Stage5;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 5";
		}
		PlayerPrefs.SetString("JoinTxt", "加入");
		PlayerPrefs.SetString("ServerListtxt", "服務器清單");
		PlayerPrefs.SetString("Exitranslatetxt", "退出");
		KeyboardHelp.sprite = KBCT;
		XboxHelp.sprite = XBOXCT;
		GetComponentInParent<SRUIManager>().BattleWintrad = "比賽勝利";
		GetComponentInParent<SRUIManager>().Moneytrad = "貨幣";
		GetComponentInParent<SRUIManager>().PlayingTimetrad = "遊戲總時數";
		GetComponentInParent<SRUIManager>().UpdateLanguage();
		OpenMenuTouche.text = "press [ESC] or [START] to open menu";
		IfYouPlayController.text = "IF YOU PLAY WITH A CONTROLLER : CHANGE KEYBOARD TO XBOX IN SETTINGS";
		GetComponent<SRTutoManager>().InfoCarsDealer = "HERE YOU CAN FIND THE CARS AND SKINS SHOP";
		GetComponent<SRTutoManager>().InfoGarage = "Here you can custom your car: \n suspension and camber of the wheels (for drift), power, braking, etc.";
		GetComponent<SRTutoManager>().Tips = "TIP : USE YOUR HANDBRAKE [SPACE] or [A] FOR PERFECT DRIFT";
		GetComponent<SRTutoManager>().InfoTofuNeedTranslate = "豆腐 ? : Fill your wallet with tofu deliveries";
		if ((bool)ChooseYourLanguage)
		{
			ChooseYourLanguage.text = "選擇你的語言";
		}
		if ((bool)WelcomeIn)
		{
			WelcomeIn.text = "歡迎來到 Initial Drift Online";
		}
	}

	public void SetChinoisSimplifier()
	{
		DriverLicence.text = "驾驶证";
		Settings.text = "设定";
		Help.text = "帮助";
		Credits.text = "致谢";
		Lobby.text = "大堂";
		Exit.text = "退出";
		TofuRunCount.text = "交货次数";
		TofuBestTime.text = "交货最佳成绩";
		TofuRunCount2.text = "交货次数";
		TofuBestTime2.text = "交货最佳成绩";
		AutoTransmission.text = "自动挡";
		Master.text = "主音量";
		Sound.text = "环境音量";
		Music.text = "音乐";
		Resolution.text = "显示模式";
		DisplayDistance.text = "观察距离";
		Quality.text = "画质";
		Fullscreen.text = "全萤幕";
		Language.text = "语言";
		Keyboard.text = "键盘";
		PlayerList.text = "球员名单";
		PlayerList2.text = "球员名单";
		Leaderboard.text = "排行榜";
		BackCam.text = "后视镜头";
		Radio.text = "收音机";
		Shadows.text = "光影";
		SteeringSensitivity.text = "方向盘灵感度";
		steeringHelper.text = "方向盘辅助";
		Vibration.text = "手柄震动";
		MinimapHigh.text = "小地图缩放";
		ReatCamRot.text = "后置摄像头旋转";
		popupchat.text = "聊天弹出";
		CarDealerTitle.text = "汽车销售员";
		GarageTitle.text = "车库";
		WheelCamber.text = "车轮外倾角";
		FrontCamber.text = "前轮外倾角";
		RearCamber.text = "后轮外倾角";
		Suspension.text = "悬挂";
		FrontSuspensions.text = "前悬挂";
		RearSuspensions.text = "后悬挂";
		Power.text = "能量";
		Break.text = "制动器";
		Torque.text = "扭力";
		EngineSettings.text = "设定发动机";
		Origin1.text = "原状";
		Origin2.text = "原状";
		Origin3.text = "原状";
		Turbo.text = "涡轮增压器";
		RevLimiter.text = "转速限制器";
		ExhaustFlame.text = "排气火焰";
		Press.text = "按";
		MakeATofuDelivery.text = "要运送豆腐吗？";
		Yes.text = "要";
		No.text = "不要";
		SRToffuManager.GetComponent<SRToffuManager>().HaveAgoodDrive = "里子: 祝你一路顺风！";
		SRToffuManager.GetComponent<SRToffuManager>().InterruptedDelivery = "运送中断！";
		SRToffuManager.GetComponent<SRToffuManager>().DeliveryCompleted = "运送成功！";
		SRToffuManager.GetComponent<SRToffuManager>().DeliveryTime = "运送时间！";
		if ((bool)EnterArea)
		{
			EnterArea.GetComponent<EnterArea>().LangWelcome = ": 欢迎光临！";
			EnterArea.GetComponent<EnterArea>().ToOpenCarDealer = "与销售员对话";
			EnterArea.GetComponent<EnterArea>().LangSeeYouSoon = ": 有空再来吧";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().LangWelcome = ": 欢迎光临！";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().ToOpenCarGarage = "展开车库";
			EnterAreaGarage.GetComponent<EnterAreaGarage>().LangSeeYouSoon = ": 有空再来吧";
		}
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangWelcome = "里子: 欢迎光临！";
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangDeliveryInProgress = "里子: 运送进行中";
		SRToffuLivraison.GetComponent<SRToffuLivraison>().LangSeeYouSoon = "里子: 有空再来吧";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangWelcome = "里子: 欢迎光临！";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangDeliveryInProgress = "里子: 运送进行中";
		SRToffuLivraison2.GetComponent<SRToffuLivraison>().LangSeeYouSoon = "里子: 有空再来吧";
		RaceManager.GetComponent<RaceManager>().ImpossibleNow = "无法进行";
		RaceManager.GetComponent<RaceManager>().ForRaceAgainst = "挑战对方 ";
		RaceManager.GetComponent<RaceManager>().InvitationSentTo = "已邀请 ";
		RaceManager.GetComponent<RaceManager>().ChallengeYou = " 想挑战你";
		RaceManager.GetComponent<RaceManager>().BattleImpossibleNow = "比赛无法进行";
		RaceManager.GetComponent<RaceManager>().BattleInterrupted = "比赛中断";
		RaceManager.GetComponent<RaceManager>().YouWinThisBattle = "玩家获胜";
		RaceManager.GetComponent<RaceManager>().YouLooseTheBattle = "玩家败北";
		RaceManager.GetComponent<RaceManager>().AsLeftTheBattleFinishFor = "已离开比赛 \n 完成获得奖励";
		SRautorespawn.GetComponent<SRautorespawn>().WAITTXT = "请稍候";
		SRautorespawn.GetComponent<SRautorespawn>().StayOnTheRoad = "保持在道路上行驶";
		Text[] stage = Stage1;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 1";
		}
		stage = Stage2;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 2";
		}
		stage = Stage3;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 3";
		}
		stage = Stage4;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 4";
		}
		stage = Stage5;
		for (int i = 0; i < stage.Length; i++)
		{
			stage[i].text = "STAGE 5";
		}
		PlayerPrefs.SetString("JoinTxt", "加入");
		PlayerPrefs.SetString("ServerListtxt", "服务器清单");
		PlayerPrefs.SetString("Exitranslatetxt", "退出");
		KeyboardHelp.sprite = KBCS;
		XboxHelp.sprite = XBOXCS;
		GetComponentInParent<SRUIManager>().BattleWintrad = "比赛胜利";
		GetComponentInParent<SRUIManager>().Moneytrad = "货币";
		GetComponentInParent<SRUIManager>().PlayingTimetrad = "游戏总时数";
		GetComponentInParent<SRUIManager>().UpdateLanguage();
		OpenMenuTouche.text = "press [ESC] or [START] to open menu";
		IfYouPlayController.text = "IF YOU PLAY WITH A CONTROLLER : CHANGE KEYBOARD TO XBOX IN SETTINGS";
		GetComponent<SRTutoManager>().InfoCarsDealer = "HERE YOU CAN FIND THE CARS AND SKINS SHOP";
		GetComponent<SRTutoManager>().InfoGarage = "Here you can custom your car: \n suspension and camber of the wheels (for drift), power, braking, etc.";
		GetComponent<SRTutoManager>().Tips = "TIP : USE YOUR HANDBRAKE [SPACE] or [A] FOR PERFECT DRIFT";
		GetComponent<SRTutoManager>().InfoTofuNeedTranslate = "豆腐 ? : Fill your wallet with tofu deliveries";
		if ((bool)ChooseYourLanguage)
		{
			ChooseYourLanguage.text = "选择你的语言";
		}
		if ((bool)WelcomeIn)
		{
			WelcomeIn.text = "欢迎来到 Initial Drift Online";
		}
	}

	public void SetPolonais()
	{
	}

	public void SetTurkish()
	{
	}

	public void SetRomanian()
	{
	}

	public void SetBrazilianportuguese()
	{
	}

	public void SetCzech()
	{
	}

	public void SetHungarian()
	{
	}
}
