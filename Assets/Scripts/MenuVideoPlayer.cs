using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MenuVideoPlayer : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "MainMenuVideo.mp4");
        videoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
