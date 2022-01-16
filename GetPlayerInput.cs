/* 플레이어 입력의 처리
 * 플레이어로부터 터치, 드래그 입력을 받아 대응되는 결과를 출력한다.
 */

using UnityEngine;

public class GetPlayerInput : MonoBehaviour
{
    RaycastHit raycastHit;
    MonsterBehaviourController monsterBehaviourController;

    AudioSource clickSound;
    GameObject currentObject;

    readonly int clickDamage = 1;
    readonly float rayLength = 10;

    readonly int dragMask = 1 << 8;
    readonly int clickMask = 1 << 9;
    readonly int bothMask = 1 << 10;

    bool isDraged = false;

    private void Start()
    {
        clickSound = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        //플레이어가 최초 입력을 실행하였을 때
        if (Input.GetMouseButtonDown(0))
        {
            //카메라에서 보는 좌표 값과 월드 상에서 좌표 값이 다르기에 차이를 보정.
            Vector3 currentInputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Drag만 해당되는 몬스터일 경우
            if (Physics.Raycast(currentInputPosition, Vector3.forward, out raycastHit, rayLength, dragMask))
            {
                clickSound.Play();

                currentObject = raycastHit.collider.gameObject;
                monsterBehaviourController = currentObject.GetComponent<MonsterBehaviourController>();
                monsterBehaviourController.monsterState = MonsterBehaviourController.MonsterStateEnum.damaged;

                isDraged = true;
            }

            //Click만 해당되는 몬스터일 경우
            if (Physics.Raycast(currentInputPosition, Vector3.forward, out raycastHit, rayLength, clickMask))
            {
                clickSound.Play();

                monsterBehaviourController = raycastHit.collider.gameObject.GetComponent<MonsterBehaviourController>();
                monsterBehaviourController.monsterHP = monsterBehaviourController.monsterHP - clickDamage;
            }

            //Drag와 Click 두 경우 모두 해당되는 몬스터일 경우
            else if (Physics.Raycast(currentInputPosition, Vector3.forward, out raycastHit, rayLength, bothMask))
            {
                clickSound.Play();

                currentObject = raycastHit.collider.gameObject;
                monsterBehaviourController = currentObject.GetComponent<MonsterBehaviourController>();
                monsterBehaviourController.monsterState = MonsterBehaviourController.MonsterStateEnum.damaged;

                monsterBehaviourController.monsterHP = monsterBehaviourController.monsterHP - clickDamage;

                isDraged = true;
            }
        }

        //플레이어의 입력이 지속되고 있을 때
        else if (Input.GetMouseButton(0))
        {
            Vector3 currentInputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Raycast 되는지 확인을 위한 Debug용 Ray 사용
            Debug.DrawRay(currentInputPosition, Vector3.forward * rayLength, Color.red);

            if (isDraged)
            {
                //입력받은 몬스터의 위치를 입력하고 있는 위치로 계속 이동
                currentObject.transform.position = new Vector3(currentInputPosition.x,
                                                               currentInputPosition.y,
                                                               currentObject.transform.position.z);
            }
        }

        //플레이어의 입력이 끝날 때
        if (Input.GetMouseButtonUp(0))
        {
            if (!isDraged)
            {
                return;
            }
            monsterBehaviourController.monsterState = MonsterBehaviourController.MonsterStateEnum.walk;

            isDraged = false;
        }
    }
}
