/* SkillWipe 실행 및 재사용대기시간 관리
 * 해당 스킬 버튼을 누를 경우 Wipe 스킬이 발동하며 Fade 이펙트 출력
 * 재사용대기시간 중일 경우 반투명 이미지와 함께 재사용대기시간이 출력되며 입력이 되지 않음
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillWipe : MonoBehaviour
{
    public MonsterManager monsterManager;
    public Image skillWipeButton;
    readonly private float wipeCool = 30f;

    public GameObject[] wipeEffect;
    public GameObject wipeCoolFade;
    public Text wipeCoolText;

    readonly float effectTimer = 0.2f;

    public void StartWipe()
    {
        skillWipeButton.raycastTarget = false;
        wipeCoolFade.SetActive(true);
        monsterManager.SetSkillWipe();

        StartCoroutine(SkillCool());
        StartCoroutine(SkillEffect());
    }

    //사용가능 할 때까지 남은 시간과 함께 반투명 이미지 출력
    IEnumerator SkillCool()
    {
        //실행에 사용할 값 복사
        float _wipeCool = wipeCool;

        while (true)
        {
            wipeCoolText.text = _wipeCool.ToString();
            //재사용대기시간 1초마다 감소
            yield return new WaitForSeconds(1.0f);
            _wipeCool--;

            //스킬이 사용 가능해졌을 때
            if (_wipeCool <= 0)
            {
                break;
            }
        }

        wipeCoolFade.SetActive(false);
        skillWipeButton.raycastTarget = true;
    }

    //사전에 기획된 3개의 이펙트 이미지 순차대로 출력
    IEnumerator SkillEffect()
    {
        wipeEffect[0].SetActive(true);
        yield return new WaitForSeconds(effectTimer);

        wipeEffect[0].SetActive(false);
        wipeEffect[1].SetActive(true);
        yield return new WaitForSeconds(effectTimer);
        
        wipeEffect[1].SetActive(false);
        wipeEffect[2].SetActive(true);
        yield return new WaitForSeconds(effectTimer);
        
        wipeEffect[2].SetActive(false);
    }
}
