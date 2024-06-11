using TMPro;
using UnityEngine;
using UnityEngine.Playables;

//자막 시스템에 동작방식을 정의한다.
//오브젝트로 받은 TMP자리에 넣어진 text 값을 넣어준다
//서서히 변하게 할 경우 투명도가 서서히 변하게 해주기도 했다.
//하지만 이는 믹서에서 작업하며 변수만 저장하게 되었다.
public class SubtitleBehaviour : PlayableBehaviour
{
	public string subtitleText;
}
