using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SR3DLB : MonoBehaviour
{
	public Text[] LivraisonCountIro;

	public TextMeshPro score;

	public TextMeshPro[] nameA;

	public GameObject EnableLB;

	public string LB1_OU_LB2;

	private int i;

	private int a;

	public float ssize;

	private void Start()
	{
		a = 0;
		i = 0;
	}

	private void Update()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("besttimeiro");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("bestscoreLivraisonIro");
		GameObject[] array3;
		if (array.Length >= 0)
		{
			array3 = array;
			foreach (GameObject gameObject in array3)
			{
				if (gameObject.GetComponentInParent<Button>().gameObject.tag == LB1_OU_LB2 && this.i < 5)
				{
					nameA[this.i].text = gameObject.GetComponent<Text>().text.ToString();
					this.i++;
				}
			}
		}
		if (array2.Length == 0)
		{
			return;
		}
		array3 = array2;
		foreach (GameObject gameObject2 in array3)
		{
			if (gameObject2.GetComponentInParent<Button>().gameObject.tag == LB1_OU_LB2 && a < 5)
			{
				score.text = score.text + gameObject2.GetComponent<Text>().text.ToString() + "<size=" + ssize + "> 's </size>\n";
				a++;
			}
		}
	}

	public void EnableLBB(bool jack)
	{
		EnableLB.SetActive(jack);
		if (jack)
		{
			GetComponent<LeaderboardUsersManager>().RefreshOnlyVar();
		}
	}
}
