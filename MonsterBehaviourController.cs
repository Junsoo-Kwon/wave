/* State 정의
 * FSM 디자인을 지향하여 몬스터 오브젝트들의 상태를 정의하고 처리한다.
 * 본 클래스의 내부 처리 혹은 플레이어의 입력에 따라 몬스터들의 상태가 변경된다.
 * 각 상태에 따라 대응된 애니메이션을 실행한다.
 */
 
using UnityEngine;

public class MonsterBehaviourController : MonoBehaviour
{
    public UI_Manager ui_Manager;
    Animator animator;

    public float goalPosition;

    readonly protected float killZonePosition = 3.5f;
    protected float monsterYPosition;

    [HideInInspector] public float monsterMoveSpeed;
    [HideInInspector] public int monsterScore;
    [HideInInspector] public int monsterType;
    public int monsterHP;

    public enum MonsterStateEnum { walk, removed, goal, damaged }
    public MonsterStateEnum monsterState = MonsterStateEnum.walk;

    private void Start()
    {
        monsterYPosition = transform.position.y;
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        switch (monsterState)
        {
            case MonsterStateEnum.walk:
                StateWalk();
                break;

            case MonsterStateEnum.removed:
                StateRemoved();
                break;

            case MonsterStateEnum.goal:
                StateGoal();
                break;

            case MonsterStateEnum.damaged:
                StateDamaged();
                break;

            default:
                break;
        }
    }

    protected void StateWalk()
    {
        //상단에서 하단으로 이동하므로 y 좌표 값을 이동속도만큼 계속 빼준다.
        monsterYPosition = gameObject.transform.position.y - monsterMoveSpeed;

        animator.Play("Run");
        if (monsterYPosition <= goalPosition)
        {
            monsterState = MonsterStateEnum.goal;
            return;
        }

        gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                    monsterYPosition,
                                                    gameObject.transform.position.z);

        //x 좌표 값이 설정된 제거 위치로 이동하였을 경우 hp를 0으로 초기화한다.
        if (gameObject.transform.position.x < -killZonePosition || gameObject.transform.position.x > killZonePosition)
        {
            monsterHP = 0;
        }

        if (monsterHP <= 0)
        {
            monsterState = MonsterStateEnum.removed;
        }
    }

    //제거되었을 경우, UI에 출력할 점수 값을 전달하며 실행한다.
    protected void StateRemoved()
    {
        ui_Manager.SetScore(monsterScore);
        gameObject.SetActive(false);
    }

    //목표 지점에 도달하였을 경우, UI에 도달한 몬스터 타입 값을 전달하며 실행한다.
    protected void StateGoal()
    {
        ui_Manager.LoseLife(monsterType);
        gameObject.SetActive(false);
    }

    protected void StateDamaged()
    {
        animator.Play("Swipe");
    }
}
