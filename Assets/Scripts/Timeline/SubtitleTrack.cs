using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//자막 트랙으로 트랙 믹서를 만들어준다
//수정하는 타입이랑 만드는 클립 타입을 설정해준다.
[TrackBindingType(typeof(TextMeshProUGUI))]
[TrackClipType(typeof(SubtitleClip))]
public class SubtitleTrack : TrackAsset
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<SubtitleTrackMixer>.Create(graph, inputCount);
	}
}
