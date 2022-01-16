/* BGM 관리
 * 최초 한 번 실행되는 BGM 이후 두 번째 BGM은 루프 형식으로 반복 실행
 */

using System.Collections;
using UnityEngine;

public class BgmController : MonoBehaviour
{
    public AudioClip[] bgm;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(PlayBGM());
    }

    IEnumerator PlayBGM()
    {
        audioSource.clip = bgm[0];
        audioSource.Play();

        //첫 BGM의 길이 이후에 실행, 즉 첫 번째 BGM 종료 이후 실행
        yield return new WaitForSeconds(bgm[0].length);

        audioSource.clip = bgm[1];
        audioSource.loop = true;
        audioSource.Play();
    }
}
