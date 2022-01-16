/* SkillSlow 실행 및 재사용대기시간 관리
 * 해당 스킬 버튼 누를 경우 Slow 스킬이 발동하며 UI에 변화
 * 재사용대기시간 중일 경우 반투명 이미지와 함께 재사용대기시간이 출력되며 입력이 되지 않음
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlow : MonoBehaviour
{
    public MonsterManager monsterManager;
    public Image skillSlowButton;

    readonly private float slowAmount = 0.5f;
    readonly private float slowTime = 3f;
    readonly private float slowCool = 10f;
    
    public GameObject slowEffect;
    public GameObject slowCoolFade;
    public Text slowCoolText;

    public void StartSlow()
    {
        skillSlowButton.raycastTarget = false;
        monsterManager.ScaleMoveSpeed(slowAmount);

        slowCoolFade.SetActive(true);
        slowEffect.SetActive(true);
        StartCoroutine(RemoveSlow());
    }

    IEnumerator RemoveSlow()
    {
        //실행에 사용할 값 복사
        float _slowCool = slowCool;

        while (true)
        {
            slowCoolText.text = _slowCool.ToString();
            //재사용대기시간 1초마다 감소
            yield return new WaitForSeconds(1.0f);
            _slowCool--;
            
            //초기값 - 현재값 = 슬로우가 지속된 시간
            if(slowTime == slowCool - _slowCool)
            {
                //슬로우 지속 시간 값이 끝났을 경우 원래 속도로 재설정
                monsterManager.ScaleMoveSpeed(1/slowAmount);
            }

            //슬로우가 끝났으면 이펙트 제거
            else if(slowTime < slowCool - _slowCool)
            {
                slowEffect.SetActive(false);
            }

            //스킬이 사용 가능해졌을 때
            if(_slowCool <= 0)
            {
                break;
            }

        }

        slowCoolFade.SetActive(false);
        skillSlowButton.raycastTarget = true;
    }
}
