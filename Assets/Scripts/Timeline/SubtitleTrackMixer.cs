using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

//트랙 믹서는 겹치거나 할 때 겹친 내용을 다 입력 받는 방식으로 서서히 사라지거나 다 끝나면 텍스를 지우게 해준다.
//입력받은 애들을 모두 돌면서 그 값의 weight가 0보다 높으면 화면에 떠야하는 값이니 추가해준다.
//현재로서는 텍스트이니 그냥 뒤에오는 값을 받아서 넣어준다.
public class SubtitleTrackMixer : PlayableBehaviour
{
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		TextMeshProUGUI text = playerData as TextMeshProUGUI;
		string currentText = "";
		float currentAlpha = 0f;

		if(!text) { return; }

		int inputCount = playable.GetInputCount();
		for(int i = 0; i < inputCount; i++)
		{
			float inputWeight = playable.GetInputWeight(i);

			if(inputWeight > 0f )
			{
				ScriptPlayable<SubtitleBehaviour> inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);

				SubtitleBehaviour input = inputPlayable.GetBehaviour();
				currentText = input.subtitleText;
				currentAlpha = inputWeight;
			}
		}

		text.text = currentText;
		text.color = new Color(1, 1, 1, currentAlpha);
	}
}
