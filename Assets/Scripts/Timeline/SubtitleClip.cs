using UnityEngine;
using UnityEngine.Playables;

//�ڸ��� �� Ŭ���̴�.
//��������� SubtitleBehaviour�� ������� ���� �־��ش�.
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
