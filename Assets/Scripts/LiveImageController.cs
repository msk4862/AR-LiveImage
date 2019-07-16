using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class LiveImageController : MonoBehaviour {

	public AugmentedLiveImage augmentedLiveImage;

	private Dictionary<int, AugmentedLiveImage> visualizers = new Dictionary<int, AugmentedLiveImage>();

	private List<AugmentedImage> images = new List<AugmentedImage>();

	void Start ()
	{
		Debug.Log("Starting");
	}

	void Update ()
	{
		if (Session.Status != SessionStatus.Tracking) {
			return;
		}

		Session.GetTrackables(images, TrackableQueryFilter.Updated);
		VisualizeTrackables();


	}


	private void VisualizeTrackables ()
	{
		foreach (var image in images) {
			Debug.Log("trackable found");
			var visualizer = GetVisualizer (image);

			if (image.TrackingState == TrackingState.Tracking && visualizer == null) {
				AddVisualizer (image);
			} else if(image.TrackingState == TrackingState.Paused && visualizer != null){
				RemoveVisualizer(image, visualizer);
			}
		}
	}


	private void AddVisualizer (AugmentedImage image)
	{
		Debug.Log("Adding visualizer");
		var anchor = image.CreateAnchor(image.CenterPose);
		var visualizor = Instantiate(augmentedLiveImage, anchor.transform);
		visualizor.Image = image;

		visualizers.Add(image.DatabaseIndex, visualizor);
	}


	private void RemoveVisualizer (AugmentedImage image, AugmentedLiveImage visualizer)
	{
		Debug.Log("Removing visualizer");
		visualizers.Remove(image.DatabaseIndex);
		Destroy(visualizer.gameObject);
	}

		
	private AugmentedLiveImage GetVisualizer (AugmentedImage image)
	{
		AugmentedLiveImage visualizer;
		visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
		return visualizer;
	}

}
