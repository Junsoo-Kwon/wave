/* SkillHeal 실행 및 재사용대기시간 관리
 * 해당 스킬 버튼을 누를 경우 Heal 스킬이 발동하며 UI에 변화
 * 재사용대기시간 중일 경우 반투명 이미지와 함께 재사용대기시간이 출력되며 입력이 되지 않음
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillHeal : MonoBehaviour
{
    public UI_Manager ui_Manager;
    public Image skillHealButton;
    readonly private float healCool = 10f;

    public GameObject healCoolFade;
    public Text healCoolText;

    public void StartHeal()
    {
        skillHealButton.raycastTarget = false;
        ui_Manager.GetLife();

        healCoolFade.SetActive(true);
        StartCoroutine(SkillCool());
    }

    //사용가능 할 때까지 남은 시간과 함께 반투명 이미지 출력
    IEnumerator SkillCool()
    {
        //실행에 사용할 값 복사
        float _healCool = healCool;

        while (true)
        {
            healCoolText.text = _healCool.ToString();
            //재사용대기시간 1초마다 감소
            yield return new WaitForSeconds(1.0f);
            _healCool--;

            //스킬이 사용 가능해졌을 때
            if (_healCool <= 0)
            {
                break;
            }

        }

        healCoolFade.SetActive(false);
        skillHealButton.raycastTarget = true;
    }
}
