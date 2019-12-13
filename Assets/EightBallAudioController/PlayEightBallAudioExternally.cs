using UnityEngine;
using System.Collections;
//Be sure to use Hear360 namespace
using Hear360;

public class PlayEightBallAudioExternally : MonoBehaviour {

	public EightBallAudioController eightBallAudioController;
	// Use this for initialization
	void Start () {
		//Demostrate how to trigger the eight ball audio controller start to play externally and how to enable loop externally
		//You could try enabling this script and disabling "AutoPlay" and "Loop" properity on EightBallAudioExternally game object
		eightBallAudioController.loop = true;
		eightBallAudioController.Play ();
	}
}
