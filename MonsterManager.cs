/* 몬스터 관리
 * 오브젝트 풀링 시스템을 활용하여 몬스터들을 관리
 * SkillSlow 로부터 입력받아 ScaleMoveSpeed를 호출하여 활성화된 오브젝트들의 이동 속도 수정
 * SkillWipe 로부터 입력받아 SetSkillWipe를 호출하여 활성화된 오브젝트들의 HP 수정
 */

using System.Collections;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    readonly int monsterKinds = 5;
    readonly int objectPoolSize = 10;
    private float monsterZPosition = 0;

    readonly float startPosition = 6;

    public LevelDesign levelDesign;
    public GameObject[] monsterType;
    private GameObject[,] objectPool;
    public GameObject[] popupGuide;

    private MonsterBehaviourController monsterBehaviourController;
    private int spawnCount = 0;
    private float currentTime = 0;
    private float speedSclae = 1;
    private float levelScale = 1;

    readonly private float gameScaleTime = 0.1f;
    readonly private float[] popupGuideTime = { 0, 3, 17, 20, 42, 45, 72, 75, 102, 105 };

    private void Start()
    {
        objectPool = new GameObject[monsterKinds, objectPoolSize];
        MonsterInstantiater();
        StartCoroutine(SpawnController());
    }

    private void MonsterInstantiater()
    {
        //기획된 값에 따라 초기 설정된만큼 생성
        for (int i = 0; i < monsterKinds; i++)
        {
            for (int j = 0; j < objectPoolSize; j++)
            {
                objectPool[i, j] = Instantiate(monsterType[i]);
                objectPool[i, j].SetActive(false);
            }
        }
    }

    private void SpawnMonster(int type)
    {
        //기획 오류로 초기 생성 값을 넘어갈 경우 에러 메시지 출력
        try
        {
            for (int i = 0; i < objectPoolSize; i++)
            {
                //비활성화된 오브젝트, 즉 사용 가능한 오브젝트만 사용
                if (!objectPool[type, i].activeSelf)
                {
                    monsterBehaviourController = objectPool[type, i].GetComponent<MonsterBehaviourController>();

                    objectPool[type, i].transform.position = new Vector3(levelDesign.dataArray[spawnCount].Lane,
                                                                         startPosition,
                                                                         monsterZPosition);
                    //겹쳐서 보이지 않도록 값 정의
                    monsterZPosition = monsterZPosition + 0.01f;

                    //생성된 오브젝트들의 데이터 값 전달
                    monsterBehaviourController.monsterMoveSpeed = levelDesign.dataArray[spawnCount].Movespeed * speedSclae;
                    monsterBehaviourController.monsterHP = levelDesign.dataArray[spawnCount].HP;
                    monsterBehaviourController.monsterScore = levelDesign.dataArray[spawnCount].Score;
                    monsterBehaviourController.monsterType = levelDesign.dataArray[spawnCount].Type;
                    monsterBehaviourController.monsterState = MonsterBehaviourController.MonsterStateEnum.walk;

                    objectPool[type, i].SetActive(true);
                    return;
                }
            }
        }

        catch (System.Exception)
        {
            Debug.LogError("Not Enough SpawnPool. Call Programmer");
        }
    }

    //몬스터들을 생성하기 위해 시간 기반 값을 사용
    IEnumerator SpawnController()
    {
        while (true)
        {
            //기획된 모든 값을 사용하였을 경우 레벨을 하나 올리고 생성 값 초기화
            if (levelDesign.dataArray.Length <= spawnCount)
            {
                spawnCount = 0;
                currentTime = 0;
                monsterZPosition = 0;
                levelScale++;
            }

            //첫 번째 스테이지 일 경우 팝업 가이드 출력
            if (levelScale == 1)
            {
                if (currentTime >= popupGuideTime[0] && currentTime <= popupGuideTime[1])
                {
                    popupGuide[0].SetActive(true);
                }

                else if (currentTime >= popupGuideTime[2] && currentTime <= popupGuideTime[3])
                {
                    popupGuide[1].SetActive(true);
                }

                else if (currentTime >= popupGuideTime[4] && currentTime <= popupGuideTime[5])
                {
                    popupGuide[2].SetActive(true);
                }

                else if (currentTime >= popupGuideTime[6] && currentTime <= popupGuideTime[7])
                {
                    popupGuide[3].SetActive(true);
                }

                else if (currentTime >= popupGuideTime[8] && currentTime <= popupGuideTime[9])
                {
                    popupGuide[4].SetActive(true);
                }

                else
                {
                    foreach (var item in popupGuide)
                    {
                        item.SetActive(false);
                    }
                }
            }

            //몬스터 생성 및 생성한 총 수치 증가
            if (currentTime >= levelDesign.dataArray[spawnCount].Spwantime)
            {
                SpawnMonster(levelDesign.dataArray[spawnCount].Type);
                spawnCount++;
            }

            //설정한 값만큼 타이머 값증가
            currentTime += gameScaleTime;
            yield return new WaitForSeconds(gameScaleTime);
        }
    }

    //SkillSlow로부터 호출받았을 경우 실행. 인수에 따라 현재 활성화된 몬스터들의 이동속도 관리
    public void ScaleMoveSpeed(float scaleSize)
    {
        speedSclae *= scaleSize;

        foreach (var item in objectPool)
        {
            if (item.activeSelf)
            {
                MonsterBehaviourController _monsterBehaviourController = item.GetComponent<MonsterBehaviourController>();
                _monsterBehaviourController.monsterMoveSpeed = _monsterBehaviourController.monsterMoveSpeed * scaleSize;
            }
        }
    }

    //SkillWipe 로부터 호출받았을 경우 실행. 현재 활성화된 모든 몬스터들의 체력을 0으로 초기화
    public void SetSkillWipe()
    {
        foreach (var item in objectPool)
        {
            if (item.activeSelf)
            {
                MonsterBehaviourController _monsterBehaviourController = item.GetComponent<MonsterBehaviourController>();
                _monsterBehaviourController.monsterHP = 0;
            }
        }
    }
}
