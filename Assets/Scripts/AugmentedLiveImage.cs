using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;
using GoogleARCore;

public class AugmentedLiveImage : MonoBehaviour {

	[SerializeField]
	private VideoClip[] videoClips;
	private VideoPlayer videoPlayer;
	public AugmentedImage Image; 

	// Use this for initialization
	void Start () {
		videoPlayer = GetComponent<VideoPlayer>();
		videoPlayer.loopPointReached += OnStop;
	}
	
	private void OnStop(VideoPlayer source) {
		gameObject.SetActive(false);
	}

	void Update ()
	{
		if (Image == null || Image.TrackingState != TrackingState.Tracking) {
			return;
		}

		if (!videoPlayer.isPlaying) {
			videoPlayer.clip = videoClips[Image.DatabaseIndex];
			videoPlayer.Play();
		}
		transform.localScale = new Vector3(Image.ExtentX, Image.ExtentZ, 1);
	}
}
