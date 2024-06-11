using UnityEngine;
using UnityEngine.Playables;

//자막의 각 클립이다.
//만들어지면 SubtitleBehaviour에 만들어진 값을 넣어준다.
public class SubtitleClip : PlayableAsset
{
	public string SubtitleText;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);

		SubtitleBehaviour subtitleBehaviour = playable.GetBehaviour();
		subtitleBehaviour.subtitleText = SubtitleText;

		return playable;
	}
}
