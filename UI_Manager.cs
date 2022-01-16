/* Life, Score와 같은 UI를 관리
 * MonsterBehaviourController로부터 SetScore을 실행받아 점수를 출력한다.
 * MonsterBehaviourController로부터 LoseLife를 실행받아 라이프를 변경한다.
 * SkillHeal로부터 GetLife를 실행받아 라이프를 변경한다.
 */

using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Text scoreText;
    public Image[] lifeSlot;
    public Sprite[] goalMonster;
    public Text gameoverScore;
    public GameObject gameoverUI;

    int score = 0;
    int[] lifeStack = { 5, 5, 5, 5 };

    private void Start()
    {
        SetScore(0);

        foreach (var item in lifeSlot)
        {
            item.sprite = goalMonster[5];
        }
    }

    public void SetScore(int scoreAmout)
    {
        score += scoreAmout;
        scoreText.text = score.ToString();
    }

    public void LoseLife(int monsterType)
    {
        for (int i = 0; i < lifeStack.Length; i++)
        {
            if (lifeStack[i] == 5)
            {
                lifeStack[i] = monsterType;
                lifeSlot[i].sprite = goalMonster[monsterType];

                return;
            }
        }
        gameoverUI.SetActive(true);
        gameoverScore.text = score.ToString();
    }

    public void GetLife()
    {
        for (int i = lifeStack.Length - 1; i >= 0; i--)
        {
            if(lifeStack[i] != 5)
            {
                lifeStack[i] = 5;
                lifeSlot[i].sprite = goalMonster[5];

                return;
            }
        }
    }
}
