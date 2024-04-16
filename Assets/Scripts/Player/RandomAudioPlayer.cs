using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    public AudioClip[] audioClips; // List of audio clips to choose from
    public AudioSource audioSource; // AudioSource component to play the audio clip

    void Start()
    {
        // Check if there are any audio clips in the list
        if (audioClips != null && audioClips.Length > 0)
        {
            // Get a random index within the range of the audioClips array
            int randomIndex = Random.Range(0, audioClips.Length);

            // Play the audio clip at the randomly chosen index
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();
        }
        else
        {
            // Log a warning if no audio clips are assigned
            Debug.LogWarning("No audio clips assigned to play random audio.");
        }
    }
}
