using UnityEngine;
using UnityEngine.UI;

public class LangManager2 : MonoBehaviour
{
	public Text Graphics;

	public Text ImagesEffects;

	public Text Music;

	public Text Siren;

	public Text Mute;

	public Text Save;

	public Text Low;

	public Text Medium;

	public Text High;

	public Text Ultra;

	public Text Bloom;

	public Text AmbientOcclusion;

	public Text MotionBlur;

	public Text Options;

	public Text Score;

	public Text TimeLeft;

	public Text Combo;

	public Text WrongWay;

	public Text BombHealth;

	public Text Speed;

	public Text Distance;

	public Text HighSpeed;

	public Text Resume;

	public Text Restart;

	public Text MainMenu;

	public Text Back;

	public Text TotalDistance;

	public Text NearMiss;

	public Text KeepAbove;

	public Text OppositeDirection;

	public Text MainMenuu;

	public Text PlayAgain;

	public Text YourScore;

	public Text RoadReflections;

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
			SetChinois();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 3)
		{
			SetChinoistradi();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 4)
		{
			SetFrancais();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 5)
		{
			SetPolonais();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 6)
		{
			SetTurkish();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 7)
		{
			SetRomanian();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 8)
		{
			SetBrazilianportuguese();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 9)
		{
			SetCzech();
		}
		if (PlayerPrefs.GetInt("LANGUAGE") == 10)
		{
			SetHungarian();
		}
	}

	public void SetEnglish()
	{
		Graphics.text = "GRAPHICS";
		ImagesEffects.text = "IMAGES EFFECTS";
		Music.text = "MUSIC";
		Siren.text = "SOUND";
		Mute.text = "MUTE";
		Save.text = "SAVE";
		Low.text = "LOW";
		Medium.text = "MEDIUM";
		High.text = "HIGH";
		Ultra.text = "ULTRA";
		Bloom.text = "BLOOM";
		AmbientOcclusion.text = "AMBIENT OCCLUSION";
		MotionBlur.text = "MOTION BLUR";
		RoadReflections.text = "ROAD REFLECTIONS";
		Options.text = "OPTIONS";
		Score.text = "SCORE";
		TimeLeft.text = "TIME LEFT";
		Combo.text = "COMBO";
		WrongWay.text = "WRONG WAY";
		BombHealth.text = "BOMB HEALTH";
		Speed.text = "SPEED";
		Distance.text = "DISTANCE";
		HighSpeed.text = "HIGH SPEED";
		Resume.text = "RESUME";
		Restart.text = "RESTART";
		MainMenu.text = "MAIN MENU";
		Back.text = "BACK";
		TotalDistance.text = "TOTAL DISTANCE";
		NearMiss.text = "NEAR MISS";
		KeepAbove.text = "KEEP ABOVE 100KM/H";
		OppositeDirection.text = "OPPOSITE DIRECTION";
		MainMenuu.text = "MAIN MENU";
		PlayAgain.text = "PLAY AGAIN";
		YourScore.text = "YOUR SCORE";
	}

	public void SetRusse()
	{
		Graphics.text = "ГРАФИКА";
		ImagesEffects.text = "ГРАФИЧЕСКИЕ ЭФФЕКТЫ";
		Music.text = "МУЗЫКА";
		Siren.text = "ЗВУК";
		Mute.text = "БЕЗ ЗВУКА";
		Save.text = "СОХРАНИТЬ";
		Low.text = "НИЗКАЯ";
		Medium.text = "СРЕДНЯЯ";
		High.text = "ВЫСОКАЯ";
		Ultra.text = "УЛЬТРА";
		Bloom.text = "БЛУМ";
		AmbientOcclusion.text = "Окружающие факторы";
		MotionBlur.text = "БЛЮР";
		RoadReflections.text = "ОТРАЖЕНИЯ ДОРОГИ";
		Options.text = "НАСТРОЙКИ";
		Score.text = "СЧЕТ";
		TimeLeft.text = "ВРЕМЕНИ ОСТАЛОСЬ";
		Combo.text = "КОМБО";
		WrongWay.text = "ВСТРЕЧКА";
		BombHealth.text = "ЗДОРОВЬЕ БОМБЫ";
		Speed.text = "СКОРОСТЬ";
		Distance.text = "РАССТОЯНИЕ";
		HighSpeed.text = "ВЫСОКАЯ СКОРОСТЬ";
		Resume.text = "ПРОДОЛЖИТЬ";
		Restart.text = "РЕСТАРТ";
		MainMenu.text = "ГЛАВНОЕ МЕНЮ";
		Back.text = "НАЗАД";
		TotalDistance.text = "ПРОЙДЕННОЕ РАССТОЯНИЕ";
		NearMiss.text = "НА ВОЛОСКЕ";
		KeepAbove.text = "СКОРОСТЬ ВЫШЕ 100 КМ/Ч";
		OppositeDirection.text = "ЕЗДА ПО ВСТРЕЧНОЙ";
		MainMenuu.text = "ГЛАВНОЕ МЕНЮ";
		PlayAgain.text = "СЫГРАТЬ СНОВА";
		YourScore.text = "ВАШ СЧЕТ";
	}

	public void SetChinois()
	{
		Graphics.text = "画质";
		ImagesEffects.text = "画面设置";
		Music.text = "音乐";
		Siren.text = "声";
		Mute.text = "静音";
		Save.text = "设定";
		Low.text = "低";
		Medium.text = "中";
		High.text = "高";
		Ultra.text = "超高";
		Bloom.text = "泛光";
		AmbientOcclusion.text = "环境光遮蔽";
		MotionBlur.text = "动态模糊";
		RoadReflections.text = "地面反射质量";
		Options.text = "游戏设定";
		Score.text = "比分";
		TimeLeft.text = "时间所剩";
		Combo.text = "连续技";
		WrongWay.text = "逆行";
		BombHealth.text = "炸弹生命值";
		Speed.text = "速度";
		Distance.text = "里程";
		HighSpeed.text = "高速";
		Resume.text = "继续";
		Restart.text = "重新开始";
		MainMenu.text = "游戏主页";
		Back.text = "返回";
		TotalDistance.text = "总里程";
		NearMiss.text = "有惊无险";
		KeepAbove.text = "维持100KM/H以上";
		OppositeDirection.text = "反方向";
		MainMenuu.text = "游戏主页";
		PlayAgain.text = "重新再玩";
		YourScore.text = "玩家总分数";
	}

	public void SetFrancais()
	{
		Graphics.text = "GRAPHISMES";
		ImagesEffects.text = "EFFETS VISUEL";
		Music.text = "MUSIQUE";
		Siren.text = "SON";
		Mute.text = "MUTER";
		Save.text = "SAUVEGARDER";
		Low.text = "BAS";
		Medium.text = "MOYEN";
		High.text = "HAUT";
		Ultra.text = "ULTRA";
		Bloom.text = "BLOOM";
		AmbientOcclusion.text = "OCCLUSION AMBIENTE";
		MotionBlur.text = "FLOU DE MOUVEMENT";
		RoadReflections.text = "REFLECTIONS";
		Options.text = "PARAMETRES";
		Score.text = "SCORE";
		TimeLeft.text = "TEMPS RESTANT";
		Combo.text = "COMBO";
		WrongWay.text = "CONTRESENS";
		BombHealth.text = "VIE BOMBE";
		Speed.text = "VITESSE";
		Distance.text = "DISTANCE";
		HighSpeed.text = "HAUTE VITESSE";
		Resume.text = "REPRENDRE";
		Restart.text = "RECOMMENCER";
		MainMenu.text = "MENU PRINCIPALE";
		Back.text = "RETOUR";
		TotalDistance.text = "DISTANCE TOTALE";
		NearMiss.text = "DE JUSTESSE";
		KeepAbove.text = "AU DESSUT DE 100KM/H";
		OppositeDirection.text = "CONTRESENS";
		MainMenuu.text = "MENU PRINCIPALE";
		PlayAgain.text = "RECOMMENCER";
		YourScore.text = "TON SCORE";
	}

	public void SetChinoistradi()
	{
		Graphics.text = "畫質";
		ImagesEffects.text = "畫面設置";
		Music.text = "音樂";
		Siren.text = "聲";
		Mute.text = "靜音";
		Save.text = "設定";
		Low.text = "低";
		Medium.text = "中";
		High.text = "高";
		Ultra.text = "超高";
		Bloom.text = "泛光";
		AmbientOcclusion.text = "環境光遮蔽";
		MotionBlur.text = "動態模糊";
		RoadReflections.text = "地面反射質量";
		Options.text = "遊戲設定";
		Score.text = "比分";
		TimeLeft.text = "時間所剩";
		Combo.text = "連續技";
		WrongWay.text = "逆行";
		BombHealth.text = "炸彈生命值";
		Speed.text = "速度";
		Distance.text = "里程";
		HighSpeed.text = "高速";
		Resume.text = "繼續";
		Restart.text = "重新開始";
		MainMenu.text = "遊戲主頁";
		Back.text = "返回";
		TotalDistance.text = "總里程";
		NearMiss.text = "有驚無險";
		KeepAbove.text = "維持100KM/H以上";
		OppositeDirection.text = "反方向";
		MainMenuu.text = "遊戲主頁";
		PlayAgain.text = "重新再玩";
		YourScore.text = "玩家總分數";
	}

	public void SetPolonais()
	{
		Graphics.text = "GRAFIKA";
		ImagesEffects.text = "EFEKTY WIZUALNE";
		Music.text = "MUZYKA";
		Siren.text = "DŹWIĘK";
		Mute.text = "WYCISZ";
		Save.text = "ZAPISZ";
		Low.text = "NISKA";
		Medium.text = "ŚREDNIA";
		High.text = "WYSOKA";
		Ultra.text = "FANTASTYCZNA";
		Bloom.text = "POŚWIATA";
		AmbientOcclusion.text = "CIENIOWANIE";
		MotionBlur.text = "ROZMYCIE RUCHU";
		RoadReflections.text = "ODBICIA ŚWIATEŁ";
		Options.text = "USTAWIENIA";
		Score.text = "WYNIK";
		TimeLeft.text = "POZOSTAŁY CZAS";
		Combo.text = "COMBO";
		WrongWay.text = "ZŁY KIERUNEK";
		BombHealth.text = "ZDROWIE BOMBY";
		Speed.text = "PRĘDKOŚĆ";
		Distance.text = "DYSTANS";
		HighSpeed.text = "WYSOKA PRĘDKOŚĆ";
		Resume.text = "WZNÓW";
		Restart.text = "RESTART";
		MainMenu.text = "MENU GŁÓWNE";
		Back.text = "POWRÓT";
		TotalDistance.text = "CAŁKOWITY DYSTANS";
		NearMiss.text = "BLISKO WYPADKU";
		KeepAbove.text = "UTRZYMANO POWYŻEJ 100KM/H";
		OppositeDirection.text = "JAZDA POD PRĄD";
		MainMenuu.text = "MENU GŁÓWNE";
		PlayAgain.text = "ZAGRAJ JESZCZE RAZ";
		YourScore.text = "TWÓJ WYNIK";
	}

	public void SetTurkish()
	{
		Graphics.text = "GRAFİKLER";
		ImagesEffects.text = "GÖRSEL EFEKTLER";
		Music.text = "MÜZİK";
		Siren.text = "SES";
		Mute.text = "SUSTUR";
		Save.text = "KAYDET";
		Low.text = "DÜŞÜK";
		Medium.text = "ORTA";
		High.text = "YÜKSEK";
		Ultra.text = "ULTRA";
		Bloom.text = "PARLAMA";
		AmbientOcclusion.text = "ORTAM AYDINLATMASI";
		MotionBlur.text = "HAREKET BULANIKLIĞI";
		RoadReflections.text = "YOL YANSIMALARI";
		Options.text = "SEÇENEKLER";
		Score.text = "SKOR";
		TimeLeft.text = "KALAN ZAMAN";
		Combo.text = "KOMBO";
		WrongWay.text = "TERS YÖN";
		BombHealth.text = "BOMBA SAĞLIĞI";
		Speed.text = "HIZ";
		Distance.text = "MESAFE";
		HighSpeed.text = "YÜKSEK HIZ";
		Resume.text = "DEVAM ET";
		Restart.text = "YENİDEN BAŞLAT";
		MainMenu.text = "ANA MENÜ";
		Back.text = "GERİ";
		TotalDistance.text = "TOPLAM MESAFE";
		NearMiss.text = "RAMAK KALA";
		KeepAbove.text = "100KM/S ÜSTÜ SEYAHAT";
		OppositeDirection.text = "TERS YÖN";
		MainMenuu.text = "ANA MENÜ";
		PlayAgain.text = "TEKRAR OYNA";
		YourScore.text = "SKORUN";
	}

	public void SetRomanian()
	{
		Graphics.text = "NIVEL DE GRAFICĂ";
		ImagesEffects.text = "EFECTE ALE IMAGINII";
		Music.text = "MUZICĂ";
		Siren.text = "SUNET";
		Mute.text = "FĂRĂ SUNET";
		Save.text = "SALVEAZĂ";
		Low.text = "SCĂZUT";
		Medium.text = "MEDIUM";
		High.text = "RIDICAT";
		Ultra.text = "FOARTE RIDICAT";
		Bloom.text = "STRĂLUCIRE";
		AmbientOcclusion.text = "LUMINĂ AMBIENTALĂ";
		MotionBlur.text = "ÎNCEȚOȘAREA IMAGINII";
		RoadReflections.text = "REFLECȚII PE ȘOSEA";
		Options.text = "OPȚIUNI";
		Score.text = "SCOR";
		TimeLeft.text = "TIMP RĂMAS";
		Combo.text = "COMBO";
		WrongWay.text = "CONTRASENS";
		BombHealth.text = "VIAȚA BOMBEI";
		Speed.text = "VITEZĂ";
		Distance.text = "DISTANȚĂ";
		HighSpeed.text = "VITEZĂ MAXIMĂ";
		Resume.text = "REVENIRE";
		Restart.text = "REPORNIRE";
		MainMenu.text = "MENIU PRINCIPAL";
		Back.text = "ÎNAPOI";
		TotalDistance.text = "DISTANȚĂ TOTALĂ";
		NearMiss.text = "EVITARE ÎN ULTIMA CLIPĂ";
		KeepAbove.text = "MENȚINERE VITEZĂ PESTE 100KM/H";
		OppositeDirection.text = "CONTRASENS";
		MainMenuu.text = "MENIU PRINCIPAL";
		PlayAgain.text = "JOC NOU";
		YourScore.text = "SCORUL OBȚINUT";
	}

	public void SetBrazilianportuguese()
	{
		Graphics.text = "GRÁFICOS";
		ImagesEffects.text = "EFEITOS DE IMAGEM";
		Music.text = "MÚSICA";
		Siren.text = "SOM";
		Mute.text = "MUDO";
		Save.text = "SALVAR";
		Low.text = "BAIXO";
		Medium.text = "MÉDIO";
		High.text = "ALTO";
		Ultra.text = "ULTRA";
		Bloom.text = "BLOOM";
		AmbientOcclusion.text = "OCLUSÃO DO AMBIENTE";
		MotionBlur.text = "DESFOQUE DE MOVIMENTO";
		RoadReflections.text = "REFLEXOS NA PISTA";
		Options.text = "OPÇÕES";
		Score.text = "PONTUAÇÃO";
		TimeLeft.text = "TEMPO RESTANTE";
		Combo.text = "COMBO";
		WrongWay.text = "CONTRAMÃO";
		BombHealth.text = "INTEGRIDADE DA BOMBA";
		Speed.text = "VELOCIDADE";
		Distance.text = "DISTÂNCIA";
		HighSpeed.text = "ALTA VELOCIDADE";
		Resume.text = "CONTINUAR";
		Restart.text = "REINICIAR";
		MainMenu.text = "MENU PRINCIPAL";
		Back.text = "VOLTAR";
		TotalDistance.text = "DISTÂNCIA TOTAL";
		NearMiss.text = "POR UM TRIZ";
		KeepAbove.text = "ACIMA DOS 100KM/H";
		OppositeDirection.text = "CONTRAMÃO";
		MainMenuu.text = "MENU PRINCIPAL";
		PlayAgain.text = "JOGAR NOVAMENTE";
		YourScore.text = "SUA PONTUAÇÃO";
	}

	public void SetCzech()
	{
		Graphics.text = "GRAFIKA";
		ImagesEffects.text = "EFEKTY";
		Music.text = "HUDBA";
		Siren.text = "ZVUK";
		Mute.text = "ZTLUMIT";
		Save.text = "ULOŽIT";
		Low.text = "NÍZKÁ";
		Medium.text = "STŘEDNÍ";
		High.text = "VYSOKÁ";
		Ultra.text = "ULTRA";
		Bloom.text = "BLOOM";
		AmbientOcclusion.text = "ZASTÍNĚNÍ OKOLÍ";
		MotionBlur.text = "ROZOSTŘENÍ POHYBU";
		RoadReflections.text = "SILNIČNÍ ODRAZY";
		Options.text = "NASTAVENÍ";
		Score.text = "SCORE";
		TimeLeft.text = "ZBÝVAJÍCÍ ČAS";
		Combo.text = "KOMBO";
		WrongWay.text = "ŠPATNÝ SMĚR";
		BombHealth.text = "ŽIVOT BOMBY";
		Speed.text = "RYCHLOST";
		Distance.text = "VZDÁLENOST";
		HighSpeed.text = "VYSOKÁ RYCHLOST";
		Resume.text = "POKRAČOVAT";
		Restart.text = "RESTART";
		MainMenu.text = "HLAVNÍ MENU";
		Back.text = "ZPĚT";
		TotalDistance.text = "CELKOVÁ VZDÁLENOST";
		NearMiss.text = "TĚSNÉ MINUTÍ";
		KeepAbove.text = "DRŽET NAD 100KM/H";
		OppositeDirection.text = "OPAČNÝ SMĚR";
		MainMenuu.text = "HLAVNÍ MENU";
		PlayAgain.text = "HRÁT ZNOVU";
		YourScore.text = "TVÉ SCORE";
	}

	public void SetHungarian()
	{
		Graphics.text = "GRAFIKA";
		ImagesEffects.text = "VIZUÁLIS EFFEKTEK";
		Music.text = "ZENE";
		Siren.text = "HANG";
		Mute.text = "NÉMÍTÁS";
		Save.text = "MENTÉS";
		Low.text = "ALACSONY";
		Medium.text = "KÖZEPES";
		High.text = "MAGAS";
		Ultra.text = "ULTRA";
		Bloom.text = "BLOOM";
		AmbientOcclusion.text = "KÖRNYEZETI VISSZAVERŐDÉSEK";
		MotionBlur.text = "MOTION BLUR";
		RoadReflections.text = "ÚTTÜKRÖZŐDÉSEK";
		Options.text = "BEÁLLÍTÁSOK";
		Score.text = "PONTSZÁM";
		TimeLeft.text = "HÁTRALÉVŐ IDŐ";
		Combo.text = "KOMBÓ";
		WrongWay.text = "ROSSZ IRÁNY";
		BombHealth.text = "BOMBA ÉLET";
		Speed.text = "SEBESSÉG";
		Distance.text = "TÁVOLSÁG";
		HighSpeed.text = "SZÁGULDÁS";
		Resume.text = "FOLYTATÁS";
		Restart.text = "ÚJRAINDÍTÁS";
		MainMenu.text = "FŐMENÜ";
		Back.text = "VISSZA";
		TotalDistance.text = "TELJES TÁVOLSÁG";
		NearMiss.text = "AZ MÁR KARCOLT / MAJDNEM PUSZI";
		KeepAbove.text = "TARTSD 100 KM/Ó FÖLÖTT";
		OppositeDirection.text = "ELLENTÉTES IRÁNY";
		MainMenuu.text = "FŐMENÜ";
		PlayAgain.text = "MÉG EGY KÖRT";
		YourScore.text = "A TE PONTSZÁMOD";
	}
}
